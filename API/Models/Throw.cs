using System;
using System.Collections.Generic;
using System.Linq;
using At.luki0606.DartZone.Shared.Results;

namespace At.luki0606.DartZone.API.Models;

internal sealed class Throw
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

    public Throw(Guid gameId, Dart dart1, Dart dart2, Dart dart3)
    {
        Id = Guid.NewGuid();
        GameId = gameId;
        Game = null;
        Darts = [dart1, dart2, dart3];
        foreach (Dart dart in Darts)
        {
            dart.SetThrow(this);
        }

        CreatedAt = DateTime.UtcNow;
    }
    #endregion

    #region Methods
    #region Public Static Methods
    public static Result<int> CalculateTotalScore(IEnumerable<Dart> darts)
    {
        if (darts == null)
        {
            return Result<int>.Failure("A throw must consist of exactly 3 darts.");
        }

        Dart[] dartArray = darts as Dart[] ?? [.. darts];

        if (dartArray.Length != 3)
        {
            return Result<int>.Failure("A throw must consist of exactly 3 darts.");
        }

        return Result<int>.Success(dartArray.Sum(d => d.Score));
    }
    #endregion
    #endregion
}
