using Api.Entities;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SystemCustomizationController : GenericRepositoryController<SystemCustomization>
    {
        public SystemCustomizationController(GenericRepositoryServices<SystemCustomization> genericRepositoryServices)
             : base(genericRepositoryServices)
        {
          
        }

        [AllowAnonymous]
        [HttpGet]
        public override async Task<ActionResult<List<SystemCustomization>>> GetAllEntities()
        {
            var entities = await _genericRepositoryServices.GetList();
            return Ok(entities);
        }
        [AllowAnonymous]
        [HttpGet("find/{id}")]
        public override async Task<ActionResult<SystemCustomization>> GetByIdAsync(int id)
        {
            var entityExist = await _genericRepositoryServices.GetByIdAsync(id);
            return Ok(entityExist);
        }
        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public override async Task<ActionResult<SystemCustomization>> UpdateEntity(int id, SystemCustomization entity)
        {
            if (id != entity.Id)
                return BadRequest("ID da URL diferente do ID do objeto.");

            // Buscar a entidade existente no banco
            var existingEntity = await _genericRepositoryServices.GetByIdAsync(id);
            if (existingEntity == null)
                return NotFound();

            // Atualizar campo a campo (evita problemas de tracking e valores indesejados)
            existingEntity.NomeSistema = entity.NomeSistema?.Trim();
            existingEntity.CorPrimaria = entity.CorPrimaria?.Trim();
            existingEntity.CorSecundaria = entity.CorSecundaria?.Trim();
            existingEntity.BackgroundColor = entity.BackgroundColor?.Trim();
            existingEntity.LogoUrl = entity.LogoUrl;
            existingEntity.BackgroundUrl = entity.BackgroundUrl;
            existingEntity.CardsColors = entity.CardsColors;
            existingEntity.Descricao = entity.Descricao;

            // Chama o serviço para atualizar (ele deve usar o Update genérico do EF)
            await _genericRepositoryServices.UpdateAsync(existingEntity);

            return Ok(existingEntity);
        }



    }
}
