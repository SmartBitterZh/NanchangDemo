using System;

namespace YYT.Core
{
    /// <summary>
    /// Maintains metadata about a property.
    /// </summary>
    public interface IPropertyInfo
    {
        /// <summary>
        /// Gets the property name value.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// Gets the friendly display name
        /// for the property.
        /// </summary>
        string FriendlyName { get; }
        /// <summary>
        /// Mapping data entity type
        /// </summary>
        Type DataEntityTyoe { get; set; }
        /// <summary>
        /// Mapping to data entity name
        /// </summary> 
        string DataEntityPropertyName { get; set; }
        /// <summary>
        /// Gets the default initial value for the property.
        /// </summary>
        /// <remarks>
        /// This value is used to initialize the property's
        /// value, and is returned from a property get
        /// if the user is not authorized to 
        /// read the property.
        /// </remarks>
        object DefaultValue { get; }
        /// <summary>
        /// Gets a new field data container for the property.
        /// </summary>
        Core.FieldManager.IFieldData NewFieldData(string name);
        /// <summary>
        /// Gets or sets the index position for the managed
        /// field storage behind the property. FOR
        /// INTERNAL YYT .NET USE ONLY.
        /// </summary>
        int Index { get; set; }
    }
}
