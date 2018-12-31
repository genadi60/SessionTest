using System.Linq;
using AutoMapper;
using SessionTest.MappingServices;

namespace SessionTest.Models
{
    public class CategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountOfAllProducts { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, CategoryViewModel>()
                .ForMember(c => c.CountOfAllProducts,
                    m => m.MapFrom(p => p.Products.Count()));
        }
    }
}
