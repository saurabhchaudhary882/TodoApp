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
    /// Query for retrieving incomplete todo items
    /// </summary>
    public class GetIncompleteTodosQuery : IRequest<IEnumerable<TodoItem>>
    {
        /// <summary>
        /// Handler for the GetIncompleteTodosQuery
        /// </summary>
        public class Handler : IRequestHandler<GetIncompleteTodosQuery, IEnumerable<TodoItem>>
        {
            private readonly ITodoRepository _todoRepository;

            public Handler(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
            }

            public async Task<IEnumerable<TodoItem>> Handle(GetIncompleteTodosQuery request, CancellationToken cancellationToken)
            {
                return await _todoRepository.GetIncompleteAsync();
            }
        }
    }
}
