using Microsoft.AspNetCore.Mvc;
using Laboratorio2.Data;
using Laboratorio2.Modelos;
using System.Collections.Generic;
using System.Linq;
using System;

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

        [HttpGet("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            return Ok(_db.ObtenerUsuarios());
        }

        [HttpGet("ObtenerUsuarios/{email}")]
        public IActionResult UsuPorId(string email)
        {
            var u = _db.UsuPorId(email);
            if (u == null) return NotFound();

            return Ok(u);
        }

        [HttpPost("CrearUsuario")]
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
                return BadRequest("ERROR 101 - Ya existe un usuario con el email: " + u.email);
            }

        }



        [HttpDelete("eliminarUsuario/{email}")]
        public IActionResult Eliminar(string email)
        {
            var usuario = _db.UsuPorId(email);
            if (usuario == null) return NotFound("ERROR 102 - No existe el usuario");

            _db.EliminarById(email);
            return Ok("Usuario: " + email + ", eliminado con exito");
        }

        [HttpPost("agregarRol/{email},{pass}")]

        public IActionResult AgregarRol(string email, string pass, ICollection<Roles> roles)
        {
            
            var aux = _db.UsuPorId(email); //busco el usuario con mail igual a email y lo igualo a aux
            if (aux == null) return NotFound("ERROR 102 - No existe el usuario");//controlo que existe el usuario

            if (aux.password!= pass) //Valido que la contraseña pasada por parametro sea igual a la contraseña del usuario
            {
                return NotFound("ERROR 104 - Password incorrecto");  
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

        [HttpPost("quitarRol/{email},{pass}")]
        public IActionResult QuitarRol(string email, string pass, List<Roles> roles)
        {
            var aux = _db.UsuPorId(email); //busco el usuario con mail igual a email y lo igualo a aux
            if (aux == null) return NotFound("ERROR 102 - No existe el usuario");//controlo que existe el usuario

            if (aux.password != pass) //Valido que la contraseña pasada por parametro sea igual a la contraseña del usuario
            {
                return NotFound("ERROR 104 - Password incorrecto");
            }

            if (aux.roles == null) //si la lista de roles del usuario es nula, devuelvo error 
            {
                return BadRequest("ERROR 105 - El usuario " + email + " no tiene asociado roles");
            }
            List<Roles> roles2 = aux.roles.ToList();
            Roles rolAux;

            foreach (var r in roles) //itero con el for para cada nuevo rol ingresado de la lista pasada por parametro 
            {
                if (!roles2.Any(x => x.Nombre == r.Nombre))
                {
                    return BadRequest("ERROR 103 - El usuario "+email+ " no tiene asociado el rol "+r.Nombre);
                }//verifico si existe ese rol en la lista de roles del usuario, si no existe devuelvo error, si existe lo elimino
                else
                {
                    rolAux = roles2.Where(x => x.Nombre == r.Nombre).FirstOrDefault();
                    roles2.Remove(rolAux);
                }
            }

            aux.roles = roles2;
            _db.EliminarById(email); //elimino el usuario 
            _db.Crear(aux); //agrego el nuevo usuario con los roles actualizados
            return Ok("Roles eliminados con exito");
        } 

        [HttpGet("login/{email},{pass}")]
        public ActionResult<bool> Login(string email, string pass)
        {
            var aux = _db.UsuPorId(email); //busco el usuario con mail igual a email y lo igualo a aux
            if (aux == null) return NotFound("ERROR 102 - No existe el usuario");//controlo que existe el usuario

            if (aux.password != pass) //Valido que la contraseña pasada por parametro sea igual a la contraseña del usuario
            {
                return false;
            }
            return true;

        }
        
        [HttpGet]
        [Route("errores")]

        public ActionResult<ICollection<string>> Errores()
        {
            var lista = new List<string>();
            lista.Add("ERROR 101 - Ya existe un usuario con el email *email*");
            lista.Add("ERROR 102 - No existe el usuario");
            lista.Add("ERROR 103 - El usuario {email} no tiene asociado el rol {rol}");
            lista.Add("ERROR 104 - Password incorrecto");
            lista.Add("ERROR 105 - El usuario {email} no tiene asociado roles");

            return lista;

        }
        



    }
}
