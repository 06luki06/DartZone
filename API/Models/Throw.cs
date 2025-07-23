using System;
using System.Collections.Generic;
using System.Linq;

namespace At.luki0606.DartZone.API.Models
{
    public class Throw
    {
        #region Properties
        public Guid Id { get; private set; }
        public Guid GameId { get; private set; }
        public Game Game { get; private set; }

        public HashSet<Dart> Darts { get; private set; } = [];

        public int TotalScore => CalculateTotalScore(Darts);

        public DateTime CreatedAt { get; private set; }
        #endregion

        #region Ctor
        private Throw() { }

        public Throw(Guid gameId, Dart throw1, Dart throw2, Dart throw3)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Game = null;
            Darts = [throw1, throw2, throw3];
            CreatedAt = DateTime.UtcNow;
        }
        #endregion

        #region Methods
        #region Public Static Methods
        public static int CalculateTotalScore(HashSet<Dart> darts)
        {
            return darts.Sum(d => d.Score);
        }
        #endregion
        #endregion
    }
}
