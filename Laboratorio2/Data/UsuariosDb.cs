using Laboratorio2.Data.Configuracion;
using Laboratorio2.Modelos;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Laboratorio2.Data
{
    public class UsuariosDb
    {
        private readonly IMongoCollection<Usuario> _context;

        public UsuariosDb(IUsuariosDbSettings settings)
        {
            var mongodbClient = new MongoClient(settings.ConnectionString);
            var database = mongodbClient.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Usuario>(settings.UsuariosCollectionName);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Find(cli => true).ToList();
        }

        public Usuario UsuPorId(string email)
        {
            return _context.Find<Usuario>(usuario => usuario.email == email).FirstOrDefault();
        }

        public bool Crear(Usuario u)
        {
            _context.InsertOne(u);
            return true;
        }

        public void Modificar(string email, Usuario u)
        {
            _context.ReplaceOne(usuario => usuario.email == email, u);
        }

        public void EliminarById(string email)
        {
            _context.DeleteOne(usuario => usuario.email == email);
        }


    }
}
