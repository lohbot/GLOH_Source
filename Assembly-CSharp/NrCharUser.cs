using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using System;
using UnityEngine;
using UnityForms;

public class NrCharUser : NrCharBase
{
	private NkCharPartControl m_kPartControl;

	private string m_strGuildName;

	private long m_i64GuildID;

	private short m_i16ColosseumGrade;

	public NrCharUser()
	{
		this.m_kPersonInfo = new NrPersonInfoUser();
		this.m_kPartControl = new NkCharPartControl();
	}

	public override string GetUserGuildName()
	{
		if (base.GetID() == 1)
		{
			this.m_strGuildName = NrTSingleton<NewGuildManager>.Instance.GetGuildName();
		}
		return this.m_strGuildName;
	}

	public override long GetUserGuildID()
	{
		if (base.GetID() == 1)
		{
			this.m_i64GuildID = NrTSingleton<NewGuildManager>.Instance.GetGuildID();
		}
		return this.m_i64GuildID;
	}

	public override short GetUserColosseumGrade()
	{
		if (base.GetID() == 1)
		{
			this.m_i16ColosseumGrade = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.ColosseumGrade;
		}
		return this.m_i16ColosseumGrade;
	}

	public void SetColosseumGrade(short i16ColosseumGrade)
	{
		this.m_i16ColosseumGrade = i16ColosseumGrade;
	}

	public void SetUserGuildName(string strGuildName, long i64GuildID)
	{
		this.m_strGuildName = strGuildName;
		this.m_i64GuildID = i64GuildID;
		base.MakeCharGuildNameShow(this.m_strGuildName, i64GuildID);
	}

	public override void Init()
	{
		base.Init();
		this.m_eCharKindType = eCharKindType.CKT_USER;
		this.m_kPartControl.Init();
		this.m_strGuildName = string.Empty;
		this.m_i64GuildID = 0L;
		this.m_i16ColosseumGrade = 0;
	}

	public override void Release()
	{
		NrPersonInfoUser nrPersonInfoUser = this.m_kPersonInfo as NrPersonInfoUser;
		nrPersonInfoUser.m_kSubChar.DeleteSubCharAll3DObject();
		base.Release();
	}

	public override bool Update()
	{
		if (!base.Update())
		{
			return false;
		}
		NrCharBase.e3DCharStep e3DCharStep = base.m_e3DCharStep;
		if (e3DCharStep != NrCharBase.e3DCharStep.READY)
		{
			if (e3DCharStep == NrCharBase.e3DCharStep.CHARACTION)
			{
				NrPersonInfoUser nrPersonInfoUser = this.m_kPersonInfo as NrPersonInfoUser;
				if (nrPersonInfoUser.m_kSubChar.IsMakeSubChar)
				{
					nrPersonInfoUser.m_kSubChar.MakeSubChar();
				}
				nrPersonInfoUser.m_kSubChar.FollowParent();
				if (!base.IsAutoMove() && this.GetFollowCharPersonID() > 0L)
				{
					this.m_kCharMove.MoveToFollowChar();
				}
				if (base.GetID() == 1)
				{
					GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
					if (rootGameObject != null && rootGameObject.transform.localPosition.y <= 0f)
					{
						base.SetSafeCharPos(rootGameObject.transform.localPosition);
					}
				}
			}
		}
		else
		{
			NrPersonInfoUser nrPersonInfoUser2 = this.m_kPersonInfo as NrPersonInfoUser;
			int subCharCount = nrPersonInfoUser2.m_kSubChar.GetSubCharCount();
			if (subCharCount > 0)
			{
				nrPersonInfoUser2.m_kSubChar.IsMakeSubChar = true;
			}
		}
		return base.PostUpdate();
	}

	public override Nr3DCharBase Create3DGrahpicData()
	{
		if (this.m_k3DChar != null)
		{
			this.Release();
		}
		string charName = this.m_kPersonInfo.GetCharName();
		this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Create3DChar<Nr3DCharActor>(base.GetID(), charName);
		if (this.m_k3DChar == null)
		{
			return null;
		}
		bool bOnlyWeapon = this.m_nFaceCharKind > 0;
		this.m_kPartControl.SetPartControl(this.m_k3DChar, bOnlyWeapon);
		return base.Create3DGrahpicData();
	}

	protected override void OnCreateAction()
	{
		this.SetReadyPartInfo();
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			this.m_kPartControl.ResetBaseBone();
			base.OnCreateAction();
		}
	}

	public override bool OnLoaded3DChar()
	{
		if (!base.OnLoaded3DChar())
		{
			return false;
		}
		if (NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			if (base.GetID() == 1)
			{
				GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.SetMyCharObject(rootGameObject);
				StageWorld.ChageWorldAudioListener();
				if (null == rootGameObject.GetComponent<TsFadeTarget>())
				{
					rootGameObject.AddComponent<TsFadeTarget>();
				}
				if (null == rootGameObject.GetComponent<NmUserBehaviour>())
				{
					rootGameObject.AddComponent<NmUserBehaviour>();
				}
			}
			if (base.IsChangedItem())
			{
				this.ChangeEquipItem();
				base.SetChangedItem(false);
			}
			this.SetFaceGradeEffect();
		}
		base.SetAnimation(base.LoadAfterAnimation);
		NrPersonInfoUser nrPersonInfoUser = this.m_kPersonInfo as NrPersonInfoUser;
		nrPersonInfoUser.m_kSubChar.ParentChar = this;
		return true;
	}

	private void SetFaceGradeEffect()
	{
		this.RemoveFaceGradeEffect();
		NrCharKindInfo faceCharKindInfo = base.GetFaceCharKindInfo();
		if (faceCharKindInfo != null && base.GetFaceCharKind() > 0)
		{
			int faceCharGrade = base.GetFaceCharGrade();
			if (faceCharGrade >= 5)
			{
				string charEffectGrade = faceCharKindInfo.GetCharEffectGrade(faceCharGrade);
				if (!charEffectGrade.Equals("0"))
				{
					this.m_nFaceSolGradeEffectNum = NrTSingleton<NkEffectManager>.Instance.AddEffect(charEffectGrade, this);
				}
			}
		}
	}

	private void RemoveFaceGradeEffect()
	{
		if (this.m_nFaceSolGradeEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nFaceSolGradeEffectNum);
			this.m_nFaceSolGradeEffectNum = 0u;
		}
	}

	public override void SetCharKind(int charkind, bool bChanged)
	{
		base.SetCharKind(charkind, bChanged);
		this.m_kPartControl.SetCharKindInfo(this.m_pkCharKindInfo);
	}

	public void SetFacialAnimation()
	{
		this.m_kAnimation.AttachFacialAnimation();
	}

	public override void SetReadyPartInfo()
	{
		NrPersonInfoUser personInfoUser = this.GetPersonInfoUser();
		NrCharBasePart basePart = personInfoUser.GetBasePart();
		NrEquipItemInfo equipItemInfo = personInfoUser.GetSoldierInfo(0).GetEquipItemInfo();
		this.m_kPartControl.CollectPartInfo(basePart, equipItemInfo);
	}

	public void ChangeBasePart()
	{
		this.SetReadyPartInfo();
		this.m_kPartControl.ChangeBasePart();
	}

	public void ChangeEquipItem()
	{
		this.SetReadyPartInfo();
		this.m_kPartControl.ChangeEquipItem();
		if (this.m_k3DChar != null && this.m_k3DChar.GetBaseObject() != null && base.IsReady3DModel())
		{
			this.m_k3DChar.SearchWeaponDummy();
		}
	}

	public void ChangeCharPartInfo(NrCharPartInfo pkCustomPartInfo, bool bChangeBase, bool bChangeEquip)
	{
		this.m_kPartControl.SetPartInfo(pkCustomPartInfo);
		this.ProcessCharPartInfo(pkCustomPartInfo, bChangeBase, bChangeEquip);
		this.SetSolEquipItemFromPartInfo(pkCustomPartInfo.m_kEquipPart);
	}

	private void ProcessCharPartInfo(NrCharPartInfo pkCustomPartInfo, bool bChangeBase, bool bChangeEquip)
	{
		NrPersonInfoUser nrPersonInfoUser = base.GetPersonInfo() as NrPersonInfoUser;
		if (bChangeBase)
		{
			nrPersonInfoUser.SetBasePart(pkCustomPartInfo.m_kBasePart);
			this.ChangeBasePart();
		}
		if (bChangeEquip)
		{
			NkSoldierInfo soldierInfo = nrPersonInfoUser.GetSoldierInfo(0);
			if (soldierInfo != null)
			{
				soldierInfo.SetEquipItemInfo(pkCustomPartInfo.m_kEquipPart);
			}
			if (this.m_k3DChar != null)
			{
				this.ChangeEquipItem();
			}
			else
			{
				base.SetChangedItem(true);
			}
		}
	}

	public override void ChangeWeaponTarget()
	{
		this.m_kPartControl.ChangeWeaponTarget();
	}

	public void ChangeCharModel(int facecharkind, byte facechargrade)
	{
		bool flag = false;
		if (this.m_nFaceCharKind != facecharkind)
		{
			this.m_nFaceCharKind = facecharkind;
			if (base.IsCreated3DModel())
			{
				base.Refresh3DChar();
				flag = true;
			}
		}
		if (this.m_nFaceCharGrade != (int)facechargrade)
		{
			this.m_nFaceCharGrade = (int)facechargrade;
			if (!flag)
			{
				this.SetFaceGradeEffect();
			}
		}
	}

	public override bool IsReayCreateCharInfo()
	{
		if (!base.IsReayCreateCharInfo())
		{
			return false;
		}
		if (!this.m_kPartControl.IsReadyPartInfo())
		{
			return false;
		}
		if (base.GetID() == 1 || !NrTSingleton<NkClientLogic>.Instance.IsGameWorld())
		{
			this.SetReadyPartInfo();
		}
		return true;
	}

	public NkCharPartControl GetPartControl()
	{
		return this.m_kPartControl;
	}

	public NrPersonInfoUser GetPersonInfoUser()
	{
		return this.m_kPersonInfo as NrPersonInfoUser;
	}

	public NkSoldierInfo GetUserSoldierInfo()
	{
		return this.m_kPersonInfo.GetLeaderSoldierInfo();
	}

	public void SetSolEquipItemFromPartInfo(NrCharEquipPart pkEquipPart)
	{
		NkSoldierInfo userSoldierInfo = this.GetUserSoldierInfo();
		if (userSoldierInfo != null && userSoldierInfo.IsValid())
		{
			userSoldierInfo.SetEquipItemInfo(pkEquipPart);
			userSoldierInfo.SetReceivedEquipItem(true);
			userSoldierInfo.UpdateSoldierStatInfo();
		}
	}

	public bool SetSubCharKind(int siKind, int siIndex)
	{
		return this.GetPersonInfoUser().m_kSubChar.SetSubCharKind(siKind, siIndex);
	}

	public bool SetSubCharKind(int siKind, int siIndex, string strStartChatText)
	{
		return this.GetPersonInfoUser().m_kSubChar.SetSubCharKind(siKind, siIndex, strStartChatText);
	}

	public void SetSubCharKindFromList(int[] siKindList)
	{
		this.GetPersonInfoUser().m_kSubChar.SetSubCharKindFromList(siKindList);
	}

	public int GetSubChsrKind(int siIndex)
	{
		return this.GetPersonInfoUser().m_kSubChar.GetSubCharKind(siIndex);
	}

	public NrCharBase GetSubChar(int siIndex)
	{
		return this.GetPersonInfoUser().m_kSubChar.GetSubChar(siIndex);
	}

	public bool DeleteSubChar(int siIndex)
	{
		this.GetPersonInfoUser().m_kSubChar.DeleteSubChar(siIndex);
		return true;
	}

	public void DeleteSubCharAll()
	{
		this.GetPersonInfoUser().m_kSubChar.DeleteSubCharAll();
	}

	public string GetSubCharStartChatText(int siCharUnique)
	{
		return this.GetPersonInfoUser().m_kSubChar.GetStartChatText(siCharUnique);
	}

	public void SetSubCharStartChatText(int siIndex, string strStartChatText)
	{
		this.GetPersonInfoUser().m_kSubChar.SetStartChatText(siIndex, strStartChatText);
	}

	public void SetFollowCharPersonID(long nPersonID, string Charname)
	{
		if (nPersonID == 0L)
		{
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nFollowCharPersonID = 0L;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_FollowCharName = string.Empty;
			NrTSingleton<FormsManager>.Instance.Hide(G_ID.DLG_FOLLOWCHAR);
		}
		else
		{
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nFollowCharPersonID = nPersonID;
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_FollowCharName = Charname;
			NrCharBase charByPersonID = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(nPersonID);
			if (charByPersonID != null)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_FOLLOWCHAR);
			}
		}
	}

	public long GetFollowCharPersonID()
	{
		return NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nFollowCharPersonID;
	}

	public string GetFollowCharName()
	{
		return NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_FollowCharName;
	}

	public void RefreshFollowCharPos()
	{
		if (this.GetFollowCharPersonID() > 0L && !NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bRequestFollowCharPos)
		{
			GS_FOLLOWCHAR_REQ gS_FOLLOWCHAR_REQ = new GS_FOLLOWCHAR_REQ();
			gS_FOLLOWCHAR_REQ.nPersonID = this.GetFollowCharPersonID();
			TKString.StringChar(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_FollowCharName, ref gS_FOLLOWCHAR_REQ.Name);
			SendPacket.GetInstance().SendObject(914, gS_FOLLOWCHAR_REQ);
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bRequestFollowCharPos = true;
		}
	}

	public void SetClassChage(int iCharKind)
	{
		if (0 >= iCharKind)
		{
			return;
		}
		this.SetCharKind(iCharKind, true);
		if (base.IsCreated3DModel())
		{
			base.Refresh3DChar();
		}
	}
}
