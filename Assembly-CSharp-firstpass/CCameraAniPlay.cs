using System;
using System.Collections.Generic;
using UnityEngine;

public class CCameraAniPlay : NrTSingleton<CCameraAniPlay>
{
	private Camera m_Camera;

	private Dictionary<string, Animation> m_ActionAnis;

	private AniKeyPairManager m_aniKeyManager;

	private Queue<AniKeyPair> m_PlayAniQue;

	private AniKeyPair m_CurrentAni;

	private E_CHAR_SELECT_STEP m_NextStep;

	private Action<object> m_PlayAniEndEvent;

	private float m_fFixTime = 2f;

	private bool m_bSendEvent;

	private bool m_bSendCheck;

	private AudioSource m_IntroVoice;

	private CCameraAniPlay()
	{
		this.Init();
	}

	public void Init()
	{
		if (this.m_aniKeyManager == null)
		{
			this.m_aniKeyManager = new AniKeyPairManager();
		}
		if (this.m_Camera == null)
		{
			this.m_Camera = Camera.main;
		}
		if (this.m_ActionAnis == null)
		{
			this.m_ActionAnis = new Dictionary<string, Animation>();
		}
		if (this.m_PlayAniQue == null)
		{
			this.m_PlayAniQue = new Queue<AniKeyPair>();
		}
		if (this.m_ActionAnis.Count == 0)
		{
			GameObject gameObject = GameObject.Find("fx_charactercreate");
			if (gameObject == null)
			{
				return;
			}
			if (Screen.width == 1024)
			{
				gameObject.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(0.88f, 1f, 1f);
				Transform transform = gameObject.transform.FindChild("humanf");
				if (null != transform)
				{
					transform.transform.localPosition = new Vector3(-28f, -41f, 0f);
				}
			}
			else if (Screen.width == 960)
			{
				gameObject.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
				gameObject.transform.localScale = new Vector3(0.88f, 1f, 1f);
				Transform transform2 = gameObject.transform.FindChild("humanf");
				if (null != transform2)
				{
					transform2.transform.localPosition = new Vector3(-28.5f, -41f, 0f);
				}
			}
			Animation[] componentsInChildren = gameObject.GetComponentsInChildren<Animation>();
			if (componentsInChildren == null)
			{
				TsLog.LogError("CCameraAniPlay Animation == null", new object[0]);
				return;
			}
			this.m_IntroVoice = gameObject.GetComponentInChildren<AudioSource>();
			Animation[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Animation animation = array[i];
				foreach (AnimationState animationState in animation)
				{
					if (!(animationState.clip == null))
					{
						if (!this.m_ActionAnis.ContainsKey(animationState.clip.name))
						{
							this.m_ActionAnis.Add(animationState.clip.name, animation);
						}
						else
						{
							Debug.LogWarning("Have Ani = " + animationState.clip.name);
						}
					}
				}
			}
		}
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.INTRO1, "intro1", "intro");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.INTROTOCREATE, "introtocreate", "create");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOSELECT, "createtoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.ELFTOCREATE, "elftocreate", "create");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.FURRYTOCREATE, "furrytocreate", "create");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMANTOCREATE, "humantocreate", "create");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMANFTOCREATE, "humanftocreate", "create");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOELF, "createtoelf", "elf_up");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOFURRY, "createtofurry", "furry_up");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOHUMAN, "createtohumanm", "human_up");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOHUMANF, "createtohumanf", "humanf_up");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.CREATETOSELECT, "createtoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.ELFTOSELECT, "elftoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.FURRYTOSELECT, "furrytoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMANTOSELECT, "humantoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMANFTOSELECT, "humanftoselect", string.Empty);
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMAN_MALETOFEMALE, string.Empty, "human_maletofemale");
		this.m_aniKeyManager.AddData(E_CAMARA_STATE_ANI.HUMAN_FEMALETOMALE, string.Empty, "human_femaletomale");
	}

	public void Add(params object[] _states)
	{
		this.Add(E_CHAR_SELECT_STEP.NONE, null, 0f, _states);
	}

	public void Add(E_CHAR_SELECT_STEP NextStep, params object[] _states)
	{
		this.Add(NextStep, null, 0f, _states);
	}

	public void Add(Action<object> a_EndEvent, params object[] _states)
	{
		this.Add(E_CHAR_SELECT_STEP.NONE, a_EndEvent, 0f, _states);
	}

	public void Add(Action<object> a_EndEvent, float _fEventFixTime, params object[] _states)
	{
		this.Add(E_CHAR_SELECT_STEP.NONE, a_EndEvent, _fEventFixTime, _states);
	}

	public void Add(E_CHAR_SELECT_STEP NextStep, Action<object> a_EndEvent, params object[] _states)
	{
		this.Add(NextStep, a_EndEvent, 0f, _states);
	}

	public void Add(E_CHAR_SELECT_STEP NextStep, Action<object> a_EndEvent, float _fEventFixTime, params object[] _states)
	{
		for (int i = 0; i < _states.Length; i++)
		{
			object obj = _states[i];
			try
			{
				E_CAMARA_STATE_ANI e_CAMARA_STATE_ANI = (E_CAMARA_STATE_ANI)((int)obj);
				AniKeyPair data = this.m_aniKeyManager.GetData(e_CAMARA_STATE_ANI);
				if (data != null)
				{
					this.m_PlayAniQue.Enqueue(data);
				}
				else
				{
					TsLog.LogWarning("STATE ITEM == NULL  : {0}", new object[]
					{
						e_CAMARA_STATE_ANI
					});
				}
			}
			catch (Exception ex)
			{
				TsLog.LogError("Error State : {0} , Message : {1}", new object[]
				{
					obj.ToString(),
					ex.Message
				});
			}
		}
		this.m_fFixTime = _fEventFixTime;
		this.m_bSendEvent = false;
		this.m_NextStep = NextStep;
		this.m_PlayAniEndEvent = a_EndEvent;
	}

	public AniKeyPair GetCurrentAni()
	{
		return this.m_CurrentAni;
	}

	public void PlayAni()
	{
		if (this.m_Camera != null)
		{
			if (!this.m_Camera.animation.isPlaying)
			{
				AnimationClip clip = this.m_Camera.animation.GetClip(this.m_CurrentAni.CameraAniKey);
				if (clip != null)
				{
					this.m_CurrentAni.PlayTime = Time.time + clip.length;
					this.m_Camera.animation.Play(this.m_CurrentAni.CameraAniKey);
				}
				else
				{
					this.m_CurrentAni.PlayTime = 0f;
				}
				if (this.m_ActionAnis.ContainsKey(this.m_CurrentAni.ActionAniKey))
				{
					this.m_ActionAnis[this.m_CurrentAni.ActionAniKey].Play(this.m_CurrentAni.ActionAniKey);
					if (clip == null)
					{
						this.m_CurrentAni = null;
					}
				}
				this.m_bSendEvent = false;
				this.m_bSendCheck = false;
				TsLog.Log("PLAYANI CKey : {0}, AKey:{1} CSTATE : {2} LENGTH : {3}", new object[]
				{
					this.m_CurrentAni.CameraAniKey,
					this.m_CurrentAni.ActionAniKey,
					this.m_CurrentAni.CameraStste,
					clip.length
				});
			}
			else
			{
				if (!this.m_bSendCheck && !this.m_bSendEvent && this.m_CurrentAni != null && Time.time > this.m_CurrentAni.PlayTime - this.m_fFixTime)
				{
					this.m_bSendEvent = true;
					this.m_bSendCheck = true;
				}
				if (this.m_CurrentAni != null && Time.time > this.m_CurrentAni.PlayTime)
				{
					this.m_CurrentAni = null;
					this.m_bSendCheck = false;
				}
			}
		}
		else
		{
			TsLog.Log("m_Camera == null", new object[0]);
		}
	}

	public E_CHAR_SELECT_STEP Update()
	{
		if (this.m_CurrentAni != null)
		{
			this.PlayAni();
		}
		if (this.m_PlayAniQue.Count > 0 && this.m_CurrentAni == null)
		{
			this.m_CurrentAni = this.m_PlayAniQue.Dequeue();
		}
		if (NkInputManager.GetKeyUp(KeyCode.Escape))
		{
			this.SkipEvent();
		}
		if (this.m_PlayAniQue.Count == 0 && this.m_bSendEvent)
		{
			this.m_bSendEvent = false;
			if (this.m_PlayAniEndEvent != null)
			{
				this.m_PlayAniEndEvent(null);
				this.m_PlayAniEndEvent = null;
			}
			return this.m_NextStep;
		}
		return E_CHAR_SELECT_STEP.NONE;
	}

	public void SkipEvent()
	{
		if (this.m_PlayAniQue.Count != 0)
		{
			this.m_Camera.animation.Stop();
			if (this.m_ActionAnis.ContainsKey(this.m_CurrentAni.ActionAniKey))
			{
				this.m_ActionAnis[this.m_CurrentAni.ActionAniKey].Stop();
				this.m_IntroVoice.Stop();
			}
			for (int i = this.m_PlayAniQue.Count; i > 1; i--)
			{
				this.m_CurrentAni = this.m_PlayAniQue.Dequeue();
			}
			this.m_CurrentAni = null;
		}
	}

	public void Clear()
	{
		try
		{
			this.m_ActionAnis.Clear();
			this.m_aniKeyManager.Clear();
		}
		catch (Exception ex)
		{
			TsLog.LogError("{0}", new object[]
			{
				ex.Message
			});
		}
		finally
		{
			this.m_ActionAnis = null;
			this.m_aniKeyManager = null;
		}
	}
}
