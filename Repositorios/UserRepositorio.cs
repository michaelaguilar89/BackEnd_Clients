using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiClientes.Data;
using WebApiClientes.Models;

namespace WebApiClientes.Repositorios
{
    public class UserRepositorio : IUserRepositorio
    {

        private readonly myDbContext _db;
        private readonly IConfiguration _configuration;
        public UserRepositorio(myDbContext db,IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task<string> Login(string userName, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(
                x => x.UserName.ToLower().Equals(userName.ToLower())
                                                           );
            if (user == null)
            {
                return "No user found on database";
            } else if (!VerificarPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return "Wrong password";
            } else {
              //  return "ok";
               return CrearToken(user);
            }

        }
        public async Task<string> Register(User user, string password)
        {
            try
            {
                if (await UserExiste(user.UserName))
                {
                    return "existe";
                }

                CrearPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                // return user.Id;
                return CrearToken(user);

            }
            catch (Exception)
            {

                return "error";
            }
        }

        public async Task<bool> UserExiste(string userName)
        {
            bool existe =                
                await _db.Users.AnyAsync(
                    x => x.UserName.ToLower().Equals(userName.ToLower())
                                          );
           if (existe==true)
            {
                return true;
            }else
            {
                return false;
            }
            
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificarPasswordHash(string password,byte[] passwsordHash, byte[] passwsordSalt)
        {
            using (var hmac= new System.Security.Cryptography.HMACSHA512(passwsordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwsordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }

        }
        private string CrearToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
                GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
