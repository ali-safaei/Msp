using Application.Abstractions.Dtos;
using Application.Abstractions.Identity;
using Application.Abstractions.Mapping;
using Application.Abstractions.Repositories;
using Application.Products.Dtos;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<Product>, IBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool AllowCustomerReviews { get; set; }
        public string Sku { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int StockQuantity { get; set; }
        public bool DisplayStockAvailability { get; set; }
        public int MinStockQuantity { get; set; }
        public int LowStockActivityId { get; set; }
        public int OrderMinimumQuantity { get; set; }
        public int OrderMaximumQuantity { get; set; }
        public bool CallForPrice { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly ICustomMapper<Product, CreateProductCommand> _customMapper;
        private readonly ICurrentUser _currentUser;
        private readonly IProductRepository _productRepository;
        public CreateProductCommandHandler(ICustomMapper<Product, CreateProductCommand> customMapper,
            ICurrentUser currentUser, IProductRepository productRepository)
        {
            _customMapper = customMapper;
            _currentUser = currentUser;
            _productRepository = productRepository;
        }
        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _customMapper.ToEntity(request);
            product.CreatedUtc = DateTime.UtcNow;
            product.CreatedBy = _currentUser.UserId;
            await _productRepository.AddAsync(product, cancellationToken);
            await _productRepository.SaveChangeAsync(cancellationToken);
            return product;
        }
    }
}
