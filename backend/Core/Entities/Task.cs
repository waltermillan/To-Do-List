using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("Tasks")]
public class Task : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("initial_date")]
    public DateTime InitialDate { get; set; }
    [Column("finish_date")]
    public DateTime FinishDate { get; set; }
    [Column("done")]
    public bool? Done { get; set; }
}
