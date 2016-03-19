using System;
using System.Collections.Generic;
using UnityEngine;

public class RadioBtnGroup
{
	private static List<RadioBtnGroup> groups = new List<RadioBtnGroup>();

	public int groupID;

	public List<UIRadioBtn> buttons = new List<UIRadioBtn>();

	public RadioBtnGroup(int id)
	{
		this.groupID = id;
		RadioBtnGroup.groups.Add(this);
	}

	~RadioBtnGroup()
	{
		RadioBtnGroup.groups.Remove(this);
	}

	public static IRadioButton GetSelected(GameObject go)
	{
		return RadioBtnGroup.GetSelected(go.transform.GetHashCode());
	}

	public static IRadioButton GetSelected(int id)
	{
		RadioBtnGroup radioBtnGroup = null;
		for (int i = 0; i < RadioBtnGroup.groups.Count; i++)
		{
			if (RadioBtnGroup.groups[i].groupID == id)
			{
				radioBtnGroup = RadioBtnGroup.groups[i];
				break;
			}
		}
		if (radioBtnGroup == null)
		{
			return null;
		}
		for (int j = 0; j < radioBtnGroup.buttons.Count; j++)
		{
			if (((IRadioButton)radioBtnGroup.buttons[j]).Value)
			{
				return radioBtnGroup.buttons[j];
			}
		}
		return null;
	}

	public static RadioBtnGroup GetGroup(int id)
	{
		RadioBtnGroup radioBtnGroup = null;
		for (int i = 0; i < RadioBtnGroup.groups.Count; i++)
		{
			if (RadioBtnGroup.groups[i].groupID == id)
			{
				radioBtnGroup = RadioBtnGroup.groups[i];
				break;
			}
		}
		if (radioBtnGroup == null)
		{
			radioBtnGroup = new RadioBtnGroup(id);
		}
		return radioBtnGroup;
	}
}
