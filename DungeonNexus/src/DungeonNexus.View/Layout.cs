namespace DungeonNexus.View
{
    internal static class Layout
    {
        public const string Container = "mat-layout-grid";
        public const string Inner = "mat-layout-grid-inner";
        public const string Cell = "mat-layout-grid-cell";
        public const string FixedColumnWidth = "mat-layout-grid-fixed-column-width";

        public static string CellSpan(int columnCount) => $"mat-layout-grid-cell-span-{columnCount}";
        public static string CellSpan(int columnCount, Device device) => $"{CellSpan(columnCount)}-{device.ToString().ToLower()}";
        public static string CellOrder(int index) => $"mat-layout-grid-cell-order-{index}";
        public static string CellAlign(Position position) => $"mat-layout-grid-cell-align-{position.ToString().ToLower()}";
        public static string Align(Alignment alignment) => $"mat-layout-grid-align-{alignment.ToString().ToLower()}";

    }
}
