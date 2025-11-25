using SQLite;
using TermTracker.Models.Enums;

namespace TermTracker.Models;

public class Note
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100), NotNull]
    public string Title { get; set; }
    [NotNull]
    public string Content { get; set; }
    [NotNull, Indexed]
    public int CourseId { get; set; }
    [Ignore]
    public NoteType Type { get; set; }
}
