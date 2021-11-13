using SQLServerProxy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SQLServerProxy.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SQLResponseController : ApiController
    {
        SQLResponse[] servers = new SQLResponse[]
        {
            new SQLResponse { Id = 1, Server = "localhost", Instance = "mssqlserver", Database = "master" },
            new SQLResponse { Id = 1, Server = "localhost", Instance = "mssqlserver", Database = "DBACollection" },
            new SQLResponse { Id = 1, Server = "localhost", Instance = "mssqlserver", Database = "msdb" }
        };

        public IEnumerable<SQLResponse> GetAllServers()
        {
            return servers;
        }

        public IHttpActionResult GetServer(int id)
        {
            var server = servers.FirstOrDefault((p) => p.Id == id);
            if (server == null)
            {
                return NotFound();
            }
            return Ok(server);
        }

        [HttpPost]
        public IHttpActionResult RunSQL(SQLResponse sql)
        {
            var server = GetSQLResults(sql);
            if (server == null)
            {
                return NotFound();
            }
            return Ok(server);
        }

        private static SQLResponse GetSQLResults(SQLResponse resp)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Data Source"] = resp.Server + (String.IsNullOrEmpty(resp.Instance) ? "" : @"\" + resp.Instance);
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = resp.Database;

            SqlConnection conn = new SqlConnection(builder.ConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand(resp.Query, conn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();

                resp.Result = ds;

                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }
    }
}
