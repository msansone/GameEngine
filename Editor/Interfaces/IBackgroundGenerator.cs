using System.Drawing;

namespace FiremelonEditor2
{
    public enum BackgroundColorScheme
    {
        StandardLight = 0,
        StandardDark = 1,
        Vivid = 2
    }
    
    public interface IBackgroundGenerator
    {
        Bitmap GenerateBackground(int width, int height);
        
        Bitmap BackgroundImage { get; }

        BackgroundColorScheme BackgroundColorScheme { get; set; }
        
        void Regenerate();
    }
}
