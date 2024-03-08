using CatalogAPI.DTO;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;

namespace CatalogAPI.Controllers;

[Route("[controller]")]
[ApiController]
//[EnableRateLimiting("fixedwindow")]
// Inibe a exibição da documentação para os endpoints 
//[ApiExplorerSettings(IgnoreApi = true)]
public class CategorysController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategorysController> _logger;

    public CategorysController(IUnitOfWork uof,
        ILogger<CategorysController> logger)
    {

        _logger = logger;
        _uof = uof;
    }

    /// <summary>
    /// Obtem uma lista de objetos Categoria
    /// </summary>
    /// <returns>Uma lista de objetos Categoria</returns>
    [HttpGet]
    [DisableRateLimiting]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categorias = await _uof.CategoryRepository.GetAllAsync();

        if (categorias is null)
            return NotFound("Não existem categorias...");

        var categoriasDto = categorias.ToCategoryDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery]
                               CategorysParameters categoriasParameters)
    {
        var categorias = await _uof.CategoryRepository.GetCategorysAsync(categoriasParameters);

        return ObterCategorias(categorias);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriasFiltradas(
                                   [FromQuery] CategorysNameFilter categoriasFiltro)
    {
        var categoriasFiltradas = await _uof.CategoryRepository
                                     .GetCategorysNameFilterAsync(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoryDTO>> ObterCategorias(IPagedList<Category> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoryDTOList();
        return Ok(categoriasDto);
    }

    /// <summary>
    /// Obtem uma Categoria pelo seu Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Objetos Categoria</returns>
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    //[Route("api/v{version:apiVersion}/ObterCategoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var categoria = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDto = categoria.ToCategoryDTO();

        return Ok(categoriaDto);
    }

    /// <summary>
    /// Inclui uma nova categoria
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    ///
    ///     POST api/categorias
    ///     {
    ///        "categoriaId": 1,
    ///        "nome": "categoria1",
    ///        "imagemUrl": "http://teste.net/1.jpg"
    ///     }
    /// </remarks>
    /// <param name="categoriaDto">objeto Categoria</param>
    /// <returns>O objeto Categoria incluida</returns>
    /// <remarks>Retorna um objeto Categoria incluído</remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategory();

        var categoriaCriada = _uof.CategoryRepository.Create(categoria);
        await _uof.Commit();

        var novaCategoriaDto = categoriaCriada.ToCategoryDTO();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = novaCategoriaDto.CategoryId },
            novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoriaDto)
    {
        if (id != categoriaDto.CategoryId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDto.ToCategory();

        var categoriaAtualizada = _uof.CategoryRepository.Update(categoria);
        await _uof.Commit();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoryDTO();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var categoria = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoryRepository.Delete(categoria);
        await _uof.Commit();

        var categoriaExcluidaDto = categoriaExcluida.ToCategoryDTO();

        return Ok(categoriaExcluidaDto);
    }
}
