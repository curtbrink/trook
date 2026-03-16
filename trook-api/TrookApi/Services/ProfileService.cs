using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;

namespace TrookApi.Services;

public class ProfileService(TrookDbContext db, ILogger<ProfileService> logger)
{
    public async Task<List<Profile>> GetAllProfiles()
    {
        return await db.Profiles.ToListAsync();
    }
    
    public async Task<Profile> CreateProfile(ProfileCreateRequest request)
    {
        logger.LogInformation("Creating new profile \"{Name}\"...", request.Name);
        var newProfile = new Profile
        {
            Name = request.Name
        };

        var entity = await db.AddAsync(newProfile);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully created profile");

        return entity.Entity;
    }
}