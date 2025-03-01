using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found in the repository
    /// </summary>
    public class EntityNotFoundException : TodoAppException
    {
        public EntityNotFoundException(string entityName, Guid id)
            : base($"{entityName} with ID {id} was not found.") { }
    }
}
