﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extentions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using System.Runtime.InteropServices;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository) 
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await productRepository.GetItems();
                var productCategories = await productRepository.GetCategories();
            
                if (products == null || productCategories == null)
                {
                    return NotFound();
                } 
                else
                {
                    var productDtos = products.ConvertToDto(productCategories);

                    return Ok(productDtos);
                }


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }
    }
}
