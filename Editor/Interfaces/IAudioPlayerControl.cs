namespace FiremelonEditor2
{
    public interface IAudioPlayerControl
    {
        IAudioAssetDtoProxy AudioAsset { get; set; }

        void Stop();
    }
}
