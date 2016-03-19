using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Agit_GoldenEggDramaDlg : Form
{
	private enum COMMANDSTATE
	{
		NONE,
		SHOW_GOLDENEGG,
		SHOW_WHITEEGG
	}

	private const int ITEMDLG_SHOWTIME_TICK = 7400;

	private const int CLOSE_TIME_TICK = 8500;

	private GameObject m_rootGameObject;

	private GameObject m_goldenEgg;

	private GameObject m_whiiteEgg;

	private GameObject m_audioGO;

	private Agit_GoldenEggDramaDlg.COMMANDSTATE m_eState;

	private ITEM m_item;

	private int m_itemDlgShowTickTime;

	private int m_CloseTickTime;

	private bool m_bIsShowedReward;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_goldenegg_drama", G_ID.AGIT_GOLDENEGGDRAMA_DLG, false);
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		string str = string.Format("effect/instant/{0}{1}", "fx_direct_goldegg", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SetBackImage), "fx_direct_goldegg");
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void SetBackImage(WWWItem _item, object _param)
	{
		if (null == _item.GetSafeBundle())
		{
			return;
		}
		if (null == _item.GetSafeBundle().mainAsset)
		{
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (null != gameObject)
		{
			this.m_rootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			this.m_rootGameObject.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
			effectUIPos.z = 300f;
			this.m_rootGameObject.transform.position = effectUIPos;
			NkUtil.SetAllChildLayer(this.m_rootGameObject, GUICamera.UILayer);
			this.m_goldenEgg = NkUtil.GetChild(this.m_rootGameObject.transform, "fx_goldegg").gameObject;
			this.m_whiiteEgg = NkUtil.GetChild(this.m_rootGameObject.transform, "fx_whiteegg").gameObject;
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_rootGameObject);
			}
		}
	}

	public void SetItem(ITEM item)
	{
		this.m_item = item;
	}

	public void ShowGoldenEgg()
	{
		this.m_eState = Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_GOLDENEGG;
	}

	public void ShowWhiteEgg()
	{
		this.m_eState = Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_WHITEEGG;
	}

	public void OnDownloaded_Sound(IDownloadedItem wItem, object obj)
	{
		if (base.IsDestroy())
		{
			return;
		}
		if (this.m_audioGO != null)
		{
			UnityEngine.Object.Destroy(this.m_audioGO);
			this.m_audioGO = null;
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
		else
		{
			TsAudio.RequestData requestData = obj as TsAudio.RequestData;
			TsAudio tsAudio = TsAudioCreator.Create(requestData.baseData);
			if (tsAudio != null)
			{
				this.m_audioGO = new GameObject("@Audio : GoldenEgg_Audio", new Type[]
				{
					typeof(AudioSource)
				});
				tsAudio.RefAudioClip = (wItem.mainAsset as AudioClip);
				tsAudio.RefAudioSource = this.m_audioGO.audio;
				tsAudio.Play();
				wItem.unloadImmediate = true;
			}
		}
	}

	public override void Update()
	{
		if (this.m_rootGameObject == null || this.m_goldenEgg == null || this.m_whiiteEgg == null)
		{
			return;
		}
		if (this.m_eState != Agit_GoldenEggDramaDlg.COMMANDSTATE.NONE)
		{
			this.m_itemDlgShowTickTime = Environment.TickCount + 7400;
			this.m_CloseTickTime = Environment.TickCount + 8500;
			this.m_rootGameObject.SetActive(true);
			this.m_goldenEgg.SetActive(this.m_eState == Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_GOLDENEGG);
			this.m_whiiteEgg.SetActive(this.m_eState == Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_WHITEEGG);
			this.Show();
			if (this.m_eState == Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_GOLDENEGG)
			{
				TsAudioManager.Container.RequestAudioClip("UI_SFX", "EGG", "GOLD", new PostProcPerItem(this.OnDownloaded_Sound));
			}
			else if (this.m_eState == Agit_GoldenEggDramaDlg.COMMANDSTATE.SHOW_WHITEEGG)
			{
				TsAudioManager.Container.RequestAudioClip("UI_SFX", "EGG", "NORMAL", new PostProcPerItem(this.OnDownloaded_Sound));
			}
			this.m_eState = Agit_GoldenEggDramaDlg.COMMANDSTATE.NONE;
		}
		if (this.m_itemDlgShowTickTime != 0 && Environment.TickCount >= this.m_itemDlgShowTickTime)
		{
			this.ShowReward();
			this.m_itemDlgShowTickTime = 0;
		}
		if (this.m_CloseTickTime != 0 && Environment.TickCount >= this.m_CloseTickTime)
		{
			this.Close();
		}
	}

	private void ShowReward()
	{
		Item_Box_Random_Dlg item_Box_Random_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEM_BOX_RANDOM_DLG) as Item_Box_Random_Dlg;
		if (item_Box_Random_Dlg != null && this.m_item != null)
		{
			item_Box_Random_Dlg.SetLocation(item_Box_Random_Dlg.GetLocationX(), item_Box_Random_Dlg.GetLocationY(), 280f);
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2845");
			item_Box_Random_Dlg.SetTitle(textFromInterface);
			item_Box_Random_Dlg.Set_Item_Complete(this.m_item, this.m_item.m_nItemNum, false);
		}
		GS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ = new GS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ();
		gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ.i32GoldenEggGetCount = NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetCount();
		gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ.i64LastGetPersonID = NrTSingleton<NewGuildManager>.Instance.GetGoldenEggGetLastPerson();
		SendPacket.GetInstance().SendObject(2321, gS_NEWGUILD_AGIT_GOLDENEGG_INFO_REQ);
		this.m_bIsShowedReward = true;
	}

	public override void OnClose()
	{
		if (!this.m_bIsShowedReward)
		{
			this.ShowReward();
		}
		base.OnClose();
		UIDataManager.MuteSound(false);
		if (null != this.m_rootGameObject)
		{
			UnityEngine.Object.Destroy(this.m_rootGameObject);
			this.m_rootGameObject = null;
		}
		if (this.m_audioGO != null)
		{
			UnityEngine.Object.Destroy(this.m_audioGO);
			this.m_audioGO = null;
		}
		Resources.UnloadUnusedAssets();
	}
}
