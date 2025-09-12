using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    // root URl => https://localhost:7080
    // Base URl => Root + Route -> https://localhost:7080/api/Categories
    // spatial URl =>  https://localhost:7080/api/Categories/MyPosts/3
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // DI
        private readonly ICategoryService _categoryService;
        public CategoriesController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Request Method
        [HttpGet] // https://localhost:7080/api/Categories
        public async Task<IActionResult> Get()
        {
            try
            {
                var Categories = await _categoryService.GetAllAsync();
                if (Categories is null || !Categories.Any())
                    return NotFound(new
                    {
                        Message = "No Categories Found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Data = new List<Category>(),
                    });
                return Ok(new
                {
                    message ="Categories Retrived Successfully!",
                    StatusCode = StatusCodes.Status200OK,
                    Data = Categories,
                });

            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while retriving Categories!",
                    Error = ex.Message,

                });
            }

        }
        [HttpGet("{id}")] // https://localhost:7080/api/Categories/{id}
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Category = await _categoryService.GetByIdAsync(id);
                if (Category is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With Id {id} Not Found",
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Retrived Successfully!",
                    Data = Category
                });


            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while retriving Categories!",
                    Error = ex.Message,

                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add(CategoryDTo category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    });
                await _categoryService.CreateAsync(category);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Caategory Created Successfully"
                });




            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while Creating Categories!",
                    Error = ex.Message,

                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id , CategoryDTo category)
        {
            try
            {
                var OldCategory = await _categoryService.GetByIdAsync(id);
                if (OldCategory is null)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Category Id Missmatch"
                    });

                var result = await _categoryService.UpdateAsync(id,category);
                if (result)
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category Updated successfully"
                    });
                else
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category Not Updated "
                    });


            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while updating Categories!",
                    Error = ex.Message,

                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTo category)
        {
            try
            {
                var OldCategory = await _categoryService.GetByIdAsync(category.CatId);
                if (OldCategory is null)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Category Id Missmatch"
                    });

                var result = await _categoryService.UpdateAsync(category);
                if (result)
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category Updated successfully"
                    });
                else
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category Not Updated "
                    });


            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while updating Categories!",
                    Error = ex.Message,

                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (result)
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Category Deleted Successfully"
                    });
                else
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With ID {id} Not Found"
                    });
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "An Error Occures while Deleting Categories!",
                    Error = ex.Message,

                });
            }
        }
    }
}
