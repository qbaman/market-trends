using CsvHelper;
using System.Globalization;
using System.IO;
namespace MarketAnalyzerApp.Data
{
    public static class CSVDataLoader
    {
        public static List<Product> LoadData()
        {
            using var reader = new StreamReader(AppSettings.DatasetPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<ProductMap>();
            return csv.GetRecords<Product>().Take(AppSettings.DataLimit).ToList();
        }
    }
}
