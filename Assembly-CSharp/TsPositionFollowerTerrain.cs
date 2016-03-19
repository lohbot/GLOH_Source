using System;
using UnityEngine;

public class TsPositionFollowerTerrain : MonoBehaviour
{
	[SerializeField]
	private float _offsetY = 0.5f;

	private TsLayerMask mc_kPickLayer = TsLayer.TERRAIN;

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, this.mc_kPickLayer) && raycastHit.collider.gameObject != null)
		{
			Vector3 point = raycastHit.point;
			point.y += this._offsetY;
			base.transform.position = point;
		}
	}
}
