using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureworksAPI.Models;

namespace AdventureworksAPI.Controllers
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
                        ProductId = Product.ProductId,
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
        public async Task<ActionResult<ProductCatalogDTO>> GetProduct(int id)
        {
            var product = await _context.Product
            .Where(p => p.ProductId == id )
            .Join(
                _context.ProductSubcategory,
                product => product.ProductSubcategoryId,
                ProductSubcategory => ProductSubcategory.ProductCategoryId,
                (Product, ProductSubcategory) => 
                    new ProductCatalogDTO {
                        ProductId = Product.ProductId,
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
            ).FirstAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        // GET: api/ProductsCatalog/images
        [HttpGet("images")]
        public async Task<ActionResult<List<ProductProductPhoto>>> GetDBImages(){  
            return await _context.ProductProductPhoto.ToListAsync();    
        }
        // GET: api/ProductsCatalog/images/thumb/{id}
        [HttpGet("images/thumb/{id}")]
        public async Task<ActionResult> GetDBImageThumbnail(int id){
            byte[] img = await _context.ProductPhoto
                .Where(photo => photo.ProductPhotoId == id)
                .Select(photo => photo.ThumbNailPhoto)
                .SingleOrDefaultAsync();

            return File(img, "image/gif");
        }
        // GET: api/ProductsCatalog/images/large/{id}
        [HttpGet("images/large/{id}")]
        public async Task<ActionResult> GetDBImage(int id){
            byte[] img = await _context.ProductPhoto
                .Where(photo => photo.ProductPhotoId == id)
                .Select(photo => photo.LargePhoto)
                .SingleOrDefaultAsync();

            return File(img, "image/gif");
        }      
    }
}
