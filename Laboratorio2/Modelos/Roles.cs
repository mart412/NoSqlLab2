namespace Laboratorio2.Modelos
{
    public class Roles
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Roles(int id, string nombre)
        {
            Nombre = nombre;
        }
    }
}
