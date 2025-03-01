using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;

namespace TodoApp.Core.Queries
{
    /// <summary>
    /// Query for retrieving completed todo items
    /// </summary>
    public class GetCompletedTodosQuery : IRequest<IEnumerable<TodoItem>>
    {
        /// <summary>
        /// Handler for the GetCompletedTodosQuery
        /// </summary>
        public class Handler : IRequestHandler<GetCompletedTodosQuery, IEnumerable<TodoItem>>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<IEnumerable<TodoItem>> Handle(GetCompletedTodosQuery request, CancellationToken cancellationToken)
            {
                return await _todoRepository.GetCompletedAsync();
            }
        }
    }
}
