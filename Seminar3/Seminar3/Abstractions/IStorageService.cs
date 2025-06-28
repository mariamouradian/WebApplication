using Seminar3.Models.Dto;

namespace Seminar3.Abstractions
{
    public interface IStorageService
    {
        IEnumerable<StorageDto> GetStorages();
        int AddStorage(StorageDto storage);
    }
}