using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    [Table("State")]
    public class State : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
    }
}
