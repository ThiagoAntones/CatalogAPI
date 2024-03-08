namespace CatalogAPI.Pagination;

public class ProductsPriceFilter : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? CriteryPrice { get; set; } // "maior", "menor" ou "igual"
}
