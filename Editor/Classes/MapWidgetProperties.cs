using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections;

namespace FiremelonEditor2
{
    public class MapWidgetProperties : CollectionBase, ICustomTypeDescriptor, IMapWidgetProperties 
    {
        IProjectController projectController_;

        public MapWidgetProperties(IProjectController projectController)
        {
            projectController_ = projectController;
        }

        public void AddProperty(PropertyDto Value)
        {
            base.List.Add(Value);
        }

        public void InsertProperty(int index, PropertyDto Value)
        {
            base.List.Insert(index, Value);
        }

        public void RemoveProperty(string Name)
        {
            foreach (PropertyDto prop in base.List)
            {
                if (prop.Name == Name)
                {
                    base.List.Remove(prop);
                    return;
                }
            }
        }

        public void RollBackProperty(string Name)
        {
            foreach (PropertyDto prop in base.List)
            {
                if (prop.Name == Name)
                {
                    prop.Value = prop.OldValue;
                    return;
                }
            }
        }

        public PropertyDto this[int index]
        {
            get
            {
                return (PropertyDto)base.List[index];
            }
            set
            {
                base.List[index] = (PropertyDto)value;
            }
        }

        #region "TypeDescriptor Implementation"
        /// <summary>
        /// Get Class Name
        /// </summary>
        /// <returns>String</returns>
        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// GetAttributes
        /// </summary>
        /// <returns>AttributeCollection</returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// GetComponentName
        /// </summary>
        /// <returns>String</returns>
        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// GetConverter
        /// </summary>
        /// <returns>TypeConverter</returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// GetDefaultEvent
        /// </summary>
        /// <returns>EventDescriptor</returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// GetDefaultProperty
        /// </summary>
        /// <returns>PropertyDescriptor</returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// GetEditor
        /// </summary>
        /// <param name="editorBaseType">editorBaseType</param>
        /// <returns>object</returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                PropertyDto prop = (PropertyDto)this[i];
                
                //string categoryName = getCategoryName(prop.Name);
                string categoryName = string.Empty;
  
                if (prop.Reserved == true)
                {
                    categoryName = "Engine Defined Properties";
                }
                else
                {
                    categoryName = "User Defined Properties";
                }
            
                Attribute category = new System.ComponentModel.CategoryAttribute(categoryName);

                bool isBrowsable = getIsBrowsable(prop.Name);
                Attribute browsable = new System.ComponentModel.BrowsableAttribute(isBrowsable);

                //Attribute editor = new System.ComponentModel.EditorAttribute(typeof(ImageFilePathUiTypeEditor), typeof(UITypeEditor));

                List<Attribute> lstAttributes = new List<Attribute>();

                lstAttributes.Add(browsable);
                lstAttributes.Add(category);
                //lstAttributes.Add(editor);

                if (prop.TypeConverter != null)
                {
                    Attribute typeConverter = new System.ComponentModel.TypeConverterAttribute(prop.TypeConverter);
                    lstAttributes.Add(typeConverter);
                }


                Attribute[] finalAttributes = lstAttributes.ToArray();

                newProps[i] = new MapWidgetPropertyDescriptor(ref prop, finalAttributes, projectController_);

            }

            return new PropertyDescriptorCollection(newProps);
        }

        private string getCategoryName(string propertyName)
        {
            switch (propertyName.ToUpper())
            {
                case "POSITIONX":
                case "POSITIONY":
                case "BOXWIDTH":
                case "BOXHEIGHT":
                case "NAME":
                case "ACCEPTINPUT":
                case "ATTACHTOCAMERA":
                case "RENDERORDER":
                case "OWNERSHIP":

                    return "Engine Defined Properties";

                default:

                    return "User Defined Properties";
            }
        }

        private bool getIsBrowsable(string propertyName)
        {
            switch (propertyName.ToUpper())
            {
                case "NAME":
                case "ATTACHTOCAMERA":

                    //findme123
                    // Currently I don't have a better way to do this than with a global.
                    if (Globals.selectedEntityInstanceCount > 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                default:

                    return true;
            }
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion
    }
}
