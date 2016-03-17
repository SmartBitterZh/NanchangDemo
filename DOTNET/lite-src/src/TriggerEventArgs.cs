using System;

namespace lite
{
	public class TriggerEventArgs : EventArgs
	{
		private IDb db;
		private Timing timing = Timing.None;
		
		public TriggerEventArgs(IDb db, Timing timing) : base()
		{
			this.db = db;
			this.timing = timing;
		}
		
		public IDb Db
		{
			get { return db; }
		}
		
		public Timing Timing
		{
			get { return timing; }
		}
	}
}
