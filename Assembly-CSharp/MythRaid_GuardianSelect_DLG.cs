using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class MythRaid_GuardianSelect_DLG : Form
{
	private Button[] btn_Guardian_Select = new Button[4];

	private DrawTexture[] dt_Guadian_Select = new DrawTexture[4];

	private Label[] lb_Guardian_Name = new Label[4];

	private Label[] lb_UserName = new Label[4];

	private DrawTexture[] dt_Guardian_Img = new DrawTexture[4];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "mythraid/dlg_myth_guardianselect", G_ID.MYTHRAID_GUARDIANSELECT_DLG, false);
	}

	public override void OnClose()
	{
	}

	public override void SetComponent()
	{
		base.SetScreenCenter();
		for (int i = 0; i < 4; i++)
		{
			int num = i + 1;
			this.btn_Guardian_Select[i] = (base.GetControl("BTN_Guardian_Select_" + num.ToString()) as Button);
			this.dt_Guadian_Select[i] = (base.GetControl("DT_Guadian_Select" + num.ToString()) as DrawTexture);
			this.lb_Guardian_Name[i] = (base.GetControl("LB_Guardian_Name" + num.ToString()) as Label);
			this.lb_UserName[i] = (base.GetControl("LB_UserName_" + num.ToString()) as Label);
			this.dt_Guardian_Img[i] = (base.GetControl("DT_Guardian_Img" + num.ToString()) as DrawTexture);
			this.dt_Guadian_Select[i].Visible = false;
			this.lb_UserName[i].Visible = false;
			this.btn_Guardian_Select[i].data = (byte)i;
			this.btn_Guardian_Select[i].Click = new EZValueChangedDelegate(this.OnClickSelect);
		}
		this.dt_Guardian_Img[0].SetTextureFromBundle("UI/mythicraid/mythic_raid_saraquael");
		this.dt_Guardian_Img[1].SetTextureFromBundle("UI/mythicraid/mythic_raid_taifu");
		this.dt_Guardian_Img[2].SetTextureFromBundle("UI/mythicraid/mythic_raid_raguel");
		this.dt_Guardian_Img[3].SetTextureFromBundle("UI/mythicraid/mythic_raid_remiel");
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	private void OnClickSelect(IUIObject obj)
	{
		GS_MYTHRAID_GUARDIANSELECT_REQ gS_MYTHRAID_GUARDIANSELECT_REQ = new GS_MYTHRAID_GUARDIANSELECT_REQ();
		gS_MYTHRAID_GUARDIANSELECT_REQ.i16SelectedGuardianUnique = (short)((byte)obj.Data);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_MYTHRAID_GUARDIANSELECT_REQ, gS_MYTHRAID_GUARDIANSELECT_REQ);
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Show()
	{
		this.UIInit();
		this.SetUI();
		base.Show();
	}

	private void UIInit()
	{
		for (int i = 0; i < 4; i++)
		{
			this.dt_Guadian_Select[i].Visible = false;
			this.lb_UserName[i].Visible = false;
			string text = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), this.lb_Guardian_Name[i].GetText());
			this.lb_Guardian_Name[i].SetText(text);
		}
	}

	private void SetUI()
	{
		for (int i = 0; i < 4; i++)
		{
			MYTHRAID_PERSON mythRaidPersonInfo = SoldierBatch.MYTHRAID_INFO.GetMythRaidPersonInfo(i);
			if (mythRaidPersonInfo != null)
			{
				if (mythRaidPersonInfo.selectedGuardianUnique >= 0)
				{
					int selectedGuardianUnique = mythRaidPersonInfo.selectedGuardianUnique;
					this.dt_Guadian_Select[selectedGuardianUnique].Visible = true;
					string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3233");
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						textFromInterface,
						"username4",
						mythRaidPersonInfo.strCharName
					});
					this.lb_UserName[selectedGuardianUnique].Visible = true;
					this.lb_UserName[selectedGuardianUnique].SetText(empty);
					string text = string.Format("{0}{1}", NrTSingleton<CTextParser>.Instance.GetTextColor("1106"), this.lb_Guardian_Name[selectedGuardianUnique].GetText());
					this.lb_Guardian_Name[selectedGuardianUnique].SetText(text);
				}
			}
		}
	}
}
