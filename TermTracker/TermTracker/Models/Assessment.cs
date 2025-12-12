using SQLite;
using TermTracker.Models.Enums;

namespace TermTracker.Models;
public class Assessment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int CourseId { get; set; }
    [MaxLength(100), NotNull]
    public string Name { get; set; }
    [NotNull]
    public DateTime StartDate { get; set; }
    [NotNull]
    public DateTime EndDate { get; set; }
    [NotNull]
    public AssessmentType Type { get; set; }
}

