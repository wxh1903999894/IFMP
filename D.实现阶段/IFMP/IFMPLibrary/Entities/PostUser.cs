using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Enums;

namespace IFMPLibrary.Entities
{
    [Table("Tb_Post_User")]
    public class PostUser
    {
        [Key]
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }

    }
}
