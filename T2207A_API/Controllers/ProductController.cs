using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2207A_API.Entities;
using T2207A_API.DTOs;
using Microsoft.EntityFrameworkCore;
using T2207A_API.Models.Product;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2207A_API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly T2207aApiContext _context;
        public ProductController(T2207aApiContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            List<ProductDTO> ls = new List<ProductDTO>();
            foreach (Product p in products)
            {
                ls.Add(new ProductDTO
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    description = p.Description,
                    thumbnail = p.Thumbnail,
                    qty = p.Qty,
                    category_id = p.CategoryId,
                    category = new CategoryDTO { id = p.Category.Id, name = p.Category.Name },
                });
            }
            return Ok(ls);
        }
        [HttpPost]
        public IActionResult Create(CreateProduct model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product data = new Product { Name = model.name, Price = model.price, Thumbnail = model.thumbnail,Description = model.description,Qty = model.qty, CategoryId = model.category_id };
                    _context.Products.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                        new ProductDTO { id = data.Id, name = data.Name, price = data.Price, thumbnail = data.Thumbnail, description=data.Description, qty=data.Qty,category_id=data.CategoryId });
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Product p = _context.Products
                    .Where(p => p.Id == id)
                    .Include(p => p.Category)
                    .First();
                if (p == null)
                    return NotFound();
                return Ok(new ProductDTO
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    description = p.Description,
                    thumbnail = p.Thumbnail,
                    qty = p.Qty,
                    category_id = p.CategoryId,
                    category = new CategoryDTO { id = p.Category.Id, name = p.Category.Name },
                });

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("relateds")]
        public IActionResult Relateds(int id)
        {
            try
            {
                Product p = _context.Products.Find(id);
                if (p == null)
                    return NotFound();
                List<Product> ls = _context.Products
                    .Where(p => p.CategoryId == p.CategoryId)
                    .Where(p => p.Id != id)
                    .Include(p => p.Category)
                    .Take(4)
                    .OrderByDescending(p => p.Id)
                    .ToList();
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}