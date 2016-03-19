using System;
using TsBundle;
using UnityEngine;

public class NmBattleGrid : MonoBehaviour
{
	private const float BLINK_TIME = 1f;

	private short mBUID = -1;

	private int mIdx;

	private eBATTLE_ALLY mAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private short mnStartPosIndex;

	public E_RENDER_MODE mMode = E_RENDER_MODE.ACTIVE;

	private bool m_bSelectBattleSkill;

	private NmGridCell mGridCell;

	private BoxCollider mCollider;

	private GameObject mTargetEffectRoot;

	private GameObject mGRID_ATTACK;

	private GameObject mGRID_NORMAL;

	private GameObject mGRID_ACTIVE;

	private GameObject mGRID_ACTIVE_SELECT;

	private GameObject mGrid_CHANGEPOS;

	private GameObject mGrid_CHANGEPOS_SELECT;

	private GameObject mGrid_SKILL;

	private GameObject mGrid_ADVANTAGE;

	private GameObject mCurrentGRID;

	private NkBattleChar mChar;

	public eBATTLE_ALLY ALLY
	{
		get
		{
			return this.mAlly;
		}
	}

	public short STARTPOS_INDEX
	{
		get
		{
			return this.mnStartPosIndex;
		}
	}

	public int INDEX
	{
		get
		{
			return this.mIdx;
		}
	}

	public short BUID
	{
		get
		{
			return this.mBUID;
		}
	}

	public E_RENDER_MODE GRID_MODE
	{
		get
		{
			return this.mMode;
		}
	}

	private NkBattleChar BattleChar
	{
		get
		{
			if (this.mChar == null && this.BUID != -1)
			{
				this.mChar = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.mBUID);
			}
			return this.mChar;
		}
	}

	public Vector3 GetCenter()
	{
		return this.mGridCell.GetCenterPos();
	}

	private bool IsEnableTurn()
	{
		return this.BattleChar != null && eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE == this.BattleChar.GetTurnState();
	}

	public bool IsSelectChar()
	{
		NkBattleChar currentSelectChar = Battle.BATTLE.GetCurrentSelectChar();
		return this.BattleChar != null && this.BattleChar == currentSelectChar;
	}

	public bool IsDisableMode()
	{
		if (this.BUID == -1)
		{
			if (this.mAlly == Battle.BATTLE.MyAlly)
			{
				if (Battle.BATTLE.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS)
				{
					return false;
				}
				if (Battle.BATTLE.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
				{
					return false;
				}
			}
			return true;
		}
		if (this.mAlly == Battle.BATTLE.MyAlly && this.mAlly == Battle.BATTLE.CurrentTurnAlly)
		{
			if (Battle.BATTLE.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS)
			{
				return false;
			}
			if (Battle.BATTLE.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
			{
				return false;
			}
			if (this.BattleChar.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE)
			{
				return true;
			}
			if ((Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT) && !this.BattleChar.MyChar)
			{
				return true;
			}
		}
		return false;
	}

	public void SetBUID(short BUID)
	{
		if (this.mBUID != BUID)
		{
			this.mBUID = BUID;
			this.SetMode(E_RENDER_MODE.NORMAL);
			if (BUID == -1)
			{
				this.mChar = null;
			}
		}
		else
		{
			this.mChar = null;
		}
	}

	public void SetSelectBattleSkill(bool bSkill)
	{
		this.m_bSelectBattleSkill = bSkill;
	}

	public static NmBattleGrid Create(eBATTLE_ALLY eAlly, short nStartPosIndex, short BUID, Vector3 Pos, int index)
	{
		string name = string.Format("GRID_{0}", index);
		GameObject gameObject = new GameObject(name);
		if (gameObject == null)
		{
			gameObject = new GameObject("NO_CELL");
		}
		Pos.y = NrTSingleton<NrTerrain>.Instance.SampleHeight(Pos);
		gameObject.transform.position = Pos;
		NmBattleGrid nmBattleGrid = gameObject.AddComponent<NmBattleGrid>();
		nmBattleGrid.Make(eAlly, nStartPosIndex, BUID, index);
		return nmBattleGrid;
	}

	private GameObject GetGridEffect(string Key, ref GameObject goRoot)
	{
		if (null == goRoot)
		{
			goRoot = new GameObject(Key);
			goRoot.transform.parent = base.transform;
			goRoot.transform.localPosition = Vector3.zero;
			NrTSingleton<NkEffectManager>.Instance.AddEffect(Key, goRoot);
		}
		return goRoot;
	}

	private void Make(eBATTLE_ALLY eAlly, short nStartPosIndex, short BUID, int index)
	{
		this.mAlly = eAlly;
		this.mnStartPosIndex = nStartPosIndex;
		this.mBUID = BUID;
		this.mIdx = index;
		this.SetBUID(this.mBUID);
		this.mGridCell = base.gameObject.AddComponent<NmGridCell>();
		this.mCollider = base.gameObject.AddComponent<BoxCollider>();
		this.mCollider.isTrigger = true;
		this.mCollider.size = new Vector3(8f, 1f, 8f);
		base.gameObject.layer = TsLayer.PC_DECORATION;
		this.SetMode(E_RENDER_MODE.NORMAL);
	}

	private void Update()
	{
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			if (E_RENDER_MODE.ATTACK > this.mMode)
			{
				this.CheckState();
			}
		}
		else
		{
			this.CheckState();
		}
	}

	public void CheckState()
	{
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT)
		{
			this.CheckStateNormal();
		}
		else if (Battle.BATTLE.IsEmotionSet && this.BattleChar != null)
		{
			if (this.BattleChar.MyChar)
			{
				this.SetMode(E_RENDER_MODE.NORMAL);
				return;
			}
			if (this.mAlly != Battle.BATTLE.MyAlly)
			{
				this.SetMode(E_RENDER_MODE.NORMAL);
				return;
			}
			this.SetMode(E_RENDER_MODE.DISABLE);
			return;
		}
		else if (this.BattleChar != null)
		{
			if (this.BattleChar.Ally != Battle.BATTLE.MyAlly)
			{
				if (E_RENDER_MODE.ATTACK > this.mMode)
				{
					this.CheckStateNormal();
				}
			}
			else if (!this.BattleChar.MyChar)
			{
				if (!Battle.BATTLE.IsEnableOrderTime)
				{
					if (Battle.BATTLE.BabelAdvantageCharUnique == this.BattleChar.GetCharUnique())
					{
						this.SetMode(E_RENDER_MODE.BABEL_ADVANTAGE);
						return;
					}
					this.CheckStateNormal();
				}
				else
				{
					this.CheckStateNormal();
				}
			}
			else if (Battle.BATTLE.IsEnableOrderTime)
			{
				this.CheckStateNormal();
			}
			else
			{
				this.SetMode(E_RENDER_MODE.DISABLE);
			}
		}
		else
		{
			this.CheckStateNormal();
		}
	}

	public void CheckStateNormal()
	{
		if (this.m_bSelectBattleSkill && this.mMode == E_RENDER_MODE.ATTACK)
		{
			return;
		}
		if (Battle.BATTLE == null)
		{
			return;
		}
		if ((Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER && Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY) || this.BUID == -1)
		{
			if (Battle.BATTLE.IsEmotionSet)
			{
				if (this.BattleChar != null)
				{
					if (this.BattleChar.MyChar)
					{
						this.SetMode(E_RENDER_MODE.NORMAL);
					}
					else if (this.mAlly != Battle.BATTLE.MyAlly)
					{
						this.SetMode(E_RENDER_MODE.NORMAL);
					}
					else
					{
						this.SetMode(E_RENDER_MODE.DISABLE);
					}
				}
			}
			else if (this.IsDisableMode())
			{
				this.SetMode(E_RENDER_MODE.DISABLE);
			}
			else if (this.IsSelectChar() && Battle.BATTLE.m_iBattleSkillIndex == -1)
			{
				this.SetMode(E_RENDER_MODE.ACTIVE_SELECT);
			}
			else if (this.IsSelectChar() && Battle.BATTLE.m_iBattleSkillIndex != -1)
			{
				this.SetMode(E_RENDER_MODE.SKILL);
			}
			else if (this.IsEnableTurn())
			{
				this.SetMode(E_RENDER_MODE.ACTIVE);
			}
			else if (this.mMode != E_RENDER_MODE.OVER)
			{
				this.SetMode(E_RENDER_MODE.NORMAL);
			}
			return;
		}
		if (Battle.BATTLE.MyAlly == this.ALLY)
		{
			this.SetMode(E_RENDER_MODE.ACTIVE_SELECT);
			return;
		}
		this.SetMode(E_RENDER_MODE.ATTACK);
	}

	public void SetMode(E_RENDER_MODE type)
	{
		bool flag = type != this.mMode;
		if (this.m_bSelectBattleSkill && type == E_RENDER_MODE.OVER)
		{
			return;
		}
		if (type == E_RENDER_MODE.DISABLE && base.gameObject.activeInHierarchy)
		{
			flag = true;
		}
		if (flag)
		{
			this.mMode = type;
			switch (this.mMode)
			{
			case E_RENDER_MODE.NORMAL:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_WHITE", ref this.mGRID_NORMAL);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.ACTIVE:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_WHITE", ref this.mGRID_ACTIVE);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.ACTIVE_SELECT:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_GREEN_SELECT", ref this.mGRID_ACTIVE_SELECT);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.ATTACK:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_RED", ref this.mGRID_ATTACK);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.CHANGEPOS:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_WHITE", ref this.mGrid_CHANGEPOS);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.CHANGEPOS_SELECT:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_GREEN", ref this.mGrid_CHANGEPOS_SELECT);
				base.gameObject.SetActive(true);
				break;
			case E_RENDER_MODE.SKILL:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_SKILL", ref this.mGrid_SKILL);
				base.gameObject.SetActive(true);
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "SKILLGRID", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
				break;
			case E_RENDER_MODE.DISABLE:
				base.gameObject.SetActive(false);
				break;
			case E_RENDER_MODE.BABEL_ADVANTAGE:
				this.mCurrentGRID = this.GetGridEffect("FX_GRID_BABELADVANTAGE", ref this.mGrid_ADVANTAGE);
				base.gameObject.SetActive(true);
				break;
			}
			bool bOn = this.mMode == E_RENDER_MODE.ATTACK || this.mMode == E_RENDER_MODE.OVER || this.mMode == E_RENDER_MODE.ACTIVE_SELECT;
			this.OnRimColor(bOn);
			this.ShowGrid(this.mCurrentGRID, true);
			this.OnOffTargetEffect(this.mMode == E_RENDER_MODE.ATTACK);
		}
	}

	private void ShowGrid(GameObject Target, bool IgnorTime)
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (null != child)
			{
				GameObject gameObject = child.gameObject;
				if (!(this.mTargetEffectRoot == gameObject))
				{
					if (gameObject != Target)
					{
						gameObject.SetActive(false);
					}
				}
			}
		}
		Target.SetActive(true);
	}

	private void OnRimColor(bool bOn)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			return;
		}
		if (this.BattleChar != null)
		{
			if (bOn)
			{
				this.BattleChar.MouseEvent_Enter();
			}
			else
			{
				this.BattleChar.MouseEvent_Exit();
			}
		}
	}

	private void OnOffTargetEffect(bool bOn)
	{
		if (bOn && this.mTargetEffectRoot == null)
		{
			this.mTargetEffectRoot = TBSUTIL.Attach("FX_ARROW_MARK", base.gameObject);
			NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_ARROW_MARK", this.mTargetEffectRoot);
		}
		if (null != this.mTargetEffectRoot)
		{
			Vector3 vector = new Vector3(0f, 1f, 0f);
			if (this.BattleChar != null && null != this.BattleChar.GetNameDummy())
			{
				vector += this.BattleChar.GetNameDummy().position;
			}
			this.mTargetEffectRoot.transform.position = vector;
			this.mTargetEffectRoot.SetActive(bOn);
		}
	}

	public void Delete()
	{
		if (null != this.mGridCell)
		{
			NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.mTargetEffectRoot);
			UnityEngine.Object.Destroy(this.mTargetEffectRoot);
			UnityEngine.Object.Destroy(base.gameObject);
			this.mGridCell = null;
			this.mChar = null;
		}
	}
}
