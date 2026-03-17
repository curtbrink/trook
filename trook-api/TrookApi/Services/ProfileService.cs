using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;
using TrookSii.Types.Raw;

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

    public async Task<Profile> CreateProfileFromSii(SiiFile file)
    {
        logger.LogInformation("Creating new profile from sii file");
        // how to find name in this file??
        if (file is not SiiTextFile stf || stf.Data.Values.Count == 0)
            throw new InvalidOperationException("Given sii file is not a text file");

        var onlyBlock = stf.Data.Values.First();
        if (onlyBlock.StructureName != "user_profile")
            throw new InvalidOperationException("Given sii file is not a user profile");

        var profileName = onlyBlock.GetScalar("profile_name");
        var companyName = onlyBlock.GetScalar("company_name");
        
        // go through the other method to save the entity
        var req = new ProfileCreateRequest { Name = $"{companyName} ({profileName})" };
        return await CreateProfile(req);
    }
}