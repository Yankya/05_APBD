using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripApp.Context;
using TripApp.Dto;
using TripApp.Models;

namespace TripApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        var dbContext = new S25463Context();
        var tripsDescending = await dbContext.Trips
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync();

        return Ok(tripsDescending);
    }

    [HttpPost("{idTrip:int}/clients")]
    public async Task<ActionResult> AddClientToTrip(int idTrip, ClientDto clientDto)
    {
        var dbContext = new S25463Context();
        
        //Czy wycieczka istnieje
        var trip = await dbContext.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound("Trip not found");
        }

        //sprawdzenie czy klient o podanym numerze PESEL istnieje
        var client = await dbContext.Clients.SingleOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
        if (client == null)
        {
            client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Pesel = clientDto.Pesel,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone
            };
            await dbContext.Clients.AddAsync(client);
        }

        //Sprawdzenie czy nie ma już przypisanej tej wycieczki
        var clientTripExists = await dbContext.ClientTrips
            .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);

        if (clientTripExists)
        {
            return BadRequest("Client is already assigned to this trip");
        }

        //dodanie po wszystkim
        var clientTrip = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientDto.PaymentDate
        };

        dbContext.ClientTrips.Add(clientTrip);
        await dbContext.SaveChangesAsync();

        return Ok();
    }
}

