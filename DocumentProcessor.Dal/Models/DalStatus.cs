using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Dal.Models
{
    [Table("Status")]
    [PrimaryKey(nameof(Id))]
    public class DalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
