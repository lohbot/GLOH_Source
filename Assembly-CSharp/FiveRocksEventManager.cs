using GAME;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using TapjoyUnity;
using UnityEngine;

public class FiveRocksEventManager : NrTSingleton<FiveRocksEventManager>
{
	private FiveRocksEventListener m_Listener;

	private List<Scene.Type> m_StageInitStack = new List<Scene.Type>();

	private List<Scene.Type> m_StageFinalizeStack = new List<Scene.Type>();

	private FiveRocksEventManager()
	{
		GameObject gameObject = GameObject.Find("FiveRocks");
		if (gameObject != null)
		{
			this.m_Listener = gameObject.GetComponent<FiveRocksEventListener>();
			if (this.m_Listener == null)
			{
				gameObject.AddComponent<FiveRocksEventListener>();
				this.m_Listener = gameObject.GetComponent<FiveRocksEventListener>();
			}
		}
	}

	public void StageInitializeLog(Scene.Type CurrentScene)
	{
		if (!this.m_StageInitStack.Contains(CurrentScene))
		{
			this.m_StageInitStack.Add(CurrentScene);
			Tapjoy.TrackEvent("GameProgress", "STAGE", CurrentScene.ToString(), "StageInitialize", 0L);
		}
	}

	public void StageFinalizeLog(Scene.Type CurrentScene)
	{
		if (!this.m_StageFinalizeStack.Contains(CurrentScene))
		{
			this.m_StageFinalizeStack.Add(CurrentScene);
			Tapjoy.TrackEvent("GameProgress", "STAGE", CurrentScene.ToString(), "m_StageFinalizeStack", 0L);
		}
	}

	public void StageFunnelsLog(Scene.Type CurrentScene)
	{
		if (PlayerPrefs.GetInt(CurrentScene.ToString(), 0) == 0)
		{
			Tapjoy.TrackEvent("GameProgress", "Funnels", CurrentScene.ToString(), null, 0L);
			PlayerPrefs.SetInt(CurrentScene.ToString(), 1);
		}
	}

	public void QuestAccept(string strQuestUnique)
	{
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique != null)
		{
			Tapjoy.TrackEvent("Play", "Quest", questByQuestUnique.GetQuestTitle(), "QuestAccept", 0L);
		}
	}

	public void QuestComplete(string strQuestUnique)
	{
		CQuest questByQuestUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestByQuestUnique(strQuestUnique);
		if (questByQuestUnique != null)
		{
			Tapjoy.TrackEvent("Play", "Quest", questByQuestUnique.GetQuestTitle(), "QuestComplete", 0L);
			this.Placement("quest_end");
		}
	}

	public void BattleResult(eBATTLE_ROOMTYPE RoomType, float fBattleTime, int InjurySolCount)
	{
		string text = RoomType.ToString().Substring(17);
		Tapjoy.TrackEvent("Play", "Battle", text, text, "BattleTime", (long)fBattleTime, "InjurySol", (long)InjurySolCount, null, 0L);
	}

	public void BabelTowerParty(int nCount)
	{
		Tapjoy.TrackEvent("Play", "BabelTowerParty", (long)nCount);
	}

	public void SolCompose(int nDelSolCnt)
	{
		Tapjoy.TrackEvent("HERO", "SOL_COMPOSE", (long)nDelSolCnt);
	}

	public void SolSell(int nDelSolCnt)
	{
		Tapjoy.TrackEvent("HERO", "SOL_SELL", (long)nDelSolCnt);
	}

	public void SolRecruit(int nSolRecruit)
	{
		Tapjoy.TrackEvent("HERO", "SOL_RECRUIT", (long)nSolRecruit);
	}

	public void FriendDel()
	{
		Tapjoy.TrackEvent("Friend", "FriendDel", 1L);
	}

	public void FriendHelpHero()
	{
		Tapjoy.TrackEvent("Friend", "FriendHelpHero", 1L);
	}

	public void PurchaseItem(long ItemMallIndex)
	{
		ITEM_MALL_ITEM item = NrTSingleton<ItemMallItemManager>.Instance.GetItem(ItemMallIndex);
		string textFromItem = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(item.m_strTextKey);
		if (item.m_nMoneyType == 1)
		{
			Tapjoy.TrackPurchase(textFromItem, "USD", (double)item.m_fPrice, null);
		}
		Tapjoy.TrackEvent("Item", "Item", "Purchase", textFromItem, ((eITEMMALL_TYPE)item.m_nGroup).ToString(), 1L, "Price", (long)item.m_fPrice, null, 0L);
	}

	public void ItemMallOpen()
	{
		Tapjoy.TrackEvent("Item", "ItemMallOpen", 1L);
		this.Placement("store_enter");
	}

	public void Placement(string comment)
	{
		if (this.m_Listener == null)
		{
			Debug.LogError("_listener is null");
			return;
		}
		this.m_Listener.RequestPlacementContent(comment);
	}

	public void HeartsConsume(eHEARTS_CONSUME eConsume, long Value)
	{
		Tapjoy.TrackEvent("Hearts", "Hearts_Consume", "1", eConsume.ToString(), Value);
	}

	public void HeartsInflow(eHEARTS_INFLOW eInflow, long Value)
	{
		Tapjoy.TrackEvent("Hearts", "Hearts_Inflow", "1", eInflow.ToString(), Value);
	}
}
