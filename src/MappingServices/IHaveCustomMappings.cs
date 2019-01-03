using AutoMapper;

namespace SessionTest.MappingServices
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
