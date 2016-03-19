using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NkBattleChar : INrCharInput, ICloneable
{
	public enum e3DCharStep
	{
		IDLE,
		READY,
		CREATED,
		LOADCOMPLETED,
		FADEIN,
		CHARACTION,
		DELETED,
		DIED
	}

	protected NkBattleCharIDInfo m_kIDInfo;

	protected NrPersonInfoBattle m_kPersonInfo;

	public Nr3DCharBase m_k3DChar;

	protected NrCharAnimation m_kAnimation;

	protected NrCharKindInfo m_pkCharKindInfo;

	protected eCharKindType m_eCharKindType;

	protected bool m_bCreateAction;

	public NkBattleCharMove m_kCharMove;

	private float m_fCharSpeed = 10f;

	protected float m_fIncreaseCharSpeed;

	protected Vector3 m_vReservedMoveTarget = Vector3.zero;

	public List<Vector3> m_liServerPos = new List<Vector3>();

	public List<eBATTLE_MOVE_STATUS> m_liServerStatus = new List<eBATTLE_MOVE_STATUS>();

	public List<Vector3> m_liClientPos = new List<Vector3>();

	public List<Vector3> m_NavPath = new List<Vector3>();

	private NkCharPartControl m_kPartControl;

	private bool m_bSetFirstAction;

	public NrDebugLoger debugLog;

	private float m_fDeadTime;

	protected eCharAnimationType LoadAfterAnimation;

	private Color m_kCharColor = Color.white;

	private bool m_bProcessMouseEvent = true;

	protected bool m_bClickedMe;

	public bool m_bDeadReaservation;

	public float fCollisionTime;

	private Queue<GS_BF_ORDER_ACK> m_OrderACKQueue = new Queue<GS_BF_ORDER_ACK>();

	private eBATTLE_ALLY m_eAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private eBATTLE_ORDER m_eCurrentOrder;

	private eBATTLE_ORDER m_eNextOrder;

	public short m_nNextOrderTargetBUID = -1;

	private float m_OrderStartTime;

	public short m_nAttackTargetBUID = -1;

	public short m_nAttackGrid = -1;

	private int m_i32OrderUnique = -1;

	private bool m_bAIChar;

	private Battle_HpDlg m_HpDlg;

	private short m_nSolIdx = -1;

	private long m_nSolID;

	private Queue<ACQUIRED_ITEM> m_qAcquiredItem = new Queue<ACQUIRED_ITEM>();

	public BATTLESKILL_BASE m_CurrentBattleSkillBase;

	public BATTLESKILL_DETAIL m_CurrentBattleSkillDetail;

	private GS_CHAR_PATH[] m_MovePath;

	private eBATTLE_TURN_STATE m_eTurnState = eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE;

	private eCharAniTypeForEvent m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Attack1;

	private int m_nTotalHitCount;

	private int m_nProcessHitCount;

	public BATTLESKILL_BUF[] m_BattleSkillBufData = new BATTLESKILL_BUF[12];

	private int m_nAttackCharWeaponType;

	private uint m_nRunEffect;

	protected uint m_nFakeShadowEffectNum;

	protected uint m_nFaceSolGradeEffectNum;

	private byte m_nBattleCharType;

	private int m_nAddHP;

	private bool m_bLastAttacker;

	private bool m_bChangePos;

	private bool m_bRivalAttack;

	private static int[] m_nHitType = new int[]
	{
		1,
		2,
		3,
		4,
		5
	};

	private bool m_bMyChar;

	private int m_nBATTLEATB;

	private int m_nBattleSkillATB;

	private short m_nStartPosIndex;

	private short m_nGridPos;

	private float m_fGridRotate;

	private bool m_bNpcMode;

	private bool m_bComeBackRotate;

	private NkHeadUpEntity m_kHeadUpEntity;

	public Color m_ClientColor = new Color(1f, 1f, 0f, 0.2f);

	public Color m_ServerColor = new Color(1f, 0f, 0f, 0.2f);

	private int nShowIndex;

	public bool m_bCreateBullet;

	private bool m_bSkillWait;

	private float m_fSkillAniEndtime;

	public NkBattleChar.e3DCharStep m_e3DCharStep
	{
		get;
		protected set;
	}

	public float m_fCurrentAlphaFadeLerp
	{
		get;
		protected set;
	}

	public bool ProcessMouseEvent
	{
		get
		{
			return this.m_bProcessMouseEvent;
		}
		set
		{
			this.m_bProcessMouseEvent = value;
		}
	}

	public eBATTLE_ALLY Ally
	{
		get
		{
			return this.m_eAlly;
		}
		set
		{
			this.m_eAlly = value;
		}
	}

	public bool AIChar
	{
		get
		{
			return this.m_bAIChar;
		}
		set
		{
			this.m_bAIChar = value;
		}
	}

	public GS_CHAR_PATH[] MovePath
	{
		get
		{
			return this.m_MovePath;
		}
		set
		{
			this.m_MovePath = value;
		}
	}

	public uint RunEffect
	{
		get
		{
			return this.m_nRunEffect;
		}
		set
		{
			this.m_nRunEffect = value;
		}
	}

	public byte BattleCharType
	{
		get
		{
			return this.m_nBattleCharType;
		}
		set
		{
			this.m_nBattleCharType = value;
		}
	}

	public bool IsMonster
	{
		get
		{
			return (this.m_nBattleCharType & 1) > 0;
		}
	}

	public bool IsFriend
	{
		get
		{
			return (this.m_nBattleCharType & 2) > 0;
		}
	}

	public bool IsChangeSoldier
	{
		get
		{
			return (this.m_nBattleCharType & 4) > 0;
		}
	}

	public bool IsDefenceObject
	{
		get
		{
			return (this.m_nBattleCharType & 8) > 0;
		}
	}

	public bool IsReviveChar
	{
		get
		{
			return (this.m_nBattleCharType & 16) > 0;
		}
	}

	public int AddHP
	{
		get
		{
			return this.m_nAddHP;
		}
		set
		{
			this.m_nAddHP = value;
		}
	}

	public bool IsLastAttacker
	{
		get
		{
			return this.m_bLastAttacker;
		}
	}

	public bool MyChar
	{
		get
		{
			return this.m_bMyChar;
		}
		set
		{
			this.m_bMyChar = value;
		}
	}

	public NkBattleChar()
	{
		this.m_fCurrentAlphaFadeLerp = 0f;
		this.m_e3DCharStep = NkBattleChar.e3DCharStep.IDLE;
		this.debugLog = new NrDebugLoger();
		this.m_kIDInfo = new NkBattleCharIDInfo(-1, -1, -1);
		this.m_kPersonInfo = new NrPersonInfoBattle();
		this.m_kAnimation = new NrCharAnimation();
		this.m_kCharMove = new NkBattleCharMove(this);
		this.Init();
	}

	public int GetMaxHP(bool bExecptAddHP)
	{
		if (this.GetSoldierInfo() == null)
		{
			return 0;
		}
		if (bExecptAddHP)
		{
			return this.GetSoldierInfo().GetMaxHP();
		}
		return this.GetSoldierInfo().GetMaxHP() + this.m_nAddHP;
	}

	public void SetAttackAniTypeEvent(eCharAniTypeForEvent eAniType)
	{
		this.m_eAttackTypeAniEvent = eAniType;
	}

	public eCharAnimationType GetAnimationFromAniType()
	{
		if (this.m_eAttackTypeAniEvent == eCharAniTypeForEvent.ANIKEY_Attack1)
		{
			return eCharAnimationType.Attack1;
		}
		if (this.m_eAttackTypeAniEvent == eCharAniTypeForEvent.ANIKEY_Attack2)
		{
			return eCharAnimationType.Attack2;
		}
		if (this.m_eAttackTypeAniEvent == eCharAniTypeForEvent.ANIKEY_Attack3)
		{
			return eCharAnimationType.Attack3;
		}
		if (this.m_eAttackTypeAniEvent == eCharAniTypeForEvent.ANIKEY_AttackLeft1)
		{
			return eCharAnimationType.AttackLeft1;
		}
		if (this.m_eAttackTypeAniEvent == eCharAniTypeForEvent.ANIKEY_AttackRight1)
		{
			return eCharAnimationType.AttackRight1;
		}
		return eCharAnimationType.Attack1;
	}

	public eCharAniTypeForEvent GetAniTypeFromAnimation(eCharAnimationType anitype)
	{
		if (anitype == eCharAnimationType.Attack1)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Attack1;
		}
		else if (anitype == eCharAnimationType.Attack2)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Attack2;
		}
		else if (anitype == eCharAnimationType.Attack3)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Attack3;
		}
		else if (anitype == eCharAnimationType.AttackLeft1)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_AttackLeft1;
		}
		else if (anitype == eCharAnimationType.AttackRight1)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_AttackRight1;
		}
		else if (anitype == eCharAnimationType.Skill1)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Skill1;
		}
		else if (anitype == eCharAnimationType.Skill2)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Skill2;
		}
		else if (anitype == eCharAnimationType.Skill3)
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Skill3;
		}
		else
		{
			this.m_eAttackTypeAniEvent = eCharAniTypeForEvent.ANIKEY_Attack1;
		}
		return this.m_eAttackTypeAniEvent;
	}

	public void SetOrderUnique(int iOrderUnique)
	{
		if (this.m_i32OrderUnique > 0)
		{
			Battle.BATTLE.UpdateCharInfoNFY(this.m_i32OrderUnique);
		}
		this.m_i32OrderUnique = iOrderUnique;
	}

	public void SetBattleCharATB(int _atb)
	{
		if ((this.m_nBATTLEATB & _atb) == 0)
		{
			this.m_nBATTLEATB |= _atb;
		}
		this.OnChangeATB();
	}

	public bool IsBattleCharATB(int _atb)
	{
		return (this.m_nBATTLEATB & _atb) > 0 || (this.m_nBattleSkillATB & _atb) > 0;
	}

	public void InitBattleCharATB()
	{
		this.m_nBATTLEATB = 0;
	}

	public void ChangeHPDlgColor()
	{
		if (this.m_HpDlg != null)
		{
			this.m_HpDlg.ChangeColor();
		}
	}

	public void SetBattleSkillCharATB(int _atb)
	{
		this.InitBattleSkillCharATB();
		this.m_nBattleSkillATB = _atb;
	}

	public int GetBattleSkillCharATB()
	{
		return this.m_nBattleSkillATB;
	}

	public void InitBattleSkillCharATB()
	{
		this.m_nBattleSkillATB = 0;
	}

	public void DelBattleSkillCharATB(int _atb)
	{
		this.m_nBattleSkillATB &= ~_atb;
	}

	public void SetStartPosIndex(short nStartPosIndex)
	{
		this.m_nStartPosIndex = nStartPosIndex;
	}

	public short GetStartPosIndex()
	{
		return this.m_nStartPosIndex;
	}

	public void SetGridPos(short nGridPos)
	{
		this.m_nGridPos = nGridPos;
	}

	public short GetGridPos()
	{
		return this.m_nGridPos;
	}

	public void SetGridRotate(float fRotate)
	{
		this.m_fGridRotate = fRotate;
	}

	public float GetGridRotate()
	{
		return this.m_fGridRotate;
	}

	public void SetNpcMode(bool bMode)
	{
		this.m_bNpcMode = bMode;
	}

	public bool IsNpcMode()
	{
		return this.m_bNpcMode;
	}

	public void SetComeBackRotate(bool bSet)
	{
		this.m_bComeBackRotate = bSet;
	}

	public bool GetComeBackRotate()
	{
		return this.m_bComeBackRotate;
	}

	public void SetCollisionTime(float collisionTime)
	{
		this.fCollisionTime = collisionTime;
	}

	public float GetCollisionTime()
	{
		return this.fCollisionTime;
	}

	public void SetSolIdx(short SolIdx)
	{
		this.m_nSolIdx = SolIdx;
	}

	public short GetSolIdx()
	{
		return this.m_nSolIdx;
	}

	public void SetSolID(long nSolID)
	{
		this.m_nSolID = nSolID;
	}

	public long GetSolID()
	{
		return this.m_nSolID;
	}

	public void SetCurrentOrder(eBATTLE_ORDER eOrder)
	{
		if (this.m_eCurrentOrder == eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
		{
			if (this.RunEffect != 0u)
			{
				NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.RunEffect);
				this.RunEffect = 0u;
			}
		}
		else if (this.m_eCurrentOrder == eBATTLE_ORDER.eBATTLE_ORDER_SKILL && eOrder != eBATTLE_ORDER.eBATTLE_ORDER_NONE && this.m_bLastAttacker)
		{
			Battle.BATTLE.BattleCamera.SetLastAttackCamera(this, false);
			this.m_bLastAttacker = false;
		}
		this.m_eCurrentOrder = eOrder;
		this.m_OrderStartTime = 0f;
	}

	public eBATTLE_ORDER GetCurrentOrder()
	{
		return this.m_eCurrentOrder;
	}

	public void SetTurnState(eBATTLE_TURN_STATE eState)
	{
		this.m_eTurnState = eState;
		if (Battle.BATTLE.ColosseumObserver)
		{
			ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
			if (colosseumObserverControlDlg != null)
			{
				colosseumObserverControlDlg.SetEnableTurn(this.Ally, this.GetBUID(), this.m_eTurnState != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
			}
		}
	}

	public eBATTLE_TURN_STATE GetTurnState()
	{
		return this.m_eTurnState;
	}

	public string GetCurOrderToString()
	{
		return this.m_eCurrentOrder.ToString().Replace("eBATTLE_ORDER_", string.Empty);
	}

	public void SetNextOrder(eBATTLE_ORDER eOrder)
	{
		this.m_eNextOrder = eOrder;
	}

	public eBATTLE_ORDER GetNextOrder()
	{
		return this.m_eNextOrder;
	}

	public void SetNextOrderTarget(short buid)
	{
		this.m_nNextOrderTargetBUID = buid;
	}

	public void SetAttackTarget(short buid, short nTargetGrid)
	{
		this.m_nAttackTargetBUID = buid;
		this.m_nAttackGrid = nTargetGrid;
	}

	public void SetBattleSkillCurrent(int skillUnique, int skillLevel)
	{
		int skillLevel2;
		if (skillLevel > 0)
		{
			skillLevel2 = skillLevel;
		}
		else if (this.m_eCharKindType == eCharKindType.CKT_MONSTER || this.m_eCharKindType == eCharKindType.CKT_NPC)
		{
			skillLevel2 = this.GetCharKindInfo().GetBattleSkillLevel(skillUnique);
		}
		else
		{
			skillLevel2 = this.GetSoldierInfo().GetBattleSkillLevel(skillUnique);
		}
		this.m_CurrentBattleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		this.m_CurrentBattleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillUnique, skillLevel2);
	}

	public int GetBattleSkillUnique(int index)
	{
		int battleSkillUnique;
		if (this.m_eCharKindType == eCharKindType.CKT_MONSTER || this.m_eCharKindType == eCharKindType.CKT_NPC)
		{
			battleSkillUnique = this.GetCharKindInfo().GetBattleSkillUnique(index);
		}
		else
		{
			battleSkillUnique = this.GetSoldierInfo().GetBattleSkillUnique(index);
		}
		return battleSkillUnique;
	}

	public int GetBattleSkillLevelByIndex(int index)
	{
		int battleSkillLevelByIndex;
		if (this.m_eCharKindType == eCharKindType.CKT_MONSTER || this.m_eCharKindType == eCharKindType.CKT_NPC)
		{
			battleSkillLevelByIndex = this.GetCharKindInfo().GetBattleSkillLevelByIndex(index);
		}
		else
		{
			battleSkillLevelByIndex = this.GetSoldierInfo().GetBattleSkillLevelByIndex(index);
		}
		return battleSkillLevelByIndex;
	}

	public int GetBattleSkillLevelByUnique(int skillUnique)
	{
		int battleSkillLevel;
		if (this.m_eCharKindType == eCharKindType.CKT_MONSTER || this.m_eCharKindType == eCharKindType.CKT_NPC)
		{
			battleSkillLevel = this.GetCharKindInfo().GetBattleSkillLevel(skillUnique);
		}
		else
		{
			battleSkillLevel = this.GetSoldierInfo().GetBattleSkillLevel(skillUnique);
		}
		return battleSkillLevel;
	}

	public int GetBattleSkillIndexByUnique(int skillunique)
	{
		int battleSkillIndexByUnique;
		if (this.m_eCharKindType == eCharKindType.CKT_MONSTER || this.m_eCharKindType == eCharKindType.CKT_NPC)
		{
			battleSkillIndexByUnique = this.GetCharKindInfo().GetBattleSkillIndexByUnique(skillunique);
		}
		else
		{
			battleSkillIndexByUnique = this.GetSoldierInfo().GetBattleSkillIndexByUnique(skillunique);
		}
		return battleSkillIndexByUnique;
	}

	public void SetBattleSkillCoolTurn(bool initTurn)
	{
	}

	public void DecreaseBattleSkillCoolTurn()
	{
	}

	public void SetClearAttackTarget()
	{
		this.m_nNextOrderTargetBUID = -1;
		this.m_nAttackTargetBUID = -1;
	}

	public void SetClickMe()
	{
		this.m_bClickedMe = true;
	}

	public void CancelClickMe()
	{
		this.m_bClickedMe = false;
	}

	public string GetState()
	{
		string result = string.Empty;
		if (this.m_nAttackTargetBUID > 0)
		{
			NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_nAttackTargetBUID);
			if (charByBUID != null)
			{
				result = string.Format("\r\nName : {0} HP : {1} {2}({3}) {4} {5}", new object[]
				{
					this.Get3DName(),
					this.GetSoldierInfo().GetHP(),
					charByBUID.Get3DName(),
					charByBUID.GetSoldierInfo().GetHP(),
					this.GetCharPos(),
					this.GetCurOrderToString()
				});
			}
			else
			{
				result = string.Format("\r\nName : {0} HP : {1} Node : {2} {3}", new object[]
				{
					this.Get3DName(),
					this.GetSoldierInfo().GetHP(),
					this.GetCharPos(),
					this.GetCurOrderToString()
				});
			}
		}
		else
		{
			result = string.Format("\r\nName : {0} HP : {1} Node : {2} {3}", new object[]
			{
				this.Get3DName(),
				this.GetSoldierInfo().GetHP(),
				this.GetCharPos(),
				this.GetCurOrderToString()
			});
		}
		return result;
	}

	public void Init()
	{
		if (this.m_kIDInfo != null)
		{
			this.m_kIDInfo.Init();
		}
		this.m_kPersonInfo.Init();
		this.m_pkCharKindInfo = null;
		this.m_eCharKindType = eCharKindType.CKT_USER;
		this.Set3DCharStep(NkBattleChar.e3DCharStep.IDLE);
		this.m_kAnimation.Init(string.Empty, this.m_k3DChar, this.m_eCharKindType);
		this.m_kCharMove.Init();
		this.m_fCharSpeed = 10f;
		this.m_fIncreaseCharSpeed = 0f;
		this.m_vReservedMoveTarget = Vector3.zero;
		this.m_bMyChar = false;
		this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
		this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
		for (int i = 0; i < 12; i++)
		{
			this.m_BattleSkillBufData[i].init();
		}
		this.m_nAddHP = 0;
		this.m_bLastAttacker = false;
		this.m_bChangePos = true;
		this.InitRelease();
	}

	private void InitRelease()
	{
		this.m_bCreateAction = false;
		this.m_k3DChar = null;
		if (this.m_kPartControl != null)
		{
			this.m_kPartControl.Init();
		}
		this.m_bSetFirstAction = false;
		this.m_fDeadTime = 0f;
		this.LoadAfterAnimation = eCharAnimationType.Respawn1;
		this.m_fCurrentAlphaFadeLerp = 0f;
		if (this.m_HpDlg != null)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(this.m_HpDlg.WindowID);
			this.m_HpDlg = null;
		}
		this.SetClearAttackTarget();
		this.HeadUp_Init();
		if (this.m_nFakeShadowEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nFakeShadowEffectNum);
			this.m_nFakeShadowEffectNum = 0u;
		}
		if (this.m_nFaceSolGradeEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_nFaceSolGradeEffectNum);
			this.m_nFaceSolGradeEffectNum = 0u;
		}
	}

	public void InitTurn()
	{
		if (Battle.BATTLE.CurrentTurnAlly == this.m_eAlly && !this.m_bDeadReaservation && !this.IsBattleCharATB(8) && !this.IsBattleCharATB(64))
		{
			this.SetTurnState(eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE);
			this.m_bChangePos = true;
			if (Battle.BATTLE.m_nTurnCount == 1 && (this.m_eCharKindType == eCharKindType.CKT_USER || this.m_eCharKindType == eCharKindType.CKT_SOLDIER))
			{
				this.SetBattleSkillCoolTurn(true);
			}
			else
			{
				this.DecreaseBattleSkillCoolTurn();
			}
		}
		else
		{
			this.SetTurnState(eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
			this.m_bChangePos = false;
		}
		this.CheckBattleSkillBuffeffect();
		if (this.IsCharKindATB(128L) && Battle.BATTLE.CurrentTurnAlly == this.m_eAlly)
		{
			Battle_BossAggro_DLG battle_BossAggro_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG) as Battle_BossAggro_DLG;
			if (battle_BossAggro_DLG != null)
			{
				battle_BossAggro_DLG.DecBossImmuneCount();
				battle_BossAggro_DLG.UpdateBossInfo();
			}
		}
		if (this.m_eCurrentOrder != eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetAnimation(this.GetStayAni());
		}
		if (this.m_bLastAttacker)
		{
			Battle.BATTLE.BattleCamera.SetLastAttackCamera(this, false);
			this.m_bLastAttacker = false;
		}
	}

	public void Set3DCharStep(NkBattleChar.e3DCharStep charstep)
	{
		this.m_e3DCharStep = charstep;
	}

	public NkBattleChar.e3DCharStep Get3DCharStep()
	{
		return this.m_e3DCharStep;
	}

	public void SetPersonInfo(NrPersonInfoBase pkPersonInfo)
	{
		if (pkPersonInfo != null)
		{
			if (!pkPersonInfo.GetLeaderSoldierInfo().IsValid())
			{
				NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
				nkSoldierInfo.SetSolID(pkPersonInfo.GetBasicInfo().m_nSolID);
				pkPersonInfo.SetSoldierInfo(0, nkSoldierInfo);
			}
			this.m_kPersonInfo.SetPersonInfo(pkPersonInfo);
			if (this.m_eCharKindType == eCharKindType.CKT_USER && this.m_kPartControl == null)
			{
				this.m_kPartControl = new NkCharPartControl();
				this.SetReadyPartInfo();
			}
			this.SetKind(this.m_kPersonInfo.GetKind(0), false);
			if (this.m_eCharKindType != eCharKindType.CKT_USER)
			{
				this.m_kPersonInfo.SetCharName(this.m_pkCharKindInfo.GetName());
			}
		}
	}

	public NrPersonInfoBase GetPersonInfo()
	{
		return this.m_kPersonInfo;
	}

	public void SetKind(int charkind, bool bChanged)
	{
		if (bChanged)
		{
			this.m_kPersonInfo.SetCharKind(0, charkind);
		}
		this.m_pkCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charkind);
		if (this.m_kPartControl != null)
		{
			this.m_kPartControl.SetCharKindInfo(this.m_pkCharKindInfo);
		}
	}

	public NrCharKindInfo GetCharKindInfo()
	{
		return this.m_pkCharKindInfo;
	}

	public long GetCharATB()
	{
		if (this.m_pkCharKindInfo == null)
		{
			return 0L;
		}
		return this.m_pkCharKindInfo.GetATB();
	}

	public float GetCharBound()
	{
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetBound();
		}
		return 1f;
	}

	public float GetCharHalfBound()
	{
		return this.GetCharBound() / 2f;
	}

	public float GetMoveRange()
	{
		CHARKIND_ATTACKINFO attackInfo = this.GetAttackInfo();
		if (attackInfo != null)
		{
			return attackInfo.fMoveRange;
		}
		return 1f;
	}

	public float GetAttackRange()
	{
		CHARKIND_ATTACKINFO attackInfo = this.GetAttackInfo();
		if (attackInfo != null)
		{
			return attackInfo.fAttackRange;
		}
		return 1f;
	}

	public float GetSightRange()
	{
		CHARKIND_ATTACKINFO attackInfo = this.GetAttackInfo();
		if (attackInfo != null)
		{
			return attackInfo.fSightRange;
		}
		return 1f;
	}

	public CHARKIND_ATTACKINFO GetAttackInfo()
	{
		if (this.GetSoldierInfo().GetAttackInfo() != null)
		{
			return this.GetSoldierInfo().GetAttackInfo();
		}
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetCHARKIND_ATTACKINFO();
		}
		return null;
	}

	public void SetIDInfo(NkBattleCharIDInfo kIDInfo)
	{
		this.m_kIDInfo = kIDInfo;
	}

	public NkBattleCharIDInfo GetIDInfo()
	{
		return this.m_kIDInfo;
	}

	public int GetID()
	{
		return this.m_kIDInfo.m_nClientID;
	}

	public short GetBUID()
	{
		return this.m_kIDInfo.m_nBUID;
	}

	public int Get3DID()
	{
		return this.m_kIDInfo.m_nClientID + 300;
	}

	public short GetCharUnique()
	{
		return this.m_kIDInfo.m_nCharUnique;
	}

	public long GetPersonID()
	{
		return this.m_kPersonInfo.GetPersonID();
	}

	public Nr3DCharBase Get3DChar()
	{
		return this.m_k3DChar;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public void SetKindType(eCharKindType kindtype)
	{
		this.m_eCharKindType = kindtype;
	}

	public eCharKindType GetCharKindType()
	{
		return this.m_eCharKindType;
	}

	public NrCharAnimation GetCharAnimation()
	{
		return this.m_kAnimation;
	}

	public string GetWeaponCode()
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null)
		{
			return soldierInfo.GetWeaponCode();
		}
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetWeaponCode();
		}
		return eWEAPON_TYPE.WEAPON_SWORD.ToString();
	}

	public int GetWeaponType()
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null)
		{
			return soldierInfo.GetWeaponType();
		}
		if (this.m_pkCharKindInfo != null)
		{
			return this.m_pkCharKindInfo.GetWeaponType();
		}
		return 1;
	}

	public byte GetJobType()
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null)
		{
			return soldierInfo.GetJobType();
		}
		return 1;
	}

	public bool IsGround()
	{
		return this.m_k3DChar != null && this.m_k3DChar.IsCreated() && this.m_k3DChar.IsGround();
	}

	public void SetShowHide3DModel(bool bShow, bool bHeadUpShow, bool bNameCheck)
	{
		if (this.m_k3DChar == null)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				Debug.LogError("SetShowHide3DModel not found 3D model resource.");
			}
			return;
		}
		this.m_k3DChar.SetShowHide(bShow);
		this.SetShowHeadUp(bHeadUpShow, false, bNameCheck);
	}

	public Nr3DCharBase Create3DGrahpicData()
	{
		if (this.m_k3DChar != null)
		{
			this.Release();
		}
		string text = this.GetCharName();
		text = text + "_" + this.GetBUID().ToString();
		if (this.m_eCharKindType == eCharKindType.CKT_USER)
		{
			this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Create3DChar<Nr3DCharActor>(this.Get3DID(), text);
			if (this.m_k3DChar == null)
			{
				this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Get3DChar(this.Get3DID());
				if (this.m_k3DChar == null)
				{
					return null;
				}
			}
			this.SetKind(this.m_kPersonInfo.GetKind(0), false);
		}
		else
		{
			this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Create3DChar<Nr3DCharNonePart>(this.Get3DID(), text);
			if (this.m_k3DChar == null)
			{
				return null;
			}
		}
		if (this.m_kPartControl != null)
		{
			this.m_kPartControl.SetPartControl(this.m_k3DChar, false);
		}
		this.m_k3DChar.removeScript = true;
		this.Set3DCharStep(NkBattleChar.e3DCharStep.CREATED);
		return this.m_k3DChar;
	}

	private void MakeBattleUser()
	{
		this.SetReadyPartInfo();
		this.m_kPartControl.ResetBaseBone();
	}

	private void MakeBattleNPC()
	{
		Nr3DCharNonePart nr3DCharNonePart = this.m_k3DChar as Nr3DCharNonePart;
		nr3DCharNonePart.SwitchModelMesh();
	}

	protected void OnCreateAction()
	{
		if (this.m_eCharKindType == eCharKindType.CKT_USER)
		{
			this.MakeBattleUser();
		}
		else
		{
			this.MakeBattleNPC();
		}
		this.m_k3DChar.Reset();
		this.m_k3DChar.SetEvent3DModelCreated(new Nr3DCharBase.func3DModelCreated(NrTSingleton<Nr3DCharSystem>.Instance.OnEvent3DModelCreated));
		if (this.IsCharKindATB(8388608L))
		{
			this.SetAnimationLoadAfter(eCharAnimationType.ExtAttack1);
		}
		else
		{
			this.SetAnimationLoadAfter(eCharAnimationType.Respawn1);
		}
		this.m_bCreateAction = true;
	}

	public void Release()
	{
		if (this.m_k3DChar != null)
		{
			NrTSingleton<Nr3DCharSystem>.Instance.Destroy3DChar(this.Get3DID());
			this.m_k3DChar = null;
		}
		this.InitRelease();
	}

	private void SetFirstAction()
	{
		if (!this.m_bSetFirstAction)
		{
			if (this.m_k3DChar != null)
			{
				Vector3 charPos = this.m_kPersonInfo.GetCharPos();
				Vector3 vector = charPos;
				GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
				if (0f >= charPos.y)
				{
					vector.y = 1000f;
				}
				Ray ray = new Ray(vector, new Vector3(0f, -1f, 0f));
				int mask = NrTSingleton<NkClientLogic>.Instance.CharColliderLayerMask();
				NkRaycast.Raycast(ray, 2000f, mask);
				Vector3 vector2 = NkRaycast.POINT;
				if (vector2 == Vector3.zero)
				{
					vector2 = vector;
				}
				else if (0f >= vector2.y)
				{
					vector2.y = vector.y;
				}
				vector2.y += 1f;
				rootGameObject.transform.localPosition = vector2;
				if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
				{
					Vector3 vDirection = this.m_kPersonInfo.GetBasicInfo().m_vDirection;
					this.m_k3DChar.SetLookAt(vDirection.x, vDirection.y, vDirection.z, false);
				}
				else
				{
					this.SetRotate(this.GetGridRotate());
				}
				this.m_kCharMove.SetCharPos(this.m_k3DChar.GetRootGameObject());
			}
			this.SetAnimation(this.LoadAfterAnimation, true, false);
			this.m_bSetFirstAction = true;
		}
	}

	public virtual void Update()
	{
		switch (this.m_e3DCharStep)
		{
		case NkBattleChar.e3DCharStep.IDLE:
			switch (Scene.CurScene)
			{
			case Scene.Type.EMPTY:
			case Scene.Type.ERROR:
			case Scene.Type.SYSCHECK:
			case Scene.Type.PREDOWNLOAD:
			case Scene.Type.NPATCH_DOWNLOAD:
			case Scene.Type.LOGIN:
			case Scene.Type.INITIALIZE:
			case Scene.Type.PREPAREGAME:
			case Scene.Type.JUSTWAIT:
			case Scene.Type.WORLD:
			case Scene.Type.CUTSCENE:
				goto IL_9C;
			}
			this.debugLog.Log("state IDLE.");
			this.Set3DCharStep(NkBattleChar.e3DCharStep.READY);
			IL_9C:
			break;
		case NkBattleChar.e3DCharStep.READY:
			this.debugLog.Log("state READY.");
			if (StageSystem.IsStable)
			{
				if (this.IsReayCreateCharInfo())
				{
					this.Create3DGrahpicData();
					this.m_bSetFirstAction = false;
					this.m_fCurrentAlphaFadeLerp = 0f;
				}
			}
			break;
		case NkBattleChar.e3DCharStep.CREATED:
			if (!this.m_bCreateAction)
			{
				this.OnCreateAction();
			}
			break;
		case NkBattleChar.e3DCharStep.LOADCOMPLETED:
			this.debugLog.Log("state LOADCOMPLETED.");
			this.SetFirstAction();
			if (!NrCharAnimation.IsIdleAnimation(this.LoadAfterAnimation))
			{
				this.m_kAnimation.ProcessAnimation(Time.deltaTime);
			}
			if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY && this.Ally == Battle.BATTLE.MyAlly && !this.MyChar && this.IsMonster)
			{
				NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_AI_MARK", this, true, false);
			}
			if (this.GetCharKindInfo().GetCharKind() == 703)
			{
				NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_TREASURE_HEADUP", this);
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("482"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			this.MakeFadeIn();
			break;
		case NkBattleChar.e3DCharStep.FADEIN:
			this.MakeFadeIn();
			this.SetCharAction();
			break;
		case NkBattleChar.e3DCharStep.CHARACTION:
			this.SetCharAction();
			break;
		case NkBattleChar.e3DCharStep.DELETED:
			this.debugLog.Log("state DELETED.");
			this.Release();
			this.m_bSetFirstAction = false;
			this.Set3DCharStep(NkBattleChar.e3DCharStep.IDLE);
			if (this.m_kCharMove.IsFastMoving())
			{
				this.m_kPersonInfo.SetCharPos(this.m_kCharMove.GetTargetPos());
				this.MoveTo(this.m_kCharMove.GetFastMoveNextTargetPos());
			}
			else if (this.m_kCharMove.IsMoving())
			{
				this.MoveTo(this.m_kCharMove.GetTargetPos());
			}
			break;
		case NkBattleChar.e3DCharStep.DIED:
			this.debugLog.Log("state DIED.");
			if (this.m_fDeadTime != 0f)
			{
				if (Time.time - this.m_fDeadTime > 1f)
				{
					if (this.m_k3DChar != null)
					{
						GameObject boneRootObject = this.m_k3DChar.GetBoneRootObject();
						if (boneRootObject != null)
						{
							Vector3 position = boneRootObject.transform.position;
							position.y -= 0.02f;
							boneRootObject.transform.position = position;
						}
					}
					if (Time.time - this.m_fDeadTime > 7f)
					{
						NrTSingleton<NkBattleCharManager>.Instance.DeleteChar(this.GetID());
						this.m_fDeadTime = 0f;
					}
				}
			}
			else
			{
				this.m_kAnimation.ProcessAnimation(Time.deltaTime);
			}
			break;
		}
	}

	private void MakeFadeIn()
	{
		this.debugLog.Log("state fade in " + this.m_fCurrentAlphaFadeLerp.ToString());
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		if (this.m_fCurrentAlphaFadeLerp <= 0f)
		{
			Renderer[] componentsInChildren = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Renderer renderer = componentsInChildren[i];
				if (renderer.gameObject.name.Contains("fx_"))
				{
					renderer.renderer.enabled = false;
				}
				else
				{
					renderer.renderer.enabled = true;
				}
			}
			this.m_fCurrentAlphaFadeLerp = 0.01f;
			this.m_k3DChar.SetEvent3DModelPartItemChanged(new Nr3DCharBase.func3DModelCreated(NrTSingleton<Nr3DCharSystem>.Instance.OnEvent3DModelPartItemChanged));
			this.Set3DCharStep(NkBattleChar.e3DCharStep.FADEIN);
			this.OneTimeAction();
		}
		else if (this.m_fCurrentAlphaFadeLerp < 1f)
		{
			Renderer[] componentsInChildren2 = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Renderer renderer2 = componentsInChildren2[j];
				if (!(renderer2.particleEmitter != null) || !(renderer2.particleSystem != null))
				{
					Material[] materials = renderer2.materials;
					for (int k = 0; k < materials.Length; k++)
					{
						Material material = materials[k];
						if (material.HasProperty("_Color"))
						{
							this.m_kCharColor = material.color;
							this.m_kCharColor.a = this.m_fCurrentAlphaFadeLerp;
							material.color = this.m_kCharColor;
						}
					}
				}
			}
			this.m_fCurrentAlphaFadeLerp = Mathf.Lerp(this.m_fCurrentAlphaFadeLerp, 1.1f, 5f * Time.deltaTime);
			this.m_fCurrentAlphaFadeLerp *= 3f;
		}
		else
		{
			string text = string.Empty;
			Renderer[] componentsInChildren3 = rootGameObject.GetComponentsInChildren<Renderer>(true);
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				Renderer renderer3 = componentsInChildren3[l];
				if (!(renderer3.particleEmitter != null) || !(renderer3.particleSystem != null))
				{
					Material[] materials2 = renderer3.materials;
					for (int m = 0; m < materials2.Length; m++)
					{
						Material material2 = materials2[m];
						if (material2.HasProperty("_Color"))
						{
							this.m_kCharColor = material2.color;
							this.m_kCharColor.a = 1f;
							text = NrTSingleton<Nr3DCharSystem>.Instance.PopAlphaShaderRecovery(material2.name);
							material2.shader = Shader.Find(text);
							if (material2.shader == null)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"SkinnedMeshRenderer Shader not found ===> name : [",
									material2.name,
									"] (",
									text,
									")"
								}));
							}
							material2.color = this.m_kCharColor;
						}
					}
				}
			}
			this.OnRecoveryShader();
			this.Set3DCharStep(NkBattleChar.e3DCharStep.CHARACTION);
		}
	}

	protected void OnRecoveryShader()
	{
		this.MakeCharName(false);
		this.m_k3DChar.OnRecoveryEnchantWeapon();
		bool fakeShadowEnable = !TsQualityManager.Instance.CurrQuality.EnableShadow;
		if (TsPlatform.IsMobile)
		{
			fakeShadowEnable = true;
		}
		this.SetFakeShadowEnable(fakeShadowEnable);
	}

	protected virtual void OneTimeAction()
	{
		this.m_kAnimation.SetBattleState();
		this.m_vReservedMoveTarget = new Vector3(0f, 0f, 0f);
		this.m_kCharMove.SetCharPos(this.m_k3DChar.GetRootGameObject());
		if (this.m_vReservedMoveTarget.x > 0f || this.m_vReservedMoveTarget.z > 0f)
		{
			this.MoveTo(this.m_vReservedMoveTarget.x, this.m_vReservedMoveTarget.y, this.m_vReservedMoveTarget.z, false);
		}
	}

	protected virtual void SetCharAction()
	{
		this.ProcessOrder();
		this.m_kAnimation.ProcessAnimation(Time.deltaTime);
		this.Update_MouseEvent();
	}

	public void Loaded3DChar()
	{
		this.OnLoaded3DChar();
	}

	public virtual bool OnLoaded3DChar()
	{
		if (this.m_k3DChar == null)
		{
			return false;
		}
		if (this.IsReady3DModel())
		{
			NrCharInfoAdaptor nrCharInfoAdaptor = NkUtil.GuarranteeComponent<NrCharInfoAdaptor>(this.m_k3DChar.GetRootGameObject());
			nrCharInfoAdaptor.SetCharInfoInterface(new NrCharInfoLogic(this));
			this.OnRecoveryShader();
			this.SetAnimation(this.m_kAnimation.GetCurrentAniType());
			return false;
		}
		int scale = (int)this.m_pkCharKindInfo.GetScale();
		float num = 1f;
		if (!this.GetCharKindInfo().IsATB(16L))
		{
			num = 1f * (float)scale / 10f;
		}
		this.m_k3DChar.GetRootGameObject().transform.localScale = new Vector3(num, num, num);
		this.m_kAnimation.Init(this.GetCharKindInfo().GetCode(), this.m_k3DChar, this.m_eCharKindType);
		this.m_kCharMove.Init();
		this.SetSpeed(this.m_kPersonInfo.GetMoveSpeed());
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject != null)
		{
			this.m_k3DChar.SetOnGround(true);
			NrCharInfoAdaptor nrCharInfoAdaptor2 = NkUtil.GuarranteeComponent<NrCharInfoAdaptor>(rootGameObject);
			nrCharInfoAdaptor2.SetCharInfoInterface(new NrCharInfoLogic(this));
		}
		if (this.GetCharKindInfo().IsSlopeMode())
		{
			this.m_k3DChar.SetSlope(true);
		}
		this.SetFaceGradeEffect();
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			this.m_k3DChar.SetLayer(TsLayer.PC);
			break;
		case eCharKindType.CKT_SOLDIER:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_MOB.ToString());
			break;
		case eCharKindType.CKT_MONSTER:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_MOB.ToString());
			break;
		case eCharKindType.CKT_NPC:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_QUEST.ToString());
			break;
		case eCharKindType.CKT_OBJECT:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_EXTRA.ToString());
			break;
		default:
			this.m_k3DChar.SetLayer(TsLayer.NPC, TsTag.NPC_EXTRA.ToString());
			break;
		}
		this.Set3DCharStep(NkBattleChar.e3DCharStep.LOADCOMPLETED);
		this.ReadyHeadUp(scale);
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			Debug.Log(string.Concat(new object[]
			{
				"AfterLoad3dChar ID : ",
				this.GetID(),
				", CharUnique : ",
				this.GetBUID()
			}));
		}
		if (this.m_HpDlg == null && !this.IsBattleCharATB(64))
		{
			this.m_HpDlg = (Battle_HpDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.BATTLE_HP_GROUP_DLG);
			if (this.m_HpDlg != null)
			{
				this.m_HpDlg.Set(this);
			}
		}
		if (this.IsCharKindATB(128L) && Battle.BATTLE != null && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_TUTORIAL)
		{
			Battle_BossAggro_DLG battle_BossAggro_DLG = (Battle_BossAggro_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_BOSSAGGRO_DLG);
			if (battle_BossAggro_DLG != null)
			{
				battle_BossAggro_DLG.SetBossData(this);
			}
		}
		return true;
	}

	public virtual bool IsReayCreateCharInfo()
	{
		return this.m_kPersonInfo.GetKind(0) != 0;
	}

	public bool IsModelLoadCompleted()
	{
		if (this.m_k3DChar == null)
		{
			return false;
		}
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			return nr3DCharActor.IsModelLoadCompleted();
		}
		return true;
	}

	public bool IsCreated3DModel()
	{
		return this.m_e3DCharStep >= NkBattleChar.e3DCharStep.CREATED && this.m_e3DCharStep < NkBattleChar.e3DCharStep.DELETED && this.m_k3DChar != null;
	}

	public bool IsReady3DModel()
	{
		return this.m_e3DCharStep >= NkBattleChar.e3DCharStep.LOADCOMPLETED && this.m_e3DCharStep < NkBattleChar.e3DCharStep.DELETED;
	}

	public bool IsReadyCharAction()
	{
		return this.m_e3DCharStep >= NkBattleChar.e3DCharStep.FADEIN && this.m_e3DCharStep <= NkBattleChar.e3DCharStep.CHARACTION;
	}

	public bool IsShaderRecovery()
	{
		return this.m_e3DCharStep == NkBattleChar.e3DCharStep.CHARACTION;
	}

	public NkCharPartControl GetPartControl()
	{
		return this.m_kPartControl;
	}

	public void SetReadyPartInfo()
	{
		NrCharBasePart basePart = this.m_kPersonInfo.GetBasePart();
		NrEquipItemInfo equipItemInfo = this.GetSoldierInfo().GetEquipItemInfo();
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
	}

	public void ChangeCharPartInfo(NrCharPartInfo pkCustomPartInfo, bool bChangeBase, bool bChangeEquip)
	{
		this.m_kPartControl.SetPartInfo(pkCustomPartInfo);
		this.ProcessCharPartInfo(pkCustomPartInfo, bChangeBase, bChangeEquip);
		if (this.m_eCharKindType == eCharKindType.CKT_USER)
		{
			this.SetSolEquipItemFromPartInfo(pkCustomPartInfo.m_kEquipPart);
		}
	}

	private void ProcessCharPartInfo(NrCharPartInfo pkCustomPartInfo, bool bChangeBase, bool bChangeEquip)
	{
		NrPersonInfoBattle nrPersonInfoBattle = this.GetPersonInfo() as NrPersonInfoBattle;
		if (bChangeBase)
		{
			nrPersonInfoBattle.SetBasePart(pkCustomPartInfo.m_kBasePart);
		}
		if (bChangeEquip)
		{
			NkSoldierInfo soldierInfo = this.GetSoldierInfo();
			if (soldierInfo != null)
			{
				soldierInfo.SetEquipItemInfo(pkCustomPartInfo.m_kEquipPart);
			}
		}
	}

	public void ChangeWeaponTarget()
	{
		this.m_kPartControl.ChangeWeaponTarget();
	}

	public void SetSolEquipItemFromPartInfo(NrCharEquipPart pkEquipPart)
	{
		NkSoldierInfo soldierInfo = this.GetSoldierInfo();
		if (soldierInfo != null && soldierInfo.IsValid())
		{
			soldierInfo.SetEquipItemInfo(pkEquipPart);
			soldierInfo.SetReceivedEquipItem(true);
			soldierInfo.UpdateSoldierStatInfo();
		}
	}

	public bool IsHaveAnimation(eCharAnimationType anitype)
	{
		return this.m_k3DChar != null && this.m_k3DChar.IsHaveAnimation(anitype);
	}

	public void SetAnimation(eCharAnimationType anitype)
	{
		this.SetAnimation(anitype, true, true);
	}

	public void SetAnimation(eCharAnimationType anitype, bool bForceAction, bool bBlend)
	{
		bool bForceReserved = true;
		if (this.IsReadyCharAction())
		{
			bForceReserved = false;
		}
		if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK && (anitype < eCharAnimationType.Attack1 || anitype > eCharAnimationType.AttackRight1))
		{
			return;
		}
		if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_SKILL && (anitype < eCharAnimationType.Attack1 || anitype > eCharAnimationType.Skill3))
		{
			return;
		}
		if (this.m_kAnimation.GetCurrentAniType() == eCharAnimationType.Die1)
		{
			return;
		}
		this.m_kAnimation.PushNextAniType(anitype, bForceAction, bForceReserved, bBlend);
	}

	public void SetAnimationLoadAfter(eCharAnimationType anitype)
	{
		this.LoadAfterAnimation = anitype;
	}

	public void RestoreSlowMotion()
	{
	}

	public eCharAnimationType GetAnimation()
	{
		return this.m_kAnimation.GetCurrentAniType();
	}

	public void SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType anitype)
	{
		if (!this.IsReadyCharAction())
		{
			return;
		}
		this.m_kAnimation.SetFacialAnimation(anitype);
	}

	public void SetLookAt(Vector3 v3Direction, bool bInterpolation)
	{
		this.SetLookAt(v3Direction.x, v3Direction.y, v3Direction.z, bInterpolation);
	}

	public void SetLookAt(float x, float y, float z, bool bInterpolation)
	{
		if (this.IsReady3DModel())
		{
			this.m_k3DChar.SetLookAt(x, y, z, bInterpolation);
		}
		else
		{
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.x = x;
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.y = y;
			this.m_kPersonInfo.GetBasicInfo().m_vDirection.z = z;
		}
	}

	public void SetRotate(float fDegree)
	{
		if (!this.IsReady3DModel())
		{
			return;
		}
		if (fDegree < 0f || fDegree >= 360f)
		{
			return;
		}
		Quaternion localRotation = Quaternion.AngleAxis(fDegree, Vector3.up);
		this.m_k3DChar.GetRootGameObject().transform.localRotation = localRotation;
	}

	public void PickingMove()
	{
		NrTSingleton<NrAutoPath>.Instance.ResetData();
		NkRaycast.Raycast();
		RaycastHit hIT = NkRaycast.HIT;
		if (NrTSingleton<NkClientLogic>.Instance.GetPickChar() == null)
		{
			this.m_kCharMove.ForceStopChar(false, -1f, -1f);
		}
		this.m_kCharMove.MoveTo(hIT.point.x, hIT.point.y, hIT.point.z);
	}

	public void ClearReservedMoveTarget()
	{
		this.m_vReservedMoveTarget = Vector3.zero;
	}

	public void MoveTo(float x, float y, float z, bool bStraightMove)
	{
		if (this.m_k3DChar != null)
		{
			if (!this.IsReadyCharAction())
			{
				this.m_vReservedMoveTarget.x = x;
				this.m_vReservedMoveTarget.y = y;
				this.m_vReservedMoveTarget.z = z;
			}
			else if (!bStraightMove)
			{
				this.m_kCharMove.MoveTo(x, y, z);
				if (this.m_vReservedMoveTarget.x > 0f || this.m_vReservedMoveTarget.z > 0f)
				{
					this.m_vReservedMoveTarget = Vector3.zero;
				}
			}
			else
			{
				this.m_kCharMove.StraightMoveTo(x, y, z);
			}
		}
		else
		{
			this.m_vReservedMoveTarget.x = x;
			this.m_vReservedMoveTarget.y = y;
			this.m_vReservedMoveTarget.z = z;
		}
	}

	public void MoveTo(Vector3 vPos)
	{
		this.MoveTo(vPos.x, vPos.y, vPos.z, false);
	}

	public void MoveTo(POS3D ToPos)
	{
		this.MoveTo(ToPos.x, ToPos.y, ToPos.z, false);
	}

	public void MoveToFast(POS3D ToPos, POS3D NextDestPos)
	{
		this.m_kCharMove.MoveToFast(ToPos.x, ToPos.y, ToPos.z, NextDestPos.x, NextDestPos.y, NextDestPos.z);
	}

	public void SetIncreaseSpeed(float fSpeed)
	{
		this.m_fIncreaseCharSpeed = fSpeed;
		this.SetSpeed(this.m_fCharSpeed);
	}

	public void SetSpeed(float fSpeed)
	{
		this.m_fCharSpeed = fSpeed;
		fSpeed += this.m_fIncreaseCharSpeed;
		if (this.m_k3DChar != null)
		{
			this.m_k3DChar.SetSpeed(fSpeed);
		}
	}

	public float GetSpeed()
	{
		return this.m_fCharSpeed;
	}

	public void DeleteChar()
	{
		this.ShowMovePath(false);
		this.Release();
		this.Init();
	}

	public virtual void funcReachedToDest()
	{
		this.m_kAnimation.SetFinishAnimation(true);
	}

	public void StopFacialAnimation()
	{
		this.m_kAnimation.SetFacialAnimation(NrCharDefine.eCharFaicalAnimationType.FStay1);
	}

	public bool IsCharKindATB(long kindatb)
	{
		return this.m_pkCharKindInfo != null && this.m_pkCharKindInfo.IsATB(kindatb);
	}

	public void SetDead()
	{
		this.m_bDeadReaservation = true;
		this.SetAnimation(eCharAnimationType.Die1);
		if (this.m_HpDlg != null)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(this.m_HpDlg.WindowID);
			this.m_HpDlg = null;
		}
		if (this.IsCharKindATB(128L))
		{
			Battle_BossAggro_DLG battle_BossAggro_DLG = (Battle_BossAggro_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG);
			if (battle_BossAggro_DLG != null)
			{
				battle_BossAggro_DLG.Close();
			}
		}
		this.ClealBuffBattleSkillBuffEffect();
	}

	public void SetDeleteChar(int nReason)
	{
		this.ProcessDeadItem();
		NrTSingleton<NkQuestManager>.Instance.UpdateMonQuestMessage((long)this.m_pkCharKindInfo.GetCharKind());
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.m_eAlly, this.m_nStartPosIndex);
		if (battleGrid != null)
		{
			battleGrid.RemoveBUID(this.GetBUID());
		}
		NrGridManager gRID_MANAGER = Battle.BATTLE.GRID_MANAGER;
		if (gRID_MANAGER != null)
		{
			gRID_MANAGER.RemoveBUID(this.m_eAlly, this.m_nStartPosIndex, this.GetBUID());
		}
		this.Set3DCharStep(NkBattleChar.e3DCharStep.DIED);
		this.m_fDeadTime = Time.time;
		if (nReason == 2)
		{
			this.m_fDeadTime = Time.time - 7f - 10f;
			this.Update();
		}
		else if (nReason == 1)
		{
			this.m_fDeadTime = Time.time - 7f - 1f;
			this.Update();
		}
		this.m_kCharMove.ForceStopChar(false, -1f, -1f);
	}

	public void CheckBattleSkillDetail(BATTLESKILL_BASE BSkillBase, BATTLESKILL_DETAIL BSkillDetail)
	{
		if (BSkillBase == null || BSkillDetail == null)
		{
			return;
		}
		eBUFF_TYPE eBUFF_TYPE = eBUFF_TYPE.BUFFTYPE_EMPTY;
		bool flag = false;
		bool flag2 = false;
		if (BSkillDetail.GetSkillDetalParamValue(75) != 0 || BSkillDetail.GetSkillDetalParamValue(98) != 0)
		{
			flag = true;
		}
		else
		{
			eBUFF_TYPE = (eBUFF_TYPE)BSkillDetail.GetSkillDetalParamValue(76);
		}
		if (flag || eBUFF_TYPE != eBUFF_TYPE.BUFFTYPE_EMPTY)
		{
			for (int i = 0; i < 12; i++)
			{
				if (this.m_BattleSkillBufData[i].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[i].BSkillBufLevel > 0)
				{
					BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_BattleSkillBufData[i].BSkillBufSkillUnique);
					BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(this.m_BattleSkillBufData[i].BSkillBufSkillUnique, this.m_BattleSkillBufData[i].BSkillBufLevel);
					if (battleSkillBase != null && battleSkillDetail != null)
					{
						if (flag)
						{
							this.DelltBuffBattleSkillEffect(this.m_BattleSkillBufData[i].BSkillBufEffectCode, 0);
							this.m_BattleSkillBufData[i].init();
							flag2 = true;
						}
						else if (battleSkillBase.m_nSkillBuffType == (int)eBUFF_TYPE)
						{
							this.DelltBuffBattleSkillEffect(this.m_BattleSkillBufData[i].BSkillBufEffectCode, 0);
							this.m_BattleSkillBufData[i].init();
							flag2 = true;
						}
					}
				}
			}
		}
		if (flag2 && this.m_HpDlg != null)
		{
			this.m_HpDlg.ClealBuffIcon();
			if (this.IsBattleCharATB(1024) || this.IsBattleCharATB(16384))
			{
				this.m_HpDlg.SetImmuneBuffIcon(true, 1024);
			}
			else if (this.IsBattleCharATB(32768))
			{
				this.m_HpDlg.SetImmuneBuffIcon(true, 32768);
			}
			else
			{
				this.m_HpDlg.SetImmuneBuffIcon(false, 0);
			}
			this.m_HpDlg.SetBuffIcon(this.m_BattleSkillBufData);
			for (int j = 0; j < 12; j++)
			{
				if (this.m_BattleSkillBufData[j].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[j].BSkillBufEffectCode == 0u && this.GetPersonInfo().GetSoldierInfo(0).GetHP() > 0 && !this.m_bDeadReaservation)
				{
					this.m_BattleSkillBufData[j].BSkillBufEffectCode = this.SetBuffBattleSkillEffect(this.m_BattleSkillBufData[j]);
				}
			}
		}
	}

	public void SetBattleSkillBuf(int skillUnique, int KeepTurn, int skillLevel, int AddUseAngerlypoint)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillUnique, skillLevel);
		if (battleSkillBase == null || battleSkillDetail == null)
		{
			return;
		}
		bool bSkillDeBuff = true;
		bool flag = true;
		if (battleSkillBase.m_nSkillTargetType == 1 || battleSkillBase.m_nSkillTargetType == 2)
		{
			bSkillDeBuff = false;
		}
		if (KeepTurn == -1)
		{
			return;
		}
		if (KeepTurn == 0 || KeepTurn == -100)
		{
			for (int i = 0; i < 12; i++)
			{
				if (this.m_BattleSkillBufData[i].BSkillBufSkillUnique == skillUnique)
				{
					if (KeepTurn == -100)
					{
						this.DelltBuffBattleSkillEffect(this.m_BattleSkillBufData[i].BSkillBufEffectCode, 0);
						this.m_BattleSkillBufData[i].init();
					}
					else
					{
						this.m_BattleSkillBufData[i].BSkillBufLastKeepTurn = 0;
					}
					break;
				}
			}
		}
		else
		{
			flag = false;
			for (int j = 0; j < 12; j++)
			{
				if (this.m_BattleSkillBufData[j].BSkillBufSkillUnique == skillUnique && this.m_BattleSkillBufData[j].BSkillBufLevel <= skillLevel)
				{
					this.m_BattleSkillBufData[j].BSkillBufSkillUnique = skillUnique;
					this.m_BattleSkillBufData[j].BSkillBufLevel = skillLevel;
					this.m_BattleSkillBufData[j].BSkillBufActionTurn = Battle.BATTLE.m_nTurnCount;
					this.m_BattleSkillBufData[j].BSkillBufLastKeepTurn = KeepTurn;
					this.m_BattleSkillBufData[j].BSkillDeBuff = bSkillDeBuff;
					this.m_BattleSkillBufData[j].BSkillBufAddUseAngerlyPoint = AddUseAngerlypoint;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int k = 0; k < 12; k++)
				{
					if (this.m_BattleSkillBufData[k].BSkillBufSkillUnique == 0)
					{
						this.m_BattleSkillBufData[k].BSkillBufSkillUnique = skillUnique;
						this.m_BattleSkillBufData[k].BSkillBufLevel = skillLevel;
						this.m_BattleSkillBufData[k].BSkillBufActionTurn = Battle.BATTLE.m_nTurnCount;
						this.m_BattleSkillBufData[k].BSkillBufLastKeepTurn = KeepTurn;
						this.m_BattleSkillBufData[k].BSkillDeBuff = bSkillDeBuff;
						this.m_BattleSkillBufData[k].BSkillBufAddUseAngerlyPoint = AddUseAngerlypoint;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					int num = 9999;
					int num2 = 0;
					for (int l = 0; l < 12; l++)
					{
						if (this.m_BattleSkillBufData[l].BSkillBufActionTurn < num)
						{
							num = this.m_BattleSkillBufData[l].BSkillBufActionTurn;
							num2 = l;
						}
					}
					this.m_BattleSkillBufData[num2].BSkillBufSkillUnique = skillUnique;
					this.m_BattleSkillBufData[num2].BSkillBufLevel = skillLevel;
					this.m_BattleSkillBufData[num2].BSkillBufActionTurn = Battle.BATTLE.m_nTurnCount;
					this.m_BattleSkillBufData[num2].BSkillBufLastKeepTurn = KeepTurn;
					this.m_BattleSkillBufData[num2].BSkillDeBuff = bSkillDeBuff;
					this.m_BattleSkillBufData[num2].BSkillBufEffectCode = 0u;
					this.m_BattleSkillBufData[num2].BSkillBufAddUseAngerlyPoint = AddUseAngerlypoint;
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (this.m_HpDlg != null)
			{
				this.m_HpDlg.ClealBuffIcon();
				if (this.IsBattleCharATB(1024) || this.IsBattleCharATB(16384))
				{
					this.m_HpDlg.SetImmuneBuffIcon(true, 1024);
				}
				else if (this.IsBattleCharATB(32768))
				{
					this.m_HpDlg.SetImmuneBuffIcon(true, 32768);
				}
				this.m_HpDlg.SetBuffIcon(this.m_BattleSkillBufData);
				for (int m = 0; m < 12; m++)
				{
					if (this.m_BattleSkillBufData[m].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[m].BSkillBufEffectCode == 0u && this.GetPersonInfo().GetSoldierInfo(0).GetHP() > 0 && !this.m_bDeadReaservation)
					{
						this.m_BattleSkillBufData[m].BSkillBufEffectCode = this.SetBuffBattleSkillEffect(this.m_BattleSkillBufData[m]);
					}
				}
			}
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg != null)
			{
				battle_Control_Dlg.SetAngryText();
			}
		}
	}

	public void ClealBuffBattleSkillBuffEffect()
	{
		for (int i = 0; i < 12; i++)
		{
			if (this.m_BattleSkillBufData[i].BSkillBufEffectCode > 0u)
			{
				this.DelltBuffBattleSkillEffect(this.m_BattleSkillBufData[i].BSkillBufEffectCode, 0);
			}
		}
	}

	public void DelltBuffBattleSkillEffect(uint effectNum, int BSkillBufSkillUnique)
	{
		if (BSkillBufSkillUnique > 0)
		{
			this.SetBuffBattleSkillEndEffect(BSkillBufSkillUnique);
		}
		if (effectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(effectNum);
		}
	}

	public uint SetBuffBattleSkillEffect(BATTLESKILL_BUF BattleSkillBufData)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillBufData.BSkillBufSkillUnique);
		string bSkillBuffEffectCode = battleSkillBase.GetBSkillBuffEffectCode();
		if (!string.IsNullOrEmpty(bSkillBuffEffectCode))
		{
			uint num = NrTSingleton<NkEffectManager>.Instance.AddEffect(bSkillBuffEffectCode, this);
			if (num != 0u || NrTSingleton<NkEffectManager>.Instance.isEffectLimit(bSkillBuffEffectCode))
			{
				return num;
			}
			Debug.LogError("Err :SetHitBattleSkill No SkillEffect:" + bSkillBuffEffectCode);
		}
		return 0u;
	}

	public uint SetBuffBattleSkillEndEffect(int BSkillBufSkillUnique)
	{
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BSkillBufSkillUnique);
		string bSkillBuffEndEffectCode = battleSkillBase.GetBSkillBuffEndEffectCode();
		if (!string.IsNullOrEmpty(bSkillBuffEndEffectCode))
		{
			uint num = NrTSingleton<NkEffectManager>.Instance.AddEffect(bSkillBuffEndEffectCode, this);
			if (num != 0u)
			{
				return num;
			}
			Debug.LogError("Err :SetBuffBattleSkillEndEffect No SkillEffect:" + bSkillBuffEndEffectCode);
		}
		return 0u;
	}

	public void SetHitBattleSkill(BATTLESKILL_BASE BattleSkillBase, BATTLESKILL_DETAIL BSkillDetail, int HitEffectGridPos, bool bEndureDamage)
	{
		string text;
		if (!bEndureDamage)
		{
			text = BattleSkillBase.GetBSkillTargetEffectCode();
		}
		else
		{
			text = BattleSkillBase.GetBSkillTargetEndureEffectCode();
		}
		if (!string.IsNullOrEmpty(text))
		{
			BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.m_eAlly, this.m_nStartPosIndex);
			if (battleGrid != null)
			{
				if (BSkillDetail.GetSkillDetalParamValue(80) > 0 || BSkillDetail.GetSkillDetalParamValue(108) > 0 || BSkillDetail.GetSkillDetalParamValue(79) > 0)
				{
					if (HitEffectGridPos > -1 && battleGrid.m_nWidthCount * battleGrid.m_nHeightCount >= HitEffectGridPos)
					{
						Vector3 v3Target = battleGrid.mListPos[HitEffectGridPos];
						if (NrTSingleton<NkEffectManager>.Instance.AddEffect(text, v3Target) == 0u && !NrTSingleton<NkEffectManager>.Instance.isEffectLimit(text))
						{
							Debug.LogError("Err :SetHitBattleSkill No SkillEffect:" + text);
						}
					}
				}
				else if (NrTSingleton<NkEffectManager>.Instance.AddEffect(text, this) == 0u && !NrTSingleton<NkEffectManager>.Instance.isEffectLimit(text))
				{
					Debug.LogError("Err :SetHitBattleSkill No SkillEffect:" + text);
				}
			}
		}
	}

	public void SetHitCenterGridEffectBattleSkill(BATTLESKILL_BASE BattleSkillBase)
	{
		string bSkillHitCenterGridEffectCode = BattleSkillBase.GetBSkillHitCenterGridEffectCode();
		bool flag = false;
		if (!string.IsNullOrEmpty(bSkillHitCenterGridEffectCode))
		{
			BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.m_eAlly, this.m_nStartPosIndex);
			if (battleGrid != null)
			{
				Vector3 center = battleGrid.GetCenter();
				if (NrTSingleton<NkEffectManager>.Instance.AddCenterPosEffect(bSkillHitCenterGridEffectCode, this, center) == 0u)
				{
					flag = true;
				}
			}
			if (flag && !NrTSingleton<NkEffectManager>.Instance.isEffectLimit(bSkillHitCenterGridEffectCode))
			{
				Debug.LogError("Err :SetHitCenterGridEffectBattleSkill No HitCenterGridEffect:" + bSkillHitCenterGridEffectCode);
			}
		}
	}

	public void SetGridEffectBattleSkill(BATTLESKILL_BASE BattleSkillBase)
	{
		string bSkillGridEffectCode = BattleSkillBase.GetBSkillGridEffectCode();
		bool flag = false;
		if (!string.IsNullOrEmpty(bSkillGridEffectCode))
		{
			if (NrTSingleton<NkEffectManager>.Instance.AddCasterEffect(bSkillGridEffectCode, this) == 0u)
			{
				flag = true;
			}
			if (flag && !NrTSingleton<NkEffectManager>.Instance.isEffectLimit(bSkillGridEffectCode))
			{
				Debug.LogError("Err :GetBSkillGridEffectCode No SkillEffect:" + bSkillGridEffectCode);
			}
		}
	}

	public void SetAcquiredItem(GS_BF_CHARINFO_NFY nfy)
	{
		int num = nfy.iPara[2];
		int num2 = nfy.iPara[3];
		int extranum = nfy.iPara[4];
		short createtype = (short)nfy.iPara[5];
		int itemRank = nfy.iPara[6];
		if (0 < num && 0 < num2)
		{
			ACQUIRED_ITEM aCQUIRED_ITEM = new ACQUIRED_ITEM();
			aCQUIRED_ITEM.Set(num, num2, extranum, createtype, itemRank);
			aCQUIRED_ITEM.CharUnique = (short)nfy.iPara[0];
			this.m_qAcquiredItem.Enqueue(aCQUIRED_ITEM);
		}
	}

	private void ProcessDeadItem()
	{
		if (this.m_qAcquiredItem.Count != 0)
		{
			int count = this.m_qAcquiredItem.Count;
			for (int i = 0; i < count; i++)
			{
				ACQUIRED_ITEM aCQUIRED_ITEM = this.m_qAcquiredItem.Dequeue();
				NkQuestManager instance = NrTSingleton<NkQuestManager>.Instance;
				instance.UpdateItemQuestMessage((long)aCQUIRED_ITEM.ItemUnique);
				GetItemDlg getItemDlg = (GetItemDlg)NrTSingleton<FormsManager>.Instance.LoadGroupForm(G_ID.GET_ITEM_DLG);
				if (getItemDlg != null)
				{
					bool flag = false;
					if (aCQUIRED_ITEM.CharUnique > -1)
					{
						NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(aCQUIRED_ITEM.CharUnique);
						if (charByBUID.MyChar || charByBUID.IsFriend)
						{
							getItemDlg.SetItem(aCQUIRED_ITEM.ItemUnique, aCQUIRED_ITEM.ItemNum, aCQUIRED_ITEM.nItemRank);
							getItemDlg.SetIndex(Battle.BATTLE.ListGetItemDlg.Count);
							flag = true;
						}
					}
					else
					{
						getItemDlg.SetItem(aCQUIRED_ITEM.ItemUnique, aCQUIRED_ITEM.ItemNum, aCQUIRED_ITEM.nItemRank);
						getItemDlg.SetIndex(Battle.BATTLE.ListGetItemDlg.Count);
						flag = true;
					}
					if (flag)
					{
						Battle.BATTLE.ListGetItemDlg.Add(getItemDlg);
					}
					else
					{
						getItemDlg.Close();
					}
				}
			}
		}
	}

	public string GetCharName()
	{
		return this.m_kPersonInfo.GetCharName();
	}

	public string Get3DName()
	{
		if (this.m_k3DChar == null)
		{
			return null;
		}
		if (this.m_k3DChar.GetRootGameObject() == null)
		{
			return null;
		}
		return this.m_k3DChar.GetRootGameObject().name;
	}

	public GameObject GetCharObject()
	{
		if (this.m_k3DChar == null)
		{
			return null;
		}
		return this.m_k3DChar.GetRootGameObject();
	}

	public Vector3 GetCenterPosition()
	{
		if (this.m_k3DChar == null)
		{
			return Vector3.zero;
		}
		CharacterController charController = this.m_k3DChar.GetCharController();
		if (charController == null)
		{
			return Vector3.zero;
		}
		return this.GetCharObject().transform.position + charController.center;
	}

	public Vector3 GetLocalCenterPosition()
	{
		if (this.m_k3DChar == null)
		{
			return Vector3.zero;
		}
		CharacterController charController = this.m_k3DChar.GetCharController();
		if (charController == null)
		{
			return Vector3.zero;
		}
		return charController.center;
	}

	public Vector3 GetCameraPosition()
	{
		if (this.m_k3DChar == null)
		{
			return Vector3.zero;
		}
		CharacterController charController = this.m_k3DChar.GetCharController();
		if (charController == null)
		{
			return Vector3.zero;
		}
		Vector3 center = charController.center;
		center.y += charController.height / 4f;
		return this.GetCharObject().transform.position + center;
	}

	public Vector3 GetShotPosition()
	{
		if (this.m_k3DChar != null)
		{
			Transform shotDummy = this.m_k3DChar.GetShotDummy();
			if (shotDummy != null)
			{
				return shotDummy.position;
			}
		}
		return this.GetCenterPosition();
	}

	public void SitDown(bool bNow, POS3D LookAt)
	{
		if (this.m_k3DChar != null)
		{
			this.m_k3DChar.SitDown(bNow, LookAt);
		}
	}

	public virtual void MouseEvent_Exit()
	{
		if (this.m_k3DChar == null)
		{
			return;
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			this.SetShowHeadUp(false, false, false);
			break;
		case eCharKindType.CKT_SOLDIER:
			this.SetShowHeadUp(false, false, false);
			break;
		case eCharKindType.CKT_MONSTER:
			this.SetShowHeadUp(false, false, false);
			break;
		case eCharKindType.CKT_NPC:
			if (!this.m_bProcessMouseEvent)
			{
				return;
			}
			break;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(64L))
			{
				this.SetShowHeadUp(false, false, false);
			}
			break;
		}
		if (this.m_bProcessMouseEvent)
		{
			TBSUTIL.SetShaderPropertyColor(this.m_k3DChar.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_NORMAL);
		}
	}

	public virtual void MouseEvent_Enter()
	{
		if (this.m_k3DChar == null)
		{
			return;
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			this.SetShowHeadUp(true, false, false);
			break;
		case eCharKindType.CKT_SOLDIER:
			this.SetShowHeadUp(true, false, false);
			break;
		case eCharKindType.CKT_MONSTER:
			this.SetShowHeadUp(true, false, false);
			break;
		case eCharKindType.CKT_NPC:
			if (!this.m_bProcessMouseEvent)
			{
				return;
			}
			break;
		case eCharKindType.CKT_OBJECT:
			if (this.IsCharKindATB(64L))
			{
				this.SetShowHeadUp(true, false, false);
			}
			break;
		}
		if (this.m_bProcessMouseEvent)
		{
			if (Battle.BATTLE.MyAlly == this.Ally)
			{
				TBSUTIL.SetShaderPropertyColor(this.m_k3DChar.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_GREEN);
			}
			else
			{
				TBSUTIL.SetShaderPropertyColor(this.m_k3DChar.GetRootGameObject(), PMSHADER.SELECT_COLOR, CHAR_SHADER_COLOR.SELECT_COLOR_RED);
			}
		}
	}

	public virtual void MouseEvent_Over()
	{
		if (!this.m_bProcessMouseEvent)
		{
			return;
		}
		bool flag = false;
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		case eCharKindType.CKT_MONSTER:
			flag = NkInputManager.IsLeftButtonUP();
			break;
		case eCharKindType.CKT_NPC:
			flag = NkInputManager.IsLeftButtonUP();
			break;
		case eCharKindType.CKT_OBJECT:
			if (!this.IsCharKindATB(64L))
			{
				return;
			}
			break;
		}
		if (flag)
		{
			NrTSingleton<NkClientLogic>.Instance.SetPickChar(this);
		}
		if (NkInputManager.IsRightButtonUP())
		{
			this.OnMouseRightClickEvent();
		}
	}

	private void OnMouseLeftClickEvent()
	{
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		}
	}

	private void OnMouseRightClickEvent()
	{
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			if (this.GetID() == 1)
			{
				return;
			}
			break;
		}
	}

	public void Update_MouseEvent()
	{
		if (!this.m_bProcessMouseEvent)
		{
			return;
		}
		if (!this.m_bClickedMe)
		{
			return;
		}
		switch (this.m_eCharKindType)
		{
		case eCharKindType.CKT_USER:
			return;
		case eCharKindType.CKT_SOLDIER:
			return;
		}
	}

	public void RefreshFindPath()
	{
		this.m_kCharMove.RefreshFindPath();
	}

	public void ForceStopChar(bool bSetAni, float x, float z)
	{
		this.m_kCharMove.ForceStopChar(bSetAni, x, z);
	}

	public void SetSelect(bool bSelect)
	{
		if (!this.MyChar)
		{
			return;
		}
		if (bSelect)
		{
		}
	}

	public void SetCharPos(Vector3 charPos)
	{
		this.GetPersonInfo().SetCharPos(charPos);
	}

	public Vector3 GetCharPos()
	{
		return this.GetPersonInfo().GetCharPos();
	}

	public void SetDamage(int iDamage, bool bCritical, NkBattleChar pkFromChar, int TargetType, int nAddAngerlyPoint, int nSkillInfoNum)
	{
		if ((iDamage == 0 && TargetType == 3) || (iDamage == 0 && TargetType == 4) || (iDamage == 0 && TargetType == 0))
		{
			if (pkFromChar != null && pkFromChar.GetBUID() == this.GetBUID())
			{
				return;
			}
			if (pkFromChar != null && pkFromChar.Ally == this.Ally)
			{
				return;
			}
			if (nAddAngerlyPoint != 0)
			{
				return;
			}
			Battle.BATTLE.PushBattleDamage(this, iDamage, false, 0, nSkillInfoNum);
			this.SetAnimation(eCharAnimationType.Evade1);
		}
		else if (iDamage != 0)
		{
			if (pkFromChar != null)
			{
				this.m_nAttackCharWeaponType = pkFromChar.GetWeaponType();
			}
			else
			{
				this.m_nAttackCharWeaponType = 0;
			}
			this.GetSoldierInfo().AddHP(iDamage, this.AddHP);
			if (this.m_HpDlg != null)
			{
				this.m_HpDlg.UpdateHP();
				if (Battle.BATTLE.ColosseumObserver)
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg != null)
					{
						colosseumObserverControlDlg.UpdateHP(this.Ally, this.GetBUID(), (float)this.GetSoldierInfo().GetHP(), iDamage, bCritical);
					}
				}
			}
			Battle_BossAggro_DLG battle_BossAggro_DLG = (Battle_BossAggro_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_BOSSAGGRO_DLG);
			if (battle_BossAggro_DLG != null)
			{
				battle_BossAggro_DLG.UpdateBossHP();
			}
			if (pkFromChar != null && iDamage < 0)
			{
				if (!bCritical)
				{
					this.SetAnimation(eCharAnimationType.Damage1);
				}
				else
				{
					this.SetAnimation(eCharAnimationType.CriDamage1);
				}
			}
			Battle.BATTLE.PushBattleDamage(this, iDamage, bCritical, 0, nSkillInfoNum);
			if (TargetType == 0)
			{
				string effectKind = "HIT";
				if (pkFromChar != null)
				{
					effectKind = pkFromChar.GetCharKindInfo().GetHitEffectCode();
				}
				if (!this.IsCharKindATB(4194304L) && pkFromChar != null)
				{
					this.SetLookAt(pkFromChar.GetCharPos(), false);
				}
				NrTSingleton<NkEffectManager>.Instance.AddEffect(effectKind, this);
			}
		}
	}

	public void UpdateHPDlg()
	{
		if (this.m_HpDlg != null)
		{
			this.m_HpDlg.UpdateHP();
		}
	}

	public void UpdateHpDlgForRestart(int nHP)
	{
		if (this.m_HpDlg != null)
		{
			this.m_HpDlg.UpdateHpFromRestart(nHP);
		}
		this.GetSoldierInfo().SetHP(nHP, this.AddHP);
	}

	public void AddAstarPath(float x, float y, float z, eBATTLE_MOVE_STATUS eSTATUS)
	{
		this.m_kCharMove.AddAstarPath(x, y, z, eSTATUS);
	}

	public void MoveServerAStar()
	{
		if (this.m_kCharMove == null)
		{
			return;
		}
		this.m_kCharMove.MoveServerAStar();
	}

	public void MakeArrow()
	{
		if (this.m_kCharMove == null)
		{
			return;
		}
		this.m_kCharMove.MakeMoveArrow();
	}

	public NkSoldierInfo GetSoldierInfo()
	{
		if (!this.MyChar)
		{
			return this.GetPersonInfo().GetSoldierInfo(0);
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NkSoldierInfo nkSoldierInfo = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(this.GetSolID());
		if (nkSoldierInfo == null)
		{
			nkSoldierInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList().GetSolInfo(this.GetSolID());
		}
		if (nkSoldierInfo == null)
		{
			return this.GetPersonInfo().GetSoldierInfo(0);
		}
		return nkSoldierInfo;
	}

	public void OnChangeATB()
	{
	}

	public bool GetGridPosCenter(ref Vector3 veCenter)
	{
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.Ally, this.GetStartPosIndex());
		return battleGrid != null && battleGrid.GetCenter(this.GetBUID(), ref veCenter);
	}

	public Vector3 GetCenterBack()
	{
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.Ally, this.GetStartPosIndex());
		if (battleGrid == null)
		{
			return this.GetCharPos();
		}
		return battleGrid.GetCenterBack();
	}

	public int[] GetGridIndexFromCharSize(short nGridPos)
	{
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.Ally, this.GetStartPosIndex());
		if (battleGrid == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		list.Clear();
		int battleSizeX = (int)this.GetCharKindInfo().GetBattleSizeX();
		int battleSizeY = (int)this.GetCharKindInfo().GetBattleSizeY();
		int num = (int)nGridPos % battleGrid.m_nWidthCount;
		int num2 = (int)nGridPos / battleGrid.m_nWidthCount;
		for (int i = 0; i < battleSizeY; i++)
		{
			for (int j = 0; j < battleSizeX; j++)
			{
				int num3 = num + j + (num2 + i) * battleGrid.m_nWidthCount;
				if (num3 / battleGrid.m_nWidthCount == num2 + i)
				{
					if (num3 >= 0 && num3 < battleGrid.m_nHeightCount * battleGrid.m_nWidthCount)
					{
						list.Add(num3);
					}
				}
			}
		}
		return list.ToArray();
	}

	public eCharAnimationType GetStayAni()
	{
		if (this.GetSoldierInfo() == null)
		{
			return eCharAnimationType.BStay1;
		}
		float num = (float)this.GetSoldierInfo().GetHP() / (float)this.GetMaxHP(false);
		if (num <= 0.3f && this.IsHaveAnimation(eCharAnimationType.Tired1))
		{
			return eCharAnimationType.Tired1;
		}
		return eCharAnimationType.BStay1;
	}

	public int GetAttackCharWeaponType()
	{
		if (this.m_nAttackCharWeaponType != 0)
		{
			return this.m_nAttackCharWeaponType;
		}
		TsLog.Log("GetAttackTarget == null " + this.GetCharName(), new object[0]);
		return 1;
	}

	public void SetFakeShadowEnable(bool bEnable)
	{
		if (!this.IsReadyCharAction())
		{
			return;
		}
		if (this.GetCharKindType() == eCharKindType.CKT_OBJECT)
		{
			return;
		}
		if (bEnable && this.GetCharKindInfo().IsATB(1048576L))
		{
			return;
		}
		if (this.m_nFakeShadowEffectNum > 0u)
		{
			NrTSingleton<NkEffectManager>.Instance.SetActiveEffect(this.m_nFakeShadowEffectNum, bEnable);
		}
	}

	public void CheckBattleSkillBuffeffect()
	{
		if (this.m_HpDlg == null)
		{
			return;
		}
		this.m_HpDlg.ClealBuffIcon();
		if (this.IsBattleCharATB(1024) || this.IsBattleCharATB(16384))
		{
			this.m_HpDlg.SetImmuneBuffIcon(true, 1024);
		}
		else if (this.IsBattleCharATB(32768))
		{
			this.m_HpDlg.SetImmuneBuffIcon(true, 32768);
		}
		else
		{
			this.m_HpDlg.SetImmuneBuffIcon(false, 0);
		}
		for (int i = 0; i < 12; i++)
		{
			if (this.m_BattleSkillBufData[i].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[i].BSkillBufLastKeepTurn <= 0)
			{
				this.DelltBuffBattleSkillEffect(this.m_BattleSkillBufData[i].BSkillBufEffectCode, this.m_BattleSkillBufData[i].BSkillBufSkillUnique);
				this.m_BattleSkillBufData[i].init();
				for (int j = 0; j < 12; j++)
				{
					if (this.m_BattleSkillBufData[j].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[j].BSkillBufEffectCode == 0u && this.GetPersonInfo().GetSoldierInfo(0).GetHP() > 0 && !this.m_bDeadReaservation)
					{
						this.m_BattleSkillBufData[j].BSkillBufEffectCode = this.SetBuffBattleSkillEffect(this.m_BattleSkillBufData[j]);
					}
				}
			}
		}
		this.m_HpDlg.SetBuffIcon(this.m_BattleSkillBufData);
	}

	private void SetFaceGradeEffect()
	{
		this.RemoveFaceGradeEffect();
		if (this.m_pkCharKindInfo != null)
		{
			NkSoldierInfo soldierInfo = this.GetSoldierInfo();
			if (soldierInfo == null || !soldierInfo.IsValid())
			{
				return;
			}
			int grade = (int)soldierInfo.GetGrade();
			if (grade >= 5)
			{
				string charEffectGrade = this.m_pkCharKindInfo.GetCharEffectGrade(grade);
				if (!charEffectGrade.Equals("0"))
				{
					this.m_nFaceSolGradeEffectNum = NrTSingleton<NkEffectManager>.Instance.AddCasterEffect(charEffectGrade, this);
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

	public int GetBattleSkillBuffData(int BuffSkillType)
	{
		if (BuffSkillType <= 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			if (this.m_BattleSkillBufData[i].BSkillBufSkillUnique > 0 && this.m_BattleSkillBufData[i].BSkillBufLevel > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_BattleSkillBufData[i].BSkillBufSkillUnique);
				BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(this.m_BattleSkillBufData[i].BSkillBufSkillUnique, this.m_BattleSkillBufData[i].BSkillBufLevel);
				if (battleSkillBase != null && battleSkillDetail != null)
				{
					int skillDetalParamValue = battleSkillDetail.GetSkillDetalParamValue(BuffSkillType);
					if (skillDetalParamValue > 0)
					{
						num += skillDetalParamValue;
					}
				}
			}
		}
		return num;
	}

	public void HeadUp_Init()
	{
		if (this.m_kHeadUpEntity == null)
		{
			this.m_kHeadUpEntity = new NkHeadUpEntity();
		}
	}

	public void ReadyHeadUp(int basescale)
	{
		this.HeadUp_Init();
		this.m_kHeadUpEntity.SetLinkHeadUpRoot(this.m_k3DChar.GetBaseObject(), true, basescale);
	}

	public void SetShowHeadUp(bool bShow, bool bForce, bool bNameCheck)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION)
		{
			return;
		}
		if (this.IsBattleCharATB(64))
		{
			this.m_kHeadUpEntity.SetShowHeadUp(false);
			return;
		}
		if (bForce || this.m_kHeadUpEntity.IsShowHeadUp() != bShow)
		{
			if (bShow && bNameCheck)
			{
				bShow = this.m_kHeadUpEntity.IsCheckShowHeadUp(this.m_eCharKindType);
			}
			this.m_kHeadUpEntity.SetShowHeadUp(bShow);
			if (bShow)
			{
				this.SetBillboardScale();
			}
		}
	}

	public void SetBillboardScale()
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION)
		{
			return;
		}
		this.m_kHeadUpEntity.SetTargetScale();
	}

	public void SyncBillboardRotate(bool bScaleUpdate)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION)
		{
			return;
		}
		this.m_kHeadUpEntity.SyncBillboardRotate(bScaleUpdate);
	}

	public void MakeCharName(bool bShowCharUnique)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsBattleScene())
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION)
		{
			return;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		string text = this.GetCharName();
		if (bShowCharUnique)
		{
			text = text + " (" + this.GetBUID().ToString() + ")";
		}
		bool ridestate = false;
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			ridestate = nr3DCharActor.IsRideState();
		}
		this.m_kHeadUpEntity.MakeName(this.m_eCharKindType, string.Empty, text, ridestate);
	}

	public bool MakeChatText(string chattext, bool checkshowstatus)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsBattleScene())
		{
			return false;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION)
		{
			return false;
		}
		bool ridestate = false;
		if (this.m_k3DChar is Nr3DCharActor)
		{
			Nr3DCharActor nr3DCharActor = this.m_k3DChar as Nr3DCharActor;
			ridestate = nr3DCharActor.IsRideState();
		}
		return this.m_kHeadUpEntity.MakeChat(this.m_eCharKindType, string.Empty, chattext, ridestate, checkshowstatus);
	}

	public bool MakeCharStatus(GameObject pkStatus, float fScale)
	{
		return this.m_kHeadUpEntity != null && this.m_kHeadUpEntity.MakeCharStatus(this.m_eCharKindType, pkStatus, fScale);
	}

	public void HideChatText()
	{
		if (this.m_kHeadUpEntity == null)
		{
			return;
		}
		this.m_kHeadUpEntity.HideChatText();
	}

	public void HideCharStatus()
	{
		if (this.m_kHeadUpEntity == null)
		{
			return;
		}
		this.m_kHeadUpEntity.HideCharStatus();
	}

	public void RefreshCharName(bool bShowCharUnique)
	{
		if (this.m_kHeadUpEntity == null)
		{
			return;
		}
		if (!this.IsReady3DModel())
		{
			return;
		}
		this.MakeCharName(bShowCharUnique);
	}

	public Transform GetNameDummy()
	{
		return this.m_kHeadUpEntity.GetNameDummy();
	}

	public GameObject GetCharStatusObject()
	{
		return this.m_kHeadUpEntity.GetUserStatus();
	}

	public void ShowMovePath(bool bShow)
	{
		if (bShow)
		{
			GameObject gameObject = GameObject.Find("SHOWPATH_" + this.GetBUID());
			if (gameObject == null)
			{
				gameObject = new GameObject("SHOWPATH_" + this.GetBUID());
			}
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			for (int i = 0; i < this.m_liClientPos.Count; i++)
			{
				GameObject gameObject2 = GameObject.Find(string.Concat(new object[]
				{
					"SHOWPATH_Client_",
					this.GetBUID(),
					"_",
					i.ToString()
				}));
				if (!(gameObject2 != null))
				{
					gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					gameObject2.name = string.Concat(new object[]
					{
						"SHOWPATH_Client_",
						this.GetBUID(),
						"_",
						i.ToString()
					});
					vector2 = this.m_liClientPos[i];
					float y = Terrain.activeTerrain.SampleHeight(vector2);
					vector2.y = y;
					gameObject2.transform.position = vector2;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
					MeshRenderer component = gameObject2.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.material = new Material(component.sharedMaterial)
						{
							color = this.m_ClientColor
						};
					}
					if (vector != Vector3.zero)
					{
						Debug.DrawLine(vector, vector2);
						vector = vector2;
					}
				}
			}
			vector = Vector3.zero;
			for (int j = 0; j < this.m_liServerPos.Count; j++)
			{
				GameObject gameObject3 = GameObject.Find(string.Concat(new object[]
				{
					"SHOWPATH_Server_",
					this.GetBUID(),
					"_",
					j.ToString()
				}));
				if (!(gameObject3 != null))
				{
					gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					gameObject3.name = string.Concat(new object[]
					{
						"SHOWPATH_Server_",
						this.GetBUID(),
						"_",
						j.ToString()
					});
					vector2 = this.m_liServerPos[j];
					eBATTLE_MOVE_STATUS eBATTLE_MOVE_STATUS = this.m_liServerStatus[j];
					float y2 = Terrain.activeTerrain.SampleHeight(vector2);
					vector2.y = y2;
					gameObject3.transform.position = vector2;
					gameObject3.transform.parent = gameObject.transform;
					gameObject3.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
					MeshRenderer component2 = gameObject3.GetComponent<MeshRenderer>();
					if (component2 != null)
					{
						Material material = new Material(component2.sharedMaterial);
						if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL)
						{
							material.color = this.m_ServerColor;
						}
						else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_RENEWPOS)
						{
							material.color = new Color(0f, 1f, 0f);
						}
						else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
						{
							material.color = new Color(0f, 0f, 1f);
						}
						else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_STOP)
						{
							material.color = new Color(0f, 0f, 0f);
						}
						else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_CHANGEPOS)
						{
							material.color = new Color(1f, 1f, 1f);
						}
						component2.material = material;
					}
					if (vector != Vector3.zero)
					{
						Debug.DrawLine(vector, vector2);
						vector = vector2;
					}
					this.nShowIndex = j;
				}
			}
		}
		else
		{
			GameObject gameObject4 = GameObject.Find("SHOWPATH_" + this.GetBUID());
			if (gameObject4 != null)
			{
				this.nShowIndex = 0;
				UnityEngine.Object.DestroyImmediate(gameObject4);
			}
		}
	}

	public void ClearPathLog()
	{
		GameObject gameObject = GameObject.Find("SHOWPATH_" + this.GetBUID());
		if (gameObject != null)
		{
			this.nShowIndex = 0;
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		this.m_liClientPos.Clear();
		this.m_liServerPos.Clear();
		this.m_liServerStatus.Clear();
	}

	public void ShowPathFromIndex(bool bReward)
	{
		if (!bReward)
		{
			if (this.nShowIndex >= this.m_liClientPos.Count)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("SHOWPATH_" + this.GetBUID());
			if (gameObject == null)
			{
				gameObject = new GameObject("SHOWPATH_" + this.GetBUID());
			}
			Vector3 vector = Vector3.zero;
			GameObject gameObject2 = GameObject.Find(string.Concat(new object[]
			{
				"SHOWPATH_Client_",
				this.GetBUID(),
				"_",
				this.nShowIndex.ToString()
			}));
			if (gameObject2 == null)
			{
				gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject2.name = string.Concat(new object[]
				{
					"SHOWPATH_Client_",
					this.GetBUID(),
					"_",
					this.nShowIndex.ToString()
				});
				vector = this.m_liClientPos[this.nShowIndex];
				float y = Terrain.activeTerrain.SampleHeight(vector);
				vector.y = y;
				gameObject2.transform.position = vector;
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				MeshRenderer component = gameObject2.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.material = new Material(component.sharedMaterial)
					{
						color = this.m_ClientColor
					};
				}
				gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				gameObject2.name = string.Concat(new object[]
				{
					"SHOWPATH_Server_",
					this.GetBUID(),
					"_",
					this.nShowIndex.ToString()
				});
				vector = this.m_liServerPos[this.nShowIndex];
				eBATTLE_MOVE_STATUS eBATTLE_MOVE_STATUS = this.m_liServerStatus[this.nShowIndex];
				y = Terrain.activeTerrain.SampleHeight(vector);
				vector.y = y;
				gameObject2.transform.position = vector;
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
				component = gameObject2.GetComponent<MeshRenderer>();
				if (component != null)
				{
					Material material = new Material(component.sharedMaterial);
					if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL)
					{
						material.color = this.m_ServerColor;
					}
					else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_RENEWPOS)
					{
						material.color = new Color(0f, 1f, 0f);
					}
					else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
					{
						material.color = new Color(0f, 0f, 1f);
					}
					else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_STOP)
					{
						material.color = new Color(0f, 0f, 0f);
					}
					else if (eBATTLE_MOVE_STATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_CHANGEPOS)
					{
						material.color = new Color(1f, 1f, 1f);
					}
					component.material = material;
				}
			}
			this.nShowIndex++;
		}
		else
		{
			if (this.nShowIndex < 0)
			{
				return;
			}
			GameObject gameObject3 = GameObject.Find(string.Concat(new object[]
			{
				"SHOWPATH_Client_",
				this.GetBUID(),
				"_",
				this.nShowIndex.ToString()
			}));
			if (gameObject3 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
			gameObject3 = GameObject.Find(string.Concat(new object[]
			{
				"SHOWPATH_Server_",
				this.GetBUID(),
				"_",
				this.nShowIndex.ToString()
			}));
			if (gameObject3 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
			this.nShowIndex--;
		}
	}

	public void ShowNavPath(bool bShow)
	{
		if (bShow)
		{
			GameObject gameObject = GameObject.Find("NAVPATH_" + this.GetBUID());
			if (gameObject == null)
			{
				gameObject = new GameObject("NAVPATH_" + this.GetBUID());
			}
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < this.m_NavPath.Count; i++)
			{
				GameObject gameObject2 = GameObject.Find(string.Concat(new object[]
				{
					"Nav_Path_",
					this.GetBUID(),
					"_",
					i.ToString()
				}));
				if (!(gameObject2 != null))
				{
					gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					gameObject2.name = string.Concat(new object[]
					{
						"Nav_Path_",
						this.GetBUID(),
						"_",
						i.ToString()
					});
					vector = this.m_NavPath[i];
					float y = Terrain.activeTerrain.SampleHeight(vector);
					vector.y = y;
					gameObject2.transform.position = vector;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
					MeshRenderer component = gameObject2.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.material = new Material(component.sharedMaterial)
						{
							color = this.m_ClientColor
						};
					}
					LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
					if (lineRenderer == null)
					{
						lineRenderer = gameObject.AddComponent<LineRenderer>();
						lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
						lineRenderer.SetColors(Color.red, Color.blue);
						lineRenderer.SetWidth(0.4f, 0.4f);
						lineRenderer.SetVertexCount(this.m_NavPath.Count);
					}
					lineRenderer.SetPosition(i, vector);
				}
			}
		}
		else
		{
			GameObject gameObject3 = GameObject.Find("NAVPATH_" + this.GetBUID());
			if (gameObject3 != null)
			{
				this.nShowIndex = 0;
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
		}
	}

	public int OrderMoveReq(Vector3 vePos, bool bStartMove, short nGridPos)
	{
		if (Battle.BATTLE.TurnOverRequest)
		{
			return -1;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eAlly)
		{
			return -1;
		}
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return -1;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return -1;
		}
		if (this.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return -1;
		}
		if (this.GetNextOrder() == eBATTLE_ORDER.eBATTLE_ORDER_NONE)
		{
			return -1;
		}
		GS_BF_ORDER_REQ gS_BF_ORDER_REQ = new GS_BF_ORDER_REQ();
		gS_BF_ORDER_REQ.iCharUnique = this.GetIDInfo().m_nCharUnique;
		gS_BF_ORDER_REQ.iFromBFCharUnique = (int)this.GetBUID();
		gS_BF_ORDER_REQ.iToBFCharUnique = (int)this.GetBUID();
		gS_BF_ORDER_REQ.iBFOrderType = 1;
		gS_BF_ORDER_REQ.cBFOrderUnique.Init((int)this.GetBUID());
		int iOrderUnique = gS_BF_ORDER_REQ.cBFOrderUnique.iOrderUnique;
		gS_BF_ORDER_REQ.Pos.x = vePos.x;
		gS_BF_ORDER_REQ.Pos.y = vePos.y;
		gS_BF_ORDER_REQ.Pos.z = vePos.z;
		gS_BF_ORDER_REQ.iPara[0] = ((!bStartMove) ? 0 : 1);
		if (bStartMove)
		{
			gS_BF_ORDER_REQ.iBFNextOrderType = (sbyte)this.GetNextOrder();
			gS_BF_ORDER_REQ.iPara[1] = (int)this.m_nNextOrderTargetBUID;
			gS_BF_ORDER_REQ.iPara[2] = 0;
			gS_BF_ORDER_REQ.iPara[3] = (int)nGridPos;
			if (this.m_CurrentBattleSkillBase != null && this.GetNextOrder() == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
			{
				gS_BF_ORDER_REQ.iPara[4] = this.m_CurrentBattleSkillBase.m_nSkillUnique;
			}
		}
		else
		{
			gS_BF_ORDER_REQ.iBFNextOrderType = 0;
			gS_BF_ORDER_REQ.iPara[1] = -1;
			gS_BF_ORDER_REQ.iPara[2] = 0;
		}
		gS_BF_ORDER_REQ.fSendTime = Time.time;
		this.SendRequestBattleOrder(ref gS_BF_ORDER_REQ);
		this.SetClearAttackTarget();
		return iOrderUnique;
	}

	public int OrderAttackReq(NkBattleChar pkTargetChar, short nGridPos, Vector3 veAttackPos)
	{
		if (Battle.BATTLE.TurnOverRequest)
		{
			return -1;
		}
		if (pkTargetChar == null)
		{
			return -1;
		}
		if (!this.m_bMyChar)
		{
			return -1;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eAlly)
		{
			return -1;
		}
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return -1;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return -1;
		}
		if (this.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return -1;
		}
		int result = -1;
		short num = -1;
		int num2 = this.CanAttack(pkTargetChar, nGridPos, veAttackPos, ref num);
		if (num2 == -1)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("578"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			return -1;
		}
		if (num2 == -2)
		{
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_ATTACK);
			this.SetNextOrderTarget(pkTargetChar.GetBUID());
			this.OrderMoveReq(veAttackPos, true, num);
		}
		else if (num2 == 1)
		{
			GS_BF_ORDER_REQ gS_BF_ORDER_REQ = new GS_BF_ORDER_REQ();
			gS_BF_ORDER_REQ.iCharUnique = this.GetIDInfo().m_nCharUnique;
			gS_BF_ORDER_REQ.iFromBFCharUnique = (int)this.GetBUID();
			gS_BF_ORDER_REQ.iToBFCharUnique = (int)pkTargetChar.GetBUID();
			gS_BF_ORDER_REQ.iBFOrderType = 2;
			gS_BF_ORDER_REQ.cBFOrderUnique.Init((int)this.GetBUID());
			result = gS_BF_ORDER_REQ.cBFOrderUnique.iOrderUnique;
			gS_BF_ORDER_REQ.Pos.x = this.GetCharPos().x;
			gS_BF_ORDER_REQ.Pos.y = this.GetCharPos().y;
			gS_BF_ORDER_REQ.Pos.z = this.GetCharPos().z;
			gS_BF_ORDER_REQ.iPara[3] = (int)num;
			this.SendRequestBattleOrder(ref gS_BF_ORDER_REQ);
		}
		else if (num2 == -3)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrderTarget(-1);
		}
		this.MouseEvent_Exit();
		return result;
	}

	public int OrderAttackLandReq(Vector3 vePos)
	{
		return -1;
	}

	public int OrderBattleSkillReq(int iBattleSkillIndex, NkBattleChar pkTargetChar, Vector3 pos, short nGridPos, int ItemBattleSkillUnique, int ItemBattleSkillLevel)
	{
		if (Battle.BATTLE.TurnOverRequest)
		{
			return -1;
		}
		if (pkTargetChar == null)
		{
			return -1;
		}
		if (!this.m_bMyChar)
		{
			return -1;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eAlly)
		{
			return -1;
		}
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return -1;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return -1;
		}
		if (this.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return -1;
		}
		int num = 0;
		int skillLevel = 0;
		if (iBattleSkillIndex == -1)
		{
			if (ItemBattleSkillUnique <= 0 || ItemBattleSkillLevel <= 0)
			{
				return -1;
			}
			num = ItemBattleSkillUnique;
			skillLevel = ItemBattleSkillLevel;
		}
		int result = -1;
		if (iBattleSkillIndex > -1 && num == 0)
		{
			num = this.GetSoldierInfo().SelectBattleSkillByWeapon(iBattleSkillIndex);
			skillLevel = this.GetSoldierInfo().GetBattleSkillLevel(num);
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(num);
		BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(num, skillLevel);
		if (battleSkillBase == null || battleSkillDetail == null)
		{
			return -1;
		}
		if (this.IsBattleCharATB(32) && battleSkillBase.m_nSkillItemType != 1)
		{
			return -1;
		}
		int num2 = this.CanBattleSkill(pkTargetChar, pos, battleSkillBase, battleSkillDetail);
		if (num2 == -1)
		{
			return -1;
		}
		if (num2 == -2)
		{
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_SKILL);
			this.SetNextOrderTarget(pkTargetChar.GetBUID());
			this.SetBattleSkillCurrent(battleSkillBase.m_nSkillUnique, -1);
			this.OrderMoveReq(pos, true, nGridPos);
		}
		else if (num2 == 1)
		{
			this.SetBattleSkillCurrent(battleSkillBase.m_nSkillUnique, -1);
			GS_BF_ORDER_REQ gS_BF_ORDER_REQ = new GS_BF_ORDER_REQ();
			gS_BF_ORDER_REQ.iCharUnique = this.GetIDInfo().m_nCharUnique;
			gS_BF_ORDER_REQ.iFromBFCharUnique = (int)this.GetBUID();
			gS_BF_ORDER_REQ.iToBFCharUnique = (int)pkTargetChar.GetBUID();
			gS_BF_ORDER_REQ.iBFOrderType = 3;
			gS_BF_ORDER_REQ.cBFOrderUnique.Init((int)this.GetBUID());
			result = gS_BF_ORDER_REQ.cBFOrderUnique.iOrderUnique;
			gS_BF_ORDER_REQ.Pos.x = this.GetCharPos().x;
			gS_BF_ORDER_REQ.Pos.y = this.GetCharPos().y;
			gS_BF_ORDER_REQ.Pos.z = this.GetCharPos().z;
			gS_BF_ORDER_REQ.iPara[2] = num;
			gS_BF_ORDER_REQ.iPara[3] = (int)nGridPos;
			this.SendRequestBattleOrder(ref gS_BF_ORDER_REQ);
		}
		else if (num2 == -3)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrderTarget(-1);
		}
		else if (num2 == -5)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrderTarget(-1);
			string text = string.Empty;
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("353");
			if (text != string.Empty)
			{
				Main_UI_SystemMessage.ADDMessage(text, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		else if (num2 == -6)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrderTarget(-1);
			string text2 = string.Empty;
			text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("352");
			if (text2 != string.Empty)
			{
				Main_UI_SystemMessage.ADDMessage(text2, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		this.MouseEvent_Exit();
		return result;
	}

	public int OrderSearchReq(NkBattleChar pkTargetChar)
	{
		if (Battle.BATTLE.TurnOverRequest)
		{
			return -1;
		}
		if (pkTargetChar == null)
		{
			return -1;
		}
		if (!this.m_bMyChar)
		{
			return -1;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eAlly)
		{
			return -1;
		}
		if (Battle.BATTLE.GetBattleRoomState() != eBATTLE_ROOM_STATE.eBATTLE_ROOM_STATE_ACTION)
		{
			return -1;
		}
		if (!Battle.BATTLE.IsEnableOrderTime)
		{
			return -1;
		}
		if (this.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return -1;
		}
		int result = -1;
		int num = this.CanSearch(pkTargetChar);
		if (num == -1)
		{
			return -1;
		}
		if (num == -2)
		{
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_SEARCH);
			this.SetNextOrderTarget(pkTargetChar.GetBUID());
			this.OrderMoveReq(pkTargetChar.GetCharPos(), true, -1);
		}
		else if (num == 1)
		{
			GS_BF_ORDER_REQ gS_BF_ORDER_REQ = new GS_BF_ORDER_REQ();
			gS_BF_ORDER_REQ.iCharUnique = this.GetIDInfo().m_nCharUnique;
			gS_BF_ORDER_REQ.iFromBFCharUnique = (int)this.GetBUID();
			gS_BF_ORDER_REQ.iToBFCharUnique = (int)pkTargetChar.GetBUID();
			gS_BF_ORDER_REQ.iBFOrderType = 5;
			gS_BF_ORDER_REQ.cBFOrderUnique.Init((int)this.GetBUID());
			result = gS_BF_ORDER_REQ.cBFOrderUnique.iOrderUnique;
			gS_BF_ORDER_REQ.Pos.x = this.GetCharPos().x;
			gS_BF_ORDER_REQ.Pos.y = this.GetCharPos().y;
			gS_BF_ORDER_REQ.Pos.z = this.GetCharPos().z;
			this.SendRequestBattleOrder(ref gS_BF_ORDER_REQ);
		}
		else if (num == -3)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetNextOrderTarget(-1);
		}
		return result;
	}

	public int OrderChangePosReq(short nStartPosIndex, short nGridPos)
	{
		if (Battle.BATTLE.TurnOverRequest)
		{
			return -1;
		}
		if (nStartPosIndex != this.GetStartPosIndex())
		{
			return -1;
		}
		if (nGridPos < 0)
		{
			return -1;
		}
		if (!this.m_bMyChar)
		{
			return -1;
		}
		if (this.GetGridPos() == nGridPos)
		{
			return -1;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eAlly)
		{
			return -1;
		}
		if (this.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
		{
			return -1;
		}
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(this.Ally, this.GetStartPosIndex());
		if ((int)nGridPos >= battleGrid.m_nWidthCount * battleGrid.m_nHeightCount)
		{
			return -1;
		}
		List<short> list = new List<short>();
		list.Clear();
		int num = this.EnableChangePos(this.GetCharKindInfo(), (sbyte)nGridPos, battleGrid, ref list);
		if (num < 0)
		{
			return num;
		}
		bool flag = false;
		if (num == 0)
		{
			flag = true;
		}
		if (flag)
		{
			if (!this.IsEnablePos(this.GetCharKindInfo(), (sbyte)nGridPos, battleGrid, -1, this.GetBUID()))
			{
				return -1;
			}
		}
		else
		{
			short gridPos = this.GetGridPos();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] > -1)
				{
					NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(list[i]);
					if (charByBUID != null)
					{
						if (charByBUID.m_bDeadReaservation)
						{
							return -1;
						}
						if (!this.IsEnablePos(charByBUID.GetCharKindInfo(), (sbyte)((int)gridPos + i), battleGrid, this.GetBUID(), charByBUID.GetBUID()))
						{
							return -1;
						}
						if (!this.IsOverLapPos(battleGrid, charByBUID.GetCharKindInfo(), (short)((int)gridPos + i), nGridPos))
						{
							return -1;
						}
					}
				}
			}
		}
		GS_BF_ORDER_REQ gS_BF_ORDER_REQ = new GS_BF_ORDER_REQ();
		gS_BF_ORDER_REQ.iCharUnique = this.GetIDInfo().m_nCharUnique;
		gS_BF_ORDER_REQ.iFromBFCharUnique = (int)this.GetBUID();
		gS_BF_ORDER_REQ.iToBFCharUnique = -1;
		gS_BF_ORDER_REQ.iBFOrderType = 6;
		gS_BF_ORDER_REQ.cBFOrderUnique.Init((int)this.GetBUID());
		int iOrderUnique = gS_BF_ORDER_REQ.cBFOrderUnique.iOrderUnique;
		gS_BF_ORDER_REQ.iPara[0] = (int)nGridPos;
		this.SendRequestBattleOrder(ref gS_BF_ORDER_REQ);
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "FORMATION-CLICK", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_bChangePos = false;
		return iOrderUnique;
	}

	public void SendRequestBattleOrder(ref GS_BF_ORDER_REQ Req)
	{
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BF_ORDER_REQ, Req);
		if ((int)Req.iBFOrderType != 6)
		{
			Battle.BATTLE.SelectNextChar();
		}
	}

	public int CanBattleSkill(NkBattleChar Target, Vector3 veHitPos, BATTLESKILL_BASE BSkillBase, BATTLESKILL_DETAIL BSkillDetail)
	{
		if (BSkillBase == null || BSkillDetail == null)
		{
			return -1;
		}
		if (Target == null)
		{
			return -1;
		}
		if (BSkillDetail.GetSkillDetalParamValue(79) > 0)
		{
			if (Target.GetSoldierInfo().GetHP() > 0)
			{
				return -3;
			}
		}
		else if (Target.GetSoldierInfo().GetHP() <= 0)
		{
			return -3;
		}
		if (this.GetSoldierInfo().GetHP() <= 0)
		{
			return -4;
		}
		if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK && Target.GetBUID() == this.m_nAttackTargetBUID)
		{
			return -1;
		}
		Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
		if (battle_Control_Dlg != null && !battle_Control_Dlg.CheckBattleSkillUseAble(BSkillDetail.m_nSkillUnique, BSkillDetail.m_nSkillNeedAngerlyPoint))
		{
			return -5;
		}
		if (BSkillBase.ChecJobTypeMove())
		{
			if (BSkillBase.ChecJobTypeHalfMove())
			{
				Vector3 charPos = this.GetCharPos();
				charPos.y = 0f;
				BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(Target.Ally, Target.GetStartPosIndex());
				if (battleGrid == null)
				{
					return -1;
				}
				Vector3 centerFront = battleGrid.GetCenterFront();
				centerFront.y = 0f;
				float num = Vector3.Distance(charPos, centerFront);
				if (num > 1f)
				{
					veHitPos = centerFront;
					return -2;
				}
			}
			else
			{
				Vector3 charPos2 = this.GetCharPos();
				charPos2.y = 0f;
				Vector3 b = veHitPos;
				b.y = 0f;
				float num2 = Vector3.Distance(charPos2, b);
				if (this.GetCharKindInfo() != null)
				{
					if (this.GetCharKindInfo().GetCHARKIND_ATTACKINFO() != null)
					{
						if (BSkillBase.m_nSkillMoveRange > 0f)
						{
							if (num2 > BSkillBase.m_nSkillMoveRange + 1f)
							{
								return -2;
							}
						}
						else if (num2 > this.GetAttackRange() + 1f)
						{
							return -2;
						}
					}
					else if (num2 > 2f)
					{
						return -2;
					}
				}
				else if (num2 > 2f)
				{
					return -2;
				}
			}
		}
		return 1;
	}

	public bool CanBattleSkillForTargetGrid(NkBattleChar Target, short nGridPos, BATTLESKILL_BASE BSkillBase, BATTLESKILL_DETAIL BSkillDetail)
	{
		if (Target == null || BSkillBase == null || BSkillDetail == null)
		{
			return false;
		}
		if (Target == null)
		{
			return false;
		}
		if (BSkillDetail.GetSkillDetalParamValue(79) > 0)
		{
			if (Target.GetSoldierInfo().GetHP() > 0)
			{
				return false;
			}
		}
		else if (Target.GetSoldierInfo().GetHP() <= 0)
		{
			return false;
		}
		return this.GetSoldierInfo().GetHP() > 0 && (this.GetCurrentOrder() != eBATTLE_ORDER.eBATTLE_ORDER_ATTACK || Target.GetBUID() != this.m_nAttackTargetBUID) && this.CanAttackRange(BSkillBase.m_nSkillRange, Target, nGridPos);
	}

	private bool CanAttackRange(int nAttackRange, NkBattleChar pkTarget, short nGridPos)
	{
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(pkTarget.Ally, pkTarget.GetStartPosIndex());
		if (battleGrid == null)
		{
			return false;
		}
		int nWidthCount = battleGrid.m_nWidthCount;
		short num = pkTarget.GetGridPos();
		if (num != nGridPos && nGridPos >= 0 && battleGrid.m_veBUID[(int)nGridPos] == pkTarget.GetBUID())
		{
			num = nGridPos;
		}
		int num2 = (int)num / nWidthCount;
		if (num2 <= 0)
		{
			return true;
		}
		int num3 = 0;
		for (int i = 0; i < num2; i++)
		{
			int num4 = (int)num - (i + 1) * nWidthCount;
			if (num4 < 0)
			{
				break;
			}
			if (battleGrid.m_veBUID[num4] > -1 && pkTarget.GetBUID() != battleGrid.m_veBUID[num4])
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(battleGrid.m_veBUID[num4]);
				if (charByBUID != null)
				{
					if (!charByBUID.m_bDeadReaservation)
					{
						num3++;
					}
				}
			}
		}
		return nAttackRange > num3;
	}

	private short FindAttackGrid(int nAttackRange, NkBattleChar pkTarget, short nGridPos)
	{
		BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(pkTarget.Ally, pkTarget.GetStartPosIndex());
		int nWidthCount = battleGrid.m_nWidthCount;
		short result = nGridPos;
		short num = pkTarget.GetGridPos();
		if (num != nGridPos && nGridPos >= 0 && battleGrid.m_veBUID[(int)nGridPos] == pkTarget.GetBUID())
		{
			num = nGridPos;
		}
		int num2 = (int)num / nWidthCount;
		if (num2 <= 0)
		{
			return nGridPos;
		}
		if (this.GetJobType() == 3 || this.GetJobType() == 4)
		{
			return nGridPos;
		}
		for (int i = 0; i < num2; i++)
		{
			int num3 = (int)num - (i + 1) * nWidthCount;
			if (num3 < 0)
			{
				break;
			}
			if (battleGrid.m_veBUID[num3] > -1 && pkTarget.GetBUID() == battleGrid.m_veBUID[num3])
			{
				result = (short)num3;
			}
		}
		return result;
	}

	public int CanAttack(NkBattleChar Target, short nGridPos, Vector3 veAttackPos, ref short nAttackGrid)
	{
		if (Target == null)
		{
			return -1;
		}
		if (Target.GetSoldierInfo().GetHP() <= 0)
		{
			return -3;
		}
		if (Target.IsBattleCharATB(64))
		{
			return -1;
		}
		if (this.GetSoldierInfo().GetHP() <= 0)
		{
			return -4;
		}
		if (this.IsBattleCharATB(2) || this.IsBattleCharATB(8192))
		{
			return -1;
		}
		if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK && Target.GetBUID() == this.m_nAttackTargetBUID)
		{
			return -1;
		}
		if (!this.CanAttackRange((int)this.GetAttackInfo().CANATTACKRANGE, Target, nGridPos))
		{
			return -1;
		}
		nAttackGrid = this.FindAttackGrid((int)this.GetAttackInfo().CANATTACKRANGE, Target, nGridPos);
		if (nAttackGrid == -1)
		{
			nAttackGrid = nGridPos;
		}
		Vector3 charPos = this.GetCharPos();
		charPos.y = 0f;
		Vector3 b = veAttackPos;
		b.y = 0f;
		float num = Vector3.Distance(charPos, b);
		if (this.GetCharKindInfo() != null)
		{
			if (this.GetCharKindInfo().GetCHARKIND_ATTACKINFO() != null)
			{
				if (num > this.GetAttackRange() + 1f)
				{
					return -2;
				}
			}
			else if (num > 2f)
			{
				return -2;
			}
		}
		else if (num > 2f)
		{
			return -2;
		}
		return 1;
	}

	private int CanSearch(NkBattleChar Target)
	{
		if (Target == null)
		{
			return -1;
		}
		if (Target.GetSoldierInfo().GetHP() > 0)
		{
			return -3;
		}
		if (this.GetSoldierInfo().GetHP() <= 0)
		{
			return -4;
		}
		if (Target.IsCharKindATB(256L))
		{
			return -1;
		}
		if (!this.CanAttackRange((int)this.GetAttackInfo().CANATTACKRANGE, Target, -1))
		{
			return -1;
		}
		Vector3 charPos = this.GetCharPos();
		charPos.y = 0f;
		Vector3 charPos2 = Target.GetCharPos();
		charPos2.y = 0f;
		float num = Vector3.Distance(charPos, charPos2);
		if (this.GetCharKindInfo() != null)
		{
			if (this.GetCharKindInfo().GetCHARKIND_ATTACKINFO() != null)
			{
				if (num > this.GetCharBound() + 1f)
				{
					return -2;
				}
			}
			else if (num > 2f)
			{
				return -2;
			}
		}
		else if (num > 2f)
		{
			return -2;
		}
		return 1;
	}

	public bool IsEnablePos(NrCharKindInfo pkKindInfo, sbyte nGridPos, BATTLE_POS_GRID start_pos, short nExceptBUID, short nMyBuid)
	{
		if ((int)nGridPos < 0)
		{
			return false;
		}
		int battleSizeX = (int)pkKindInfo.GetBattleSizeX();
		int battleSizeY = (int)pkKindInfo.GetBattleSizeY();
		int num = (int)nGridPos % start_pos.m_nWidthCount;
		int num2 = (int)nGridPos / start_pos.m_nWidthCount;
		if (num + battleSizeX > start_pos.m_nWidthCount)
		{
			return false;
		}
		if (num2 + battleSizeY > start_pos.m_nHeightCount)
		{
			return false;
		}
		for (int i = 0; i < battleSizeY; i++)
		{
			for (int j = 0; j < battleSizeX; j++)
			{
				int num3 = num + j + (num2 + i) * start_pos.m_nWidthCount;
				if (num3 < 0 || num3 >= start_pos.m_nHeightCount * start_pos.m_nWidthCount)
				{
					return false;
				}
				if (start_pos.m_veBUID[num3] > -1 && start_pos.m_veBUID[num3] != nExceptBUID && start_pos.m_veBUID[num3] != nMyBuid)
				{
					return false;
				}
				if (!start_pos.m_vebActive[num3])
				{
					return false;
				}
			}
		}
		return true;
	}

	private int EnableChangePos(NrCharKindInfo pkKindInfo, sbyte nGridPos, BATTLE_POS_GRID start_pos, ref List<short> veBUID)
	{
		if (pkKindInfo == null)
		{
			return -1;
		}
		if ((int)nGridPos < 0)
		{
			return -1;
		}
		if (!this.m_bChangePos)
		{
			return -2;
		}
		int battleSizeX = (int)pkKindInfo.GetBattleSizeX();
		int battleSizeY = (int)pkKindInfo.GetBattleSizeY();
		int num = battleSizeX * battleSizeY;
		int num2 = (int)nGridPos % start_pos.m_nWidthCount;
		int num3 = (int)nGridPos / start_pos.m_nWidthCount;
		if (num2 + battleSizeX > start_pos.m_nWidthCount)
		{
			return -1;
		}
		if (num3 + battleSizeY > start_pos.m_nHeightCount)
		{
			return -1;
		}
		int num4 = 0;
		int num5 = 0;
		veBUID.Clear();
		for (int i = 0; i < num; i++)
		{
			veBUID.Add(-1);
		}
		for (int j = 0; j < battleSizeY; j++)
		{
			for (int k = 0; k < battleSizeX; k++)
			{
				int num6 = num2 + k + (num3 + j) * start_pos.m_nWidthCount;
				if (num6 < 0 || num6 >= start_pos.m_nHeightCount * start_pos.m_nWidthCount)
				{
					return -1;
				}
				if (start_pos.m_veBUID[num6] > -1 && start_pos.m_veBUID[num6] != this.GetBUID())
				{
					veBUID[num4] = start_pos.m_veBUID[num6];
					num5++;
				}
				if (!start_pos.m_vebActive[num6])
				{
					return -1;
				}
				num4++;
			}
		}
		return num5;
	}

	private bool IsOverLapPos(BATTLE_POS_GRID start_pos, NrCharKindInfo pkTargetKindInfo, short nTargetMoveGrid, short nMoveGrid)
	{
		if (pkTargetKindInfo == null)
		{
			return false;
		}
		if (nTargetMoveGrid < 0)
		{
			return false;
		}
		if (nMoveGrid < 0)
		{
			return false;
		}
		List<int> list = new List<int>();
		list.Clear();
		List<int> list2 = new List<int>();
		list2.Clear();
		int battleSizeX = (int)pkTargetKindInfo.GetBattleSizeX();
		int battleSizeY = (int)pkTargetKindInfo.GetBattleSizeY();
		int num = (int)nTargetMoveGrid % start_pos.m_nWidthCount;
		int num2 = (int)nTargetMoveGrid / start_pos.m_nWidthCount;
		if (num + battleSizeX > start_pos.m_nWidthCount)
		{
			return false;
		}
		if (num2 + battleSizeY > start_pos.m_nHeightCount)
		{
			return false;
		}
		for (int i = 0; i < battleSizeY; i++)
		{
			for (int j = 0; j < battleSizeX; j++)
			{
				int num3 = num + j + (num2 + i) * start_pos.m_nWidthCount;
				if (num3 < 0 || num3 >= start_pos.m_nHeightCount * start_pos.m_nWidthCount)
				{
					return false;
				}
				list.Add(num3);
			}
		}
		battleSizeX = (int)this.GetCharKindInfo().GetBattleSizeX();
		battleSizeY = (int)this.GetCharKindInfo().GetBattleSizeY();
		num = (int)nMoveGrid % start_pos.m_nWidthCount;
		num2 = (int)nMoveGrid / start_pos.m_nWidthCount;
		if (num + battleSizeX > start_pos.m_nWidthCount)
		{
			return false;
		}
		if (num2 + battleSizeY > start_pos.m_nHeightCount)
		{
			return false;
		}
		for (int i = 0; i < battleSizeY; i++)
		{
			for (int j = 0; j < battleSizeX; j++)
			{
				int num4 = num + j + (num2 + i) * start_pos.m_nWidthCount;
				if (num4 < 0 || num4 >= start_pos.m_nHeightCount * start_pos.m_nWidthCount)
				{
					return false;
				}
				list2.Add(num4);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				if (list[i] == list2[j])
				{
					return false;
				}
			}
		}
		return true;
	}

	public void EnOrderACK(GS_BF_ORDER_ACK _OrderACK)
	{
		this.m_OrderACKQueue.Enqueue(_OrderACK);
	}

	public void PopCharOrder()
	{
		if (this.m_OrderACKQueue.Count > 0)
		{
			GS_BF_ORDER_ACK orderACK = this.m_OrderACKQueue.Peek();
			if (this.DoOrderACK(orderACK))
			{
				this.m_OrderACKQueue.Dequeue();
			}
		}
	}

	private bool DoOrderACK(GS_BF_ORDER_ACK _OrderACK)
	{
		bool result = true;
		eBATTLE_ORDER eBATTLE_ORDER = (eBATTLE_ORDER)_OrderACK.iBFOrderType;
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID((short)_OrderACK.iToBFCharUnique);
		Battle.BATTLE.m_fOrderPing = Battle.BATTLE.BattleTimeLagFromServer(_OrderACK.fServerTime);
		if (Battle.BATTLE.m_fOrderPing >= 1f)
		{
			Debug.Log("Order Diff Time : " + Battle.BATTLE.m_fOrderPing);
			TsPlatform.FileLog("Order Diff Time : " + Battle.BATTLE.m_fOrderPing);
		}
		switch (eBATTLE_ORDER)
		{
		case eBATTLE_ORDER.eBATTLE_ORDER_MOVE:
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_MOVE);
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_ATTACK:
			if (charByBUID == null)
			{
				return false;
			}
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_ATTACK);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetOrderUnique(_OrderACK.cBFOrderUnique.iOrderUnique);
			this.SetAttackTarget((short)_OrderACK.iToBFCharUnique, (short)_OrderACK.iPara[3]);
			this.SetAttackAniTypeEvent((eCharAniTypeForEvent)_OrderACK.iPara[4]);
			this.SetTurnState((eBATTLE_TURN_STATE)_OrderACK.nTurnState);
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_SKILL:
		{
			if (charByBUID == null)
			{
				return false;
			}
			int skillUnique = _OrderACK.iPara[2];
			int skillLevel = _OrderACK.iPara[1];
			this.m_bRivalAttack = false;
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_SKILL);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetOrderUnique(_OrderACK.cBFOrderUnique.iOrderUnique);
			this.SetAttackTarget((short)_OrderACK.iToBFCharUnique, (short)_OrderACK.iPara[3]);
			this.SetBattleSkillCurrent(skillUnique, skillLevel);
			this.GetAniTypeFromAnimation((eCharAnimationType)this.m_CurrentBattleSkillBase.m_nSkillAniSequenceCode);
			this.SetTurnState((eBATTLE_TURN_STATE)_OrderACK.nTurnState);
			if (_OrderACK.iPara[0] == 1)
			{
				this.m_bLastAttacker = true;
			}
			if ((_OrderACK.nTemp & 1) > 0)
			{
				this.m_bRivalAttack = true;
			}
			this.SetBattleSkillCoolTurn(false);
			int num = _OrderACK.iPara[4];
			if (num >= 0)
			{
				if (!Battle.BATTLE.ColosseumObserver)
				{
					Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
					if (battle_Control_Dlg != null)
					{
						if (!Battle.BATTLE.Observer)
						{
							if (Battle.BATTLE.MyAlly == this.m_eAlly && Battle.BATTLE.MyStartPosIndex == this.GetStartPosIndex())
							{
								battle_Control_Dlg.SetAngerlyPoint(num);
								battle_Control_Dlg.UpdateBattleSkillData();
							}
						}
						else if (this.GetStartPosIndex() == 0 && this.Ally == Battle.BATTLE.CurrentTurnAlly)
						{
							battle_Control_Dlg.SetAngerlyPoint(num);
							battle_Control_Dlg.UpdateBattleSkillData();
						}
					}
				}
				else
				{
					ColosseumObserverControlDlg colosseumObserverControlDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG) as ColosseumObserverControlDlg;
					if (colosseumObserverControlDlg != null)
					{
						colosseumObserverControlDlg.SetAngerPoint(this.Ally, num);
					}
				}
			}
			break;
		}
		case eBATTLE_ORDER.eBATTLE_ORDER_SEARCH:
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_SEARCH);
			this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			this.SetOrderUnique(_OrderACK.cBFOrderUnique.iOrderUnique);
			this.SetTurnState((eBATTLE_TURN_STATE)_OrderACK.nTurnState);
			NrSound.ImmedatePlay("UI_SFX", "BATTLE", "SEARCH");
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS:
			this.SetTurnState((eBATTLE_TURN_STATE)_OrderACK.nTurnState);
			break;
		}
		return result;
	}

	private void ProcessOrder()
	{
		if (Battle.BATTLE.Stop)
		{
			return;
		}
		if (!this.IsReady3DModel())
		{
			return;
		}
		switch (this.GetCurrentOrder())
		{
		case eBATTLE_ORDER.eBATTLE_ORDER_NONE:
			this.SetAttackTarget(-1, -1);
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_MOVE:
			this.ProcessMoveOrder();
			this.SetAttackTarget(-1, -1);
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_ATTACK:
			this.ProcessMoveOrder();
			this.ProcessAttackOrder();
			break;
		case eBATTLE_ORDER.eBATTLE_ORDER_SKILL:
			this.ProcessMoveOrder();
			this.ProcessBattleSkillOrder();
			break;
		}
	}

	private void ProcessNextOrder()
	{
		switch (this.GetNextOrder())
		{
		}
	}

	private void ProcessMoveOrder()
	{
		if (this.m_kCharMove.SetCurrentPosInfo())
		{
			this.m_kCharMove.ProcessCharMove(false);
		}
	}

	private void ProcessAttackOrder()
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_nAttackTargetBUID);
		if (charByBUID == null || charByBUID.Get3DCharStep() == NkBattleChar.e3DCharStep.DIED)
		{
			this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			return;
		}
		if (this.m_OrderStartTime == 0f)
		{
			if (!this.IsCharKindATB(4194304L))
			{
				BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
				Vector3 v3Direction = Vector3.zero;
				if (this.m_nAttackGrid != -1)
				{
					if (battleGrid != null)
					{
						v3Direction = battleGrid.mListPos[(int)this.m_nAttackGrid];
						v3Direction.y = charByBUID.GetCharPos().y;
					}
					else
					{
						v3Direction = charByBUID.GetCharPos();
					}
				}
				else
				{
					v3Direction = charByBUID.GetCharPos();
				}
				this.SetLookAt(v3Direction, false);
			}
			this.SetAnimation(this.GetAnimationFromAniType(), true, false);
			this.m_OrderStartTime = Time.time;
			this.m_bCreateBullet = false;
			int weaponType = this.GetSoldierInfo().GetWeaponType();
			this.m_nTotalHitCount = this.GetCharKindInfo().GetHitAniCount(weaponType, (int)this.m_eAttackTypeAniEvent);
			if (this.m_nTotalHitCount <= 0)
			{
				this.m_nTotalHitCount = 1;
			}
			if (this.m_nTotalHitCount >= CharDefine.MAX_HIT_COUNT)
			{
				this.m_nTotalHitCount = CharDefine.MAX_HIT_COUNT;
			}
			this.m_nProcessHitCount = 0;
		}
		else
		{
			if (!this.m_bCreateBullet && !this.IsCharKindATB(4194304L))
			{
				BATTLE_POS_GRID battleGrid2 = Battle.BATTLE.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
				Vector3 v3Direction2 = Vector3.zero;
				if (battleGrid2 != null)
				{
					if (this.m_nAttackGrid != -1)
					{
						v3Direction2 = battleGrid2.mListPos[(int)this.m_nAttackGrid];
						v3Direction2.y = charByBUID.GetCharPos().y;
					}
					else
					{
						v3Direction2 = charByBUID.GetCharPos();
					}
				}
				else
				{
					v3Direction2 = charByBUID.GetCharPos();
				}
				this.SetLookAt(v3Direction2, false);
			}
			int weaponType2 = this.GetSoldierInfo().GetWeaponType();
			float num = 0f;
			if (!this.m_bCreateBullet)
			{
				num = this.GetCharKindInfo().GetAnimationEvent(weaponType2, (int)this.m_eAttackTypeAniEvent, NkBattleChar.m_nHitType[this.m_nProcessHitCount]);
			}
			float animationEvent = this.GetCharKindInfo().GetAnimationEvent(weaponType2, (int)this.m_eAttackTypeAniEvent, 0);
			if (num == 0f)
			{
				num = 0.3f;
			}
			if (Mathf.Abs(Time.time - this.m_OrderStartTime) > num && !this.m_bCreateBullet)
			{
				if (charByBUID.m_bDeadReaservation && this.m_nTotalHitCount > 1)
				{
					this.m_bCreateBullet = true;
					if (this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK)
					{
						this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
						this.SetAnimation(this.GetStayAni(), true, true);
						return;
					}
				}
				if (this.GetJobType() != 1 && this.GetJobType() != 2)
				{
					NrTSingleton<NkBulletManager>.Instance.CreateBullet(this.GetSoldierInfo().GetAttackInfo().BulletCode, this, charByBUID, Time.time);
				}
				this.m_nProcessHitCount++;
				if (this.m_nProcessHitCount >= this.m_nTotalHitCount)
				{
					this.m_bCreateBullet = true;
				}
			}
			if (this.m_bCreateBullet && Mathf.Abs(Time.time - this.m_OrderStartTime) > animationEvent && this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK)
			{
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
			}
		}
	}

	private void ProcessBattleSkillOrder()
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_nAttackTargetBUID);
		if (charByBUID == null || charByBUID.Get3DCharStep() == NkBattleChar.e3DCharStep.DIED)
		{
			if (!this.m_bLastAttacker)
			{
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				return;
			}
			Battle.BATTLE.BattleCamera.SetLastAttackCamera(this, false);
			if (this.m_fSkillAniEndtime > 0f && Mathf.Abs(Time.time - this.m_OrderStartTime) > this.m_fSkillAniEndtime)
			{
				this.m_bLastAttacker = false;
				this.m_fSkillAniEndtime = 0f;
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				this.SetAnimation(this.GetStayAni());
				return;
			}
			return;
		}
		else
		{
			if (this.m_CurrentBattleSkillBase == null || this.m_CurrentBattleSkillDetail == null)
			{
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				this.SetNextOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				this.SetNextOrderTarget(-1);
				this.SetAnimation(this.GetStayAni());
				return;
			}
			if (this.m_OrderStartTime == 0f)
			{
				if (this.MyChar && this.m_CurrentBattleSkillBase.m_nSkillItemType == 0)
				{
					Battle_Skill_Direction_Dlg battle_Skill_Direction_Dlg = (Battle_Skill_Direction_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILL_DIRECTION_DLG);
					battle_Skill_Direction_Dlg.SetMagic(this, this.m_CurrentBattleSkillBase.m_nSkillUnique, this.m_bRivalAttack);
				}
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				this.SetAnimation(this.GetStayAni());
				this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_SKILL);
				this.m_bSkillWait = false;
				this.m_OrderStartTime = Time.time;
			}
			if (!this.m_bSkillWait)
			{
				bool flag = false;
				if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY && this.m_CurrentBattleSkillBase.m_nSkillItemType == 0)
				{
					if (Mathf.Abs(Time.time - this.m_OrderStartTime) > 1f)
					{
						flag = true;
					}
					if (this.IsMonster)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					if ((!this.IsCharKindATB(4194304L) || this.m_CurrentBattleSkillBase.m_nSkillTargetType == 1 || this.m_CurrentBattleSkillBase.m_nSkillTargetType == 2) && !this.IsCharKindATB(4194304L))
					{
						BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
						Vector3 v3Direction = Vector3.zero;
						if (battleGrid != null)
						{
							if (this.m_CurrentBattleSkillBase.m_nSkillGridType == 6)
							{
								v3Direction = battleGrid.GetCenter();
							}
							else if (this.m_nAttackGrid != -1 && (int)this.m_nAttackGrid < battleGrid.mListPos.Length)
							{
								v3Direction = battleGrid.mListPos[(int)this.m_nAttackGrid];
								v3Direction.y = charByBUID.GetCharPos().y;
							}
							else
							{
								v3Direction = charByBUID.GetCharPos();
							}
						}
						else
						{
							v3Direction = charByBUID.GetCenterPosition();
						}
						this.SetLookAt(v3Direction, false);
					}
					this.SetAnimation((eCharAnimationType)this.m_CurrentBattleSkillBase.m_nSkillAniSequenceCode, true, false);
					if (this.m_bLastAttacker)
					{
						Battle.BATTLE.BattleCamera.SetLastAttackCamera(this, true);
					}
					bool flag2 = true;
					if (TsPlatform.IsIPhone && TsPlatform.IsLowSystemMemory && (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY))
					{
						flag2 = false;
					}
					string text = string.Empty;
					text = this.m_CurrentBattleSkillBase.GetBSkillCasterEffectCode();
					if (!string.IsNullOrEmpty(text) && flag2)
					{
						NrTSingleton<NkEffectManager>.Instance.AddEffect(text, this);
					}
					string text2 = string.Empty;
					text2 = this.m_CurrentBattleSkillBase.GetBSkillEnemyCastEffect();
					if (!string.IsNullOrEmpty(text2))
					{
						BATTLE_POS_GRID battleGrid2 = Battle.BATTLE.GetBattleGrid(charByBUID.m_eAlly, charByBUID.m_nStartPosIndex);
						if (battleGrid2 != null && flag2)
						{
							Vector3 center = battleGrid2.GetCenter();
							NrTSingleton<NkEffectManager>.Instance.AddCenterPosEffect(text2, charByBUID, center);
						}
					}
					Battle_SkilldescDlg battle_SkilldescDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BATTLE_SKILLDESC_DLG) as Battle_SkilldescDlg;
					if (battle_SkilldescDlg != null && !this.m_bLastAttacker)
					{
						battle_SkilldescDlg.SetBuffTextShowUp(this.m_CurrentBattleSkillBase.m_nSkillUnique, this);
					}
					this.m_OrderStartTime = Time.time;
					this.m_bCreateBullet = false;
					int weapontype = this.GetSoldierInfo().GetWeaponType();
					if (this.m_CurrentBattleSkillBase.m_nSkilNeedWeapon > 0)
					{
						weapontype = this.m_CurrentBattleSkillBase.m_nSkilNeedWeapon;
					}
					this.m_nTotalHitCount = this.GetCharKindInfo().GetHitAniCount(weapontype, (int)this.m_eAttackTypeAniEvent);
					if (this.m_nTotalHitCount <= 0)
					{
						this.m_nTotalHitCount = 1;
					}
					if (this.m_nTotalHitCount >= CharDefine.MAX_HIT_COUNT)
					{
						this.m_nTotalHitCount = CharDefine.MAX_HIT_COUNT;
					}
					this.m_nProcessHitCount = 0;
					this.m_bSkillWait = true;
				}
			}
			else
			{
				int weapontype2 = this.GetSoldierInfo().GetWeaponType();
				if (this.m_CurrentBattleSkillBase.m_nSkilNeedWeapon > 0)
				{
					weapontype2 = this.m_CurrentBattleSkillBase.m_nSkilNeedWeapon;
				}
				float num = 0f;
				if (!this.m_bCreateBullet)
				{
					if (!this.IsCharKindATB(4194304L))
					{
						BATTLE_POS_GRID battleGrid3 = Battle.BATTLE.GetBattleGrid(charByBUID.Ally, charByBUID.GetStartPosIndex());
						Vector3 v3Direction2 = Vector3.zero;
						if (battleGrid3 != null)
						{
							if (this.m_CurrentBattleSkillBase.m_nSkillGridType == 6)
							{
								v3Direction2 = battleGrid3.GetCenter();
							}
							else if (this.m_nAttackGrid != -1 && (int)this.m_nAttackGrid < battleGrid3.mListPos.Length)
							{
								v3Direction2 = battleGrid3.mListPos[(int)this.m_nAttackGrid];
								v3Direction2.y = charByBUID.GetCharPos().y;
							}
							else
							{
								v3Direction2 = charByBUID.GetCharPos();
							}
						}
						else
						{
							v3Direction2 = charByBUID.GetCenterPosition();
						}
						this.SetLookAt(v3Direction2, false);
					}
					num = this.GetCharKindInfo().GetAnimationEvent(weapontype2, (int)this.m_eAttackTypeAniEvent, NkBattleChar.m_nHitType[this.m_nProcessHitCount]);
				}
				float animationEvent = this.GetCharKindInfo().GetAnimationEvent(weapontype2, (int)this.m_eAttackTypeAniEvent, 0);
				this.m_fSkillAniEndtime = animationEvent;
				if (num == 0f)
				{
					num = 0.3f;
				}
				if (Mathf.Abs(Time.time - this.m_OrderStartTime) > num && !this.m_bCreateBullet)
				{
					if (this.m_CurrentBattleSkillBase.ChecJobTypeBullet())
					{
						if (this.m_CurrentBattleSkillBase.m_strSkillBulletCode != string.Empty)
						{
							NrTSingleton<NkBulletManager>.Instance.CreateBullet(this.m_CurrentBattleSkillBase.m_strSkillBulletCode, this, charByBUID, Time.time);
						}
					}
					this.m_nProcessHitCount++;
					if (this.m_nProcessHitCount >= this.m_nTotalHitCount)
					{
						this.m_bCreateBullet = true;
					}
				}
				if (this.m_bCreateBullet && Mathf.Abs(Time.time - this.m_OrderStartTime) > animationEvent && this.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
				{
					this.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
					this.SetRotate(this.GetGridRotate());
					this.m_bSkillWait = false;
					if (this.m_bLastAttacker)
					{
						Battle.BATTLE.BattleCamera.SetLastAttackCamera(this, false);
						this.m_bLastAttacker = false;
						this.SetAnimation(this.GetStayAni());
					}
				}
			}
			return;
		}
	}
}
