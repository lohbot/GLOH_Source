using SOL_COMBINATION_DIRECTION_DLG;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolCombinationDirection_Dlg : Form
{
	public delegate void DirectionEndCallback();

	private readonly float _timeOutTime;

	private float _directionStartTime;

	private SolCombinationDirection_Dlg.DirectionEndCallback _directionEndCallback;

	private readonly int NOT_EXIST = -1;

	private int _solCombinationUniqeKey;

	private DirectionEffectSetter _directionEffectSetter;

	public Label _solCombinationTitle;

	public Label _solCombinationDescript;

	public SolCombinationDirection_Dlg()
	{
		this._timeOutTime = 5f;
	}

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_BattleCombination", G_ID.SOLCOMBINATION_DIRECTION_DLG, false);
		base.DonotDepthChange(UIPanelManager.TOPMOST_UI_DEPTH + 100f);
		this._directionStartTime = Time.realtimeSinceStartup;
		this._solCombinationUniqeKey = this.GetBattleCharSolCombination();
		this._directionEffectSetter = new DirectionEffectSetter(this, this._solCombinationUniqeKey);
		base.SetScreenCenter();
		NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.SetRecentBattleSolCombinationInfo(Battle.BATTLE.BattleRoomtype, this._solCombinationUniqeKey, 0);
		Debug.Log("SolCombinationDirectionUniqeKey : " + this._solCombinationUniqeKey);
	}

	public override void OnLoad()
	{
		this._directionEffectSetter.RequestSolCombinationDirectionEffect();
	}

	public override void SetComponent()
	{
		this._solCombinationTitle = (base.GetControl("Label_title") as Label);
		this._solCombinationTitle.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetTextTitleKeyByUniqeKey(this._solCombinationUniqeKey));
		this._solCombinationDescript = (base.GetControl("Label_text") as Label);
		this._solCombinationDescript.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetTextDetailKeyByUniqeKey(this._solCombinationUniqeKey));
	}

	public override void Update()
	{
		if (this.IsTimeOut())
		{
			Debug.LogError("ERROR, SolCombinationDirection_Dlg.cs, Update(). TimeOut");
			this.DirectionEndProccess();
			return;
		}
		if (!this.IsDirectionEnd())
		{
			return;
		}
		this.DirectionEndProccess();
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this._directionEffectSetter != null)
		{
			this._directionEffectSetter.OnClose();
		}
		this._directionEffectSetter = null;
		if (this._directionEndCallback != null)
		{
			this._directionEndCallback();
			this._directionEndCallback = null;
		}
	}

	public void SetDirectionEndCallback(SolCombinationDirection_Dlg.DirectionEndCallback directionEndCallback)
	{
		this._directionEndCallback = directionEndCallback;
	}

	private bool IsTimeOut()
	{
		return this._directionStartTime + this._timeOutTime < Time.realtimeSinceStartup;
	}

	private void DirectionEndProccess()
	{
		if (this._directionEndCallback != null)
		{
			this._directionEndCallback();
			this._directionEndCallback = null;
		}
		this.Close();
	}

	private bool IsDirectionEnd()
	{
		return !this.IsSolCombinationExist() || (this._directionEffectSetter != null && !(this._directionEffectSetter._directionAni == null) && !this._directionEffectSetter._directionAni.isPlaying);
	}

	private int GetBattleCharSolCombination()
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER && NrTSingleton<NkBabelMacroManager>.Instance.IsMacro())
		{
			int recentBattleSolCombinationInfo = NrTSingleton<SolCombination_RecentBattleStartInfoManager>.Instance.GetRecentBattleSolCombinationInfo(eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER, 0);
			if (!this.SolcombinationValidCheck(recentBattleSolCombinationInfo))
			{
				return NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCompleteCombinationTopUniqeKey(Battle.BATTLE.GetBattleCharOwnKindList());
			}
			return recentBattleSolCombinationInfo;
		}
		else
		{
			if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
			{
				return NrTSingleton<SolCombination_BatchSelectInfoManager>.Instance.GetUserSelectedUniqeKey(0);
			}
			return NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCompleteCombinationTopUniqeKey(Battle.BATTLE.GetBattleCharOwnKindList());
		}
	}

	private bool IsSolCombinationExist()
	{
		return this._solCombinationUniqeKey != this.NOT_EXIST && this._solCombinationUniqeKey >= 0;
	}

	private bool SolcombinationValidCheck(int uniqeKey)
	{
		Dictionary<int, string> completeCombinationDic = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetCompleteCombinationDic(Battle.BATTLE.GetBattleCharOwnKindList());
		if (completeCombinationDic == null || completeCombinationDic.Keys == null)
		{
			return false;
		}
		foreach (int current in completeCombinationDic.Keys)
		{
			if (current == uniqeKey)
			{
				return true;
			}
		}
		return false;
	}
}
