using Microsoft.Extensions.Configuration;
using SrvIDO.DOMAIN;
using SrvIDO.INFRAESTRUCTURE.DbInitialize;
using System.Data;
using SrvIDO.DATA.Interfaces;
using SrvIDO.DATA.Entities;
using System.Data.SqlClient;
using System.Drawing;

namespace SrvIDO.DATA.Repository
{
    public class QueriesRepository : IQueriesRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly string _dbOcorrencia;
        private readonly string _dbResultadoNCD;
        private readonly string _dbIDO;


        public QueriesRepository(IDbConnectionFactory connectionFactory, IConfiguration Configuration)
        {
            _connectionFactory = connectionFactory;
            _dbOcorrencia = Configuration.GetValue<string>("Ocorrencia");
            _dbResultadoNCD = Configuration.GetValue<string>("ResultadoNCD");
            _dbIDO = Configuration.GetValue<string>("IDO");
        }

        public async Task<List<OcorrenciaModel>> GetAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var retorno = new List<OcorrenciaModel>();
            var query = @"******";

            try
            {
                // Create the command.
                var command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                // Open the connection.
                //connection.Open();

                // Retrieve the data.
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var ret = new OcorrenciaModel();
                    ret.ID = (long)reader["ID"];
                    ret.dataOcorrencia = (DateTime)reader["dataOcorrencia"];
                    ret.numeroOcorrencia = reader["numeroOcorrencia"] != DBNull.Value ? (int)reader["numeroOcorrencia"] : null;
                    ret.cadCod = reader["cadCod"] != DBNull.Value ? (short)reader["cadCod"] : (short)0;
                    ret.codigoServico = reader["codigoServico"] != DBNull.Value ? (short)reader["codigoServico"] : (short)0;
                    ret.dataHoraEncerramento = reader["dataHoraEncerramento"] != DBNull.Value ? (DateTime)reader["dataHoraEncerramento"] : null;
                    ret.naturezaCodigo = reader["naturezaCodigo"] != DBNull.Value ? (string)reader["naturezaCodigo"] : "";
                    ret.naturezaComplemento = reader["naturezaComplemento"] != DBNull.Value ? (short)reader["naturezaComplemento"] : (short)0;
                    ret.naturezaDetalhe = reader["naturezaDetalhe"] != DBNull.Value ? (short)reader["naturezaDetalhe"] : (short)0;
                    ret.localCodigo = reader["localCodigo"] != DBNull.Value ? (string)reader["localCodigo"] : null;
                    ret.localComplemento = reader["localComplemento"] != DBNull.Value ? (short)reader["localComplemento"] : (short)0;
                    ret.localDetalhe = reader["localDetalhe"] != DBNull.Value ? (short)reader["localDetalhe"] : (short)0;
                    ret.textoHistoricoFinal = reader["textoHistoricoFinal"] != DBNull.Value ? (string)reader["textoHistoricoFinal"] : null;
                    ret.codigoResultado = reader["codigoResultado"] != DBNull.Value ? (short)reader["codigoResultado"] : (short)0;
                    ret.indicadorHorarioVerao = reader["indicadorHorarioVerao"] != DBNull.Value ? (int)reader["indicadorHorarioVerao"] : 0;
                    retorno.Add(ret);
                }
                return retorno;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<bool> UpdateAsync(IDOUpdate listaUpdate)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var query = @"****";

            IDbTransaction transaction;
            SqlCommand command = (SqlCommand)connection.CreateCommand();
            transaction = connection.BeginTransaction();
            // Create the command.
            command.Connection = (SqlConnection)connection;
            command.Transaction = (SqlTransaction)transaction;
            try
            {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdoIdc", listaUpdate.IdoIdc);
                    command.Parameters.AddWithValue("@IdoDatEnvio", listaUpdate.IdoDatEnvio);
                    command.Parameters.AddWithValue("@OcrID", listaUpdate.OcrID);
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                
                transaction.Commit();
            }

            catch (Exception e)
            {
                transaction.Rollback();
                connection.Close();
                throw;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
    }
}
