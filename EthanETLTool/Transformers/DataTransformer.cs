using EthanETLTool.Interfaces;
using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Transformers
{
    /// <summary>
    /// This class uses the Interface to implements methods to use in various data transformations
    /// </summary>
    public class DataTransformer : IDataTransformer
    {
        /// <summary>
        /// This method transforms the given data and modifies it.
        /// In this exmaple, it now makes strings uppercase, formats DateTime, and keeps other types as they are.
        /// </summary>
        /// <param name="data">This parameter holds the data that will be used in the transformer</param>
        /// <returns>This returns the transformed data</returns>
        public IEnumerable<DataRecords> Transform(IEnumerable<DataRecords> data)
        {
            var transformedData = new List<DataRecords>();

            foreach (var record in data)
            {
                var transformedRecord = new DataRecords();

                foreach (var field in record.Fields)
                {
                    object transformedValue;

                    switch (field.Value)
                    {
                        case string stringValue:
                            transformedValue = stringValue.ToUpper();
                            break;

                        case DateTime dateTimeValue:
                            transformedValue = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
                            break;

                        case int intValue:
                            transformedValue = intValue;
                            break;

                        case double doubleValue:
                            transformedValue = doubleValue;
                            break;

                        default:
                            transformedValue = field.Value;
                            break;
                    }

                    transformedRecord.Fields.Add(field.Key, transformedValue);
                }
                transformedData.Add(transformedRecord);
            }

            return transformedData;
        }
    }
}
