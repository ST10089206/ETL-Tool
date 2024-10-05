using EthanETLTool.Helpers;
using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using EthanETLTool.Readers;
using EthanETLTool.Transformers;
using EthanETLTool.Writers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EthanETLTool
{

    class Program
    {
        static void Main(string[] args)
        {
            //Builds the host with configured services below
            var host = CreateHostBuilder(args).Build();

            //Retrieves the interfaces of Reader, Writer and Datatransfomrer servuces
            var reader = host.Services.GetRequiredService<IReader>();
            var writer = host.Services.GetRequiredService<IWriter>();
            var transformer = host.Services.GetRequiredService<ITransformer>();

            //Reader Sources
            //var sqlSourcePath = "SELECT * FROM SQLReaderTable";
            var excelSourcePath = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\ExcelFileTest.xlsx";
            //var csvSourcePath = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel.csv";

            //Writer Destinations
            //var SqlSqlDestination = "SQLReaderTable";
            //var ExcelSqlDestination = "ExcelReaderAccounts";
            //var CsvSqlDestination = "CSVReaderTable";
            //var ExcelExcelDestination = "Sheet3";
            //var SqlExcelDestination = "Sheet2";
            //var CsvExcelDestination = "Sheet1";
            //var CsvCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";
            //var SqlCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";
            var ExcelCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";

            //Reads data from the specified table using the service retrieved
            //var sqlData = reader.Read(sqlSourcePath);
            var excelData = reader.Read(excelSourcePath);
            //var csvData = reader.Read(csvSourcePath);

            //Transforms the data using the service retrieved
            //var transformedData = transformer.Transform(sqlData);
            var transformedData = transformer.Transform(excelData);
            //var transformedData = transformer.Transform(csvData);

            //Writes the transformed data back to the sepcified table using the service retrieved
            //writer.Write(SqlSqlDestination, transformedData);
            //writer.Write(ExcelSqlDestination, transformedData);
            //writer.Write(CsvSqlDestination, transformedData);
            //writer.Write(ExcelExcelDestination, transformedData);
            //writer.Write(SqlExcelDestination, transformedData);
            //writer.Write(CsvExcelDestination, transformedData);
            //writer.Write(CsvCsvDestination, transformedData);
            //writer.Write(SqlCsvDestination, transformedData);
            writer.Write(ExcelCsvDestination, transformedData);
        }

        /// <summary>
        /// This method creates and configures the IHostBuider instance
        /// </summary>
        /// <param name="args">this keeps all command line arguments passed</param>
        /// <returns>This returns the IHostBuilder instance with the configured services</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    //Connection string for the sql server
                    var connectionString = "Data Source=DESKTOP-M7FN247\\SQLEXPRESS01;Initial Catalog=EthanETLTool;Integrated Security=True;TrustServerCertificate=True;";

                    //Reader Sources
                    //var sqlSourcePath = "SELECT * FROM SQLReaderTable";
                    var excelSourcePath = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\ExcelFileTest.xlsx";
                    //var csvSourcePath = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel.csv";

                    //Writer Destinations
                    //var SqlSqlDestination = "SQLReaderTable";
                    //var ExcelSqlDestination = "ExcelReaderAccounts";
                    //var CsvSqlDestination = "CSVReaderTable";
                    //var ExcelExcelDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\ExcelFileTest2.xlsx";
                    //var SqlExcelDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\ExcelFileTest2.xlsx";
                    //var CsvExcelDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\ExcelFileTest2.xlsx";
                    //var CsvCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";
                    //var SqlCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";
                    var ExcelCsvDestination = "C:\\Users\\EthanMakgopa\\Downloads\\ETLTool\\Bike4 Excel2.csv";

                    var mappingDetector = new MappingDetector(connectionString);
                    //var SqlSqlMapping = mappingDetector.DetectSQLSQLMapping(sqlSourcePath, SqlSqlDestination);
                    //var ExcelSqlMapping = mappingDetector.DetectExcelSQLMapping(excelSourcePath, ExcelSqlDestination);
                    //var CsvSqlMapping = mappingDetector.DetectCsvSQLMapping(csvSourcePath, CsvSqlDestination);
                    //var SqlExcelMapping = mappingDetector.DetectSQLExcelMapping(sqlSourcePath, SqlExcelDestination);
                    //var ExcelExcelMapping = mappingDetector.DetectExcelExcelMapping(excelSourcePath, ExcelExcelDestination);
                    //var CsvExcelMapping = mappingDetector.DetectCsvExcelMapping(csvSourcePath, CsvExcelDestination);
                    //var CsvCsvMapping = mappingDetector.DetectCsvCsvMapping(csvSourcePath, CsvCsvDestination);
                    //var SqlCsvMapping = mappingDetector.DetectSQLCsvMapping(sqlSourcePath, SqlCsvDestination);
                    var ExcelCsvMapping = mappingDetector.DetectExcelCsvMapping(excelSourcePath, ExcelCsvDestination);

                    //This registers the MappingDetector helper
                    //services.AddSingleton(SqlSqlMapping);
                    //services.AddSingleton(ExcelSqlMapping);
                    //services.AddSingleton(CsvSqlMapping);
                    //services.AddSingleton(SqlExcelMapping);
                    //services.AddSingleton(ExcelExcelMapping);
                    //services.AddSingleton(CsvExcelMapping);
                    //services.AddSingleton(CsvCsvMapping);
                    //services.AddSingleton(SqlCsvMapping);
                    services.AddSingleton(ExcelCsvMapping);


                    //This registers the IReader services with the Raeders implementation
                    //services.AddTransient<IReader, SQLReader>(provider =>
                    //{
                    //    return new SQLReader(connectionString, provider.GetRequiredService<Mapping>());
                    //});

                    services.AddTransient<IReader, ExcelReader>(provider =>
                    {
                        return new ExcelReader(excelSourcePath, provider.GetRequiredService<Mapping>());
                    });

                    //services.AddTransient<IReader, CSVReader>(provider =>
                    //{
                    //    return new CSVReader(csvSourcePath, provider.GetRequiredService<Mapping>());
                    //});



                    //This registers the IWriter services with the Writers implementation
                    //services.AddTransient<IWriter, SQLWriter>(provider =>
                    //{
                    //    return new SQLWriter(connectionString, provider.GetRequiredService<Mapping>());
                    //});

                    //services.AddTransient<IWriter, ExcelWriter>(provider =>
                    //{
                    //    return new ExcelWriter(SqlExcelDestination, provider.GetRequiredService<Mapping>());
                    //});

                    //services.AddTransient<IWriter, ExcelWriter>(provider =>
                    //{
                    //    return new ExcelWriter(ExcelExcelDestination, provider.GetRequiredService<Mapping>());
                    //});

                    //services.AddTransient<IWriter, ExcelWriter>(provider =>
                    //{
                    //    return new ExcelWriter(CsvExcelDestination, provider.GetRequiredService<Mapping>());
                    //});

                    //services.AddTransient<IWriter, CSVWriter>(provider =>
                    //{
                    //    return new CSVWriter(CsvCsvDestination, provider.GetRequiredService<Mapping>());
                    //});

                    //services.AddTransient<IWriter, CSVWriter>(provider =>
                    //{
                    //    return new CSVWriter(SqlCsvDestination, provider.GetRequiredService<Mapping>());
                    //});

                    services.AddTransient<IWriter, CSVWriter>(provider =>
                    {
                        return new CSVWriter(ExcelCsvDestination, provider.GetRequiredService<Mapping>());
                    });

                    //This registers the IDataTransformer service with the DataTransformer implementation
                    services.AddTransient<ITransformer, DataTransformer>();
                });
    }
}
