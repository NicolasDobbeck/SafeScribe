using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeScribe.Data;
using SafeScribe.DTOs;
using SafeScribe.DTOs.Response;
using SafeScribe.Models;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotasController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotasController(AppDbContext context)
    {
        _context = context;
    }

    // --- POST /api/v1/notas ---
    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Editor},{AppRoles.Admin}")]
    public async Task<IActionResult> CreateNote([FromBody] NoteCreateDto noteDto)
    {
        var userId = GetUserIdFromToken();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new { Message = "Token inválido ou não contém ID de usuário." });
        }

        var newNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = noteDto.Title,
            Content = noteDto.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _context.Notes.AddAsync(newNote);
        await _context.SaveChangesAsync();

        var responseDto = NoteResponseDto.FromNote(newNote);

        return CreatedAtAction(nameof(GetNoteById), new { id = responseDto.Id }, responseDto);
    }


    // --- GET /api/v1/notas/{id}  ---
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound(new { Message = "Nota não encontrada." });
        }


        var userId = GetUserIdFromToken();
        var userRole = GetUserRoleFromToken();

        if (userRole == AppRoles.Leitor || userRole == AppRoles.Editor)
        {
            if (note.UserId != userId)
            {
                return Forbid();
            }
        }

        var responseDto = NoteResponseDto.FromNote(note);
        return Ok(responseDto);
    }


    // --- PUT /api/v1/notas/{id} ---
    [HttpPut("{id}")]
    [Authorize(Roles = $"{AppRoles.Editor},{AppRoles.Admin}")]
    public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteUpdateDto updateDto)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound(new { Message = "Nota não encontrada." });
        }

        var userId = GetUserIdFromToken();
        var userRole = GetUserRoleFromToken();

        if (userRole == AppRoles.Editor && note.UserId != userId)
        {
            return Forbid();
        }

        note.Title = updateDto.Title;
        note.Content = updateDto.Content;

        _context.Notes.Update(note);
        await _context.SaveChangesAsync();

        return Ok(NoteResponseDto.FromNote(note));
    }


    // --- DELETE /api/v1/notas/{id} ---
    [HttpDelete("{id}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound(new { Message = "Nota não encontrada." });
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdClaim?.Value, out var userId))
        {
            return userId;
        }
        return Guid.Empty;
    }

    private string GetUserRoleFromToken()
    {
        var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return userRoleClaim?.Value ?? string.Empty;
    }
}