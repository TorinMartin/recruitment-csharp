using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;

namespace MusicApi.Controllers;

[ApiController]
[Route("v1/artists")]
public class ArtistController : BaseController
{
    private readonly ArtistService _artistService;
    
    public ArtistController(ArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _artistService.GetAsync();
        return await HandleServiceResult(result);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var result = await _artistService.GetAsync(id);
        return await HandleServiceResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddArtistAsync(string name)
    {
        if (string.IsNullOrEmpty(name)) return BadRequest("Missing required field: name");
        var result = await _artistService.CreateAsync(name);
        return await HandleServiceResult(result);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> DeleteArtistAsync(int id)
    {
        var result = await _artistService.DeleteAsync(id);
        return await HandleServiceResult(result);
    }

    [HttpPut("id")]
    public async Task<IActionResult> UpdateArtistAsync(int id, string name)
    {
        var result = await _artistService.UpdateAsync(id, name);
        return await HandleServiceResult(result);
    }
}