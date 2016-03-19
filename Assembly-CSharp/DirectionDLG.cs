using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class DirectionDLG : Form
{
	public enum eDIRECTIONTYPE
	{
		eDIRECTION_COMMUNITY,
		eDIRECTION_BABEL,
		eDIRECTION_PLUNDER,
		eDIRECTION_ITEMSKILL,
		eDIRECTION_REDUCE,
		eDIRECTION_MINESEARCH,
		eDIRECTION_MINETUTORIAL,
		eDIRECTION_EXPLORATION
	}

	private const string PLAYERPREF_COMMUNITY = "Community DLG Effect";

	private const string PLAYERPREF_BABEL = "BabelTowerMainDLG Effect";

	private const string PLAYERPREF_PLUNDER = "PlunderMainDLG Effect";

	private const string PLAYERPREF_ITEMSKILL = "ItemSkill Effect";

	private const string PLAYERPREF_REDUCE = "Reduce Effect";

	private Button m_btSkipDirectionDlg;

	private Label m_lSkip;

	private GameObject m_EffectDirection;

	private bool m_bActionShowDirection;

	private float fStartTime_ShowEffect;

	private float m_EffectShowTime = 32f;

	private bool IsCheckShowDirection;

	private short m_nBabel_FloorType;

	private DirectionDLG.eDIRECTIONTYPE m_direction_type;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DLG_Direction", G_ID.DLG_DIRECTION, true);
		base.TopMost = true;
		base.ShowBlackBG(1f);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btSkipDirectionDlg = (base.GetControl("Button_Button0") as Button);
		Button expr_1C = this.m_btSkipDirectionDlg;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.BtnClickClose));
		this.m_btSkipDirectionDlg.Visible = false;
		this.m_lSkip = (base.GetControl("Label_SKIP") as Label);
		this.m_lSkip.Visible = true;
	}

	public override void InitData()
	{
	}

	public override void OnClose()
	{
		if (null != this.m_EffectDirection)
		{
			UnityEngine.Object.Destroy(this.m_EffectDirection);
		}
		if (this.m_direction_type == DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL)
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BABELTOWERMAIN_DLG))
			{
				BabelTowerMainDlg babelTowerMainDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.BABELTOWERMAIN_DLG) as BabelTowerMainDlg;
				if (babelTowerMainDlg != null)
				{
					babelTowerMainDlg.FloorType = this.m_nBabel_FloorType;
					babelTowerMainDlg.ShowList();
				}
			}
		}
		else if (this.m_direction_type == DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINETUTORIAL)
		{
			MineTutorialStepDlg mineTutorialStepDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINE_TUTORIAL_STEP_DLG) as MineTutorialStepDlg;
			if (mineTutorialStepDlg != null)
			{
				mineTutorialStepDlg.SetStep(1L);
			}
			UIDataManager.MuteSound(false);
		}
		else if (this.m_direction_type == DirectionDLG.eDIRECTIONTYPE.eDIRECTION_EXPLORATION)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.EXPLORATION_DLG);
		}
		else
		{
			UIDataManager.MuteSound(false);
		}
	}

	private void CloseAction()
	{
		if (null != this.m_EffectDirection)
		{
			UnityEngine.Object.Destroy(this.m_EffectDirection);
		}
		this.fStartTime_ShowEffect = 0f;
		this.m_bActionShowDirection = false;
		this.Close();
	}

	public override void Update()
	{
		if (this.m_bActionShowDirection && Time.realtimeSinceStartup - this.fStartTime_ShowEffect > this.m_EffectShowTime)
		{
			if (null != this.m_EffectDirection)
			{
				UnityEngine.Object.Destroy(this.m_EffectDirection);
			}
			this.fStartTime_ShowEffect = 0f;
			this.m_bActionShowDirection = false;
			this.Close();
		}
		if (0f < this.fStartTime_ShowEffect && Time.realtimeSinceStartup - this.fStartTime_ShowEffect > 1f && !this.m_btSkipDirectionDlg.Visible)
		{
			this.m_btSkipDirectionDlg.Visible = true;
		}
		base.Update();
	}

	public void SetDirection(DirectionDLG.eDIRECTIONTYPE type)
	{
		switch (this.m_direction_type)
		{
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY:
			PlayerPrefs.SetInt("Community DLG Effect", 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL:
			PlayerPrefs.SetInt("BabelTowerMainDLG Effect", 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER:
			PlayerPrefs.SetInt("PlunderMainDLG Effect", 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_ITEMSKILL:
			PlayerPrefs.SetInt("ItemSkill Effect", 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_REDUCE:
			PlayerPrefs.SetInt("Reduce Effect", 1);
			break;
		}
	}

	public bool CheckDirection()
	{
		switch (this.m_direction_type)
		{
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY:
			this.IsCheckShowDirection = (PlayerPrefs.GetInt("Community DLG Effect") != 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL:
			this.IsCheckShowDirection = true;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER:
			this.IsCheckShowDirection = (PlayerPrefs.GetInt("PlunderMainDLG Effect") != 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_ITEMSKILL:
			this.IsCheckShowDirection = (PlayerPrefs.GetInt("ItemSkill Effect") != 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_REDUCE:
			this.IsCheckShowDirection = (PlayerPrefs.GetInt("Reduce Effect") != 1);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINESEARCH:
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINETUTORIAL:
			this.IsCheckShowDirection = true;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_EXPLORATION:
			this.IsCheckShowDirection = true;
			break;
		}
		return this.IsCheckShowDirection;
	}

	public void ReviewDirection(DirectionDLG.eDIRECTIONTYPE type)
	{
		switch (type)
		{
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY:
			PlayerPrefs.SetInt("Community DLG Effect", 0);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER:
			PlayerPrefs.SetInt("PlunderMainDLG Effect", 0);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_ITEMSKILL:
			PlayerPrefs.SetInt("ItemSkill Effect", 0);
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_REDUCE:
			PlayerPrefs.SetInt("Reduce Effect", 0);
			break;
		}
		this.ShowDirection(type, 0);
	}

	public void ShowDirection(DirectionDLG.eDIRECTIONTYPE type, int sub_data = 0)
	{
		this.m_direction_type = type;
		if (!this.CheckDirection())
		{
			this.Close();
			return;
		}
		UIDataManager.MuteSound(true);
		string arg = string.Empty;
		this.SetDirection(this.m_direction_type);
		switch (this.m_direction_type)
		{
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_COMMUNITY:
			arg = string.Format("{0}", "UI/Etc/fx_helpfriend" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 32f;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_BABEL:
			this.m_nBabel_FloorType = (short)sub_data;
			if (PlayerPrefs.GetInt("BabelTowerMainDLG Effect") == 0)
			{
				arg = "Effect/Instant/fx_direct_chaostower" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 17f;
			}
			else
			{
				arg = "Effect/Instant/fx_towerloading" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 2.5f;
			}
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_PLUNDER:
		{
			arg = string.Format("{0}", "UI/Etc/fx_herobattle" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 32f;
			Vector3 position = new Vector3(0f, 0f, 0f);
			position.z = 200f;
			position.x = this.m_lSkip.transform.position.x;
			position.y = this.m_lSkip.transform.position.y;
			this.m_lSkip.transform.position = position;
			break;
		}
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_ITEMSKILL:
			arg = string.Format("{0}", "UI/item/fx_direct_improvemagic" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 22.2f;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_REDUCE:
			arg = string.Format("{0}{1}{2}", "UI/item/", "fx_direct_improveweapon", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 38f;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINESEARCH:
		{
			if (sub_data == 1)
			{
				arg = "UI/Mine/fx_direct_forestroad" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 5f;
			}
			else if (sub_data == 2)
			{
				arg = "UI/Mine/fx_direct_iceroad" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 5f;
			}
			else if (sub_data == 3)
			{
				arg = "UI/Mine/fx_direct_goldroad" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 5f;
			}
			else if (sub_data == 4)
			{
				arg = "UI/Mine/fx_direct_goldroad" + NrTSingleton<UIDataManager>.Instance.AddFilePath;
				this.m_EffectShowTime = 5f;
			}
			Vector3 position2 = new Vector3(0f, 0f, 0f);
			position2.z = 200f;
			position2.x = this.m_lSkip.transform.position.x;
			position2.y = this.m_lSkip.transform.position.y;
			this.m_lSkip.transform.position = position2;
			break;
		}
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_MINETUTORIAL:
			arg = string.Format("{0}{1}{2}", "UI/Mine/", "fx_direct_minewar", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 15f;
			break;
		case DirectionDLG.eDIRECTIONTYPE.eDIRECTION_EXPLORATION:
			arg = string.Format("{0}{1}{2}", "UI/Exploration/", "fx_direct_exploreloding", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			this.m_EffectShowTime = 3f;
			break;
		}
		string str = string.Format("{0}", arg);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SetActionDirection), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void SetActionDirection(WWWItem _item, object _param)
	{
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_EffectDirection = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.m_EffectDirection.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_EffectDirection, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.m_EffectDirection);
				this.fStartTime_ShowEffect = Time.realtimeSinceStartup;
				this.m_bActionShowDirection = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_EffectDirection);
				}
			}
		}
	}

	public void BtnClickClose(IUIObject obj)
	{
		if (Time.realtimeSinceStartup - this.fStartTime_ShowEffect > 1f)
		{
			this.Close();
		}
	}
}
