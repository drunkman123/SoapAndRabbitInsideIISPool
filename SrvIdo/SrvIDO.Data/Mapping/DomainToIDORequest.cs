using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SrvIDO.DATA.Entities;
using SrvIDO.DOMAIN;

namespace SrvIDO.DATA.Mapping
{
    public static class DomainToIDORequest
    {
        public static List<IDORequest> ToIDORequest(this List<OcorrenciaModel> listaocorrencia)
        {
            List<IDORequest> request = new();
            foreach (var ocorrencia in listaocorrencia)
            {
                IDORequest requestItem = new();
                requestItem.ID= ocorrencia.ID;
                requestItem.dataOcorrencia = ocorrencia.dataOcorrencia;
                requestItem.numeroOcorrencia = ocorrencia.numeroOcorrencia;
                requestItem.cadCod = ocorrencia.cadCod;
                requestItem.codigoServico = ocorrencia.codigoServico;
                requestItem.dataHoraEncerramento = ocorrencia.dataHoraEncerramento;
                requestItem.naturezaCodigo = ocorrencia.naturezaCodigo;
                requestItem.naturezaComplemento = ocorrencia.naturezaComplemento;
                requestItem.naturezaDetalhe = ocorrencia.naturezaDetalhe;
                requestItem.localCodigo = ocorrencia.localCodigo;
                requestItem.localComplemento = ocorrencia.localComplemento;
                requestItem.localDetalhe = ocorrencia.localDetalhe;
                requestItem.textoHistoricoFinal = ocorrencia.textoHistoricoFinal;
                requestItem.codigoResultado = ocorrencia.codigoResultado;
                requestItem.indicadorHorarioVerao = ocorrencia.indicadorHorarioVerao;
                request.Add(requestItem);
            }
            return request;
        }
    }
}
