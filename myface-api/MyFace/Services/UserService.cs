using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFace.Helpers;
using MyFace.Models.Database;
using MyFace.Repositories;

namespace MyFace.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {

        private readonly IUsersRepo _users;

        public UserService(IUsersRepo users) {
            _users = users;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = _users.GetByUserName(username);

            if (user == null)
                return null;

            var HashedPassword = SaltAndHashCreator.GetHash(password, user.Salt);

            if (HashedPassword != user.HashedPassword) {
                return null;
            }

            // authentication successful
            return user;
        }
    }
}