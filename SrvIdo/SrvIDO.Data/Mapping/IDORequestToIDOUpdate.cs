using SrvIDO.DATA.Entities;
using SrvIDO.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.DATA.Mapping
{
    public static class IDORequestToIDOUpdate
    {
        public static List<IDOUpdate> ToIDOUpdate(this List<IDORequest> listaRequest)
        {
            List<IDOUpdate> update = new();
            foreach (var ocorrencia in listaRequest)
            {
                IDOUpdate updateItem = new();
                updateItem.OcrID = ocorrencia.ID;
                update.Add(updateItem);
            }
            return update;
        }
    }
}
