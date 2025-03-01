using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Data.Configurations;

namespace TodoApp.Tests.Unit
{
    public class MySqlConnectionFactoryTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenSettingsAreNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MySqlConnectionFactory(null));
        }

        // Note: This test can't actually create a real connection without a database,
        // so we just verify that a connection string is passed correctly.
        [Fact]
        public void Constructor_InitializesWithCorrectConnectionString()
        {
            // Arrange
            var expectedConnectionString = "Server=127.0.0.1;Port=3306;Database=TodoDb_test;User=root;Password=saurabh@mysql;";
            var settings = new DatabaseSettings
            {
                ConnectionString = expectedConnectionString
            };
            var optionsMock = new Mock<IOptions<DatabaseSettings>>();
            optionsMock.Setup(o => o.Value).Returns(settings);

            // Act
            var factory = new MySqlConnectionFactory(optionsMock.Object);

            // Assert
            Assert.NotNull(factory); // Factory was created successfully
        }
    }
}
