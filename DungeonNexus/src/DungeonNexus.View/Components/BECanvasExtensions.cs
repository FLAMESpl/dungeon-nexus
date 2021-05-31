using Blazor.Extensions.Canvas.Canvas2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonNexus.View.Components
{
    static class BECanvasExtensions
    {
        public static Task SetFillColorAsync(this Canvas2DContext context, int red, int green, int blue)
        {
            return context.SetFillStyleAsync($"rgb({red},{green},{blue})");
        }
    }
}
