using Dapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Models;
using TodoApp.Infrastructure.Data.Configurations.Interfaces;

namespace TodoApp.Tests.Unit
{
    public class TodoRepositoryTests
    {
        private readonly Mock<IDatabaseConnectionFactory> _mockConnectionFactory;
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDapperWrapper> _mockDapperWrapper;

        public TodoRepositoryTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockConnectionFactory = new Mock<IDatabaseConnectionFactory>();
            _mockDapperWrapper = new Mock<IDapperWrapper>();

            _mockConnectionFactory.Setup(f => f.CreateConnectionAsync()).ReturnsAsync(_mockConnection.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Title = "Todo 1" },
                new TodoItem { Id = Guid.NewGuid(), Title = "Todo 2" }
            };

            _mockDapperWrapper.Setup(d => d.QueryAsync<TodoItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    null))
                .ReturnsAsync(expectedTodos);

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedTodos[0].Title, result.First().Title);
            Assert.Equal(expectedTodos[1].Title, result.Last().Title);

            _mockDapperWrapper.Verify(d => d.QueryAsync<TodoItem>(
                _mockConnection.Object,
                It.IsAny<string>(),
                null), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTodo_WhenTodoExists()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var expectedTodo = new TodoItem { Id = todoId, Title = "Test Todo" };

            _mockDapperWrapper.Setup(d => d.QueryFirstOrDefaultAsync<TodoItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(expectedTodo);

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.GetByIdAsync(todoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoId, result.Id);
            Assert.Equal("Test Todo", result.Title);

            _mockDapperWrapper.Verify(d => d.QueryFirstOrDefaultAsync<TodoItem>(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.Is<object>(p => true)), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();

            _mockDapperWrapper.Setup(d => d.QueryFirstOrDefaultAsync<TodoItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync((TodoItem)null);

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.GetByIdAsync(todoId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldInsertTodoAndReturnIt()
        {
            // Arrange
            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "New Todo",
                Description = "New Todo Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                IsCompleted = false
            };

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(1); // 1 row affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.AddAsync(todo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todo.Id, result.Id);
            Assert.Equal(todo.Title, result.Title);
            Assert.Equal(todo.Description, result.Description);

            _mockDapperWrapper.Verify(d => d.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.Is<TodoItem>(t => t.Id == todo.Id && t.Title == todo.Title)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "Updated Todo",
                Description = "Updated Description",
                IsCompleted = true
            };

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(1); // 1 row affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.UpdateAsync(todo);

            // Assert
            Assert.True(result);
            _mockDapperWrapper.Verify(d => d.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.Is<TodoItem>(t => t.Id == todo.Id && t.Title == todo.Title)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
        {
            // Arrange
            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "Updated Todo"
            };

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(0); // 0 rows affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.UpdateAsync(todo);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenDeleteSucceeds()
        {
            // Arrange
            var todoId = Guid.NewGuid();

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(1); // 1 row affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.DeleteAsync(todoId);

            // Assert
            Assert.True(result);
            _mockDapperWrapper.Verify(d => d.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(0); // 0 rows affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.DeleteAsync(todoId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetCompletedAsync_ShouldReturnCompletedTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Title = "Completed Todo 1", IsCompleted = true },
                new TodoItem { Id = Guid.NewGuid(), Title = "Completed Todo 2", IsCompleted = true }
            };

            _mockDapperWrapper.Setup(d => d.QueryAsync<TodoItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    null))
                .ReturnsAsync(expectedTodos);

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.GetCompletedAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.All(t => t.IsCompleted));
        }

        [Fact]
        public async Task GetIncompleteAsync_ShouldReturnIncompleteTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid(), Title = "Incomplete Todo 1", IsCompleted = false },
                new TodoItem { Id = Guid.NewGuid(), Title = "Incomplete Todo 2", IsCompleted = false }
            };

            _mockDapperWrapper.Setup(d => d.QueryAsync<TodoItem>(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    null))
                .ReturnsAsync(expectedTodos);

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.GetIncompleteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.All(t => !t.IsCompleted));
        }

        [Fact]
        public async Task MarkAsCompletedAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var todoId = Guid.NewGuid();

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(1); // 1 row affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.MarkAsCompletedAsync(todoId);

            // Assert
            Assert.True(result);
            _mockDapperWrapper.Verify(d => d.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.Is<object>(p => true)), Times.Once);
        }

        [Fact]
        public async Task MarkAsIncompleteAsync_ShouldReturnTrue_WhenUpdateSucceeds()
        {
            // Arrange
            var todoId = Guid.NewGuid();

            _mockDapperWrapper.Setup(d => d.ExecuteAsync(
                    _mockConnection.Object,
                    It.IsAny<string>(),
                    It.IsAny<object>()))
                .ReturnsAsync(1); // 1 row affected

            var repository = new TodoRepository(_mockConnectionFactory.Object, _mockDapperWrapper.Object);

            // Act
            var result = await repository.MarkAsIncompleteAsync(todoId);

            // Assert
            Assert.True(result);
            _mockDapperWrapper.Verify(d => d.ExecuteAsync(
                _mockConnection.Object,
                It.IsAny<string>(),
                It.Is<object>(p => true)), Times.Once);
        }
    }
}
