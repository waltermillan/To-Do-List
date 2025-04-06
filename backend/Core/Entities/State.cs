using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("States")]
public class State : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }
}
