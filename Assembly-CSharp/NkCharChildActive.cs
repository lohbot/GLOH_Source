using System;
using System.Collections.Generic;
using UnityEngine;

public class NkCharChildActive
{
	private List<Transform> kExceptActiveList;

	public NkCharChildActive()
	{
		this.kExceptActiveList = new List<Transform>();
		this.Init();
	}

	public void Init()
	{
		this.kExceptActiveList.Clear();
	}

	private bool SetExceptActive(ref Transform pkTarget, bool bActive)
	{
		if (!bActive)
		{
			if (!pkTarget.gameObject.activeInHierarchy)
			{
				this.kExceptActiveList.Add(pkTarget);
				return true;
			}
		}
		else if (this.kExceptActiveList.Contains(pkTarget))
		{
			this.kExceptActiveList.Remove(pkTarget);
			return true;
		}
		return false;
	}

	public void SetChildActive(GameObject charroot, bool bActive)
	{
		if (charroot == null)
		{
			return;
		}
		int childCount = charroot.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = charroot.transform.GetChild(i);
			if (!(child == null))
			{
				bool flag = this.SetExceptActive(ref child, bActive);
				if (!flag)
				{
					int childCount2 = child.childCount;
					if (childCount2 == 0)
					{
						child.gameObject.SetActive(bActive);
					}
					else
					{
						for (int j = 0; j < childCount2; j++)
						{
							Transform child2 = child.GetChild(j);
							if (!(child2 == null))
							{
								flag = this.SetExceptActive(ref child2, bActive);
								if (!flag)
								{
									child2.gameObject.SetActive(bActive);
								}
							}
						}
					}
				}
			}
		}
		charroot.SetActive(bActive);
		if (bActive)
		{
			this.kExceptActiveList.Clear();
		}
	}
}
