using GAME;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class GameGuideManager : NrTSingleton<GameGuideManager>
{
	private GameGuideInfo m_kCurrentGameGuideInfo;

	private List<GameGuideInfo> m_DefaultGuide = new List<GameGuideInfo>();

	private List<GameGuideInfo> m_GameGuide = new List<GameGuideInfo>();

	private Queue<GameGuideInfo> m_kReserveGuide = new Queue<GameGuideInfo>();

	private float checkTime = Time.realtimeSinceStartup;

	private float fpsCheckTime = Time.realtimeSinceStartup;

	private float totalCheckTime;

	private float delayTime = 20f;

	private AfterFunDelegate m_AfterFunDelegate;

	private bool m_bExecuteGuide;

	public float updateInterval = 0.5f;

	private double lastInterval;

	private float frames;

	private float fps;

	private float averageFps;

	private int count;

	private bool m_bWinBattle;

	private int m_nMonsterLevel;

	public bool WinBattle
	{
		get
		{
			return this.m_bWinBattle;
		}
		set
		{
			this.m_bWinBattle = value;
		}
	}

	public int MonsterLevel
	{
		get
		{
			return this.m_nMonsterLevel;
		}
		set
		{
			this.m_nMonsterLevel = value;
		}
	}

	public bool ExecuteGuide
	{
		get
		{
			return this.m_bExecuteGuide;
		}
		set
		{
			this.m_bExecuteGuide = value;
		}
	}

	private GameGuideManager()
	{
	}

	private void CheckFps()
	{
		this.frames += 1f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > this.lastInterval + (double)this.updateInterval)
		{
			this.fps = (float)((double)this.frames / ((double)realtimeSinceStartup - this.lastInterval));
			this.frames = 0f;
			this.lastInterval = (double)realtimeSinceStartup;
			this.averageFps += this.fps;
			this.count++;
		}
	}

	public void AddAfterFunDelegate(AfterFunDelegate del)
	{
		this.m_AfterFunDelegate = (AfterFunDelegate)Delegate.Combine(this.m_AfterFunDelegate, del);
	}

	public void ExcuteAfterFunDelegate()
	{
		if (this.m_AfterFunDelegate != null)
		{
			this.m_AfterFunDelegate();
			this.m_AfterFunDelegate = null;
			this.m_kCurrentGameGuideInfo = null;
		}
	}

	public GameGuideInfo GetGameGuideInfo()
	{
		return this.m_kCurrentGameGuideInfo;
	}

	public string GetDefaultGuideText()
	{
		int num = this.m_DefaultGuide.Count;
		if (0 >= num)
		{
			return string.Empty;
		}
		int index = UnityEngine.Random.Range(0, num);
		if (this.m_DefaultGuide[index] != null)
		{
			return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_DefaultGuide[index].m_strTalkKey);
		}
		return string.Empty;
	}

	public void RemoveEquipGuide()
	{
		if (this.m_kCurrentGameGuideInfo == null)
		{
			return;
		}
		if (!NrTSingleton<GameGuideManager>.Instance.ExecuteGuide && (this.m_kCurrentGameGuideInfo.m_eType == GameGuideType.EQUIP_ITEM || this.m_kCurrentGameGuideInfo.m_eType == GameGuideType.RECOMMEND_EQUIP))
		{
			NrTSingleton<GameGuideManager>.Instance.InitGameGuide();
			NoticeIconDlg.SetIcon(ICON_TYPE.GAMEGUIDE, false);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUBBLEGAMEGUIDE_DLG);
		}
	}

	public void RemoveGuide(GameGuideType Type)
	{
		if (this.m_kCurrentGameGuideInfo == null)
		{
			return;
		}
		if (!NrTSingleton<GameGuideManager>.Instance.ExecuteGuide && this.m_kCurrentGameGuideInfo.m_eType == Type)
		{
			NrTSingleton<GameGuideManager>.Instance.InitGameGuide();
			NoticeIconDlg.SetIcon(ICON_TYPE.GAMEGUIDE, false);
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUBBLEGAMEGUIDE_DLG);
		}
	}

	public void InitReserveGuide()
	{
		this.m_kReserveGuide.Clear();
	}

	public void InitGameGuide()
	{
		if (this.m_kCurrentGameGuideInfo != null)
		{
			this.m_kCurrentGameGuideInfo.InitData();
			this.m_kCurrentGameGuideInfo = null;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUBBLEGAMEGUIDE_DLG);
		this.m_bExecuteGuide = false;
	}

	public void ExcuteGameGuide()
	{
		if (this.m_kCurrentGameGuideInfo != null && this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.DEFAULT)
		{
			this.m_kCurrentGameGuideInfo.ExcuteGameGuide();
		}
		if (this.m_AfterFunDelegate == null && this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.EQUIP_ITEM)
		{
			this.m_kCurrentGameGuideInfo = null;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUBBLEGAMEGUIDE_DLG);
	}

	public string GetGameGuideText()
	{
		if (this.m_kCurrentGameGuideInfo == null)
		{
			return string.Empty;
		}
		if (this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.DEFAULT)
		{
			return this.m_kCurrentGameGuideInfo.GetGameGuideText();
		}
		return string.Empty;
	}

	public void AddDefaultGuid(GameGuideInfo gameGuideInfo)
	{
		this.m_DefaultGuide.Add(gameGuideInfo);
	}

	public static int CompareIndices(GameGuideInfo a, GameGuideInfo b)
	{
		return a.m_nPriority - b.m_nPriority;
	}

	public void AddGameGuide(GameGuideInfo gameGuideInfo)
	{
		GameGuideInfo gameGuideInfo2 = null;
		if (gameGuideInfo.m_eType == GameGuideType.EQUIP_ITEM)
		{
			gameGuideInfo2 = new GameGuideEquip();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.FRIEND_RECOMMEND1 || gameGuideInfo.m_eType == GameGuideType.FRIEND_RECOMMEND2 || gameGuideInfo.m_eType == GameGuideType.FRIEND_RECOMMEND3)
		{
			gameGuideInfo2 = new GameGuideAddFriend();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_EQUIP)
		{
			gameGuideInfo2 = new GameGuideRecommendEquip();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.EQUIP_SELL)
		{
			gameGuideInfo2 = new GameGuideEquipSell();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.CHECK_FPS)
		{
			gameGuideInfo2 = new GameGuideCheckFPS();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.PLUNDER_INFO)
		{
			gameGuideInfo2 = new GameGuidePlunderInfo();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.SUPPORT_GOLD)
		{
			gameGuideInfo2 = new GameGuideSupportGoldInfo();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.PLUNDER_REQUEST)
		{
			gameGuideInfo2 = new GameGuidePlunderRequest();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.SELL_ITEM)
		{
			gameGuideInfo2 = new GameGuideSellItem();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.ENCHANT_SOL)
		{
			gameGuideInfo2 = new GameGuideEnchantSol();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_REFORGE)
		{
			gameGuideInfo2 = new GameGuideRecommendReforge();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_HEALER)
		{
			gameGuideInfo2 = new GameGuideRecommendHealer();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_SKILL)
		{
			gameGuideInfo2 = new GameGuideRecommendSkill();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_MON)
		{
			gameGuideInfo2 = new GameGuideRecommendMon();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_SOL)
		{
			gameGuideInfo2 = new GameGuideRecommendSol();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.RECOMMEND_COMPOSE)
		{
			gameGuideInfo2 = new GameGuideRecommendCompose();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.GET_RUNESTONE)
		{
			gameGuideInfo2 = new GameGuideGetRunestone();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.GET_ORIHARCON)
		{
			gameGuideInfo2 = new GameGuideGetOriharcon();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.CERTIFY_EMAIL)
		{
			gameGuideInfo2 = new GameGuideCertifyEMAIL();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.MINE_ITEMGET)
		{
			gameGuideInfo2 = new GameGuideMineNotify();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.MINE_PLUNDER)
		{
			gameGuideInfo2 = new GameGuideMinePlunder();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.REVIEW)
		{
			gameGuideInfo2 = new GameGuideReview();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.COLOSSEUM)
		{
			gameGuideInfo2 = new GameGuideColosseum();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.FRIEND_LIMIT)
		{
			gameGuideInfo2 = new GameGuideFriendLimit();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.PURCHASE_RESTORE)
		{
			gameGuideInfo2 = new GameGuidePurchaseRestore();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.TREASURE_ALARM)
		{
			gameGuideInfo2 = new GameGuideTreasureAlarm();
		}
		else if (gameGuideInfo.m_eType == GameGuideType.EXPEDITION_ITEMGET)
		{
			gameGuideInfo2 = new GameGuideExpeditionNotify();
		}
		if (gameGuideInfo2 == null)
		{
			return;
		}
		gameGuideInfo2.Set(gameGuideInfo);
		this.m_GameGuide.Add(gameGuideInfo2);
		if (1 < this.m_GameGuide.Count)
		{
			this.m_GameGuide.Sort(new Comparison<GameGuideInfo>(GameGuideManager.CompareIndices));
		}
	}

	public GameGuideInfo GetGuide(GameGuideType eType)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return null;
		}
		foreach (GameGuideInfo current in this.m_GameGuide)
		{
			if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && eType == current.m_eType)
			{
				return current;
			}
		}
		return null;
	}

	public bool ContinueCheckRecommandEquip()
	{
		if (this.m_kCurrentGameGuideInfo == null)
		{
			return false;
		}
		if (this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.EQUIP_ITEM && this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.PURCHASE_RESTORE)
		{
			return false;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		if (this.m_kCurrentGameGuideInfo.GetMinLevel() <= myCharInfo.GetLevel() && this.m_kCurrentGameGuideInfo.GetMaxLevel() >= myCharInfo.GetLevel() && this.m_kCurrentGameGuideInfo.CheckGameGuideOnce())
		{
			return true;
		}
		this.InitGameGuide();
		return false;
	}

	public bool ContinueCheck(GameGuideType type)
	{
		if (this.m_kCurrentGameGuideInfo == null)
		{
			return false;
		}
		if (this.m_kCurrentGameGuideInfo.m_eType != type)
		{
			return false;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return false;
		}
		if (this.m_kCurrentGameGuideInfo.GetMinLevel() <= myCharInfo.GetLevel() && this.m_kCurrentGameGuideInfo.GetMaxLevel() >= myCharInfo.GetLevel() && this.m_kCurrentGameGuideInfo.CheckGameGuideOnce())
		{
			return true;
		}
		this.InitGameGuide();
		return false;
	}

	public void Update()
	{
		if (Scene.CurScene != Scene.Type.WORLD)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this.checkTime > this.delayTime)
		{
			foreach (GameGuideInfo current in this.m_GameGuide)
			{
				if (current.m_eCheck == GameGuideCheck.CYCLECAL)
				{
					if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current.m_eType)
					{
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						if (myCharInfo == null)
						{
							break;
						}
						if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && current.CheckGameGuide())
						{
							bool flag = false;
							foreach (GameGuideInfo current2 in this.m_kReserveGuide)
							{
								if (current2.m_eType == current.m_eType)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.m_kReserveGuide.Enqueue(current);
								break;
							}
						}
					}
				}
			}
			this.checkTime = Time.realtimeSinceStartup;
		}
		if (0f < this.totalCheckTime && !PlayerPrefs.HasKey("CheckFps") && Time.realtimeSinceStartup - this.totalCheckTime < 180f && TsQualityManager.Instance.CurrLevel != TsQualityManager.Level.LOWEST && !System_Option_Dlg.m_bSaveMode)
		{
			this.CheckFps();
			if (Time.realtimeSinceStartup - this.fpsCheckTime > 60f)
			{
				this.averageFps /= (float)this.count;
				if (this.averageFps <= 14f)
				{
					foreach (GameGuideInfo current3 in this.m_GameGuide)
					{
						if (current3.m_eType == GameGuideType.CHECK_FPS)
						{
							if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current3.m_eType)
							{
								this.m_kReserveGuide.Enqueue(current3);
								break;
							}
						}
					}
				}
				this.averageFps = 0f;
				this.count = 0;
				this.fpsCheckTime = Time.realtimeSinceStartup;
			}
		}
		if (0 < this.m_kReserveGuide.Count && this.m_kCurrentGameGuideInfo == null)
		{
			GameGuideInfo kCurrentGameGuideInfo = this.m_kReserveGuide.Dequeue();
			this.m_kCurrentGameGuideInfo = kCurrentGameGuideInfo;
			if (this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.DEFAULT)
			{
				NoticeIconDlg.SetIcon(ICON_TYPE.GAMEGUIDE, true);
				this.SetBubbleGameGuide();
			}
			else
			{
				this.m_kCurrentGameGuideInfo = null;
			}
		}
	}

	public void Update(GameGuideCheck checkType, GameGuideType type)
	{
		foreach (GameGuideInfo current in this.m_GameGuide)
		{
			if (current.m_eType == type)
			{
				if (current.m_eCheck == checkType)
				{
					if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current.m_eType)
					{
						NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
						if (myCharInfo == null)
						{
							break;
						}
						if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && current.CheckGameGuide())
						{
							bool flag = false;
							foreach (GameGuideInfo current2 in this.m_kReserveGuide)
							{
								if (current2.m_eType == current.m_eType)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.m_kReserveGuide.Enqueue(current);
							}
						}
					}
				}
			}
		}
	}

	public void Update(GameGuideCheck checkType)
	{
		this.fpsCheckTime = Time.realtimeSinceStartup;
		this.totalCheckTime = Time.realtimeSinceStartup;
		foreach (GameGuideInfo current in this.m_GameGuide)
		{
			if (current.m_eCheck == checkType)
			{
				if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current.m_eType)
				{
					NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
					if (myCharInfo == null)
					{
						break;
					}
					if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && current.CheckGameGuide())
					{
						bool flag = false;
						foreach (GameGuideInfo current2 in this.m_kReserveGuide)
						{
							if (current2.m_eType == current.m_eType)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.m_kReserveGuide.Enqueue(current);
						}
					}
				}
			}
		}
		this.checkTime = Time.realtimeSinceStartup;
	}

	public void Update(GameGuideType type)
	{
		foreach (GameGuideInfo current in this.m_GameGuide)
		{
			if (current.m_eType == type)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo == null)
				{
					break;
				}
				if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current.m_eType)
				{
					if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && current.CheckGameGuide())
					{
						bool flag = false;
						foreach (GameGuideInfo current2 in this.m_kReserveGuide)
						{
							if (current2.m_eType == current.m_eType)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.m_kReserveGuide.Enqueue(current);
						}
					}
				}
			}
		}
	}

	public void SetBubbleGameGuide()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			return;
		}
		if (this.m_kCurrentGameGuideInfo != null && this.m_kCurrentGameGuideInfo.m_eType != GameGuideType.DEFAULT)
		{
			if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE) == null)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BUBBLEGAMEGUIDE_DLG);
			}
			else if (!NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_LOADINGPAGE).Visible)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BUBBLEGAMEGUIDE_DLG);
			}
		}
	}

	public bool CheckGameGuide(GameGuideType type)
	{
		foreach (GameGuideInfo current in this.m_GameGuide)
		{
			if (current.m_eType == type)
			{
				NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
				if (myCharInfo == null)
				{
					break;
				}
				if (this.m_kCurrentGameGuideInfo == null || this.m_kCurrentGameGuideInfo.m_eType != current.m_eType)
				{
					if (current.GetMinLevel() <= myCharInfo.GetLevel() && current.GetMaxLevel() >= myCharInfo.GetLevel() && current.CheckGameGuideOnce())
					{
						bool flag = false;
						foreach (GameGuideInfo current2 in this.m_kReserveGuide)
						{
							if (current2.m_eType == current.m_eType)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.m_kReserveGuide.Enqueue(current);
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}
