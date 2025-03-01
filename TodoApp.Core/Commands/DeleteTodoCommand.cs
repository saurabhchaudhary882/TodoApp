using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces;

namespace TodoApp.Core.Commands
{
    /// <summary>
    /// Command for deleting a todo item
    /// </summary>
    public class DeleteTodoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the DeleteTodoCommand
        /// </summary>
        public class Handler : IRequestHandler<DeleteTodoCommand, bool>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
            {
                // Check if todo item exists
                var todoItem = await _todoRepository.GetByIdAsync(request.Id);
                if (todoItem == null)
                {
                    throw new EntityNotFoundException(nameof(Models.TodoItem), request.Id);
                }

                // Delete from repository
                return await _todoRepository.DeleteAsync(request.Id);
            }
        }
    }
}
