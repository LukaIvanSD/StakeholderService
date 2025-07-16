using Stakeholders.Core.Domain;

namespace Stakeholders.Api.Dtos
{
    public class AccountRegistrationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string? Bio { get; set; }
        public string? PictureBase64 { get; set; }
        public string? Moto { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
