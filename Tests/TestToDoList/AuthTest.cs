using WebAPITodoList.Repositories.Interfaces;
using Moq;
using WebAPITodoList.Controllers;
using WebAPITodoList.Services;
using WebAPITodoList.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestToDoList;

public class AuthTest
{
    [Fact]
    public void Register_UserTest()
    {
        // Arrange  
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(repo => repo.RegisterUserUserAsync(It.IsAny<RegisterUserDto>()))
            .ReturnsAsync(true); // Simula il successo della registrazione

        var tokenServiceMock = new Mock<ITokenService>();

        var controller = new AuthController(userRepositoryMock.Object, tokenServiceMock.Object);

        // Act  
        var res = controller.Register(new RegisterUserDto
        {
            FirstName = "Test", // Fix for CS9035: Required member 'RegisterUserDto.FirstName'  
            LastName = "User",  // Fix for CS9035: Required member 'RegisterUserDto.LastName'  
            BirthDate = new DateTime(1990, 1, 1), // Fix for CS9035: Required member 'RegisterUserDto.BirthDate'  
            UserName = "testuser",
            Password = "Test1234!",
            Email = "papa@william.com",
        }).Result;

        // Assert   
        userRepositoryMock.Verify(repo => repo.RegisterUserUserAsync(It.IsAny<RegisterUserDto>()), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(res);
        Assert.Equal("Registrazione completata", okResult.Value);
    }
}