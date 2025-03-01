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
    /// Command for marking a todo item as completed
    /// </summary>
    public class CompleteTodoCommand : IRequest<TodoItem>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the CompleteTodoCommand
        /// </summary>
        public class Handler : IRequestHandler<CompleteTodoCommand, TodoItem>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<TodoItem> Handle(CompleteTodoCommand request, CancellationToken cancellationToken)
            {
                // Mark as completed in repository
                var success = await _todoRepository.MarkAsCompletedAsync(request.Id);
                if (!success)
                {
                    throw new EntityNotFoundException(nameof(TodoItem), request.Id);
                }

                // Return updated todo item
                return await _todoRepository.GetByIdAsync(request.Id);
            }
        }
    }
}
