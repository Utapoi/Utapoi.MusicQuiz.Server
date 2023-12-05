using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

namespace Utapoi.MusicQuiz.Server.Controllers;

[DisplayName("Rooms")]
public class RoomsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetRooms.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await Mediator.Send(new GetRooms.Request(), cancellationToken);

        return Ok(rooms.Value);
    }
}