using Microsoft.EntityFrameworkCore;
using WebApiClientes.Models;

namespace WebApiClientes.Data
{
    public class myDbContext : DbContext
    {
        public myDbContext(DbContextOptions<myDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Cliente> Clientes {get; set;}
    }
}
