using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : GenericRepositoryController<AppUser>
    {
        private readonly LoginServices _loginServices;
        private readonly CreateUserAdminServices _createUserAdmin;
        public AppUserController(GenericRepositoryServices<AppUser> genericRepositoryServices, LoginServices loginServices, CreateUserAdminServices createUserAdmin)
              : base(genericRepositoryServices)
        {
            _loginServices = loginServices;
            _createUserAdmin = createUserAdmin;
        }
        public override Task<ActionResult<AppUser>> GetByIdAsync(int id)
        {
            return Task.FromResult<ActionResult<AppUser>>(Unauthorized("Acesso negado. Você não tem permissão para executar esta ação."));
        }
        [HttpPost("create")]
        [AllowAnonymous] 
        public override async Task<ActionResult<AppUser>> CreateEntity(AppUser entity)
        {
            var createdEntity = await _genericRepositoryServices.CreateAsync(entity);
            return Created("", createdEntity);
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPost("createUserAdmin")]
        public async Task<ActionResult<AppUser>> CreateUserAdmin(AppUser appUser)
        {
            var createuserAdmin = await _createUserAdmin.CreateAppuserAdminAsync(appUser);

            return Created("", createuserAdmin);

        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _loginServices.GetUserProfileByEmailAsync(email);

            if (user == null)
                return NotFound("Usuário não encontrado.");

           
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl) && !user.ProfilePictureUrl.StartsWith("http"))
            {
                user.ProfilePictureUrl = $"{Request.Scheme}://{Request.Host}/uploads/{user.ProfilePictureUrl}";
            }

            return Ok(user);
        }
        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido");

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(new { url = imageUrl });
        }

    }
  
}
