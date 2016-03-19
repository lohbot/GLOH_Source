using System;
using UnityEngine;

public class SoldierBatchGrid : MonoBehaviour
{
	public GameObject m_goGridChar;

	private long m_SolID;

	private long m_PersonID;

	private int m_CharKind;

	private byte m_ObjID;

	private int mIdx;

	private eBATTLE_ALLY mAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private short mnStartPosIndex;

	private SoldierBatchGridCell mGridCell;

	private BoxCollider mCollider;

	private GameObject mGRID_NORMAL;

	private GameObject mGRID_SELECT;

	private GameObject mGrid_Attack;

	private E_RENDER_MODE m_eMode = E_RENDER_MODE.DISABLE;

	private GameObject mCurrentGRID;

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

	public long SolID
	{
		get
		{
			return this.m_SolID;
		}
		set
		{
			this.m_SolID = value;
		}
	}

	public long PersonID
	{
		get
		{
			return this.m_PersonID;
		}
		set
		{
			this.m_PersonID = value;
		}
	}

	public int CharKind
	{
		get
		{
			return this.m_CharKind;
		}
		set
		{
			this.m_CharKind = value;
		}
	}

	public byte ObjID
	{
		get
		{
			return this.m_ObjID;
		}
		set
		{
			this.m_ObjID = value;
		}
	}

	public Vector3 GetCenter()
	{
		return this.mGridCell.GetCenterPos();
	}

	public static SoldierBatchGrid Create(eBATTLE_ALLY eAlly, short nStartPosIndex, Vector3 Pos, int index)
	{
		string name = string.Format("GRID_{0}", index);
		GameObject gameObject = new GameObject(name);
		if (gameObject == null)
		{
			gameObject = new GameObject("NO_CELL");
		}
		Pos.y = NrTSingleton<NrTerrain>.Instance.SampleHeight(Pos);
		gameObject.transform.position = Pos;
		SoldierBatchGrid soldierBatchGrid = gameObject.AddComponent<SoldierBatchGrid>();
		soldierBatchGrid.Make(eAlly, nStartPosIndex, index);
		return soldierBatchGrid;
	}

	public void GridShowHide(bool bShow)
	{
		if (base.gameObject.activeInHierarchy != bShow)
		{
			base.gameObject.SetActive(bShow);
		}
		if (bShow)
		{
			this.SetMODE(E_RENDER_MODE.NORMAL);
		}
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

	private void Make(eBATTLE_ALLY eAlly, short nStartPosIndex, int index)
	{
		this.mAlly = eAlly;
		this.mnStartPosIndex = nStartPosIndex;
		this.mIdx = index;
		this.mGridCell = base.gameObject.AddComponent<SoldierBatchGridCell>();
		this.mCollider = base.gameObject.AddComponent<BoxCollider>();
		this.mCollider.isTrigger = true;
		this.mCollider.size = new Vector3(6f, 1f, 6f);
		base.gameObject.layer = TsLayer.PC_DECORATION;
		base.gameObject.SetActive(true);
		this.SetMODE(E_RENDER_MODE.NORMAL);
	}

	public void SetMODE(E_RENDER_MODE type)
	{
		if (type == this.m_eMode)
		{
			return;
		}
		switch (type)
		{
		case E_RENDER_MODE.NORMAL:
			this.mCurrentGRID = this.GetGridEffect("FX_GRID_WHITE", ref this.mGRID_NORMAL);
			break;
		case E_RENDER_MODE.ACTIVE_SELECT:
			this.mCurrentGRID = this.GetGridEffect("FX_GRID_GREEN_SELECT", ref this.mGRID_SELECT);
			break;
		case E_RENDER_MODE.ATTACK:
			this.mCurrentGRID = this.GetGridEffect("FX_GRID_RED", ref this.mGrid_Attack);
			break;
		}
		this.m_eMode = type;
		this.ShowGrid(this.mCurrentGRID);
	}

	private void ShowGrid(GameObject Target)
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (null != child)
			{
				GameObject gameObject = child.gameObject;
				if (gameObject != Target)
				{
					gameObject.SetActive(false);
				}
			}
		}
		Target.SetActive(true);
	}

	private void Update()
	{
		if (SoldierBatch.SOLDIERBATCH == null)
		{
			return;
		}
		if (this.mAlly == eBATTLE_ALLY.eBATTLE_ALLY_1)
		{
			return;
		}
		if (SoldierBatch.SOLDIERBATCH.SelectGrid != this)
		{
			if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER)
			{
				if (this.SolID > 0L)
				{
					NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
					if (charPersonInfo.GetSoldierInfoFromSolID(this.SolID) == null)
					{
						this.SetMODE(E_RENDER_MODE.NORMAL);
					}
					else
					{
						this.SetMODE(E_RENDER_MODE.ATTACK);
					}
				}
				else
				{
					this.SetMODE(E_RENDER_MODE.NORMAL);
				}
			}
			else
			{
				this.SetMODE(E_RENDER_MODE.NORMAL);
			}
		}
	}
}
