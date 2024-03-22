namespace JWTlesson1.API.Enums.Models
{
    public class PageDto
    {
        public required Guid Id { get; set; }
        public required string? Title { get; set; }
        public required string? Body { get; set; }
        public required string? Author { get; set; } 
    }
}
