using GAME;
using System;
using UnityEngine;

public class NrCharObject : NrCharBase
{
	private float maketime;

	public NrCharObject()
	{
		this.m_kPersonInfo = new NrPersonInfoNPC();
	}

	public override void Init()
	{
		base.Init();
		this.m_eCharKindType = eCharKindType.CKT_OBJECT;
		this.maketime = 0f;
	}

	public override bool Update()
	{
		if (!base.Update())
		{
			return false;
		}
		if (this.maketime > 0f && this.maketime <= Time.time)
		{
			base.SetAnimation(this.m_k3DChar.GetIdleAnimation());
			this.maketime = 0f;
		}
		return base.PostUpdate();
	}

	public override Nr3DCharBase Create3DGrahpicData()
	{
		if (this.m_k3DChar != null)
		{
			this.Release();
		}
		string strName = this.m_kPersonInfo.GetCharName() + "_" + base.GetID().ToString();
		this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Create3DChar<Nr3DCharObject>(base.GetID(), strName);
		if (this.m_k3DChar == null)
		{
			Debug.LogError("Create3DGrahpicData failed. GetID : " + base.GetID());
			return null;
		}
		return base.Create3DGrahpicData();
	}

	protected override void OnCreateAction()
	{
		Nr3DCharObject nr3DCharObject = this.m_k3DChar as Nr3DCharObject;
		nr3DCharObject.SwitchModelMesh();
		this.m_k3DChar.Reset();
		base.OnCreateAction();
	}

	public override bool OnLoaded3DChar()
	{
		if (!base.OnLoaded3DChar())
		{
			return false;
		}
		GameObject rootGameObject = this.m_k3DChar.GetRootGameObject();
		rootGameObject.transform.rotation = Quaternion.Euler(this.m_kPersonInfo.GetBasicInfo().m_vDirection);
		base.SetWideCollArea();
		base.SetAnimation(base.LoadAfterAnimation);
		return true;
	}

	public void MakeMonsterAnimation()
	{
		if (this.maketime <= 0f)
		{
			base.SetAnimation(this.m_k3DChar.GetIdleAnimation());
		}
		this.maketime = Time.time + 8f;
	}

	public override void Release()
	{
		base.Release();
	}
}
