using Services.Services.Interfaces;

namespace Services.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        _cloudinary =
            new Cloudinary(new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret));
    }

    public async Task<ImageUploadResult> AddPhotoToCloudAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length <= 0) return uploadResult;
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        };
        uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoFromCloudAsync(string publicId)
    {
        return await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}