using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;

[Synchronization]
public class NrUpdateProcessor : NrTSingleton<NrUpdateProcessor>
{
	private Dictionary<string, UpdateFunc> m_hashWork;

	private Dictionary<string, UpdateFunc> m_hashUpdate;

	private NrUpdateProcessor()
	{
		this.m_hashWork = new Dictionary<string, UpdateFunc>();
		this.Clone();
	}

	~NrUpdateProcessor()
	{
	}

	private void Clone()
	{
		this.m_hashUpdate = new Dictionary<string, UpdateFunc>(this.m_hashWork);
	}

	private string GetDelegateKey(UpdateFunc updateFunc)
	{
		return string.Format("{0}:{1}:", updateFunc.Target, updateFunc.Method.Name);
	}

	public void AddUpdate(UpdateFunc updateFunc)
	{
		string delegateKey = this.GetDelegateKey(updateFunc);
		if (!this.m_hashWork.ContainsKey(delegateKey))
		{
			this.m_hashWork.Add(delegateKey, updateFunc);
		}
		this.Clone();
	}

	public void DellUpdate(UpdateFunc updateFunc)
	{
		string delegateKey = this.GetDelegateKey(updateFunc);
		if (this.m_hashWork.ContainsKey(delegateKey))
		{
			this.m_hashWork.Remove(delegateKey);
		}
		this.Clone();
	}

	public void MainUpdate()
	{
		foreach (UpdateFunc current in this.m_hashUpdate.Values)
		{
			current();
		}
	}
}
