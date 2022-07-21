using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiClientes.Data;
using WebApiClientes.Models;
using WebApiClientes.Models.Dto;
using WebApiClientes.Repositorios;

namespace WebApiClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        // private readonly myDbContext _context;

        private readonly IClienteRepositorio _clienteRepositorio;
        protected ResponseDto _response;

        public ClientesController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
            _response = new ResponseDto();
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                var lista = await _clienteRepositorio.GetClientes();
                _response.Result = lista;
                _response.IsSuccess = true;
                _response.DisplayMessages = "Lista de Clientes";


            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string>{ex.ToString()};
            }
            return Ok(_response);

        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _clienteRepositorio.GetClienteById(id);
            if (cliente==null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "Cliente no existe";
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.Result=cliente;
            _response.DisplayMessages = "Informacion del cliente";
            return Ok(_response);
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente( ClienteDto clienteDto)
        {
            try
            {
                ClienteDto model = await _clienteRepositorio.CreateUpdate(clienteDto);
                _response.Result = model;
                _response.IsSuccess = true;
                _response.DisplayMessages = "Cliente actualizado con exito";
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.DisplayMessages = "Error al actualizar el cliente";
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);


            }
           
        }

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(ClienteDto clienteDto)
        {
            try
            {
                ClienteDto model = await _clienteRepositorio.CreateUpdate(clienteDto);
                _response.IsSuccess = true;
                _response.Result = model;
                _response.DisplayMessages = "Cliente creado exitosamente";
                return CreatedAtAction("GetCliente", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessages = "Error al crear el cliente";
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
                
            }

            
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                bool eliminado = await _clienteRepositorio.DeleteCliente(id);
                if (eliminado == true)
                {
                    _response.Result = eliminado;
                    _response.DisplayMessages = "ClienteEliminado con exito";
                    return Ok(_response);

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessages = "Error al eliminar el cliente";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
                
            }
           
        }

       
    }
}
