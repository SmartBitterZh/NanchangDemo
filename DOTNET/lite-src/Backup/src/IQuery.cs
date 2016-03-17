using System.Collections;

namespace lite
{
	public interface IQuery
	{
		IQuery And();
		IQuery Or();
		
		IQuery Constrain(string column);
		IQuery Constrain(IQuery query);
		
		IQuery Equal(object val);
		IQuery NotEqual(object val);
		IQuery Greater(object val);
		IQuery GreaterEqual(object val);
		IQuery Less(object val);
		IQuery LessEqual(object val);
		IQuery Like(string val);
		
		IQuery In(IList values);
		IQuery NotIn(IList values);
		
		IQuery Order(string column, bool ascending);
		
		IConstraint Get(int index);
	}
}
