using CatalogAPI.Controllers;
using CatalogAPI.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPIxUnitTest.UnitTests;

public class PostProductsUnitTests : IClassFixture<ProductsUnitTestController>
{
    private readonly ProductsController _controller;

    public PostProductsUnitTests(ProductsUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    //metodos de testes para POST
    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        // Arrange  
        var novoProdutoDto = new ProductDTO
        {
            Name = "Novo Produto",
            Description = "Descrição do Novo Produto",
            Price = 10.99m,
            ImageUrl = "imagemfake1.jpg",
            CategoryId = 2
        };

        // Act  
        var data = await _controller.Post(novoProdutoDto);

        // Assert  
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        ProductDTO prod = null;

        // Act              
        var data = await _controller.Post(prod);

        // Assert  
        var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}
