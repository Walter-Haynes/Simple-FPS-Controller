using JetBrains.Annotations;
// ReSharper disable CheckNamespace
// ReSharper disable once InconsistentNaming

namespace CommonGames.Utilities.CustomTypes
{
    /// <summary>
    /// Instead of having multiple booleans ie (talkingBlocksWalking, fallingBlocksWalking, etc) just use one lock and increment if something is
    /// blocking it and decrement it when something stops blocking it.
    /// </summary>
    public struct CGLock
    {
        private readonly int _value;

        [PublicAPI]
        public bool Locked => _value > 0;

        public CGLock(in int value = 0) => _value = value;

        public static CGLock operator +(in CGLock a, in CGLock b) => new CGLock(a._value + b._value);
        public static CGLock operator -(in CGLock a, in CGLock b) => new CGLock(a._value - b._value);

        public static CGLock operator ++(in CGLock a) => new CGLock(a._value + 1);
        public static CGLock operator --(in CGLock a) => new CGLock(a._value - 1);

        public static implicit operator int(in CGLock a) => a._value;
        public static implicit operator CGLock(in int i) => new CGLock(i);

        public static implicit operator bool(in CGLock cgLock) => !cgLock.Locked;
    }
}