using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Pagination;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace CatalogAPI.Controllers;

[Route("[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
// Inibe a exibição da documentação para os endpoints 
//[ApiExplorerSettings(IgnoreApi = true)]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("produtos/{id}")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProdutosCategoria(int id)
    {
        var produtos = await _uof.ProductRepository.GetProdutosPorCategoriaAsync(id);

        if (produtos is null)
            return NotFound();

        //var destino = _mapper.Map<Destino>(origem);
        var produtosDto = _mapper.Map<IEnumerable<ProductDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery]
                                   ProductsParameters produtosParameters)
    {
        var produtos = await _uof.ProductRepository.GetProdutosAsync(produtosParameters);

        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProdutosFilterPreco([FromQuery] ProductsPriceFilter
                                                                                    produtosFilterParameters)
    {
        var produtos = await _uof.ProductRepository.GetProdutosFiltroPrecoAsync(produtosFilterParameters);
        return ObterProdutos(produtos);
    }
    private ActionResult<IEnumerable<ProductDTO>> ObterProdutos(IPagedList<Product> produtos)
    {
        var metadata = new
        {
            produtos.Count,
            produtos.PageSize,
            produtos.PageCount,
            produtos.TotalItemCount,
            produtos.HasNextPage,
            produtos.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var produtosDto = _mapper.Map<IEnumerable<ProductDTO>>(produtos);
        return Ok(produtosDto);
    }

    /// <summary>
    /// Exibe uma relação dos produtos
    /// </summary>
    /// <returns>Retorna uma lista de objetos Produto</returns>
    [HttpGet]
    [Authorize(Policy ="UserOnly")]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var produtos = await _uof.ProductRepository.GetAllAsync();
        if (produtos is null)
            return NotFound();

        var produtosDto = _mapper.Map<IEnumerable<ProductDTO>>(produtos);
        return Ok(produtosDto);
    }

    /// <summary>
    /// Obtem o produto pelo seu identificador id
    /// </summary>
    /// <param name="id">Código do produto</param>
    /// <returns>Um objeto Produto</returns>
    [HttpGet("{id}", Name = "ObterProduto")]
    public async Task<ActionResult<ProductDTO>> Get(int id)
    {
        var produto = await _uof.ProductRepository.GetAsync(c => c.ProductId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        var produtoDto = _mapper.Map<ProductDTO>(produto);
        return Ok(produtoDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Post(ProductDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Product>(produtoDto);

        var novoProduto = _uof.ProductRepository.Create(produto);
        await _uof.Commit();

        var novoProdutoDto = _mapper.Map<ProductDTO>(novoProduto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto.ProductId }, novoProdutoDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id,
        JsonPatchDocument<ProductDTOUpdateRequest> patchProdutoDto)
    {
        if (patchProdutoDto == null || id <= 0)
            return BadRequest();

        var produto = await _uof.ProductRepository.GetAsync(c => c.ProductId == id);

        if (produto == null)
            return NotFound();

        var produtoUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(produto);

        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        _uof.ProductRepository.Update(produto);
        await _uof.Commit();

        return Ok(_mapper.Map<ProductDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO produtoDto)
    {
        if (id != produtoDto.ProductId)
            return BadRequest();//400

        var produto = _mapper.Map<Product>(produtoDto);

        var produtoAtualizado = _uof.ProductRepository.Update(produto);
        await _uof.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProductDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var produto = await _uof.ProductRepository.GetAsync(p => p.ProductId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

        var produtoDeletado = _uof.ProductRepository.Delete(produto);
        await _uof.Commit();

        var produtoDeletadoDto = _mapper.Map<ProductDTO>(produtoDeletado);

        return Ok(produtoDeletadoDto);
    }
}
