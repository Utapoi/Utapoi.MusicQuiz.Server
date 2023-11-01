using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

public static partial class GetRooms
{
    public class Request : IRequest<Result<Response>>
    {
    }
}