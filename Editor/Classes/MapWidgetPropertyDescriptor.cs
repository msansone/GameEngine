using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;

namespace FiremelonEditor2
{
    public class MapWidgetPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDto property_;
        private IProjectController projectController_;

        public MapWidgetPropertyDescriptor(ref PropertyDto property, Attribute[] attrs, IProjectController projectController)
            : base(property.Name, attrs)
        {
            property_ = property;
            projectController_ = projectController;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return null;
            }
        }

        public override object GetValue(object component)
        {
            return property_.Value;
        }

        public override string Description
        {
            get
            {
                return property_.Name;
            }
        }

        public override string DisplayName
        {
            get
            {
                return property_.Name;
            }

        }
        public override bool IsReadOnly
        {
            get
            {
                return property_.ReadOnly;
            }
        }

        public override void ResetValue(object component)
        {
            //Have to implement
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            projectController_.SetMapWidgetPropertyValue(property_.OwnerId, property_.Id, value);                
        }

        public override Type PropertyType
        {
            get { return property_.Value.GetType(); }
        }

        #endregion

        private void OnPropertyValueChanged(PropertyChangedEventArgs e)
        {

        }
    }
}
