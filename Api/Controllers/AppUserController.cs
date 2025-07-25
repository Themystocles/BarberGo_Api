﻿using Domain.Entities;
using Domain.Entities.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : GenericRepositoryController<AppUser>
    {
        private readonly LoginServices _loginServices;
        private readonly UserAccountServices _userAccountServices;
        private readonly EmailConfirmationServices _emailConfirmationServices;

        public AppUserController(GenericRepositoryServices<AppUser> genericRepositoryServices, LoginServices loginServices, UserAccountServices userAccount, EmailConfirmationServices emailConfirmationServices)
              : base(genericRepositoryServices)
        {
            _loginServices = loginServices;
            _userAccountServices = userAccount;
            _emailConfirmationServices = emailConfirmationServices;
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

        [HttpPost("send-code")]
        [AllowAnonymous]
        public async Task<IActionResult> SendVerificationCode([FromBody] string email)
        {
            await _emailConfirmationServices.SendCodeverifyEmail(email);
            return Ok(new { message = "Código enviado para o email." });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterUserDto>> RegisterUser(RegisterUserDto entity)
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

            var isVerified = await _emailConfirmationServices.VerificationEmail(entity.Email, entity.RecoveryCode);
            if (!isVerified)
            {
                return BadRequest(new { message = "Código de verificação inválido ou expirado." });
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




        [Authorize(Policy = "AdminOnly")]
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
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("PromoverUsuarioparaAdmin")]
        public async Task<ActionResult<AppUser>> ToggleAdminStatus(PromoteUserDto user)
        {
            try
            {
                await _userAccountServices.ToggleAdminStatus(user.Email);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhe = ex.Message });
            }



        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult<AppUser>> getUserByEmail([FromQuery] string email)
        {
          var user =  await _userAccountServices.GetUserbyEmail(email);
            return Ok(user);

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
