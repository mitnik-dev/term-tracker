using SQLite;
using TermTracker.Models.Enums;

namespace TermTracker.Models;

public class Term
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100), NotNull]
    public string Name { get; set; }
    [NotNull]
    public DateTime StartDate { get; set; }
    [NotNull]
    public DateTime EndDate { get; set; }
    [Ignore]
    public StatusType Status
    {
        get
        {
            if (DateTime.Today < StartDate)
                return StatusType.Future;
            if (DateTime.Today > EndDate)
                return StatusType.Completed;
            return StatusType.Active;
        }
    }
    [Ignore]
    public List<Course> Courses { get; set; } = new();
}
