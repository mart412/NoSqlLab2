
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laboratorio2.Modelos
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId id { get; set; }
        
        
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required] 
        public string nombre   { get; set; }

        [Required] 
        public string apellido { get; set; }

        [BsonElement("roles")]
        public ICollection<Roles> roles { get; set;  }

        public Usuario(string email, string password, string nombre, string apellido)
        {
            this.email = email;
            this.password = password;
            this.nombre = nombre;
            this.apellido = apellido;
        }

        public Usuario()
        {

        }

    }
}
