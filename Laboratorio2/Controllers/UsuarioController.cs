using Microsoft.AspNetCore.Mvc;
using Laboratorio2.Data;
using Laboratorio2.Modelos;

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
            _db.Crear(u);

            return Ok("Usuario: " + u.email + ", creado correctamente");
        }

        [HttpPut("{email}")]
        public IActionResult Actualizar(string email, Usuario u)
        {
            var usuario = _db.UsuPorId(email);
            if (usuario == null) return NotFound();

            _db.Modificar(email, u);
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
    }
}
