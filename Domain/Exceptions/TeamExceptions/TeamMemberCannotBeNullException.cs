namespace Exceptions;

public class TeamMemberCannotBeNullException : Exception
{
    public TeamMemberCannotBeNullException(String ? message) : base(message)
    {
    }
}