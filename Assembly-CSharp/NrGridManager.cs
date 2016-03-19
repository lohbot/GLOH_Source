using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NrGridManager
{
	private const int EMPTY_TARTGET_INDEX = -1;

	private Dictionary<KeyValuePair<eBATTLE_ALLY, int>, Vector2> m_BattlePosGridSize = new Dictionary<KeyValuePair<eBATTLE_ALLY, int>, Vector2>();

	private Dictionary<KeyValuePair<eBATTLE_ALLY, int>, List<NmBattleGrid>> m_BattlePosGrid = new Dictionary<KeyValuePair<eBATTLE_ALLY, int>, List<NmBattleGrid>>();

	private GameObject mRoot;

	private int preTargetIndex = -1;

	private NmBattleGrid mMouseOver;

	public void Init()
	{
		if (TsPlatform.IsIPhone && (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MINE || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_EXPEDITION))
		{
			return;
		}
		GameObject parent = GameObject.Find(TsSceneSwitcher.ESceneType.BattleScene.ToString());
		if (this.mRoot == null)
		{
			this.mRoot = TBSUTIL.Attach("GridManager", parent);
		}
		eBATTLE_ALLY eBATTLE_ALLY = eBATTLE_ALLY.eBATTLE_ALLY_0;
		Dictionary<int, BATTLE_POS_GRID> battleGrid = Battle.BATTLE.GetBattleGrid(eBATTLE_ALLY);
		foreach (KeyValuePair<int, BATTLE_POS_GRID> current in battleGrid)
		{
			this.Insert(eBATTLE_ALLY, (short)current.Key, current.Value);
		}
		eBATTLE_ALLY = eBATTLE_ALLY.eBATTLE_ALLY_1;
		battleGrid = Battle.BATTLE.GetBattleGrid(eBATTLE_ALLY);
		foreach (KeyValuePair<int, BATTLE_POS_GRID> current2 in battleGrid)
		{
			this.Insert(eBATTLE_ALLY, (short)current2.Key, current2.Value);
		}
	}

	public void ShowHideGrid(bool bShow)
	{
		if (this.mRoot != null)
		{
			this.mRoot.SetActive(bShow);
		}
	}

	public void InitPreTargetIndex()
	{
		this.preTargetIndex = -1;
	}

	public void Delete()
	{
		foreach (List<NmBattleGrid> current in this.m_BattlePosGrid.Values)
		{
			foreach (NmBattleGrid current2 in current)
			{
				current2.Delete();
			}
			current.Clear();
		}
		this.m_BattlePosGrid.Clear();
		UnityEngine.Object.Destroy(this.mRoot);
	}

	private void Insert(eBATTLE_ALLY Ally, short nStartPosIndex, BATTLE_POS_GRID kPosGrid)
	{
		KeyValuePair<eBATTLE_ALLY, int> key = new KeyValuePair<eBATTLE_ALLY, int>(Ally, (int)nStartPosIndex);
		if (this.m_BattlePosGrid.ContainsKey(key))
		{
			this.UpdateGrid(Ally, nStartPosIndex, kPosGrid);
			return;
		}
		List<NmBattleGrid> list = new List<NmBattleGrid>();
		GameObject gameObject = TBSUTIL.Attach(string.Format("GRID{0}_{1}", (int)Ally, nStartPosIndex), this.mRoot);
		for (int i = 0; i < kPosGrid.mListPos.Length; i++)
		{
			Vector3 pos = kPosGrid.mListPos[i];
			short bUID = kPosGrid.m_veBUID[i];
			NmBattleGrid nmBattleGrid = NmBattleGrid.Create(Ally, nStartPosIndex, bUID, pos, i);
			nmBattleGrid.gameObject.transform.parent = gameObject.transform;
			list.Insert(list.Count, nmBattleGrid);
		}
		this.m_BattlePosGridSize.Add(key, new Vector2((float)kPosGrid.m_nWidthCount, (float)kPosGrid.m_nHeightCount));
		this.m_BattlePosGrid.Add(key, list);
	}

	private void UpdateGrid(eBATTLE_ALLY Ally, short nStartPosIndex, BATTLE_POS_GRID kPosGrid)
	{
		List<NmBattleGrid> list = this.GetBattleGridList(Ally, nStartPosIndex);
		if (list == null)
		{
			return;
		}
		if (list.Count != kPosGrid.m_veBUID.Length)
		{
			KeyValuePair<eBATTLE_ALLY, int> key = new KeyValuePair<eBATTLE_ALLY, int>(Ally, (int)nStartPosIndex);
			Transform child = NkUtil.GetChild(this.mRoot.transform, string.Format("GRID{0}_{1}", (int)Ally, nStartPosIndex));
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
			this.m_BattlePosGrid.Remove(key);
			this.m_BattlePosGridSize.Remove(key);
			list = new List<NmBattleGrid>();
			GameObject gameObject = TBSUTIL.Attach(string.Format("GRID{0}_{1}", (int)Ally, nStartPosIndex), this.mRoot);
			for (int i = 0; i < kPosGrid.mListPos.Length; i++)
			{
				Vector3 pos = kPosGrid.mListPos[i];
				short bUID = kPosGrid.m_veBUID[i];
				NmBattleGrid nmBattleGrid = NmBattleGrid.Create(Ally, nStartPosIndex, bUID, pos, i);
				nmBattleGrid.gameObject.transform.parent = gameObject.transform;
				list.Insert(list.Count, nmBattleGrid);
			}
			this.m_BattlePosGridSize.Add(key, new Vector2((float)kPosGrid.m_nWidthCount, (float)kPosGrid.m_nHeightCount));
			this.m_BattlePosGrid.Add(key, list);
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			NmBattleGrid nmBattleGrid2 = list[i];
			if (!(nmBattleGrid2 == null))
			{
				nmBattleGrid2.SetBUID(kPosGrid.m_veBUID[i]);
			}
		}
	}

	private Vector2 GetSize(eBATTLE_ALLY Ally, short nStartPosIndex)
	{
		KeyValuePair<eBATTLE_ALLY, int> key = new KeyValuePair<eBATTLE_ALLY, int>(Ally, (int)nStartPosIndex);
		if (this.m_BattlePosGridSize.ContainsKey(key))
		{
			return this.m_BattlePosGridSize[key];
		}
		return Vector2.zero;
	}

	private List<NmBattleGrid> GetBattleGridList(eBATTLE_ALLY Ally, short nStartPosIndex)
	{
		KeyValuePair<eBATTLE_ALLY, int> key = new KeyValuePair<eBATTLE_ALLY, int>(Ally, (int)nStartPosIndex);
		if (this.m_BattlePosGrid.ContainsKey(key))
		{
			return this.m_BattlePosGrid[key];
		}
		return null;
	}

	private NmBattleGrid GetBattleGrid(eBATTLE_ALLY Ally, short nStartPosIndex, int index)
	{
		List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
		if (battleGridList != null)
		{
			return battleGridList[index];
		}
		return null;
	}

	public void RemoveBUID(eBATTLE_ALLY Ally, short nStartPosIndex, short BUID)
	{
		List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
		if (battleGridList != null)
		{
			foreach (NmBattleGrid current in battleGridList)
			{
				if (current.BUID == BUID)
				{
					current.SetBUID(-1);
				}
			}
		}
	}

	public void InitAll()
	{
		this.InitbyAlly(eBATTLE_ALLY.eBATTLE_ALLY_0);
		this.InitbyAlly(eBATTLE_ALLY.eBATTLE_ALLY_1);
	}

	public void InitbyAlly(eBATTLE_ALLY Ally)
	{
		foreach (List<NmBattleGrid> current in this.m_BattlePosGrid.Values)
		{
			foreach (NmBattleGrid current2 in current)
			{
				if (current2.ALLY == Ally)
				{
					current2.SetSelectBattleSkill(false);
					current2.CheckState();
				}
			}
		}
		this.preTargetIndex = -1;
	}

	public void SetSelectBattleSkillGrid()
	{
		foreach (List<NmBattleGrid> current in this.m_BattlePosGrid.Values)
		{
			foreach (NmBattleGrid current2 in current)
			{
				current2.SetSelectBattleSkill(true);
			}
		}
	}

	public void BabelTower_Battle_Grid_Update()
	{
		foreach (List<NmBattleGrid> current in this.m_BattlePosGrid.Values)
		{
			foreach (NmBattleGrid current2 in current)
			{
				current2.CheckState();
			}
		}
	}

	public void ActiveAttack(NkBattleChar pkTarget)
	{
		if (pkTarget != null)
		{
			this.ActiveAttack(pkTarget.Ally, pkTarget.GetStartPosIndex(), (int)pkTarget.GetGridPos(), pkTarget);
		}
	}

	public void ActiveAttack(eBATTLE_ALLY Ally, short nStartPosIndex, int TargetIndex, NkBattleChar pkTarget)
	{
		NkBattleChar currentSelectChar = Battle.BATTLE.GetCurrentSelectChar();
		if (currentSelectChar != null)
		{
			if (this.preTargetIndex == TargetIndex)
			{
				return;
			}
			this.preTargetIndex = TargetIndex;
			short num = 0;
			if (pkTarget != null && currentSelectChar.CanAttack(pkTarget, (short)TargetIndex, Vector3.zero, ref num) == -1)
			{
				return;
			}
			NkSoldierInfo soldierInfo = currentSelectChar.GetSoldierInfo();
			E_ATTACK_GRID_TYPE aTTACKGRID = (E_ATTACK_GRID_TYPE)soldierInfo.GetAttackInfo().ATTACKGRID;
			Vector2 size = this.GetSize(Ally, nStartPosIndex);
			int xMax = (int)size.x;
			int yMax = (int)size.y;
			int[] index = BASE_BATTLE_GridData_Manager.GetInstance().GetIndex(aTTACKGRID, TargetIndex, xMax, yMax);
			if (index != null)
			{
				List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
				foreach (NmBattleGrid current in battleGridList)
				{
					if (current.BUID != -1)
					{
						current.SetMode(E_RENDER_MODE.NORMAL);
					}
					else
					{
						current.SetMode(E_RENDER_MODE.DISABLE);
					}
				}
				int[] array = index;
				for (int i = 0; i < array.Length; i++)
				{
					int index2 = array[i];
					if (NrGridData.IndexAccessAble(index2, xMax, yMax))
					{
						battleGridList[index2].SetMode(E_RENDER_MODE.ATTACK);
					}
				}
			}
		}
	}

	public void ActiveBattleCharGrid(eBATTLE_ALLY Ally, short nStartPosIndex)
	{
		List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
		foreach (NmBattleGrid current in battleGridList)
		{
			if (current.BUID != -1)
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(current.BUID);
				if (charByBUID != null && charByBUID.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
				{
					current.SetMode(E_RENDER_MODE.NORMAL);
				}
				else
				{
					current.SetMode(E_RENDER_MODE.DISABLE);
				}
			}
			else
			{
				current.SetMode(E_RENDER_MODE.DISABLE);
			}
		}
	}

	public void ActiveAttackGridCanTarget()
	{
		NkBattleChar currentSelectChar = Battle.BATTLE.GetCurrentSelectChar();
		if (currentSelectChar == null)
		{
			return;
		}
		eBATTLE_ALLY eBATTLE_ALLY = (currentSelectChar.Ally != eBATTLE_ALLY.eBATTLE_ALLY_0) ? eBATTLE_ALLY.eBATTLE_ALLY_0 : eBATTLE_ALLY.eBATTLE_ALLY_1;
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		short num = 0;
		NkBattleChar[] array = charArray;
		for (int i = 0; i < array.Length; i++)
		{
			NkBattleChar nkBattleChar = array[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && charArray[iD] != null && charArray[iD].Ally == eBATTLE_ALLY)
				{
					Vector2 size = this.GetSize(eBATTLE_ALLY, nkBattleChar.GetStartPosIndex());
					int xMax = (int)size.x;
					int yMax = (int)size.y;
					int gridPos = (int)nkBattleChar.GetGridPos();
					List<NmBattleGrid> battleGridList = this.GetBattleGridList(eBATTLE_ALLY, nkBattleChar.GetStartPosIndex());
					int[] gridIndexFromCharSize = nkBattleChar.GetGridIndexFromCharSize((short)gridPos);
					if (gridIndexFromCharSize != null && gridIndexFromCharSize.Length > 0)
					{
						int[] array2 = gridIndexFromCharSize;
						for (int j = 0; j < array2.Length; j++)
						{
							int num2 = array2[j];
							int num3 = currentSelectChar.CanAttack(charArray[iD], (short)num2, Vector3.zero, ref num);
							if ((num3 == 1 || num3 == -2) && NrGridData.IndexAccessAble(num2, xMax, yMax))
							{
								battleGridList[num2].SetMode(E_RENDER_MODE.ATTACK);
							}
						}
					}
				}
			}
		}
	}

	public void ActiveBattleSkillGrid(eBATTLE_ALLY Ally, short nStartPosIndex, int TargetIndex, int skillUnique)
	{
		NkBattleChar currentSelectChar = Battle.BATTLE.GetCurrentSelectChar();
		if (currentSelectChar != null)
		{
			if (this.preTargetIndex == TargetIndex)
			{
				return;
			}
			this.preTargetIndex = TargetIndex;
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
			if (battleSkillBase == null)
			{
				return;
			}
			E_ATTACK_GRID_TYPE nSkillGridType = (E_ATTACK_GRID_TYPE)battleSkillBase.m_nSkillGridType;
			Vector2 size = this.GetSize(Ally, nStartPosIndex);
			int xMax = (int)size.x;
			int yMax = (int)size.y;
			int[] index = BASE_BATTLE_GridData_Manager.GetInstance().GetIndex(nSkillGridType, TargetIndex, xMax, yMax);
			if (index != null)
			{
				List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
				foreach (NmBattleGrid current in battleGridList)
				{
					if (current.BUID != -1)
					{
						current.SetMode(E_RENDER_MODE.NORMAL);
					}
					else
					{
						current.SetMode(E_RENDER_MODE.DISABLE);
					}
				}
				int[] array = index;
				for (int i = 0; i < array.Length; i++)
				{
					int index2 = array[i];
					if (NrGridData.IndexAccessAble(index2, xMax, yMax))
					{
						battleGridList[index2].SetMode(E_RENDER_MODE.ATTACK);
					}
				}
			}
		}
	}

	public void ActiveBattleSkillGridCanTarget(NkBattleChar pkSendChar, BATTLESKILL_BASE BSkillBase, BATTLESKILL_DETAIL BSkillDetail)
	{
		if (pkSendChar == null || BSkillBase == null || BSkillDetail == null)
		{
			return;
		}
		eBATTLE_ALLY eBATTLE_ALLY = eBATTLE_ALLY.eBATTLE_ALLY_1;
		bool flag = false;
		switch (BSkillBase.m_nSkillTargetType)
		{
		case 1:
		case 2:
			eBATTLE_ALLY = ((pkSendChar.Ally != eBATTLE_ALLY.eBATTLE_ALLY_0) ? eBATTLE_ALLY.eBATTLE_ALLY_1 : eBATTLE_ALLY.eBATTLE_ALLY_0);
			break;
		case 3:
			eBATTLE_ALLY = ((pkSendChar.Ally != eBATTLE_ALLY.eBATTLE_ALLY_0) ? eBATTLE_ALLY.eBATTLE_ALLY_0 : eBATTLE_ALLY.eBATTLE_ALLY_1);
			break;
		case 4:
			flag = true;
			break;
		}
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		NkBattleChar[] array = charArray;
		for (int i = 0; i < array.Length; i++)
		{
			NkBattleChar nkBattleChar = array[i];
			if (nkBattleChar != null)
			{
				int iD = nkBattleChar.GetID();
				if (iD >= 0 && charArray[iD] != null)
				{
					bool flag2 = flag || charArray[iD].Ally == eBATTLE_ALLY;
					if (flag2)
					{
						List<NmBattleGrid> battleGridList = this.GetBattleGridList(charArray[iD].Ally, nkBattleChar.GetStartPosIndex());
						int gridPos = (int)nkBattleChar.GetGridPos();
						Vector2 size = this.GetSize(charArray[iD].Ally, nkBattleChar.GetStartPosIndex());
						int xMax = (int)size.x;
						int yMax = (int)size.y;
						int[] gridIndexFromCharSize = nkBattleChar.GetGridIndexFromCharSize((short)gridPos);
						int[] array2 = gridIndexFromCharSize;
						for (int j = 0; j < array2.Length; j++)
						{
							int num = array2[j];
							if (pkSendChar.CanBattleSkillForTargetGrid(charArray[iD], (short)num, BSkillBase, BSkillDetail))
							{
								if (NrGridData.IndexAccessAble(num, xMax, yMax))
								{
									battleGridList[num].SetMode(E_RENDER_MODE.ATTACK);
								}
							}
							else if (battleGridList[num].GRID_MODE != E_RENDER_MODE.ACTIVE_SELECT)
							{
								battleGridList[num].SetMode(E_RENDER_MODE.DISABLE);
							}
						}
					}
					else
					{
						List<NmBattleGrid> battleGridList2 = this.GetBattleGridList(charArray[iD].Ally, nkBattleChar.GetStartPosIndex());
						if (battleGridList2 != null)
						{
							int gridPos2 = (int)nkBattleChar.GetGridPos();
							if (battleGridList2[gridPos2].GRID_MODE != E_RENDER_MODE.ACTIVE_SELECT)
							{
								int[] gridIndexFromCharSize2 = nkBattleChar.GetGridIndexFromCharSize((short)gridPos2);
								List<NmBattleGrid> battleGridList = this.GetBattleGridList(charArray[iD].Ally, nkBattleChar.GetStartPosIndex());
								int[] array3 = gridIndexFromCharSize2;
								for (int k = 0; k < array3.Length; k++)
								{
									int index = array3[k];
									battleGridList[index].SetMode(E_RENDER_MODE.DISABLE);
								}
							}
						}
					}
				}
			}
		}
	}

	public void ActiveChangePos(eBATTLE_ALLY Ally, short nStartPosIndex, short nBUID, short nGridPos)
	{
		if (nBUID <= -1)
		{
			return;
		}
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID);
		if (charByBUID == null)
		{
			return;
		}
		int[] gridIndexFromCharSize = charByBUID.GetGridIndexFromCharSize(nGridPos);
		if (gridIndexFromCharSize == null)
		{
			return;
		}
		Vector2 size = this.GetSize(Ally, nStartPosIndex);
		int xMax = (int)size.x;
		int yMax = (int)size.y;
		List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
		foreach (NmBattleGrid current in battleGridList)
		{
			if (current.GRID_MODE != E_RENDER_MODE.ACTIVE_SELECT)
			{
				current.SetMode(E_RENDER_MODE.NORMAL);
			}
		}
		int[] array = gridIndexFromCharSize;
		for (int i = 0; i < array.Length; i++)
		{
			int index = array[i];
			if (NrGridData.IndexAccessAble(index, xMax, yMax))
			{
				battleGridList[index].SetMode(E_RENDER_MODE.CHANGEPOS_SELECT);
			}
		}
	}

	public void SetOver(NmBattleGrid kOverGrid)
	{
		if (Battle.BATTLE == null)
		{
			return;
		}
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
		{
			return;
		}
		if (null != this.mMouseOver && this.mMouseOver != kOverGrid)
		{
			if (!this.mMouseOver.IsSelectChar())
			{
				this.mMouseOver.SetMode(E_RENDER_MODE.NORMAL);
			}
			this.mMouseOver.CheckState();
		}
		if (kOverGrid && kOverGrid.ALLY == Battle.BATTLE.MyAlly)
		{
			this.mMouseOver = kOverGrid;
			if (this.mMouseOver.IsSelectChar() && Battle.BATTLE.m_iBattleSkillIndex == -1)
			{
				this.mMouseOver.SetMode(E_RENDER_MODE.ACTIVE_SELECT);
			}
			else if (this.mMouseOver.IsSelectChar() && Battle.BATTLE.m_iBattleSkillIndex != -1)
			{
				this.mMouseOver.SetMode(E_RENDER_MODE.SKILL);
			}
			else
			{
				this.mMouseOver.SetMode(E_RENDER_MODE.OVER);
			}
		}
	}

	public void SetGridChangePosMode(bool bOn, eBATTLE_ALLY Ally, short nStartPosIndex)
	{
		if (bOn)
		{
			List<NmBattleGrid> battleGridList = this.GetBattleGridList(Ally, nStartPosIndex);
			if (battleGridList == null)
			{
				return;
			}
			for (int i = 0; i < battleGridList.Count; i++)
			{
				NmBattleGrid nmBattleGrid = battleGridList[i];
				if (!(nmBattleGrid == null))
				{
					if (nmBattleGrid.mMode == E_RENDER_MODE.DISABLE)
					{
						nmBattleGrid.SetMode(E_RENDER_MODE.CHANGEPOS);
					}
				}
			}
		}
		else
		{
			List<NmBattleGrid> battleGridList2 = this.GetBattleGridList(Ally, nStartPosIndex);
			if (battleGridList2 == null)
			{
				return;
			}
			for (int j = 0; j < battleGridList2.Count; j++)
			{
				NmBattleGrid nmBattleGrid2 = battleGridList2[j];
				if (!(nmBattleGrid2 == null))
				{
					if (nmBattleGrid2.mMode == E_RENDER_MODE.CHANGEPOS)
					{
						nmBattleGrid2.SetMode(E_RENDER_MODE.DISABLE);
					}
				}
			}
			if (Battle.BATTLE.m_iBattleSkillIndex == -1)
			{
				this.ActiveAttackGridCanTarget();
			}
		}
	}
}
