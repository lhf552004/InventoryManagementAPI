public interface IProductService
{
    List<Product> GetAllProducts();
}

public class ProductService : IProductService
{
    private readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Product A", Price = 100.0m },
        new Product { Id = 2, Name = "Product B", Price = 200.0m },
        new Product { Id = 3, Name = "Product C", Price = 300.0m },
        new Product { Id = 4, Name = "Product D", Price = 400.0m },
        new Product { Id = 5, Name = "Product E", Price = 500.0m },
        new Product { Id = 6, Name = "Product F", Price = 600.0m },
        new Product { Id = 7, Name = "Product G", Price = 700.0m },
        new Product { Id = 8, Name = "Product H", Price = 800.0m },
    };

    public List<Product> GetAllProducts()
    {
        // Here you would typically fetch from a database.
        return _products;
    }
}
