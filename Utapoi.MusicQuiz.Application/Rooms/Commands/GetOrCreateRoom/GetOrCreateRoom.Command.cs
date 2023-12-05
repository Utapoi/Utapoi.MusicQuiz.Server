using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;

public static partial class GetOrCreateRoom
{
    public sealed record Command : IRequest<Result<Response>>
    {
        public Guid RoomId { get; init; } = Guid.Empty;

        public string RoomPassword { get; init; } = string.Empty;

        [Required]
        public Guid UserId { get; init; }
    }
}