using DocumentProcessor.Enums;

namespace DocumentProcessor.Models
{
    public class Document
    {
        public long Id { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public List<DocumentTask> Tasks { get => _tasks; set => _tasks = [.. value.OrderBy(x => x.Id)]; }

        private List<DocumentTask> _tasks;
    }
}
