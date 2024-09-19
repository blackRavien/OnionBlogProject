using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Services.AbstractServices;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authorList = await _authorService.GetAuthors();
            return Ok(authorList); // Burada AuthorVm kullanılıyor.
        }
    }
}
