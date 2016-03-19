using System;
using UnityForms;

public class ColosseumCardSettingDlg : Form
{
	public Label m_lbName;

	public Label m_lbSkillDesc;

	public DrawTexture m_dwSolFace;

	public DrawTexture m_dwWeaphonIcon;

	public DrawTexture m_dwCardBG;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.TopMost = true;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_SettingCard", G_ID.COLOSSEUM_SETTING_CARD_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_lbName = (base.GetControl("Label_Name01") as Label);
		this.m_lbSkillDesc = (base.GetControl("Label_SkillText01") as Label);
		this.m_dwSolFace = (base.GetControl("DrawTexture_SolFace01") as DrawTexture);
		this.m_dwWeaphonIcon = (base.GetControl("DrawTexture_WeaponIcon01") as DrawTexture);
		this.m_dwCardBG = (base.GetControl("DrawTexture_CardBG01") as DrawTexture);
		this.m_dwCardBG.SetTextureFromBundle("UI/pvp/cardback");
		this.Hide();
	}

	public override void Show()
	{
		base.Show();
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width - base.GetSizeX() - 100f, (GUICamera.height - base.GetSizeY()) / 2f);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void SetSolInfo(int nCharKind)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.SetCharKind(nCharKind);
		nkSoldierInfo.SetLevel(1);
		this.m_lbName.SetText(nkSoldierInfo.GetName());
		string textureFromBundle = string.Empty;
		if (UIDataManager.IsUse256Texture())
		{
			textureFromBundle = "UI/Soldier/256/" + charKindInfo.GetPortraitFile1((int)nkSoldierInfo.GetGrade()) + "_256";
		}
		else
		{
			textureFromBundle = "UI/Soldier/512/" + charKindInfo.GetPortraitFile1((int)nkSoldierInfo.GetGrade()) + "_512";
		}
		this.m_dwSolFace.SetTextureFromBundle(textureFromBundle);
		if (charKindInfo.GetWeaponType() > 0)
		{
			this.m_dwWeaphonIcon.SetTexture(string.Format("Win_I_Weapon{0}", charKindInfo.GetWeaponType().ToString()));
		}
		int battleSkillUnique = charKindInfo.GetBattleSkillUnique(0);
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, 1);
		if (battleSkillBase == null || battleSkillDetail == null)
		{
			return;
		}
		if (battleSkillBase.m_nColosseumSkillDesc > 0)
		{
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_nColosseumSkillDesc.ToString());
			if (textFromInterface != string.Empty)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromInterface,
					"skillname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"count",
					battleSkillDetail.m_nSkillNeedAngerlyPoint
				});
				this.m_lbSkillDesc.SetText(empty);
			}
		}
		this.Show();
	}
}
