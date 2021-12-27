namespace FiremelonEditor2
{
    public interface ITileSheetsEditorControl : IAssetsEditorControl
    {
        event TileSheetSelectionChangedHandler TileSheetSelectionChanged;

        void AddAnimation();

        void AddTileObject();

        void DeleteTileObject();

        void DeleteAnimation();
    }
}
