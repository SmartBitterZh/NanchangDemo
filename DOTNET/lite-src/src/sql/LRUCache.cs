using System;
using System.Collections.Generic;

namespace lite.sqlserver
{
	public class LRUCache<T>
	{
		private Dictionary<string, LinkedListNode<T>> dict;
		private LinkedList<T> list;
		private int size = 0;
		
		public LRUCache(int sz)
		{
			size = sz < 10 ? 10 : sz;
			dict = new Dictionary<string, LinkedListNode<T>>(size);
			list = new LinkedList<T>();
		}
		
		public int Size
		{
			get { return size; }
			set { size = value < 10 ? 10 : value; }
		}
		
		public void Put(string key, T item)
		{
			LinkedListNode<T> node;
			if(dict.TryGetValue(key, out node))
			{
				list.Remove(node);
				node = new LinkedListNode<T>(item);
				dict[key] = node;
				list.AddFirst(node);
			}
			else
			{
				if(list.Count == size)
					list.RemoveLast();
				node = new LinkedListNode<T>(item);
				dict[key] = node;
				list.AddFirst(node);
			}
		}
		
		public T Get(string key)
		{
			LinkedListNode<T> node;
			if(dict.TryGetValue(key, out node))
			{
				list.Remove(node);
				list.AddFirst(node);
				return node.Value;
			}
			return default(T);
		}
	}
}
