using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public User GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("GetByEmail failed: email was empty");
                return null;
            }

            var user = GetAll().FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                _logger.LogWarning("GetByEmail: user not found for email {Email}", email);
                return null;
            }

            _logger.LogDebug("GetByEmail: user found. userId {UserId}", user.Id);
            return user;
        }

        public User Create(User user)
        {
            _logger.LogInformation("Create user attempt. email {Email}", user?.Email);

            if (user == null)
            {
                _logger.LogWarning("Create failed: user was null");
                return null;
            }

            try
            {
                var created = _userRepository.Create(user);

                if (created == null)
                {
                    _logger.LogWarning("Create failed: repository returned null. email {Email}", user.Email);
                    return null;
                }

                _logger.LogInformation("Create succeeded. userId {UserId} email {Email}", created.Id, created.Email);
                return created;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create error. email {Email}", user.Email);
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            _logger.LogDebug("GetAll attempt");

            try
            {
                var users = _userRepository.GetAll();

                // Avoid enumerating twice; just log that call returned something.
                _logger.LogDebug("GetAll succeeded");
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAll error");
                throw;
            }
        }

        public User? GetById(Guid id)
        {
            _logger.LogInformation("GetById attempt. userId {UserId}", id);

            if (id == Guid.Empty)
            {
                _logger.LogWarning("GetById failed: id was empty");
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }

            try
            {
                var user = _userRepository.GetById(id);

                if (user == null)
                {
                    _logger.LogWarning("GetById: user not found. userId {UserId}", id);
                    throw new ArgumentException("No user found with the specified id.");
                }

                _logger.LogDebug("GetById: user found. userId {UserId} email {Email}", user.Id, user.Email);
                return user;
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                _logger.LogError(ex, "GetById error. userId {UserId}", id);
                throw;
            }
        }

        public void Update(User user)
        {
            _logger.LogInformation("Update user attempt. userId {UserId} email {Email}", user?.Id, user?.Email);

            if (user == null)
            {
                _logger.LogWarning("Update failed: user was null");
                return;
            }

            try
            {
                _userRepository.Update(user);
                _logger.LogInformation("Update succeeded. userId {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update error. userId {UserId}", user.Id);
                throw;
            }
        }
    }
}
