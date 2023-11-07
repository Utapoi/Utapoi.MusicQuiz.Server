using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;
using Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;
using Utapoi.MusicQuiz.Server.Requests.Rooms;

namespace Utapoi.MusicQuiz.Server.Controllers;

[DisplayName("Rooms")]
public class RoomsController : ApiControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetRooms.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await Mediator.Send(new GetRooms.Request(), cancellationToken);

        return Ok(rooms.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateRoom.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoomRequest req, CancellationToken cancellationToken = default)
    {
        var room = await Mediator.Send(new CreateRoom.Command
        {
            Name = req.Name,
            Password = req.Password,
            MaxPlayers = req.MaxPlayers,
            Rounds = req.Rounds,
        }, cancellationToken);

        return Ok(room.Value);
    }
}