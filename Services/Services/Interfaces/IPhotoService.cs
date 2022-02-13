namespace Services.Services.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoToCloudAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoFromCloudAsync(string publicId);
}