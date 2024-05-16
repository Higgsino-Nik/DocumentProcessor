using AutoMapper;
using DocumentProcessor.Enums;
using DocumentProcessor.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace DocumentProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController(Repository _repository, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            await _repository.AddDocumentAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var documents = await _repository.GetDocumentsAsync();
            return Ok(documents.Select(_mapper.Map<DocumentResponse>));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var document = await _repository.GetDocumentAsync(id);
            return Ok(_mapper.Map<DocumentResponse>(document));
        }

        [HttpPut("StartWork/{id}")]
        public async Task<IActionResult> StartWork(long id)
        {
            var document = await _repository.GetDocumentAsync(id);
            if (document.Tasks.Count == 0)
                return BadRequest($"Document with {id} doesn't have any tasks");
            if (document.Status != Status.NotStarted)
                return BadRequest($"Document with {id} is already in work");

            var changeDocumentStatusTask = _repository.ChangeDocumentStatusAsync(id, Status.InProgress);
            var changetaskStatusTask = _repository.ChangeTaskStatusAsync(document.Tasks.First().Id, Status.InProgress);
            await Task.WhenAll(changeDocumentStatusTask, changetaskStatusTask);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _repository.MarkDocumentAsDeletedAsync(id);
            return Ok();
        }
    }
}
