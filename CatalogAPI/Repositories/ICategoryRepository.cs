using CatalogAPI.Models;
using CatalogAPI.Pagination;
using X.PagedList;

namespace CatalogAPI.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IPagedList<Category>> GetCategorysAsync(CategorysParameters categoriasParams);
    Task<IPagedList<Category>> GetCategorysNameFilterAsync(CategorysNameFilter categoriasParams);
}
