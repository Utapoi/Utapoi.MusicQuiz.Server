using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRoom;

public static partial class GetRoom
{
    public sealed class Request : IRequest<Result<Response>>
    {
        public Guid RoomId { get; set; }
    }
}