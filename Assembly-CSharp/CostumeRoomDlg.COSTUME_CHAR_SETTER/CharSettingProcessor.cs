using System;
using TsBundle;
using UnityEngine;
using UnityForms;

namespace CostumeRoomDlg.COSTUME_CHAR_SETTER
{
	public class CharSettingProcessor
	{
		private CostumeRoom_Dlg _costumeRoomDlg;

		private CostumeCharSetter _costumeCharSetter;

		private GameObject _costumePrefab;

		private float _charScale;

		private float _charRotationY;

		private CharCostumeInfo_Data _settedCostumeData;

		private Transform _effectBillboardTargetObj;

		public CharSettingProcessor(CostumeRoom_Dlg costumeRoomDlg, CostumeCharSetter costumeCharSetter)
		{
			this._costumeRoomDlg = costumeRoomDlg;
			this._costumeCharSetter = costumeCharSetter;
			this._charScale = 210f;
			this._charRotationY = 180f;
		}

		public void SetChar(ref CharCostumeInfo_Data costumeData)
		{
			if (costumeData == null)
			{
				return;
			}
			this._settedCostumeData = costumeData;
			string path = "Char/" + costumeData.m_BundlePath;
			NrTSingleton<NkBundleCallBack>.Instance.RequestBundleCallBackRunTime(path, NkBundleCallBack.BattlePreLoadingChar, new NkBundleParam.funcParamBundleCallBack(this.CostumeCharLoadComplete), costumeData, false);
		}

		public void Close()
		{
			if (this._effectBillboardTargetObj != null)
			{
				UnityEngine.Object.Destroy(this._effectBillboardTargetObj);
			}
		}

		public void CostumeCharLoadComplete(ref IDownloadedItem bundle, object paramobj)
		{
			if (bundle == null || bundle.isCanceled)
			{
				return;
			}
			if (bundle.GetSafeBundle() == null)
			{
				Debug.LogWarning("wItem.mainAsset is null -> Path = {0}" + bundle.assetPath);
				return;
			}
			if (bundle.GetSafeBundle().mainAsset == null)
			{
				return;
			}
			this.DestroyPrevCostumeChar();
			this._costumePrefab = (bundle.GetSafeBundle().mainAsset as GameObject);
			this._costumeCharSetter._CostumeChar = (GameObject)UnityEngine.Object.Instantiate(this._costumePrefab);
			this.SetCostumeCharLayer();
			this.SetCharBillboardCamera();
			this.InitCharTransform();
			this.RestoreCharShader();
			this.SetParticleEffectSize();
			this._costumeCharSetter._CharAni = this._costumeCharSetter._CostumeChar.GetComponent<Animation>();
			this._costumeCharSetter._animationProcessor.PlayAttackAni(true);
		}

		private void SetCostumeCharLayer()
		{
			GameObject costumeChar = this._costumeCharSetter._CostumeChar;
			if (costumeChar == null)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("UI Camera");
			if (gameObject == null)
			{
				return;
			}
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			if (componentInChildren == null)
			{
				return;
			}
			Collider[] componentsInChildren = costumeChar.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i];
				collider.enabled = false;
			}
			NkUtil.SetAllChildLayer(costumeChar, GUICamera.UILayer);
		}

		private void InitCharTransform()
		{
			GameObject costumeChar = this._costumeCharSetter._CostumeChar;
			if (costumeChar == null)
			{
				return;
			}
			DrawTexture costumeCharView = this._costumeRoomDlg._variables._costumeCharView;
			Vector3 location = costumeCharView.GetLocation();
			location.x = costumeCharView.transform.position.x + costumeCharView.GetSize().x / 2f;
			location.y = -(Mathf.Abs(costumeCharView.transform.position.y) + costumeCharView.GetSize().y);
			location.z = this._charScale * 2f;
			costumeChar.transform.position = location;
			costumeChar.transform.localPosition = new Vector3(costumeChar.transform.localPosition.x + this._settedCostumeData.m_MoveX, costumeChar.transform.localPosition.y + this._settedCostumeData.m_MoveY, costumeChar.transform.localPosition.z);
			float calculateCharScale = this.GetCalculateCharScale();
			costumeChar.transform.localScale = new Vector3(calculateCharScale, calculateCharScale, calculateCharScale);
			costumeChar.transform.localRotation = Quaternion.Euler(0f, this._charRotationY, 0f);
		}

		private void RestoreCharShader()
		{
			GameObject costumeChar = this._costumeCharSetter._CostumeChar;
			if (costumeChar == null)
			{
				return;
			}
			Renderer[] componentsInChildren = costumeChar.GetComponentsInChildren<Renderer>(true);
			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer renderer = array[i];
				if (!(renderer == null) && renderer.sharedMaterials != null)
				{
					for (int j = 0; j < renderer.sharedMaterials.Length; j++)
					{
						if (!(renderer.sharedMaterials[j] == null))
						{
							renderer.sharedMaterials[j].shader = Shader.Find(renderer.sharedMaterials[j].shader.name);
						}
					}
				}
			}
		}

		private void DestroyPrevCostumeChar()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			this._costumeCharSetter._CostumeChar.SetActive(false);
			UnityEngine.Object.Destroy(this._costumeCharSetter._CostumeChar);
			this._costumeCharSetter._CostumeChar = null;
		}

		private void SetParticleEffectSize()
		{
			if (this._costumeCharSetter._CostumeChar == null || this._settedCostumeData == null)
			{
				return;
			}
			ParticleSystem[] componentsInChildren = this._costumeCharSetter._CostumeChar.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren == null)
			{
				return;
			}
			ParticleSystem[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				ParticleSystem particleSystem = array[i];
				particleSystem.startSize *= this.GetCalculateCharScale();
			}
		}

		private float GetCalculateCharScale()
		{
			if (this._settedCostumeData == null)
			{
				return this._charScale;
			}
			return this._charScale * this._settedCostumeData.m_Scale;
		}

		private void SetCharBillboardCamera()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return;
			}
			FX_BB_MakeBillboard[] componentsInChildren = this._costumeCharSetter._CostumeChar.GetComponentsInChildren<FX_BB_MakeBillboard>();
			if (componentsInChildren == null)
			{
				return;
			}
			Transform billboardTargetObj = this.GetBillboardTargetObj();
			if (billboardTargetObj == null)
			{
				return;
			}
			FX_BB_MakeBillboard[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				FX_BB_MakeBillboard fX_BB_MakeBillboard = array[i];
				if (!(fX_BB_MakeBillboard == null))
				{
					fX_BB_MakeBillboard.target = billboardTargetObj;
				}
			}
		}

		private Transform GetBillboardTargetObj()
		{
			if (this._costumeCharSetter._CostumeChar == null)
			{
				return null;
			}
			if (this._effectBillboardTargetObj == null)
			{
				GameObject gameObject = GameObject.Find("BillBoardTarget");
				if (gameObject != null)
				{
					this._effectBillboardTargetObj = gameObject.transform;
				}
			}
			if (this._effectBillboardTargetObj == null)
			{
				this._effectBillboardTargetObj = new GameObject().transform;
				this._effectBillboardTargetObj.transform.name = "BillBoardTarget";
			}
			NkUtil.SetAllChildLayer(this._effectBillboardTargetObj.gameObject, GUICamera.UILayer);
			this._effectBillboardTargetObj.localPosition = new Vector3(this._costumeCharSetter._CostumeChar.transform.localPosition.x, this._costumeCharSetter._CostumeChar.transform.localPosition.y, this._costumeCharSetter._CostumeChar.transform.localPosition.z - 1000f);
			return this._effectBillboardTargetObj;
		}
	}
}
