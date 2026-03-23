using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;

namespace TrookApi.Services;

public class LocalizationService(TrookDbContext db, ILogger<LocalizationService> logger)
{
    public async Task<List<LocalizationEntry>> GetAllStringsForProfile(Guid profileId)
    {
        return await db.LocalizationEntries.Where(le => le.ProfileId == profileId).ToListAsync();
    }

    public async Task<LocalizationEntry> CreateString(Guid profileId, CreateStringRequest request)
    {
        var newEntry = new LocalizationEntry
        {
            ProfileId = profileId, Key = request.Key, Localized = request.Localized
        };

        await db.AddAsync(newEntry);
        await db.SaveChangesAsync();
        return newEntry;
    }

    public async Task<LocalizationEntry> UpdateString(Guid id, Guid profileId, CreateStringRequest request)
    {
        try
        {
            var entry = db.LocalizationEntries.First(le =>
                le.Id == id && le.ProfileId == profileId && le.Key == request.Key);
            entry.Localized = request.Localized;
            await db.SaveChangesAsync();
            return entry;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Given localization entry could not be updated", e);
        }
    }
}