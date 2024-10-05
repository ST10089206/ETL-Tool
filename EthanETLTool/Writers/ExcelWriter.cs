using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Writers
{
    public class ExcelWriter : IExcelWriter
    {
        private readonly string _filePath;
        private readonly Mapping _mapping;

        /// <summary>
        /// Initializes a new instance of the ExcelWriter class with the specified parameters
        /// </summary>
        /// <param name="filePath">This string parameter holds a filepath used to navigate the destination of the file that will be used</param>
        /// <param name="mapping">This parameter of Mapping holds a specified mapping used to map reader and writer sources and destinations</param>
        public ExcelWriter(string filePath, Mapping mapping)
        {
            _filePath = filePath;
            _mapping = mapping;
        }

        /// <summary>
        /// This methods writes the specified data to the specified destination of an excel file
        /// </summary>
        /// <param name="destination">The paramter holds the name of the file destination</param>
        /// <param name="data">The paramter holds the data to be written to the file</param>
        public void Write(string destination, IEnumerable<DataRecords> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(destination);

                int colIndex = 1;
                foreach (var columnMapping in _mapping.ColumnMappings)
                {
                    worksheet.Cells[1, colIndex++].Value = columnMapping.Value;
                }

                int rowIndex = 2;
                foreach (var record in data)
                {
                    colIndex = 1;
                    foreach (var columnMapping in _mapping.ColumnMappings)
                    {
                        if (record.Fields.ContainsKey(columnMapping.Key))
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = record.Fields[columnMapping.Key];
                        }
                        colIndex++;
                    }
                    rowIndex++;
                }

                var fileInfo = new FileInfo(_filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}
