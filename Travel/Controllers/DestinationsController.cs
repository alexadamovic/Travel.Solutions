using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.Models;
using System.Linq;

namespace Travel.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DestinationsController : ControllerBase
  {
    private readonly TravelContext _db;
    public DestinationsController(TravelContext db)
    {
      _db = db;
    }

    //GET api/destinations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Destination>>> Get(string mostReviews, string country, string highestOverallRating)
    {
      var query = _db.Destinations
        .Include(a => a.Reviews)
        .AsQueryable();

        if (highestOverallRating == "yes")
        {
          // foreach (Destination destination in query)
          // {
          //   destination.FindAverage();
          // }
          query = query.OrderByDescending(a => a.AverageRating);
        }

        if (mostReviews == "yes") //if the destination reviews are >0
        {
          query = query.OrderByDescending(a => a.Reviews.Count()); //sort destinations by most reviews -> least
        }

        if (country != null)
        {
          query = query.Where(a => a.Country == country);
        }

      return await query.ToListAsync(); // return that list
    }
    //http://localhost:5000/api/destinations/?mostReviews=yes

    //POST api/destinations
    [HttpPost]
    public async Task<ActionResult<Destination>> Post(Destination destination)
    {
      _db.Destinations.Add(destination);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetDestination), new { id = destination.DestinationId}, destination);
    }

    // GET: api/Destinations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetDestination(int id)
    {
        var destination = await _db.Destinations.FindAsync(id);

        if (destination == null)
        {
            return NotFound();
        }

        return destination;
    }

    // PUT: api/Destinations/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Destination destination)
    {
      if (id != destination.DestinationId)
      {
        return BadRequest();
      }

      _db.Entry(destination).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!DestinationExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // DELETE: api/Destinations/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDestination(int id)
    {
      var destination = await _db.Destinations.FindAsync(id);
      if (destination == null)
      {
        return NotFound();
      }

      _db.Destinations.Remove(destination);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    private bool DestinationExists(int id)
    {
      return _db.Destinations.Any(e => e.DestinationId == id);
    }
  }
}