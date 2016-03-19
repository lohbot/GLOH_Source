using GAME;
using System;
using UnityEngine;

public class NrCharMonster : NrCharBase
{
	public short m_TargetCharUnique;

	public GameObject m_QuestMob
	{
		get;
		private set;
	}

	public NrCharMonster()
	{
		this.m_kPersonInfo = new NrPersonInfoNPC();
	}

	public override void Init()
	{
		base.Init();
		this.m_eCharKindType = eCharKindType.CKT_MONSTER;
		this.m_TargetCharUnique = 0;
	}

	public override bool Update()
	{
		return base.Update() && base.PostUpdate();
	}

	public override Nr3DCharBase Create3DGrahpicData()
	{
		if (this.m_k3DChar != null)
		{
			this.Release();
		}
		string strName = this.m_kPersonInfo.GetCharName() + "_" + base.GetID().ToString();
		this.m_k3DChar = NrTSingleton<Nr3DCharSystem>.Instance.Create3DChar<Nr3DCharNonePart>(base.GetID(), strName);
		if (this.m_k3DChar == null)
		{
			return null;
		}
		return base.Create3DGrahpicData();
	}

	protected override void OnCreateAction()
	{
		Nr3DCharNonePart nr3DCharNonePart = this.m_k3DChar as Nr3DCharNonePart;
		nr3DCharNonePart.SwitchModelMesh();
		this.m_k3DChar.Reset();
		base.OnCreateAction();
	}

	public override bool OnLoaded3DChar()
	{
		if (!base.OnLoaded3DChar())
		{
			return false;
		}
		if (base.IsClientNPC)
		{
			base.ProcessMouseEvent = false;
		}
		else if (base.IsCharStateAtb(8192L))
		{
			base.ProcessMouseEvent = false;
		}
		base.SetAnimation(base.LoadAfterAnimation);
		return true;
	}
}
