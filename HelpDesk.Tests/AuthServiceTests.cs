using HelpDesk.Data;
using HelpDesk.DTOs;
using HelpDesk.Entities;
using HelpDesk.Exceptions;
using HelpDesk.Services;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsDuplicateEmailException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        context.Users.Add(new User
        {
            Email = "cliente@test.com",
            PasswordHash = "already-hashed-password",
            Role = UserRole.Cliente
        });

        await context.SaveChangesAsync();

        var passwordService = new PasswordService();
        var authService = new AuthService(context, passwordService);

        var request = new RegisterRequest
        {
            Email = "cliente@test.com",
            Password = "Password123!",
            Role = UserRole.Cliente
        };

        await Assert.ThrowsAsync<DuplicateEmailException>(() => authService.RegisterAsync(request));
    }
}