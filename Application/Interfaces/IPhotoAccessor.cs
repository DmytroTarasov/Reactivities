using System.Threading.Tasks;
using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    // methods in this interface doesn`t touch a DB - they are working with the Cloud service - Cloudinary
    public interface IPhotoAccessor
    {
        // IFormFile represents a file send with the HTTP-request
        Task<PhotoUploadResult> AddPhoto(IFormFile file);

        Task<string> DeletePhoto(string publicId);
    }
}