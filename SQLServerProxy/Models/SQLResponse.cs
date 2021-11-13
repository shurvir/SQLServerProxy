using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerProxy.Models
{
    public class SQLResponse
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Instance { get; set; }
        public string Database { get; set; }
        public string Query { get; set; }
        public DataSet Result { get; set; }
    }
}
