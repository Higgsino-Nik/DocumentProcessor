using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Dal.Models
{
    [Table("Document")]
    [PrimaryKey(nameof(Id))]
    public class DalDocument
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public DalStatus Status { get; set; }
        public List<DalTask> Tasks { get; set; }
    }
}
