using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    // In-memory collection of products
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

    [HttpGet]
    public IActionResult GetProducts(int pageNumber = 1, int pageSize = 3, string sortBy = "name")
    {
        // Sorting logic
        var products = _products.AsQueryable();
        switch (sortBy.ToLower())
        {
            case "price":
                products = products.OrderBy(p => p.Price);
                break;
            case "name":
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }

        // Pagination logic
        int totalItems = products.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var pagedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Return metadata and paginated result
        var result = new
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = pagedProducts
        };

        return Ok(result);
    }
}
