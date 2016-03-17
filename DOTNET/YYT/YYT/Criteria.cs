using System;
using YYT.Serialization.Mobile;
using YYT.Core;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using YYT.Query;
using System.Collections.Generic;
using System.Collections;
#if SILVERLIGHT
using YYT.Serialization;
#endif

namespace YYT
{
    /// <summary>
    /// Base type from which Criteria classes can be
    /// derived in a business class. 
    /// </summary>
    [Serializable]
    public class Criteria<T> : ManagedObjectBase,
      ICriteria<T>
    {
        #region Fields

        private static bool _forceInit = false;
        /// <summary>
        /// Defines the TypeName property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<string> TypeNameProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<string>("TypeName"));

        [NonSerialized]
        [NotUndoable]
        private Type _objectType;

        CriteriaProvider provider;
        Dictionary<string, List<Expression>> expressionDictionary;

        #endregion

        #region Property

        /// <summary>
        /// Type of the business object to be instantiated by
        /// the server-side DataPortal. 
        /// </summary>
        public Type ObjectType
        {
            get
            {
                if (_objectType == null && FieldManager.FieldExists(TypeNameProperty))
                {
                    string typeName = ReadProperty(TypeNameProperty);
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        _objectType = Type.GetType(typeName, false);
                    }
                }
                return _objectType;
            }
        }

        /// <summary>
        /// Assembly qualified type name of the business 
        /// object to be instantiated by
        /// the server-side DataPortal. 
        /// </summary>
        public string TypeName
        {
            get { return ReadProperty(TypeNameProperty); }
            protected set { LoadProperty(TypeNameProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<bool> IsCacheProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<bool>("IsCache"));

        public bool IsCache
        {
            get { return ReadProperty(IsCacheProperty); }
            set { LoadProperty(IsCacheProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<bool> IsTransactionProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<bool>("IsTransaction"));

        public bool IsTransaction
        {
            get { return ReadProperty(IsTransactionProperty); }
            set { LoadProperty(IsTransactionProperty, value); }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<int> PageSizeProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<int>("PageSize"));

        public int PageSize
        {
            get { return ReadProperty(PageSizeProperty); }
            set { LoadProperty(PageSizeProperty, value); }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<int> PageIndexProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<int>("PageIndex"));

        public int PageIndex
        {
            get { return ReadProperty(PageIndexProperty); }
            set { LoadProperty(PageIndexProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<int> CountProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<int>("Count"));

        public int Count
        {
            get { return ReadProperty(CountProperty); }
            set { LoadProperty(CountProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty(
          typeof(Criteria<>),
          new PropertyInfo<string>("Name"));

        public string Name
        {
            get { return ReadProperty(NameProperty); }
            set { LoadProperty(NameProperty, value); }
        }

        #endregion

#if SILVERLIGHT
    /// <summary>
    /// Creates an instance of the object. For use by
    /// MobileFormatter only - you must provide a 
    /// Type parameter in your code.
    /// </summary>
    [Obsolete("For use by MobileFormatter only")]
    public Criteria<>()
    {
      _forceInit = _forceInit && false;
    }

    /// <summary>
    /// Method called by MobileFormatter when an object
    /// should be deserialized. The data should be
    /// deserialized from the SerializationInfo parameter.
    /// </summary>
    /// <param name="info">
    /// Object containing the serialized data.
    /// </param>
    /// <param name="mode">Serialization mode.</param>
    protected override void OnSetState(SerializationInfo info, StateMode mode)
    {
      _forceInit = _forceInit && false;
      base.OnSetState(info, mode);
    }
#else
        /// <summary>
        /// Initializes empty Criteria<>. The type of
        /// business object to be created by the DataPortal
        /// MUST be supplied by the subclass.
        /// </summary>
        public Criteria()
        {
            provider = new CriteriaProvider();
            expressionDictionary = new Dictionary<string, List<Expression>>();
        }

        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserializedHandler(System.Runtime.Serialization.StreamingContext context)
        {
            OnDeserialized(context);
        }

        /// <summary>
        /// This method is called on a newly deserialized object
        /// after deserialization is complete.
        /// </summary>
        /// <param name="context">Serialization context object.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            _forceInit = _forceInit && false;
            if (FieldManager != null)
                FieldManager.SetPropertyList(this.GetType());
        }

#endif

        /// <summary>
        /// Initializes Criteria<> with the type of
        /// business object to be created by the DataPortal.
        /// </summary>
        /// <param name="type">The type of the
        /// business object the data portal should create.</param>
        protected Criteria(Type type)
        {
            _objectType = type;
            TypeName = type.AssemblyQualifiedName;
        }

        #region interface Methods

        public ICriteria<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return AddExpression("Where", predicate);
        }

        public ICriteria<T> OrderBy<K>(System.Linq.Expressions.Expression<Func<T, K>> predicate)
        {
            return AddExpression("OrderBy", predicate);
        }

        public ICriteria<T> OrderByDescending<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("OrderByDescending", predicate);
        }

        public ICriteria<T> ThenBy<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("ThenBy", predicate);
        }

        public ICriteria<T> ThenByDescending<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("ThenByDescending", predicate);
        }

        public ICriteria<T> Skip(int count)
        {
            ConstantExpression constSkip = Expression.Constant(count, typeof(Int32));
            return AddExpression("Skip", constSkip);
        }

        public ICriteria<T> Take(int count)
        {
            ConstantExpression constSkip = Expression.Constant(count, typeof(Int32));
            return AddExpression("Take", constSkip);
        }

        public ICriteria<T> First()
        {
            throw new NotImplementedException();
        }

        public ICriteria<T> First(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("First", predicate);
        }

        public ICriteria<T> Distinct<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Distinct", predicate);
        }

        public ICriteria<T> All(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("All", predicate);
        }

        public ICriteria<T> Any(Expression<Func<T, bool>> predicate)
        {
            return AddExpression("Any", predicate);
        }

        public ICriteria<T> GroupBy<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("GroupBy", predicate);
        }

        public ICriteria<T> Max<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Max", predicate);
        }

        public ICriteria<T> Min<K>(Expression<Func<T, K>> predicate)
        {
            return AddExpression("Min", predicate);
        }
        public Dictionary<string, List<Expression>> ExpressionDictionary
        {
            get { return this.expressionDictionary; }
        }


        public ICriteriaProvider Provider
        {
            get
            {
                return this.provider;
            }

        }

        #endregion

        #region Assistant Methods

        private ICriteria<T> AddExpression(string key, Expression predicate)
        {
            List<Expression> expressionList = null;
            if (!(expressionDictionary.TryGetValue(key, out expressionList)))
            {
                lock (expressionDictionary)
                {
                    if (!(expressionDictionary.TryGetValue(key, out expressionList)))
                    {
                        expressionList = new List<Expression>() { predicate };
                        this.expressionDictionary.Add(key, expressionList);
                    }
                }
            }
            else
            {
                lock (expressionDictionary)
                {
                    expressionList = this.expressionDictionary[key];
                    expressionList.Add(predicate);
                    this.expressionDictionary[key] = expressionList;
                }
            }

            return this;
        }
        #endregion
    }
}
