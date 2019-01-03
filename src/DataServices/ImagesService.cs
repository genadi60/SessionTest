using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SessionTest.Common;
using SessionTest.DataServices.Contracts;
using SessionTest.Models;

namespace WebshopApp.Services.DataServices
{
    public class ImagesService : IImagesService
    {
        private readonly IHostingEnvironment host;
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Image> imagesRepository;

        public ImagesService(IRepository<Product> productsRepository, IHostingEnvironment host, IRepository<Image> imagesRepository)
        {
            this.productsRepository = productsRepository;
            this.host = host;
            this.imagesRepository = imagesRepository;
        }

        public async void UploadImagesToProduct(string productId, List<IFormFile> files)
        {
            var product = this.productsRepository.All().FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                throw new KeyNotFoundException();
            }

            var uploadFolderPath = Path.Combine(host.WebRootPath, "images/products");

            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                var filePath = Path.Combine(uploadFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var image = new Image
                {
                    FileName = fileName,
                    ProductId = product.Id
                };

                await this.imagesRepository.AddAsync(image);

                product.Images.Add(image);

                await this.productsRepository.SaveChangesAsync();
                await this.imagesRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<Image> GetImagesOfProduct(string productId)
        {
            var images = this.imagesRepository.All().Where(x => x.ProductId == productId).ToList();

            if (!images.Any())
            {
                throw new ArgumentNullException();
            }

            return images;
        }
    }
}
