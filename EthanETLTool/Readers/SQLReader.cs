using Dapper;
using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Readers
{
    public class SQLReader : ISQLReader
    {
        private readonly string _connectionString;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializing a new instance if the SQLReader class
        /// </summary>
        /// <param name="connectionString">This parameter holds the connection string that will be used in Read method</param>
        /// <param name="mapping">This parameter holds the mapping that will be used in Read method</param>
        public SQLReader(string connectionString, Mapping mapping)
        {
            _connectionString = connectionString;
            _mapping = mapping;
        }

        /// <summary>
        /// This implemenatation of the IENumerable interface Read method reads data from a SQL database using Dapper, which is used to access each row using the dictionary
        /// </summary>
        /// <param name="source">This parameter is a string of the query for selecting the source of the data you will read</param>
        /// <returns></returns>
        public IEnumerable<DataRecords> Read(string source)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                var data = db.Query(source);

                var records = new List<DataRecords>();

                foreach (var row in data)
                {
                    var record = new DataRecords();
                    foreach (var property in row)
                    {
                        if (_mapping.ColumnMappings.ContainsKey(property.Key))
                        {
                            var mappedColumn = _mapping.ColumnMappings[property.Key];
                            record.Fields.Add(mappedColumn, property.Value);
                        }
                    }
                    records.Add(record);
                }
                return records;
            }
        }
    }
}
