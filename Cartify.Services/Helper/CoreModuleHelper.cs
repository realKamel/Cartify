using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Cartify.Services.Helper;

public static partial class CoreModuleHelper
{
    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex RemoveNonAlphanumeric();


    [GeneratedRegex(@"\s+")]
    private static partial Regex ReplaceSpaceWithHyphens();


    [GeneratedRegex(@"-+")]
    private static partial Regex RemoveConsecutiveHyphens();

    public static string GenerateSlug(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var slug = name.ToLowerInvariant();
        slug = RemoveNonAlphanumeric().Replace(slug, "");
        slug = ReplaceSpaceWithHyphens().Replace(slug, "-").Trim();
        slug = RemoveConsecutiveHyphens().Replace(slug, "-");

        // Trim leading/trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }


    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    public static async Task<string> SaveCoverImageAndGeneratePathAsync(IFormFile image,
        string slug, string pathGuid,
        string directoryName,
        CancellationToken cancellationToken)
    {
        var safeImageName = ValidateImageAndGenerateName(image, slug, null);

        var dbPath = Path.Combine(directoryName, pathGuid);

        var serverPath = Path.Combine("wwwroot", dbPath);

        Directory.CreateDirectory(serverPath);

        var generatedImageDbPath = Path.Combine(dbPath, safeImageName);
        var generatedImageOnServerPath = Path.Combine(serverPath, safeImageName);

        try
        {
            await using var fileStream = new FileStream(generatedImageOnServerPath, FileMode.Create);
            await image.CopyToAsync(fileStream, cancellationToken);
        }
        catch (Exception e)
        {
            RemoveImagesFromStorage(generatedImageDbPath);
            throw;
        }

        return generatedImageDbPath;
    }

    public static async Task<List<string>> SaveMultiImageAndGeneratePathAsync(List<IFormFile> images,
        string slug, string pathGuid, string directoryName, CancellationToken cancellationToken)
    {
        var imagePaths = new List<string>(images.Count);

        var generatedPath = Path.Combine(directoryName, pathGuid, "images");

        Directory.CreateDirectory(generatedPath);

        try
        {
            for (var index = 0; index < images.Count; index++)
            {
                var image = images[index];

                var safeImageName = ValidateImageAndGenerateName(image, slug, index);

                var generatedImagePath = Path.Combine(generatedPath, safeImageName);

                imagePaths.Add(generatedImagePath);

                await using var fileStream = new FileStream(generatedImagePath, FileMode.Create);

                await image.CopyToAsync(fileStream, cancellationToken);
            }
        }
        catch (Exception e)
        {
            RemoveImagesFromStorage(generatedPath);
            throw;
        }

        return imagePaths;
    }

    private static string ValidateImageAndGenerateName(IFormFile image, string slug, int? index)
    {
        switch (image.Length)
        {
            case 0:
                throw new ValidationException("The provided image is invalid");
            case > 5 * 1024 * 1024 /* 5 MB */:
                throw new ValidationException("The image is too large max size is 5 MB");
        }

        var imageName = Path.GetFileName(image.FileName); // e.g., "image.jpg"

        var extension = Path.GetExtension(imageName).ToLowerInvariant();

        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            throw new ValidationException("Image Must be In Valid Format (\".jpg\", \".jpeg\", \".png\", \".webp\")");
        }

        var safeImageName = index is not null ? $"{slug}-{index}{extension}" : $"{slug}{extension}";

        return safeImageName;
    }

    public static bool RemoveImagesFromStorage(string path)
    {
        var extractedDirectoryName = Path.GetDirectoryName(path);

        if (!Directory.Exists(extractedDirectoryName))
        {
            return false;
        }

        Directory.Delete(extractedDirectoryName, true);

        return true;
    }
}