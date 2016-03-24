using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class ExplorationRewardDlg : Form
{
	private DrawTexture m_BG;

	private Button m_btClose;

	private ListBox m_ItemList;

	private Label m_lbGold;

	private NewListBox m_nlSolInfo;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Exploration/Exploration_Result", G_ID.EXPLORATION_REWARD_DLG, false, true);
		base.ShowBlackBG(1f);
	}

	public override void SetComponent()
	{
		this.m_BG = (base.GetControl("Main_BG") as DrawTexture);
		this.m_BG.SetTextureFromBundle("UI/Exploration/MainBG");
		this._SetComponetSol();
		this._SetComponetRootItem();
		this._SetSolInfo();
		this._SetRewardItemInfo();
		base.ShowLayer(1);
		base.SetScreenCenter();
	}

	public void _SetComponetSol()
	{
		this.m_nlSolInfo = (base.GetControl("NewListBox_SolGetEXP") as NewListBox);
		this.m_nlSolInfo.Reserve = false;
	}

	public void _SetComponetRootItem()
	{
		this.m_ItemList = (base.GetControl("ListBox_RootItemList") as ListBox);
		this.m_ItemList.ColumnNum = 6;
		this.m_ItemList.LineHeight = 100f;
		this.m_ItemList.itemSpacing = 2f;
		this.m_ItemList.UseColumnRect = true;
		this.m_ItemList.SetColumnRect(0, 0, 0, 530, 100);
		this.m_ItemList.SetColumnRect(1, 13, 13, 74, 74);
		this.m_ItemList.SetColumnRect(2, 20, 20, 60, 60);
		this.m_ItemList.SetColumnRect(3, 101, 17, 300, 30, SpriteText.Anchor_Pos.Middle_Left, 28f);
		this.m_ItemList.SetColumnRect(4, 101, 53, 300, 30, SpriteText.Anchor_Pos.Middle_Left, 28f);
		this.m_ItemList.SetColumnRect(5, 423, 32, 80, 36, SpriteText.Anchor_Pos.Middle_Left, 32f);
		this.m_ItemList.Reserve = false;
		this.m_btClose = (base.GetControl("Button_ok") as Button);
		Button expr_10A = this.m_btClose;
		expr_10A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_10A.Click, new EZValueChangedDelegate(this.OnClickClose));
		this.m_lbGold = (base.GetControl("Label_getgold") as Label);
	}

	public void OnClickClose(IUIObject obj)
	{
		this.Close();
	}

	public override void OnClose()
	{
		NrTSingleton<ExplorationManager>.Instance.ClearReward();
	}

	private void _SetSolInfo()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			return;
		}
		int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_EXPLORATION_EXP);
		int num = 0;
		List<NkSoldierInfo> solInfo = NrTSingleton<ExplorationManager>.Instance.GetSolInfo();
		foreach (NkSoldierInfo current in solInfo)
		{
			if (NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(current.GetCharKind()) != null)
			{
				NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
				if (charPersonInfo == null)
				{
					return;
				}
				NewListItem newListItem = new NewListItem(this.m_nlSolInfo.ColumnNum, true, string.Empty);
				string text = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("471"),
					"targetname",
					current.GetName(),
					"count",
					current.GetLevel()
				});
				int num2 = 0;
				if (!current.IsMaxLevel())
				{
					num2 = value * (int)current.GetLevel() * NrTSingleton<ExplorationManager>.Instance.GetExpCount();
				}
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" ",
					NrTSingleton<CTextParser>.Instance.GetTextColor("1107"),
					"+",
					num2.ToString()
				});
				newListItem.SetListItemData(1, current.GetListSolInfo(false), null, null, null);
				newListItem.SetListItemData(2, text, null, null, null);
				NkSoldierInfo soldierInfoFromSolID = charPersonInfo.GetSoldierInfoFromSolID(current.GetSolID());
				if (soldierInfoFromSolID != null)
				{
					float num3 = soldierInfoFromSolID.GetExpPercent();
					string empty = string.Empty;
					if (num3 < 0f)
					{
						num3 = 0f;
					}
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672"),
						"Count",
						((int)(num3 * 100f)).ToString()
					});
					newListItem.SetListItemData(4, "Com_T_GauWaPr4", 400f * num3, null, null);
					newListItem.SetListItemData(5, empty, null, null, null);
				}
				this.m_nlSolInfo.Add(newListItem);
				num++;
			}
		}
		this.m_nlSolInfo.RepositionItems();
	}

	private void _SetRewardItemInfo()
	{
		Dictionary<int, ITEM> rewardItem = NrTSingleton<ExplorationManager>.Instance.GetRewardItem();
		foreach (ITEM current in rewardItem.Values)
		{
			ListItem listItem = new ListItem();
			listItem.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
			listItem.SetColumnGUIContent(1, string.Empty, "Win_I_CPortFrame");
			listItem.SetColumnGUIContent(2, current);
			string str = string.Empty;
			str = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current);
			listItem.SetColumnStr(3, str);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1804"),
				"itemnum",
				current.m_nItemNum.ToString()
			});
			listItem.SetColumnStr(5, empty);
			this.m_ItemList.Add(listItem);
		}
		this.m_ItemList.RepositionItems();
		string empty2 = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1721"),
			"gold",
			NrTSingleton<ExplorationManager>.Instance.GetRewardMoney()
		});
		this.m_lbGold.Text = empty2;
	}

	public override void Show()
	{
		base.Show();
		this.m_nlSolInfo.clipWhenMoving = true;
	}
}
