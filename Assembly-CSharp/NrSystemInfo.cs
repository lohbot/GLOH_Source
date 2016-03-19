using System;
using System.Collections.Generic;
using UnityEngine;

internal class NrSystemInfo : NrTSingleton<NrSystemInfo>
{
	private Dictionary<string, uint> MemorycheckList = new Dictionary<string, uint>();

	private NrSystemInfo()
	{
	}

	public void MemoryCheck_Start(string Name)
	{
		if (this.MemorycheckList.ContainsKey(Name))
		{
			this.MemorycheckList.Remove(Name);
		}
		this.MemorycheckList.Add(Name, Profiler.usedHeapSize);
	}

	public uint MemoryCheck_End(string Name)
	{
		if (!this.MemorycheckList.ContainsKey(Name))
		{
			return 0u;
		}
		uint num = Profiler.usedHeapSize - this.MemorycheckList[Name];
		Debug.Log(string.Concat(new string[]
		{
			"Used Memory - ",
			Name,
			": ",
			string.Format("{0:####,####,####,####}", Profiler.usedHeapSize),
			"(",
			string.Format("{0:####,####,####,####}", num),
			")"
		}));
		this.MemorycheckList.Remove(Name);
		return num;
	}

	public void PrintCurrentHeapSize(string name)
	{
		Debug.Log(" *** Print Current HeapSize - " + name + ": " + string.Format("{0:####,####,####,####}", Profiler.usedHeapSize));
	}
}
