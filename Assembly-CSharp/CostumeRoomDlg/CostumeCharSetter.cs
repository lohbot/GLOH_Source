using CostumeRoomDlg.COSTUME_CHAR_SETTER;
using System;
using UnityEngine;

namespace CostumeRoomDlg
{
	public class CostumeCharSetter
	{
		private CostumeRoom_Dlg _owner;

		private GameObject _costumeChar;

		private int _settingCostumeUnique;

		private Vector3 _v3TouchStart = Vector3.zero;

		private Vector2 _v2TouchStart = Vector2.zero;

		private float _fTempAngle;

		private float _fAngle;

		private Animation _charAni;

		private string _costumeLightObjName = string.Empty;

		private static Light _directionLight;

		private CharSettingProcessor _charSettingProcessor;

		public AnimationProcessor _animationProcessor;

		private float _lightIntense = 0.35f;

		public GameObject _CostumeChar
		{
			get
			{
				return this._costumeChar;
			}
			set
			{
				this._costumeChar = value;
			}
		}

		public Animation _CharAni
		{
			get
			{
				return this._charAni;
			}
			set
			{
				this._charAni = value;
			}
		}

		public CostumeCharSetter(CostumeRoom_Dlg owner)
		{
			this._owner = owner;
			this._costumeLightObjName = "CostumeLight";
			this._charSettingProcessor = new CharSettingProcessor(owner, this);
			this._animationProcessor = new AnimationProcessor(this);
		}

		public void InitCostumeChar(CharCostumeInfo_Data costumeData)
		{
			if (this._settingCostumeUnique == costumeData.m_costumeUnique)
			{
				this._animationProcessor.PlayAttackAni(false);
				return;
			}
			this._settingCostumeUnique = costumeData.m_costumeUnique;
			this.SetLight();
			this._charSettingProcessor.SetChar(ref costumeData);
		}

		public void Update()
		{
			this._animationProcessor.PlayDefaultIdelAni();
			this.CharacterRotation();
			this.AniButtonActivateColor();
		}

		public void VisibleChar(bool visible)
		{
			if (this._costumeChar == null)
			{
				return;
			}
			if (this._costumeChar.activeSelf == visible)
			{
				return;
			}
			this._costumeChar.SetActive(visible);
		}

		public void OnClose()
		{
			this.DestroyPrevCostumeChar();
			this.HideLight();
		}

		public void CostumeViewOnPress(IUIObject obj)
		{
			if (this._costumeChar == null)
			{
				return;
			}
			Vector3 mousePosition = Input.mousePosition;
			if (mousePosition != Vector3.zero)
			{
				this._v2TouchStart = new Vector2(mousePosition.x, (float)Screen.height - mousePosition.y);
				this._v3TouchStart = mousePosition;
				this._fAngle = this._costumeChar.transform.rotation.eulerAngles.y;
			}
		}

		public void OnClickActionWalk(IUIObject obj)
		{
			this.HideEffect();
			this._animationProcessor.PlayMoveAni(true);
		}

		public void OnClickActionStay(IUIObject obj)
		{
			this.HideEffect();
			this._animationProcessor.PlayIdelAni(true);
		}

		public void OnClickActionAttack(IUIObject obj)
		{
			this.HideEffect();
			this._animationProcessor.PlayAttackAni(true);
		}

		private void CharacterRotation()
		{
			if (this._costumeChar == null)
			{
				return;
			}
			Vector3 mousePosition = Input.mousePosition;
			if (NkInputManager.GetMouseButton(0) && this._v3TouchStart != Vector3.zero && this._v3TouchStart != mousePosition)
			{
				if (mousePosition != Vector3.zero && Mathf.Abs(this._v2TouchStart.x - mousePosition.x) > 5f)
				{
					this._fTempAngle = 360f * ((this._v2TouchStart.x - mousePosition.x) / (float)Screen.width);
					Quaternion rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.AngleAxis(this._fAngle + this._fTempAngle, Vector3.up), Time.time * 0.1f);
					this._costumeChar.transform.rotation = rotation;
				}
			}
			else if (NkInputManager.GetMouseButtonUp(0) && mousePosition != Vector3.zero)
			{
				this._v3TouchStart = Vector3.zero;
			}
		}

		private void SetLight()
		{
			if (CostumeCharSetter._directionLight != null)
			{
				CostumeCharSetter._directionLight.gameObject.SetActive(true);
				this.SetLightIntense();
				return;
			}
			GameObject gameObject = new GameObject(this._costumeLightObjName);
			CostumeCharSetter._directionLight = gameObject.AddComponent<Light>();
			CostumeCharSetter._directionLight.transform.position = Vector3.zero;
			CostumeCharSetter._directionLight.transform.rotation = Quaternion.identity;
			CostumeCharSetter._directionLight.type = LightType.Directional;
			this.SetLightIntense();
		}

		private void SetLightIntense()
		{
			if (CostumeCharSetter._directionLight == null)
			{
				return;
			}
			float num = this._lightIntense;
			CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(this._settingCostumeUnique);
			if (costumeData != null)
			{
				num *= costumeData.m_Light;
			}
			CostumeCharSetter._directionLight.intensity = num;
		}

		private void HideLight()
		{
			if (CostumeCharSetter._directionLight == null)
			{
				return;
			}
			CostumeCharSetter._directionLight.gameObject.SetActive(false);
		}

		private void AniButtonActivateColor()
		{
			if (this._charAni == null)
			{
				return;
			}
			this._owner._variables._btnAttack.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			this._owner._variables._btnWalk.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			this._owner._variables._btnStay.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			if (this._animationProcessor.IsAttackAniPlaying())
			{
				this._owner._variables._btnAttack.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			else if (this._animationProcessor.IsMoveAniPlaying())
			{
				this._owner._variables._btnWalk.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			else if (this._animationProcessor.IsIdleAniPlaying())
			{
				this._owner._variables._btnStay.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
		}

		private void DestroyPrevCostumeChar()
		{
			if (this._CostumeChar == null)
			{
				return;
			}
			this._CostumeChar.SetActive(false);
			UnityEngine.Object.Destroy(this._CostumeChar);
			this._CostumeChar = null;
		}

		private void HideEffect()
		{
			if (this._costumeChar == null)
			{
				return;
			}
			Transform child = NkUtil.GetChild(this._costumeChar.transform, "fx_dummy");
			if (child == null)
			{
				return;
			}
			MeshRenderer[] componentsInChildren = child.GetComponentsInChildren<MeshRenderer>();
			if (componentsInChildren == null)
			{
				return;
			}
			MeshRenderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				MeshRenderer meshRenderer = array[i];
				if (!(meshRenderer == null))
				{
					meshRenderer.enabled = false;
				}
			}
		}
	}
}
