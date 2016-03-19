using Ndoors.Framework.Stage;
using StageHelper;
using System;
using UnityEngine;

public class NmMotionBlurLoading : MonoBehaviour
{
	private float m_Time;

	private float m_SendTime;

	public float SHOW_TIME;

	public float m_Speed = 1.7f;

	private CameraController mCam;

	private maxCamera WCam;

	public static void GoToNormalBattle()
	{
		NrSound.ImmedatePlay("UI_SFX", "BATTLE", "ENTER", true);
		NmMotionBlurLoading.ToBattle();
	}

	public static void GoToBlurNormalBattle()
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null && nrCharUser.Get3DChar() != null)
		{
			GameObject rootGameObject = nrCharUser.Get3DChar().GetRootGameObject();
			if (rootGameObject)
			{
				rootGameObject.AddComponent<NmMotionBlurLoading>();
			}
			NrSound.ImmedatePlay("UI_SFX", "BATTLE", "ENTER", true);
		}
	}

	private void Start()
	{
		this.Init();
	}

	private void Init()
	{
		Camera main = Camera.main;
		this.WCam = main.GetComponent<maxCamera>();
		if (this.WCam)
		{
			this.WCam.enabled = false;
		}
		this.mCam = main.gameObject.AddComponent<CameraController>();
		this.mCam.yMinLimit = 45f;
		this.mCam.yMaxLimit = 45f;
		this.mCam.Angley = 45f;
		this.mCam.distanceMin = 20f;
		this.mCam.m_bMotionBlurCamera = true;
		this.mCam.distanceMax = 200f;
		this.mCam.distance = 150f;
		this.mCam.fFov = 33f;
		this.mCam.ffovMax = 33f;
		this.mCam.ffovMin = 33f;
		this.mCam.InitSrcPos(base.transform.position);
		this.m_Time = Time.time + this.SHOW_TIME;
		this.m_SendTime = Time.time + this.SHOW_TIME;
	}

	private void Release()
	{
		UnityEngine.Object.Destroy(this);
		if (this.WCam)
		{
			this.WCam.enabled = true;
		}
		if (this.mCam)
		{
			UnityEngine.Object.Destroy(this.mCam);
		}
	}

	private static void ToBattle()
	{
		TsAudioBGM.SaveCurrentBGMPlayTime();
		if (!Scene.IsCurScene(Scene.Type.BATTLE))
		{
			CommonTasks.GotoBattleReserve();
		}
	}

	private void Update()
	{
		if (base.enabled)
		{
			this.UpdateRotate();
			if (Time.time > this.m_SendTime)
			{
				NmMotionBlurLoading.ToBattle();
			}
			if (Time.time > this.m_Time)
			{
				this.Release();
			}
		}
	}

	private void UpdateRotate()
	{
		this.mCam.distance -= this.m_Speed;
	}
}
