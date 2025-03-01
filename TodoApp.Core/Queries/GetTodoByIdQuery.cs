using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;

namespace TodoApp.Core.Queries
{
    /// <summary>
    /// Query for retrieving a specific todo item by ID
    /// </summary>
    public class GetTodoByIdQuery : IRequest<TodoItem>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Handler for the GetTodoByIdQuery
        /// </summary>
        public class Handler : IRequestHandler<GetTodoByIdQuery, TodoItem>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<TodoItem> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
            {
                var todoItem = await _todoRepository.GetByIdAsync(request.Id);
                if (todoItem == null)
                {
                    throw new EntityNotFoundException(nameof(TodoItem), request.Id);
                }

                return todoItem;
            }
        }
    }
}
