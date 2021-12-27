using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class NameValidator : INameValidator
    {
        #region Constructors

        public NameValidator(INameGenerator nameGenerator)
        {
            nameGenerator_ = nameGenerator;
        }

        #endregion

        #region Private Variables

        INameGenerator nameGenerator_;

        #endregion

        #region Public Functions

        public void ValidateAssetName(Guid id, ProjectDto project, string name)
        {
            // Valid python names are a letter or underscore character, followed by
            // an unlimited series of letters, digits, or underscores.
            // See: https://docs.python.org/2/reference/lexical_analysis.html#identifiers

            if (string.IsNullOrWhiteSpace(name) == true)
            {
                throwException(NameValidatorResult.IsBlank);
            }

            string validFirstChars = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXY";
            string validChars = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXY0123456789";

            bool isFirstChar = true;

            foreach (char c in name)
            {
                if (isFirstChar)
                {
                    if (validFirstChars.Contains(c) == false)
                    {
                        throwException(NameValidatorResult.InvalidFirstChar);
                    }
                }
                else
                {
                    if (validChars.Contains(c) == false)
                    {
                        throwException(NameValidatorResult.ContainsInvalidChar);
                    }
                }

                isFirstChar = false;
            }

            if (nameGenerator_.IsAssetNameInUse(id, project, name) == true)
            {
                throwException(NameValidatorResult.NameInUse);
            }
        }

        public void ValidateMapWidgetName(string name)
        {
            if (nameGenerator_.IsMapWidgetNameInUse(name) == true)
            {
                throwException(NameValidatorResult.NameInUse);
            }
        }

        #endregion

        #region Private Variables

        private void throwException(NameValidatorResult result)
        {
            switch (result)
            {
                case NameValidatorResult.IsBlank:
                    throw new InvalidNameException("Name cannot be blank.");

                case NameValidatorResult.InvalidFirstChar:
                    throw new InvalidNameException("The first character of the name is invalid.");

                case NameValidatorResult.ContainsInvalidChar:
                    throw new InvalidNameException("The name contains an invalid character");

                case NameValidatorResult.NameInUse:
                    throw new InvalidNameException("Name is already in use.");
            }
        }

        #endregion
    }
}
