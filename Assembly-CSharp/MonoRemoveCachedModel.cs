using System;
using UnityEngine;

public class MonoRemoveCachedModel : MonoBehaviour
{
	private Action<string> RemoveCacheModel;

	private string m_Key;

	public void RegisterAction(string key, Action<string> func)
	{
		this.m_Key = string.Intern(key);
		this.RemoveCacheModel = func;
	}

	private void OnDestroy()
	{
		if (this.RemoveCacheModel != null)
		{
			this.RemoveCacheModel(this.m_Key);
		}
		else
		{
			Debug.LogWarning(string.Format("[{0}] ĳ�� �� ������Ʈ�� �����ϱ� ���� delegator�� �����Ǿ� ���� �ʽ��ϴ�. (Key=\"{1}\")", base.name, this.m_Key));
		}
	}
}
