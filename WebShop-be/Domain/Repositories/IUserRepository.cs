using Domain.Models;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        User Create(User user);
        void Delete(User user);
        IEnumerable<User> GetAll();
        User? GetById(Guid id);
        void Update(User user);
    }
}
