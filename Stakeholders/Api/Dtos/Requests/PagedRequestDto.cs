using System.ComponentModel.DataAnnotations;

namespace Stakeholders.Api.Dtos.Requests;

public class PagedRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize { get; set; }
}