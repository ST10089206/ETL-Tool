using CsvHelper;
using CsvHelper.Configuration;
using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Writers
{
    public class CSVWriter : ICSVWriter
    {
        private readonly string _filePath;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializes a new instance of the CSVWriter class with the specified parameters
        /// </summary>
        /// <param name="filePath">This string parameter holds a filepath used to navigate the destination of the file that will be used</param>
        /// <param name="mapping">This parameter of Mapping holds a specified mapping used to map reader and writer sources and destinations</param>
        public CSVWriter(string filePath, Mapping mapping)
        {
            _filePath = filePath;
            _mapping = mapping;
        }

        /// <summary>
        /// This methods writes the specified data to the specified destination of a CSV file
        /// </summary>
        /// <param name="destination">The paramter holds the name of the file destination</param>
        /// <param name="data">The paramter holds the data to be written to the file</param>
        public void Write(string destination, IEnumerable<DataRecords> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var writer = new StreamWriter(destination))
            using (var csv = new CsvWriter(writer, config))
            {
                foreach (var columnMapping in _mapping.ColumnMappings)
                {
                    csv.WriteField(columnMapping.Value);
                }
                csv.NextRecord();

                foreach (var record in data)
                {
                    foreach (var columnMapping in _mapping.ColumnMappings)
                    {
                        if (record.Fields.ContainsKey(columnMapping.Key))
                        {
                            csv.WriteField(record.Fields[columnMapping.Key]);
                        }
                        else
                        {
                            csv.WriteField(string.Empty);
                        }
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}
