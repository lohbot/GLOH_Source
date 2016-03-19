using System;
using UnityEngine;
using UnityForms;

public class QuestReward_DLG : Form
{
	private ListBox m_kReputeRewardList;

	private DrawTexture m_BGImage;

	private Button m_Ok;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "NpcTalk/DLG_QUESTREWARD", G_ID.QUEST_REWARD, true);
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public override void SetComponent()
	{
		this.m_kReputeRewardList = (base.GetControl("ListBox_List") as ListBox);
		this.m_kReputeRewardList.UseColumnRect = true;
		this.m_kReputeRewardList.ColumnNum = 5;
		this.m_kReputeRewardList.LineHeight = 106f;
		this.m_kReputeRewardList.itemSpacing = 5f;
		this.m_kReputeRewardList.SelectStyle = "Win_B_ListBtn02";
		this.m_kReputeRewardList.AutoListBox = false;
		this.m_kReputeRewardList.touchScroll = false;
		this.m_kReputeRewardList.SetColumnRect(0, new Rect(6f, 6f, 94f, 94f));
		this.m_kReputeRewardList.SetColumnRect(1, new Rect(13f, 13f, 80f, 80f));
		this.m_kReputeRewardList.SetColumnRect(2, new Rect(108f, 24f, 350f, 24f), SpriteText.Anchor_Pos.Middle_Left, 24f);
		this.m_kReputeRewardList.SetColumnRect(3, new Rect(108f, 56f, 350f, 24f), SpriteText.Anchor_Pos.Middle_Left, 24f);
		this.m_kReputeRewardList.Reserve = false;
		if (this.m_kReputeRewardList.slider)
		{
			UnityEngine.Object.Destroy(this.m_kReputeRewardList.slider.gameObject);
		}
		this.m_Ok = (base.GetControl("Button_OK") as Button);
		this.m_Ok.AddValueChangedDelegate(new EZValueChangedDelegate(NrTSingleton<NkQuestManager>.Instance.OnClose));
		this.m_Ok.data = G_ID.QUEST_REWARD;
		this.m_BGImage = (base.GetControl("DrawTexture_Innerbg") as DrawTexture);
	}

	public void ClickOk(IUIObject obj)
	{
		this.Close();
	}

	public void SetRewardInfo(RewardInfo kInfo)
	{
		this.m_kReputeRewardList.Clear();
		for (byte b = 0; b < 3; b += 1)
		{
			if (kInfo != null)
			{
				if (0 >= (kInfo.bType[(int)b] & 1))
				{
					if (0 < (kInfo.bType[(int)b] & 16))
					{
						ListItem listItem = new ListItem();
						listItem.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
						listItem.SetColumnGUIContent(1, string.Empty, kInfo.imgLoder[(int)b]);
						listItem.SetColumnGUIContent(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1694"));
						string text = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1803"),
							"itemname",
							kInfo.str1[(int)b]
						});
						text = text + " " + kInfo.str2[(int)b];
						listItem.SetColumnGUIContent(3, text);
						this.m_kReputeRewardList.Add(listItem);
					}
					else if (0 < (kInfo.bType[(int)b] & 2) || 0 < (kInfo.bType[(int)b] & 8))
					{
						ListItem listItem2 = new ListItem();
						listItem2.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
						listItem2.SetColumnGUIContent(1, string.Empty, "Main_I_ExtraI02");
						listItem2.SetColumnGUIContent(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1695"));
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1698"),
							"exp",
							kInfo.str1[(int)b]
						});
						listItem2.SetColumnGUIContent(3, empty);
						this.m_kReputeRewardList.Add(listItem2);
					}
					else if (0 < (kInfo.bType[(int)b] & 4))
					{
						ListItem listItem3 = new ListItem();
						listItem3.SetColumnGUIContent(0, string.Empty, "Main_T_AreaBg3");
						listItem3.SetColumnGUIContent(1, string.Empty, "Main_I_ExtraI01");
						listItem3.SetColumnGUIContent(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1693"));
						listItem3.SetColumnGUIContent(3, kInfo.str1[(int)b]);
						this.m_kReputeRewardList.Add(listItem3);
					}
				}
			}
		}
		float num = this.m_kReputeRewardList.LineHeight * (float)this.m_kReputeRewardList.Count + this.m_kReputeRewardList.itemSpacing * (float)(this.m_kReputeRewardList.Count - 1);
		float num2 = 85f + num + this.m_Ok.GetSize().y;
		base.SetSize(base.GetSize().x, num2);
		this.m_BGImage.SetSize(this.m_BGImage.width, num + 10f);
		this.m_kReputeRewardList.ResizeViewableArea(this.m_kReputeRewardList.viewableArea.x, num);
		this.m_kReputeRewardList.RepositionItems();
		this.m_Ok.SetLocation(this.m_Ok.GetLocationX(), num2 - this.m_Ok.GetSize().y - 11f);
		base.SetScreenCenter();
	}
}
