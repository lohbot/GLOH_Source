using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Panels/Panel")]
[Serializable]
public class UIPanel : UIPanelBase
{
	[HideInInspector]
	public EZTransitionList transitions = new EZTransitionList(new EZTransition[]
	{
		new EZTransition("Bring In Forward"),
		new EZTransition("Bring In Back"),
		new EZTransition("Dismiss Forward"),
		new EZTransition("Dismiss Back")
	});

	public override EZTransitionList Transitions
	{
		get
		{
			return this.transitions;
		}
	}

	public static UIPanel Create(string name, Vector3 pos)
	{
		return (UIPanel)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIPanel));
	}

	public static UIPanel Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIPanel)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIPanel));
	}
}
