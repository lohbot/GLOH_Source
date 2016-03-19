using System;
using System.Collections.Generic;

public class EZLinkedList<T> where T : IEZLinkedListItem<T>
{
	private List<EZLinkedListIterator<T>> iters = new List<EZLinkedListIterator<T>>();

	private List<EZLinkedListIterator<T>> freeIters = new List<EZLinkedListIterator<T>>();

	protected T head;

	protected T cur;

	protected T nextItem;

	protected int count;

	public int Count
	{
		get
		{
			return this.count;
		}
	}

	public bool Empty
	{
		get
		{
			return this.head == null;
		}
	}

	public T Head
	{
		get
		{
			return this.head;
		}
	}

	public T Current
	{
		get
		{
			return this.cur;
		}
		set
		{
			this.cur = value;
		}
	}

	public EZLinkedListIterator<T> Begin()
	{
		EZLinkedListIterator<T> eZLinkedListIterator;
		if (this.freeIters.Count > 0)
		{
			eZLinkedListIterator = this.freeIters[this.freeIters.Count - 1];
			this.freeIters.RemoveAt(this.freeIters.Count - 1);
		}
		else
		{
			eZLinkedListIterator = new EZLinkedListIterator<T>();
		}
		this.iters.Add(eZLinkedListIterator);
		eZLinkedListIterator.Begin(this);
		return eZLinkedListIterator;
	}

	public void End(EZLinkedListIterator<T> it)
	{
		if (this.iters.Remove(it))
		{
			this.freeIters.Add(it);
		}
	}

	public bool Rewind()
	{
		this.cur = this.head;
		if (this.cur != null)
		{
			this.nextItem = this.cur.next;
			return true;
		}
		this.nextItem = default(T);
		return false;
	}

	public bool MoveNext()
	{
		this.cur = this.nextItem;
		if (this.cur != null)
		{
			this.nextItem = this.cur.next;
		}
		return this.cur != null;
	}

	public void Add(T item)
	{
		if (this.head != null)
		{
			item.next = this.head;
			this.head.prev = item;
		}
		this.head = item;
		this.count++;
	}

	public void Remove(T item)
	{
		if (this.head == null || item == null)
		{
			return;
		}
		if (this.head.Equals(item))
		{
			this.head = item.next;
			if (this.iters.Count > 0)
			{
				for (int i = 0; i < this.iters.Count; i++)
				{
					if (this.iters[i].Current != null)
					{
						T current = this.iters[i].Current;
						if (current.Equals(item))
						{
							this.iters[i].Current = item.next;
						}
					}
				}
			}
		}
		else
		{
			if (this.iters.Count > 0)
			{
				for (int j = 0; j < this.iters.Count; j++)
				{
					if (this.iters[j].Current != null)
					{
						T current2 = this.iters[j].Current;
						if (current2.Equals(item))
						{
							this.iters[j].Current = item.prev;
						}
					}
				}
			}
			if (item.next != null)
			{
				T prev = item.prev;
				prev.next = item.next;
				T next = item.next;
				next.prev = item.prev;
			}
			else if (item.prev != null)
			{
				T prev2 = item.prev;
				prev2.next = default(T);
			}
		}
		item.next = default(T);
		item.prev = default(T);
		this.count--;
	}

	public void Clear()
	{
		if (this.head == null)
		{
			return;
		}
		this.cur = this.head;
		this.head = default(T);
		do
		{
			T next = this.cur.next;
			this.cur.prev = default(T);
			this.cur.next = default(T);
			this.cur = next;
		}
		while (this.cur != null);
	}
}
