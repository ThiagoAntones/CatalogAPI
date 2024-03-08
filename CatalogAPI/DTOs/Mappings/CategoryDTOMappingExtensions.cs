using CatalogAPI.DTO;
using CatalogAPI.Models;

namespace CatalogAPI.DTOs.Mappings;

public static class CategoryDTOMappingExtensions
{
    public static CategoryDTO? ToCategoryDTO(this Category category)
    {
        if (category is null)
            return null;

        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        };
    }

    public static Category? ToCategory(this CategoryDTO categoryDto)
    {
        if (categoryDto is null) return null;

        return new Category
        {
            CategoryId = categoryDto.CategoryId,
            Name = categoryDto.Name,
            ImageUrl = categoryDto.ImageUrl
        };
    }

    public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categorys)
    {
        if (categorys is null || !categorys.Any())
        {
            return new List<CategoryDTO>();
        }

        return categorys.Select(category => new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ImageUrl = category.ImageUrl
        }).ToList();
    }
}
