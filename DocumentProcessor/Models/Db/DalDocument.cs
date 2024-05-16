using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Models.Db
{
    [Table("Document")]
    [PrimaryKey(nameof(Id))]
    public class DalDocument
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<DalTask> Tasks { get => _tasks ?? []; set => _tasks = value; }
        
        private ICollection<DalTask> _tasks;
    }
}
