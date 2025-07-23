using System;
using System.Collections.Generic;
using System.Linq;
using At.luki0606.DartZone.API.Models;

namespace At.luki0606.DartZone.API.Dtos
{
    public class GameDto
    {
        #region Properties
        public Guid Id { get; }
        public int StartScore { get; }
        public int CurrentScore { get; }
        public DateTime CreatedAt { get; }
        public bool HasFinished { get; }
        public bool HasStarted { get; }
        public IReadOnlyList<ThrowDto> Throws { get; }
        #endregion

        #region Ctor
        public GameDto(Game game)
        {
            Id = game.Id;
            StartScore = game.StartScore;
            CurrentScore = game.CurrentScore;
            CreatedAt = game.CreatedAt;
            HasFinished = game.HasFinished;
            HasStarted = game.HasStarted;
            Throws = [.. game.Throws.Select(t => new ThrowDto(t))];
        }
        #endregion
    }
}
