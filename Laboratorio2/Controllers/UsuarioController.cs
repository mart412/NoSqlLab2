using Microsoft.AspNetCore.Mvc;
using Laboratorio2.Data;
using Laboratorio2.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace Laboratorio2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuariosDb _db;
        
        public UsuarioController(UsuariosDb db)
        {
            _db=db;
        }

        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            return Ok(_db.ObtenerUsuarios());
        }

        [HttpGet("{email}")]
        public IActionResult UsuPorId(string email)
        {
            var u = _db.UsuPorId(email);
            if (u == null) return NotFound();

            return Ok(u);
        }

        [HttpPost]
        public IActionResult Crear(Usuario u)
        {
            var aux = _db.UsuPorId(u.email);
            if (aux == null)
            {
                _db.Crear(u);

                return Ok("Usuario: " + u.email + ", creado correctamente");

            }
            else
            {
                return BadRequest("Ya existe un usuario con el email: " + u.email);
            }

        }

        [HttpPost("{email}")]
        public IActionResult Actualizar(string email, Usuario u)
        {
            var usuario = _db.UsuPorId(email);
            if (usuario == null) return NotFound("No existe el usuario");
            _db.EliminarById(email);
            _db.Crear(u);
            return Ok("Usuario: " + email + ", modificado con exito");
        }


        [HttpDelete("{email}")]
        public IActionResult Eliminar(string email)
        {
            var usuario = _db.UsuPorId(email);
            if (usuario == null) return NotFound("No existe el usuario");

            _db.EliminarById(email);
            return Ok("Usuario: " + email + ", eliminado con exito");
        }

        [HttpPost("{email},{pass}")]
        public IActionResult AgregarRol(string email, string pass, ICollection<Roles> roles)
        {
            
            var aux = _db.UsuPorId(email); //busco el usuario con mail igual a email y lo igualo a aux
            if (aux == null) return NotFound("No existe el usuario");//controlo que existe el usuario

            if (aux.password!= pass) //Valido que la contraseña pasada por parametro sea igual a la contraseña del usuario
            {
                return NotFound("Password incorrecto");  
            }

            if (aux.roles == null) //si la lista de roles del usuario es nula, la igualo directamente a la pasada por parametros
            {
                aux.roles = roles;
            }
            else //si no es nula 
            {
                foreach (var r in roles) //itero con el for para cada nuevo rol ingresado de la lista pasada por parametro 
                {
                    if (!aux.roles.Any(x => x.Nombre == r.Nombre))
                    {
                        aux.roles.Add(r);
                    }//verifico si ya existe ese rol asignado al usuario, si ya esta asignado no hago nada y si no esta lo agrego
                }
            }

            _db.EliminarById(email); //elimino el usuario 
            _db.Crear(aux); //agrego el nuevo usuario con los roles actualizados
            return Ok("Roles actualizados correctamente");

        }
    }
}
