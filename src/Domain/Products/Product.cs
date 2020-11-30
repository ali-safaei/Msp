using Domain.Common;
using Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Products
{
    public class Product :
        AuditableEntity<int>, ISoftDelete, ISeo, IPublished
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
        public Money Price { get; set; }
        public Money OldPrice { get; set; }

        //isoftdelete
        public bool Deleted { get; set; }
        //iseo 
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        //ipublished
        public bool Published { get; set; }


        //public Product() { }
        
    }
}
