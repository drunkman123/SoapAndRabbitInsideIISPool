using EnviarOcorrenciaIDO;
using SrvIDO.DOMAIN;
using SrvIDO.INFRAESTRUCTURE.Interfaces;
using SrvIDO.INFRAESTRUCTURE.Repository;
using SrvIDO.SERVICES.Interfaces;
using SrvIDO.SERVICES.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.SERVICES.Service
{
    public class IDOService : IIDOService
    {
        private readonly IQueriesRepository _queriesRepository;
        private readonly OcorrenciaClient _soapService;
        public IDOService(IQueriesRepository queriesRepository)
        {
            _queriesRepository = queriesRepository;
            _soapService = new OcorrenciaClient(OcorrenciaClient.EndpointConfiguration.basic);
        }
        public async Task<bool> EnviaIDO()
        {
            List<OcorrenciaModel> listaEnvio = await _queriesRepository.GetAsync();
            var IDORequest = listaEnvio.ToIDORequest();
            int i = 0;
            int[] z = new int[0];
            foreach (var ocorrencia in IDORequest)
            {               
                var request = _soapService.EncerrarOcorrenciaAsync(new DateTime(2021,09,21), 1, 1,
                    3, ocorrencia.dataHoraEncerramento, ocorrencia.naturezaCodigo, ocorrencia.naturezaComplemento,
                    ocorrencia.naturezaDetalhe, ocorrencia.localCodigo, ocorrencia.localComplemento, ocorrencia.localDetalhe, ocorrencia.textoHistoricoFinal,
                    ocorrencia.codigoResultado, ocorrencia.indicadorHorarioVerao);
                //var request = _soapService.EncerrarOcorrenciaAsync(ocorrencia.dataOcorrencia, ocorrencia.numeroOcorrencia, ocorrencia.cadCod,
                //    ocorrencia.codigoServico, ocorrencia.dataHoraEncerramento, ocorrencia.naturezaCodigo, ocorrencia.naturezaComplemento,
                //    ocorrencia.naturezaDetalhe, ocorrencia.localCodigo, ocorrencia.localComplemento, ocorrencia.localDetalhe, ocorrencia.textoHistoricoFinal,
                //    ocorrencia.codigoResultado, ocorrencia.indicadorHorarioVerao);

                var response = request.Result;
                if (response.Mensagem != "Ocorrência já encerrada!") 
                {
                    int length = z.Length;
                    Array.Resize(ref z, length + 1);
                    z[length] = i;
                }
                i++;
            }
            foreach(var indice in z.Reverse())
            {
                IDORequest.RemoveAt(indice);
            }
            return true;
        }
    }
}