using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_CountDlg : Form
{
	private Label m_lbCount;

	private DrawTexture m_dwBG;

	private DrawTexture m_dtPreviewBG;

	private Label m_lbPreview;

	private float m_TurnSwapTime;

	private float m_TurnTime;

	private float m_TurnTimeEnd;

	private bool m_PlayerTurn = true;

	private bool m_bStop;

	private float m_fTurnStopStartTime;

	private float m_fBattleContinueEffectUV = 0.2f;

	private int m_nCurrentContinueCount;

	private string m_strText = string.Empty;

	private GameObject m_goCountDown;

	private GameObject m_EffectTurnMyAlly;

	private GameObject m_EffectTurnEnemyAlly;

	private GameObject m_EffectBattleChallenge;

	private GameObject m_EffectTreasureMonsterDie;

	private GameObject m_EffectHiddenTreasureMonsterDie;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Count", G_ID.BATTLE_COUNT_DLG, false);
		if (base.InteractivePanel != null)
		{
			base.Draggable = false;
			base.AlwaysUpdate = true;
		}
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbCount = (base.GetControl("Label_count") as Label);
		this.m_dwBG = (base.GetControl("bg") as DrawTexture);
		if (this.m_dwBG != null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_COUNTDOWN", this.m_dwBG, this.m_dwBG.GetSize());
			this.m_dwBG.AddGameObjectDelegate(new EZGameObjectDelegate(this.effectdownload));
		}
		this.m_dtPreviewBG = (base.GetControl("DT_PreviewBG") as DrawTexture);
		this.m_lbPreview = (base.GetControl("Label_Preview") as Label);
		this.m_dtPreviewBG.Visible = false;
		this.m_lbPreview.Visible = false;
		this.m_strText = "mythic_raid_nextround";
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strText))
		{
			string str = string.Format("{0}", "UI/mythicraid/" + this.m_strText + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBundleTextureImage), this.m_strText);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		this.SetVisibleFlag(false);
		if (Battle.BATTLE != null && Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			this.SetControl_PreviewHero();
		}
	}

	public override void InitData()
	{
		base.InitData();
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		if (this.m_EffectTurnMyAlly != null)
		{
			this.m_EffectTurnMyAlly.transform.position = base.GetEffectUIPos(screenPos);
		}
		if (this.m_EffectTurnEnemyAlly != null)
		{
			this.m_EffectTurnEnemyAlly.transform.position = base.GetEffectUIPos(screenPos);
		}
		if (this.m_EffectBattleChallenge != null)
		{
			this.m_EffectBattleChallenge.transform.position = base.GetEffectUIPos(screenPos);
		}
		if (this.m_EffectTreasureMonsterDie != null)
		{
			this.m_EffectTreasureMonsterDie.transform.position = base.GetEffectUIPos(screenPos);
		}
		if (this.m_EffectHiddenTreasureMonsterDie != null)
		{
			this.m_EffectHiddenTreasureMonsterDie.transform.position = base.GetEffectUIPos(screenPos);
		}
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public override void OnClose()
	{
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_EffectTurnMyAlly);
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_EffectTurnEnemyAlly);
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_EffectBattleChallenge);
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_EffectTreasureMonsterDie);
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(this.m_EffectHiddenTreasureMonsterDie);
		UnityEngine.Object.Destroy(this.m_EffectTurnMyAlly);
		UnityEngine.Object.Destroy(this.m_EffectTurnEnemyAlly);
		UnityEngine.Object.Destroy(this.m_EffectBattleChallenge);
		UnityEngine.Object.Destroy(this.m_EffectTreasureMonsterDie);
		UnityEngine.Object.Destroy(this.m_EffectHiddenTreasureMonsterDie);
		if (this.m_goCountDown != null)
		{
			UnityEngine.Object.Destroy(this.m_goCountDown);
			this.m_goCountDown = null;
		}
		if (NrTSingleton<FormsManager>.Instance.IsPopUPDlgNotExist(base.WindowID))
		{
			NrTSingleton<UIImageBundleManager>.Instance.DeleteTexture(this.m_strText);
		}
	}

	public void ShowTurnCount(bool _PlayerTurn)
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_COLOSSEUM)
		{
			this.SetVisibleFlag(true);
		}
		else
		{
			if (this.m_TurnTimeEnd - Time.realtimeSinceStartup > 21f)
			{
				this.SetVisibleFlag(false);
				return;
			}
			this.SetVisibleFlag(true);
		}
		long count = 0L;
		if (this.m_TurnTimeEnd >= Time.realtimeSinceStartup)
		{
			float num = Time.realtimeSinceStartup - this.m_TurnSwapTime;
			count = (long)(this.m_TurnTime - num);
		}
		this.SetTurnNumber(_PlayerTurn, count);
	}

	private void SetTurnNumber(bool _PlayerTurn, long Count)
	{
		string szColorNum = (!_PlayerTurn) ? "1305" : "2002";
		string text = NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum) + Count.ToString();
		this.m_lbCount.SetText(text);
		if (Count <= 10L)
		{
			if (this.m_goCountDown != null && !this.m_goCountDown.activeInHierarchy)
			{
				this.m_goCountDown.SetActive(true);
			}
		}
		else if (this.m_goCountDown != null && this.m_goCountDown.activeInHierarchy)
		{
			this.m_goCountDown.SetActive(false);
		}
	}

	public void SwapTurn(bool _PlayerTurn, float _Time)
	{
		this.m_PlayerTurn = _PlayerTurn;
		this.m_TurnTime = _Time;
		this.m_TurnTimeEnd = Time.realtimeSinceStartup + this.m_TurnTime;
		this.m_TurnSwapTime = Time.realtimeSinceStartup;
		this.SetVisibleFlag(false);
		Vector2 screenPos = new Vector2(GUICamera.width / 2f, GUICamera.height / 2f);
		if (EffectDefine.IsValidParent(this.m_EffectTurnMyAlly) && this.m_PlayerTurn)
		{
			this.m_EffectTurnMyAlly = NrTSingleton<NkEffectManager>.Instance.CreateEffectUI("FX_PLAYER_PHASE", screenPos, new NkEffectUnit.DeleteCallBack(this.TurnEffectDeleteCallBack));
		}
		if (EffectDefine.IsValidParent(this.m_EffectTurnEnemyAlly) && !this.m_PlayerTurn)
		{
			this.m_EffectTurnEnemyAlly = NrTSingleton<NkEffectManager>.Instance.CreateEffectUI("FX_ENEMY_PHASE", screenPos, new NkEffectUnit.DeleteCallBack(this.TurnEffectDeleteCallBack));
		}
		GameObject gameObject;
		GameObject gameObject2;
		if (this.m_PlayerTurn)
		{
			gameObject = this.m_EffectTurnMyAlly;
			gameObject2 = this.m_EffectTurnEnemyAlly;
		}
		else
		{
			gameObject = this.m_EffectTurnEnemyAlly;
			gameObject2 = this.m_EffectTurnMyAlly;
		}
		if (null != gameObject)
		{
			Vector2 screenPos2 = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			gameObject.transform.position = base.GetEffectUIPos(screenPos2);
			gameObject.layer = TsLayer.GUI;
			gameObject.SetActive(true);
			Animation componentInChildren = gameObject.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				componentInChildren.cullingType = AnimationCullingType.AlwaysAnimate;
			}
		}
		if (null != gameObject2)
		{
			Vector2 screenPos3 = new Vector2((float)(Screen.width / 2), (float)(-(float)Screen.height / 2));
			gameObject2.transform.position = base.GetEffectUIPos(screenPos3);
			gameObject2.layer = TsLayer.GUI;
			gameObject2.SetActive(false);
		}
		NrSound.ImmedatePlay("UI_SFX", "BATTLE", (!_PlayerTurn) ? "YOURTURN" : "MYTURN");
	}

	public void StopTurn(bool bStop)
	{
		if (this.m_bStop == bStop)
		{
			return;
		}
		if (bStop)
		{
			this.m_fTurnStopStartTime = Time.realtimeSinceStartup;
		}
		else
		{
			this.m_TurnTimeEnd += Time.realtimeSinceStartup - this.m_fTurnStopStartTime;
			this.m_TurnSwapTime += Time.realtimeSinceStartup - this.m_fTurnStopStartTime;
		}
		this.m_bStop = bStop;
	}

	public void SetVisibleFlag(bool bShow)
	{
		if (Battle.BATTLE != null && Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			return;
		}
		if (base.Visible != bShow)
		{
			base.Visible = bShow;
		}
		if (bShow)
		{
			this.InitData();
		}
	}

	public override void Update()
	{
		if (this.m_bStop)
		{
			return;
		}
		Battle bATTLE = Battle.BATTLE;
		if (bATTLE == null)
		{
			return;
		}
		if (bATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PREVIEW)
		{
			return;
		}
		this.ShowTurnCount(this.m_PlayerTurn);
	}

	public void TurnEffectDeleteCallBack()
	{
		if (this.m_EffectTurnEnemyAlly != null)
		{
			this.m_EffectTurnEnemyAlly.SetActive(false);
		}
		if (this.m_EffectTurnMyAlly)
		{
			this.m_EffectTurnMyAlly.SetActive(false);
		}
	}

	public void SetChallengeCount(int nCount, bool bBoss, int[] nMonsterKind)
	{
		Vector2 screenPos = new Vector2(GUICamera.width / 2f, GUICamera.height / 2f);
		if (EffectDefine.IsValidParent(this.m_EffectBattleChallenge))
		{
			this.m_EffectBattleChallenge = NrTSingleton<NkEffectManager>.Instance.CreateEffectUI("FX_BATTLE_CHALLENGE", screenPos, new NkEffectUnit.DeleteCallBack(this.ChallengeEffectDeleteCallBack));
		}
		else
		{
			this.m_EffectBattleChallenge.SetActive(false);
			this.m_EffectBattleChallenge.layer = TsLayer.GUI;
		}
		Transform child = NkUtil.GetChild(this.m_EffectBattleChallenge.transform, "fx_text");
		Transform child2 = NkUtil.GetChild(this.m_EffectBattleChallenge.transform, "fx_final");
		Transform child3 = NkUtil.GetChild(this.m_EffectBattleChallenge.transform, "fx_challenge");
		Transform child4 = NkUtil.GetChild(this.m_EffectBattleChallenge.transform, "fx_bonus");
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		GameObject gameObject3 = null;
		if (child2 != null)
		{
			gameObject = child2.gameObject;
		}
		if (child4 != null)
		{
			gameObject3 = child4.gameObject;
		}
		if (child != null)
		{
			GameObject gameObject4 = child.gameObject;
			if (gameObject4 != null)
			{
				MeshFilter component = gameObject4.GetComponent<MeshFilter>();
				Vector2[] array = new Vector2[component.mesh.uv.Length];
				if (component != null)
				{
					if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID)
					{
						Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.m_strText);
						if (component != null)
						{
							array[0].x = 0f;
							array[0].y = 1f;
							array[1].x = 1f;
							array[1].y = 1f;
							array[2].x = 0f;
							array[2].y = 0f;
							array[3].x = 1f;
							array[3].y = 0f;
							component.mesh.uv = array;
						}
						Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
						if (null != gameObject4.renderer && null != material)
						{
							gameObject4.renderer.sharedMaterial = material;
						}
						if (null != texture)
						{
							material.mainTexture = texture;
						}
						else
						{
							CustomMonsterProtriteInfo customMonsterProtriteInfo = new CustomMonsterProtriteInfo();
							customMonsterProtriteInfo.m_goAniObject = gameObject4;
							customMonsterProtriteInfo.m_Material = gameObject4.renderer.sharedMaterial;
							customMonsterProtriteInfo.m_szImageKey = this.m_strText;
							string str = string.Format("{0}", "UI/mythicraid/" + this.m_strText + NrTSingleton<UIDataManager>.Instance.AddFilePath);
							WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
							wWWItem.SetItemType(ItemType.USER_ASSETB);
							wWWItem.SetCallback(new PostProcPerItem(this.SetBundleImage), customMonsterProtriteInfo);
							TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
						}
					}
					else if (nCount > 1)
					{
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = component.mesh.uv[i];
							Vector2[] expr_360_cp_0 = array;
							int expr_360_cp_1 = i;
							expr_360_cp_0[expr_360_cp_1].y = expr_360_cp_0[expr_360_cp_1].y - this.m_fBattleContinueEffectUV;
						}
						if (component != null)
						{
							component.mesh.uv = array;
						}
					}
				}
			}
			if (bBoss)
			{
				if (nCount < 100)
				{
					if (gameObject != null)
					{
						if (child3.gameObject != null)
						{
							child3.gameObject.SetActive(false);
						}
						if (gameObject3 != null)
						{
							gameObject3.SetActive(false);
						}
						gameObject.SetActive(true);
						Animation component2 = gameObject.GetComponent<Animation>();
						if (component2 != null)
						{
							Battle.BATTLE.m_fContinueBattleWaitTime = Time.realtimeSinceStartup + component2.clip.length + 0.1f;
						}
						else
						{
							Battle.BATTLE.m_fContinueBattleWaitTime = Time.realtimeSinceStartup + 2f;
						}
						NrSound.ImmedatePlay("UI_SFX", "BATTLE", "BATTLE-CONTINUE-BOSS");
						if (Battle.BATTLE != null)
						{
							Battle.BATTLE.PlayBossBGM();
						}
					}
				}
				else if (nCount >= 100 && gameObject3 != null)
				{
					if (child3.gameObject != null)
					{
						child3.gameObject.SetActive(false);
					}
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
					gameObject3.SetActive(true);
					Animation component3 = gameObject3.GetComponent<Animation>();
					if (component3 != null)
					{
						Battle.BATTLE.m_fContinueBattleWaitTime = Time.realtimeSinceStartup + component3.clip.length + 0.1f;
					}
					else
					{
						Battle.BATTLE.m_fContinueBattleWaitTime = Time.realtimeSinceStartup + 2f;
					}
					NrSound.ImmedatePlay("UI_SFX", "BATTLE", "HIDDEN_BONUS");
					if (Battle.BATTLE != null)
					{
						Battle.BATTLE.PlayBossBGM();
					}
				}
			}
			else
			{
				child3.gameObject.SetActive(false);
				gameObject2 = child3.gameObject;
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
				if (gameObject3 != null)
				{
					gameObject3.SetActive(false);
				}
				NrSound.ImmedatePlay("UI_SFX", "BATTLE", "BATTLE-CONTINUE");
			}
		}
		if (!bBoss)
		{
			for (int j = 0; j < 6; j++)
			{
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nMonsterKind[j]);
				string strName = string.Empty;
				string text = string.Empty;
				eCharImageType type;
				if (j == 0)
				{
					strName = string.Format("fx_boss", new object[0]);
					if (charKindInfo != null)
					{
						if (UIDataManager.IsUse256Texture())
						{
							text = charKindInfo.GetPortraitFile1(-1, string.Empty) + "_256";
						}
						else
						{
							text = charKindInfo.GetPortraitFile1(-1, string.Empty) + "_512";
						}
					}
					type = eCharImageType.LARGE;
				}
				else
				{
					strName = string.Format("fx_enemy0{0}", j.ToString());
					if (charKindInfo != null)
					{
						text = charKindInfo.GetPortraitFile1(-1, string.Empty) + "_64";
					}
					type = eCharImageType.SMALL;
				}
				Transform child5 = NkUtil.GetChild(this.m_EffectBattleChallenge.transform, strName);
				if (child5 != null)
				{
					GameObject gameObject5 = child5.gameObject;
					if (null != gameObject5)
					{
						if (charKindInfo == null)
						{
							gameObject5.SetActive(false);
						}
						else
						{
							gameObject5.SetActive(true);
							Renderer component4 = gameObject5.GetComponent<Renderer>();
							if (component4 != null)
							{
								Material material2 = component4.material;
								if (null != material2)
								{
									if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(text))
									{
										CustomMonsterProtriteInfo customMonsterProtriteInfo2 = new CustomMonsterProtriteInfo();
										customMonsterProtriteInfo2.m_goAniObject = gameObject2;
										customMonsterProtriteInfo2.m_Material = material2;
										customMonsterProtriteInfo2.m_szImageKey = text;
										NrTSingleton<UIImageBundleManager>.Instance.RequestCharImageCustomParam(text, type, new PostProcPerItem(this.SetBundleImage), customMonsterProtriteInfo2);
									}
									else
									{
										material2.mainTexture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(text);
										gameObject2.SetActive(true);
									}
								}
							}
						}
					}
				}
			}
		}
		Vector2 screenPos2 = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		this.m_EffectBattleChallenge.transform.position = base.GetEffectUIPos(screenPos2);
		this.m_EffectBattleChallenge.SetActive(true);
		this.m_nCurrentContinueCount = nCount;
	}

	public void ChallengeEffectDeleteCallBack()
	{
		if (this.m_EffectBattleChallenge != null)
		{
			this.m_EffectBattleChallenge.SetActive(false);
		}
	}

	public int GetContinueCount()
	{
		return this.m_nCurrentContinueCount;
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D && _param is CustomMonsterProtriteInfo)
			{
				string imageKey = string.Empty;
				CustomMonsterProtriteInfo customMonsterProtriteInfo = _param as CustomMonsterProtriteInfo;
				if (customMonsterProtriteInfo == null)
				{
					return;
				}
				imageKey = customMonsterProtriteInfo.m_szImageKey;
				NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				customMonsterProtriteInfo.m_Material.mainTexture = texture2D;
				if (customMonsterProtriteInfo.m_goAniObject != null)
				{
					customMonsterProtriteInfo.m_goAniObject.SetActive(true);
				}
			}
		}
	}

	public void ShowTreasureMonsterDie()
	{
		Vector2 screenPos = new Vector2(GUICamera.width / 2f, GUICamera.height / 2f);
		if (EffectDefine.IsValidParent(this.m_EffectTreasureMonsterDie))
		{
			this.m_EffectTreasureMonsterDie = NrTSingleton<NkEffectManager>.Instance.CreateEffectUI("FX_TREASURE_UI", screenPos, new NkEffectUnit.DeleteCallBack(this.TreasureMonsterDieEffect));
			this.m_EffectTreasureMonsterDie.layer = TsLayer.GUI;
		}
		else
		{
			this.m_EffectTreasureMonsterDie.SetActive(true);
			this.m_EffectTreasureMonsterDie.layer = TsLayer.GUI;
		}
	}

	public void TreasureMonsterDieEffect()
	{
		if (this.m_EffectTreasureMonsterDie != null)
		{
			this.m_EffectTreasureMonsterDie.SetActive(false);
			this.m_EffectTreasureMonsterDie.layer = TsLayer.GUI;
		}
	}

	public void ShowHiddenTreasureMonsterDie()
	{
		Vector2 screenPos = new Vector2(GUICamera.width / 2f, GUICamera.height / 2f);
		if (EffectDefine.IsValidParent(this.m_EffectHiddenTreasureMonsterDie))
		{
			this.m_EffectHiddenTreasureMonsterDie = NrTSingleton<NkEffectManager>.Instance.CreateEffectUI("FX_UI_BIGTREASUREBOX", screenPos, new NkEffectUnit.DeleteCallBack(this.HiddenTreasureMonsterDieEffect));
			this.m_EffectHiddenTreasureMonsterDie.layer = TsLayer.GUI;
		}
		else
		{
			this.m_EffectHiddenTreasureMonsterDie.SetActive(true);
			this.m_EffectHiddenTreasureMonsterDie.layer = TsLayer.GUI;
		}
	}

	public void HiddenTreasureMonsterDieEffect()
	{
		if (this.m_EffectTreasureMonsterDie != null)
		{
			this.m_EffectTreasureMonsterDie.SetActive(false);
			this.m_EffectTreasureMonsterDie.layer = TsLayer.GUI;
		}
	}

	public void effectdownload(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goCountDown = obj;
		this.m_goCountDown.SetActive(false);
		Vector3 localPosition = this.m_goCountDown.transform.localPosition;
		localPosition.z = 1f;
		this.m_goCountDown.transform.localPosition = localPosition;
	}

	private void SetBundleTextureImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}

	private void SetControl_PreviewHero()
	{
		this.SetVisibleFlag(true);
		base.SetShowLayer(0, false);
		base.SetShowLayer(1, true);
		this.m_dtPreviewBG.SetSize(GUICamera.width, this.m_dtPreviewBG.GetSize().y);
		this.m_dtPreviewBG.SetLocation(0f, this.m_dtPreviewBG.GetSize().y * 0.8f);
		this.m_lbPreview.SetLocation(0, 0);
	}
}
