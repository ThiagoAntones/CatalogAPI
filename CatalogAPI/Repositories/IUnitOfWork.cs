﻿namespace CatalogAPI.Repositories;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task Commit();
}
