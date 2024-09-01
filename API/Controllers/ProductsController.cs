using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepositry productRepositry;
    public ProductsController(IProductRepositry productRepositry)
    {
        this.productRepositry = productRepositry;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand,string? type, string? sort)
    {
        return Ok(await productRepositry.GetProductsAsync(brand,type,sort));
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepositry.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepositry.AddProduct(product);
        if (await productRepositry.SaveChangesAsync())
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

        productRepositry.UpdateProduct(product);
        if (await productRepositry.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem while updating the product");
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepositry.GetProductByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        productRepositry.DeleteProduct(product);
        if (await productRepositry.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem while deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands(){
            return  Ok(await productRepositry.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes(){
            return  Ok(await productRepositry.GetTypesAsync());
    }
    private bool ProductExixts(int id)
    {
        return productRepositry.ProductExists(id);
    }

}
