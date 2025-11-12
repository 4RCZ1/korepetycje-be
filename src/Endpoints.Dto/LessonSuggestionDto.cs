namespace Endpoints.Dto;

public class LessonSuggestionDto
{
    public string? ExternalId { get; set; }
    public DateTimeOffset? SuggestedStart { get; set; }
    public DateTimeOffset? SuggestedEnd { get; set; }
    public LessonDto? Lesson { get; set; }
    public AddressDto? Address { get; set; }
    public StudentDto? Student { get; set; }
}
