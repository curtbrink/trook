using System.Security.Cryptography;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookSii;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class FileService(TrookDbContext db, ILogger<FileService> logger)
{
    public async Task<SiiFile?> ReadAndSaveFileAsync(string filePath)
    {
        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var fileHash = MD5.HashData(fileBytes);

        if (db.ProcessedFiles.Any(pf => pf.IsSuccess && pf.FileHash == fileHash))
        {
            logger.LogInformation("File already processed, skipping");
            return null;
        }
        
        var decrypted = await SiiDecryptor.DecryptScsc(fileBytes);
        var decoded = SiiDecoder.DecodeSii(decrypted);

        db.ProcessedFiles.Add(new ProcessedFile
        {
            FileHash = fileHash,
            FileName = Path.GetFileName(filePath),
            IsSuccess = true
        });
        await db.SaveChangesAsync();

        return decoded;
    }
}