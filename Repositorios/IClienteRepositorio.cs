using WebApiClientes.Models;
using WebApiClientes.Models.Dto;

namespace WebApiClientes.Repositorios
{
    public interface IClienteRepositorio
    {
        Task<List<ClienteDto>> GetClientes();

        Task<ClienteDto> GetClienteById(int id);

        Task<ClienteDto> CreateUpdate(ClienteDto clienteDto);

        Task<bool> DeleteCliente(int id);

    }
}
