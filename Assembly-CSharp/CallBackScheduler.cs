using System;
using System.Collections.Generic;
using UnityEngine;

public class CallBackScheduler : MonoBehaviour
{
	private class CallBackSchedule
	{
		private long _RegTime;

		private long _CallTime;

		private Action _CallBackFunc;

		public CallBackSchedule(long regtime, long calltime, Action func)
		{
			this._RegTime = regtime;
			this._CallTime = calltime;
			this._CallBackFunc = func;
		}

		public bool IsAction(long now)
		{
			return now >= this._RegTime + this._CallTime;
		}

		public void Action()
		{
			if (this._CallBackFunc != null)
			{
				this._CallBackFunc();
			}
		}
	}

	private static CallBackScheduler _instance;

	private Queue<CallBackScheduler.CallBackSchedule> _Scheduler = new Queue<CallBackScheduler.CallBackSchedule>();

	public static CallBackScheduler Instance
	{
		get
		{
			if (CallBackScheduler._instance == null)
			{
				CallBackScheduler._instance = CallBackScheduler.GetInstance();
			}
			return CallBackScheduler._instance;
		}
	}

	private static CallBackScheduler GetInstance()
	{
		GameObject gameObject = GameObject.Find(typeof(CallBackScheduler).Name);
		if (gameObject == null)
		{
			gameObject = new GameObject(typeof(CallBackScheduler).Name, new Type[]
			{
				typeof(CallBackScheduler)
			});
		}
		CallBackScheduler component = gameObject.GetComponent<CallBackScheduler>();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		UnityEngine.Object.DontDestroyOnLoad(component);
		return component;
	}

	public void RegFunc(long millisecond, Action func)
	{
		CallBackScheduler.CallBackSchedule item = new CallBackScheduler.CallBackSchedule(DateTime.Now.Ticks / 10000L, millisecond, func);
		this._Scheduler.Enqueue(item);
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		int count = this._Scheduler.Count;
		for (int i = 0; i < count; i++)
		{
			CallBackScheduler.CallBackSchedule callBackSchedule = this._Scheduler.Dequeue();
			if (callBackSchedule.IsAction(DateTime.Now.Ticks / 10000L))
			{
				callBackSchedule.Action();
			}
			else
			{
				this._Scheduler.Enqueue(callBackSchedule);
			}
		}
		if (this._Scheduler.Count <= 0)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void Clear()
	{
		this._Scheduler.Clear();
	}
}
