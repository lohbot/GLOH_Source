using System;
using UnityForms;

public class QuestGradeSelect_DLG : Form
{
	private int m_i32SelGrade = -1;

	private int m_i32CurGrade = 1;

	private Button m_QuestGrade_okbutton;

	private Button m_QuestGrade_cancelbutton;

	private DrawTexture m_DrawTexture_DrawTexture41;

	private DrawTexture m_DrawTexture_DrawTexture42;

	private DrawTexture m_DrawTexture_DrawTexture43;

	private DrawTexture m_DrawTexture_DrawTexture44;

	private DrawTexture m_DrawTexture_DrawTexture45;

	private DropDownList m_DropDownList_DropDownList46;

	public int I32SelGrade
	{
		get
		{
			return this.m_i32SelGrade;
		}
		set
		{
			this.m_i32SelGrade = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestGradeSelect/DLG_QuestGradeSelect", G_ID.QUEST_GRADESELECT_DLG, true);
		float x = (GUICamera.width - base.GetSize().x) / 2f;
		float y = (GUICamera.height - base.GetSize().y) / 2f;
		base.SetLocation(x, y);
	}

	public override void SetComponent()
	{
		this.m_QuestGrade_okbutton = (base.GetControl("QuestGrade_okbutton") as Button);
		this.m_QuestGrade_cancelbutton = (base.GetControl("QuestGrade_okbutton") as Button);
		this.m_DrawTexture_DrawTexture41 = (base.GetControl("DrawTexture_DrawTexture41") as DrawTexture);
		this.m_DrawTexture_DrawTexture42 = (base.GetControl("DrawTexture_DrawTexture42") as DrawTexture);
		this.m_DrawTexture_DrawTexture43 = (base.GetControl("DrawTexture_DrawTexture43") as DrawTexture);
		this.m_DrawTexture_DrawTexture44 = (base.GetControl("DrawTexture_DrawTexture44") as DrawTexture);
		this.m_DrawTexture_DrawTexture45 = (base.GetControl("DrawTexture_DrawTexture45") as DrawTexture);
		this.m_DropDownList_DropDownList46 = (base.GetControl("DropDownList_DropDownList46") as DropDownList);
		Button expr_B6 = this.m_QuestGrade_okbutton;
		expr_B6.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B6.Click, new EZValueChangedDelegate(this.BtnOk));
		Button expr_DD = this.m_QuestGrade_cancelbutton;
		expr_DD.Click = (EZValueChangedDelegate)Delegate.Combine(expr_DD.Click, new EZValueChangedDelegate(this.BtnCancle));
		this.m_DropDownList_DropDownList46.Clear();
		this.m_DropDownList_DropDownList46.Add("1");
		this.m_DropDownList_DropDownList46.Add("2");
		this.m_DropDownList_DropDownList46.Add("3");
		this.m_DropDownList_DropDownList46.Add("4");
		this.m_DropDownList_DropDownList46.Add("5");
		this.m_DropDownList_DropDownList46.RepositionItems();
		this.m_DropDownList_DropDownList46.SetFirstItem();
		this.m_DropDownList_DropDownList46.AddValueChangedDelegate(new EZValueChangedDelegate(this.DropDownGrade));
	}

	private void SetGrade(int i32CurGrade)
	{
		switch (i32CurGrade)
		{
		case 1:
			this.m_DrawTexture_DrawTexture41.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture42.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture43.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture44.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture45.SetTexture("Com_I_Star22");
			break;
		case 2:
			this.m_DrawTexture_DrawTexture41.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture42.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture43.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture44.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture45.SetTexture("Com_I_Star22");
			break;
		case 3:
			this.m_DrawTexture_DrawTexture41.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture42.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture43.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture44.SetTexture("Com_I_Star22");
			this.m_DrawTexture_DrawTexture45.SetTexture("Com_I_Star22");
			break;
		case 4:
			this.m_DrawTexture_DrawTexture41.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture42.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture43.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture44.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture45.SetTexture("Com_I_Star22");
			break;
		case 5:
			this.m_DrawTexture_DrawTexture41.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture42.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture43.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture44.SetTexture("Com_I_Star21");
			this.m_DrawTexture_DrawTexture45.SetTexture("Com_I_Star21");
			break;
		}
	}

	public void SetQuestUnique(CQuest kQuest)
	{
		int num = 1;
		USER_QUEST_COMPLETE_INFO completeQuestInfo = NrTSingleton<NkQuestManager>.Instance.GetCompleteQuestInfo(kQuest.GetQuestGroupUnique());
		if (completeQuestInfo != null)
		{
			num = completeQuestInfo.i32LastGrade;
		}
		this.m_i32CurGrade = num;
		this.SetGrade(num);
	}

	private void DropDownGrade(IUIObject obj)
	{
		DropDownList dropDownList = (DropDownList)obj;
		this.m_i32SelGrade = dropDownList.SelectIndex + 1;
	}

	private void BtnOk(IUIObject obj)
	{
		if (0 < this.m_i32SelGrade && this.m_i32CurGrade + 1 >= this.m_i32SelGrade)
		{
			NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
			if (npcTalkUI_DLG != null)
			{
				npcTalkUI_DLG.SetGrade(this.m_i32SelGrade);
				this.Hide();
			}
		}
	}

	private void BtnCancle(IUIObject obj)
	{
		this.Hide();
	}

	public override void OnClose()
	{
		this.Hide();
	}
}
