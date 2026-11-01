using Microsoft.AspNetCore.Mvc;

namespace Despliegue_de_contenedores_en_AWS.Controllers
{
    /// <summary>
    /// CRUD sencillo en memoria sobre una lista de estudiantes.
    /// </summary>
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        // Estática para que los cambios persistan entre peticiones,
        // ya que ASP.NET Core crea una instancia nueva del controlador por cada request.
        private static readonly List<Student> Students =
        [
            new Student { Id = 1, Nombre = "Anderson", Apellido = "Sena", Correo = "pruebaAWS@example.com" },
            new Student { Id = 2, Nombre = "David", Apellido = "Gomez", Correo = "pruebaAction.com" },
            new Student { Id = 3, Nombre = "Juan", Apellido = "Perez", Correo = "juan@example.com" },
            new Student { Id = 4, Nombre = "Roberto", Apellido = "Diaz", Correo = "roberto@example.com" },
            new Student { Id = 5, Nombre = "Maria", Apellido = "Lopez", Correo = "maria@example.com" },
            new Student { Id = 6, Nombre = "Joaquin", Apellido = "Ramirez", Correo = "joaquin@example.com" }
        ];

        private static int _nextId = Students.Max(s => s.Id) + 1;

        /// <summary>Lista todos los estudiantes.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(Students);
        }

        /// <summary>Obtiene un estudiante por su Id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetById(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound($"No se encontró el estudiante con id {id}.");

            return Ok(student);
        }

        /// <summary>Crea un nuevo estudiante.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Student> Create([FromBody] StudentCreateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                return BadRequest("El apellido no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.Correo))
                return BadRequest("El correo no puede estar vacío.");

            var student = new Student
            {
                Id = _nextId++,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo = request.Correo
            };
            Students.Add(student);

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        /// <summary>Elimina un estudiante por su Id.</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound($"No se encontró el estudiante con id {id}.");

            Students.Remove(student);

            return NoContent();
        }
    }
}
