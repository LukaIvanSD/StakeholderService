using System.Data;
using System.Security.Claims;

namespace Stakeholders.Core.Domain
{
    public enum UserRole
    {
        Administrator,
        Tourist,
        Author
    }
    public class User : Entity
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public UserRole Role { get; private set; }

        public bool IsBlocked { get; private set; } = false;

        public User() { }

        public User(string username,string password,string email, UserRole role, bool isBlocked)
        {
            Username = username;
            Password = password;
            Email = email;
            Role = role;
            IsBlocked = isBlocked;
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Invalid Username");
            if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentException("Invalid Password");
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentException("Invalid Email");
        }

        internal string GetPrimaryRoleName()
        {
            return Role.ToString().ToLower(); ;
        }

        internal bool IsAdmin()
        {
            return Role == UserRole.Administrator;
        }
    }
}
