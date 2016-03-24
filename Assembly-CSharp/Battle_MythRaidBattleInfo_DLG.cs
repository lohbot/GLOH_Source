using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_MythRaidBattleInfo_DLG : Form
{
	private Label m_lbRoundNum;

	private Label m_lbRoundNum2;

	private Label m_lbDamageNum;

	private Label m_lBestDamageUserNameInParty;

	private Label m_lBestDamageInParty;

	private Label m_lBestDamageText;

	private DrawTexture m_BestDamageBG;

	private DrawTexture m_BestDamageLine;

	private bool m_bUsedRenewEffect;

	private long m_TotalMythRaidDamage;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "MythRaid/dlg_myth_battleinfo", G_ID.MYTHRAID_BATTLEINFO_DLG, false);
		form.AlwaysUpdate = true;
		form.TopMost = true;
		this.m_bUsedRenewEffect = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbRoundNum = (base.GetControl("LB_Round_Info") as Label);
		this.m_lbRoundNum2 = (base.GetControl("LB_Round_Info_2") as Label);
		this.m_lbDamageNum = (base.GetControl("LB_Damage_Info") as Label);
		this.m_lbRoundNum.SetText("1");
		this.m_lbRoundNum2.SetText("1");
		this.m_lbDamageNum.SetText("0");
		this.TextAniSetting(this.m_lbDamageNum);
		this.m_lBestDamageUserNameInParty = (base.GetControl("LB_BestPlayer") as Label);
		this.m_lBestDamageInParty = (base.GetControl("LB_BestDamage") as Label);
		this.m_lBestDamageText = (base.GetControl("LB_BestDamage_Text") as Label);
		this.m_BestDamageBG = (base.GetControl("DT_BestDamage_BG") as DrawTexture);
		this.m_BestDamageLine = (base.GetControl("DT_BestDamage_Line") as DrawTexture);
		this.m_lBestDamageUserNameInParty.SetText(string.Empty);
		this.m_lBestDamageInParty.SetText(string.Empty);
		this.TextAniSetting(this.m_lBestDamageInParty);
		this.BestPartyDamageUIOff();
		if (Battle.BATTLE.BattleRoomtype != eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
		{
			this.Hide();
		}
		else
		{
			switch (NrTSingleton<MythRaidManager>.Instance.GetRaidType())
			{
			case eMYTHRAID_DIFFICULTY.eMYTHRAID_EASY:
			case eMYTHRAID_DIFFICULTY.eMYTHRAID_NORMAL:
				base.SetShowLayer(0, false);
				base.SetShowLayer(1, false);
				base.SetShowLayer(2, true);
				break;
			case eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD:
				base.SetShowLayer(0, true);
				if (Battle.BabelPartyCount > 1)
				{
					base.SetShowLayer(1, true);
				}
				else
				{
					base.SetShowLayer(1, false);
				}
				base.SetShowLayer(2, false);
				break;
			}
		}
	}

	public void _SetDialogPos()
	{
		base.SetLocation(0f, 0f);
	}

	public void UpdateRoundInfo(int roundCount)
	{
		roundCount += 2;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2158"),
			"count",
			roundCount.ToString()
		});
		if (this.m_lbRoundNum.Visible)
		{
			this.m_lbRoundNum.SetText(empty);
		}
		else
		{
			this.m_lbRoundNum2.SetText(empty);
		}
	}

	public void UpdateDamageInfo(long addDamageData)
	{
		if (addDamageData > 0L)
		{
			string empty = string.Empty;
			this.m_TotalMythRaidDamage += addDamageData;
			string text = ANNUALIZED.Convert(this.m_TotalMythRaidDamage);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3133"),
				"damage",
				text
			});
			this.TextUpdateAndPlayAni(this.m_lbDamageNum, empty);
		}
		this.CheckTotalDamageRecordInWeek();
	}

	public void UpdatePartyBestDamge(string bestDamageUserName, long bestDamage)
	{
		if (!string.IsNullOrEmpty(bestDamageUserName))
		{
			this.m_lBestDamageUserNameInParty.SetText(bestDamageUserName);
		}
		this.TextUpdateAndPlayAni(this.m_lBestDamageInParty, bestDamage.ToString());
	}

	private void TextAniSetting(Label text)
	{
		if (text == null)
		{
			Debug.LogError("ERROR, Battle_MythRaidBattleInfo_DLG.cs, TextAniSetting(), text is Null");
			return;
		}
		UILabelStepByStepAni uILabelStepByStepAni = text.transform.gameObject.AddComponent<UILabelStepByStepAni>();
		uILabelStepByStepAni._loopTime = -1f;
		uILabelStepByStepAni._loopInterval = 0.01f;
		uILabelStepByStepAni._nextValueStopInterval = 0.5f;
		uILabelStepByStepAni._reverse = true;
		uILabelStepByStepAni._changePartUpdate = true;
		uILabelStepByStepAni._useComma = true;
	}

	private void TextUpdateAndPlayAni(Label text, string damage)
	{
		UILabelStepByStepAni component = text.GetComponent<UILabelStepByStepAni>();
		if (component == null)
		{
			Debug.LogError("ERROR, Battle_MythRaidBattleInfo_DLG.cs, TextUpdateAndPlayAni(), textAni is Null");
			return;
		}
		component.Clear();
		text.SetText(damage);
	}

	private void BestPartyDamageUIOff()
	{
		if (NrTSingleton<MythRaidManager>.Instance.GetRaidType() == eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD && 1 < Battle.BabelPartyCount)
		{
			return;
		}
		this.m_lBestDamageText.Visible = false;
		this.m_BestDamageBG.Visible = false;
		this.m_BestDamageLine.Visible = false;
		this.m_lBestDamageUserNameInParty.Visible = false;
		this.m_lBestDamageInParty.Visible = false;
	}

	private void CheckTotalDamageRecordInWeek()
	{
		if (this.m_bUsedRenewEffect)
		{
			return;
		}
		if (NrTSingleton<MythRaidManager>.Instance.GetRaidType() != eMYTHRAID_DIFFICULTY.eMYTHRAID_HARD)
		{
			return;
		}
		long weekBestDamage = this.GetWeekBestDamage();
		if (weekBestDamage <= 0L)
		{
			return;
		}
		if (this.m_TotalMythRaidDamage <= weekBestDamage)
		{
			return;
		}
		this.PlayNewRecordEffect();
		this.m_bUsedRenewEffect = true;
	}

	private long GetWeekBestDamage()
	{
		if (Battle.BabelPartyCount <= 1)
		{
			return NrTSingleton<MythRaidManager>.Instance.GetMyInfo().soloDamage;
		}
		return NrTSingleton<MythRaidManager>.Instance.GetMyInfo().partyDamage;
	}

	private void PlayNewRecordEffect()
	{
		if (this.m_bUsedRenewEffect)
		{
			return;
		}
		string str = string.Format("{0}", "ui/mythicraid/fx_myth_raid_new_record" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.NewRecordEffectPlay), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void NewRecordEffectPlay(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() == null || _item.GetSafeBundle().mainAsset == null)
		{
			Debug.LogError("ERROR, Battle_MythRaidBattleInfo_DLG.cs, NewRecordEffectPlay(), _item.GetSafeBundle() is Null ");
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (gameObject == null)
		{
			return;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 500f;
		gameObject2.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(gameObject2, GUICamera.UILayer);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref gameObject2);
		}
	}
}
