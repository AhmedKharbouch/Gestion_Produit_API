using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todoItemProject.Data;
using todoItemProject.Models;

namespace todoItemProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GestionDbContext context;

        public ProductController(GestionDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await context.products.ToListAsync();
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await context.products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var productdb = context.products.Where(d => d.label == product.label);
           /* if (productdb.Count() > 0)
            {
                return BadRequest("Product already exists");
            }*/
            if (!productdb.IsNullOrEmpty()) return BadRequest("Product already exists");


            product.prixTT = product.prixHT + (product.prixHT * 0.2);

            if (product.categoryId != 0)
            {
                var category = await context.categories.FindAsync(product.categoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }
                product.category = category;
            }
            
            await context.products.AddAsync(product);
            await context.SaveChangesAsync();
            return Ok(product);


        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            if (product.categoryId != 0)
            {
                var category = await context.categories.FindAsync(product.categoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }
                product.category = category;
            }
            
            product.prixTT = product.prixHT + (product.prixHT * 0.2);
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await context.products.FindAsync(id);
            if (product == null) return NotFound("Product not found");
            context.products.Remove(product);
            await context.SaveChangesAsync();
            return product;
        }

        /*****************************************Categories*************************************************/

        //get all categories
        [HttpGet("categories")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await context.categories.ToListAsync();
        }
        //get category by id
        [HttpGet("categories/{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await context.categories.FindAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        //delete category by id
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await context.categories.FindAsync(id);

            //delete all product
            if (category == null) return NotFound("Category not found");
            context.categories.Remove(category);
            await context.SaveChangesAsync();
            return category;
        }

        //create category
        [HttpPost("categories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var categorydb = context.categories.Where(d => d.Name == category.Name);
            if (categorydb.Count() > 0)
            {
                return BadRequest("Category already exists");
            }
            await context.categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }

        //get category by id
        [HttpPut("categories/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await context.categories.FindAsync(id);
            if (id != category.Id) return BadRequest();

            context.Entry(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }






    }
}
