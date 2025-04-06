using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}
