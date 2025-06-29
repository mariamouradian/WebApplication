using Seminar4Application.DataStore.Entity;

namespace Seminar4Application.Abstraction
{
    public interface IUserService
    {
        public Guid UserAdd(string name, string password, UserRole roleId);
        public string CheckUserRole(string name, string password);

    }
}
