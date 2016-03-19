using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class LineWebHelper : MonoBehaviour
{
	public delegate void HttpRequestDelegate(int id, WWW www);

	private int requestId;

	private static LineWebHelper current;

	private static GameObject container;

	public event LineWebHelper.HttpRequestDelegate OnHttpRequest
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.OnHttpRequest = (LineWebHelper.HttpRequestDelegate)Delegate.Combine(this.OnHttpRequest, value);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.OnHttpRequest = (LineWebHelper.HttpRequestDelegate)Delegate.Remove(this.OnHttpRequest, value);
		}
	}

	public static LineWebHelper Instance
	{
		get
		{
			if (LineWebHelper.current == null)
			{
				LineWebHelper.container = new GameObject();
				LineWebHelper.container.name = "LineWebHelper";
				LineWebHelper.current = (LineWebHelper.container.AddComponent(typeof(LineWebHelper)) as LineWebHelper);
			}
			return LineWebHelper.current;
		}
	}

	public void get(int id, string url)
	{
		UnityEngine.Debug.LogError("get start");
		WWW www = new WWW(url);
		base.StartCoroutine(this.WaitForRequest(id, www));
		UnityEngine.Debug.LogError("get end");
	}

	public void post(int id, string url, string data)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Content-Type", "application/json; charset=uft8");
		WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), hashtable);
		base.StartCoroutine(this.WaitForRequest(id, www));
	}

	public void post2(int id, string url, string data)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Content-Type", "application/x-www-form-urlencoded; charset=uft8");
		WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), hashtable);
		base.StartCoroutine(this.WaitForRequest(id, www));
	}

	public void postToken(int id, string url, string data)
	{
		UnityEngine.Debug.LogError("get start");
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Content-Type", "application/json; charset=uft8");
		hashtable.Add("X-Line-ChannelToken", "Access Token");
		WWW www = new WWW(url, Encoding.UTF8.GetBytes(data), hashtable);
		base.StartCoroutine(this.WaitForRequest(id, www));
	}

	[DebuggerHidden]
	private IEnumerator WaitForRequest(int id, WWW www)
	{
		LineWebHelper.<WaitForRequest>c__IteratorB <WaitForRequest>c__IteratorB = new LineWebHelper.<WaitForRequest>c__IteratorB();
		<WaitForRequest>c__IteratorB.www = www;
		<WaitForRequest>c__IteratorB.id = id;
		<WaitForRequest>c__IteratorB.<$>www = www;
		<WaitForRequest>c__IteratorB.<$>id = id;
		<WaitForRequest>c__IteratorB.<>f__this = this;
		return <WaitForRequest>c__IteratorB;
	}
}
