using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class PropertyDto : BaseDto
    {
        public PropertyDto()
		{
            readOnly_ = false;
            updateValue_ = false;
        }

        private string defaultValue_;
        public string DefaultValue
        {
            get { return defaultValue_; }
            set { defaultValue_ = value; }
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        private string name_;
		public string Name
		{
			get { return name_;	}
            set { name_ = value; }
		}

        [BrowsableAttribute(false)]
        private object value_;
        public object Value
        {
            get { return value_; }
            set
            {
                oldValue_ = value_;
                value_ = value;
                updateValue_ = false;
            }
        }

        [BrowsableAttribute(false)]
        private bool reserved_;
        public bool Reserved
        {
            get { return reserved_; }
            set { reserved_ = value; }
        }

        private object oldValue_;
        public object OldValue
        {
            get { return oldValue_; }            
        }

        [BrowsableAttribute(false)]
        private bool readOnly_;
        public bool ReadOnly
        {
            get { return readOnly_; }
            set { readOnly_ = value; }
        }

        [BrowsableAttribute(false)]
        private bool updateValue_;
        public bool UpdateValue
        {
            // The UpdateValue property exists so that when adding a new property,
            // after it gets added to all the object instances, it will also allow you
            // to fill in the value if you enter a default value.

            // If you change the property value, and THEN try to set the default, it
            // will know not to overwrite it. Also, any instances that were created after the new 
            // property was added will not have the property's default value overwritten, as it
            // should retain whatever the default value was at the time it was created.
            get { return updateValue_; }
            set { updateValue_ = value; }
        }

        private Type typeConverter_;
        public Type TypeConverter
        {
            get { return typeConverter_; }
            set { typeConverter_ = value; }
        }
    }
    public class PropertyDtoProxy : IPropertyDtoProxy
    {
        private IProjectController projectController_;
        private Guid propertyId_;

        public PropertyDtoProxy(IProjectController projectController, Guid propertyId)
        {
            projectController_ = projectController;
            propertyId_ = propertyId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                PropertyDto property = projectController_.GetProperty(propertyId_);

                return property.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Property name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    PropertyDto property = projectController_.GetProperty(propertyId_);

                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.Properties[property.OwnerId].Count; i++)
                    {
                        PropertyDto currentProperty = project.Properties[property.OwnerId][i];

                        if (value.ToUpper() == currentProperty.Name.ToUpper() && propertyId_ != currentProperty.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Property name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetPropertyName(propertyId_, value);
                }
            }
        }

        public string DefaultValue
        {
            get
            {
                PropertyDto property = projectController_.GetProperty(propertyId_);

                return property.DefaultValue;
            }
            set
            {
                projectController_.SetPropertyDefaultValue(propertyId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return propertyId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                PropertyDto property = projectController_.GetProperty(propertyId_);

                return property.OwnerId;
            }
        }
    }
}
