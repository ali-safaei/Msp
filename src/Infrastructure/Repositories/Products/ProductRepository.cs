using Application.Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Products;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IInfraUnitOfWork unitOfWork) :
            base(unitOfWork)
        { }
    }
}
