﻿using MusicApi.Data;
using MusicApi.Model;

namespace MusicApi.Services;

public interface IArtistService
{
    public Task<ServiceResult<CountResult>> CreateAsync(string name);
    public Task<ServiceResult<CountResult>> UpdateAsync(int id, string name);
}

public class ArtistService : BaseService<Artist>, IArtistService
{
    public ArtistService(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<ServiceResult<CountResult>> CreateAsync(string name)
    {
        var artist = new Artist { Name = name };
        return await InsertAsync(artist);
    }

    public async Task<ServiceResult<CountResult>> UpdateAsync(int id, string name)
    {
        return await UpdateAsync(id, artist =>
        {
            artist.Name = name;
        });
    }
}