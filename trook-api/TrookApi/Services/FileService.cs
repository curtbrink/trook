using System.Security.Cryptography;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;
using TrookSii;

namespace TrookApi.Services;

public class FileService(TrookDbContext db, ILogger<FileService> logger)
{
    public async Task<SaveFileResult> ReadAndSaveFileAsync(string filePath)
    {
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var fileName = Path.GetFileName(filePath);
        return await SaveFileAsync(fileName, fileBytes);
    }

    public async Task<SaveFileResult> SaveFileFromFormAsync(IFormFile formFile, Guid? profileId = null)
    {
        logger.LogInformation("Processing form data file {FileName}", formFile.FileName);
        var l = formFile.Length;
        var bytes = new byte[l];
        var writeStream = new MemoryStream(bytes);
        await using var stream = formFile.OpenReadStream();
        await stream.CopyToAsync(writeStream);

        return await SaveFileAsync(formFile.FileName, bytes, profileId);
    }

    public async Task<SaveFileResult> SaveFileAsync(string fileName, byte[] fileBytes, Guid? profileId = null)
    {
        logger.LogInformation("Processing file {FileName} (size = {Size} bytes)...", fileName, fileBytes.Length);
        var fileHash = MD5.HashData(fileBytes);

        // DISABLING UNIQUENESS FOR NOW
        // var foundFile = db.ProcessedFiles.FirstOrDefault(pf => pf.IsSuccess && pf.FileHash == fileHash);
        // if (foundFile is not null)
        // {
        //     logger.LogInformation("File already processed; skipping");
        //     return new SaveFileResult(null, foundFile);
        // }

        logger.LogInformation("Decrypting and decoding file...");
        var decrypted = await SiiDecryptor.DecryptScsc(fileBytes);
        var decoded = SiiDecoder.DecodeSii(decrypted);

        var entityEntry = db.ProcessedFiles.Add(new ProcessedFile
        {
            FileHash = fileHash,
            FileName = fileName,
            IsSuccess = true,
            ProfileId = profileId
        });
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully processed file");
        return new SaveFileResult(decoded, entityEntry.Entity);
    }
}