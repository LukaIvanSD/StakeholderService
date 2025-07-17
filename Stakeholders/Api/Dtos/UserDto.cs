using Stakeholders.Core.Domain;

namespace Stakeholders.Api.Dtos;

public class UserDto
{
    public long Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public UserRole Role { get; set; }
    
    public bool IsBlocked { get; set; }
}