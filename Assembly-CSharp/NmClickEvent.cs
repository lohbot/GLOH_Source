using System;
using UnityEngine;

public class NmClickEvent : MonoBehaviour
{
	private INrCharInput m_kCharInput;

	public INrCharInput CharInput
	{
		get
		{
			return this.m_kCharInput;
		}
		set
		{
			this.m_kCharInput = value;
		}
	}

	public NmClickEvent()
	{
		this.m_kCharInput = new NkInputNull();
	}

	private void OnMouseExit()
	{
		this.m_kCharInput.MouseEvent_Exit();
	}

	private void OnMouseEnter()
	{
		this.m_kCharInput.MouseEvent_Enter();
	}

	private void OnMouseOver()
	{
		this.m_kCharInput.MouseEvent_Over();
	}

	private void OnTriggerEnter(Collider other)
	{
	}
}
