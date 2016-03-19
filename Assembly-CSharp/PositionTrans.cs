using System;
using UnityEngine;

public class PositionTrans : MonoBehaviour
{
	public GameObject m_RootGameObject;

	public string m_PositionGameObjectName;

	public Vector3 m_Position = Vector3.zero;

	public Vector3 m_Rotation = Vector3.zero;

	public Vector3 m_Scale = Vector3.zero;

	public GameObject goChild;

	public Transform m_Transform
	{
		get
		{
			if (this.m_PositionGameObjectName == null || this.m_PositionGameObjectName == string.Empty)
			{
				return this.m_RootGameObject.transform;
			}
			return EventTriggerHelper.GetChildTransform(this.m_RootGameObject.transform, this.m_PositionGameObjectName);
		}
	}

	private void Start()
	{
		if (this.m_RootGameObject == null)
		{
			return;
		}
		this.Trans();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetPosition(Vector3 position)
	{
		this.m_Position = position;
	}

	public void SetRotation(Vector3 rotation)
	{
		this.m_Rotation = rotation;
	}

	public void SetScale(Vector3 scale)
	{
		this.m_Scale = scale;
	}

	public void Trans()
	{
		if (this.m_RootGameObject == null)
		{
			return;
		}
		Transform transform;
		if (this.m_PositionGameObjectName == string.Empty || this.m_PositionGameObjectName == null)
		{
			transform = base.gameObject.transform;
		}
		else
		{
			transform = EventTriggerHelper.GetChildTransform(this.m_RootGameObject.transform, this.m_PositionGameObjectName);
		}
		if (transform == null)
		{
			return;
		}
		base.gameObject.transform.position = transform.position;
		base.gameObject.transform.eulerAngles = transform.eulerAngles;
		base.gameObject.transform.localScale = transform.root.localScale;
		if (base.gameObject.transform.childCount > 0)
		{
			Transform child = base.gameObject.transform.GetChild(0);
			if (child)
			{
				Vector3 localPosition = child.localPosition;
				Vector3 localEulerAngles = child.localEulerAngles;
				Vector3 localScale = child.localScale;
				child.parent = transform;
				child.localPosition = localPosition;
				child.localEulerAngles = localEulerAngles;
				child.localScale = localScale;
				child.localPosition = this.m_Position;
				child.localEulerAngles = this.m_Rotation;
				if (!this.m_Scale.Equals(Vector3.zero))
				{
					child.localScale = this.m_Scale;
				}
			}
		}
	}
}
