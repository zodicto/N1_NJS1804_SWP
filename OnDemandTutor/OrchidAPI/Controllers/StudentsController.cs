using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchidBusinessObjects;
using OrchidServices;

namespace OrchidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IOrchidServies _orchidService;

        public StudentsController(IOrchidServies orchidService)
        {
            _orchidService = orchidService;
        }

        // GET: api/Students
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            var students = _orchidService.GetStudent();
            if (students == null)
            {
                return NotFound();
            }
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _orchidService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _orchidService.AddStudent(student);

            return NoContent();
        }

        // POST: api/Students
        [HttpPost]
        public ActionResult<Student> PostStudent(Student student)
        {
            _orchidService.AddStudent(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _orchidService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            _orchidService.GetStudent().Remove(student);
            return NoContent();
        }
    }
}
