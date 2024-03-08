

using CatalogAPI.Controllers;
using CatalogAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPIxUnitTest.UnitTests;

public class PutProductsUnitTests : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public PutProductsUnitTests(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    //testes de unidade para PUT
    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        //Arrange  
        var prodId = 7;

        var updatedProdutoDto = new ProductDTO
        {
            ProductId = prodId,
            Name = "Produto Atualizado - Testes",
            Description = "Minha Descricao",
            ImageUrl = "imagem1.jpg",
            CategoryId = 2
        };

        // Act
        var result = await _controller.Put(prodId, updatedProdutoDto) as ActionResult<ProductDTO>;

        // Assert  
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é OkObjectResult
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        //Arrange
        var prodId = 1000;

        var meuProduto = new ProductDTO
        {
            ProductId = 7,
            Name = "Produto Atualizado - Testes",
            Description = "Minha Descricao alterada",
            ImageUrl = "imagem11.jpg",
            CategoryId = 2
        };

        //Act              
        var data = await _controller.Put(prodId, meuProduto);

        // Assert  
        data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);

    }
}
