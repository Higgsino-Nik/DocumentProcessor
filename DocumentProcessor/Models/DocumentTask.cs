using DocumentProcessor.Enums;

namespace DocumentProcessor.Models
{
    public class DocumentTask
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public long PreviousTaskId { get; set; }
    }
}
