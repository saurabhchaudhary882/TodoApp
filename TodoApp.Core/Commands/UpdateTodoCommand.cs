using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;

namespace TodoApp.Core.Commands
{
    /// <summary>
    /// Command for updating an existing todo item
    /// </summary>
    public class UpdateTodoCommand : IRequest<TodoItem>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Handler for the UpdateTodoCommand
        /// </summary>
        public class Handler : IRequestHandler<UpdateTodoCommand, TodoItem>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<TodoItem> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
            {
                // Get existing todo item
                var todoItem = await _todoRepository.GetByIdAsync(request.Id);
                if (todoItem == null)
                {
                    throw new EntityNotFoundException(nameof(TodoItem), request.Id);
                }

                // Update properties
                if (!string.IsNullOrWhiteSpace(request.Title))
                {
                    todoItem.Title = request.Title;
                }

                if (!string.IsNullOrWhiteSpace(request.Description))
                {
                    todoItem.Description = request.Description;
                }

                if (request.DueDate.HasValue)
                {
                    todoItem.DueDate = request.DueDate.Value;
                }
                todoItem.UpdatedAt = DateTime.UtcNow;

                // Update in repository
                var success = await _todoRepository.UpdateAsync(todoItem);
                if (!success)
                {
                    throw new TodoAppException($"Failed to update todo item with ID {request.Id}");
                }

                return todoItem;
            }
        }
    }
}
