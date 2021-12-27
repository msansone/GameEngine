using System.Drawing;

namespace FiremelonEditor2
{
    public enum OverlayColorScheme
    {
        Standard = 0
    }

    public interface IOverlayGenerator
    {
        Bitmap GenerateOverlay(int width, int height, Point2D origin);

        Bitmap OverlayImage { get; }

        OverlayColorScheme OverlayColorScheme { get; set; }

        void Regenerate();
    }
}
