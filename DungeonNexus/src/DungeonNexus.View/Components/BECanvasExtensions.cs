using Blazor.Extensions.Canvas.Canvas2D;
using System.Threading.Tasks;

namespace DungeonNexus.View.Components
{
    static class BECanvasExtensions
    {
        public static Task SetFillColorAsync(this Canvas2DContext context, RgbColor color)
        {
            return context.SetFillStyleAsync($"rgb({color.Red},{color.Green},{color.Blue})");
        }
    }
}
