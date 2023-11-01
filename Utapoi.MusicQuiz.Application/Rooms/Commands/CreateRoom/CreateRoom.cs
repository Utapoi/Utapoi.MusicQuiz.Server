using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;

public static partial class CreateRoom
{
    internal sealed class Handler : IRequestHandler<Command, Result<Response>>
    {
        private readonly IRoomsService _roomsService;

        public Handler(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            var room = await _roomsService.CreateAsync(command, cancellationToken);

            return Result.Ok(new Response
            {
                RoomId = room.Id
            });
        }
    }
}