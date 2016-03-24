using GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_Skill_Direction_Dlg : Form
{
	private GameObject m_goSkillDirecting;

	private bool m_bSetFace;

	private string faceImageKey = string.Empty;

	private float m_fEndTime;

	private bool m_bRival;

	private TsWeakReference<NkBattleChar> m_TargetChar;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		this.AlwaysUpdate = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Skill_Direction", G_ID.BATTLE_SKILL_DIRECTION_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		Vector2 location = new Vector2(0f, 0f);
		base.SetLocation(location);
		Texture2D texture2D = (Texture2D)Resources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/BattleSkill_Icon/fxk_dark_line" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		if (texture2D != null)
		{
			base.SetBGImage(texture2D);
			base.SetSize(GUICamera.width, GUICamera.height);
			base.BG.SetColor(new Color(1f, 1f, 1f, 0.7f));
		}
	}

	public override void Update()
	{
		base.Update();
		if (this.m_goSkillDirecting == null)
		{
			return;
		}
		this.CloseUpdate();
		if (!this.m_bSetFace)
		{
			Transform child;
			if (this.m_bRival)
			{
				child = NkUtil.GetChild(this.m_goSkillDirecting.transform, "fx_boss");
			}
			else
			{
				child = NkUtil.GetChild(this.m_goSkillDirecting.transform, "fx_face_base");
			}
			if (child != null)
			{
				GameObject gameObject = child.gameObject;
				if (null != gameObject)
				{
					Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
					if (null != texture)
					{
						Renderer component = gameObject.GetComponent<Renderer>();
						if (component != null)
						{
							Material material = component.material;
							if (null != material)
							{
								material.mainTexture = texture;
								this.m_bSetFace = true;
								this.m_goSkillDirecting.SetActive(true);
								Animation componentInChildren = this.m_goSkillDirecting.GetComponentInChildren<Animation>();
								if (componentInChildren != null)
								{
									this.m_fEndTime = Time.time + componentInChildren.clip.length + 0.1f;
									componentInChildren.Stop();
									componentInChildren.Play();
								}
								else
								{
									this.m_fEndTime = 0f;
								}
							}
						}
					}
				}
			}
		}
	}

	private void CloseUpdate()
	{
		if (Time.time > this.m_fEndTime)
		{
			this.Close();
		}
	}

	public override void Close()
	{
		UnityEngine.Object.Destroy(this.m_goSkillDirecting);
		this.m_goSkillDirecting = null;
		this.m_TargetChar = null;
		base.Close();
	}

	public void SetMagic(NkBattleChar pkTarget, int BattleSkillUnique, bool bRival)
	{
		if (pkTarget == null)
		{
			return;
		}
		if (this.m_TargetChar != null && this.m_TargetChar.CastedTarget.GetBUID() == pkTarget.GetBUID())
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BattleSkillUnique);
		if (battleSkillBase != null && Battle.BATTLE.SkillDirecting != null)
		{
			this.m_bRival = false;
			if (this.m_goSkillDirecting != null)
			{
				UnityEngine.Object.Destroy(this.m_goSkillDirecting);
				this.m_goSkillDirecting = null;
			}
			if (bRival && Battle.BATTLE.SkillRivalDirecting != null)
			{
				this.m_goSkillDirecting = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.SkillRivalDirecting, Vector3.zero, Quaternion.identity);
				this.m_bRival = true;
			}
			else
			{
				this.m_goSkillDirecting = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.SkillDirecting, Vector3.zero, Quaternion.identity);
			}
			NkUtil.SetAllChildLayer(this.m_goSkillDirecting, GUICamera.UILayer);
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
			this.m_goSkillDirecting.transform.position = effectUIPos;
			string costumePortraitPath = this.GetCostumePortraitPath(pkTarget.GetSoldierInfo());
			if (UIDataManager.IsUse256Texture())
			{
				this.faceImageKey = pkTarget.GetCharKindInfo().GetPortraitFile1((int)pkTarget.GetSoldierInfo().GetGrade(), costumePortraitPath) + "_256";
			}
			else
			{
				this.faceImageKey = pkTarget.GetCharKindInfo().GetPortraitFile1((int)pkTarget.GetSoldierInfo().GetGrade(), costumePortraitPath) + "_512";
			}
			if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey))
			{
				NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.faceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
			}
			this.m_bSetFace = false;
			Animation componentInChildren = this.m_goSkillDirecting.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				this.m_fEndTime = Time.time + 10f;
			}
			else
			{
				this.m_fEndTime = Time.time + 10f;
			}
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goSkillDirecting);
			}
			this.m_goSkillDirecting.SetActive(false);
		}
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null)
		{
			if (null != _item.GetSafeBundle().mainAsset)
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
		else
		{
			this.Close();
		}
	}

	private string GetCostumePortraitPath(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return string.Empty;
		}
		int costumeUnique = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		return NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(costumeUnique);
	}
}
