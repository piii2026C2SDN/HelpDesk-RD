using HelpDesk.Data;
using HelpDesk.DTOs;
using HelpDesk.Entities;
using HelpDesk.Models;
using HelpDesk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    public AuthController(AppDbContext context, PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var emailExists = await _context.Users.AnyAsync(user => user.Email == request.Email);

        if (emailExists)
        {
            return Conflict("Email already exists.");
        }

        var user = new User
        {
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            Role = request.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created(string.Empty, new
        {
            user.Id,
            user.Email,
            Role = user.Role.ToString()
        });
    }
}