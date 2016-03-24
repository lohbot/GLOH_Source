using GAME;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolRecruitSuccessGroupDlg : Form
{
	private const int ONEROWMAX = 6;

	private const float OPENTIME = 0.1f;

	private Label lb_Title;

	private NewListBox nlb_CardArea1;

	private NewListBox nlb_CardArea2;

	private Queue<bool> textureAllLoad = new Queue<bool>();

	private bool isStartTextureLoad;

	private Texture2D _backBG;

	private DrawTexture dt_Back;

	private Button bt_TouchArea;

	private Label lb_Touch;

	private float nowTime;

	private int nowIndex;

	private bool isCloseTouch;

	private SOLDIER_INFO[] solArray;

	public bool IsCloseTouch
	{
		get
		{
			return this.isCloseTouch;
		}
		set
		{
			this.isCloseTouch = value;
			this.lb_Touch.Visible = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolRecruitTwelve", G_ID.SOLRECRUITSUCCESS_GROUP_DLG, true);
		base.InteractivePanel.draggable = false;
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		base.TopMost = true;
		base.bCloseAni = false;
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		NrTSingleton<NkClientLogic>.Instance.SetCanOpenTicket(true);
		SolRecruitDlg solRecruitDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLRECRUIT_DLG) as SolRecruitDlg;
		if (solRecruitDlg != null)
		{
			solRecruitDlg.SetTicketList();
		}
		NrTSingleton<EventConditionHandler>.Instance.CloseUI.OnTrigger();
		base.OnClose();
	}

	public override void SetComponent()
	{
		this.lb_Title = (base.GetControl("LB_Title") as Label);
		this.nlb_CardArea1 = (base.GetControl("NLB_CardArea1") as NewListBox);
		this.nlb_CardArea2 = (base.GetControl("NLB_CardArea2") as NewListBox);
		this.dt_Back = (base.GetControl("DT_Back") as DrawTexture);
		this.dt_Back.SetTextureFromBundle("ui/soldier/background");
		this.bt_TouchArea = (base.GetControl("BT_TouchArea") as Button);
		this.bt_TouchArea.Click = new EZValueChangedDelegate(this.ClickClose);
		this.lb_Touch = (base.GetControl("LB_Touch") as Label);
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture("UI/pvp/cardback");
		if (null != texture)
		{
			this._backBG = texture;
		}
		else
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage("UI/pvp/cardback", new PostProcPerItem(this.SetImage));
		}
		this.IsCloseTouch = false;
		UIDataManager.MuteSound(false);
	}

	public void ShowList(int _index)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.solArray[_index].CharKind);
		if (charKindInfo == null)
		{
			return;
		}
		string imageKey;
		if (UIDataManager.IsUse256Texture())
		{
			imageKey = charKindInfo.GetPortraitFile1((int)this.solArray[_index].Grade, string.Empty) + "_256";
		}
		else
		{
			imageKey = charKindInfo.GetPortraitFile1((int)this.solArray[_index].Grade, string.Empty) + "_512";
		}
		NewListItem newListItem = new NewListItem(this.nlb_CardArea1.ColumnNum, true, string.Empty);
		newListItem.SetListItemData(0, this._backBG, null, null, null, null);
		for (int i = 1; i <= 6; i++)
		{
			newListItem.SetListItemData(i, false);
		}
		newListItem.SetListItemData((int)(this.solArray[_index].Grade + 1), true);
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey);
		newListItem.SetListItemData(7, texture, null, null, null, null);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			charKindInfo.GetSeason(this.solArray[_index].Grade) + 1
		});
		newListItem.SetListItemData(8, empty, null, null, null);
		newListItem.SetListItemData(9, true);
		newListItem.SetListItemData(10, true);
		newListItem.SetListItemData(11, charKindInfo.GetName(), null, null, null);
		if (_index < 6)
		{
			this.nlb_CardArea1.Add(newListItem);
			this.nlb_CardArea1.RepositionItems();
		}
		else
		{
			this.nlb_CardArea2.Add(newListItem);
			this.nlb_CardArea2.RepositionItems();
		}
	}

	public override void Update()
	{
		if (this.isStartTextureLoad && this.textureAllLoad.Count == 0 && Time.realtimeSinceStartup - this.nowTime >= 0.1f && this._backBG != null)
		{
			this.ShowList(this.nowIndex);
			this.nowTime = Time.realtimeSinceStartup;
			this.nowIndex++;
			if (this.nowIndex >= this.solArray.Length)
			{
				this.isStartTextureLoad = false;
				this.IsCloseTouch = true;
			}
		}
	}

	public void SetList(SOLDIER_INFO[] _solArray, string _itemName)
	{
		this.nowIndex = 0;
		this.lb_Title.SetText(_itemName);
		this.textureAllLoad.Clear();
		this.isStartTextureLoad = true;
		this.solArray = _solArray;
		for (int i = 0; i < _solArray.Length; i++)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.solArray[i].CharKind);
			if (charKindInfo == null)
			{
				return;
			}
			NewListItem newListItem = new NewListItem(this.nlb_CardArea1.ColumnNum, true, string.Empty);
			newListItem.SetListItemData(1, "Win_I_WorrGradeS" + this.solArray[i].Grade, null, null, null);
			string imageKey;
			if (UIDataManager.IsUse256Texture())
			{
				imageKey = charKindInfo.GetPortraitFile1((int)this.solArray[i].Grade, string.Empty) + "_256";
			}
			else
			{
				imageKey = charKindInfo.GetPortraitFile1((int)this.solArray[i].Grade, string.Empty) + "_512";
			}
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey))
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(imageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
				this.textureAllLoad.Enqueue(true);
			}
		}
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
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
					this.textureAllLoad.Dequeue();
				}
			}
		}
	}

	private void SetImage(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.GetSafeBundle() != null)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this._backBG = texture2D;
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
					return;
				}
			}
		}
	}

	private void ClickClose(IUIObject obj)
	{
		if (this.IsCloseTouch)
		{
			this.IsCloseTouch = false;
			this.CloseForm(null);
		}
		else if (this.textureAllLoad.Count == 0)
		{
			while (this.nowIndex < this.solArray.Length)
			{
				this.ShowList(this.nowIndex);
				this.nowIndex++;
			}
			this.isStartTextureLoad = false;
			this.IsCloseTouch = true;
		}
	}
}
