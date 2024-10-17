using Domain;

namespace BusinessLogic.DTOs;
    
public class CommentDto
{
    public int Id { get; private init; }
    public string? Content { get; set; }
    public UserDto? AuthorDto { get; set; }
    public DateTime DateAndTime { get; set; }
    public DateTime? ResolutionDateTime { get; init; }
    public bool Resolved { get; init; }
    public UserDto? ResolverDto { get; private set; }

    public static CommentDto FromComment(Comment comment)
    {
        CommentDto dto = new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            AuthorDto = UserDto.FromUser(comment.Author),
            DateAndTime = comment.DateAndTime,
            ResolutionDateTime = comment.ResolutionDateTime,
            Resolved = comment.Resolved
        };
        if (comment.Resolver is null)
        {
            dto.ResolverDto = null;
        }
        else
        {
            dto.ResolverDto = UserDto.FromUser(comment.Resolver);
        }
        return dto;
    }
    
    public Comment ToComment()
    {
        Comment comment = new Comment(AuthorDto!.ToUser(), Content!)
        {
            Id = Id,
            DateAndTime = DateAndTime,
            Resolved = Resolved,
            ResolutionDateTime = ResolutionDateTime
        };
        if (ResolverDto is null)
        {
            comment.Resolver = null;
        }
        else
        {
            comment.Resolver = ResolverDto.ToUser();
        }
        
        return comment;
    }
}