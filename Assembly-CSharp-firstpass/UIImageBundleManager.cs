using GameMessage;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class UIImageBundleManager : NrTSingleton<UIImageBundleManager>
{
	private Dictionary<string, Texture2D> m_mapUIImage = new Dictionary<string, Texture2D>();

	private UIImageBundleManager()
	{
	}

	public Texture2D GetTexture(string imageKey)
	{
		if (!this.m_mapUIImage.ContainsKey(imageKey))
		{
			return null;
		}
		if (null == this.m_mapUIImage[imageKey])
		{
			this.m_mapUIImage.Remove(imageKey);
			return null;
		}
		return this.m_mapUIImage[imageKey];
	}

	public void AddTexture(string imageKey, Texture2D texture)
	{
		if (this.m_mapUIImage.ContainsKey(imageKey))
		{
			return;
		}
		this.m_mapUIImage.Add(imageKey, texture);
	}

	public void DeleteTexture()
	{
		List<string> list = new List<string>();
		foreach (string current in this.m_mapUIImage.Keys)
		{
			list.Add(current);
		}
		foreach (string current2 in list)
		{
			if (!current2.Contains("64"))
			{
				Resources.UnloadAsset(this.m_mapUIImage[current2]);
				this.m_mapUIImage.Remove(current2);
			}
		}
		list.Clear();
	}

	private string _ImagePath(eCharImageType type)
	{
		string result = string.Empty;
		if (type == eCharImageType.SMALL)
		{
			result = "UI/Soldier/64";
		}
		else if (type == eCharImageType.MIDDLE)
		{
			result = "UI/Soldier/256";
		}
		else if (type == eCharImageType.LARGE)
		{
			if (UIDataManager.IsUse256Texture())
			{
				result = "UI/Soldier/256";
			}
			else
			{
				result = "UI/Soldier/512";
			}
		}
		return result;
	}

	public void RequestCharImage(string imageKey, eCharImageType type, PostProcPerItem callbackDelegate)
	{
		string arg = this._ImagePath(type);
		string str = string.Format("{0}/{1}", arg, imageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, MsgHandler.HandleReturn<string>("UIBundleStackName", new object[0]));
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(callbackDelegate, imageKey);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void RequestCharImageCustomParam(string imageKey, eCharImageType type, PostProcPerItem callbackDelegate, object callBackParam)
	{
		string arg = this._ImagePath(type);
		string str = string.Format("{0}/{1}", arg, imageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, MsgHandler.HandleReturn<string>("UIBundleStackName", new object[0]));
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(callbackDelegate, callBackParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void RequestBundleImage(string bundlepath, PostProcPerItem callbackDelegate)
	{
		string str = string.Format("{0}{1}", bundlepath, NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, MsgHandler.HandleReturn<string>("UIBundleStackName", new object[0]));
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(callbackDelegate, bundlepath);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	public void SetCharImage(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}
}
