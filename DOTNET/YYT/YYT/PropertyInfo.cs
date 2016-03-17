using System;

namespace YYT
{
    /// <summary>
    /// Maintains metadata about a property.
    /// </summary>
    /// <typeparam name="T">
    /// Data type of the property.
    /// </typeparam>
    public class PropertyInfo<T> : Core.IPropertyInfo, IComparable
    {
        #region Fields

        private string _name;
        private string _friendlyName;
        private T _defaultValue;
        private int _index = -1;

        #endregion

        #region Property

        /// <summary>
        /// Gets the property name value.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets or sets the index position for the managed
        /// field storage behind the property. FOR
        /// INTERNAL YYT .NET USE ONLY.
        /// </summary>
        public int Index
        {
            get
            {
                //if (_index == -1)
                //  throw new InvalidOperationException(string.Format(Resources.UnRegisteredPropertyException, _name));
                return _index;
            }
            set
            {
                _index = value;
            }
        }


        /// <summary>
        /// Gets the friendly display name
        /// for the property.
        /// </summary>
        /// <remarks>
        /// If no friendly name was provided, the
        /// property name itself is returned as a
        /// result.
        /// </remarks>
        public virtual string FriendlyName
        {
            get
            {
                if (!(string.IsNullOrEmpty(_friendlyName)))
                {
                    return _friendlyName;

                }
                else
                {
                    return _name;
                }
            }
        }

        /// <summary>
        /// Gets the default initial value for the property.
        /// </summary>
        /// <remarks>
        /// This value is used to initialize the property's
        /// value, and is returned from a property get
        /// if the user is not authorized to 
        /// read the property.
        /// </remarks>
        public virtual T DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        object Core.IPropertyInfo.DefaultValue
        {
            get
            {
                return DefaultValue;
            }
        }

        Core.FieldManager.IFieldData Core.IPropertyInfo.NewFieldData(string name)
        {
            return NewFieldData(name);
        }

        public Type DataEntityTyoe
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string DataEntityPropertyName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        public PropertyInfo(string name)
            : this(name, "")
        { }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="friendlyName">
        /// Friendly display name for the property.
        /// </param>
        public PropertyInfo(string name, string friendlyName)
        {
            _name = name;
            _friendlyName = friendlyName;
            if (typeof(T).Equals(typeof(string)))
                _defaultValue = (T)((object)string.Empty);
            else
                _defaultValue = default(T);
        }


        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="friendlyName">
        /// Friendly display name for the property.
        /// </param>
        /// <param name="defaultValue">
        /// Default value for the property.
        /// </param>
        public PropertyInfo(string name, string friendlyName, T defaultValue)
        {
            _name = name;
            _defaultValue = defaultValue;
            _friendlyName = friendlyName;
        }

        #endregion

        #region Assistant Methods

        /// <summary>
        /// Create and return a new IFieldData object
        /// to store an instance value for this
        /// property.
        /// </summary>
        /// <param name="name">
        /// Property name.
        /// </param>
        protected virtual Core.FieldManager.IFieldData NewFieldData(string name)
        {
            return new Core.FieldManager.FieldData<T>(name);
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            return _name.CompareTo(((Core.IPropertyInfo)obj).Name);
        }

        #endregion

    }
}
