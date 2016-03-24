using System;
using TsBundle;
using UnityEngine;

namespace SOL_COMBINATION_DIRECTION_DLG
{
	public class DirectionEffectSetter
	{
		private SolCombinationDirection_Dlg _owner;

		private int _solCombinationUniqeKey = -1;

		private GameObject _directionEffect;

		public Animation _directionAni;

		public DirectionEffectSetter(SolCombinationDirection_Dlg owner, int solCombinationUniqeKey)
		{
			this._owner = owner;
			this._solCombinationUniqeKey = solCombinationUniqeKey;
		}

		public void RequestSolCombinationDirectionEffect()
		{
			string effecType = this.GetEffecType();
			if (string.IsNullOrEmpty(effecType))
			{
				return;
			}
			WWWItem wWWItem = NkEffectManager.CreateWItem(effecType);
			if (wWWItem == null)
			{
				Debug.LogError("ERROR, SolCombinationDirection_Dlg.cs, RequestSolCombinationEffect(), witem Is Null");
				return;
			}
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.ShowSolCombinationDirectionEffect), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}

		public void OnClose()
		{
			if (this._directionEffect != null)
			{
				UnityEngine.Object.DestroyObject(this._directionEffect);
				this._directionEffect = null;
			}
		}

		private void ShowSolCombinationDirectionEffect(WWWItem item, object param)
		{
			if (item == null || item.GetSafeBundle() == null)
			{
				return;
			}
			if (item.GetSafeBundle().mainAsset == null)
			{
				return;
			}
			GameObject original = item.GetSafeBundle().mainAsset as GameObject;
			this._directionEffect = (UnityEngine.Object.Instantiate(original) as GameObject);
			if (this._directionEffect == null)
			{
				return;
			}
			this.SettingDirectionAni();
			this.SettingEffectPos();
			this.SettingSolCombinationLabel();
			this.DisableDirectionCharPortrait();
			this.SettingDirectionCharPortrait();
		}

		private int GetSolCombinationCharCount(string[] charCodeList)
		{
			if (charCodeList == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < charCodeList.Length; i++)
			{
				string text = charCodeList[i];
				if (!string.IsNullOrEmpty(text) && !(text == "0"))
				{
					num++;
				}
			}
			return num;
		}

		private void SettingEffectPos()
		{
			if (this._directionEffect == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, SettingEffectPos(), _directionEffect is Null");
				return;
			}
			Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			Vector3 effectUIPos = this._owner.GetEffectUIPos(screenPos);
			this._directionEffect.transform.position = effectUIPos;
			NkUtil.SetAllChildLayer(this._directionEffect, GUICamera.UILayer);
			if (TsPlatform.IsMobile && TsPlatform.IsEditor)
			{
				NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this._directionEffect);
			}
		}

		private void SettingDirectionAni()
		{
			if (this._directionEffect == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, CheckEffectAni(), CheckEffectAni is Null");
				return;
			}
			Animation componentInChildren = this._directionEffect.GetComponentInChildren<Animation>();
			if (componentInChildren == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, CheckEffectAni(), directionAni is Null");
				return;
			}
			this._directionAni = componentInChildren;
			if (!this._directionAni.isPlaying)
			{
				this._directionAni.Play();
			}
		}

		private void SettingSolCombinationLabel()
		{
			if (this._directionEffect == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, SettingSolCombinationLabel(), CheckEffectAni is Null");
				return;
			}
			Transform child = NkUtil.GetChild(this._directionEffect.transform, "fx_text_line");
			if (child != null && this._owner != null && this._owner._solCombinationTitle != null)
			{
				this._owner._solCombinationTitle.transform.parent = child;
			}
			Transform child2 = NkUtil.GetChild(this._directionEffect.transform, "fx_text_line");
			if (child2 != null && this._owner != null && this._owner._solCombinationDescript != null)
			{
				this._owner._solCombinationDescript.transform.parent = child2;
			}
		}

		private void DisableDirectionCharPortrait()
		{
			if (this._directionEffect == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, DisableDirectionCharPortrait(), _directionEffect is Null");
				return;
			}
			int num = 5;
			for (int i = 0; i < num; i++)
			{
				Transform child = NkUtil.GetChild(this._directionEffect.transform, "fx_hero_0" + (i + 1).ToString());
				if (!(child == null))
				{
					child.gameObject.SetActive(false);
				}
			}
		}

		private void SettingDirectionCharPortrait()
		{
			if (this._directionEffect == null)
			{
				Debug.LogError("ERROR, DirectionEffectSetter.cs, SettingDirectionCharPortrait(), CheckEffectAni is Null");
				return;
			}
			SolCombinationInfo_Data solCombinationDataByUniqeKey = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetSolCombinationDataByUniqeKey(this._solCombinationUniqeKey);
			if (solCombinationDataByUniqeKey == null || solCombinationDataByUniqeKey.m_szCombinationIsCharCode == null)
			{
				return;
			}
			int solCombinationCharCount = this.GetSolCombinationCharCount(solCombinationDataByUniqeKey.m_szCombinationIsCharCode);
			for (int i = 0; i < solCombinationCharCount; i++)
			{
				Transform child = NkUtil.GetChild(this._directionEffect.transform, "fx_hero_0" + (i + 1).ToString());
				if (!(child == null))
				{
					child.gameObject.SetActive(true);
					TsMeshCharPortraitSetter tsMeshCharPortraitSetter = child.gameObject.AddComponent<TsMeshCharPortraitSetter>();
					tsMeshCharPortraitSetter.SetTexture(solCombinationDataByUniqeKey.m_szCombinationIsCharCode[i]);
				}
			}
		}

		private string GetEffecType()
		{
			SolCombinationInfo_Data solCombinationDataByUniqeKey = NrTSingleton<NrSolCombinationSkillInfoManager>.Instance.GetSolCombinationDataByUniqeKey(this._solCombinationUniqeKey);
			if (solCombinationDataByUniqeKey == null)
			{
				return string.Empty;
			}
			if (solCombinationDataByUniqeKey.m_szCombinationIsCharCode == null)
			{
				return string.Empty;
			}
			int solCombinationCharCount = this.GetSolCombinationCharCount(solCombinationDataByUniqeKey.m_szCombinationIsCharCode);
			if (solCombinationCharCount == 3)
			{
				return "FX_SOL_COMBINATION3";
			}
			if (solCombinationCharCount == 4)
			{
				return "FX_SOL_COMBINATION4";
			}
			return "FX_SOL_COMBINATION";
		}
	}
}
