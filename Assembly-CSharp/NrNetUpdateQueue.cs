using System;
using System.Collections;

public class NrNetUpdateQueue : NrTSingleton<NrNetUpdateQueue>
{
	private class NUpdataUnit
	{
		public UpdatePackFunc mUpdateFunc;

		public object mData;

		public NUpdataUnit(UpdatePackFunc updateFunc, object Data)
		{
			this.mUpdateFunc = updateFunc;
			this.mData = Data;
		}

		public void Do()
		{
			this.mUpdateFunc(this.mData);
		}
	}

	private const int UPDATE_COUNT = 2;

	private Queue mPacketQueue = new Queue();

	private NrNetUpdateQueue()
	{
		NrTSingleton<NrUpdateProcessor>.Instance.AddUpdate(new UpdateFunc(this.FixedUpdate));
	}

	public void Release()
	{
		NrTSingleton<NrUpdateProcessor>.Instance.DellUpdate(new UpdateFunc(this.FixedUpdate));
	}

	public void Insert(UpdatePackFunc updateFunc, object Data)
	{
		this.mPacketQueue.Enqueue(new NrNetUpdateQueue.NUpdataUnit(updateFunc, Data));
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.mPacketQueue.Count != 0)
			{
				NrNetUpdateQueue.NUpdataUnit nUpdataUnit = (NrNetUpdateQueue.NUpdataUnit)this.mPacketQueue.Dequeue();
				nUpdataUnit.Do();
			}
		}
	}
}
