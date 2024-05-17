namespace DocumentProcessor.Models.Api
{
    public class DocumentResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public TaskResponse ActiveTask { get; set; }
        public List<TaskResponse> Tasks { get; set; }
    }
}
