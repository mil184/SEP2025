using Domain.Models;

namespace Domain.Services
{
    public interface IUserService
    {
        User GetByEmail(string email);
        User Create(User user);
        IEnumerable<User> GetAll();
        User? GetById(Guid id);
        void Update(User user);
    }
}
