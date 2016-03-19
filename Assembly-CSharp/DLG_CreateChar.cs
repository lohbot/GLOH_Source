using GAME;
using SERVICE;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class DLG_CreateChar : Form
{
	public enum EQUIP
	{
		TYPE0 = 1501
	}

	public Button _Back;

	public Button _Complete;

	private TextField m_ctrlCharName;

	private NrCharUser m_kCreateChar;

	private NrCharPartInfo m_kCustomPartInfo = new NrCharPartInfo();

	private NrCharPartInfo m_kCreatePartInfo = new NrCharPartInfo();

	private int m_nCreateCharKind = 1;

	private string m_strLocalName = string.Empty;

	private string m_strUserName = string.Empty;

	private bool m_bCharCreate;

	private E_CHAR_TRIBE m_Tribe;

	private bool m_bSoundStart;

	private DrawTexture _CharWeapon1;

	private DrawTexture _CharWeapon2;

	private Label _CharTitle;

	private Label _CharInfo;

	private DrawTexture _CharSkill1;

	private DrawTexture _CharSkill2;

	private Label _CharSkillName1;

	private Label _CharSkillName2;

	private Button _Intro;

	private eSERVICE_AREA _eCurrentService = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "SelectChar/dlg_newcreatechar", G_ID.NEW_CREATECHAR_DLG, false);
	}

	public override void SetComponent()
	{
		this._Back = (base.GetControl("Button_Button15") as Button);
		this._Complete = (base.GetControl("Button_Button14") as Button);
		this._Complete.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickComplete));
		this.m_ctrlCharName = (base.GetControl("TextField_TextField6") as TextField);
		this.m_ctrlCharName.Text = string.Empty;
		this._CharWeapon1 = (base.GetControl("DT_Weapon01") as DrawTexture);
		this._CharWeapon2 = (base.GetControl("DT_Weapon02") as DrawTexture);
		this._CharTitle = (base.GetControl("LB_Title") as Label);
		this._CharInfo = (base.GetControl("LB_Info") as Label);
		this._CharSkill1 = (base.GetControl("DT_SkillIcon01") as DrawTexture);
		this._CharSkill2 = (base.GetControl("DT_SkillIcon02") as DrawTexture);
		this._CharSkillName1 = (base.GetControl("LB_SkillName01") as Label);
		this._CharSkillName2 = (base.GetControl("LB_SkillName02") as Label);
		this._Intro = (base.GetControl("BT_Movie") as Button);
		this._Intro.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickIntro));
	}

	public void ClickPrev(IUIObject obj)
	{
		this.m_Tribe--;
		if (this.m_Tribe == E_CHAR_TRIBE.NULL)
		{
			this.m_Tribe = E_CHAR_TRIBE.HUMAN;
		}
		this.SetCharKind(this.m_Tribe);
	}

	public void ClickNext(IUIObject obj)
	{
		this.m_Tribe++;
		if (this.m_Tribe == E_CHAR_TRIBE.END)
		{
			this.m_Tribe = E_CHAR_TRIBE.HUMANF;
		}
		this.SetCharKind(this.m_Tribe);
	}

	public void ClickIntro(IUIObject obj)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_nCreateCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		string sOLINTRO = charKindInfo.GetCHARKIND_INFO().SOLINTRO;
		if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				string str = string.Format("{0}SOLINTRO/", Option.GetProtocolRootPath(Protocol.HTTP));
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false);
		}
	}

	public void ClickSkill(IUIObject obj)
	{
	}

	public void SetCharKind(E_CHAR_TRIBE m_Type)
	{
		this.m_bSoundStart = false;
		this.m_Tribe = m_Type;
		this.m_nCreateCharKind = m_Type * E_CHAR_TRIBE.FURRY - E_CHAR_TRIBE.HUMAN;
		if (m_Type == E_CHAR_TRIBE.FURRY)
		{
			this.m_nCreateCharKind = m_Type * E_CHAR_TRIBE.FURRY - E_CHAR_TRIBE.HUMAN;
		}
		else if (m_Type == E_CHAR_TRIBE.ELF)
		{
			this.m_nCreateCharKind = (int)(m_Type * E_CHAR_TRIBE.FURRY);
		}
		if (m_Type == E_CHAR_TRIBE.HUMAN)
		{
			this.m_nCreateCharKind = 1;
			this._CharWeapon1.SetTextureKey("Win_I_Weapon1");
			this._CharWeapon2.SetTextureKey("Win_I_Weapon2");
			this._CharTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50006");
			this._CharInfo.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50011");
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9400);
			if (battleSkillBase != null)
			{
				this._CharSkill1.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique));
				this._CharSkillName1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
			}
			BATTLESKILL_BASE battleSkillBase2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9420);
			if (battleSkillBase2 != null)
			{
				this._CharSkill2.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase2.m_nSkillUnique));
				this._CharSkillName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase2.m_strTextKey);
			}
		}
		else if (m_Type == E_CHAR_TRIBE.HUMANF)
		{
			this.m_nCreateCharKind = 2;
			this._CharWeapon1.SetTextureKey("Win_I_Weapon4");
			this._CharWeapon2.SetTextureKey("Win_I_Weapon5");
			this._CharTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50005");
			this._CharInfo.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50010");
			BATTLESKILL_BASE battleSkillBase3 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9200);
			if (battleSkillBase3 != null)
			{
				this._CharSkill1.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase3.m_nSkillUnique));
				this._CharSkillName1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase3.m_strTextKey);
			}
			BATTLESKILL_BASE battleSkillBase4 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9220);
			if (battleSkillBase4 != null)
			{
				this._CharSkill2.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase4.m_nSkillUnique));
				this._CharSkillName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase4.m_strTextKey);
			}
		}
		else if (m_Type == E_CHAR_TRIBE.FURRY)
		{
			this._CharWeapon1.SetTextureKey("Win_I_Weapon3");
			this._CharWeapon2.SetTextureKey("Win_I_Weapon6");
			this._CharTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50004");
			this._CharInfo.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50008");
			BATTLESKILL_BASE battleSkillBase5 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9100);
			if (battleSkillBase5 != null)
			{
				this._CharSkill1.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase5.m_nSkillUnique));
				this._CharSkillName1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase5.m_strTextKey);
			}
			BATTLESKILL_BASE battleSkillBase6 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9120);
			if (battleSkillBase6 != null)
			{
				this._CharSkill2.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase6.m_nSkillUnique));
				this._CharSkillName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase6.m_strTextKey);
			}
		}
		else if (m_Type == E_CHAR_TRIBE.ELF)
		{
			this._CharWeapon1.SetTextureKey("Win_I_Weapon7");
			this._CharWeapon2.SetTextureKey("Win_I_Weapon8");
			this._CharTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50007");
			this._CharInfo.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50009");
			BATTLESKILL_BASE battleSkillBase7 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9300);
			if (battleSkillBase7 != null)
			{
				this._CharSkill1.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase7.m_nSkillUnique));
				this._CharSkillName1.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase7.m_strTextKey);
			}
			BATTLESKILL_BASE battleSkillBase8 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(9320);
			if (battleSkillBase8 != null)
			{
				this._CharSkill2.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase8.m_nSkillUnique));
				this._CharSkillName2.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase8.m_strTextKey);
			}
		}
		TsLog.Log("SetCHARKIND : {0}  KIND : {1}", new object[]
		{
			m_Type,
			this.m_nCreateCharKind
		});
		this.SetCustomChar();
		this.m_bSoundStart = true;
	}

	private bool _CheckOverlapName()
	{
		this.m_strLocalName = this.m_ctrlCharName.Text;
		if (this.m_strLocalName.Length == 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("81");
			Main_UI_SystemMessage.ADDMessage(textFromNotify);
			this.m_bCharCreate = false;
			return false;
		}
		if (this.m_strLocalName.Length > 20)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("126"));
			return false;
		}
		if (this.m_strLocalName.Equals(this.m_strUserName))
		{
			if (NrTSingleton<ContentsLimitManager>.Instance.IsSupporter())
			{
				this.ConfirmCharName(true);
				return false;
			}
			return true;
		}
		else
		{
			if (UIDataManager.IsFilterSpecialCharacters(this.m_strLocalName, this._eCurrentService))
			{
				string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79");
				Main_UI_SystemMessage.ADDMessage(textFromNotify2);
				this.m_bCharCreate = false;
				return false;
			}
			NrNetworkCustomizing nrNetworkCustomizing = (NrNetworkCustomizing)NrTSingleton<NrNetworkSystem>.Instance.GetNetworkComponent();
			nrNetworkCustomizing.SendPacket_CheckUserName(this.m_strLocalName);
			this.ConfirmCharName(false);
			return true;
		}
	}

	private bool _CheckOverlapNameSupporter()
	{
		this.m_strLocalName = this.m_ctrlCharName.Text;
		if (this.m_strLocalName.Length == 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("81");
			Main_UI_SystemMessage.ADDMessage(textFromNotify);
			this.m_bCharCreate = false;
			return false;
		}
		if (this.m_strLocalName.Length > 20)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("126"));
			return false;
		}
		if (this.m_strLocalName.Equals(this.m_strUserName))
		{
			return true;
		}
		if (UIDataManager.IsFilterSpecialCharacters(this.m_strLocalName, this._eCurrentService))
		{
			string textFromNotify2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("79");
			Main_UI_SystemMessage.ADDMessage(textFromNotify2);
			this.m_bCharCreate = false;
			return false;
		}
		NrNetworkCustomizing nrNetworkCustomizing = (NrNetworkCustomizing)NrTSingleton<NrNetworkSystem>.Instance.GetNetworkComponent();
		nrNetworkCustomizing.SendPacket_CheckUserName(this.m_strLocalName);
		this.ConfirmCharName(false);
		return true;
	}

	public void ConfirmCharName(bool bCheckSuccess)
	{
		if (bCheckSuccess)
		{
			this.m_strUserName = this.m_strLocalName;
			if (NrTSingleton<ContentsLimitManager>.Instance.IsSupporter())
			{
				if (this.m_bCharCreate)
				{
					SupporterDlg supporterDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SUPPORTER_DLG) as SupporterDlg;
					if (supporterDlg == null)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!!!!!! Not SUPPORTER_DLG ", new object[0]);
						this.SetCharacterCustomNext(string.Empty);
					}
					else
					{
						supporterDlg.SetType(this.m_strUserName, true);
					}
				}
			}
			else if (this.m_bCharCreate)
			{
				this._AgreementCharacterCustomNext(string.Empty);
			}
		}
		else
		{
			this.m_strUserName = string.Empty;
		}
	}

	public void SetCharacterCustomNext(string strSupporterName)
	{
		if (this._CheckOverlapNameSupporter())
		{
			this._AgreementCharacterCustomNext(strSupporterName);
		}
	}

	private void SetCustomChar()
	{
		if (this.m_kCreateChar != null)
		{
			this.m_kCreateChar.Release();
		}
		this.m_kCustomPartInfo.Init();
		this.m_kCreatePartInfo.Init();
		short num = 30000;
		NrTSingleton<NkCharManager>.Instance.SetDummyChar(num);
		this.m_kCreateChar = (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(num) as NrCharUser);
		if (this.m_kCreateChar != null)
		{
			this.m_kCreateChar.SetCharKind(this.m_nCreateCharKind, true);
			this.SetEquip(1501);
			this.SetCurrentPartInfo();
			this.m_kCreateChar.SetReadyPartInfo();
		}
	}

	private void SetCurrentPartInfo()
	{
		if (this.m_kCreateChar == null)
		{
			return;
		}
		NrPersonInfoUser nrPersonInfoUser = this.m_kCreateChar.GetPersonInfo() as NrPersonInfoUser;
		nrPersonInfoUser.SetCharPartInfo(this.m_kCreatePartInfo);
	}

	private void SetEquip(int index)
	{
		this.m_kCustomPartInfo.m_kEquipPart.SetData(eAT2CharEquipPart.CHAREQUIPPART_ARMOR, index);
		this.m_kCustomPartInfo.m_kEquipPart.SetData(eAT2CharEquipPart.CHAREQUIPPART_GLOVE, index);
		this.m_kCustomPartInfo.m_kEquipPart.SetData(eAT2CharEquipPart.CHAREQUIPPART_BOOTS, index);
		this.m_kCustomPartInfo.m_kEquipPart.SetData(eAT2CharEquipPart.CHAREQUIPPART_RING, index);
	}

	public NrCharUser GetCharCustomizing()
	{
		return this.m_kCreateChar;
	}

	private void ChangeCharKind(int charkind)
	{
		if (this.m_kCreateChar == null)
		{
			return;
		}
		this.m_kCreateChar.SetCharKind(charkind, true);
		this.m_kCustomPartInfo.Init();
		this.m_kCreatePartInfo.Init();
		this.SetCurrentPartInfo();
		this.m_kCreateChar.Refresh3DChar();
	}

	public void RestoreCustomPartInfo()
	{
		this.m_kCustomPartInfo.m_kBasePart.SetData(this.m_kCreatePartInfo.m_kBasePart);
		this.m_kCustomPartInfo.m_kEquipPart.SetData(this.m_kCreatePartInfo.m_kEquipPart);
	}

	public void SetCreatePartInfo()
	{
		this.m_kCreatePartInfo.m_kBasePart.SetData(this.m_kCustomPartInfo.m_kBasePart);
		this.m_kCreatePartInfo.m_kEquipPart.SetData(this.m_kCustomPartInfo.m_kEquipPart);
		this.SetCurrentPartInfo();
	}

	public bool IsCheckCreateComplete()
	{
		this.m_strLocalName = this.m_ctrlCharName.Text;
		if (!this.m_strLocalName.Equals(this.m_strUserName))
		{
			this._CheckOverlapName();
			return false;
		}
		return this._CheckOverlapName() && this._AgreementCharacterCustomNext(string.Empty);
	}

	private void ChangeCharBasePart(eAT2CharBasePart eBasePart, int partvalue)
	{
		if (this.m_kCreateChar == null)
		{
			return;
		}
		this.m_kCustomPartInfo.m_kBasePart.SetData(eBasePart, partvalue);
		this.m_kCreateChar.ChangeCharPartInfo(this.m_kCustomPartInfo, true, false);
	}

	private void ChangeCharEquipPart()
	{
		if (this.m_kCreateChar == null)
		{
			return;
		}
		this.m_kCreateChar.ChangeCharPartInfo(this.m_kCustomPartInfo, false, true);
	}

	private void ChangeBodyPart(int _BodyStyle)
	{
		Nr3DCharActor nr3DCharActor = this.m_kCreateChar.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		nr3DCharActor.RemoveItemAll();
	}

	private void ClickComplete(IUIObject obj)
	{
		this.IsCheckCreateComplete();
		this.m_bCharCreate = true;
	}

	private void ClickCancel(IUIObject obj)
	{
	}

	private void ClickCheckName(IUIObject obj)
	{
		this._CheckOverlapName();
	}

	private void ClickToCharGender(IUIObject obj)
	{
		Toggle toggle = obj as Toggle;
		if (toggle == null)
		{
			return;
		}
		if (this.m_nCreateCharKind % 2 == (int)toggle.Data % 2)
		{
			return;
		}
		if (this.m_nCreateCharKind % 2 == 1)
		{
			this.m_nCreateCharKind++;
			E_CHAR_TRIBE tribe = this.m_Tribe;
			if (tribe == E_CHAR_TRIBE.HUMAN)
			{
				NrTSingleton<CCameraAniPlay>.Instance.Add(new object[]
				{
					E_CAMARA_STATE_ANI.HUMAN_MALETOFEMALE
				});
			}
		}
		else
		{
			this.m_nCreateCharKind--;
			E_CHAR_TRIBE tribe = this.m_Tribe;
			if (tribe == E_CHAR_TRIBE.HUMAN)
			{
				NrTSingleton<CCameraAniPlay>.Instance.Add(new object[]
				{
					E_CAMARA_STATE_ANI.HUMAN_FEMALETOMALE
				});
			}
		}
		this.ChangeCharKind(this.m_nCreateCharKind);
		if (this.m_bSoundStart)
		{
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "GENDER_SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private bool _AgreementCharacterCustomNext(string strSupporterName)
	{
		if (this.m_kCreateChar == null)
		{
			return false;
		}
		if (this.m_strUserName.Length == 0)
		{
			return false;
		}
		if (COMMON_CONSTANT_Manager.GetInstance() == null)
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!!!!!!!! NOT Error Common_Constant", new object[0]);
			return false;
		}
		this.m_kCreateChar.GetPersonInfo().SetCharName(this.m_strUserName);
		this.SetCurrentPartInfo();
		NrNetworkCustomizing nrNetworkCustomizing = (NrNetworkCustomizing)NrTSingleton<NrNetworkSystem>.Instance.GetNetworkComponent();
		nrNetworkCustomizing.SendPacket_CreateAvatar(this.GetCharCustomizing(), strSupporterName);
		return true;
	}

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this._Back.CallChangeDelegate();
		}
	}
}
