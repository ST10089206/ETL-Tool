using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Interfaces
{
    /// <summary>
    /// This interface resembles the Interface Segregation Principle (ISP) which ensures that the Writer classes will use the method for write data to different destinations
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// This void method takes in a string and the IEnumarable model of the data it will write
        /// </summary>
        /// <param name="destination">This parameter sets the destination of where to write the data</param>
        /// <param name="data">This parameter set the data that will be written</param>
        void Write (string destination, IEnumerable<DataRecords> data);
    }
}
