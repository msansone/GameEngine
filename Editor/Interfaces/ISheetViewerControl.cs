namespace FiremelonEditor2
{
    public interface ISheetViewerControl
    {
        ISheetDtoProxy Sheet { get; set; }

        void RefreshImage();
    }
}
