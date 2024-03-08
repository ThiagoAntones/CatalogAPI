using CatalogAPI.Controllers;
using CatalogAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPIxUnitTest.UnitTests;

public class DeleteProductsUnitTests : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public DeleteProductsUnitTests(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    //testes para o Delete
    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        var prodId = 6;

        // Act
        var result = await _controller.Delete(prodId) as ActionResult<ProductDTO>;

        // Assert  
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado é OkResult
    }

    [Fact]
    public async Task DeleteProdutoById_Return_NotFound()
    {
        // Arrange  
        var prodId = 999;

        // Act
        var result = await _controller.Delete(prodId) as ActionResult<ProductDTO>;

        // Assert  
        result.Should().NotBeNull(); // Verifica se o resultado não é nulo
        result.Result.Should().BeOfType<NotFoundObjectResult>(); // Verifica se o resultado é NotFoundResult

    }
}
