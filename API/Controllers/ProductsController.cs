using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // private readonly IProductRepositry productRepositry;
    // public ProductsController(IProductRepositry productRepositry)
    // {
    //     this.productRepositry = productRepositry;
    // }
     private readonly IGenericRepositry<Product> repositry;
    public ProductsController(IGenericRepositry<Product> repositry)
    {
        this.repositry = repositry;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand,string? type, string? sort)
    {
        var spec = new ProductSpecification(brand,type,sort);
        var products = await repositry.ListAsync(spec);
        return Ok(products);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repositry.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repositry.Add(product);
        if (await repositry.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("problem while creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExixts(id))
        {
            return BadRequest("Cannot update this product");
        }

        repositry.Update(product);
        if (await repositry.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem while updating the product");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repositry.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        repositry.Remove(product);
        if (await repositry.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem while deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands(){
           // implement method
            return  Ok();
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes(){
            // implement method
            return  Ok();
    }
    private bool ProductExixts(int id)
    {
        return repositry.Exists(id);
    }

}
