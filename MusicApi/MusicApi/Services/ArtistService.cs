﻿using MusicApi.Controllers;
using MusicApi.Data;
using MusicApi.Model;

namespace MusicApi.Services;

public interface IArtistService
{
    public Task<ServiceResult<CountResult>> CreateAsync(ArtistCreationRequest request);
    public Task<ServiceResult<CountResult>> UpdateAsync(ArtistUpdateRequest request);
}

public class ArtistService : BaseService<Artist>, IArtistService
{
    public ArtistService(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<ServiceResult<CountResult>> CreateAsync(ArtistCreationRequest request)
    {
        if (string.IsNullOrEmpty(request.Name)) return await HandleServiceError<CountResult>("Missing required field: name");
        
        var artist = new Artist { Name = request.Name };
        return await InsertAsync(artist);
    }

    public async Task<ServiceResult<CountResult>> UpdateAsync(ArtistUpdateRequest request)
    {
        return await UpdateAsync(request.Id, artist =>
        {
            artist.Name = request.Name;
        });
    }
}