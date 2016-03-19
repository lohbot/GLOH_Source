using GameMessage;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CharSelectCommandLayer : InputCommandLayer
{
	private const float CREATEANI_FIX_TIME = 1f;

	private GameObject m_objHit;

	private E_CHAR_TRIBE m_CreateTribeType;

	private CCameraAniPlay m_CameraAniPlay = NrTSingleton<CCameraAniPlay>.Instance;

	private RaceSelectDlg m_RaceSelForm;

	public CharSelectCommandLayer()
	{
		base.AddTapInputDelegate(new InputDelegate(this.MouserClickUpdate));
		base.AddMoveInputDelegate(new InputDelegate(this.MouseOverUpdate));
	}

	public GameObject GetHitObj()
	{
		return this.m_objHit;
	}

	private void MouseOverUpdate()
	{
		if (this.m_RaceSelForm == null)
		{
			this.m_RaceSelForm = (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.RACE_SELECT_DLG) as RaceSelectDlg);
		}
		if (this.m_RaceSelForm != null && this.m_RaceSelForm.MouseOver)
		{
			if (this.m_objHit != null)
			{
				string animation = string.Format("{0}_off", this.m_objHit.name);
				this.m_objHit.animation.Play(animation);
				this.m_objHit = null;
			}
			TsLog.Log("MouseOver!!!!", new object[0]);
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(NkInputManager.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f))
		{
			if (raycastHit.collider.gameObject != null && this.m_objHit != raycastHit.collider.gameObject)
			{
				string text = string.Empty;
				if (this.m_objHit != null)
				{
					text = string.Format("{0}_off", this.m_objHit.name);
					this.m_objHit.animation.Play(text);
					this.m_objHit = null;
				}
				Animation component = raycastHit.collider.gameObject.GetComponent<Animation>();
				if (component == null)
				{
					return;
				}
				this.m_objHit = raycastHit.collider.gameObject;
				text = string.Format("{0}_on", raycastHit.collider.gameObject.name);
				AnimationClip clip = component.GetClip(text);
				if (clip != null)
				{
					this.m_objHit = raycastHit.collider.gameObject;
					this.m_objHit.animation.Play(text);
					this.OnMouseOverSound();
				}
			}
		}
		else if (this.m_objHit != null)
		{
			string animation2 = string.Format("{0}_off", this.m_objHit.name);
			if (this.m_objHit.GetComponent<Animation>() != null)
			{
				this.m_objHit.animation.Play(animation2);
			}
			this.m_objHit = null;
		}
	}

	private void MouserClickUpdate()
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			Ray ray = Camera.main.ScreenPointToRay(NkInputManager.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 100f) && raycastHit.collider.gameObject != null)
			{
				this.m_objHit = raycastHit.collider.gameObject;
			}
		}
		if (this.m_objHit != null)
		{
			TsLog.LogWarning("UI FOCUS : {0} CLICK : {1}", new object[]
			{
				NrTSingleton<UIManager>.Instance.FocusObject,
				NrTSingleton<UIManager>.Instance.ClickUI
			});
			E_CHAR_TRIBE e_CHAR_TRIBE = E_CHAR_TRIBE.NULL;
			E_CHAR_SELECT_STEP e_CHAR_SELECT_STEP = E_CHAR_SELECT_STEP.NONE;
			for (E_CHAR_TRIBE e_CHAR_TRIBE2 = E_CHAR_TRIBE.END; e_CHAR_TRIBE2 > E_CHAR_TRIBE.NULL; e_CHAR_TRIBE2--)
			{
				if (this.m_objHit.name.Contains(e_CHAR_TRIBE2.ToString().ToLower()))
				{
					TsLog.LogWarning("Select Tribe : {0}", new object[]
					{
						e_CHAR_TRIBE2
					});
					e_CHAR_TRIBE = e_CHAR_TRIBE2;
					break;
				}
			}
			switch (e_CHAR_TRIBE)
			{
			case E_CHAR_TRIBE.HUMAN:
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
				this.m_CameraAniPlay.Add(new Action<object>(this._OnCreateChar), 0f, new object[]
				{
					E_CAMARA_STATE_ANI.CREATETOHUMAN
				});
				e_CHAR_SELECT_STEP = E_CHAR_SELECT_STEP.NONE;
				break;
			case E_CHAR_TRIBE.FURRY:
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
				this.m_CameraAniPlay.Add(new Action<object>(this._OnCreateChar), 0f, new object[]
				{
					E_CAMARA_STATE_ANI.CREATETOFURRY
				});
				e_CHAR_SELECT_STEP = E_CHAR_SELECT_STEP.NONE;
				break;
			case E_CHAR_TRIBE.ELF:
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
				this.m_CameraAniPlay.Add(new Action<object>(this._OnCreateChar), 0f, new object[]
				{
					E_CAMARA_STATE_ANI.CREATETOELF
				});
				e_CHAR_SELECT_STEP = E_CHAR_SELECT_STEP.NONE;
				break;
			case E_CHAR_TRIBE.HUMANF:
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
				this.m_CameraAniPlay.Add(new Action<object>(this._OnCreateChar), 0f, new object[]
				{
					E_CAMARA_STATE_ANI.CREATETOHUMANF
				});
				e_CHAR_SELECT_STEP = E_CHAR_SELECT_STEP.NONE;
				break;
			}
			MsgHandler.Handle("SetCreateChar", new object[]
			{
				e_CHAR_TRIBE
			});
			MsgHandler.Handle("SetSelectStep", new object[]
			{
				e_CHAR_SELECT_STEP
			});
			this.m_CreateTribeType = e_CHAR_TRIBE;
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "SPECIES_SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			TsLog.Log("m_objHit == NULL", new object[0]);
		}
	}

	private void _OnCreateChar(object obj)
	{
		TsLog.LogWarning("_OnCreateChar", new object[0]);
		DLG_CreateChar dLG_CreateChar = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEW_CREATECHAR_DLG) as DLG_CreateChar;
		dLG_CreateChar.SetCharKind(this.m_CreateTribeType);
		Button expr_34 = dLG_CreateChar._Back;
		expr_34.Click = (EZValueChangedDelegate)Delegate.Combine(expr_34.Click, new EZValueChangedDelegate(this._OnCreateCharToSelectBack));
		dLG_CreateChar.SetLocation(GUICamera.width - dLG_CreateChar.GetSizeX() - 10f, 0f);
	}

	private void _OnCreateCharToSelectBack(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.NEW_CREATECHAR_DLG);
		E_CAMARA_STATE_ANI e_CAMARA_STATE_ANI = E_CAMARA_STATE_ANI.NONE;
		switch (this.m_CreateTribeType)
		{
		case E_CHAR_TRIBE.HUMAN:
			e_CAMARA_STATE_ANI = E_CAMARA_STATE_ANI.HUMANTOCREATE;
			break;
		case E_CHAR_TRIBE.FURRY:
			e_CAMARA_STATE_ANI = E_CAMARA_STATE_ANI.FURRYTOCREATE;
			break;
		case E_CHAR_TRIBE.ELF:
			e_CAMARA_STATE_ANI = E_CAMARA_STATE_ANI.ELFTOCREATE;
			break;
		case E_CHAR_TRIBE.HUMANF:
			e_CAMARA_STATE_ANI = E_CAMARA_STATE_ANI.HUMANFTOCREATE;
			break;
		}
		this.m_CameraAniPlay.Add(E_CHAR_SELECT_STEP.CREATE_SELECT, new Action<object>(this._OnRaceSelect), new object[]
		{
			e_CAMARA_STATE_ANI
		});
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "RETURN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void _OnRaceSelect(object obj)
	{
		this.m_objHit = null;
	}

	private void _OnReceSelectToSelectBack(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.RACE_SELECT_DLG);
		NrTSingleton<FormsManager>.Instance.Show(G_ID.CHAR_SELECT_DLG);
		MsgHandler.Handle("SetSelectStep", new object[]
		{
			E_CHAR_SELECT_STEP.CHAR_SELECT
		});
		NmMainFrameWork.AddBGM();
		TsAudioManager.Instance.AudioContainer.RemoveBGM("intro");
	}

	private void OnMouseOverSound()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "CUSTOMOZING", "CHAR_MOUSEOVER", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
