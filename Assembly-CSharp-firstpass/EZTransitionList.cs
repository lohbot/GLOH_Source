using System;

[Serializable]
public class EZTransitionList
{
	public EZTransition[] list = new EZTransition[0];

	public EZTransitionList(EZTransition[] l)
	{
		this.list = l;
	}

	public EZTransitionList()
	{
		this.list = new EZTransition[0];
	}

	public void Clone(int source, bool force)
	{
		if (source >= this.list.Length)
		{
			return;
		}
		EZTransition eZTransition = this.list[source];
		if (!force && eZTransition.animationTypes.Length < 1)
		{
			return;
		}
		for (int i = 0; i < this.list.Length; i++)
		{
			if (i != source && (force || !this.list[i].initialized))
			{
				this.list[i].Copy(eZTransition);
			}
		}
	}

	public void CloneAsNeeded(int source)
	{
		this.Clone(source, false);
	}

	public void CloneAll(int source)
	{
		this.Clone(source, true);
	}

	public void MarkAllInitialized()
	{
		for (int i = 0; i < this.list.Length; i++)
		{
			this.list[i].initialized = true;
		}
	}

	public string[] GetTransitionNames()
	{
		if (this.list == null)
		{
			return null;
		}
		string[] array = new string[this.list.Length];
		for (int i = 0; i < this.list.Length; i++)
		{
			array[i] = this.list[i].name;
		}
		return array;
	}

	public void CopyTo(EZTransitionList target)
	{
		this.CopyTo(target, false);
	}

	public void CopyTo(EZTransitionList target, bool copyInit)
	{
		if (target == null)
		{
			return;
		}
		if (target.list == null)
		{
			return;
		}
		int num = 0;
		while (num < this.list.Length && num < target.list.Length)
		{
			if (target.list[num] != null)
			{
				target.list[num].Copy(this.list[num]);
				if (copyInit)
				{
					target.list[num].initialized = this.list[num].initialized;
				}
			}
			num++;
		}
	}

	public void CopyToNew(EZTransitionList target)
	{
		this.CopyToNew(target, false);
	}

	public void CopyToNew(EZTransitionList target, bool copyInit)
	{
		if (target == null)
		{
			return;
		}
		if (target.list == null)
		{
			return;
		}
		target.list = new EZTransition[this.list.Length];
		for (int i = 0; i < target.list.Length; i++)
		{
			target.list[i] = new EZTransition(this.list[i].name);
		}
		this.CopyTo(target, copyInit);
	}
}
