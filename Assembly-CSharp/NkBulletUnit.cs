using System;
using UnityEngine;

public class NkBulletUnit
{
	private static float gravitationalConstant = 40f;

	private BULLET_INFO m_BulletInfo;

	private float m_fStartTime;

	private float m_fHitEndTime;

	private float m_fBulletEndTime;

	private TsWeakReference<NkBattleChar> m_TargetChar;

	private TsWeakReference<NkBattleChar> m_SourceChar;

	private Vector3 m_PosStart = Vector3.zero;

	private Vector3 m_HitPosEnd = Vector3.zero;

	private Vector3 m_BulletPosEnd = Vector3.zero;

	private TsWeakReference<NkEffectUnit> m_EffectUnit;

	private uint m_nEffectIndex;

	private Vector3 m_initVelVector = Vector3.zero;

	public bool CreateBulletUnit(BULLET_INFO BulletInfo, NkBattleChar pkSourceChar, NkBattleChar pkTargetchar, float fStartTime)
	{
		if (BulletInfo == null)
		{
			return false;
		}
		if (pkSourceChar == null)
		{
			return false;
		}
		if (pkTargetchar == null)
		{
			return false;
		}
		this.m_BulletInfo = BulletInfo;
		this.m_SourceChar = pkSourceChar;
		this.m_TargetChar = pkTargetchar;
		this.m_fStartTime = fStartTime;
		if (this.m_SourceChar.CastedTarget.m_bDeadReaservation)
		{
			Debug.Log("Already Dead Char Request Bullet");
		}
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Vector3.zero;
		if (this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_PASS)
		{
			BATTLE_POS_GRID battleGrid = Battle.BATTLE.GetBattleGrid(pkSourceChar.Ally, pkSourceChar.GetStartPosIndex());
			BATTLE_POS_GRID battleGrid2 = Battle.BATTLE.GetBattleGrid(pkTargetchar.Ally, pkTargetchar.GetStartPosIndex());
			if (battleGrid == null)
			{
				return false;
			}
			vector = battleGrid.GetCenterBack();
			this.m_PosStart = vector;
			vector.y = 0f;
			if (battleGrid2 == null)
			{
				return false;
			}
			vector3 = battleGrid2.GetCenterBack();
			this.m_BulletPosEnd = vector3;
			vector3.y = 0f;
			vector2 = pkTargetchar.GetCenterPosition();
			this.m_HitPosEnd = vector2;
			vector2.y = 0f;
		}
		else
		{
			vector = pkSourceChar.GetShotPosition();
			this.m_PosStart = vector;
			vector.y = 0f;
			vector3 = pkTargetchar.GetCenterPosition();
			this.m_BulletPosEnd = vector3;
			vector3.y = 0f;
			vector2 = pkTargetchar.GetCenterPosition();
			this.m_HitPosEnd = vector2;
			vector2.y = 0f;
		}
		float num = Vector3.Distance(vector, vector3);
		float num2 = Vector3.Distance(vector, vector2);
		if (this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_LINE || this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_CURVE || this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_PASS)
		{
			if (this.m_BulletInfo.SPEED > 0f)
			{
				this.m_fBulletEndTime = fStartTime + num / this.m_BulletInfo.SPEED;
				this.m_fHitEndTime = fStartTime + num2 / this.m_BulletInfo.SPEED;
			}
			else
			{
				this.m_fBulletEndTime = fStartTime;
				this.m_fHitEndTime = fStartTime;
			}
			if (this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_CURVE)
			{
				this.m_BulletPosEnd = this.m_TargetChar.CastedTarget.GetCharPos();
				this.m_HitPosEnd = this.m_TargetChar.CastedTarget.GetCharPos();
				this.calculateInitialVelocity();
			}
		}
		else if (this.m_BulletInfo.MOVE_TYPE == eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_NONE)
		{
			this.m_fBulletEndTime = fStartTime;
			this.m_fHitEndTime = fStartTime;
		}
		this.m_nEffectIndex = NrTSingleton<NkEffectManager>.Instance.AddEffect(this.m_BulletInfo.EFFECT_KIND, this.m_PosStart);
		this.m_EffectUnit = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(this.m_nEffectIndex);
		if (this.m_EffectUnit != null)
		{
			this.m_EffectUnit.CastedTarget.LifeTime = float.PositiveInfinity;
		}
		return true;
	}

	public bool Update()
	{
		if (this.m_EffectUnit == null)
		{
			this.m_EffectUnit = NrTSingleton<NkEffectManager>.Instance.GetEffectUnit(this.m_nEffectIndex);
			if (this.m_EffectUnit == null)
			{
				return true;
			}
			this.m_EffectUnit.CastedTarget.LifeTime = float.PositiveInfinity;
		}
		if (this.m_fBulletEndTime < Time.time)
		{
			if (this.m_EffectUnit.IsAlive)
			{
				uint registNum = this.m_EffectUnit.CastedTarget.RegistNum;
				NrTSingleton<NkEffectManager>.Instance.DeleteEffect(registNum);
			}
			this.EndEffect();
			return false;
		}
		switch (this.m_BulletInfo.MOVE_TYPE)
		{
		case eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_LINE:
		case eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_PASS:
			this.UpdateLineMove();
			break;
		case eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_CURVE:
			this.UpdateCurveMove();
			break;
		}
		return true;
	}

	private void UpdateLineMove()
	{
		if (this.m_EffectUnit.CastedTarget.m_goEffect == null)
		{
			return;
		}
		float num = this.m_fBulletEndTime - this.m_fStartTime;
		float num2 = Time.time - this.m_fStartTime;
		if (this.m_BulletInfo.HIT_TYPE == eBULLET_HIT_TYPE.eBULLET_HIT_TYPE_CHAR)
		{
			this.m_HitPosEnd = this.m_TargetChar.CastedTarget.GetCenterPosition();
		}
		Vector3 position = Vector3.Lerp(this.m_PosStart, this.m_BulletPosEnd, num2 / num);
		if (!this.m_EffectUnit.CastedTarget.m_goEffect.activeInHierarchy)
		{
			this.m_EffectUnit.CastedTarget.m_goEffect.SetActive(true);
			if (this.m_EffectUnit.CastedTarget.m_TrailerRenderer != null)
			{
				this.m_EffectUnit.CastedTarget.m_TrailerRenderer.enabled = true;
			}
			NkUtil.SetAllChildActive(this.m_EffectUnit.CastedTarget.m_goEffect, true);
		}
		if (this.m_EffectUnit.IsAlive)
		{
			this.m_EffectUnit.CastedTarget.m_goEffect.transform.position = position;
			Vector3 a = new Vector3(this.m_BulletPosEnd.x, this.m_PosStart.y, this.m_BulletPosEnd.z);
			Vector3 forward = a - this.m_PosStart;
			Quaternion localRotation = Quaternion.LookRotation(forward);
			this.m_EffectUnit.CastedTarget.m_goEffect.transform.localRotation = localRotation;
		}
	}

	private void UpdateCurveMove()
	{
		float num = Time.time - this.m_fStartTime;
		Vector3 position;
		position.x = this.m_PosStart.x + this.m_initVelVector.x * num;
		position.y = this.m_PosStart.y + this.m_initVelVector.y * num - 0.5f * NkBulletUnit.gravitationalConstant * num * num;
		position.z = this.m_PosStart.z + this.m_initVelVector.z * num;
		if (!this.m_EffectUnit.CastedTarget.m_goEffect.activeInHierarchy)
		{
			this.m_EffectUnit.CastedTarget.m_goEffect.SetActive(true);
			if (this.m_EffectUnit.CastedTarget.m_TrailerRenderer != null)
			{
				this.m_EffectUnit.CastedTarget.m_TrailerRenderer.enabled = true;
			}
			NkUtil.SetAllChildActive(this.m_EffectUnit.CastedTarget.m_goEffect, true);
		}
		if (this.m_EffectUnit.IsAlive)
		{
			this.m_EffectUnit.CastedTarget.m_goEffect.transform.position = position;
			Vector3 a = new Vector3(this.m_HitPosEnd.x, this.m_PosStart.y, this.m_HitPosEnd.z);
			Vector3 forward = a - this.m_PosStart;
			Quaternion localRotation = Quaternion.LookRotation(forward);
			this.m_EffectUnit.CastedTarget.m_goEffect.transform.localRotation = localRotation;
		}
	}

	private void calculateInitialVelocity()
	{
		float num = this.m_fHitEndTime - this.m_fStartTime;
		Vector3 a = new Vector3(this.m_HitPosEnd.x, this.m_PosStart.y, this.m_HitPosEnd.z);
		Vector3 vector = a - this.m_PosStart;
		float magnitude = vector.magnitude;
		float num2 = magnitude / num;
		Vector3 vector2 = vector;
		vector2.Normalize();
		float x = num2 * vector2.x;
		float z = num2 * vector2.z;
		float y = (this.m_HitPosEnd.y - this.m_PosStart.y) / num + 0.5f * NkBulletUnit.gravitationalConstant * num;
		this.m_initVelVector.x = x;
		this.m_initVelVector.y = y;
		this.m_initVelVector.z = z;
	}

	private void EndEffect()
	{
		if (this.m_BulletInfo.END_EFFECT_KIND != "NULL")
		{
			NrTSingleton<NkEffectManager>.Instance.AddEffect(this.m_BulletInfo.END_EFFECT_KIND, this.m_BulletPosEnd);
		}
	}

	public void RemoveBullet()
	{
		if (this.m_EffectUnit == null)
		{
			return;
		}
		if (!this.m_EffectUnit.IsAlive)
		{
			return;
		}
		uint registNum = this.m_EffectUnit.CastedTarget.RegistNum;
		NrTSingleton<NkEffectManager>.Instance.DeleteEffect(registNum);
	}
}
