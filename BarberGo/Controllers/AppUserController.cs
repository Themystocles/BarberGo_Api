using BarberGo.Entities;
using BarberGo.Entities.DTOs;
using BarberGo.Interfaces;
using BarberGo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32;

namespace BarberGo.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : GenericRepositoryController<AppUser>
    {
        private readonly LoginServices _loginServices;
        private readonly UserAccountServices _userAccountServices;
        
        public AppUserController(GenericRepositoryServices<AppUser> genericRepositoryServices, LoginServices loginServices, UserAccountServices userAccount)
              : base(genericRepositoryServices)
        {
            _loginServices = loginServices;
            _userAccountServices = userAccount;
        }
        public override Task<ActionResult<AppUser>> GetByIdAsync(int id)
        {
            return Task.FromResult<ActionResult<AppUser>>(Unauthorized("Acesso negado. Você não tem permissão para executar esta ação."));
        }
        [Obsolete("Use o endpoint /register em vez deste.")]
        [HttpPost("create")]
        [AllowAnonymous] 
        public override async Task<ActionResult<AppUser>> CreateEntity(AppUser entity)
        {

            try
            {
                await _userAccountServices.VerifyEmailExsist(entity.Email);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }


            var passwordHasher = new PasswordHasher<AppUser>();
            entity.PasswordHash = passwordHasher.HashPassword(entity, entity.PasswordHash);

            var createdEntity = await _genericRepositoryServices.CreateAsync(entity);
            return Created("", createdEntity);



        }
        [HttpPost("register")]
        [AllowAnonymous]
        public  async Task<ActionResult<RegisterUserDto>> RegisterUser(RegisterUserDto entity)
        {
            try
            {
                await _userAccountServices.VerifyEmailExsist(entity.Email);
            } 
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            try
            {
                await _userAccountServices.verifyPassword(entity.Password, entity.ConfirmPassword); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            

            var appUser = new AppUser();
                appUser.Name = entity.Name;
                appUser.Email = entity.Email;
                appUser.Phone = entity.Phone;
                appUser.ProfilePictureUrl = entity.ProfilePictureUrl;
                appUser.Type = entity.Type;

            var passwordHasher = new PasswordHasher<AppUser>();
            appUser.PasswordHash = passwordHasher.HashPassword(appUser, entity.Password);


            var createdEntity = await _genericRepositoryServices.CreateAsync(appUser);
            return Created("", createdEntity);

        }




        [Authorize(Policy ="AdminOnly")]
        [HttpPost("createUserAdmin")]
        public async Task<ActionResult<RegisterUserDto>> CreateUserAdmin(RegisterUserDto entity)
        {

            try
            {
                await _userAccountServices.VerifyEmailExsist(entity.Email);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            try
            {
                await _userAccountServices.verifyPassword(entity.Password, entity.ConfirmPassword);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            var appUser = new AppUser();
            appUser.Name = entity.Name;
            appUser.Email = entity.Email;
            appUser.Phone = entity.Phone;
            appUser.ProfilePictureUrl = entity.ProfilePictureUrl;
            appUser.Type = entity.Type;

            var passwordHasher = new PasswordHasher<AppUser>();
            appUser.PasswordHash = passwordHasher.HashPassword(appUser, entity.Password);

            var createuserAdmin = await _userAccountServices.CreateAppuserAdminAsync(appUser);

            return Created("", appUser);

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
