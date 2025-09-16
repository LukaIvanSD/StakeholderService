namespace Stakeholders.Api.Dtos
{
  public class TokenDto
  {
    public string? Role { get; set; }
    public long? UserId { get; set; }
    public long? PersonId { get; set; }
    public bool IsValid { get; set; }
  }
}