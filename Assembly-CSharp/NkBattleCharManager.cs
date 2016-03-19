using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using System;
using UnityEngine;

public class NkBattleCharManager : NrTSingleton<NkBattleCharManager>
{
	private NkBattleChar[] m_arChar;

	private bool m_bShowCharUnique;

	private NrCharDefine.CharLODStep m_eCharLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;

	private bool m_bFakeShadowEnable;

	private NkBattleCharManager()
	{
		this.m_arChar = new NkBattleChar[120];
		this.m_bShowCharUnique = false;
		this.m_eCharLODStep = NrCharDefine.CharLODStep.CHARLOD_STEP_1;
		this.m_bFakeShadowEnable = false;
	}

	public bool IsAllCharLoadComplete()
	{
		if (this.m_arChar == null)
		{
			Debug.Log("NkBattleCharManager Is Not Created");
			return false;
		}
		for (int i = 1; i < 120; i++)
		{
			if (this.m_arChar[i] != null && !this.m_arChar[i].IsReady3DModel())
			{
				return false;
			}
		}
		return true;
	}

	public NkBattleChar[] GetCharArray()
	{
		return this.m_arChar;
	}

	public void Init()
	{
		this.DeleteAllChar();
	}

	public void InitChar3DModelAll()
	{
		this.ChangeChar3DStepToDeleted();
	}

	public void InitTurn()
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				nkBattleChar.InitTurn();
			}
		}
	}

	public int FindEmptyChar()
	{
		for (int i = 1; i < 120; i++)
		{
			if (this.m_arChar[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindCharIDByBUID(short buid)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				if (nkBattleChar.GetBUID() == buid)
				{
					return nkBattleChar.GetID();
				}
			}
		}
		return -1;
	}

	public int SetChar(BATTLE_SOLDIER_INFO MakeCharInfo)
	{
		if (MakeCharInfo == null)
		{
			return -1;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(MakeCharInfo.CharKind);
		NrPersonInfoBattle nrPersonInfoBattle = new NrPersonInfoBattle();
		if (charKindInfo != null)
		{
			eCharKindType charKindType = (eCharKindType)MakeCharInfo.CharKindType;
			nrPersonInfoBattle.SetUserData(MakeCharInfo);
			return this.SetChar(MakeCharInfo.CharUnique, MakeCharInfo.BUID, MakeCharInfo.CharKind, charKindType, nrPersonInfoBattle, MakeCharInfo.Speed, ref MakeCharInfo.kPartInfo, MakeCharInfo.SolIndex);
		}
		Debug.LogWarning("CharKind not found !!!!! Name = " + nrPersonInfoBattle.GetCharName() + ", Kind = " + MakeCharInfo.CharKind.ToString());
		return -1;
	}

	public int SetChar(short charunique, short buid, int charkind, eCharKindType kindtype, NrPersonInfoBase kPersonInfo, float speed, ref NrCharPartInfo pkPartInfo, byte nsolidx)
	{
		int num = this.AddNewChar(charunique, buid);
		NkBattleChar @char = this.GetChar(num);
		if (@char != null)
		{
			@char.SetKindType(kindtype);
			@char.SetPersonInfo(kPersonInfo);
			NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (charunique == char2.GetCharUnique() && !NrTSingleton<NkBattleReplayManager>.Instance.IsReplay)
			{
				@char.MyChar = true;
				@char.SetSolIdx((short)nsolidx);
			}
			if (kindtype == eCharKindType.CKT_USER && charunique != char2.GetCharUnique())
			{
				@char.ChangeCharPartInfo(pkPartInfo, true, true);
				@char.SetReadyPartInfo();
			}
			if (speed > 0f)
			{
				NrPersonInfoBase personInfo = @char.GetPersonInfo();
				if (personInfo != null)
				{
					personInfo.SetMoveSpeed(speed);
				}
			}
			if (kindtype == eCharKindType.CKT_NPC && @char.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
			{
				@char.MoveTo(kPersonInfo.GetDirection().x, kPersonInfo.GetDirection().y, kPersonInfo.GetDirection().z, false);
			}
		}
		return num;
	}

	public int AddNewChar(short charunique, short buid)
	{
		int num = this.FindCharIDByBUID(buid);
		if (num < 0)
		{
			num = this.FindEmptyChar();
			if (num >= 0 && !this.AddChar(num, charunique, buid))
			{
				return 0;
			}
		}
		return num;
	}

	public bool AddChar(int id, short charunique, short buid)
	{
		if (this.m_arChar[id] != null)
		{
			return false;
		}
		this.m_arChar[id] = new NkBattleChar();
		NkBattleCharIDInfo iDInfo = new NkBattleCharIDInfo(id, charunique, buid);
		this.m_arChar[id].SetIDInfo(iDInfo);
		return true;
	}

	public NkBattleChar GetCharClone(int id)
	{
		if (this.m_arChar[id] != null)
		{
			return (NkBattleChar)this.m_arChar[id].Clone();
		}
		return null;
	}

	public NkBattleChar GetCharByBUID(short buid)
	{
		int num = this.FindCharIDByBUID(buid);
		if (num < 0)
		{
			return null;
		}
		return this.m_arChar[num];
	}

	public NkBattleChar GetCharByCharKind(int charkind)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				if (nkBattleChar.GetPersonInfo().GetKind(0) == charkind)
				{
					return nkBattleChar;
				}
			}
		}
		return null;
	}

	public NkBattleChar GetChar(int id)
	{
		if (id < 0 || id >= 120)
		{
			return null;
		}
		return this.m_arChar[id];
	}

	public NkBattleChar GetChar(GameObject kGameObj)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				if (nkBattleChar.Get3DChar() != null)
				{
					if (nkBattleChar.Get3DChar().GetRootGameObject() == kGameObj)
					{
						return nkBattleChar;
					}
				}
			}
		}
		return null;
	}

	public NrPersonInfoUser GetCharPersonInfo(int id)
	{
		NkBattleChar @char = this.GetChar(id);
		if (@char == null)
		{
			return null;
		}
		return @char.GetPersonInfo() as NrPersonInfoUser;
	}

	public NrCharKindInfo GetCharKindInfo(int id)
	{
		NkBattleChar @char = this.GetChar(id);
		if (@char == null)
		{
			return null;
		}
		return @char.GetCharKindInfo();
	}

	public bool DeleteChar(int id)
	{
		if (id < 0 || id >= 120)
		{
			return false;
		}
		if (this.m_arChar[id] != null)
		{
			this.m_arChar[id].DeleteChar();
			this.m_arChar[id] = null;
			return true;
		}
		return false;
	}

	public void DeleteAllChar()
	{
		if (Battle.BATTLE != null && Battle.BATTLE.BattleCamera != null)
		{
			Battle.BATTLE.BattleCamera.SetLastAttackCamera(null, false);
		}
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && this.m_arChar[iD] != null)
				{
					this.m_arChar[iD].DeleteChar();
					this.m_arChar[iD] = null;
				}
			}
		}
	}

	public void DeleteAllCharByAlly(eBATTLE_ALLY eAlly)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && this.m_arChar[iD] != null && this.m_arChar[iD].Ally == eAlly)
				{
					this.m_arChar[iD].DeleteChar();
					this.m_arChar[iD] = null;
				}
			}
		}
	}

	public NkBattleChar SelectBattleSkillChar_GRID_ALL(NkBattleChar pkSendChar, int BattleSkillUnique, eBATTLE_ALLY eAlly)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
		int battleSkillLevelByUnique = pkSendChar.GetBattleSkillLevelByUnique(BattleSkillUnique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(BattleSkillUnique, battleSkillLevelByUnique);
		if (battleSkillBase == null || battleSkillDetail == null)
		{
			return null;
		}
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && this.m_arChar[iD] != null && this.m_arChar[iD].Ally == eAlly)
				{
					int num = pkSendChar.CanBattleSkill(this.m_arChar[iD], this.m_arChar[iD].GetCharPos(), battleSkillBase, battleSkillDetail);
					if (num == 1 || num == -2)
					{
						if (this.CheckBuffSkillToType(nkBattleChar, 98))
						{
							if (battleSkillDetail.GetSkillDetalParamValue(75) > 0 || battleSkillDetail.GetSkillDetalParamValue(76) > 0)
							{
								return this.m_arChar[iD];
							}
						}
						else
						{
							if (!this.CheckBuffSkillToType(nkBattleChar, 99))
							{
								return this.m_arChar[iD];
							}
							if (nkBattleChar.Ally != pkSendChar.Ally && (battleSkillDetail.GetSkillDetalParamValue(75) > 0 || battleSkillDetail.GetSkillDetalParamValue(76) > 0))
							{
								return this.m_arChar[iD];
							}
						}
					}
				}
			}
		}
		return null;
	}

	public bool CheckBuffSkillToType(NkBattleChar TargetChar, int SkillDetailType)
	{
		for (int i = 0; i < 12; i++)
		{
			if (TargetChar.m_BattleSkillBufData[i].BSkillBufSkillUnique > 0 && TargetChar.m_BattleSkillBufData[i].BSkillBufLevel > 0)
			{
				BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(TargetChar.m_BattleSkillBufData[i].BSkillBufSkillUnique, TargetChar.m_BattleSkillBufData[i].BSkillBufLevel);
				if (battleSkillDetail != null)
				{
					if (battleSkillDetail.GetSkillDetalParamValue(SkillDetailType) != 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void DeleteDeadChar()
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && this.m_arChar[iD] != null && (this.m_arChar[iD].GetSoldierInfo().GetHP() <= 0 || this.m_arChar[iD].Get3DCharStep() == NkBattleChar.e3DCharStep.DIED))
				{
					this.m_arChar[iD].DeleteChar();
					this.m_arChar[iD] = null;
				}
			}
		}
	}

	public void ReleaseChar(NkBattleChar kCharInst)
	{
		this.m_arChar[kCharInst.GetID()] = null;
		kCharInst.Release();
	}

	public void Update()
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.GetID() >= 0)
			{
				nkBattleChar.Update();
			}
		}
	}

	public void ShowHideAll(bool bShow, bool bNameCheck)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.m_k3DChar != null)
			{
				nkBattleChar.SetShowHide3DModel(bShow, bShow, bNameCheck);
			}
		}
	}

	public void ShowHideAlly(eBATTLE_ALLY eBattleAlly, short nExceptBUID, bool bShow, bool bNameCheck)
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.m_k3DChar != null)
			{
				if (nkBattleChar.Ally == eBattleAlly && nkBattleChar.GetBUID() != nExceptBUID)
				{
					nkBattleChar.SetShowHide3DModel(bShow, bShow, bNameCheck);
				}
				if (nkBattleChar.Ally != eBattleAlly)
				{
					nkBattleChar.SetShowHeadUp(bShow, !bShow, true);
				}
			}
		}
	}

	public void ChangeChar3DStepToDeleted()
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.GetID() > 0)
			{
				nkBattleChar.Set3DCharStep(NkBattleChar.e3DCharStep.DELETED);
			}
		}
	}

	public void SyncBillboardRotate()
	{
		bool bScaleUpdate = NrTSingleton<NkClientLogic>.Instance.IsRemainFrame(2);
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null)
			{
				nkBattleChar.SyncBillboardRotate(bScaleUpdate);
			}
		}
	}

	public void ToggleShowCharUnique()
	{
		this.m_bShowCharUnique = !this.m_bShowCharUnique;
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.GetID() > 0)
			{
				nkBattleChar.RefreshCharName(this.m_bShowCharUnique);
			}
		}
	}

	public void RefreshCharName()
	{
		NkBattleChar[] arChar = this.m_arChar;
		for (int i = 0; i < arChar.Length; i++)
		{
			NkBattleChar nkBattleChar = arChar[i];
			if (nkBattleChar != null && nkBattleChar.GetID() > 0)
			{
				nkBattleChar.RefreshCharName(this.m_bShowCharUnique);
			}
		}
	}

	public void SetCharLODStep(NrCharDefine.CharLODStep lodstep)
	{
		if (this.m_eCharLODStep == lodstep)
		{
			return;
		}
		this.m_eCharLODStep = lodstep;
	}

	public NrCharDefine.CharLODStep GetCharLODStep()
	{
		return this.m_eCharLODStep;
	}

	public string GetCharLODStepString()
	{
		if (Scene.IsCurScene(Scene.Type.CUTSCENE))
		{
			return string.Empty;
		}
		switch (this.m_eCharLODStep)
		{
		case NrCharDefine.CharLODStep.CHARLOD_STEP_1:
			return "_LOD1";
		case NrCharDefine.CharLODStep.CHARLOD_STEP_2:
			return "_LOD1";
		case NrCharDefine.CharLODStep.CHARLOD_STEP_3:
			return "_LOD1";
		default:
			return "_LOD1";
		}
	}

	public Nr3DCharBase GetCollisionChar(Nr3DCharBase kChar, Vector2 movepos)
	{
		if (kChar.GetCharController() == null)
		{
			return null;
		}
		Vector3 vector = Vector3.zero;
		float num = -3.40282347E+38f;
		NkBattleChar nkBattleChar = null;
		NkBattleChar[] charArray = this.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar2 = charArray[i];
			if (nkBattleChar2 != null)
			{
				if (nkBattleChar2.GetBUID() != kChar.GetParentBattleChar().GetBUID())
				{
					if (!(nkBattleChar2.Get3DChar().GetCharController() == null))
					{
						vector = nkBattleChar2.Get3DChar().GetCharController().transform.position;
						float num2 = kChar.GetParentBattleChar().GetCharHalfBound() + nkBattleChar2.GetCharHalfBound();
						float num3 = Vector2.Distance(movepos, new Vector2(vector.x, vector.z));
						if (num3 <= num2 && num2 - num3 > num)
						{
							num = num2 - num3;
							nkBattleChar = nkBattleChar2;
						}
					}
				}
			}
		}
		if (nkBattleChar != null)
		{
			return nkBattleChar.Get3DChar();
		}
		return null;
	}

	public bool AdjustMovePosByCollision(Nr3DCharBase kChar, ref Vector2 vMovePos, bool bAlly)
	{
		if (Battle.BATTLE.m_bOnlyServerMove)
		{
			return false;
		}
		if (kChar.GetCharController() == null)
		{
			return false;
		}
		Vector2 b = Vector2.zero;
		bool result = false;
		for (int i = 1; i < 120; i++)
		{
			NkBattleChar nkBattleChar = this.m_arChar[i];
			if (nkBattleChar != null)
			{
				Nr3DCharBase nr3DCharBase = nkBattleChar.Get3DChar();
				if (!(nr3DCharBase.GetCharController() == null))
				{
					if (nr3DCharBase.GetID() != kChar.GetID())
					{
						if (bAlly || nr3DCharBase.GetParentBattleChar().Ally != kChar.GetParentBattleChar().Ally)
						{
							b = vMovePos - new Vector2(nr3DCharBase.GetParentBattleChar().GetCharPos().x, nr3DCharBase.GetParentBattleChar().GetCharPos().z);
							float num = Vector2.Distance(Vector2.zero, b);
							float num2 = kChar.GetParentBattleChar().GetCharHalfBound() + nr3DCharBase.GetParentBattleChar().GetCharHalfBound() - num;
							if (num2 >= 0f)
							{
								this.GetTurningPos(kChar, nr3DCharBase, ref vMovePos);
								result = true;
							}
						}
					}
				}
			}
		}
		return result;
	}

	public void GetTurningPos(Nr3DCharBase kChar, Nr3DCharBase k3DChar, ref Vector2 vMovePos)
	{
		float x = kChar.GetCharController().transform.position.x;
		float z = kChar.GetCharController().transform.position.z;
		float x2 = vMovePos.x;
		float y = vMovePos.y;
		Vector2 b = Vector2.zero;
		for (int i = 44; i < 180; i += 2)
		{
			double num = (double)i / 180.0 * 3.1414999961853027;
			float x3 = (float)Math.Cos(num) * (x2 - x) - (float)Math.Sin(num) * (y - z) + x;
			float y2 = (float)Math.Sin(num) * (x2 - x) + (float)Math.Cos(num) * (y - z) + z;
			b = new Vector2(x3, y2) - new Vector2(k3DChar.GetParentBattleChar().GetCharPos().x, k3DChar.GetParentBattleChar().GetCharPos().z);
			float num2 = Vector2.Distance(Vector2.zero, b);
			float num3 = kChar.GetParentBattleChar().GetCharHalfBound() + k3DChar.GetParentBattleChar().GetCharHalfBound() - num2;
			if (num3 < 0f)
			{
				vMovePos.x = x3;
				vMovePos.y = y2;
				return;
			}
		}
	}

	public Vector2 ObstacleAvoidance(Nr3DCharBase kChar, Vector2 curPos, Vector2 dir)
	{
		if (kChar.GetCharController() == null)
		{
			return Vector2.zero;
		}
		float num = 10f;
		float num2 = 3.40282347E+38f;
		Nr3DCharBase nr3DCharBase = null;
		Vector2 vector = Vector2.zero;
		NkBattleChar[] charArray = this.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar = charArray[i];
			if (nkBattleChar != null)
			{
				Nr3DCharBase nr3DCharBase2 = nkBattleChar.Get3DChar();
				if (nkBattleChar.GetBUID() != kChar.GetParentBattleChar().GetBUID())
				{
					if (!(nr3DCharBase2.GetCharController() == null))
					{
						Vector2 pos = new Vector2(nr3DCharBase2.GetCharController().transform.position.x, nr3DCharBase2.GetCharController().transform.position.z);
						Vector2 vector2 = this.PointToLocalSpace(pos, dir, curPos);
						if (vector2.x >= 0f)
						{
							float num3 = kChar.GetParentBattleChar().GetCharHalfBound() + nr3DCharBase2.GetParentBattleChar().GetCharHalfBound();
							if (Mathf.Abs(vector2.y) < num3)
							{
								float x = vector2.x;
								float y = vector2.y;
								float num4 = Mathf.Sqrt(num3 * num3 - y * y);
								float num5 = x - num4;
								if (num5 <= 0f)
								{
									num5 = x + num4;
								}
								if (num5 < num2)
								{
									num2 = num5;
									nr3DCharBase = nr3DCharBase2;
									vector = vector2;
								}
							}
						}
					}
				}
			}
		}
		Vector2 zero = Vector2.zero;
		if (nr3DCharBase != null)
		{
			float num6 = 1f + (num - vector.x) / num;
			zero.y = (nr3DCharBase.GetParentBattleChar().GetCharHalfBound() - vector.y) * num6;
			zero.x = (nr3DCharBase.GetParentBattleChar().GetCharHalfBound() - vector.x) * 0.2f;
			if (kChar.GetParentBattleChar().GetBUID() == 0)
			{
				Debug.Log("*** TYS : steering Force " + zero);
				Debug.Log("*** TYS : closestIntersectionObstacle " + nr3DCharBase.GetCharCode());
			}
		}
		return this.VectorToWorldSpace(zero, dir);
	}

	public Vector2 PointToLocalSpace(Vector2 pos, Vector2 agentDir, Vector2 agentPos)
	{
		Vector2 result = pos;
		Vector2 rhs = new Vector2(-agentDir.y, agentDir.x);
		float num = -Vector2.Dot(agentPos, agentDir);
		float num2 = -Vector2.Dot(agentPos, rhs);
		float[,] array = new float[3, 3];
		array[0, 0] = agentDir.x;
		array[0, 1] = rhs.x;
		array[1, 0] = agentDir.y;
		array[1, 1] = rhs.y;
		array[2, 0] = num;
		array[2, 1] = num2;
		float x = array[0, 0] * result.x + array[1, 0] * result.y + array[2, 0];
		float y = array[0, 1] * result.x + array[1, 1] * result.y + array[2, 1];
		result.x = x;
		result.y = y;
		return result;
	}

	public Vector2 VectorToLocalSpace(Vector2 vec, Vector2 agentDir)
	{
		Vector2 result = vec;
		Vector2 vector = new Vector2(-agentDir.y, agentDir.x);
		float[,] array = new float[3, 3];
		array[0, 0] = agentDir.x;
		array[0, 1] = vector.x;
		array[1, 0] = agentDir.y;
		array[1, 1] = vector.y;
		float x = array[0, 0] * result.x + array[1, 0] * result.y + array[2, 0];
		float y = array[0, 1] * result.x + array[1, 1] * result.y + array[2, 1];
		result.x = x;
		result.y = y;
		return result;
	}

	public Vector2 VectorToWorldSpace(Vector2 vec, Vector2 agentDir)
	{
		Vector2 result = vec;
		Vector2 vector = new Vector2(-agentDir.y, agentDir.x);
		Matrix4x4 matrix4x = default(Matrix4x4) * new Matrix4x4
		{
			m00 = agentDir.x,
			m01 = agentDir.y,
			m02 = 0f,
			m10 = vector.x,
			m11 = vector.y,
			m12 = 0f,
			m20 = 0f,
			m21 = 0f,
			m22 = 1f
		};
		float x = matrix4x[0, 0] * result.x + matrix4x[1, 0] * result.y + matrix4x[2, 0];
		float y = matrix4x[0, 1] * result.x + matrix4x[1, 1] * result.y + matrix4x[2, 1];
		result.x = x;
		result.y = y;
		return result;
	}

	public bool IsValidChar(int nCharID)
	{
		return nCharID >= 0 && nCharID < 120 && this.m_arChar[nCharID] != null;
	}

	public void SetSlowMotion()
	{
	}

	public void RestoreSlowMotion()
	{
	}

	public void SetFakeShadowEnable(bool bEnable)
	{
		if (this.m_bFakeShadowEnable == bEnable)
		{
			return;
		}
		NkBattleChar[] charArray = this.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar = charArray[i];
			if (nkBattleChar != null)
			{
				nkBattleChar.SetFakeShadowEnable(bEnable);
			}
		}
		this.m_bFakeShadowEnable = bEnable;
	}
}
