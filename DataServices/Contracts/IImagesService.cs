using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SessionTest.Models;

namespace SessionTest.DataServices.Contracts
{
    public interface IImagesService
    {
        void UploadImagesToProduct(string productId, List<IFormFile> files);

        IEnumerable<Image> GetImagesOfProduct(string productId);
    }
}
