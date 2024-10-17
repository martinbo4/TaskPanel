using Domain.Exceptions.CommentExceptions;

namespace Domain;

public class Comment : IComparable<Comment>
{
    public User Author { get; }
    public string Content { get; }
    public DateTime DateAndTime { get; init; }
    public bool Resolved { get; set; }
    public User? Resolver { get; set; }
    public DateTime? ResolutionDateTime { get; set; }
    
    public Comment(User author, string content)
    {
        Author = author;
        Content = content;
        DateAndTime = DateTime.Now;
        Resolved = false;
        Id = _idCounter++;
    }
    
    public int Id { get; init; }
    private static int _idCounter = 1;

    public void Solve()
    {
        Resolved = true;
        ResolutionDateTime = DateTime.Now;
    }
    
    public void SetResolver(User resolver)
    {
        if (ResolverIsNull(resolver))
        {
            throw new CommentResolverIsNullException("Resolver cannot be null");
        }
        Resolver = resolver;
    }

    private static bool ResolverIsNull(User resolver)
    {
        return resolver == null;
    }

    public int CompareTo(Comment? other)
    {
        if(other == null) return 1;
        return DateAndTime.CompareTo(other.DateAndTime);
    }
}