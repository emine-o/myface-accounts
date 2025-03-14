using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using MyFace.Helpers;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    public class LoginController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersRepo _users;


        public LoginController(IHttpContextAccessor httpContextAccessor, IUsersRepo users) {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
        }
        public bool CheckAuthHeader(int userId)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault();

            if (String.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic")) {
                return false;
            }

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':');

            var username = usernamePassword.Substring(0, seperatorIndex);
            var password = usernamePassword.Substring(seperatorIndex + 1);

            var user = _users.GetById(userId);
            var HashedPassword = SaltAndHashCreator.GetHash(password, user.Salt);

            if (username != user.Username || HashedPassword != user.HashedPassword) {
                return false;
            }

            return true;
        }
    }
}