using BusinessLogic.DTOs;
using Domain;
using Task = Domain.Task;

namespace BusinessLogic;

using Memory;
public class CommentLogic
{
    private readonly PanelMemory _panelMemory;
    private readonly UserMemory _userMemory;
    
    public CommentLogic(PanelMemory panelMemory, UserMemory userMemory)
    {
        _panelMemory = panelMemory;
        _userMemory = userMemory;
    }

    public void CreateComment(CommentDto? commentDto, int taskId, int panelId)
    {
        ValidateComment(commentDto);
        Comment comment = new Comment(commentDto.AuthorDto!.ToUser(), commentDto.Content!);
        Panel commentPanel = _panelMemory.GetPanelById(panelId);
        Task commentTask = commentPanel.Tasks.Find(t => t.Id == taskId)!;
        commentTask.AddComment(comment);
    }
    
    public CommentDto GetCommentById(int commentId, int taskId, int panelId)
    {
        Panel commentPanel = _panelMemory.GetPanelById(panelId);
        Task commentTask = commentPanel.Tasks.Find(t => t.Id == taskId)!;
        Comment comment = commentTask.Comments.Find(c => c.Id == commentId)!;
        return CommentDto.FromComment(comment);
    }
    
    public void SolveComment(int commentId, int userId, int taskId, int panelId)
    {
        Panel commentPanel = _panelMemory.GetPanelById(panelId);
        Task commentTask = commentPanel.Tasks.Find(t => t.Id == taskId)!;
        Comment comment = commentTask.Comments.Find(c => c.Id == commentId)!;
        User resolver = _userMemory.GetUserById(userId);
        comment.Solve();
        comment.SetResolver(resolver);
    }
    
    private static void ValidateComment(CommentDto? commentDto)
    {
        if (string.IsNullOrWhiteSpace(commentDto.Content))
        {
            throw new ArgumentException("Comment content cannot be empty");
        }
    }
}