using System;
using TsBundle;
using UnityEngine;

public class EventTrigger_AreaEffect : MonoBehaviour
{
	public string _ToolTipMsg = string.Empty;

	private bool _Visible;

	public bool Visible
	{
		get
		{
			return this._Visible;
		}
		set
		{
			EventTriggerHelper.ActiveAllTreeChildren(base.gameObject, value);
			if (base.gameObject.collider != null)
			{
				base.gameObject.collider.enabled = value;
			}
			this._Visible = value;
		}
	}

	private void Start()
	{
		TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.WorldScene, base.gameObject);
		base.gameObject.AddComponent<BoxCollider>();
	}

	public void SetInfo(float x, float z, string ToolTipMsg)
	{
		base.transform.localPosition = new Vector3(x, EventTriggerGameHelper.GetGroundPosition(x, z), z);
		this._ToolTipMsg = ToolTipMsg;
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
		else
		{
			UnityEngine.Object mainAsset = wItem.mainAsset;
			if (mainAsset != null && base.gameObject)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(mainAsset, new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y + 0.5f, base.gameObject.transform.position.z), base.gameObject.transform.localRotation) as GameObject;
				gameObject.transform.parent = base.gameObject.transform;
				EventTriggerHelper.ActiveAllTreeChildren(base.gameObject, this._Visible);
				base.gameObject.collider.enabled = this._Visible;
			}
			wItem.unloadImmediate = true;
		}
	}

	private void OnMouseExit()
	{
	}

	private void OnMouseEnter()
	{
		if (!string.IsNullOrEmpty(this._ToolTipMsg))
		{
		}
	}
}
