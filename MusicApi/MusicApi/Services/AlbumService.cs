using Microsoft.EntityFrameworkCore;
using MusicApi.Controllers;
using MusicApi.Data;
using MusicApi.Model;

namespace MusicApi.Services;

public interface IAlbumService
{
    public Task<ServiceResult<CountResult>> CreateAsync(AlbumCreationRequest request);
}

public class AlbumService : BaseService<Album>, IAlbumService
{
    private readonly DatabaseContext _dbContext;
    
    public AlbumService(DatabaseContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ServiceResult<CountResult>> CreateAsync(AlbumCreationRequest request)
    {
        var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.Id == request.ArtistId);
        if (artist is null) return await HandleServiceError<CountResult>("Invalid artist id");

        var album = new Album { Name = request.Name, YearReleased = request.YearReleased, Artist = artist };
        return await InsertAsync(album);
    }   
}