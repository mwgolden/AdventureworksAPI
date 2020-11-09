using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApi.Models;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCatalogController : ControllerBase
    {
        private readonly AdventureWorks2012Context _context;

        public ProductsCatalogController(AdventureWorks2012Context context)
        {
            _context = context;
        }

        // GET: api/ProductsCatalog
        [HttpGet]
        public async Task<ActionResult<ApiQueryResult<ProductCatalogDTO>>> GetProduct(int pageIndex = 0, int pageSize = 0)
        {
            var source = _context.Product.Join(
                _context.ProductSubcategory,
                product => product.ProductSubcategoryId,
                ProductSubcategory => ProductSubcategory.ProductCategoryId,
                (Product, ProductSubcategory) => 
                    new ProductCatalogDTO {
                        Name = Product.Name,
                        ProductNumber = Product.ProductNumber,
                        Color = Product.Color,
                        ListPrice = Product.ListPrice,
                        Size = Product.Size,
                        SizeUnitMeasureCode = Product.SizeUnitMeasureCode,
                        Weight = Product.Weight,
                        WeightUnitMeasureCode = Product.WeightUnitMeasureCode,
                        ProductCategory = ProductSubcategory.Name
                    }
            ).AsQueryable();

            return await ApiQueryResult<ProductCatalogDTO>.CreateAsync(source, pageIndex, pageSize);
        }

        // GET: api/ProductsCatalog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}
