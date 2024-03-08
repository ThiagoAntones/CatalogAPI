using CatalogAPI.Context;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using X.PagedList;

namespace CatalogAPI.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Category>> GetCategorysAsync(CategorysParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        // OrderBy síncrono
        var categoriasOrdenadas = categorias.OrderBy(p => p.CategoryId).ToList();

        //var resultado =  PagedList<Categoria>.ToPagedList(categoriasOrdenadas,
        //                         categoriasParams.PageNumber, categoriasParams.PageSize);
        var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParams.PageNumber,
                                                                   categoriasParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Category>> GetCategorysNameFilterAsync(CategorysNameFilter categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Name))
        {
            categorias = categorias.Where(c => c.Name.Contains(categoriasParams.Name));
        }

        var categoriasFiltradas = await categorias.ToPagedListAsync(
                                             categoriasParams.PageNumber,
                                             categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}
