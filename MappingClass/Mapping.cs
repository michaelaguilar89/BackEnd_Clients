using AutoMapper;
using WebApiClientes.Models;
using WebApiClientes.Models.Dto;

namespace WebApiClientes.MappingClass
{
    public class Mapping
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<ClienteDto, Cliente>();
                config.CreateMap<Cliente, ClienteDto>();

            });
            return mapping;
        }
    }
}
