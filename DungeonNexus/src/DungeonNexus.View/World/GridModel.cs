using Blazor.Extensions.Canvas.Canvas2D;
using DungeonNexus.View.Components;
using System.Drawing;
using System.Threading.Tasks;

namespace DungeonNexus.View.World
{
    class GridModel
    {
        private readonly int width;
        private readonly int height;
        private readonly int size;
        private RgbColor[,] filled;

        public GridModel(int width, int height, int size)
        {
            this.width = width;
            this.height = height;
            this.size = size;
            filled = new RgbColor[width, height];

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    filled[i, j] = RgbColor.White;
        }

        public async Task Draw(Canvas2DContext context)
        {
            await context.BeginPathAsync();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    await DrawSquareOutline(context, x, y);
                }
            }

            await context.StrokeAsync();
        }

        public async Task ToogleSquare(Canvas2DContext context, double mouseX, double mouseY, RgbColor fillColor)
        {
            var position = MouseToGrid(mouseX, mouseY);
            if (IsInGrid(position))
            {
                if (filled[position.X, position.Y] == fillColor)
                {
                    await ClearSquare(context, position);
                    filled[position.X, position.Y] = RgbColor.White;
                }
                else
                {
                    await DrawSquare(context, position, fillColor);
                    filled[position.X, position.Y] = fillColor;
                }
            }
        }

        private async Task DrawSquareOutline(Canvas2DContext context, int x, int y)
        {
            await context.MoveToAsync(x * size, y * size);
            await context.LineToAsync((x + 1) * size, y * size);
            await context.LineToAsync((x + 1) * size, (y + 1) * size); 
            await context.LineToAsync(x * size, (y + 1) * size);
            await context.LineToAsync(x * size, y * size);
        }

        private async Task ClearSquare(Canvas2DContext context, Point point)
        {
            await context.ClearRectAsync(point.X * size + 1, point.Y * size + 1, size - 2, size - 2);
        }

        private async Task DrawSquare(Canvas2DContext context, Point point, RgbColor fillColor)
        {
            await context.SetFillColorAsync(fillColor);
            await context.FillRectAsync(point.X * size + 1, point.Y * size + 1, size - 2, size - 2);
        }

        private Point MouseToGrid(double mouseX, double mouseY)
        {
            return new((int)(mouseX / size), (int)(mouseY / size));
        }

        private bool IsInGrid(Point point)
        {
            return point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
        }
    }
}
