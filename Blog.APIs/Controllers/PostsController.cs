using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PostsController : ControllerBase
    {
        // DI
        //private readonly IGenaricReposatory<Post> _posts;
        //public PostsController(IGenaricReposatory<Post> posts)
        //{
        //    _posts = posts;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Policy = "AllRoles")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Posts = await _unitOfWork.Posts.GetAllAsync();
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
        [Authorize(Roles = "Admin,Reader")]
        [HttpGet("NewAll")]
        public async Task<IActionResult> NewGetAll()
        {
            try
            {
                var Posts = await _unitOfWork.Posts.GetAllAsync(
                    //predicate: p => p.CategoryId == 1,
                    includes: new Expression<Func<Post, object>>[]
                    {
                        p => p.User,
                        p => p.Category,
                        p => p.Comments
                    }
                    );
                if (Posts == null || !Posts.Any())
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
                    Data = Posts.Select(p => new 
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        CreatedAt = p.CreatedAt,
                        UserId = p.UserId,
                        UserName = p.User.UserName,//
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name, //
                        Comments = p.Comments.Select(c => new 
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            UserId = c.UserId,
                            UserName = c.User.UserName,
                            PostId = c.PostId
                        }),//

                    })
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
                var Posts = await _unitOfWork.Posts.FindAsync(p => p.CategoryId == id);
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
        public async Task<IActionResult> GetByUserId(string id)
        {
            try
            {
                var Posts = await _unitOfWork.Posts.FindAsync(p => p.UserId == id);
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
        [Authorize(Policy = "ManageOnly")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Post = await _unitOfWork.Posts.GetByIdAsync(id);
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
        [Authorize]
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
                await _unitOfWork.Posts.CreateAsync(Post);
                await _unitOfWork.SaveAsync();
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
                var OldPost = await _unitOfWork.Posts.GetByIdAsync(postDTo.Id);
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
                _unitOfWork.Posts.Update(OldPost);
                await _unitOfWork.SaveAsync();
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
                var Post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (Post is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Post Not Found",

                    });
                _unitOfWork.Posts.Delete(Post);
                await _unitOfWork.SaveAsync();
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
