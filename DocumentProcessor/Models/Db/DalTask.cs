using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Models.Db
{
    [Table("Task")]
    [PrimaryKey(nameof(Id))]
    public class DalTask
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public long DocumentId { get; set; }
        [ForeignKey(nameof(DocumentId))]
        public DalDocument Document { get; set; }
        public long? PreviousTaskId { get; set; }
    }
}
