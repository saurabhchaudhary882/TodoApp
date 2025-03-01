using TodoApp.API.DTOs;
using TodoApp.Core.Commands;
using TodoApp.Core.Models;

namespace TodoApp.API.Mappings
{
    /// <summary>
    /// Mapping methods for converting between DTOs and domain models
    /// </summary>
    public static class TodoMappings
    {
        /// <summary>
        /// Maps a TodoItem domain model to a TodoItemDto
        /// </summary>
        public static TodoItemDto ToDto(this TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return null;
            }

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
                CreatedAt = todoItem.CreatedAt,
                UpdatedAt = todoItem.UpdatedAt,
                DueDate = todoItem.DueDate
            };
        }

        /// <summary>
        /// Maps a CreateTodoDto to a CreateTodoCommand
        /// </summary>
        public static CreateTodoCommand ToCommand(this CreateTodoDto dto)
        {
            return new CreateTodoCommand
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate
            };
        }

        /// <summary>
        /// Maps an UpdateTodoDto to an UpdateTodoCommand
        /// </summary>
        public static UpdateTodoCommand ToCommand(this UpdateTodoDto dto, Guid id)
        {
            return new UpdateTodoCommand
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate
            };
        }
    }
}
