using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRoom;

public static partial class GetRoom
{
    internal sealed class Handler : IRequestHandler<Request, Result<Response>>
    {
        private readonly IRoomsService _roomsService;

        public Handler(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            var result = await _roomsService.GetAsync(request.RoomId, cancellationToken);

            if (result == null)
            {
                // TODO: Create a EntityNotFoundError
                return Result.Fail("Not Found");
            }

            return Result.Ok(new Response
            {
                Room = result
            });
        }
    }
}