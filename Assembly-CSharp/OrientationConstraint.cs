using System;
using UnityEngine;

public class OrientationConstraint : MonoBehaviour
{
	public enum ortType
	{
		Camera,
		Node
	}

	public enum tmRule
	{
		Local,
		World
	}

	public OrientationConstraint.ortType orientationType;

	public Transform node;

	public OrientationConstraint.tmRule transformRule = OrientationConstraint.tmRule.World;

	private Transform targetNode;

	private void Update()
	{
		OrientationConstraint.ortType ortType = this.orientationType;
		if (ortType != OrientationConstraint.ortType.Camera)
		{
			if (ortType == OrientationConstraint.ortType.Node)
			{
				if (this.node != null)
				{
					this.targetNode = this.node;
				}
			}
		}
		else if (Camera.main != null)
		{
			this.targetNode = Camera.main.transform;
		}
		if (this.targetNode == null)
		{
			return;
		}
		OrientationConstraint.tmRule tmRule = this.transformRule;
		if (tmRule != OrientationConstraint.tmRule.Local)
		{
			if (tmRule == OrientationConstraint.tmRule.World)
			{
				base.transform.rotation = this.targetNode.rotation;
			}
		}
		else
		{
			base.transform.localRotation = this.targetNode.localRotation;
		}
	}
}
