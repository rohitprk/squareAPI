using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SquareAPI.Business.Constants;
using SquareAPI.Data;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business.Test;

[TestClass]
public class UserServiceTest
{
   private readonly IDictionary<string, string> _inMemorySettings = new Dictionary<string, string> {
        {ApplicationConstant.SymmetricKeyConfig, "abcdefgh12345678"},
        {ApplicationConstant.JWTKeyConfig, "anyHashKey"},
    };

    private IUserService _userService;

    [TestInitialize]
    public void TestInitialize()
    {
        string userName = "Test1";
        var mockUserRepo = new Mock<IUserRepository>();
         mockUserRepo.Setup(x => x.GetUser(userName)).Returns(Task.FromResult(GetUser_TestData()));
        mockUserRepo.Setup(x => x.RegisterUser(GetUser_TestData())).Returns(Task.FromResult(true));

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings)
            .Build();

        _userService = new UserService(mockUserRepo.Object, configuration);

    }

    [TestMethod]
    public async Task GetUserTest()
    {
        string userName = "Test1";
        var userData = await _userService.GetUser(userName);
        Assert.IsNotNull(userData);
        Assert.IsInstanceOfType(userData, typeof(Users));
        Assert.IsTrue(userData.UserId > 0);
        Assert.IsNotNull(userData.Password);
    }

    [TestMethod]
    public async Task RegisterTest()
    {
        string password = "Test123";
        var newUser = new Users
        {
            Name = "Test12",
            Password = password.AESEncrypt(_inMemorySettings[ApplicationConstant.SymmetricKeyConfig])
        };

        var isUserRegistered = await _userService.Register(newUser);
        Assert.IsTrue(isUserRegistered);
        Assert.AreEqual(password.GenerateHash(_inMemorySettings[ApplicationConstant.JWTKeyConfig]), newUser.Password);
    }

    [TestMethod]
    public async Task RegisterTestFail()
    {
        string password = "Test123";
        var newUser = new Users
        {
            Name = "Test1",
            Password = password.AESEncrypt(_inMemorySettings[ApplicationConstant.SymmetricKeyConfig])
        };

        var isUserRegistered = await _userService.Register(newUser);
        Assert.IsFalse(isUserRegistered);
    }

    private Users GetUser_TestData()
    {
        return new Users
        {
            Name = "Test1",
            Password = ("Test123").GenerateHash(_inMemorySettings[ApplicationConstant.JWTKeyConfig]),
            UserId = 1
        };
    }
}