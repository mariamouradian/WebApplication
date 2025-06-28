using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Seminar3.Abstractions;
using Seminar3.Models;
using Seminar3.Models.Dto;

namespace Seminar3.Services
{
    public class StorageService : IStorageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public StorageService(AppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public int AddStorage(StorageDto storage)
        {
            var entity = _mapper.Map<StorageEntity>(storage);
            _context.Storages.Add(entity);
            _context.SaveChanges();
            _cache.Remove("storages");
            return entity.Id;
        }

        public IEnumerable<StorageDto> GetStorages()
        {
            if (_cache.TryGetValue("storages", out List<StorageDto>? storages) && storages != null)
            {
                return storages;
            }

            storages = _context.Storages
                .Select(x => _mapper.Map<StorageDto>(x))
                .ToList();

            _cache.Set("storages", storages, TimeSpan.FromMinutes(30));
            return storages;
        }
    }
}