using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionDemo
{
    public class CriteriaTranslator : ExpressionVisitor
    {
        #region Fields

        StringBuilder sb;
        Dictionary<string, string> resultDictionary = null;
        private Type objectType;

        #endregion

        #region Constructor

        internal CriteriaTranslator(Type type)
        {
            objectType = type;
        }

        #endregion

        #region Main Methods

        internal Dictionary<string, string> Translate(Dictionary<string, List<Expression>> expressionDictionary)
        {
            resultDictionary = new Dictionary<string, string>();

            foreach (var expressionKeyValePair in expressionDictionary)
            {
                this.sb = new StringBuilder();
                foreach (Expression expression in expressionKeyValePair.Value)
                {
                    this.Visit(Evaluator.PartialEval(expression));
                }
                AddExpressionResult(expressionKeyValePair.Key, sb.ToString());

            }
            return resultDictionary;
        }

        #endregion

        #region Override Methods

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(string))
            {
                switch (m.Method.Name)
                {
                    case "StartsWith":
                        sb.Append("(");
                        this.Visit(m.Object);
                        return m;
                    case "Contains":
                        sb.Append("(");
                        this.Visit(m.Object);
                        sb.Append(" LIKE '%' + ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" + '%')");
                        return m;
                }
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }
            else
                throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));

        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
                sb.Append("SELECT * FROM ");
                sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        #endregion

        #region Assistant Methods

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }


        private void AddExpressionResult(string key, string value)
        {
            string tempValue = string.Empty;
            if (!resultDictionary.TryGetValue(key, out tempValue))
            {
                lock (resultDictionary)
                {
                    if (!resultDictionary.TryGetValue(key, out tempValue))
                    {
                        resultDictionary.Add(key, value);
                    }
                }
            }
        }

        #endregion
    }
}
