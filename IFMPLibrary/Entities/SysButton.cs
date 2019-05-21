using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IFMPLibrary.Entities
{
    [Table("Tb_SysButton")]
    public class SysButton
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string BCode { get; set; }
    }
}
