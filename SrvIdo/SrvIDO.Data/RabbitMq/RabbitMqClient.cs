using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SrvIDO.DATA.Entities;
using SrvIDO.DATA.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SrvIDO.DATA.RabbitMq
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IQueriesRepository _queriesRepository;

        public RabbitMqClient(IConfiguration configuration, IQueriesRepository queriesRepository)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Convert.ToInt32(_configuration["Port"]),
                UserName = _configuration["UsernameRabbit"],
                Password = _configuration["Password"]
            }
                    .CreateConnection();
            _channel = _connection.CreateModel();
            var arguments = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead-letter-exchange-ido" }
            };
            _channel.QueueDeclare(queue: "Ido",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: arguments);
            _queriesRepository = queriesRepository;
        }

        public void EnviaOcorrencia(IDOUpdate ocorrencia)
        {
            var mensagem = JsonSerializer.Serialize(ocorrencia);
            var body = Encoding.UTF8.GetBytes(mensagem);
            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            _channel.BasicPublish(exchange: string.Empty,
                     routingKey: "Ido",
                     basicProperties: properties,
                     body: body
                );
        }
        public void ConsomeOcorrencia()
        {
            var consumidor = new EventingBasicConsumer(_channel);
            consumidor.Received += (ModuleHandle, ea) =>
            {
                var body = ea.Body;
                var mensagem = Encoding.UTF8.GetString(body.ToArray());
                var IdoObject = JsonSerializer.Deserialize<IDOUpdate>(mensagem);
                var result = _queriesRepository.UpdateAsync(IdoObject);
                if (result.IsFaulted) 
                {
                    _channel.BasicNack(ea.DeliveryTag, false,false);
                }
                else
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }


            };
            _channel.BasicConsume(queue: "Ido",
                               autoAck: false,
                               consumer: consumidor);
        }
    }
}
