using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Item_Box_Random_Dlg : Form
{
	private const int N_RANDOM_TIME = 1500;

	private const int N_HIDE_TIME = 1500;

	private const int N_ITEM_ICON_CHANGE = 60;

	private const int N_ROTATE = -15;

	private Label m_laBoxItemName;

	private Label m_laItemName;

	private Label m_laItemNum;

	private DrawTexture m_dtItemIcon;

	private ItemTexture m_itSelectItemIcon;

	private DrawTexture m_dtBackAni;

	private Button m_buExit;

	private GameObject m_audioGO;

	private bool m_bCompleted;

	private bool m_bSelectItem;

	private bool m_bAutoClose = true;

	private int m_nTime;

	private int m_nItemChangeTime;

	private int m_nArrayIndex;

	private int m_lItemUnique;

	private Action<object> m_deDelegate;

	private object m_oObject;

	private Protocol_Item_Box.Roulette_Item[] m_saRouletteItem;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item_Box/DLG_Itembox_Random", G_ID.ITEM_BOX_RANDOM_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.m_laBoxItemName = (base.GetControl("Label_BoxName") as Label);
		this.m_laItemName = (base.GetControl("Label_ItemName") as Label);
		this.m_laItemNum = (base.GetControl("Label_ItemNUM") as Label);
		this.m_dtItemIcon = (base.GetControl("DrawTexture_Item_Img") as DrawTexture);
		this.m_itSelectItemIcon = (base.GetControl("ItemTexture_Item_Img") as ItemTexture);
		this.m_itSelectItemIcon.Visible = false;
		this.m_dtBackAni = (base.GetControl("DrawTexture_Ani") as DrawTexture);
		this.m_buExit = (base.GetControl("Button_Exit") as Button);
		Button expr_AC = this.m_buExit;
		expr_AC.Click = (EZValueChangedDelegate)Delegate.Combine(expr_AC.Click, new EZValueChangedDelegate(this.On_Exit_Button));
		base.SetScreenCenter();
	}

	public override void InitData()
	{
		this.m_nTime = (this.m_nItemChangeTime = Environment.TickCount);
		this.m_bCompleted = false;
		this.m_bSelectItem = false;
		this.m_nArrayIndex = 0;
		this.m_laBoxItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_lItemUnique);
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "ROULETTE", new PostProcPerItem(this.OnDownloaded_Sound));
	}

	public void OnDownloaded_Sound(IDownloadedItem wItem, object obj)
	{
		if (base.IsDestroy())
		{
			return;
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
				this.m_audioGO = new GameObject("@Audio : RandomBox_Audio", new Type[]
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
		if (!this.m_bCompleted && Environment.TickCount - this.m_nTime > 1500)
		{
			this.m_bCompleted = true;
			if (this.m_deDelegate != null)
			{
				this.m_deDelegate(this.m_oObject);
			}
		}
		if (!this.m_bSelectItem && Environment.TickCount - this.m_nItemChangeTime > 60)
		{
			this.m_nItemChangeTime = Environment.TickCount;
			this.m_dtItemIcon.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_saRouletteItem[this.m_nArrayIndex].m_nItemUnique);
			this.m_laItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_saRouletteItem[this.m_nArrayIndex].m_nItemUnique);
			this.m_laItemNum.Text = this.m_saRouletteItem[this.m_nArrayIndex].m_strText;
			this.m_nArrayIndex++;
			if (this.m_nArrayIndex >= this.m_saRouletteItem.Length)
			{
				this.m_nArrayIndex = 0;
			}
			this.m_dtBackAni.Rotate(-15f);
		}
		if (this.m_bSelectItem && this.m_bAutoClose && Environment.TickCount - this.m_nTime > 1500)
		{
			this.Close();
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_audioGO != null)
		{
			UnityEngine.Object.Destroy(this.m_audioGO);
			this.m_audioGO = null;
		}
	}

	private void On_Exit_Button(IUIObject a_cUIObject)
	{
		base.CloseNow();
	}

	public void SetTitle(string strTitel)
	{
		this.m_laBoxItemName.Text = strTitel;
	}

	public void Set_Item(int a_lItemUnique, Action<object> a_deDelegate, object a_oObject, Protocol_Item_Box.Roulette_Item[] a_saRouletteItem)
	{
		this.m_lItemUnique = a_lItemUnique;
		if (a_lItemUnique > 0)
		{
			this.m_deDelegate = a_deDelegate;
			if (a_deDelegate != null)
			{
				this.m_oObject = a_oObject;
				if (a_oObject != null)
				{
					this.m_saRouletteItem = a_saRouletteItem;
					if (a_saRouletteItem != null && a_saRouletteItem.Length != 0)
					{
						goto IL_4D;
					}
				}
			}
		}
		this.Close();
		IL_4D:
		this.m_dtItemIcon.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_saRouletteItem[0].m_nItemUnique);
		this.m_laItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_saRouletteItem[0].m_nItemUnique);
		this.m_laItemNum.Text = this.m_saRouletteItem[0].m_strText;
		this.Show();
	}

	public void Set_Item_Complete(ITEM a_lItem, int newItemNum, bool bAutoClose = true)
	{
		this.m_nTime = Environment.TickCount;
		this.m_bSelectItem = true;
		this.m_bAutoClose = bAutoClose;
		this.m_dtItemIcon.Visible = false;
		this.m_itSelectItemIcon.SetItemTexture(a_lItem, false);
		this.m_itSelectItemIcon.Visible = true;
		this.m_laItemName.Text = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(a_lItem.m_nItemUnique);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("572"),
			"Count",
			newItemNum.ToString()
		});
		this.m_laItemNum.Text = empty;
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(a_lItem.m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Container.RequestAudioClip("UI_ITEM", itemMaterialCode, "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "ROULETTE_POP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		if (this.m_audioGO != null)
		{
			UnityEngine.Object.Destroy(this.m_audioGO);
			this.m_audioGO = null;
		}
	}
}
