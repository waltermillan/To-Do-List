using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("TasksHistory")]
public class TaskHistory : BaseEntity
{
    [Column("task_id")]
    public int TaskId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("changed_date")]
    public DateTime ChangedDate { get; set; }

}
