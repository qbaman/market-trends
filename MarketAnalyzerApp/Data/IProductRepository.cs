namespace MarketAnalyzerApp.Data;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
}
