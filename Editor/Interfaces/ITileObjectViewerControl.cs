namespace FiremelonEditor2
{
    public interface ITileObjectViewerControl
    {
        ITileObjectDtoProxy TileObject { get; set; }

        ITileSheetDtoProxy TileSheet { get; set; }
    }
}
