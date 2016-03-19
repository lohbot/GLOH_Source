using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TsFadeBuilding/TsFadeRayCaster")]
public class TsFadeRayCaster : MonoBehaviour
{
	public class TsFadeObjectContainer
	{
		private List<GameObject> listFadeTargets = new List<GameObject>();

		public int TargetCount
		{
			get
			{
				return this.listFadeTargets.Count;
			}
		}

		~TsFadeObjectContainer()
		{
			this.listFadeTargets.Clear();
		}

		public void AddFadeTarget(GameObject TargetObject)
		{
			if (TargetObject != null)
			{
				this.listFadeTargets.Add(TargetObject);
			}
		}

		private void DeleteFadeTarget(GameObject TargetObject)
		{
			if (this.listFadeTargets.Remove(TargetObject))
			{
				Debug.Log(" Delete TargetObject!!");
			}
		}

		public GameObject GetFadeTargetforList(int index)
		{
			return this.listFadeTargets[index];
		}
	}

	private List<GameObject> _currentFadeBuilding = new List<GameObject>();

	[HideInInspector]
	public TsFadeRayCaster.TsFadeObjectContainer fadeObjectContainer = new TsFadeRayCaster.TsFadeObjectContainer();

	private void Start()
	{
		this._currentFadeBuilding.Clear();
	}

	private void Update()
	{
		this._DetectHitBuilding();
	}

	private void _DetectHitBuilding()
	{
		TsFadeRayCaster.TsFadeObjectContainer tsFadeObjectContainer = this.fadeObjectContainer;
		if (tsFadeObjectContainer == null)
		{
			Debug.Log("Not Found FadeContainer!");
			return;
		}
		for (int i = 0; i < tsFadeObjectContainer.TargetCount; i++)
		{
			GameObject fadeTargetforList = tsFadeObjectContainer.GetFadeTargetforList(i);
			if (!(fadeTargetforList == null) && !(Camera.main == null))
			{
				Vector3 direction = fadeTargetforList.transform.position - Camera.main.transform.position;
				direction.Normalize();
				float distance = Vector3.Distance(Camera.main.transform.position, fadeTargetforList.transform.position);
				Debug.DrawLine(Camera.main.transform.position, fadeTargetforList.transform.position, Color.red);
				TsLayerMask layerMask = TsLayer.IGNORE_RAYCAST;
				RaycastHit[] array = Physics.RaycastAll(Camera.main.transform.position, direction, distance, layerMask);
				if (array.Length > 0)
				{
					this._OnFadeOut(array);
					this._OnFadeIn(array);
				}
				else
				{
					this._OnFadeIn(array);
				}
			}
		}
	}

	private void _OnFadeOut(RaycastHit[] kHitInfos)
	{
		string tag = TsTag.FADE_OBJECT.ToString();
		for (int i = 0; i < kHitInfos.Length; i++)
		{
			RaycastHit raycastHit = kHitInfos[i];
			GameObject gameObject = raycastHit.transform.gameObject;
			if (gameObject.CompareTag(tag))
			{
				TsFadeBuilding component = gameObject.GetComponent<TsFadeBuilding>();
				if (!(component == null))
				{
					bool flag = false;
					foreach (GameObject current in this._currentFadeBuilding)
					{
						if (current.GetInstanceID() == gameObject.GetInstanceID())
						{
							flag = true;
							break;
						}
					}
					if (!flag && !component.FadeOut)
					{
						component.FadeOut = true;
						component.FadeIn = false;
						this._currentFadeBuilding.Add(gameObject);
					}
				}
			}
		}
	}

	private void _OnFadeIn(RaycastHit[] kHitInfos)
	{
		GameObject[] array = new GameObject[32];
		int num = 0;
		foreach (GameObject current in this._currentFadeBuilding)
		{
			bool flag = true;
			for (int i = 0; i < kHitInfos.Length; i++)
			{
				RaycastHit raycastHit = kHitInfos[i];
				GameObject gameObject = raycastHit.transform.gameObject;
				if (current.GetInstanceID() == gameObject.GetInstanceID())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				TsFadeBuilding component = current.GetComponent<TsFadeBuilding>();
				component.FadeIn = true;
				component.FadeOut = false;
				array[num] = current;
				num++;
			}
		}
		for (int j = 0; j < num; j++)
		{
			this._currentFadeBuilding.Remove(array[j]);
		}
	}
}
