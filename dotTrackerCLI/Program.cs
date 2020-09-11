using System;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

// TODO parse csv
// TODO build db tables
// TODO write csv data to tables
namespace dotTrackerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvAsJson = ReadCsvFile("/Users/adam/RiderProjects/dotTracker/dotTrackerCLI/Daily Report (31235194) 20200908.csv");
            
            Console.WriteLine(csvAsJson);
        }

        private static string ReadCsvFile(string csvFilePath)
        {
            DataTable csvData = new DataTable();
            string jsonString;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csvFilePath))
                {
                    csvReader.SetDelimiters(new string[] {","});
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn dataColumn = new DataColumn(column);
                            dataColumn.AllowDBNull = true;
                            csvData.Columns.Add(dataColumn);
                        }

                        tableCreated = true;
                    }

                    while (!csvReader.EndOfData)
                    {
                        csvData.Rows.Add(csvReader.ReadFields());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error: Parsing CSV";
            }

            jsonString = JsonConvert.SerializeObject(csvData);

            return jsonString;
        }
    }
}