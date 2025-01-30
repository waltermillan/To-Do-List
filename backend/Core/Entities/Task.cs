using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Core.Entities
{
    [Table("Task")]
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
}
