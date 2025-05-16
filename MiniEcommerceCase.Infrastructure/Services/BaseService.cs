using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniEcommerceCase.Application.Interfaces;
using MiniEcommerceCase.Domain.Common;

namespace MiniEcommerceCase.Infrastructure.Services
{
    public abstract class BaseService<TEntity, TRequest, TResponse> : IBaseService<TRequest, TResponse>
        where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly IMapper _mapper;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogger _logger;

        public BaseService(DbContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = _context.Set<TEntity>();
            _logger = logger;   
        }

        public virtual async Task<TResponse> CreateAsync(TRequest request)
        {
            var entity = _mapper.Map<TEntity>(request);
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResponse>(entity);
        }

        public virtual async Task<List<TResponse>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<List<TResponse>>(entities);
        }
    }
}



















