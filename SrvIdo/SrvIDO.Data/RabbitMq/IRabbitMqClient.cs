using SrvIDO.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.DATA.RabbitMq
{
    public interface IRabbitMqClient
    {
        void EnviaOcorrencia(IDOUpdate ocorrencia);
        void ConsomeOcorrencia();
    }
}
