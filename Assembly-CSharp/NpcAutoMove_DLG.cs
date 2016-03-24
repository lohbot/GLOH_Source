using System;
using UnityForms;

public class NpcAutoMove_DLG : Form
{
	private int[] AutoNpc = new int[]
	{
		4499,
		68,
		109,
		125,
		610,
		640,
		595,
		4619,
		23
	};

	private NewListBox m_NpcAutoMoveList;

	private Button m_btnConfiem;

	private DrawTexture m_txNPCImage;

	private Label m_lbNPCInfoText;

	private Label m_lbNPCNameText;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "system/dlg_npcautomove", G_ID.NPC_AUTOMOVE_DLG, true);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public override void SetComponent()
	{
		this.m_NpcAutoMoveList = (base.GetControl("NLB_NpcAutoMove") as NewListBox);
		this.m_NpcAutoMoveList.Reserve = false;
		this.m_NpcAutoMoveList.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnNPCClick));
		this.m_btnConfiem = (base.GetControl("Button_Button12") as Button);
		this.m_btnConfiem.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClick));
		this.m_txNPCImage = (base.GetControl("DT_Npcimg01") as DrawTexture);
		this.m_lbNPCInfoText = (base.GetControl("LB_Npcinfo") as Label);
		this.m_lbNPCNameText = (base.GetControl("LB_NpcName02") as Label);
		this.m_btClose = (base.GetControl("Close_Button") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.SetNpcList();
	}

	private void SetNpcList()
	{
		for (int i = 0; i < this.AutoNpc.Length; i++)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.AutoNpc[i]);
			if (charKindInfo != null)
			{
				if (!NrTSingleton<ContentsLimitManager>.Instance.IsNPCLimit(charKindInfo.GetCharKind()))
				{
					if (charKindInfo.IsATB(512L))
					{
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsPointExchage())
						{
							goto IL_113;
						}
					}
					else if (charKindInfo.IsATB(1152921504606846976L))
					{
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsGuildWarExchangeLimit())
						{
							goto IL_113;
						}
					}
					else if (charKindInfo.IsATB(549755813888L) && !NrTSingleton<ContentsLimitManager>.Instance.IsItemLevelCheckBlock() && !NrTSingleton<ContentsLimitManager>.Instance.IsItemEvolution(false))
					{
						goto IL_113;
					}
					NewListItem newListItem = new NewListItem(3, true, string.Empty);
					newListItem.SetListItemData(1, charKindInfo.GetCharKind(), null, null, null);
					newListItem.SetListItemData(2, charKindInfo.GetName(), null, null, null);
					newListItem.Data = charKindInfo.GetCharKind();
					this.m_NpcAutoMoveList.Add(newListItem);
				}
			}
			IL_113:;
		}
		this.m_NpcAutoMoveList.RepositionItems();
		this.m_NpcAutoMoveList.SetSelectedItem(0);
		this.OnNPCClick(null);
	}

	private void OnClick(IUIObject Obj)
	{
		if (null == this.m_NpcAutoMoveList.SelectedItem)
		{
			return;
		}
		int num = (int)this.m_NpcAutoMoveList.SelectedItem.Data;
		if (num == 4619)
		{
			if (NrTSingleton<NewGuildManager>.Instance.IsExitAgit())
			{
				NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_ENTER_REQ();
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("843"), SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE_GREEN);
			}
		}
		else if (num != 0)
		{
			NrTSingleton<NkQuestManager>.Instance.NPCAutoMove(num);
		}
		this.Close();
	}

	private void OnNPCClick(IUIObject Obj)
	{
		if (null == this.m_NpcAutoMoveList.SelectedItem)
		{
			return;
		}
		this.m_txNPCImage.SetTexture(string.Empty);
		this.m_lbNPCNameText.SetText(string.Empty);
		int num = (int)this.m_NpcAutoMoveList.SelectedItem.Data;
		if (num != 0)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
			if (charKindInfo != null)
			{
				this.m_txNPCImage.SetTexture(eCharImageType.LARGE, num, -1, string.Empty);
				this.m_lbNPCNameText.SetText(charKindInfo.GetName());
				this.m_lbNPCInfoText.SetText(charKindInfo.GetDesc());
			}
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MAINMENU_DLG);
	}
}
