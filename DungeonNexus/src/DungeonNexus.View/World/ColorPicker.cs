using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using DungeonNexus.View.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonNexus.View.World
{
    public partial class ColorPicker
    {
        [AllowNull] private BECanvasComponent paletteCanvas;
        [AllowNull] private BECanvasComponent selectorCanvas;
        [AllowNull] private Canvas2DContext selectorContext;

        private static readonly int[] colorValues = { 0, 127, 255 };
        private const int colorBoxSize = 30;
        private const int colorPickerRadius = colorBoxSize / 2;
        private const int colorsInARow = 4;
        private const int margin = 5;
        private bool initialized = false;

        private readonly List<(int X, int Y, RgbColor Color)?> colorPickerPositions = new();

        public static int Width { get; } = (colorBoxSize + margin) * colorsInARow + margin;
        public static int Height { get; } = 500;

        [Parameter] public RgbColor Value { get; set; } = new RgbColor(0, 0, 0);
        [Parameter] public EventCallback<RgbColor> ValueChanged { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                selectorContext = await selectorCanvas.CreateCanvas2DAsync();
                await RenderPalette();
                await SelectColor(0, 0);
                initialized = true;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RenderPalette()
        {
            var context = await paletteCanvas.CreateCanvas2DAsync();

            var x = 0;
            var y = 0;

            foreach (var color in GetPaletteColors())
            {
                colorPickerPositions.Add((x, y, color));

                await context.BeginPathAsync();
                await context.ArcAsync(
                    x: GetXOrigin(x),
                    y: GetYOrigin(y),
                    radius: colorPickerRadius,
                    startAngle: 0,
                    endAngle: Math.PI * 2);

                await context.SetFillColorAsync(color);
                await context.ClosePathAsync();
                await context.FillAsync();
                await context.StrokeAsync();

                if (x < colorsInARow - 1)
                {
                    x++;
                }
                else
                {
                    x = 0;
                    y++;
                }
            }
        }

        private async Task SelectColor(int x, int y)
        {
            await selectorContext.ClearRectAsync(0, 0, Width, Height);
            await selectorContext.BeginPathAsync();
            await selectorContext.ArcAsync(
                x: GetXOrigin(x),
                y: GetYOrigin(y),
                radius: colorPickerRadius,
                startAngle: 0,
                endAngle: Math.PI * 2);

            await selectorContext.SetLineWidthAsync(4);
            await selectorContext.StrokeAsync();
        }

        private static int GetXOrigin(int x) => (margin + colorBoxSize) * x + colorBoxSize / 2 + margin;
        private static int GetYOrigin(int y) => (margin + colorBoxSize) * y + colorBoxSize / 2 + margin;

        private static IEnumerable<RgbColor> GetPaletteColors()
        {
            var colors = new HashSet<RgbColor>();

            for (var red = 0; red < colorValues.Length; red++)
            {
                for (var green = 0; green < colorValues.Length; green++)
                {
                    for (var blue = 0; blue < colorValues.Length; blue++)
                    {
                        colors.Add(new(colorValues[red], colorValues[green], colorValues[blue]));
                    }
                }
            }

            return colors;
        }

        private async Task HandleClick(MouseEventArgs mouseEventArgs)
        {
            if (initialized)
            {
                var clicked = colorPickerPositions
                    .FirstOrDefault(x => x.HasValue && IsInsidePicker(
                        mouseEventArgs.OffsetX,
                        mouseEventArgs.OffsetY,
                        GetXOrigin(x.Value.X),
                        GetYOrigin(x.Value.Y)));

                if (clicked is { } pickerPosition)
                {
                    await SelectColor(pickerPosition.X, pickerPosition.Y);
                    Value = pickerPosition.Color;
                    await ValueChanged.InvokeAsync(Value);
                }
            }
        }

        private static bool IsInsidePicker(double mouseX, double mouseY, double x, double y)
        {
            var result = Math.Pow(mouseX - x, 2) + Math.Pow(mouseY - y, 2) - Math.Pow(colorPickerRadius, 2);
            return result <= 0;
        }
    }
}
