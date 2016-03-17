using System.Collections;

namespace lite
{
	public sealed class ProviderManager
	{
		public static readonly ProviderManager Instance = new ProviderManager();
		
		private string defaultKey = "";
		private Hashtable items = new Hashtable(1, (float)1);
		
		private ProviderManager()
		{}
		
		public void Register(string key, IProvider provider)
		{
			items.Add(key, provider);
		}
		
		public void Unregister(string key)
		{
			if (key == null)
				return;
			items.Remove(key);
			if (key.Equals(defaultKey))
				defaultKey = "";
		}
		
		public string DefaultKey
		{
			get { return defaultKey; }
			set
			{
				if (value == null || value.Length == 0)
					defaultKey = "";
				else if (items.ContainsKey(value))
					defaultKey = value;
			}
		}
		
		public IProvider Get(string key)
		{
			return (IProvider) items[key];
		}
		
		public IProvider Get()
		{
			if (defaultKey == null || defaultKey.Length == 0)
				return (IProvider) null;
			return Get(defaultKey);
		}
	}
}
