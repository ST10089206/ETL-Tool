using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Interfaces
{
    /// <summary>
    /// This interface uses the ISP principle and defines a method for transforming data between the reading and writing processes.
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// This IEnumerabele interface method focuses solely on transforming data retrieved
        /// </summary>
        /// <param name="data">This parameter takes in the IEnumerable method that it will transform</param>
        /// <returns></returns>
        IEnumerable<DataRecords> Transform(IEnumerable<DataRecords> data);
    }
}
