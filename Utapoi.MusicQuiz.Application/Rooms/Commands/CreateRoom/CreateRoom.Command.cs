using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;

namespace Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;

public static partial class CreateRoom
{
    public class Command : IRequest<Result<Response>>
    {
        public string Name { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;

        public int MaxPlayers { get; init; }

        public int Rounds { get; init; }
    }
}