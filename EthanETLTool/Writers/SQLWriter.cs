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

namespace EthanETLTool.Writers
{

    public class SQLWriter: ISQLWriter
    {
        private readonly string _connectionString;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializes a new instance of the SQLWriter class with the specified connection string
        /// </summary>
        /// <param name="connectionString">This string parameter holds a connection string use dto connect to the database</param>
        public SQLWriter(string connectionString, Mapping mapping)
        {
            _connectionString = connectionString;
            _mapping = mapping;
        }

        /// <summary>
        /// This methods writes the specified data to the specified destination table in the SQL database
        /// </summary>
        /// <param name="destination">The paramter holds the name of the destination table</param>
        /// <param name="data">The paramter holds the data to be written to the table</param>
        public void Write(string destination, IEnumerable<DataRecords> data)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                foreach (var record in data)
                {
                    var mappedRecord = new Dictionary<string, object>();
                    foreach (var field in record.Fields)
                    {
                        if (_mapping.ColumnMappings.ContainsValue(field.Key))
                        {
                            var sourceColumn = _mapping.ColumnMappings.First(x => x.Value == field.Key).Key;
                            mappedRecord.Add(field.Key, record.Fields[sourceColumn]);
                        }
                    }

                    var columns = string.Join(", ", mappedRecord.Keys);
                    var parameters = string.Join(", ", mappedRecord.Keys.Select(key => "@" + key));
                    var sql = $"INSERT INTO {destination} ({columns}) VALUES ({parameters})";

                    db.Execute(sql, mappedRecord);
                }
            }
        }
    }
}
