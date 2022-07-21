using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiClientes.Models;
using WebApiClientes.Models.Dto;
using WebApiClientes.Repositorios;

namespace WebApiClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepositorio _userRepositorio;
        private ResponseDto _response;
        public UsersController(IUserRepositorio userRepositorio)
        {
            _userRepositorio = userRepositorio;
            _response = new ResponseDto();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserDto user)
        {
            var respuesta = await _userRepositorio.Login(user.UserName, user.Password);


            if (respuesta == "No user found on database")
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "El usuario no existe";
                return BadRequest(_response);
            }
            if (respuesta == "Wrong password")
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "Password Incorrecta";
                return BadRequest(_response);
            }

            //return Ok("Usuario Conectado");
            _response.IsSuccess = true;
            // _response.Result=respuesta;
            _response.IsSuccess = true;
            JwtPackage jwt = new JwtPackage();
            jwt.userName = user.UserName;
            jwt.token = respuesta;            
            _response.Result = jwt;

            _response.DisplayMessages = "Usuario Conectado";
            return Ok(_response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Resgister(UserDto user) 
        {
            var respuesta = await _userRepositorio.Register(
                new User
                {
                    UserName = user.UserName
                }, user.Password);

            if (respuesta == "existe")
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "Usuario ya existe";
                return BadRequest(_response);
            }

            if (respuesta == "error")
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "Error al crear el usuario";
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            JwtPackage jwt = new JwtPackage();
            jwt.userName=user.UserName;
            jwt.token = respuesta;
            // _response.Result = respuesta;
            _response.Result = jwt;

            _response.DisplayMessages = "Usuario creado con exito";
            return Ok(_response);

                
        }
        
    }
    public class JwtPackage
    {
        public string userName { get; set; }

        public string token { get; set; }
    }
}
