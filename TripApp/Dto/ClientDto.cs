namespace TripApp.Dto;

public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public int IdTrip { get; set;}
    public string TripName { get; set; } = null!;
    public DateTime? PaymentDate { get; set; }
}