using DocumentProcessor.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DocumentProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController(Repository _repository) : ControllerBase
    {
        [HttpPost("{documentId}")]
        public async Task<IActionResult> Create(long documentId, string taskName)
        {
            await _repository.AddTaskAsync(documentId, taskName);
            return Ok();
        }

        [HttpPut("Complete/{id}")]
        public async Task<IActionResult> Complete(long id)
        {
            var isInProgress = await IsInProgress(id);
            if (!isInProgress)
                return BadRequest($"Task with {id} is not in progress");

            var document = await _repository.GetDocumentByTaskIdAsync(id);
            var nextTask = document.Tasks.FirstOrDefault(x => x.PreviousTaskId == id);
            var updateCurrentTask = _repository.ChangeTaskStatusAsync(id, Status.Finished);
            var updateNextTask = nextTask is null
                ? _repository.ChangeDocumentStatusAsync(document.Id, Status.Finished)
                : _repository.ChangeTaskStatusAsync(nextTask.Id, Status.InProgress);
            await Task.WhenAll(updateCurrentTask, updateNextTask);
            return Ok();
        }

        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> Cancel(long id)
        {
            var isInProgress = await IsInProgress(id);
            if (!isInProgress)
                return BadRequest($"Task with {id} is not in progress");

            var document = await _repository.GetDocumentByTaskIdAsync(id);
            List<Task> updateTasks = [];
            updateTasks.Add(_repository.ChangeDocumentStatusAsync(document.Id, Status.Rejected));
            
            foreach (var task in document.Tasks.Where(x => x.Status != Status.Finished))
            {
                updateTasks.Add(_repository.ChangeTaskStatusAsync(task.Id, Status.Rejected));
            }
            await Task.WhenAll(updateTasks);
            return Ok();
        }

        private async Task<bool> IsInProgress(long taskId)
        {
            var task = await _repository.GetTaskAsync(taskId);
            return task.Status == Status.InProgress;
        }
    }
}
