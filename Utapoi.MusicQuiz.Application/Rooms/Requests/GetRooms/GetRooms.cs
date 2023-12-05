using FluentResults;
using MediatR;
using Utapoi.MusicQuiz.Application.Games;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

public static partial class GetRooms
{
    internal sealed class Handler : IRequestHandler<Request, Result<Response>>
    {
        private readonly IGameManager _gameManager;

        public Handler(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = _gameManager.GetRooms();

            return Task.FromResult(Result.Ok(new Response
            {
                Rooms = response,
            }));
        }
    }
}