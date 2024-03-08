using AutoMapper;
using CatalogAPI.Context;
using CatalogAPI.DTOs.Mappings;
using CatalogAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPIxUnitTest.UnitTests;

public class ProductsUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static string connectionString =
      "Server=localhost;DataBase=CatalogDB;Uid=root;Pwd=d#tqX.IgJ~AKVW0";
    static ProductsUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .Options;
    }
    public ProductsUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductDTOMappingProfile());
        });

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }

    //criar testes de unidade
}
