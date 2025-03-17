using AutoMapper;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Services
{
    public class GenericRepositoryServices<T> where T : class, IEntity
    {
        private readonly IGenericRepository<T> _genericRepository;
        private readonly IMapper _mapper;

        public GenericRepositoryServices(IGenericRepository<T> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _genericRepository.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar entidade por ID.", ex);
            }
        }
        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");
            }
            
            return await _genericRepository.CreateAsync(entity);

        }
        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            { 
                throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");
            }
            if (entity.Id <= 0)
            {
                throw new ArgumentException($"O ID informado ({entity.Id}) é inválido para atualização. Certifique-se de fornecer um ID válido.");
            }
            var entityExist = await _genericRepository.GetByIdAsync(entity.Id);

            if (entityExist == null)
            {
                throw new KeyNotFoundException($"Entidade com ID {entity.Id} não encontrada.");
            }

            _mapper.Map(entity, entityExist);

            return await _genericRepository.UpdateAsync(entityExist);



        }
        public async Task<bool> DeleteAsync(int id)
        {
            
            if (id <= 0)
            {
                throw new ArgumentException($"O ID informado ({id}) é inválido. Certifique-se de fornecer um ID válido.");
            }
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");
            }

            return await _genericRepository.DeleteAsync(id);

        }

    }
}
