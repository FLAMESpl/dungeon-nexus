using Blazor.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DungeonNexus.View.World
{
    public partial class WorldEditor
    {
        [AllowNull]
        private BECanvasComponent canvas;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var ctx = await canvas.CreateCanvas2DAsync();
            await ctx.SetFillStyleAsync("green");
            await ctx.FillRectAsync(10, 100, 100, 100);

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
