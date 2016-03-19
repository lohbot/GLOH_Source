using GameMessage;
using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NkBundleCallBack : NrTSingleton<NkBundleCallBack>
{
	private float m_fPreloadCount;

	private float m_fPreloadMaxCount;

	public static string PlayerBundleStackName
	{
		get
		{
			return "PlayerBundleStack";
		}
	}

	public static string NPCBundleStackName
	{
		get
		{
			return "NPCBundleStack";
		}
	}

	public static string EffectBundleStackName
	{
		get
		{
			return "EffectBundleStack";
		}
	}

	public static string BuildingBundleStackName
	{
		get
		{
			return "BuildingBundleStack";
		}
	}

	public static string BattlePreLoadingChar
	{
		get
		{
			return "BattlePreLoadingChar";
		}
	}

	public static string UIBundleStackName
	{
		get
		{
			return "UIBundleStackName";
		}
	}

	public static string AudioBundleStackName
	{
		get
		{
			return "AudioBundleStack";
		}
	}

	private NkBundleCallBack()
	{
	}

	public void ClearPlayerBundleStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.PlayerBundleStackName, false);
	}

	public void ClearNPCBundleStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.NPCBundleStackName, false);
	}

	public void ClearEffectBundleStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.EffectBundleStackName, false);
	}

	public void ClearBuildingBundleStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.BuildingBundleStackName, false);
	}

	public void ClearBattlePreLoadingCharStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.BattlePreLoadingChar, false);
	}

	public void ClearUIBundleStackName()
	{
		Holder.ClearStackItem(NkBundleCallBack.UIBundleStackName, false);
	}

	public void ClearAudioBundleStack()
	{
		Holder.ClearStackItem(NkBundleCallBack.AudioBundleStackName, true);
		Holder.ClearStackItem(TsAudio.AssetBundleStackName, true);
	}

	public float GetPreloadMaxCount()
	{
		return this.m_fPreloadMaxCount;
	}

	public float GetRatioPreloadCompleted()
	{
		if (this.m_fPreloadMaxCount == 0f)
		{
			return 1f;
		}
		return (this.m_fPreloadMaxCount - this.m_fPreloadCount) / this.m_fPreloadMaxCount;
	}

	public bool IsCompletedPreload()
	{
		return this.m_fPreloadCount == 0f;
	}

	public WWWItem RequestBundlePreload(string path, string stackname, ItemType itemtype, int iParam, string szParam, NkBundleParam.eBundleType bundletype, string bundlekey)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, stackname);
		wWWItem.SetItemType(itemtype);
		NkBundleParam nkBundleParam = new NkBundleParam(bundletype, bundlekey);
		nkBundleParam.SetPreload(true);
		nkBundleParam.SetParam(iParam, szParam);
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.BGLOAD, false);
		return wWWItem;
	}

	public void RequestBundleRuntime(string path, string stackname, ItemType itemtype, int iParam, string szParam, NkBundleParam.eBundleType bundletype, string bundlekey)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, stackname);
		wWWItem.SetItemType(itemtype);
		NkBundleParam nkBundleParam = new NkBundleParam(bundletype, bundlekey);
		nkBundleParam.SetPreload(false);
		nkBundleParam.SetParam(iParam, szParam);
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
	}

	public void RequestBundleRuntime(string path, string stackname, ItemType itemtype, int iParam, string szParam, NkBundleParam.eBundleType bundletype, string bundlekey, bool bUnloadAfter)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, stackname);
		wWWItem.SetItemType(itemtype);
		NkBundleParam nkBundleParam = new NkBundleParam(bundletype, bundlekey);
		nkBundleParam.SetPreload(false);
		nkBundleParam.SetParam(iParam, szParam);
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, bUnloadAfter);
	}

	public void RequestBundleUI(string path, int dialogid, string checkname)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		NkBundleParam nkBundleParam = new NkBundleParam(NkBundleParam.eBundleType.BUNDLE_UI_DIALOG, string.Empty);
		nkBundleParam.SetPreload(false);
		nkBundleParam.SetParam(dialogid, checkname);
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
	}

	public void RequestBundleCallBack(string path, NkBundleParam.funcBundleCallBack callbackfunc)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, "NormalCallBack");
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		NkBundleParam nkBundleParam = new NkBundleParam(NkBundleParam.eBundleType.BUNDLE_NORMAL_CALLBACK, string.Empty);
		nkBundleParam.funcCallBack = callbackfunc;
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
	}

	public void RequestBundleCallBackRunTime(string path, string stackname, NkBundleParam.funcParamBundleCallBack callbackfunc, object paramobj, bool unloadAfterPostProcess)
	{
		string str = path;
		if (TsPlatform.IsMobile && !path.Contains("_mobile"))
		{
			str += "_mobile";
		}
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, stackname);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		NkBundleParam nkBundleParam = new NkBundleParam(NkBundleParam.eBundleType.BUNDLE_OBJECTPARAM_CALLBACK, stackname);
		nkBundleParam.SetPreload(false);
		nkBundleParam.funcParamCallBack = callbackfunc;
		nkBundleParam.SetParamObject(paramobj);
		wWWItem.SetCallback(new PostProcPerItem(this.ProcessBundleCallBack), nkBundleParam);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, unloadAfterPostProcess);
	}

	private Nr3DCharBase Get3DChar(int id)
	{
		Nr3DCharBase result;
		if (id <= 300)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(id);
			if (@char == null)
			{
				return null;
			}
			result = @char.Get3DChar();
		}
		else
		{
			id -= 300;
			NkBattleChar char2 = NrTSingleton<NkBattleCharManager>.Instance.GetChar(id);
			if (char2 == null)
			{
				return null;
			}
			result = char2.Get3DChar();
		}
		return result;
	}

	public void ProcessBundleCallBack(IDownloadedItem wItem, object kParamObj)
	{
		if (wItem == null || kParamObj == null)
		{
			return;
		}
		NkBundleParam nkBundleParam = kParamObj as NkBundleParam;
		string bundleKey = nkBundleParam.GetBundleKey();
		if (!wItem.canAccessAssetBundle)
		{
			switch (nkBundleParam.GetBundleType())
			{
			case NkBundleParam.eBundleType.BUNDLE_CHAR_BONE:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_BONEPRELOAD:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_SWITCHPART:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHPART:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHITEM:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_RIDE:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART:
			case NkBundleParam.eBundleType.BUNDLE_CHAR_OBJECT:
			{
				if (nkBundleParam.GetBundleType() == NkBundleParam.eBundleType.BUNDLE_CHAR_BONEPRELOAD)
				{
					this.m_fPreloadCount -= 1f;
				}
				int id = int.Parse(bundleKey);
				Nr3DCharBase nr3DCharBase = this.Get3DChar(id);
				if (nr3DCharBase == null)
				{
					return;
				}
				bool itembundle = nkBundleParam.GetBundleType() == NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHITEM;
				nr3DCharBase.InitBundleLoadFailed(itembundle, wItem.indexParam);
				if (nkBundleParam.GetBundleType() == NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART || nkBundleParam.GetBundleType() == NkBundleParam.eBundleType.BUNDLE_CHAR_OBJECT)
				{
					nr3DCharBase.Set3DCharLoadFailed(true);
				}
				if (Scene.IsCurScene(Scene.Type.SELECTCHAR))
				{
					MsgHandler.Handle("SetCreateCharPartInfo", new object[]
					{
						true,
						false
					});
				}
				break;
			}
			}
			return;
		}
		WWWItem wWWItem = wItem as WWWItem;
		if (wWWItem != null)
		{
			wWWItem.SetIndexParam(nkBundleParam.GetNumParam());
			wWWItem.SetStringParam(nkBundleParam.GetStrParam());
		}
		switch (nkBundleParam.GetBundleType())
		{
		case NkBundleParam.eBundleType.BUNDLE_CHAR_BONE:
		case NkBundleParam.eBundleType.BUNDLE_CHAR_BONEPRELOAD:
			this.FinishDownloadAsync(wItem, nkBundleParam);
			if (nkBundleParam.GetBundleType() == NkBundleParam.eBundleType.BUNDLE_CHAR_BONEPRELOAD)
			{
				this.m_fPreloadCount -= 1f;
			}
			break;
		case NkBundleParam.eBundleType.BUNDLE_CHAR_ANIMATION:
			NrTSingleton<Nr3DCharSystem>.Instance.FinishDownloadAnimation(wItem);
			break;
		case NkBundleParam.eBundleType.BUNDLE_CHAR_SWITCHPART:
		{
			int id2 = int.Parse(bundleKey);
			Nr3DCharActor nr3DCharActor = this.Get3DChar(id2) as Nr3DCharActor;
			if (nr3DCharActor == null)
			{
				return;
			}
			nr3DCharActor.FinishDownloadPart(ref wItem);
			break;
		}
		case NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHPART:
		case NkBundleParam.eBundleType.BUNDLE_CHAR_ATTACHITEM:
		{
			int id3 = int.Parse(bundleKey);
			Nr3DCharBase nr3DCharBase2 = this.Get3DChar(id3);
			if (nr3DCharBase2 == null)
			{
				return;
			}
			nr3DCharBase2.FinishDownloadItem(ref wItem);
			break;
		}
		case NkBundleParam.eBundleType.BUNDLE_CHAR_RIDE:
		{
			int id4 = int.Parse(bundleKey);
			Nr3DCharActor nr3DCharActor2 = this.Get3DChar(id4) as Nr3DCharActor;
			if (nr3DCharActor2 == null)
			{
				return;
			}
			nr3DCharActor2.FinishDownloadRide(ref wItem);
			break;
		}
		case NkBundleParam.eBundleType.BUNDLE_CHAR_NONEPART:
		case NkBundleParam.eBundleType.BUNDLE_CHAR_OBJECT:
		{
			int id5 = int.Parse(bundleKey);
			Nr3DCharBase nr3DCharBase3 = this.Get3DChar(id5);
			if (nr3DCharBase3 == null)
			{
				return;
			}
			nr3DCharBase3.FinishDownloadBase(ref wItem);
			break;
		}
		case NkBundleParam.eBundleType.BUNDLE_UI_DIALOG:
		{
			int indexParam = wItem.indexParam;
			Form form = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)indexParam);
			if (form == null)
			{
				return;
			}
			form.FinishDownloadBundle(ref wItem);
			break;
		}
		case NkBundleParam.eBundleType.BUNDLE_NORMAL_CALLBACK:
			nkBundleParam.funcCallBack(ref wItem);
			break;
		case NkBundleParam.eBundleType.BUNDLE_OBJECTPARAM_CALLBACK:
			nkBundleParam.funcParamCallBack(ref wItem, nkBundleParam.GetParamObject());
			break;
		default:
			return;
		}
	}

	public void FinishDownloadAsync(IDownloadedItem wItem, NkBundleParam kBundleParam)
	{
		if (wItem.canAccessAssetBundle)
		{
			TsImmortal.bundleService.RequestLoadAsync(new LoadAsyncCallback(this.LoadAsyncCallback), wItem, kBundleParam, "characterbase", typeof(GameObject));
		}
		else
		{
			UnityEngine.Debug.LogError("Failed to access Nr3DCharActor Base assetbundle! " + wItem.assetPath);
		}
	}

	[DebuggerHidden]
	public IEnumerator LoadAsyncCallback(IDownloadedItem wItem, object kParamObject, string name, Type type)
	{
		NkBundleCallBack.<LoadAsyncCallback>c__IteratorA <LoadAsyncCallback>c__IteratorA = new NkBundleCallBack.<LoadAsyncCallback>c__IteratorA();
		<LoadAsyncCallback>c__IteratorA.wItem = wItem;
		<LoadAsyncCallback>c__IteratorA.kParamObject = kParamObject;
		<LoadAsyncCallback>c__IteratorA.name = name;
		<LoadAsyncCallback>c__IteratorA.type = type;
		<LoadAsyncCallback>c__IteratorA.<$>wItem = wItem;
		<LoadAsyncCallback>c__IteratorA.<$>kParamObject = kParamObject;
		<LoadAsyncCallback>c__IteratorA.<$>name = name;
		<LoadAsyncCallback>c__IteratorA.<$>type = type;
		<LoadAsyncCallback>c__IteratorA.<>f__this = this;
		return <LoadAsyncCallback>c__IteratorA;
	}

	public void StartDownloadPlayerAnimations()
	{
		List<string> playerCodeList = NrTSingleton<NrCharKindInfoManager>.Instance.GetPlayerCodeList();
		if (playerCodeList == null || playerCodeList.Count == 0)
		{
			return;
		}
		string text = string.Empty;
		foreach (string current in playerCodeList)
		{
			CHARKIND_INFO baseCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetBaseCharKindInfo(current);
			if (baseCharKindInfo != null)
			{
				text = "Char/Player/" + baseCharKindInfo.BUNDLE_PATH + "/";
				text = text + "ani/base/" + baseCharKindInfo.BUNDLE_PATH + "_baseani";
				this.RequestBundlePreload(text, NkBundleCallBack.PlayerBundleStackName, ItemType.ANIMATION, 0, current, NkBundleParam.eBundleType.BUNDLE_CHAR_ANIMATION, "Preload");
			}
		}
	}

	public void _PreloadPlayerModel()
	{
		List<string> playerCodeList = NrTSingleton<NrCharKindInfoManager>.Instance.GetPlayerCodeList();
		if (playerCodeList == null || playerCodeList.Count == 0)
		{
			return;
		}
		string text = string.Empty;
		foreach (string current in playerCodeList)
		{
			CHARKIND_INFO baseCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetBaseCharKindInfo(current);
			if (baseCharKindInfo != null)
			{
				text = "Char/Player/" + baseCharKindInfo.BUNDLE_PATH + "/";
				text = text + baseCharKindInfo.BUNDLE_PATH + "_bone";
				this.RequestBundlePreload(text, NkBundleCallBack.PlayerBundleStackName, ItemType.SKIN_BONE, 0, current, NkBundleParam.eBundleType.BUNDLE_CHAR_BONEPRELOAD, "Preload");
				this.m_fPreloadCount += 1f;
			}
		}
		this.m_fPreloadMaxCount = this.m_fPreloadCount;
	}
}
