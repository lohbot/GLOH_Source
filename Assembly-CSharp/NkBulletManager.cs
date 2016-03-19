using System;
using System.Collections.Generic;

public class NkBulletManager : NrTSingleton<NkBulletManager>
{
	private Dictionary<uint, NkBulletUnit> m_kBulletList = new Dictionary<uint, NkBulletUnit>();

	private uint m_nCurrentRegistNum;

	private List<uint> m_RemoveList = new List<uint>();

	private Dictionary<string, BULLET_INFO> m_kBulletInfoList = new Dictionary<string, BULLET_INFO>();

	private NkValueParse<eBULLET_MOVE_TYPE> m_kBulletMoveType = new NkValueParse<eBULLET_MOVE_TYPE>();

	private NkValueParse<eBULLET_HIT_TYPE> m_kBulletHitType = new NkValueParse<eBULLET_HIT_TYPE>();

	private NkBulletManager()
	{
		this._RegisterMoveType();
		this._RegisterHitType();
	}

	private uint _GetNextRegistNum()
	{
		if ((this.m_nCurrentRegistNum += 1u) == 0u)
		{
			this.m_nCurrentRegistNum = 1u;
		}
		return this.m_nCurrentRegistNum;
	}

	public void RemoveAllBullet()
	{
		if (this.m_kBulletList.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<uint, NkBulletUnit> current in this.m_kBulletList)
		{
			current.Value.RemoveBullet();
		}
		this.m_kBulletList.Clear();
	}

	public bool CreateBullet(string szBullet_KIND, NkBattleChar pkSourceChar, NkBattleChar pkTargetchar, float fStartTime)
	{
		if (szBullet_KIND == null)
		{
			return false;
		}
		BULLET_INFO bulletInfo = this.GetBulletInfo(szBullet_KIND);
		if (bulletInfo == null)
		{
			TsLog.Assert(false, "Bullet Create Fail " + pkSourceChar.Get3DName() + " Kind : " + szBullet_KIND, new object[0]);
			return false;
		}
		NkBulletUnit nkBulletUnit = new NkBulletUnit();
		if (nkBulletUnit.CreateBulletUnit(bulletInfo, pkSourceChar, pkTargetchar, fStartTime))
		{
			uint key = this._GetNextRegistNum();
			this.m_kBulletList.Add(key, nkBulletUnit);
			return true;
		}
		return false;
	}

	public void Update()
	{
		if (this.m_kBulletList.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<uint, NkBulletUnit> current in this.m_kBulletList)
		{
			if (!current.Value.Update())
			{
				this.m_RemoveList.Add(current.Key);
			}
		}
		if (this.m_RemoveList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.m_RemoveList.Count; i++)
		{
			this.m_kBulletList.Remove(this.m_RemoveList[i]);
		}
		this.m_RemoveList.Clear();
	}

	private void _RegisterMoveType()
	{
		this.m_kBulletMoveType.InsertCodeValue("NONE", eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_NONE);
		this.m_kBulletMoveType.InsertCodeValue("LINE", eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_LINE);
		this.m_kBulletMoveType.InsertCodeValue("CURVE", eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_CURVE);
		this.m_kBulletMoveType.InsertCodeValue("PASS", eBULLET_MOVE_TYPE.eBULLET_MOVE_TYPE_PASS);
	}

	private void _RegisterHitType()
	{
		this.m_kBulletHitType.InsertCodeValue("NONE", eBULLET_HIT_TYPE.eBULLET_HIT_TYPE_NONE);
		this.m_kBulletHitType.InsertCodeValue("CHAR", eBULLET_HIT_TYPE.eBULLET_HIT_TYPE_CHAR);
		this.m_kBulletHitType.InsertCodeValue("POS", eBULLET_HIT_TYPE.eBULLET_HIT_TYPE_POS);
	}

	public eBULLET_MOVE_TYPE ConvertBulletMoveType(string strMoveType)
	{
		return this.m_kBulletMoveType.GetValue(strMoveType);
	}

	public eBULLET_HIT_TYPE ConvertBulletHitType(string strHitType)
	{
		return this.m_kBulletHitType.GetValue(strHitType);
	}

	public BULLET_INFO GetBulletInfo(string szBulletName)
	{
		if (this.m_kBulletInfoList.ContainsKey(szBulletName))
		{
			return this.m_kBulletInfoList[szBulletName];
		}
		return null;
	}

	public void AddBulletInfo(BULLET_INFO kBulletInfo)
	{
		if (this.m_kBulletInfoList.ContainsKey(kBulletInfo.NAME))
		{
			return;
		}
		this.m_kBulletInfoList.Add(kBulletInfo.NAME, kBulletInfo);
	}
}
