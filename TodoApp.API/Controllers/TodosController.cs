using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.API.DTOs;
using TodoApp.API.Mappings;
using TodoApp.Core.Commands;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Queries;

namespace TodoApp.API.Controllers
{
    /// <summary>
    /// API controller for managing todo items
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IMediator mediator, ILogger<TodosController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns>List of all todo items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAll()
        {
            _logger.LogInformation("Getting all todo items");
            var todos = await _mediator.Send(new GetAllTodosQuery());
            return Ok(todos.Select(t => t.ToDto()));
        }

        /// <summary>
        /// Get a specific todo item by ID
        /// </summary>
        /// <param name="id">ID of the todo item</param>
        /// <returns>Todo item with the specified ID</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDto>> GetById(Guid id)
        {
            _logger.LogInformation("Getting todo item with ID: {Id}", id);

            try
            {
                var todo = await _mediator.Send(new GetTodoByIdQuery { Id = id });
                return Ok(todo.ToDto());
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo item not found");
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        /// <param name="dto">Data for creating the todo item</param>
        /// <returns>The created todo item</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoDto dto)
        {
            _logger.LogInformation("Creating new todo item");

            var command = dto.ToCommand();
            var createdTodo = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdTodo.Id },
                createdTodo.ToDto()
            );
        }

        /// <summary>
        /// Update an existing todo item
        /// </summary>
        /// <param name="id">ID of the todo item to update</param>
        /// <param name="dto">Data for updating the todo item</param>
        /// <returns>The updated todo item</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDto>> Update(Guid id, [FromBody] UpdateTodoDto dto)
        {
            _logger.LogInformation("Updating todo item with ID: {Id}", id);

            try
            {
                var command = dto.ToCommand(id);
                var updatedTodo = await _mediator.Send(command);

                return Ok(updatedTodo.ToDto());
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo item not found");
                return NotFound(new { message = ex.Message });
            }
            catch (TodoAppException ex)
            {
                _logger.LogError(ex, "Error updating todo item");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a todo item
        /// </summary>
        /// <param name="id">ID of the todo item to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting todo item with ID: {Id}", id);

            try
            {
                await _mediator.Send(new DeleteTodoCommand { Id = id });
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo item not found");
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark a todo item as completed
        /// </summary>
        /// <param name="id">ID of the todo item to mark as completed</param>
        /// <returns>The updated todo item</returns>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDto>> Complete(Guid id)
        {
            _logger.LogInformation("Marking todo item as completed, ID: {Id}", id);

            try
            {
                var updatedTodo = await _mediator.Send(new CompleteTodoCommand { Id = id });
                return Ok(updatedTodo.ToDto());
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo item not found");
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all completed todo items
        /// </summary>
        /// <returns>List of completed todo items</returns>
        [HttpGet("completed")]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetCompleted()
        {
            _logger.LogInformation("Getting completed todo items");

            var todos = await _mediator.Send(new GetCompletedTodosQuery());
            return Ok(todos.Select(t => t.ToDto()));
        }

        /// <summary>
        /// Get all incomplete todo items
        /// </summary>
        /// <returns>List of incomplete todo items</returns>
        [HttpGet("incomplete")]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetIncomplete()
        {
            _logger.LogInformation("Getting incomplete todo items");

            var todos = await _mediator.Send(new GetIncompleteTodosQuery());
            return Ok(todos.Select(t => t.ToDto()));
        }
    }
}