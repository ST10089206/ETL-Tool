using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Models
{
    public class DataRecords
    {
        /// <summary>
        /// Gets and Sets the fields of the various data
        /// </summary>
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// Intializes a new instance of the DataRecords class
        /// </summary>
        public DataRecords()
        {
            Fields = new Dictionary<string, object>();
        }
    }
}
