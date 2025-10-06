using MarketAnalyzerApp.Data;
namespace MarketAnalyzerApp.Services;

public interface IProductSearch
{
    IReadOnlyList<Product> ByBrand(IEnumerable<Product> source, string term);
}
