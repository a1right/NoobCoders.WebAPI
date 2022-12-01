using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoobCoders.Application.Interfaces;
using NoobCoders.Application.Models;
using NoobCoders.Domain;

namespace NoobCoders.Application.Services
{
    public class CsvReaderService : ICsvReader
    {
        public List<RecordTemplate> GetRecordTemplates(IConfiguration configuration)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = configuration["CSVConfiguration:Delimiter"]
            };
            using (var reader = new StreamReader(configuration["CSVConfiguration:PathToInitialDataFile"]))
            using (var csv = new CsvReader(reader, config)) 
                return csv.GetRecords<RecordTemplate>().ToList();
        }
    }
}
