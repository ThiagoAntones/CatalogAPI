using CatalogAPI.Context;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using X.PagedList;

namespace CatalogAPI.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Product>> GetProdutosAsync(ProductsParameters produtosParams)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenados = produtos.OrderBy(p => p.ProductId).AsQueryable();

        var resultado = await produtosOrdenados.ToPagedListAsync(produtosParams.PageNumber,
                                                           produtosParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Product>> GetProdutosFiltroPrecoAsync(ProductsPriceFilter produtosFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (produtosFiltroParams.Price.HasValue && !
            string.IsNullOrEmpty(produtosFiltroParams.CriteryPrice))
        {
            if (produtosFiltroParams.CriteryPrice.Equals("maior",
                               StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Price > produtosFiltroParams.Price.Value)
                                                                    .OrderBy(p => p.Price);
            }
            else if (produtosFiltroParams.CriteryPrice.Equals("menor",
                                    StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Price < produtosFiltroParams.Price.Value)
                                                                   .OrderBy(p => p.Price);
            }
            else if (produtosFiltroParams.CriteryPrice.Equals("igual",
                                     StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Price == produtosFiltroParams.Price.Value)
                                                                     .OrderBy(p => p.Price);
            }
        }

        var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber,
                                                          produtosFiltroParams.PageSize);
        return produtosFiltrados;
    }

    public async Task<IEnumerable<Product>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();
        var produtosCategoria = produtos.Where(c => c.CategoryId == id);
        return produtosCategoria;
    }
}
