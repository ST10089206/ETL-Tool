using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Readers
{
    public class ExcelReader : IExcelReader
    {
        private readonly string _filePath;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializing a new instance if the ExcelReader class
        /// </summary>
        /// <param name="filePath">This parameter holds the filePath that will be used in Read method</param>
        /// <param name="mapping">This parameter holds the mapping that will be used in Read method</param>
        public ExcelReader(string filePath, Mapping mapping)
        {
            _filePath = filePath;
            _mapping = mapping;
        }

        /// <summary>
        /// This implemenatation of the IENumerable interface Read method reads data from a filepath using EPPlus, which is used to access each row using the dictionary
        /// </summary>
        /// <param name="source">This parameter is a string of the fielpath for selecting the source of the data you will read</param>
        /// <returns></returns>
        public IEnumerable<DataRecords> Read(string source)
        {
            var records = new List<DataRecords>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(source)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                    throw new InvalidDataException("The Excel file contains no worksheets.");

                var worksheet = package.Workbook.Worksheets[0];

                if (worksheet.Dimension == null)
                    throw new InvalidDataException("The worksheet is empty.");

                var header = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    header.Add(worksheet.Cells[1, col].Text);

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var record = new DataRecords();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        var columnName = worksheet.Cells[1, col].Text;
                        if (_mapping.ColumnMappings.ContainsKey(columnName))
                        {
                            var mappedColumnName = _mapping.ColumnMappings[columnName];
                            record.Fields.Add(mappedColumnName, worksheet.Cells[row, col].Text);
                        }
                    }
                    records.Add(record);
                }
            }
            return records;
        }
    }
}
