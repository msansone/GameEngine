using System;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public enum NameValidatorResult
    {
        InvalidFirstChar = 0,
        ContainsInvalidChar = 1,
        IsBlank = 2,
        NameInUse = 3
    };

    public interface INameValidator
    {
        void ValidateAssetName(Guid assetId, ProjectDto project, string name);

        void ValidateMapWidgetName(string name);
    }
}
