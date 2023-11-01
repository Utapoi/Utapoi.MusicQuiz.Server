using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

public static partial class GetRooms
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
            var response = await _roomsService.GetAllAsync(cancellationToken);

            return Result.Ok(new Response
            {
                Rooms = response,
            });
        }
    }
}