using GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class EventTriggerMiniDrama : NrTSingleton<EventTriggerMiniDrama>
{
	public class ActorManager
	{
		public static readonly string GeneralName = "General";

		public Dictionary<string, MiniDramaActorController> m_ActorControllerList = new Dictionary<string, MiniDramaActorController>();

		private MiniDramaActorController GetActorController(string ActorName)
		{
			MiniDramaActorController result = null;
			if (this.m_ActorControllerList.TryGetValue(ActorName, out result))
			{
				return result;
			}
			return null;
		}

		private NrCharBase GetActor(string ActorName)
		{
			MiniDramaActorController miniDramaActorController = null;
			if (this.m_ActorControllerList.TryGetValue(ActorName, out miniDramaActorController))
			{
				return miniDramaActorController.GetCharBase();
			}
			return null;
		}

		private bool IsActor(string ActorName)
		{
			return this.m_ActorControllerList.ContainsKey(ActorName);
		}

		public void Move(string ActorName, float x, float y, float time)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController != null)
			{
				actorController.Move(x, y, time);
			}
		}

		public void Walk(string ActorName, float x, float y, float time)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController != null)
			{
				actorController.Walk(x, y, time);
			}
		}

		public void Run(string ActorName, float x, float y, float time)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController != null)
			{
				actorController.Run(x, y, time);
			}
		}

		public void Stop(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController != null)
			{
				actorController.StopMove();
			}
		}

		public bool IsMove(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			return !(actorController == null) && actorController.IsMove();
		}

		public void RotateActor(string ActorName, float Angle, float time)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Rotate(Angle, time);
		}

		public float AniActor(string ActorName, string Ani)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return 0f;
			}
			return actorController.Ani(Ani);
		}

		public MiniDramaActorController GetController(string ActorName)
		{
			return this.GetActorController(ActorName);
		}

		public bool AddGeneral(string ActorName, bool Hide)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			return @char != null && this.AddActor(ActorName, new NkCharIDInfo
			{
				m_nCharUnique = @char.GetCharUnique(),
				m_nClientID = @char.GetID(),
				m_nWorldID = @char.GetWorldID()
			}, false, Hide);
		}

		public bool AddActor(string ActorName, NkCharIDInfo IDInfo, bool Actor, bool bHide)
		{
			if (this.IsActor(ActorName))
			{
				TsLog.LogWarning(string.Format("[EventTriggerMiniDrama] ActorManager : Have Equal CharName {0}", ActorName), new object[0]);
				return false;
			}
			GameObject gameObject = new GameObject(typeof(MiniDramaActorController).Name);
			TsSceneSwitcher.Instance.Collect(TsSceneSwitcher.ESceneType.WorldScene, gameObject);
			MiniDramaActorController miniDramaActorController = gameObject.AddComponent<MiniDramaActorController>();
			miniDramaActorController.SetChar(ActorName, IDInfo, Actor, bHide);
			this.m_ActorControllerList.Add(ActorName, miniDramaActorController);
			return true;
		}

		public bool AddChar(string ActorName, string CharKindCode, float X, float Y, int Angle, bool Hide)
		{
			if (string.IsNullOrEmpty(ActorName) || string.IsNullOrEmpty(CharKindCode))
			{
				return false;
			}
			if (this.IsActor(ActorName))
			{
				TsLog.LogWarning(string.Format("[EventTriggerMiniDrama] ActorManager : Have Equal CharName {0}", ActorName), new object[0]);
				return false;
			}
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(CharKindCode);
			if (charKindInfoFromCode == null)
			{
				return false;
			}
			NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
			nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfoFromCode.GetName());
			nEW_MAKECHAR_INFO.CharPos.x = X;
			nEW_MAKECHAR_INFO.CharPos.y = EventTriggerGameHelper.GetGroundPosition(X, Y);
			nEW_MAKECHAR_INFO.CharPos.z = Y;
			float f = (float)Angle * 0.0174532924f;
			nEW_MAKECHAR_INFO.Direction.x = 1f * Mathf.Sin(f);
			nEW_MAKECHAR_INFO.Direction.y = 0f;
			nEW_MAKECHAR_INFO.Direction.z = 1f * Mathf.Cos(f);
			nEW_MAKECHAR_INFO.CharKind = charKindInfoFromCode.GetCharKind();
			nEW_MAKECHAR_INFO.CharKindType = 3;
			nEW_MAKECHAR_INFO.CharUnique = NrTSingleton<NkQuestManager>.Instance.GetClientNpcUnique();
			int id = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, false);
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(id);
			NkCharIDInfo nkCharIDInfo = new NkCharIDInfo();
			nkCharIDInfo.m_nClientID = @char.GetID();
			nkCharIDInfo.m_nCharUnique = @char.GetCharUnique();
			nkCharIDInfo.m_nWorldID = @char.GetWorldID();
			@char.SetExceptHideForLoad(true);
			return this.AddActor(ActorName, nkCharIDInfo, true, Hide);
		}

		public void DelActor(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Del();
			this.m_ActorControllerList.Remove(ActorName);
		}

		public bool IsMakeChar(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			return !(actorController == null) && actorController.IsMake();
		}

		public void DelActorAll()
		{
			foreach (MiniDramaActorController current in this.m_ActorControllerList.Values)
			{
				current.Del();
			}
			this.m_ActorControllerList.Clear();
		}

		public void CloseAllActor()
		{
			MiniDramaActorController actorController = this.GetActorController(EventTriggerMiniDrama.ActorManager.GeneralName);
			if (actorController == null)
			{
				return;
			}
			actorController.Show(true);
			actorController.Fade(0.5f, 1f, 3.2f);
			foreach (string current in this.m_ActorControllerList.Keys)
			{
				if (!(current == EventTriggerMiniDrama.ActorManager.GeneralName))
				{
					MiniDramaActorController miniDramaActorController = this.m_ActorControllerList[current];
					if (!(miniDramaActorController == null))
					{
						if (miniDramaActorController.RealActor)
						{
							miniDramaActorController.Fade(0.8f, 0f, 3.2f);
						}
					}
				}
			}
		}

		public void RecoveryGeneral()
		{
			MiniDramaActorController actorController = this.GetActorController(EventTriggerMiniDrama.ActorManager.GeneralName);
			if (actorController == null)
			{
				return;
			}
		}

		public void ShowActor(string ActorName, bool Show)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Show(Show);
		}

		public void FadeActor(string ActorName, float StartValue, float EndValue, float ActionTime)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Fade(StartValue, EndValue, ActionTime);
		}

		public bool IsFadeActor(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			return !(actorController == null) && actorController.IsFade();
		}

		public void ShowTalk(string ActorName, string Talk, float TalkTime)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Talk(Talk, TalkTime);
		}

		public void HideTalk(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.HideTalk();
		}

		public void ShowCaption(string ActorName, string Talk)
		{
			UI_MiniDramaCaption uI_MiniDramaCaption = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINIDRAMACAPTION_DLG) as UI_MiniDramaCaption;
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController != null)
			{
				string name = string.Empty;
				if (actorController.GetCharBase().GetID() == 1)
				{
					name = actorController.GetCharBase().GetCharName();
				}
				else
				{
					name = actorController.GetCharBase().GetCharKindInfo().GetName();
				}
				uI_MiniDramaCaption.SetName(name);
			}
			else
			{
				uI_MiniDramaCaption.SetName(string.Empty);
			}
			uI_MiniDramaCaption.ShowBG(true);
			uI_MiniDramaCaption.SetTalk(Talk);
		}

		public void HideCaption(string Talk)
		{
			UI_MiniDramaCaption uI_MiniDramaCaption = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINIDRAMACAPTION_DLG) as UI_MiniDramaCaption;
			if (uI_MiniDramaCaption != null && uI_MiniDramaCaption.GetCurrentTalk() == Talk)
			{
				uI_MiniDramaCaption.ShowBG(false);
			}
		}

		public void ShowEmoticon(string ActorName, string EmoticonName, float TalkTime)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.Emoticon(EmoticonName, TalkTime);
		}

		public void HideEmoticon(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.HideEmoticon();
		}

		public void ShowChatEmoticon(string ActorName, string EmoticonText, float TalkTime)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.ChatEmoticon(EmoticonText, TalkTime);
		}

		public void HideChatEmoticon(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return;
			}
			actorController.HideChatEmoticon();
		}

		public Vector3 GetPosition(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return Vector3.zero;
			}
			return actorController.transform.position;
		}

		public GameObject GetGameObject(string ActorName)
		{
			MiniDramaActorController actorController = this.GetActorController(ActorName);
			if (actorController == null)
			{
				return null;
			}
			return actorController.transform.parent.gameObject;
		}
	}

	public class MiniDramaCamera
	{
		private MiniDramaCameraController _Camera;

		public bool IsCameraAction
		{
			get
			{
				return this._Camera != null && this._Camera.IsCameraAction;
			}
		}

		public void Standby()
		{
			this._Camera = Camera.main.gameObject.AddComponent<MiniDramaCameraController>();
			CameraController.EnableControll = false;
		}

		public void Cut(Action action)
		{
			if (this._Camera != null)
			{
				this._Camera.Cut(action);
			}
		}

		public void Destory(Action action)
		{
			CameraController.EnableControll = true;
			if (this._Camera != null)
			{
				this._Camera.Destory();
			}
		}

		public void Quake(float QuakeScaleX, float QuakeScaleY, float ActionTime)
		{
			if (this._Camera != null)
			{
				this._Camera.Quake(QuakeScaleX, QuakeScaleY, ActionTime);
			}
		}

		public void Move(Vector3 Position, float PositionMoveTime, Vector3 Angle, float AngleMoveTime, float FieldOfView, float FOVMoveTime, float ActionTime, iTween.EaseType EaseType)
		{
			if (this._Camera != null)
			{
				this._Camera.Move(Position, PositionMoveTime, Angle, AngleMoveTime, FieldOfView, FOVMoveTime, ActionTime, EaseType);
			}
		}

		public void Position(Vector3 Position)
		{
			if (this._Camera != null)
			{
				this._Camera.Position(Position);
			}
		}

		public void Angle(Vector3 Angle)
		{
			if (this._Camera != null)
			{
				this._Camera.Angle(Angle);
			}
		}

		public void FieldOfView(float FOV)
		{
			if (this._Camera != null)
			{
				this._Camera.FieldOfView(FOV);
			}
		}

		public void CameraPanning(float Angle, float ActionTime)
		{
			if (this._Camera != null)
			{
				this._Camera.Panning(Angle, ActionTime);
			}
		}

		public void Fade(float Red, float Green, float Blue, float FadeInTime, float DurationTime, float FadeOutTime)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Repeat;
			Color[] pixels = texture2D.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i].a = 1f;
				pixels[i].r = Red;
				pixels[i].g = Green;
				pixels[i].b = Blue;
			}
			texture2D.SetPixels(pixels);
			texture2D.Apply();
		}
	}

	private EventTriggerMiniDrama.ActorManager _ActorManger;

	private EventTriggerMiniDrama.MiniDramaCamera _Camera;

	private bool _ShowTime;

	private TsAudio m_BGMAudio;

	private bool _bNpcTalk;

	private List<MonoBehaviour> _EnableComponent = new List<MonoBehaviour>();

	public bool ShowTime
	{
		get
		{
			return this._ShowTime;
		}
	}

	private EventTriggerMiniDrama()
	{
		this._ActorManger = new EventTriggerMiniDrama.ActorManager();
		this._Camera = new EventTriggerMiniDrama.MiniDramaCamera();
	}

	public bool Initialize()
	{
		if (Camera.main != null)
		{
			MiniDramaCameraController component = Camera.main.GetComponent<MiniDramaCameraController>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		this._ActorManger.DelActorAll();
		return true;
	}

	public bool StartMiniDrama()
	{
		this.m_BGMAudio = null;
		UIDataManager.MuteMiniDramaSound(true);
		if (!UIDataManager.MuteBGM && NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
		{
			TsAudioAdapterBGM[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsAudioAdapterBGM)) as TsAudioAdapterBGM[];
			TsAudioAdapterBGM[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TsAudioAdapterBGM tsAudioAdapterBGM = array2[i];
				if (tsAudioAdapterBGM != null && tsAudioAdapterBGM.gameObject != null)
				{
					string text = tsAudioAdapterBGM.gameObject.name.ToLower();
					if (text.Contains("bgm"))
					{
						this.m_BGMAudio = tsAudioAdapterBGM.GetAudioEx();
						this.m_BGMAudio.Stop();
						break;
					}
				}
			}
		}
		foreach (MonoBehaviour current in this._EnableComponent)
		{
			current.enabled = false;
		}
		NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(true);
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(false, false, false);
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.NPCTALK_DLG);
		if (form != null)
		{
			this._bNpcTalk = form.Visible;
		}
		NrTSingleton<FormsManager>.Instance.HideAll();
		UI_MiniDramaCaption uI_MiniDramaCaption = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MINIDRAMACAPTION_DLG) as UI_MiniDramaCaption;
		if (uI_MiniDramaCaption != null)
		{
			if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MINIDRAMACAPTION_DLG))
			{
				uI_MiniDramaCaption.Show();
				uI_MiniDramaCaption.ShowBG(false);
			}
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MINIDRAMACAPTION_DLG);
		}
		this._Camera.Standby();
		this._ShowTime = true;
		if (TsPlatform.IsMobile)
		{
		}
		return true;
	}

	public void ReadyMiniDrama()
	{
	}

	public void CloseMiniDrama()
	{
		this._ActorManger.RecoveryGeneral();
		this._Camera.Cut(new Action(this.EndMiniDrama));
	}

	public void ClearMiniDrama()
	{
		if (null != BugFixAudio.PlayOnceRoot)
		{
			int childCount = BugFixAudio.PlayOnceRoot.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = BugFixAudio.PlayOnceRoot.transform.GetChild(i);
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
		UIDataManager.MuteMiniDramaSound(false);
		if (!UIDataManager.MuteBGM && this.m_BGMAudio != null)
		{
			this.m_BGMAudio.Play();
		}
		this._Camera.Destory(null);
		foreach (MonoBehaviour current in this._EnableComponent)
		{
			if (current != null)
			{
				current.enabled = true;
			}
		}
		this._EnableComponent.Clear();
		NrTSingleton<NkCharManager>.Instance.ShowHideAll(true, true, true);
		this._ActorManger.DelActorAll();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MINIDRAMACAPTION_DLG);
		this._ShowTime = false;
		if (this._bNpcTalk)
		{
			NrTSingleton<FormsManager>.Instance.Show(G_ID.NPCTALK_DLG);
		}
		else
		{
			NrTSingleton<NkClientLogic>.Instance.SetNPCTalkState(false);
			if (NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
			{
				NrTSingleton<FormsManager>.Instance.Main_UI_Show(FormsManager.eMAIN_UI_VISIBLE_MODE.COMMON);
			}
		}
		if (TsPlatform.IsMobile)
		{
		}
		NrTSingleton<NkQuestManager>.Instance.AutoQuestExcute();
	}

	private void EndMiniDrama()
	{
		this.ClearMiniDrama();
	}

	public bool AddGeneral(string ActorName, bool Hide)
	{
		return this._ActorManger.AddGeneral(ActorName, Hide);
	}

	public bool MakeActor(string ActorName, string CharKindCode, float X, float Y, short ViewAngle, bool Hide)
	{
		return this._ActorManger.AddChar(ActorName, CharKindCode, X, Y, (int)ViewAngle, Hide);
	}

	public bool IsMakeActor(string ActorName)
	{
		return this._ActorManger.IsMakeChar(ActorName);
	}

	public void DelActor(string ActorName)
	{
		this._ActorManger.DelActor(ActorName);
	}

	public void ShowActor(string ActorName, bool Show)
	{
		this._ActorManger.ShowActor(ActorName, Show);
	}

	public void FadeActor(string ActorName, float StartValue, float EndValue, float ActionTime)
	{
		this._ActorManger.FadeActor(ActorName, StartValue, EndValue, ActionTime);
	}

	public bool IsFadeActor(string ActorName)
	{
		return this._ActorManger.IsFadeActor(ActorName);
	}

	public void ShowTalk(string ActorName, string TalkKey, float TalkTime)
	{
		this._ActorManger.ShowTalk(ActorName, this.GetText(TalkKey), TalkTime);
	}

	public void HideTalk(string ActorName)
	{
		this._ActorManger.HideTalk(ActorName);
	}

	public void ShowCaption(string ActorName, string TalkKey, float TalkTime)
	{
		this._ActorManger.ShowCaption(ActorName, this.GetText(TalkKey));
	}

	public void HideCaption(string TalkKey)
	{
		this._ActorManger.HideCaption(this.GetText(TalkKey));
	}

	public void ShowEmoticon(string ActorName, string EmoticonName, float TalkTime)
	{
		this._ActorManger.ShowEmoticon(ActorName, EmoticonName, TalkTime);
	}

	public void HideEmoticon(string ActorName)
	{
		this._ActorManger.HideEmoticon(ActorName);
	}

	public void ShowChatEmoticon(string ActorName, string EmoticonName, float TalkTime)
	{
		this._ActorManger.ShowChatEmoticon(ActorName, EmoticonName, TalkTime);
	}

	public void HideChatEmoticon(string ActorName)
	{
		this._ActorManger.HideChatEmoticon(ActorName);
	}

	public bool IsMoveActor(string ActorName)
	{
		return this._ActorManger.IsMove(ActorName);
	}

	public float AniActor(string ActorName, string Ani)
	{
		return this._ActorManger.AniActor(ActorName, Ani);
	}

	public void MoveActor(string ActorName, float x, float y, float time)
	{
		this._ActorManger.Move(ActorName, x, y, time);
	}

	public void WalkActor(string ActorName, float x, float y, float time)
	{
		this._ActorManger.Walk(ActorName, x, y, time);
	}

	public void RunActor(string ActorName, float x, float y, float time)
	{
		this._ActorManger.Run(ActorName, x, y, time);
	}

	public void StopActor(string ActorName)
	{
		this._ActorManger.Stop(ActorName);
	}

	public void RotateActor(string ActorName, float Angle, float time)
	{
		this._ActorManger.RotateActor(ActorName, Angle, time);
	}

	private MiniDramaActorController GetActorController(string ActorName)
	{
		return this._ActorManger.GetController(ActorName);
	}

	public GameObject GetActorObject(string ActorName)
	{
		MiniDramaActorController controller = this._ActorManger.GetController(ActorName);
		if (controller != null)
		{
			return controller.GetGameObject();
		}
		return null;
	}

	public string GetText(string Key)
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromMINIDRAMA(Key);
	}

	public void CameraMove(Vector3 Position, float PositionMoveTime, Vector3 Angle, float AngleMoveTime, float FieldOfView, float FOVMoveTime, float ActionTime, iTween.EaseType EaseType)
	{
		if (this._Camera != null)
		{
			this._Camera.Move(Position, PositionMoveTime, Angle, AngleMoveTime, FieldOfView, FOVMoveTime, ActionTime, EaseType);
		}
	}

	public void CameraQuake(float QuakeScaleX, float QuakeScaleY, float ActionTime)
	{
		this._Camera.Quake(QuakeScaleX, QuakeScaleY, ActionTime);
	}

	public void CameraFade(float Red, float Green, float Blue, float FadeInTime, float DurationTime, float FadeOutTime)
	{
		this._Camera.Fade(Red, Green, Blue, FadeInTime, DurationTime, FadeOutTime);
	}

	public bool IsCameraAction()
	{
		return this._Camera.IsCameraAction;
	}
}
