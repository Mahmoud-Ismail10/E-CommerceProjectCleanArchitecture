using Microsoft.AspNetCore.Http;

namespace E_Commerce.Service.Services.Contract
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(string location, IFormFile file);
    }
}
