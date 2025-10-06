using MarketAnalyzerApp.Data;
namespace MarketAnalyzerApp.Services;

public sealed class BrandSearchService : IProductSearch
{
    public IReadOnlyList<Product> ByBrand(IEnumerable<Product> source, string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return source.ToList();
        return source.Where(p => (p.Brand ?? "")
            .IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }
}
