using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Core.Exceptions
{
    /// <summary>
    /// Base exception class for application-specific exceptions
    /// </summary>
    public class TodoAppException : Exception
    {
        public TodoAppException() { }

        public TodoAppException(string message) : base(message) { }

        public TodoAppException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
