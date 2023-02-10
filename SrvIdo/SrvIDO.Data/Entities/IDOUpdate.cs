using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.DATA.Entities
{
    public class IDOUpdate
    {
        public long OcrID { get; set; }
        public short IdoIdc { get; set; } = 1;
        public DateTime IdoDatEnvio { get; set; }= DateTime.Now;
        
    }
}
