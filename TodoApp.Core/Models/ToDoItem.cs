using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Core.Models
{
    /// <summary>
    /// Represents a single todo item in the system
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// Unique identifier for the todo item
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the todo item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the todo item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Flag indicating if the todo item is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// When the todo item was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the todo item was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Optional due date for the todo item
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}

