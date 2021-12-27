namespace FiremelonEditor2
{
    public interface ITileSheetViewerControl : ISheetViewerControl
    {
        event TileSheetSelectionChangedHandler TileSheetSelectionChanged;

        TileObjectDto GetSelectionAsObject();
    }
}
