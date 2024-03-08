using CatalogAPI.Models;
using CatalogAPI.Pagination;
using X.PagedList;

namespace CatalogAPI.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IPagedList<Product>> GetProdutosAsync(ProductsParameters produtosParams);
    Task<IPagedList<Product>> GetProdutosFiltroPrecoAsync(ProductsPriceFilter produtosFiltroParams);
    Task<IEnumerable<Product>> GetProdutosPorCategoriaAsync(int id);
}
