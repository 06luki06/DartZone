using System;
using At.luki0606.DartZone.API.Enums;

namespace At.luki0606.DartZone.API.Models
{
    public class Dart
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

        public Dart(Guid throwId, Multiplier multiplier, int field)
        {
            Id = Guid.NewGuid();
            ThrowId = throwId;
            Throw = null;
            Multiplier = multiplier;
            Field = field;
        }
        #endregion

        #region Methods
        #region Public Static Methods
        public static int CalculateScore(Multiplier multiplier, int field)
        {
            if (field < 1 || field > 20)
            {
                return 0;
            }
            int baseScore = field switch
            {
                25 => 25,
                _ => field
            };
            return baseScore * (int)multiplier;
        }
        #endregion
        #endregion
    }
}
