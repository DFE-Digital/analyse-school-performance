using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? TableName { get; set; }
    }
}
