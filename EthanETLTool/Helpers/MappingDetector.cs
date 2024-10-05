using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Helpers
{
    public class MappingDetector
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializing a new instance if the MappingDetector class
        /// </summary>
        /// <param name="connectionString">This parameter holds the connection string to the data in order for the mappings to find the tables in the database that will be used.</param>
        public MappingDetector(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Detects the mapping between SQL source columns and SQL destination columns where the column names match.
        /// </summary>
        /// <param name="source">The connection string or identifier for the source SQL database.</param>
        /// <param name="destination">The connection string or identifier for the destination SQL database.</param>
        /// <returns>A Mapping object containing the column mappings where the source and destination columns have the same names.</returns>
        public Mapping DetectSQLSQLMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var sqlSourceColumns = GetSqlSourceColumns(source);
            var sqlDestinationColumns = GetSqlDestinationColumns(destination);

            foreach (var sourceColumn in sqlSourceColumns)
            {
                if (sqlDestinationColumns.Contains(sourceColumn))
                {
                    mapping.ColumnMappings.Add(sourceColumn, sourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectExcelSQLMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var excelSourceColumns = GetExcelSourceColumns(source);
            var sqlDestinationColumns = GetSqlDestinationColumns(destination);

            foreach (var excelSourceColumn in excelSourceColumns)
            {
                if (sqlDestinationColumns.Contains(excelSourceColumn))
                {
                    mapping.ColumnMappings.Add(excelSourceColumn, excelSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectCsvSQLMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var csvSourceColumns = GetCsvSourceColumns(source);
            var sqlDestinationColumns = GetSqlDestinationColumns(destination);

            foreach (var csvSourceColumn in csvSourceColumns)
            {
                if (sqlDestinationColumns.Contains(csvSourceColumn))
                {
                    mapping.ColumnMappings.Add(csvSourceColumn, csvSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectSQLExcelMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var sqlSourceColumns = GetSqlSourceColumns(source);
            var excelDestinationColumns = GetExcelDestinationColumns(destination);

            foreach (var sqlSourceColumn in sqlSourceColumns)
            {
                if (excelDestinationColumns.Contains(sqlSourceColumn))
                {
                    mapping.ColumnMappings.Add(sqlSourceColumn, sqlSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectExcelExcelMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var excelSourceColumns = GetExcelSourceColumns(source);
            var excelDestinationColumns = GetExcelDestinationColumns(destination);

            foreach (var excelSourceColumn in excelSourceColumns)
            {
                if (excelDestinationColumns.Contains(excelSourceColumn))
                {
                    mapping.ColumnMappings.Add(excelSourceColumn, excelSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectCsvExcelMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var csvSourceColumns = GetCsvSourceColumns(source);
            var excelDestinationColumns = GetExcelDestinationColumns(destination);

            foreach (var csvSourceColumn in csvSourceColumns)
            {
                if (excelDestinationColumns.Contains(csvSourceColumn))
                {
                    mapping.ColumnMappings.Add(csvSourceColumn, csvSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectCsvCsvMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var csvSourceColumns = GetCsvSourceColumns(source);
            var csvDestinationColumns = GetCsvDestinationColumns(destination);

            foreach (var csvSourceColumn in csvSourceColumns)
            {
                if (csvDestinationColumns.Contains(csvSourceColumn))
                {
                    mapping.ColumnMappings.Add(csvSourceColumn, csvSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectSQLCsvMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var sqlSourceColumns = GetSqlSourceColumns(source);
            var csvDestinationColumns = GetCsvDestinationColumns(destination);

            foreach (var sqlSourceColumn in sqlSourceColumns)
            {
                if (csvDestinationColumns.Contains(sqlSourceColumn))
                {
                    mapping.ColumnMappings.Add(sqlSourceColumn, sqlSourceColumn);
                }
            }

            return mapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Mapping DetectExcelCsvMapping(string source, string destination)
        {
            var mapping = new Mapping();
            var excelSourceColumns = GetExcelSourceColumns(source);
            var csvDestinationColumns = GetCsvDestinationColumns(destination);

            foreach (var excelSourceColumn in excelSourceColumns)
            {
                if (csvDestinationColumns.Contains(excelSourceColumn))
                {
                    mapping.ColumnMappings.Add(excelSourceColumn, excelSourceColumn);
                }
            }

            return mapping;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<string> GetExcelSourceColumns(string source)
        {
            var columns = new List<string>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(source)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                    throw new InvalidDataException("The Excel file contains no worksheets.");

                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet.Dimension == null)
                    throw new InvalidDataException("The worksheet is empty.");

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    columns.Add(worksheet.Cells[1, col].Text);
            }
            return columns;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<string> GetSqlSourceColumns(string source)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                var query = $"SET FMTONLY ON; {source}; SET FMTONLY OFF;";
                var reader = db.ExecuteReader(query);
                var schemaTable = reader.GetSchemaTable();
                var columns = schemaTable.Rows.Cast<DataRow>().Select(row => row["ColumnName"].ToString()).ToList();
                reader.Close();
                return columns;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<string> GetCsvSourceColumns(string source)
        {
            var columns = new List<string>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(source))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                columns = csv.HeaderRecord.ToList();
            }

            return columns;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<string> GetSqlDestinationColumns(string destination)
        {
            var columns = new List<string>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                var query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{destination}'";
                columns = db.Query<string>(query).ToList();
            }

            return columns;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<string> GetExcelDestinationColumns(string destination)
        {
            var columns = new List<string>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(destination)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                    throw new InvalidDataException("The Excel file contains no worksheets.");

                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet.Dimension == null)
                    throw new InvalidDataException("The worksheet is empty.");

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    columns.Add(worksheet.Cells[1, col].Text);
            }
            return columns;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        private List<string> GetCsvDestinationColumns(string destination)
        {
            var columns = new List<string>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(destination))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                columns = csv.HeaderRecord.ToList();
            }

            return columns;
        }
    }
}
