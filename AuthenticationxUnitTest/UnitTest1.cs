using AuthenticationAPIApp.Controllers;
using AuthenticationAPIApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthenticationxUnitTest
{
    public class UnitTest1
    {
        #region Property  


        private AuthController _authController;
        private Mock<ILogger<AuthController>> _logger;
        //public Mock<IConfiguration> mockConfiguration;

        #endregion
        public UnitTest1()
        {
            var myConfiguration = new Dictionary<string, string> { { "Jwt:Key", "This is my token admin" }, { "Jwt:Issuer", "issuer" }, { "Jwt:Audience", "audience" } };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            _logger = new Mock<ILogger<AuthController>>();

            _authController = new AuthController(_logger.Object, configuration);

        }

        [Fact]
        public void Login_Return_OkResult()
        {
            //Arrange  
            var loginMode = new Login { UserName = "user1", Password = "password1" };
            //Act
            IActionResult result = _authController.Login(loginMode);
            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void Login_Return_UnauthorizedObjectResult()
        {
            //Arrange  
            var loginMode = new Login { UserName = "username1", Password = "password1" };
            //Act           
            var result = _authController.Login(loginMode) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result.Value);
            Assert.Equal("The user is not found in the list.", result.Value);

        }

    }
}