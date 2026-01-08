using System;
using System.Collections.Generic;
using At.luki0606.DartZone.Shared.Results;

namespace At.luki0606.DartZone.API.Models;

internal class Game
{
    #region Properties
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public int StartScore { get; private set; }
    public int CurrentScore { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool HasFinished { get; internal set; }
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

    #region Methods
    public Result AddThrow(Throw throwObj)
    {
        if (HasFinished)
        {
            return Result.Failure("Cannot add throws to a finished game.");
        }

        if (throwObj == null)
        {
            return Result.Failure("Throw cannot be null.");
        }

        if (throwObj.GameId != Id)
        {
            return Result.Failure("Throw does not belong to this game.");
        }

        if (Throws.Contains(throwObj))
        {
            return Result.Failure("This throw has already been added to the game.");
        }

        if (CurrentScore - throwObj.TotalScore < 0)
        {
            return Result.Failure("Cannot add a throw that would result in a negative score.");
        }

        if (!HasStarted)
        {
            HasStarted = true;
        }

        CurrentScore -= throwObj.TotalScore;

        if (CurrentScore == 0)
        {
            HasFinished = true;
        }

        Throws.Add(throwObj);
        return Result.Success();
    }
    #endregion
}
