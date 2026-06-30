using HelpDesk.Data;
using HelpDesk.DTOs;
using HelpDesk.Entities;
using HelpDesk.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    public AuthService(AppDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    public async Task<User> RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _context.Users.AnyAsync(user => user.Email == request.Email);

        if (emailExists)
        {
            throw new DuplicateEmailException(request.Email);
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            Role = request.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}