using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;
using Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;
using Utapoi.MusicQuiz.Core.Sockets;
using Utapoi.MusicQuiz.Server.Requests.Rooms;

namespace Utapoi.MusicQuiz.Server.Controllers;

[DisplayName("Rooms")]
public class RoomsController : ApiControllerBase
{
    private readonly IGameManager _gameManager;

    public RoomsController(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(GetRooms.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await Mediator.Send(new GetRooms.Request(), cancellationToken);

        return Ok(rooms.Value);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateRoom.Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoomRequest req, CancellationToken cancellationToken = default)
    {
        var p = _gameManager.GetPlayer(GetUserId());

        if (p == null)
        {
            return BadRequest();
        }

        var room = await Mediator.Send(new CreateRoom.Command
        {
            Name = req.Name,
            Password = req.Password,
            MaxPlayers = req.MaxPlayers,
            Rounds = req.Rounds,
        }, cancellationToken);

        var socketRoom = new WebSocketRoom
        {
            Id = room.Value.Id,
            Name = room.Value.Name,
            Players = new ConcurrentDictionary<Guid, WebSocketUser>()
        };

        socketRoom.Players.TryAdd(p.Id, p);
        _gameManager.AddRoom(socketRoom);

        return Ok(room.Value);
    }
}