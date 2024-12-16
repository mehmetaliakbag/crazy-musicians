using CrazyMusicians.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrazyMusicians.Data;


namespace CrazyMusicians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusiciansController : ControllerBase
    {
        [HttpGet("all")]
        public ActionResult<IEnumerable<Musician>> GetAllMusicians()
        {
            var musicians = Repo.musicians;

            if (musicians is null || !musicians.Any())
                return NotFound(new { Message = "No musicians found." });

            return Ok(musicians);
        }

        [HttpGet("details/{id:int}")]
        public ActionResult<Musician> Get(int id)
        {
            var musician = Repo.musicians.FirstOrDefault(musician => musician.Id == id);

            if (musician is null)
                return NotFound(new { Message = "Musician not found." });

            return Ok(musician);
        }

        [HttpGet("search")]
        public ActionResult<Musician> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be null.");
            }

            // Divide names and surnames for better query search
            var musician = Repo.musicians
                .Where(musician => musician.Name.Split(" ")
                .Any(part => part.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!musician.Any())
            {
                return NotFound($"Musician with \"{name}\" not found");
            }

            return Ok(musician);
        }

        [HttpPost("create")]
        public ActionResult<Musician> Post([FromBody] Musician newMusician)
        {
            if (newMusician is null)
            {
                return BadRequest(new { Message = "Musician information cannot be null." });
            }

            newMusician.Id = !Repo.musicians.Any() ? 1 : Repo.musicians.Max(musician => musician.Id) + 1;

            Repo.musicians.Add(newMusician);

            return CreatedAtAction(nameof(Get), new { id = newMusician.Id }, newMusician);
        }

        //[HttpPatch("update/{id:int}")]
        //public IActionResult Patch(int id, [FromBody] JsonPatchDocument<Musician> patchDocument)
        //{
        //    var musician = Repo.musicians.FirstOrDefault(musician => musician.Id == id);

        //    if (musician == null)
        //        return NotFound(new { Message = "Musician not found." });

        //    if (patchDocument == null)
        //        return BadRequest(new { Message = "Patch document cannot be null." });

        //    patchDocument.ApplyTo(musician);

        //    return NoContent();
        //}

        [HttpDelete("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var musician = Repo.musicians.FirstOrDefault(musician => musician.Id == id);

            if (musician is null)
                return NotFound(new { Message = "Musician not found." });

            Repo.musicians.Remove(musician);

            return NoContent();
        }

    }
}
