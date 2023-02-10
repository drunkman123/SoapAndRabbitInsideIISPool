using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.DATA.Entities
{
    public class IDORequest
    {
        public long ID { get; set; }
        public DateTime dataOcorrencia { get; set; }
        public int? numeroOcorrencia { get; set; }
        public short? cadCod { get; set; }
        public short? codigoServico { get; set; }
        public DateTime? dataHoraEncerramento { get; set; }
        public string? naturezaCodigo { get; set; }
        public short? naturezaComplemento { get; set; }
        public short? naturezaDetalhe { get; set; }
        public string? localCodigo { get; set; }
        public short? localComplemento { get; set; }
        public short? localDetalhe { get; set; }
        public string? textoHistoricoFinal { get; set; }
        public short? codigoResultado { get; set; }
        public int? indicadorHorarioVerao { get; set; }
    }
}
