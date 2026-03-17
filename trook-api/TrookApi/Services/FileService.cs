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
        var fileName = Path.GetFileName(filePath);
        return await SaveFileAsync(fileName, fileBytes);
    }

    public async Task<SiiFile?> SaveFileFromFormAsync(IFormFile formFile)
    {
        logger.LogInformation("Processing form data file {FileName}", formFile.FileName);
        var l = formFile.Length;
        var bytes = new byte[l];
        var writeStream = new MemoryStream(bytes);
        await using var stream = formFile.OpenReadStream();
        await stream.CopyToAsync(writeStream);

        return await SaveFileAsync(formFile.FileName, bytes);
    }

    public async Task<SiiFile?> SaveFileAsync(string fileName, byte[] fileBytes)
    {
        logger.LogInformation("Processing file {FileName} (size = {Size} bytes)...", fileName, fileBytes.Length);
        var fileHash = MD5.HashData(fileBytes);

        if (db.ProcessedFiles.Any(pf => pf.IsSuccess && pf.FileHash == fileHash))
        {
            logger.LogInformation("File already processed; skipping");
            return null;
        }

        logger.LogInformation("Decrypting and decoding file...");
        var decrypted = await SiiDecryptor.DecryptScsc(fileBytes);
        var decoded = SiiDecoder.DecodeSii(decrypted);

        db.ProcessedFiles.Add(new ProcessedFile
        {
            FileHash = fileHash,
            FileName = fileName,
            IsSuccess = true
        });
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully processed file");
        return decoded;
    }
}