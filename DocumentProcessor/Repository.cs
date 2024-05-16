using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocumentProcessor.Configuration;
using DocumentProcessor.Enums;
using DocumentProcessor.Exceptions;
using DocumentProcessor.Models;
using DocumentProcessor.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor
{
    public class Repository(IMapper _mapper, AppDbContext _context)
    {
        public async Task AddDocumentAsync()
        {
            var document = new DalDocument
            {
                StatusId = (int)Status.NotStarted
            };
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }

        public async Task<Document> GetDocumentAsync(long id)
        {
            var document = await _context.Documents
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == id);
            return document is null
                ? throw new ItemNotFoundException($"Document with {id} id does not exist or was deleted")
                : _mapper.Map<Document>(document);
        }

        public async Task<Document> GetDocumentByTaskIdAsync(long taskId)
        {
            var document = await _context.Documents.Include(x => x.Tasks).SingleAsync(doc => doc.Tasks.Select(task => task.Id).Contains(taskId));
            return _mapper.Map<Document>(document);
        }

        public async Task<List<Document>> GetDocumentsAsync() =>
            await _context.Documents.Where(x => !x.IsDeleted).ProjectTo<Document>(_mapper.ConfigurationProvider).ToListAsync();

        public async Task<DocumentTask> GetTaskAsync(long id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new ItemNotFoundException($"Task with {id} id does not exist"); ;
            return _mapper.Map<DocumentTask>(task);
        }

        public async Task AddTaskAsync(long documentId, string taskName)
        {
            var document = await _context.Documents.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted)
                ?? throw new ItemNotFoundException($"Document with {documentId} id does not exist or was deleted");
            var lastTask = document.Tasks.LastOrDefault();
            var newTask = new DalTask
            {
                DocumentId = documentId,
                Name = taskName,
                StatusId = (int)Status.NotStarted,
                PreviousTaskId = lastTask?.Id
            };
            
            _context.Add(newTask);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeTaskStatusAsync(long taskId, Status status)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId)
                ?? throw new ItemNotFoundException($"Task with {taskId} id does not exist");
            task.StatusId = (int)status;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeDocumentStatusAsync(long documentId, Status status)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted)
                ?? throw new ItemNotFoundException($"Document with {documentId} id does not exist or was deleted");
            document.StatusId = (int)status;
            await _context.SaveChangesAsync();
        }

        public async Task MarkDocumentAsDeletedAsync(long documentId)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted)
                ?? throw new ItemNotFoundException($"Document with {documentId} id does not exist or was deleted");
            document.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
