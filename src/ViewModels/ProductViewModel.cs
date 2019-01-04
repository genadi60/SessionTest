using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SessionTest.MappingServices;
using SessionTest.Models;

namespace SessionTest.ViewModels
{
    public class ProductViewModel : IMapTo<Product>, IMapFrom<Product>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Unit { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public string ImagePath { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Product, ProductViewModel>()
                .ForMember(c => c.ImagePath,
                    m => m.MapFrom(p => p.Images.Select(x => x.FileName).FirstOrDefault()));
        }
    }
}
