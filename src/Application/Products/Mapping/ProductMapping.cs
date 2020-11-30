using Application.Abstractions.Mapping;
using Application.Products.Commands.Create;
using Domain.Common.ValueObjects;
using Domain.Products;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Application.Products.Mapping
{
    public class CreateProductMapping : ICustomMapper<Product, CreateProductCommand>
    {
        private readonly TypeAdapterConfig config = new TypeAdapterConfig();
        public CreateProductMapping()
        {
            MappingConfig();
        }
        public void MappingConfig()
        {
            //entity -> dto
            config.NewConfig<Product, CreateProductCommand>().Map(des => des.Price, src => src.Price.Value)
                .Map(des => des.OldPrice, src => src.OldPrice.Value).ShallowCopyForSameType(true);
            //dto -> entity
            config.NewConfig<CreateProductCommand, Product>().Map(des => des.Price, src => new Money(src.Price))
                .Map(d => d.OldPrice, s => new Money(s.OldPrice)).ShallowCopyForSameType(true);
        }
        public CreateProductCommand ToDto(Product entity)
        {
            return entity.Adapt<CreateProductCommand>(config);
        }
        public Product ToEntity(CreateProductCommand dto)
        {
            return dto.Adapt<Product>(config);
        }
    }
}
