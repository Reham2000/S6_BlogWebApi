using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        // DI
        private readonly IGenaricReposatory<Post> _posts;
        public PostsController(IGenaricReposatory<Post> posts)
        {
            _posts = posts;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Posts = await _posts.GetAllAsync();
                if(Posts == null || !Posts.Any())
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Posts Not Found",
                        Data = new List<Post>()
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Date Retrived Successfully",
                    Data = Posts
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
        [HttpGet("GetByCategoryId/{id}")]
        public async Task<IActionResult> GetByCategoryId(int id)
        {
            try
            {
                var Posts = await _posts.FindAsync(p => p.CategoryId == id);
                if (Posts is null || !Posts.Any())
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Posts Not Found",
                        Data = new List<Post>()
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Posts Retrived Successfully",
                    Data = Posts
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
        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var Posts = await _posts.FindAsync(p => p.UserId == id);
                if (Posts is null || !Posts.Any())
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Posts Not Found",
                        Data = new List<Post>()
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Posts Retrived Successfully",
                    Data = Posts
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Post = await _posts.GetByIdAsync(id);
                if(Post is null)
                {
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Posts Not Found"
                    });
                }
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Post Retrived Successfully",
                    Data = Post
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

        [HttpPost]
        public async Task<IActionResult> Create (PostDTo postDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Post = new Post
                {
                    Title = postDTo.Title,
                    Content = postDTo.Content,
                    CreatedAt = DateTime.Now,
                    CategoryId = postDTo.CategoryId,
                    UserId = postDTo.UserId
                };
                await _posts.CreateAsync(Post);
                await _posts.SaveAsync();
                return StatusCode(201, new
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Post Created Successfully",

                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Createing data",
                    Error = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update (PostDTo postDTo)
        {
            try
            {
                var OldPost = await _posts.GetByIdAsync(postDTo.Id);
                if (OldPost is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Post Not Found"
                    });

                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                OldPost.Title = postDTo.Title;
                OldPost.Content = postDTo.Content;
                OldPost.CategoryId = postDTo.CategoryId;
                _posts.Update(OldPost);
                await _posts.SaveAsync();
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Updating data",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Post = await _posts.GetByIdAsync(id);
                if (Post is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Post Not Found",

                    });
                _posts.Delete(Post);
                await _posts.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occuered While Deleteing data",
                    Error = ex.Message
                });
            }
        }
    }
}
