using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetByEmail(string email)
        {
            var user = GetAll().FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new ArgumentException("No user found with the specified email.");
            }

            return user;
        }

        public User Create(User user)
        {
            return _userRepository.Create(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User? GetById(Guid id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                throw new ArgumentException("No user found with the specified id.");
            }

            return user;
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
