using System;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Enums;

namespace At.luki0606.DartZone.API.Models;

internal sealed class Dart
{
    #region Properties
    public Guid Id { get; private set; }

    public Guid ThrowId { get; private set; }
    public Throw Throw { get; private set; }

    public Multiplier Multiplier { get; private set; }
    public int Field { get; private set; }
    public int Score => CalculateScore(Multiplier, Field);
    #endregion

    #region Ctor
    private Dart() { }

    public Dart(Multiplier multiplier, int field)
    {
        Id = Guid.NewGuid();
        Multiplier = multiplier;
        Field = field;
    }

    public Dart(DartRequestDto dartRequestDto)
        : this(dartRequestDto?.Multiplier ?? throw new ArgumentNullException(nameof(dartRequestDto)),
               dartRequestDto.Field)
    {
    }
    #endregion

    #region Methods
    #region Internal Methods
    internal void SetThrow(Throw throwObj)
    {
        if (ThrowId != Guid.Empty)
        {
            throw new InvalidOperationException("This dart is already associated with a throw.");
        }

        Throw = throwObj ?? throw new ArgumentNullException(nameof(throwObj), "Throw cannot be null.");
        ThrowId = throwObj.Id;
    }
    #endregion

    #region Public Static Methods
    public static int CalculateScore(Multiplier multiplier, int field)
    {
        if (field is not 25 and (< 0 or > 20))
        {
            return 0;
        }

        return multiplier == Multiplier.Triple && field == 25 ? 0 : field * (int)multiplier;
    }
    #endregion
    #endregion
}
