using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripApp.Context;
using TripApp.Models;

namespace TripApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{

    [HttpDelete("{idClient:int}")]
    public async Task<ActionResult> DeleteClient(int idClient)
    {
        var dbContext = new S25463Context();
        var clientToRemove = new Client()
        {
            IdClient = idClient
        };

        dbContext.Clients.Attach(clientToRemove);

        var hasTrips = await dbContext.ClientTrips.AnyAsync(clientTrips => clientTrips.IdClient == idClient);

        if (hasTrips)
        {
            return BadRequest("Cannot delete client with assigned trips");
        }

        dbContext.Clients.Remove(clientToRemove);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}