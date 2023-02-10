using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvIDO.INFRAESTRUCTURE.DbInitialize
{
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();

    }
}
