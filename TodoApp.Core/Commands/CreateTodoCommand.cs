using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;

namespace TodoApp.Core.Commands
{
    // <summary>
    /// Command for creating a new todo item
    /// </summary>
    public class CreateTodoCommand : IRequest<TodoItem>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Handler for the CreateTodoCommand
        /// </summary>
        public class Handler : IRequestHandler<CreateTodoCommand, TodoItem>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<TodoItem> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
            {
                // Create a new todo item from the command parameters
                var todoItem = new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow,
                    DueDate = request.DueDate
                };

                // Add to repository and return the created item
                return await _todoRepository.AddAsync(todoItem);
            }
        }
    }
    }
