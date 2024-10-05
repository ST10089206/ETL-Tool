using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Interfaces
{
    /// <summary>
    /// This interface resembles the Interface Segregation Principle (ISP) which ensures that the Reader classes will use the IEnumerator interface method for reading data from different sources
    /// </summary>
    public interface ISQLReader : IReader
    {
        /// <summary>
        /// This IEnumerable interface method will retrieve data read from a sql source
        /// </summary>
        /// <param name="source">This parameter takes in a string that has the sql source of where it will read the data from</param>
        /// <returns></returns>
        //IEnumerable<DataRecords> Read(string source);
    }
}
