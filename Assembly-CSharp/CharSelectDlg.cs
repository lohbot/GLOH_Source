using System;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CharSelectDlg : Form
{
	private DrawTexture m_txSelect;

	private Button m_SelectButton;

	private Button[] m_btCharSlot = new Button[3];

	private Button[] m_btDelete = new Button[3];

	private FlashLabel[] m_CharName = new FlashLabel[3];

	private FlashLabel[] m_CharLv = new FlashLabel[3];

	private FlashLabel[] m_CharCreateText = new FlashLabel[3];

	private DrawTexture[] m_txCharSlot = new DrawTexture[3];

	private DrawTexture[] m_txNewCharBK = new DrawTexture[3];

	private DrawTexture[] m_txNameBK = new DrawTexture[3];

	private DrawTexture[] m_txShadow = new DrawTexture[3];

	private DrawTexture m_txMainBG;

	public Button SelectButton
	{
		get
		{
			return this.m_SelectButton;
		}
		set
		{
			this.m_SelectButton = value;
			this.m_txSelect.SetSize(value.GetSize().x, value.GetSize().y);
			this.m_txSelect.SetLocation(value.GetLocationX(), value.GetLocationY());
			this.m_txSelect.Visible = true;
		}
	}

	public Button[] CharSlot
	{
		get
		{
			return this.m_btCharSlot;
		}
		set
		{
			this.m_btCharSlot = value;
		}
	}

	public Button[] Delete
	{
		get
		{
			return this.m_btDelete;
		}
		set
		{
			this.m_btDelete = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "SelectChar/dlg_charselect", G_ID.CHAR_SELECT_DLG, false);
	}

	public override void SetComponent()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_btCharSlot[i] = (base.GetControl(string.Format("Button_SelectButton{0}", (i + 1).ToString("00"))) as Button);
			this.m_btCharSlot[i].EffectAni = false;
			this.m_btCharSlot[i].TabIndex = i + 1;
			this.m_btDelete[i] = (base.GetControl(string.Format("Button_DeleteButton{0}", (i + 1).ToString("00"))) as Button);
			this.m_btDelete[i].TabIndex = i + 1;
			this.m_txCharSlot[i] = (base.GetControl(string.Format("DrawTexture_CharSlot{0}", (i + 1).ToString("00"))) as DrawTexture);
			this.m_txNewCharBK[i] = (base.GetControl(string.Format("DrawTexture_NewCharBK{0}", (i + 1).ToString("00"))) as DrawTexture);
			this.m_txNameBK[i] = (base.GetControl(string.Format("DrawTexture_NameBk{0}", (i + 1).ToString("00"))) as DrawTexture);
			this.m_CharName[i] = (base.GetControl(string.Format("FlashLabel_CharName{0}", (i + 1).ToString("00"))) as FlashLabel);
			this.m_CharName[i].Visible = true;
			this.m_CharLv[i] = (base.GetControl(string.Format("FlashLabel_CharLevel{0}", (i + 1).ToString("00"))) as FlashLabel);
			this.m_CharLv[i].Visible = true;
			this.m_CharCreateText[i] = (base.GetControl(string.Format("FlashLabel_CharCreateText{0}", (i + 1).ToString("00"))) as FlashLabel);
			this.m_txShadow[i] = (base.GetControl(string.Format("DrawTexture_SlotShadow{0}", (i + 1).ToString("00"))) as DrawTexture);
		}
		this.m_txSelect = (base.GetControl("DrawTexture_Select") as DrawTexture);
		this.m_txSelect.Visible = false;
		this.m_txMainBG = (base.GetControl("DrawTexture_MainBG") as DrawTexture);
		base.SetScreenCenter();
		this.m_txMainBG.SetLocation(-base.GetLocationX(), -base.GetLocationY());
		this.m_txMainBG.SetSize(GUICamera.width, GUICamera.height);
		base.ShowLayer(1);
		this.InitImg();
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetScreenCenter();
		this.m_txMainBG.SetLocation(-base.GetLocationX(), -base.GetLocationY());
		this.m_txMainBG.SetSize(GUICamera.width, GUICamera.height);
	}

	public void SetCharSet(int nSlotIdx, int Level, string name, string CharTribe, long _PersonID, int CHARKIND)
	{
		if (nSlotIdx > 0 && nSlotIdx <= 3)
		{
			this.m_CharName[nSlotIdx - 1].SetFlashLabel(name);
			this.m_CharName[nSlotIdx - 1].Data = name;
			this.m_CharLv[nSlotIdx - 1].SetFlashLabel(string.Format("Lv. {0}", Level));
			this.m_btCharSlot[nSlotIdx - 1].Data = _PersonID;
			this.SetTribeImg(this.m_txCharSlot[nSlotIdx - 1], CharTribe, CHARKIND);
		}
	}

	public void ShowHideCharName(int nSlotIdx, bool bShow)
	{
		if (nSlotIdx > 0 && nSlotIdx <= 3)
		{
			this.m_CharName[nSlotIdx - 1].Visible = bShow;
			this.m_CharLv[nSlotIdx - 1].Visible = bShow;
			this.m_btDelete[nSlotIdx - 1].Visible = bShow;
			this.m_txNameBK[nSlotIdx - 1].Visible = bShow;
			this.m_txNewCharBK[nSlotIdx - 1].Visible = !bShow;
			this.m_CharCreateText[nSlotIdx - 1].Visible = !bShow;
			if (!bShow)
			{
				this.BlankImg(this.m_txCharSlot[nSlotIdx - 1]);
			}
		}
	}

	public bool GetShowChar(int nSlotIdx)
	{
		return nSlotIdx > 0 && nSlotIdx <= 3 && this.m_CharName[nSlotIdx - 1].Visible;
	}

	public string GetName(int nSlotIdx)
	{
		if (this.GetShowChar(nSlotIdx))
		{
			return this.m_CharName[nSlotIdx - 1].Data as string;
		}
		return string.Empty;
	}

	public long GetPersonID(int nSlotIdx)
	{
		if (this.GetShowChar(nSlotIdx))
		{
			return (long)this.m_btCharSlot[nSlotIdx - 1].Data;
		}
		return 0L;
	}

	private void InitImg()
	{
		this.m_txMainBG.SetTextureFromBundle("UI/charselect/ChaSelect_BG");
		for (int i = 0; i < 3; i++)
		{
			this.m_txShadow[i].SetTextureFromBundle("UI/charselect/ChShadow");
		}
	}

	public void BlankImg(DrawTexture _img)
	{
		_img.SetTextureFromBundle("UI/charselect/blank");
	}

	public void SetTribeImg(DrawTexture _img, string _Tribe, int CHARKIND)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (CHARKIND % 2 == 1)
		{
			stringBuilder.Append(string.Format("UI/CharSelect/{0}male", _Tribe));
		}
		else
		{
			stringBuilder.Append(string.Format("UI/CharSelect/{0}female", _Tribe));
		}
		_img.SetTextureFromBundle(stringBuilder.ToString());
	}

	public Texture2D RequestDownload(string strAssetPath, PostProcPerItem callbackDelegate, object obj)
	{
		Texture2D texture2D = null;
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(strAssetPath + Option.extAsset, null);
		if (wWWItem != null && (wWWItem.isCanceled || !wWWItem.canAccessAssetBundle))
		{
			wWWItem.SetItemType(ItemType.TEXTURE2D);
			wWWItem.SetCallback(callbackDelegate, obj);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		if (wWWItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wWWItem.assetPath
			});
		}
		else
		{
			texture2D = (wWWItem.mainAsset as Texture2D);
			DrawTexture drawTexture = obj as DrawTexture;
			if (drawTexture != null)
			{
				drawTexture.SetTexture(texture2D);
			}
		}
		return texture2D;
	}

	private void _OnImageProcess(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			Texture2D texture = wItem.mainAsset as Texture2D;
			DrawTexture drawTexture = obj as DrawTexture;
			if (drawTexture != null)
			{
				drawTexture.SetTexture(texture);
			}
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
	}

	private void _OnShadowImageProcess(IDownloadedItem wItem, object objs)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			Texture2D texture = wItem.mainAsset as Texture2D;
			DrawTexture[] array = objs as DrawTexture[];
			DrawTexture[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DrawTexture drawTexture = array2[i];
				if (drawTexture != null)
				{
					drawTexture.SetTexture(texture);
				}
			}
		}
		if (wItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
		}
	}

	public Texture2D RequestDownloads(string strAssetPath, PostProcPerItem callbackDelegate, object objs)
	{
		Texture2D texture2D = null;
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(strAssetPath + Option.extAsset, null);
		if (wWWItem != null && (wWWItem.isCanceled || !wWWItem.canAccessAssetBundle))
		{
			wWWItem.SetItemType(ItemType.TEXTURE2D);
			wWWItem.SetCallback(callbackDelegate, objs);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		if (wWWItem.mainAsset == null)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wWWItem.assetPath
			});
		}
		else
		{
			texture2D = (wWWItem.mainAsset as Texture2D);
			DrawTexture[] array = objs as DrawTexture[];
			DrawTexture[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				DrawTexture drawTexture = array2[i];
				if (drawTexture != null)
				{
					drawTexture.SetTexture(texture2D);
				}
			}
		}
		return texture2D;
	}

	public override void InitData()
	{
		base.InitData();
		base.SetScreenCenter();
		this.m_txMainBG.SetLocation(-base.GetLocationX(), -base.GetLocationY());
		this.m_txMainBG.SetSize(GUICamera.width, GUICamera.height);
		TsLog.Log("GUICamera.width : {0} GUICamera.height : {1}", new object[]
		{
			GUICamera.width,
			GUICamera.height
		});
	}
}
