using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Models
{
    public class Mapping
    {
        /// <summary>
        /// Gets and Sets for columm mappings of various sources and destinations
        /// </summary>
        public Dictionary<string, string> ColumnMappings { get; set; }

        /// <summary>
        /// Initializing a new instance if the Mappings class
        /// </summary>
        public Mapping()
        {
            ColumnMappings = new Dictionary<string, string>();
        }
    }
}
