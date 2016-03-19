using System;

public class EZLinkedListIterator<T> where T : IEZLinkedListItem<T>
{
	protected T cur;

	protected EZLinkedList<T> list;

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

	public bool Done
	{
		get
		{
			return this.cur == null;
		}
	}

	public bool Begin(EZLinkedList<T> l)
	{
		this.list = l;
		this.cur = l.Head;
		return this.cur == null;
	}

	public void End()
	{
		this.list.End(this);
	}

	public bool Next()
	{
		if (this.cur != null)
		{
			this.cur = this.cur.next;
		}
		if (this.cur == null)
		{
			this.list.End(this);
			return false;
		}
		return true;
	}

	public bool NextNoRemove()
	{
		if (this.cur != null)
		{
			this.cur = this.cur.next;
		}
		return this.cur != null;
	}
}
