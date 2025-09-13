using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // DI
        private readonly IGenaricReposatory<Category> _category;
        public CategoriesController(IGenaricReposatory<Category> category)
        {
            _category = category;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Categories = await _category.GetAllAsync();
                if (Categories is null || !Categories.Any())
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Categories Not Found",
                        Data = new List<Category>()
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Categories retrived successfully",
                    Data = Categories
                });
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Retriving data",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Category = await _category.GetByIdAsync(id);
                if (Category is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category with id {id} Not Found"
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status302Found,
                    Message = "Categorr retrived Successfully",
                    Data = Category
                });
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Retriving data",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTo categoryDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Category = new Category
                {
                    Name = categoryDTo.Name,
                };
                await _category.CreateAsync(Category);
                await _category.SaveAsync();
                return StatusCode(201, new
                {
                    StatusCode = 201,
                    Message = "Category Created Successfully",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Retriving data",
                    Error = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTo categoryDTo)
        {
            try
            {
                var OldCategory = await _category.GetByIdAsync(categoryDTo.CatId);
                if (OldCategory is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With Id {categoryDTo.CatId} Not Found",

                    });
                OldCategory.Name = categoryDTo.Name;
                _category.Update(OldCategory);
                await _category.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Retriving data",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Category = await _category.GetByIdAsync(id);
                if (Category is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Category Not Found"
                    });
                _category.Delete(Category);
                await _category.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Retriving data",
                    Error = ex.Message
                });
            }
        }
    }
}
