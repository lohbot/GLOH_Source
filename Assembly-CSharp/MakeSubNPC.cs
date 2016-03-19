using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MakeSubNPC : MonoBehaviour
{
	public delegate void LoadComplete(MakeSubNPC SubNPC);

	private const float distance = 15f;

	public NrCharBase m_CenterChar;

	public List<string> _MakeCharCodes;

	public List<int> _PositionList = new List<int>();

	public Dictionary<string, SubNpc> m_MakeNpcTable;

	public bool MakeComplete;

	private readonly float[] degreeTemp = new float[]
	{
		0f,
		30f,
		-30f,
		60f,
		-60f
	};

	private MakeSubNPC.LoadComplete _CompleteFunc;

	public bool SetMakeChar(NrCharBase CenterChar, string[] CharCodes, MakeSubNPC.LoadComplete CompleteFunc)
	{
		bool flag = false;
		if (this._MakeCharCodes == null)
		{
			this._MakeCharCodes = new List<string>();
		}
		this.m_CenterChar = CenterChar;
		this._MakeCharCodes.Clear();
		this._PositionList.Clear();
		for (int i = 0; i < CharCodes.Length; i++)
		{
			string text = CharCodes[i];
			if (!(text == string.Empty))
			{
				if (!(text == "event"))
				{
					if (this.m_MakeNpcTable != null && this.m_MakeNpcTable.ContainsKey(text))
					{
						SubNpc subNpc = this.m_MakeNpcTable[text];
						if (subNpc != null)
						{
							subNpc.bDel = false;
						}
					}
					else if (!text.Equals(this.m_CenterChar.GetCharKindInfo().GetCode()))
					{
						this._MakeCharCodes.Add(text);
						this._PositionList.Add(-1);
						flag = true;
					}
				}
			}
		}
		if (this.m_MakeNpcTable != null)
		{
			this.DelChar();
			foreach (SubNpc current in this.m_MakeNpcTable.Values)
			{
				current.bDel = true;
			}
		}
		if (flag)
		{
			this._CompleteFunc = CompleteFunc;
		}
		return flag;
	}

	public bool SetMakeChar(NrCharBase CenterChar, QUEST_NPC_POS_INFO npcPosInfo, MakeSubNPC.LoadComplete CompleteFunc)
	{
		bool flag = false;
		if (this._MakeCharCodes == null)
		{
			this._MakeCharCodes = new List<string>();
		}
		this.m_CenterChar = CenterChar;
		this._MakeCharCodes.Clear();
		this._PositionList.Clear();
		for (int i = 0; i < npcPosInfo.CHAR_CODE.Length; i++)
		{
			string text = npcPosInfo.CHAR_CODE[i];
			if (!(text == string.Empty))
			{
				if (!(text == "event"))
				{
					if (this.m_MakeNpcTable != null && this.m_MakeNpcTable.ContainsKey(text))
					{
						SubNpc subNpc = this.m_MakeNpcTable[text];
						if (subNpc != null)
						{
							subNpc.bDel = false;
						}
					}
					else if (!text.Equals(this.m_CenterChar.GetCharKindInfo().GetCode()))
					{
						this._MakeCharCodes.Add(text);
						flag = true;
					}
				}
			}
		}
		if (this.m_MakeNpcTable != null)
		{
			this.DelChar();
			foreach (SubNpc current in this.m_MakeNpcTable.Values)
			{
				current.bDel = true;
			}
		}
		if (flag)
		{
			this._CompleteFunc = CompleteFunc;
		}
		return flag;
	}

	public void DelChar()
	{
		if (this.m_MakeNpcTable != null && this.m_MakeNpcTable.Count > 0)
		{
			List<string> list = new List<string>();
			foreach (SubNpc current in this.m_MakeNpcTable.Values)
			{
				if (current != null)
				{
					if (current.bDel)
					{
						NrTSingleton<NkCharManager>.Instance.DeleteChar(current.i32ID);
						list.Add(current.CharCode);
					}
				}
			}
			foreach (string current2 in list)
			{
				this.m_MakeNpcTable.Remove(current2);
			}
			if (this.m_MakeNpcTable.Count <= 0)
			{
				this.m_MakeNpcTable.Clear();
			}
		}
	}

	public void Init()
	{
		this.m_CenterChar = null;
		this._MakeCharCodes = null;
		this.DelChar();
		this.MakeComplete = false;
	}

	public void MakeChar()
	{
		if (this.m_CenterChar == null || this._MakeCharCodes == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.MakeComplete = false;
		base.enabled = true;
		if (this.m_MakeNpcTable == null)
		{
			this.m_MakeNpcTable = new Dictionary<string, SubNpc>();
		}
		for (int i = 0; i < this._MakeCharCodes.Count; i++)
		{
			float y = this.m_CenterChar.m_k3DChar.GetRootGameObject().transform.localEulerAngles.y;
			float num = 0f;
			int num2 = this._PositionList[i];
			if (num2 == -1)
			{
				num = y + this.degreeTemp[this._GetEmptySlot()];
			}
			else if (0 <= num2 && 5 > num2)
			{
				num = y + this.degreeTemp[num2];
			}
			float f = num * 0.0174532924f;
			POS3D pOS3D = new POS3D();
			pOS3D.x = this.m_CenterChar.GetPersonInfo().GetCharPos().x + 15f * Mathf.Sin(f);
			pOS3D.y = 0f;
			pOS3D.z = this.m_CenterChar.GetPersonInfo().GetCharPos().z + 15f * Mathf.Cos(f);
			float f2 = (num + 180f) * 0.0174532924f;
			POS3D pOS3D2 = new POS3D();
			pOS3D2.x = 1f * Mathf.Sin(f2);
			pOS3D2.y = 0f;
			pOS3D2.z = 1f * Mathf.Cos(f2);
			NrCharBase nrCharBase = this._CreateClientNPC(this._MakeCharCodes[i], pOS3D, pOS3D2);
			if (nrCharBase != null)
			{
				SubNpc subNpc = new SubNpc();
				subNpc.i32ID = nrCharBase.GetID();
				subNpc.CharCode = nrCharBase.GetCharKindInfo().GetCode();
				subNpc.i16CharUnique = nrCharBase.GetCharUnique();
				if (!this.m_MakeNpcTable.ContainsKey(nrCharBase.GetCharKindInfo().GetCode()))
				{
					this.m_MakeNpcTable.Add(nrCharBase.GetCharKindInfo().GetCode(), subNpc);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (!this.MakeComplete && this.m_MakeNpcTable != null)
		{
			this.MakeComplete = true;
			if (this.m_MakeNpcTable.Count > 0)
			{
				foreach (SubNpc current in this.m_MakeNpcTable.Values)
				{
					if (!current.bLoad)
					{
						NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(current.i16CharUnique);
						if (charByCharUnique != null)
						{
							if (!charByCharUnique.IsGround())
							{
								this.MakeComplete = false;
							}
							if (charByCharUnique.IsReady3DModel())
							{
								charByCharUnique.SetShowHide3DModel(false, false, false);
								current.bLoad = true;
							}
						}
					}
				}
			}
			if (this.MakeComplete && this._CompleteFunc != null)
			{
				this._CompleteFunc(this);
				base.enabled = false;
			}
		}
	}

	private NrCharBase _CreateClientNPC(string CharCode, POS3D CharPos, POS3D CharDirection)
	{
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(CharCode);
		if (charKindInfoFromCode == null)
		{
			return null;
		}
		NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
		nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfoFromCode.GetName());
		nEW_MAKECHAR_INFO.CharPos.x = CharPos.x;
		nEW_MAKECHAR_INFO.CharPos.y = CharPos.y;
		nEW_MAKECHAR_INFO.CharPos.z = CharPos.z;
		nEW_MAKECHAR_INFO.Direction.x = CharDirection.x;
		nEW_MAKECHAR_INFO.Direction.y = CharDirection.y;
		nEW_MAKECHAR_INFO.Direction.z = CharDirection.z;
		nEW_MAKECHAR_INFO.CharKind = charKindInfoFromCode.GetCharKind();
		nEW_MAKECHAR_INFO.CharKindType = 3;
		nEW_MAKECHAR_INFO.CharUnique = (short)(31000 + this._GetEmptySlot());
		NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, false);
		NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(nEW_MAKECHAR_INFO.CharUnique);
		if (charByCharUnique != null)
		{
			if (charByCharUnique.IsHaveAnimation(eCharAnimationType.TalkStart1))
			{
				charByCharUnique.SetAnimationLoadAfter(eCharAnimationType.TalkStart1);
			}
			else if (charByCharUnique.IsHaveAnimation(eCharAnimationType.TalkStay1))
			{
				charByCharUnique.SetAnimationLoadAfter(eCharAnimationType.TalkStay1);
			}
			else
			{
				charByCharUnique.SetAnimationLoadAfter(eCharAnimationType.Stay1);
			}
		}
		return charByCharUnique;
	}

	public short GetCharUnique(string CharCode)
	{
		if (this.m_MakeNpcTable != null && this.m_MakeNpcTable.ContainsKey(CharCode))
		{
			return this.m_MakeNpcTable[CharCode].i16CharUnique;
		}
		if (this.m_CenterChar != null && this.m_CenterChar.GetCharKindInfo().GetCode().Equals(CharCode))
		{
			return this.m_CenterChar.GetCharUnique();
		}
		return 0;
	}

	private int _GetEmptySlot()
	{
		for (NrCharDefine.ReserveCharUnique reserveCharUnique = NrCharDefine.ReserveCharUnique.QUEST_SUB_NPC; reserveCharUnique < NrCharDefine.ReserveCharUnique.QUEST_SUB_NPC_END; reserveCharUnique += 1)
		{
			if (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)reserveCharUnique) == null)
			{
				return (int)(reserveCharUnique - NrCharDefine.ReserveCharUnique.QUEST_SUB_NPC);
			}
		}
		return 31004;
	}
}
