using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;

namespace MusicApi.Controllers;

[ApiController]
[Route("v1/songs")]
public class SongController : BaseController
{
    private readonly SongService _songService;

    public SongController(SongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _songService.GetAsync();
        return await HandleServiceResult(result);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var result = await _songService.GetAsync(id);
        return await HandleServiceResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddSongAsync(int track, string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest("Missing required field: name");
        var result = await _songService.CreateAsync(track, name);
        return await HandleServiceResult(result);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeleteSongAsync(int id)
    {
        var result = await _songService.DeleteAsync(id);
        return await HandleServiceResult(result);
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdateSongAsync(int id, int track, string name)
    {
        var result = await _songService.UpdateAsync(id, track, name);
        return await HandleServiceResult(result);
    }
}