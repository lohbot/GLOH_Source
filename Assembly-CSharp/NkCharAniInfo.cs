using System;
using System.Collections.Generic;

public class NkCharAniInfo
{
	public class CHARKIND_ANIEVENTTIME
	{
		public float[] fTime;

		public int nHitCount;

		public CHARKIND_ANIEVENTTIME()
		{
			int num = 10;
			this.fTime = new float[num];
			this.nHitCount = 0;
		}
	}

	private Dictionary<int, NkCharAniInfo.CHARKIND_ANIEVENTTIME>[] kAniEventType;

	public NkCharAniInfo()
	{
		this.InitAniInfo();
	}

	public void InitAniInfo()
	{
		int num = 2;
		this.kAniEventType = new Dictionary<int, NkCharAniInfo.CHARKIND_ANIEVENTTIME>[num];
		for (int i = 0; i < num; i++)
		{
			this.kAniEventType[i] = new Dictionary<int, NkCharAniInfo.CHARKIND_ANIEVENTTIME>();
		}
	}

	public float GetAnimationEvent(int weaponkey, int anitype, int eventype)
	{
		if (!this.kAniEventType[weaponkey].ContainsKey(anitype))
		{
			return 0f;
		}
		NkCharAniInfo.CHARKIND_ANIEVENTTIME cHARKIND_ANIEVENTTIME = null;
		if (!this.kAniEventType[weaponkey].TryGetValue(anitype, out cHARKIND_ANIEVENTTIME))
		{
			return 0f;
		}
		return cHARKIND_ANIEVENTTIME.fTime[eventype];
	}

	public void SetAniEventTime(int weaponkey, int anitype, int eventtype, float eventtime)
	{
		bool flag = false;
		if (anitype >= 1 && anitype <= 8 && eventtype >= 1 && eventtype <= 5)
		{
			flag = true;
		}
		if (this.kAniEventType[weaponkey].ContainsKey(anitype))
		{
			this.kAniEventType[weaponkey][anitype].fTime[eventtype] = eventtime;
			if (flag && eventtime != 0f)
			{
				this.kAniEventType[weaponkey][anitype].nHitCount++;
			}
			return;
		}
		NkCharAniInfo.CHARKIND_ANIEVENTTIME cHARKIND_ANIEVENTTIME = new NkCharAniInfo.CHARKIND_ANIEVENTTIME();
		cHARKIND_ANIEVENTTIME.fTime[eventtype] = eventtime;
		if (flag && eventtime != 0f)
		{
			cHARKIND_ANIEVENTTIME.nHitCount++;
		}
		this.kAniEventType[weaponkey].Add(anitype, cHARKIND_ANIEVENTTIME);
	}

	public int GetHitAniCount(int weaponkey, int anitype)
	{
		if (!this.kAniEventType[weaponkey].ContainsKey(anitype))
		{
			return 0;
		}
		NkCharAniInfo.CHARKIND_ANIEVENTTIME cHARKIND_ANIEVENTTIME = null;
		if (!this.kAniEventType[weaponkey].TryGetValue(anitype, out cHARKIND_ANIEVENTTIME))
		{
			return 0;
		}
		return cHARKIND_ANIEVENTTIME.nHitCount;
	}
}
