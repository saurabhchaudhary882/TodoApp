using System.ComponentModel.DataAnnotations;

namespace TodoApp.API.DTOs
{
    /// <summary>
    /// Data transfer object for updating an existing todo item
    /// </summary>
    public class UpdateTodoDto
    {
        /// <summary>
        /// Title of the todo item (required)
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>
        /// Optional description of the todo item
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Optional due date for the todo item
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
