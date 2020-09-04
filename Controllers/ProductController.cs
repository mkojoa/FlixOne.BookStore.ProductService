﻿using FlixOne.BookStore.ProductService.Models;
using FlixOne.BookStore.ProductService.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FlixOne.BookStore.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository) => _productRepository = productRepository;

        [HttpGet]
        [Route("productlist")]
        public IActionResult GetList()
        {
            return new OkObjectResult(_productRepository.GetAll().Select(ToProductvm).ToList());
        }

        [HttpGet]
        [Route("product/{productid}")]
        public IActionResult Get(string productId)
        {
            var productModel = _productRepository.GetBy(new Guid(productId));

            return new OkObjectResult(ToProductvm(productModel));
        }

        [HttpPost]
        [Route("addproduct")]
        public IActionResult Post([FromBody] ProductViewModel productvm)
        {
            if (productvm == null)
                return BadRequest();
            var productModel = ToProductModel(productvm);

            _productRepository.Add(productModel);

            return StatusCode(201);
            //return StatusCode(201, Json(true));
        }

        [HttpPut]
        [Route("updateproduct/{productid}")]
        public IActionResult Update(string productid, [FromBody] ProductViewModel productvm)
        {
            var productId = new Guid(productid);
            if (productvm == null || productvm.ProductId != productId)
                return BadRequest();

            var product = _productRepository.GetBy(productId);
            if (product == null)
                return NotFound();

            product.Name = productvm.ProductName;
            product.Description = productvm.ProductDescription;
            _productRepository.Update(product);
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("deleteproduct/{productid}")]
        public IActionResult Delete(string productid)
        {
            var productId = new Guid(productid);
            var product = _productRepository.GetBy(productId);
            if (product == null)
                return NotFound();

            _productRepository.Remove(productId);
            return new NoContentResult();
        }

        private Product ToProductModel(ProductViewModel productvm)
        {
            return new Product
            {
                CategoryId = productvm.CategoryId,
                Description = productvm.ProductDescription,
                Id = productvm.ProductId,
                Name = productvm.ProductName,
                Price = productvm.ProductPrice
            };
        }

        private ProductViewModel ToProductvm(Product productModel)
        {
            return new ProductViewModel
            {
                CategoryId = productModel.CategoryId,
                CategoryDescription = productModel.Category.Description,
                CategoryName = productModel.Category.Name,
                ProductDescription = productModel.Description,
                ProductId = productModel.Id,
                ProductImage = productModel.Image,
                ProductName = productModel.Name,
                ProductPrice = productModel.Price
            };
        }
    }
}
