using System;

namespace FiremelonEditor2
{
    public class BaseDto : IBaseDtoProxy
    {
        public BaseDto()
        {
        }

        private string name_ = string.Empty;
        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }

        private Guid id_ = Guid.NewGuid();
        public Guid Id
        {
            get { return id_; }
            set { id_ = value; }
        }

        private Guid ownerId_ = Guid.Empty;
        public Guid OwnerId
        {
            get { return ownerId_; }
            set { ownerId_ = value; }
        }

        private Guid rootOwnerId_ = Guid.Empty;
        public Guid RootOwnerId
        {
            get { return rootOwnerId_; }
            set { rootOwnerId_ = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
