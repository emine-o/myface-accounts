using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/posts")]
    public class PostsController : ControllerBase
    {    
        private readonly IPostsRepo _posts;
        private readonly AuthController _authController;

        public PostsController(IPostsRepo posts, AuthController authController)
        {
            _posts = posts;
            _authController = authController;
        }
        
        [HttpGet("")]
        public ActionResult<PostListResponse> Search([FromQuery] PostSearchRequest searchRequest)
        {
            bool isAuthHeaderCorrect = _authController.CheckAuthHeader();

            if (!isAuthHeaderCorrect)
            {
                return BadRequest("Authorization header is not correct.");
            }
            var posts = _posts.Search(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return PostListResponse.Create(searchRequest, posts, postCount);
        }

        [HttpGet("{id}")]
        public ActionResult<PostResponse> GetById([FromRoute] int id)
        {
            var post = _posts.GetById(id);
            return new PostResponse(post);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePostRequest newPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isAuthHeaderCorrect = _authController.CheckAuthHeader();

            if (!isAuthHeaderCorrect)
            {
                return BadRequest("Authorization header is not correct.");
            }

            var post = _posts.Create(newPost);

            var url = Url.Action("GetById", new { id = post.Id });
            var postResponse = new PostResponse(post);
            return Created(url, postResponse);
        }

        [HttpPatch("{id}/update")]
        public ActionResult<PostResponse> Update([FromRoute] int id, [FromBody] UpdatePostRequest update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isAuthHeaderCorrect = _authController.CheckAuthHeader();

            if (!isAuthHeaderCorrect)
            {
                return BadRequest("Authorization header is not correct.");
            }

            var post = _posts.Update(id, update);
            return new PostResponse(post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _posts.Delete(id);
            return Ok();
        }
    }
}