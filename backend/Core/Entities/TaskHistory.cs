using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    [Table("TaskHistory")]
    public class TaskHistory : BaseEntity
    {
        [Column("task_id")]
        public int TaskId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("changed_date")]
        public DateTime ChangedDate { get; set; }

    }
}
