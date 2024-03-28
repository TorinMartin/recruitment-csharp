using MusicApi.Data;
using MusicApi.Model;

namespace MusicApi.Services;

public interface ISongService
{
    public Task<ServiceResult<CountResult>> CreateAsync(int track, string name);
    public Task<ServiceResult<CountResult>> UpdateAsync(int id, int track, string name);
}

// Generic base service allows individual services to reuse very little code
public class SongService : BaseService<Song>, ISongService
{
    public SongService(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<ServiceResult<CountResult>> CreateAsync(int track, string name)
    {
        var song = new Song { Track = track, Name = name };
        return await InsertAsync(song);
    }

    public async Task<ServiceResult<CountResult>> UpdateAsync(int id, int track, string name)
    {
        return await UpdateAsync(id, song =>
        {
            song.Track = track;
            song.Name = name;
        });
    }
}