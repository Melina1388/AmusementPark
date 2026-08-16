namespace Application.DTOs;

public class SelectTicketRequestDto
{
    public int GameId { get; set; }

    public int Quantity { get; set; } = 1;
}