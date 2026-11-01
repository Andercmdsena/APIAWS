namespace Despliegue_de_contenedores_en_AWS
{
    public class Student
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;
    }

    /// <summary>Datos necesarios para crear un estudiante nuevo.</summary>
    public class StudentCreateDto
    {
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;
    }
}
