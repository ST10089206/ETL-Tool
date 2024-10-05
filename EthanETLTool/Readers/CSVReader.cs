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

namespace EthanETLTool.Readers
{
    public class CSVReader : ICSVReader
    {
        private readonly string _filePath;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializing a new instance if the CSVReader class
        /// </summary>
        /// <param name="filePath">This parameter holds the file path that will be used in Read method</param>
        /// <param name="mapping">This parameter holds the mappings that will be used in Read method</param>
        public CSVReader(string filePath, Mapping mapping)
        {
            _filePath = filePath;
            _mapping = mapping;
        }

        /// <summary>
        /// This implemenatation of the IENumerable interface Read method reads data from a filepath using CsvHelper, which is used to access each row using the dictionary
        /// </summary>
        /// <param name="source">This parameter is a string of the fielpath for selecting the source of the data you will read</param>
        /// <returns></returns>
        public IEnumerable<DataRecords> Read (string source)
        {
            var records = new List<DataRecords>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                var header = csv.HeaderRecord;

                while (csv.Read())
                {
                    var record = new DataRecords();
                    foreach (var mapping in _mapping.ColumnMappings)
                    {
                        var value = csv.GetField(mapping.Key);
                        record.Fields.Add(mapping.Value, value);
                    }
                    records.Add(record);
                }
            }

            return records;
        }
    }
}
