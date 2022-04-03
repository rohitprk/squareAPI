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
public class JWTRepositoryTest
{
    private readonly IDictionary<string, string> _inMemorySettings = new Dictionary<string, string> {
        {ApplicationConstant.SymmetricKeyConfig, "abcdefgh12345678"},
        {ApplicationConstant.JWTKeyConfig, "anyHashKey@456789"},
    };

    private IJWTRepository _jwtRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        string userName = "Test1";
        var mockUserRepo = new Mock<IUserService>();
        mockUserRepo.Setup(x => x.GetUser(userName)).Returns(Task.FromResult(GetUser_TestData()));

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemorySettings)
            .Build();

        _jwtRepository = new JWTRepository(configuration, mockUserRepo.Object);

    }
    
    [TestMethod]
    public async Task AuthenticateTest()
    {
        var users = new Users
        {
            Name = "Test1",
            Password = ("Test123").AESEncrypt(_inMemorySettings[ApplicationConstant.SymmetricKeyConfig])
        };

        string token = await _jwtRepository.Authenticate(users);
        Assert.IsTrue(!string.IsNullOrEmpty(token));
    }

    [TestMethod]
    public async Task AuthenticateTestFail()
    {
        var users = new Users
        {
            Name = "Test1",
            Password = ("Test1234").AESEncrypt(_inMemorySettings[ApplicationConstant.SymmetricKeyConfig])
        };

        string token = await _jwtRepository.Authenticate(users);
        Assert.IsTrue(string.IsNullOrEmpty(token));
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