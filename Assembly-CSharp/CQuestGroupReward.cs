using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CQuestGroupReward : Form
{
	private DrawTexture m_DrawTexture_Bg2;

	private DrawTexture m_DrawTexture_Bg1;

	private DrawTexture m_DrawTexture_Deco1;

	private DrawTexture m_DrawTexture_Deco2;

	private DrawTexture m_DrawTexture_Deco3;

	private DrawTexture m_DrawTexture_Deco4;

	private DrawTexture m_DrawTexture_Star1;

	private DrawTexture m_DrawTexture_Star2;

	private DrawTexture m_DrawTexture_Star3;

	private DrawTexture m_DrawTexture_Star4;

	private DrawTexture m_DrawTexture_Star5;

	private DrawTexture m_DrawTexture_Item;

	private Label m_Label_Chapter;

	private Label m_Label_ChapterTitle;

	private Label m_Label_StageName;

	private Button m_Button_Ok;

	private GameObject m_gobjUseRecruitAroundEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NpcTalk/DLG_Reward", G_ID.QUEST_GROUP_REWARD, false);
		float x = (GUICamera.width - base.GetSize().x) / 2f;
		float y = (GUICamera.height - base.GetSize().y) / 2f;
		base.SetLocation(x, y);
	}

	public override void SetComponent()
	{
		this.m_DrawTexture_Bg2 = (base.GetControl("DrawTexture_Bg2") as DrawTexture);
		this.m_DrawTexture_Bg1 = (base.GetControl("DrawTexture_Bg1") as DrawTexture);
		this.m_DrawTexture_Deco1 = (base.GetControl("DrawTexture_Deco1") as DrawTexture);
		this.m_DrawTexture_Deco2 = (base.GetControl("DrawTexture_Deco2") as DrawTexture);
		this.m_DrawTexture_Deco3 = (base.GetControl("DrawTexture_Deco3") as DrawTexture);
		this.m_DrawTexture_Deco4 = (base.GetControl("DrawTexture_Deco4") as DrawTexture);
		this.m_DrawTexture_Star1 = (base.GetControl("DrawTexture_Star1") as DrawTexture);
		this.m_DrawTexture_Star2 = (base.GetControl("DrawTexture_Star2") as DrawTexture);
		this.m_DrawTexture_Star3 = (base.GetControl("DrawTexture_Star3") as DrawTexture);
		this.m_DrawTexture_Star4 = (base.GetControl("DrawTexture_Star4") as DrawTexture);
		this.m_DrawTexture_Star5 = (base.GetControl("DrawTexture_Star5") as DrawTexture);
		this.m_DrawTexture_Item = (base.GetControl("DrawTexture_Item") as DrawTexture);
		this.m_Label_Chapter = (base.GetControl("Label_Chapter") as Label);
		this.m_Label_ChapterTitle = (base.GetControl("Label_ChapterTitle") as Label);
		this.m_Label_StageName = (base.GetControl("Label_StageName") as Label);
		this.m_Button_Ok = (base.GetControl("Button_Ok") as Button);
		Button expr_166 = this.m_Button_Ok;
		expr_166.Click = (EZValueChangedDelegate)Delegate.Combine(expr_166.Click, new EZValueChangedDelegate(NrTSingleton<NkQuestManager>.Instance.OnClose));
		this.m_Button_Ok.data = G_ID.QUEST_GROUP_REWARD;
		this.m_DrawTexture_Bg2.SetAlpha(0.7f);
		this.m_DrawTexture_Bg1.SetAlpha(0.9f);
		this.m_DrawTexture_Deco1.SetTextureUVs(new Vector2(971f, 138f), new Vector2(50f, 96f));
		this.m_DrawTexture_Deco2.SetTextureUVs(new Vector2(971f, 138f), new Vector2(50f, 96f));
		this.m_DrawTexture_Deco3.SetTextureUVs(new Vector2(981f, 109f), new Vector2(40f, 68f));
		this.m_DrawTexture_Deco4.SetTextureUVs(new Vector2(981f, 109f), new Vector2(40f, 68f));
	}

	public void SetData(GS_QUEST_GROUP_REWARD_ACK kInfo)
	{
		CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(kInfo.i32QuestGroupUnique);
		if (questGroupByGroupUnique == null)
		{
			return;
		}
		this.SetGrade(kInfo.i32Grade);
		this.m_DrawTexture_Item.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(kInfo.stItemInfo.m_nItemUnique);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
		string text = questGroupByGroupUnique.GetChapterUnique().ToString() + textFromInterface2 + questGroupByGroupUnique.GetPage() + textFromInterface;
		string groupTitle = questGroupByGroupUnique.GetGroupTitle();
		string text2 = text + " " + groupTitle;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(kInfo.stItemInfo);
		this.m_Label_Chapter.Text = text;
		this.m_Label_ChapterTitle.Text = groupTitle;
		string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1239");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface3,
			"targetname1",
			text2,
			"targetname2",
			itemNameByItemUnique,
			"targetname3",
			kInfo.stItemInfo.m_nItemNum
		});
		this.m_Label_StageName.Text = empty;
		this.SetGrade(kInfo.i32Grade);
		Vector3 vector = default(Vector3);
		vector.x = base.GetLocation().x + 295f;
		vector.y = base.GetLocation().y - 190f;
		vector.z = this.m_DrawTexture_Item.transform.position.z;
	}

	private void _funcDownloaded(IDownloadedItem wItem, object obj)
	{
		try
		{
			GameObject original = wItem.mainAsset as GameObject;
			this.m_gobjUseRecruitAroundEffect = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity);
			NkUtil.SetAllChildLayer(this.m_gobjUseRecruitAroundEffect, GUICamera.UILayer);
			Vector3 position = default(Vector3);
			position.x = base.GetLocation().x + 295f;
			position.y = base.GetLocation().y - 190f;
			position.z = this.m_DrawTexture_Item.transform.position.z;
			this.m_gobjUseRecruitAroundEffect.transform.position = position;
			this.m_gobjUseRecruitAroundEffect.transform.parent = base.InteractivePanel.gameObject.transform;
			this.m_gobjUseRecruitAroundEffect.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
		}
	}

	private void SetGrade(int i32Grade)
	{
		switch (i32Grade)
		{
		case 1:
			this.m_DrawTexture_Star1.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star2.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star3.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star4.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star5.SetTexture("Com_I_Star12");
			break;
		case 2:
			this.m_DrawTexture_Star1.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star2.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star3.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star4.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star5.SetTexture("Com_I_Star12");
			break;
		case 3:
			this.m_DrawTexture_Star1.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star2.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star3.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star4.SetTexture("Com_I_Star12");
			this.m_DrawTexture_Star5.SetTexture("Com_I_Star12");
			break;
		case 4:
			this.m_DrawTexture_Star1.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star2.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star3.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star4.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star5.SetTexture("Com_I_Star12");
			break;
		case 5:
			this.m_DrawTexture_Star1.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star2.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star3.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star4.SetTexture("Com_I_Star11");
			this.m_DrawTexture_Star5.SetTexture("Com_I_Star11");
			break;
		}
	}
}
