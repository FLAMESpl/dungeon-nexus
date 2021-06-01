using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Threading;

namespace DungeonNexus.View
{
    public readonly struct RgbColor
    {
        public readonly int Red { get; }
        public readonly int Green { get; }
        public readonly int Blue { get; }

        public RgbColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public readonly void Deconstruct(out int red, out int green, out int blue)
        {
            red = Red;
            green = Green;
            blue = Blue;
        }

        public static RgbColor White { get; } = new(255, 255, 255);

        public static bool operator ==(RgbColor _1, RgbColor _2) =>
            _1.Red == _2.Red && _1.Green == _2.Green && _1.Blue == _2.Blue;

        public static bool operator !=(RgbColor _1, RgbColor _2) => !(_1 == _2);
    }
}
