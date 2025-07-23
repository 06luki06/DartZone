using System;
using System.Collections.Generic;

namespace At.luki0606.DartZone.API.Models
{
    public class Game
    {
        #region Properties
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public int StartScore { get; private set; }
        public int CurrentScore { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool HasFinished { get; private set; }
        public bool HasStarted { get; private set; }

        public HashSet<Throw> Throws { get; private set; } = [];
        #endregion

        #region Ctor
        private Game() { }

        public Game(Guid userId, int startScore)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            User = null;
            StartScore = startScore;
            CurrentScore = startScore;
            CreatedAt = DateTime.UtcNow;
            HasFinished = false;
            HasStarted = false;
            Throws = [];
        }
        #endregion
    }
}
