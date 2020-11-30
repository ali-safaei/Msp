using Domain.Abstractions;
using Domain.Common.Dependencies;
using Domain.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Repositories
{
    public interface IProductRepository : IRepository<Product>, ITransient
    {
    }
}
