using EnviarOcorrenciaIDO;
using SrvIDO.DOMAIN;
using SrvIDO.DATA.Interfaces;
using SrvIDO.DATA.Mapping;
using SrvIDO.DATA.RabbitMq;
using System.Reflection.Metadata.Ecma335;

namespace SrvIDO.DATA.Service
{
    public class IDOService : IIDOService
    {
        private readonly IQueriesRepository _queriesRepository;
        private readonly OcorrenciaClient _soapService;
        private readonly IRabbitMqClient _rabbitMqClient;
        public IDOService(IQueriesRepository queriesRepository, IRabbitMqClient rabbitMqClient)
        {
            _queriesRepository = queriesRepository;
            _soapService = new OcorrenciaClient(OcorrenciaClient.EndpointConfiguration.basic);
            _rabbitMqClient = rabbitMqClient;
        }
        public async Task<bool> EnviaIDO()
        {
            List<OcorrenciaModel> listaEnvio = await _queriesRepository.GetAsync();
            var IDORequest = listaEnvio.ToIDORequest();
            int i = 0;
            int[] z = new int[0];
            foreach (var ocorrencia in IDORequest)
            {
                var request = _soapService.*****Async(Parameters.....);

                var response = request.Result;

                break;
                if (response is null || response.Mensagem != "OK")
                {
                    int length = z.Length;
                    Array.Resize(ref z, length + 1);
                    z[length] = i;
                }
                i++;
                break;
            }
            foreach (var indice in z.Reverse())
            {
                IDORequest.RemoveAt(indice);
            }

            var IDOUpdate = IDORequest.ToIDOUpdate();

            foreach(var ocorrencia in IDOUpdate)
            {
                _rabbitMqClient.EnviaOcorrencia(ocorrencia);
                break;
            }

            //await _queriesRepository.UpdateAsync(IDOUpdate);
            return true;
        }

        public void UpdateIDO()
        {
            _rabbitMqClient.ConsomeOcorrencia();       
        }
    }
}