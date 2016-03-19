using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelPrefabBundleLinker : MonoBehaviour
{
	public List<ModelPrefabInfo> m_ModelPrefabInfo;

	public void Add(UnityEngine.Object prefab, ModelInfo info)
	{
		if (this.m_ModelPrefabInfo == null)
		{
			this.m_ModelPrefabInfo = new List<ModelPrefabInfo>();
		}
		ModelPrefabInfo modelPrefabInfo = null;
		foreach (ModelPrefabInfo current in this.m_ModelPrefabInfo)
		{
			if (current.m_PrefabName.Equals(prefab.name))
			{
				modelPrefabInfo = current;
			}
		}
		if (modelPrefabInfo == null)
		{
			modelPrefabInfo = new ModelPrefabInfo();
			modelPrefabInfo.m_PrefabName = prefab.name;
			modelPrefabInfo.m_PrefabAssetPath = EventTriggerEditorUtil._OnGetAssetPath(prefab);
			modelPrefabInfo.m_Prefab = prefab;
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
			modelPrefabInfo.m_PrefabScale = gameObject.transform.localScale;
			UnityEngine.Object.DestroyImmediate(gameObject);
			modelPrefabInfo.m_ModelInfoList = new List<ModelInfo>();
			this.m_ModelPrefabInfo.Add(modelPrefabInfo);
		}
		foreach (ModelInfo current2 in modelPrefabInfo.m_ModelInfoList)
		{
			if (current2.Model.Equals(info.Model))
			{
				return;
			}
		}
		modelPrefabInfo.m_ModelInfoList.Add(info);
		if (info.ChangeScale && !modelPrefabInfo.m_PrefabScale.Equals(info.Scale))
		{
			info.Model.transform.localScale = modelPrefabInfo.m_PrefabScale;
		}
		int childCount = info.Model.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = info.Model.transform.GetChild(0);
			if (!(child == null))
			{
				GameObject gameObject2 = child.gameObject;
				if (!(gameObject2 == null))
				{
					UnityEngine.Object.DestroyImmediate(gameObject2);
				}
			}
		}
	}

	public void GameObjectActive(bool bActive)
	{
		foreach (ModelPrefabInfo current in this.m_ModelPrefabInfo)
		{
			foreach (ModelInfo current2 in current.m_ModelInfoList)
			{
				current2.Model.SetActive(bActive);
			}
		}
	}

	public ModelPrefabInfo GetInfo(UnityEngine.Object oj)
	{
		return this.GetInfo(oj.name);
	}

	public ModelPrefabInfo GetInfo(string name)
	{
		foreach (ModelPrefabInfo current in this.m_ModelPrefabInfo)
		{
			if (current.m_PrefabName.Equals(name))
			{
				return current;
			}
		}
		return null;
	}

	public void SetPrefab(string name, UnityEngine.Object prefab)
	{
		ModelPrefabInfo info = this.GetInfo(name);
		if (info != null)
		{
			info.m_Prefab = prefab;
		}
	}

	public void CreateModel()
	{
		foreach (ModelPrefabInfo current in this.m_ModelPrefabInfo)
		{
			this.CreateModel(current);
		}
	}

	public bool CreateModel(ModelPrefabInfo prefabInfo)
	{
		if (prefabInfo == null)
		{
			return false;
		}
		if (prefabInfo.m_Prefab == null)
		{
			return false;
		}
		foreach (ModelInfo current in prefabInfo.m_ModelInfoList)
		{
			if (!(current.Model == null))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(prefabInfo.m_Prefab, current.Model.transform.position, current.Model.transform.rotation) as GameObject;
				if (!gameObject.transform.localScale.Equals(prefabInfo.m_PrefabScale))
				{
					TsLog.LogWarning(string.Concat(new object[]
					{
						"Bundle Scale Miss ",
						prefabInfo.m_Prefab.name,
						" Org: ",
						prefabInfo.m_PrefabScale,
						" Bundle: ",
						gameObject.transform.localScale
					}), new object[0]);
				}
				int childCount = gameObject.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = gameObject.transform.GetChild(0);
					if (!(child == null))
					{
						child.parent = current.Model.transform;
					}
				}
				Component[] components = gameObject.GetComponents(typeof(Component));
				Component[] array = components;
				for (int j = 0; j < array.Length; j++)
				{
					Component component = array[j];
					if (current.Model.GetComponent(component.GetType()) == null)
					{
						EventTriggerHelper.ComponentCopy(component, current.Model, false);
					}
					if (component.GetType() == typeof(Animation))
					{
						EventTriggerHelper.ComponentCopy(component, current.Model, true);
						Animation component2 = current.Model.GetComponent<Animation>();
						Animation animation = component as Animation;
						if (component2 != null && animation != null)
						{
							foreach (AnimationState animationState in animation)
							{
								component2.AddClip(animationState.clip, animationState.clip.name);
							}
							component2.clip = animation.clip;
						}
					}
				}
				UnityEngine.Object.Destroy(gameObject);
				if (current.ChangeScale && !prefabInfo.m_PrefabScale.Equals(current.Scale))
				{
					current.Model.transform.localScale = current.Scale;
				}
			}
		}
		return true;
	}
}
