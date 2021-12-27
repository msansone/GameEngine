namespace FiremelonEditor2
{
    public enum GrabberDirection
    {
        None = -1,
        NorthWest = 0,
        North = 1,
        NorthEast = 2,
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7
    };

    public interface IResizableMapWidgetController : IMapWidgetController
    {
        GrabberDirection GetSizeMode(System.Drawing.Point cursorLocation);
    }
}
