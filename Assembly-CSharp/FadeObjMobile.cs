using System;
using UnityEngine;

public class FadeObjMobile : MonoBehaviour
{
	public Transform m_Target;

	private GameObject FadeObject;

	public Shader shader1;

	public Shader shader2;

	public Shader shader3;

	private void Start()
	{
		base.InvokeRepeating("FadeObj", 1f, 0.3f);
		this.FadeObject = null;
	}

	public void SetPositionFollower(Transform Target)
	{
		this.m_Target = Target;
	}

	private void FadeObj()
	{
		if (this.m_Target == null)
		{
			return;
		}
		int layerMask = TsLayer.EVERYTHING - TsLayer.TERRAIN - TsLayer.NPC - TsLayer.PC;
		RaycastHit raycastHit;
		if (Physics.Linecast(this.m_Target.position + base.transform.forward * 5f, base.transform.position, out raycastHit, layerMask))
		{
			if (raycastHit.collider.gameObject != null)
			{
				if (raycastHit.transform.collider.tag == "FadeObjMobile")
				{
					if (this.FadeObject != null)
					{
						this.OffFade(this.FadeObject);
					}
					GameObject gameObject = raycastHit.transform.parent.gameObject;
					Transform child = gameObject.transform.GetChild(0);
					this.FadeObject = child.transform.gameObject;
					this.OnFade(this.FadeObject);
				}
				else if (raycastHit.transform.collider.tag == "FadeTreeMobile")
				{
					if (this.FadeObject != null)
					{
						this.OffFade(this.FadeObject);
					}
					GameObject gameObject2 = raycastHit.transform.gameObject;
					this.FadeObject = gameObject2.transform.gameObject;
					this.OnFade(this.FadeObject);
				}
				else if (raycastHit.transform.collider.tag != "FadeObjMobile" || raycastHit.transform.collider.tag != "FadeTreeMobile")
				{
					this.OffFade(this.FadeObject);
				}
			}
		}
		else
		{
			this.OffFade(this.FadeObject);
		}
	}

	private void OnFade(GameObject FadeObject)
	{
		if (FadeObject == null)
		{
			return;
		}
		if (FadeObject.tag == "FadeObjMobile" || FadeObject.tag == "FadeTreeMobile")
		{
			Renderer componentInChildren = FadeObject.transform.GetComponentInChildren<Renderer>();
			componentInChildren.material.shader = this.shader1;
			componentInChildren.material.SetFloat("_Alpha", 0.3f);
			FadeObject = FadeObject.gameObject;
		}
	}

	private void OffFade(GameObject FadeObject)
	{
		if (FadeObject == null)
		{
			return;
		}
		if (FadeObject.tag == "FadeObjMobile")
		{
			Renderer componentInChildren = FadeObject.transform.GetComponentInChildren<Renderer>();
			Shader shader = this.shader1;
			componentInChildren.material.shader = shader;
			if (shader)
			{
				componentInChildren.material.shader = this.shader2;
			}
		}
		else if (FadeObject.tag == "FadeTreeMobile")
		{
			Renderer componentInChildren2 = FadeObject.transform.GetComponentInChildren<Renderer>();
			Shader shader = this.shader1;
			componentInChildren2.material.shader = shader;
			if (shader)
			{
				componentInChildren2.material.shader = this.shader3;
			}
		}
		FadeObject = null;
	}

	private void OnDrawGizmos()
	{
		if (this.m_Target == null)
		{
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.m_Target.position + base.transform.forward * 5f, base.transform.position);
	}
}
