namespace FiremelonEditor2
{
    public interface ISheetUtility
    {
        void SplitImageIntoCells(BitmapResourceDto bitmap, int cols, int rows, int cellWidth, int cellHeight, float scaleFactor);
    }
}
