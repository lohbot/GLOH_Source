using GAME;
using System;
using System.Collections;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MiniDramaActorController : MonoBehaviour
{
	public enum EMOTICON
	{
		ExclamAtion,
		Question,
		MAX_EMOTICON
	}

	public interface IActorAction
	{
		void Update(ref MiniDramaActorController.Actor kActor);

		bool IsDestory();
	}

	private class ActorMake : MiniDramaActorController.IActorAction
	{
		private string _CharName = string.Empty;

		private int _CharID;

		private GameObject _goController;

		public bool m_Hide;

		public bool m_RealActor = true;

		private bool _Ready;

		public ActorMake(string CharName, int CharID, GameObject go, bool RealActor, bool Hide)
		{
			this._CharName = CharName;
			this._CharID = CharID;
			this._goController = go;
			this.m_RealActor = RealActor;
			this.m_Hide = Hide;
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(this._CharID);
			if (@char != null && @char.IsShaderRecovery())
			{
				GameObject rootGameObject = @char.Get3DChar().GetRootGameObject();
				if (this._goController != null)
				{
					this._goController.transform.parent = rootGameObject.transform;
					this._goController.transform.localPosition = Vector3.zero;
				}
				if (MiniDramaActorController.EnableTypeList != null && MiniDramaActorController.EnableTypeList.Length > 0)
				{
					Type[] enableTypeList = MiniDramaActorController.EnableTypeList;
					for (int i = 0; i < enableTypeList.Length; i++)
					{
						Type type = enableTypeList[i];
						MonoBehaviour monoBehaviour = rootGameObject.GetComponent(type) as MonoBehaviour;
						if (monoBehaviour != null)
						{
							monoBehaviour.enabled = false;
						}
					}
				}
				TsLog.Log("[ActorController] {0}", new object[]
				{
					this.m_Hide
				});
				@char.SetShowHide3DModel(!this.m_Hide, !this.m_Hide, !this.m_Hide);
				kActor.SetActor(@char, this._CharName, this.m_RealActor, !this.m_Hide);
				kActor.SetIdleAni(@char.GetCharAnimation().GetCurrentAniType());
				this._Ready = true;
			}
		}

		public bool IsDestory()
		{
			return this._Ready;
		}
	}

	private class ActorRotate : MiniDramaActorController.IActorAction
	{
		private ObjectRotate _Rotate;

		public ActorRotate(MiniDramaActorController.Actor kActor, float Angle, float time)
		{
			GameObject charObject = kActor.m_ActorBase.GetCharObject();
			Hashtable args = iTween.Hash(new object[]
			{
				"x",
				charObject.transform.localEulerAngles.x,
				"y",
				Angle,
				"z",
				charObject.transform.localEulerAngles.z,
				"time",
				time
			});
			iTween.RotateTo(charObject, args);
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
		}

		public bool IsDestory()
		{
			return this._Rotate == null;
		}
	}

	private class ActorMove : MiniDramaActorController.IActorAction
	{
		private const float RUNSPEED = 0.1f;

		private Vector3 _Dest = Vector3.zero;

		private Vector3 _MoveDir = Vector3.zero;

		private float _Distance;

		private float _MoveTime;

		private float _StartTime;

		protected eCharAnimationType _MoveAni = eCharAnimationType.None;

		public ActorMove(MiniDramaActorController.Actor kActor, Vector3 Src, Vector3 Dest, float time)
		{
			this._Dest = Dest;
			this._MoveDir = Dest - Src;
			this._MoveDir.y = 0f;
			this._MoveDir.Normalize();
			if (this._MoveDir != Vector3.zero)
			{
				kActor.m_ActorBase.GetCharObject().transform.localRotation = Quaternion.LookRotation(this._MoveDir, Vector3.up);
			}
			this._Distance = Vector3.Distance(Src, Dest);
			this._MoveTime = time;
			this._StartTime = Time.time;
		}

		public virtual void Update(ref MiniDramaActorController.Actor kActor)
		{
			if (this.IsDestory())
			{
				GameObject charObject = kActor.m_ActorBase.GetCharObject();
				charObject.transform.localPosition = this._Dest;
				charObject.transform.LookAt(this._Dest);
				kActor.SetIdleAni(this._MoveAni);
			}
			else if (this._Distance != 0f)
			{
				CharacterController charController = kActor.m_ActorBase.Get3DChar().GetCharController();
				charController.Move(this._MoveDir * (this._Distance / this._MoveTime) * Time.deltaTime);
			}
		}

		public virtual bool IsDestory()
		{
			return Math.Abs(Time.time - this._StartTime) >= this._MoveTime;
		}
	}

	private class ActorWalk : MiniDramaActorController.ActorMove
	{
		public ActorWalk(MiniDramaActorController.Actor kActor, Vector3 Src, Vector3 Dest, float time) : base(kActor, Src, Dest, time)
		{
			this._MoveAni = eCharAnimationType.Walk1;
			kActor.SetAnimation(ref this._MoveAni);
		}
	}

	private class ActorRun : MiniDramaActorController.ActorMove
	{
		public ActorRun(MiniDramaActorController.Actor kActor, Vector3 Src, Vector3 Dest, float time) : base(kActor, Src, Dest, time)
		{
			this._MoveAni = eCharAnimationType.Run1;
			kActor.SetAnimation(ref this._MoveAni);
		}
	}

	private class ActorTalk : MiniDramaActorController.IActorAction
	{
		private UI_MiniDramaTalk _Talk;

		public ActorTalk(MiniDramaActorController.Actor kActor, ref UI_MiniDramaTalk TalkUI, string strText, float ActionTime)
		{
			if (TalkUI == null)
			{
			}
			this._Talk = TalkUI;
			this._Talk.Init(kActor.m_ActorBase);
			this._Talk.SetTalk(strText, ActionTime);
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
			if (!this.IsDestory())
			{
				this._Talk.Update();
				this._Talk.UpatePotition();
			}
		}

		public bool IsDestory()
		{
			return this._Talk == null || !this._Talk.m_ShowUI;
		}
	}

	private class ActorChatEmoticon : MiniDramaActorController.IActorAction
	{
		private float _ActionTime;

		public ActorChatEmoticon(MiniDramaActorController.Actor kActor, string EmoticonText, float ActionTime)
		{
			UIEmoticonManager instance = NrTSingleton<UIEmoticonManager>.Instance;
			string chattext = instance.UIEmoticonENGKey[EmoticonText];
			kActor.m_ActorBase.MakeChatText(chattext, true);
			this._ActionTime = ActionTime + Time.time;
			this._ActionTime = 5f + Time.time;
			string audioKey = string.Format("{0}_EMOTICON", EmoticonText.ToUpper());
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "QUEST_SFX", audioKey, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
		}

		public bool IsDestory()
		{
			return Time.time >= this._ActionTime;
		}
	}

	private class ActorFade : MiniDramaActorController.IActorAction
	{
		private float _ActionTime;

		private float _EndValue;

		public ActorFade(MiniDramaActorController.Actor kActor, float StartValue, float EndValue, float ActionTime)
		{
			this._ActionTime = ActionTime + Time.time;
			kActor.m_ActorBase.Set3DCharStep(NrCharBase.e3DCharStep.CHARACTION);
			this._EndValue = EndValue;
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
			if (this.IsDestory())
			{
				kActor.RecevoryShader();
				if (this._EndValue <= 0f)
				{
					kActor.SetActorShow(false);
				}
			}
		}

		public bool IsDestory()
		{
			return Time.time >= this._ActionTime;
		}
	}

	private class ActorRide : MiniDramaActorController.IActorAction
	{
		private bool _Ride;

		public ActorRide(MiniDramaActorController.Actor kActor, string RideName)
		{
		}

		public void Update(ref MiniDramaActorController.Actor kActor)
		{
		}

		public bool IsDestory()
		{
			return this._Ride;
		}
	}

	public class Actor : IDisposable
	{
		public NrCharBase m_ActorBase;

		public string m_ActorName = string.Empty;

		public bool m_ActorShow;

		public bool m_RealActor = true;

		private Vector3 _PrePosition = Vector3.zero;

		private Quaternion _PreRotate = Quaternion.identity;

		public void Dispose()
		{
			if (this.m_ActorBase == null)
			{
				return;
			}
			this.RecevoryShader();
			if (this._PrePosition != Vector3.zero)
			{
				this.m_ActorBase.GetCharObject().transform.localPosition = this._PrePosition;
			}
			if (this._PreRotate != Quaternion.identity)
			{
				this.m_ActorBase.GetCharObject().transform.localRotation = this._PreRotate;
			}
		}

		public void SetActor(NrCharBase kCharBase, string ActorName, bool RealActor, bool bShow)
		{
			this.m_ActorBase = kCharBase;
			if (this.m_ActorBase.Get3DChar() != null)
			{
				this.m_ActorBase.Get3DChar().SetMiniDramaChar();
			}
			this.m_ActorBase.GetCharAnimation().ClearNextAni();
			this.m_ActorName = ActorName;
			this.m_RealActor = RealActor;
			this.m_ActorShow = bShow;
			if (!this.m_RealActor && !this.m_ActorName.Equals(EventTriggerMiniDrama.ActorManager.GeneralName))
			{
				this._PrePosition = kCharBase.GetCharObject().transform.localPosition;
				this._PreRotate = kCharBase.GetCharObject().transform.localRotation;
			}
		}

		public void SetActorShow(bool bShow)
		{
			if (this.m_ActorShow != bShow)
			{
				this.m_ActorBase.SetShowHide3DModel(bShow, bShow, bShow);
				this.m_ActorShow = bShow;
			}
		}

		public void SetAnimation(ref eCharAnimationType AniType)
		{
			this.SetCurrentAnimation(AniType);
			AniType = this.m_ActorBase.GetCharAnimation().GetCurrentAniType();
		}

		public float PlayTimeAniState(eCharAnimationType AniType)
		{
			return this.m_ActorBase.GetCharAnimation().PlayTimeAniState(AniType);
		}

		private bool Decide(ref eCharAnimationType AniType)
		{
			if (this.m_ActorBase.GetCharAnimation().GetCurrentAniType() == AniType)
			{
				return false;
			}
			eCharAnimationType eCharAnimationType = AniType;
			if (eCharAnimationType != eCharAnimationType.Stay1)
			{
				if (eCharAnimationType == eCharAnimationType.TalkStart1)
				{
					if (this.m_ActorBase.GetCharAnimation().GetCurrentAniType() != eCharAnimationType.Stay1)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		public void SetIdleAni(eCharAnimationType PreAniType)
		{
			if (this.m_ActorBase.GetCharAnimation().GetCurrentAniType() == PreAniType)
			{
				this.SetCurrentAnimation(eCharAnimationType.Stay1);
			}
		}

		private void SetCurrentAnimation(eCharAnimationType AniType)
		{
			this.m_ActorBase.GetCharAnimation().SetCurrentAniType(AniType, 0.3f);
		}

		public void RecevoryShader()
		{
		}
	}

	private NkCharIDInfo _IDInfo;

	private UI_MiniDramaTalk _Talk;

	private UI_MiniDramaEmoticon _Emoticon;

	private List<MiniDramaActorController.IActorAction> _ActorAction = new List<MiniDramaActorController.IActorAction>();

	private MiniDramaActorController.Actor _Actor = new MiniDramaActorController.Actor();

	private static Type[] EnableTypeList = new Type[]
	{
		typeof(NmUserBehaviour)
	};

	private bool _Start;

	private List<string> AniList;

	public bool RealActor
	{
		get
		{
			return this._Actor == null || this._Actor.m_RealActor;
		}
	}

	private void Start()
	{
		if (this._IDInfo == null)
		{
			this.Del();
		}
		this._Start = true;
	}

	private void Update()
	{
		foreach (MiniDramaActorController.IActorAction current in this._ActorAction)
		{
			current.Update(ref this._Actor);
		}
		this._ActorAction.RemoveAll((MiniDramaActorController.IActorAction value) => value.IsDestory());
	}

	public void SetChar(string CharName, NkCharIDInfo IDInfo, bool RealActor, bool Hide)
	{
		if (this._IDInfo == null)
		{
			this._IDInfo = IDInfo;
			this._ActorAction.Add(new MiniDramaActorController.ActorMake(CharName, IDInfo.m_nClientID, base.gameObject, RealActor, Hide));
		}
	}

	public NrCharBase GetCharBase()
	{
		return NrTSingleton<NkCharManager>.Instance.GetChar(this._IDInfo.m_nClientID);
	}

	public bool IsValidActor()
	{
		return this._Actor != null && this._Actor.m_ActorBase != null && this._Actor.m_ActorBase.GetCharObject() != null;
	}

	public GameObject GetGameObject()
	{
		NrCharBase charBase = this.GetCharBase();
		if (charBase != null)
		{
			return charBase.GetCharObject();
		}
		return null;
	}

	public MiniDramaActorController.IActorAction GetActionItem<T>() where T : MiniDramaActorController.IActorAction
	{
		return this._ActorAction.Find((MiniDramaActorController.IActorAction value) => value is T);
	}

	public bool IsActionItem<T>() where T : MiniDramaActorController.IActorAction
	{
		return this.GetActionItem<T>() != null;
	}

	private void RemoveActionItem<T>() where T : MiniDramaActorController.IActorAction
	{
		MiniDramaActorController.IActorAction actionItem = this.GetActionItem<T>();
		this._ActorAction.Remove(actionItem);
	}

	public void PlayAni(string Ani)
	{
	}

	public bool IsMake()
	{
		return this._Start && !this.IsActionItem<MiniDramaActorController.ActorMake>();
	}

	public void Show(bool Show)
	{
		if (!this.IsMake())
		{
			MiniDramaActorController.ActorMake actorMake = this.GetActionItem<MiniDramaActorController.ActorMake>() as MiniDramaActorController.ActorMake;
			actorMake.m_Hide = !Show;
		}
		else
		{
			this._Actor.SetActorShow(Show);
		}
	}

	public void Fade(float StartValue, float EndValue, float ActionTime)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorFade(this._Actor, StartValue, EndValue, ActionTime));
	}

	public bool IsFade()
	{
		return this.IsActionItem<MiniDramaActorController.ActorFade>();
	}

	public void Move(float x, float y, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		Vector3 localPosition = base.transform.parent.localPosition;
		float groundPosition = EventTriggerGameHelper.GetGroundPosition(x, y);
		Vector3 dest = new Vector3(x, groundPosition, y);
		dest.y += 0.2f;
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorMove(this._Actor, localPosition, dest, time));
	}

	public void Walk(float x, float y, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		Vector3 localPosition = base.transform.parent.localPosition;
		Vector3 dest = new Vector3(x, EventTriggerGameHelper.GetGroundPosition(x, y), y);
		dest.y += 0.2f;
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorWalk(this._Actor, localPosition, dest, time));
	}

	public void Run(float x, float y, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		Vector3 localPosition = base.transform.parent.localPosition;
		Vector3 dest = new Vector3(x, EventTriggerGameHelper.GetGroundPosition(x, y), y);
		dest.y += 0.2f;
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorRun(this._Actor, localPosition, dest, time));
	}

	public void StopMove()
	{
		this.RemoveActionItem<MiniDramaActorController.ActorMove>();
	}

	public bool IsMove()
	{
		return this.IsActionItem<MiniDramaActorController.ActorMove>();
	}

	public void Rotate(float Angle, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorRotate(this._Actor, Angle, time));
	}

	public void Talk(string talk, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		this.HideChatEmoticon();
		this._Actor.SetActorShow(true);
		this._ActorAction.Add(new MiniDramaActorController.ActorTalk(this._Actor, ref this._Talk, talk, time));
	}

	public void HideTalk()
	{
		if (this._Talk != null)
		{
			this._Talk.m_ShowUI = false;
		}
	}

	public void Emoticon(string EmoticonName, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		this._Actor.SetActorShow(true);
	}

	public void HideEmoticon()
	{
		if (this._Emoticon != null)
		{
			this._Emoticon.m_ShowUI = false;
		}
	}

	public void ChatEmoticon(string EmoticonText, float time)
	{
		if (!this.IsValidActor())
		{
			return;
		}
		if (!string.IsNullOrEmpty(EmoticonText))
		{
			this.HideTalk();
			this._Actor.SetActorShow(true);
			this._ActorAction.Add(new MiniDramaActorController.ActorChatEmoticon(this._Actor, EmoticonText, time));
		}
	}

	public void HideChatEmoticon()
	{
	}

	public float Ani(string Ani)
	{
		if (!this.IsValidActor())
		{
			return 0f;
		}
		if (this.AniList == null)
		{
			this.AniList = new List<string>();
			string[] names = Enum.GetNames(typeof(eCharAnimationType));
			this.AniList.AddRange(names);
		}
		if (this.AniList.Contains(Ani))
		{
			eCharAnimationType aniType = (eCharAnimationType)((int)Enum.Parse(typeof(eCharAnimationType), Ani));
			this._Actor.SetAnimation(ref aniType);
			return this._Actor.PlayTimeAniState(aniType);
		}
		return 0f;
	}

	public void Del()
	{
		if (this._Talk != null)
		{
			this._Talk.Close();
			this._Talk = null;
		}
		if (this._Emoticon != null)
		{
			this._Emoticon.Close();
			this._Emoticon = null;
		}
		this._ActorAction.Clear();
		this._Actor.Dispose();
		if (this._Actor.m_RealActor)
		{
			if (this._Actor.m_ActorBase != null)
			{
				this._Actor.m_ActorBase.SetShowHide3DModel(false, false, false);
			}
			NrTSingleton<NkCharManager>.Instance.DeleteChar(this._IDInfo.m_nClientID);
		}
		else
		{
			NrCharBase charBase = this.GetCharBase();
			if (charBase != null)
			{
				GameObject charObject = charBase.GetCharObject();
				if (MiniDramaActorController.EnableTypeList != null && MiniDramaActorController.EnableTypeList.Length > 0)
				{
					Type[] enableTypeList = MiniDramaActorController.EnableTypeList;
					for (int i = 0; i < enableTypeList.Length; i++)
					{
						Type type = enableTypeList[i];
						MonoBehaviour monoBehaviour = charObject.GetComponent(type) as MonoBehaviour;
						if (monoBehaviour != null)
						{
							monoBehaviour.enabled = true;
						}
					}
				}
				this.Show(true);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
