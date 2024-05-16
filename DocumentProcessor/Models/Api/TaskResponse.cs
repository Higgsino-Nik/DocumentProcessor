namespace DocumentProcessor.Models.Api
{
    public class TaskResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public long PreviousTaskId { get; set; }
    }
}
