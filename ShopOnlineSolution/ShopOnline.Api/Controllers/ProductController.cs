﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await productRepository.GetItem(id);
               
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productCategory = await productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productCategory);

                    return Ok(productDto);
                }


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }


        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();

                if (productCategories == null)
                {
                    return NotFound();
                }

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await productRepository.GetItemsByCategory(categoryId);
                var productCategories = await productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }

                var productDtos = products.ConvertToDto(productCategories);

                return Ok(productDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                               "Error retrieving data from the database");
            }
        }
    }
}
 