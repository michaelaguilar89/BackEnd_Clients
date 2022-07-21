using WebApiClientes.Models;

namespace WebApiClientes.Repositorios
{
    public interface IUserRepositorio
    {
        Task<string> Register(User user, string password);
        Task<string> Login(string userName, string password);
        Task<bool> UserExiste(string userName);
    }
}
