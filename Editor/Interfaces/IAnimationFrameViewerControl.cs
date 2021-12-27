namespace FiremelonEditor2
{
    public interface IAnimationFrameViewerControl
    {
        IAnimationDtoProxy Animation { get; set; }

        int ActionPointIndex { get; set; }

        int FrameIndex { get; set; }

        int HitboxIndex { get; set; }

        void RefreshImage();
    }
}
