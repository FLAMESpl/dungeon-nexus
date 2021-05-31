using Blazor.Extensions.Canvas.Canvas2D;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace DungeonNexus.View.World
{
    class GridModel
    {
        private readonly int width;
        private readonly int height;
        private readonly int size;
        private bool[,] filled;

        public GridModel(int width, int height, int size)
        {
            this.width = width;
            this.height = height;
            this.size = size;
            filled = new bool[width, height];
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

        public async Task ToogleSquare(Canvas2DContext context, double mouseX, double mouseY)
        {
            var position = MouseToGrid(mouseX, mouseY);
            if (IsInGrid(position))
            {
                if (filled[position.X, position.Y])
                {
                    await ClearSquare(context, position);
                    filled[position.X, position.Y] = false;
                }
                else
                {
                    await DrawSquare(context, position);
                    filled[position.X, position.Y] = true;
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

        private async Task DrawSquare(Canvas2DContext context, Point point)
        {
            await context.SetFillStyleAsync("black");
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
