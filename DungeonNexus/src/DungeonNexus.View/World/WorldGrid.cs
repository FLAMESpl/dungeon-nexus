using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Threading.Tasks;

namespace DungeonNexus.View.World
{
    public partial class WorldGrid
    {
        private const int GRID_SIZE = 60;

        [AllowNull] private BECanvasComponent canvas;
        [AllowNull] private Canvas2DContext context;
        [AllowNull] private GridModel grid;

        private bool initialized = false;

        [Parameter] public int Height { get; set; }
        [Parameter] public int Width { get; set; }
        [Parameter] public RgbColor FillColor { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                context = await canvas.CreateCanvas2DAsync();
                grid = new GridModel(Width, Height, GRID_SIZE);
                initialized = true;
                await grid.Draw(context);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task HandleClick(MouseEventArgs mouseEventArgs)
        {
            if (initialized)
            {
                await grid.ToogleSquare(context, mouseEventArgs.OffsetX, mouseEventArgs.OffsetY, FillColor);
            }
        }
    }
}
