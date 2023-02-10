using SrvIDO.DOMAIN;
using SrvIDO.DATA.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.DATA.Interfaces
{
    public interface IQueriesRepository
    {
        Task<List<OcorrenciaModel>> GetAsync();
        Task<bool> UpdateAsync(IDOUpdate listaUpdate);

    }
}
