using System;
using System.Collections.Generic;

public class NrTableTranscendenceManager : NrTSingleton<NrTableTranscendenceManager>
{
	private List<TRANSCENDENCE_COST> m_TranscendenceCostList = new List<TRANSCENDENCE_COST>();

	private List<TRANSCENDENCE_RATE> m_TranscendenceRateList = new List<TRANSCENDENCE_RATE>();

	private List<TRANSCENDENCE_SOL> m_TranscendenceSolList = new List<TRANSCENDENCE_SOL>();

	private List<TRANSCENDENCE_FAILREWARD> m_TranscendenceFailRewardList = new List<TRANSCENDENCE_FAILREWARD>();

	private NrTableTranscendenceManager()
	{
		this.m_TranscendenceCostList.Clear();
		this.m_TranscendenceRateList.Clear();
		this.m_TranscendenceSolList.Clear();
		this.m_TranscendenceFailRewardList.Clear();
	}

	public void AddTranscendenceCost(TRANSCENDENCE_COST Cost)
	{
		this.m_TranscendenceCostList.Add(Cost);
	}

	public void AddTranscendenceRate(TRANSCENDENCE_RATE Rate)
	{
		this.m_TranscendenceRateList.Add(Rate);
	}

	public void AddTranscendenceSol(TRANSCENDENCE_SOL Sol)
	{
		this.m_TranscendenceSolList.Add(Sol);
	}

	public void AddTranscendenceFailReward(TRANSCENDENCE_FAILREWARD FailReward)
	{
		this.m_TranscendenceFailRewardList.Add(FailReward);
	}

	public int GetTranscendenceRate(byte BaseGrade, int BaseSeason, int BaseKind, byte MaterialGrade, int MaterialSeason, int Materialind)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_TranscendenceSolList.Count; i++)
		{
			if (this.m_TranscendenceSolList[i].m_i32MaterialSeason == MaterialSeason + 1)
			{
				for (int j = 0; j < 10; j++)
				{
					if (j == BaseSeason)
					{
						num = this.m_TranscendenceSolList[i].m_i32BaseSeason[j];
						break;
					}
				}
			}
		}
		TsLog.LogError(" 성공확률 시즌 [{0}] 베이스시즌 , [{1}] 소재시즌, [{2}] 시즌확률", new object[]
		{
			BaseSeason,
			MaterialSeason,
			num
		});
		for (int i = 0; i < this.m_TranscendenceRateList.Count; i++)
		{
			if (this.m_TranscendenceRateList[i].m_bGrade == MaterialGrade - 6)
			{
				switch (BaseGrade)
				{
				case 6:
				case 10:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_A;
					break;
				case 7:
				case 11:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_S;
					break;
				case 8:
				case 12:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_SS;
					break;
				}
				break;
			}
			if (this.m_TranscendenceRateList[i].m_bGrade == MaterialGrade - 10)
			{
				switch (BaseGrade)
				{
				case 6:
				case 10:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_A;
					break;
				case 7:
				case 11:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_S;
					break;
				case 8:
				case 12:
					num2 = (int)this.m_TranscendenceRateList[i].m_i16BassGrade_SS;
					break;
				}
				break;
			}
		}
		if (BaseKind == Materialind)
		{
			num2 = (int)((float)num2 * 3.34f);
		}
		TsLog.LogError(" 성공확률 등급 [{0}] 베이스등급 , [{1}] 소재등급, [{2}] 등급확률", new object[]
		{
			BaseGrade,
			MaterialGrade,
			num2
		});
		int num3;
		if (num == 10000)
		{
			num3 = num2;
		}
		else
		{
			num3 = (int)((float)(num + num2) * 0.5f);
		}
		if (num3 > 10000)
		{
			num3 = 10000;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (BaseKind == Materialind)
		{
			num3 = num3 / 100 * 100;
		}
		TsLog.LogError(" 성공확률 최종 [{0}] 시즌 , [{1}] 등급, [{2}] 확률", new object[]
		{
			num,
			num2,
			num3
		});
		return num3;
	}

	public long GetTranscendenceMoney(byte BaseGrade, int BaseSeason, byte MaterialGrade, int MaterialSeason)
	{
		long num = 0L;
		for (int i = 0; i < this.m_TranscendenceCostList.Count; i++)
		{
			if (this.m_TranscendenceCostList[i].m_i32Season == BaseSeason + 1)
			{
				switch (MaterialGrade)
				{
				case 6:
				case 10:
					num = this.m_TranscendenceCostList[i].m_i64Cost[0];
					break;
				case 7:
				case 11:
					num = this.m_TranscendenceCostList[i].m_i64Cost[1];
					break;
				case 8:
				case 12:
					num = this.m_TranscendenceCostList[i].m_i64Cost[2];
					break;
				case 9:
				case 13:
					num = this.m_TranscendenceCostList[i].m_i64Cost[3];
					break;
				}
				break;
			}
		}
		TsLog.LogError(" 필요비용 [{0}] 등급 , [{1}] 시즌, [{2}] 필요비용", new object[]
		{
			BaseGrade,
			BaseSeason,
			num
		});
		return num;
	}

	public short GetTranscendenceFailItemNum(byte BaseGrade, int BaseSeason, byte MaterialGrade, int MaterialSeason)
	{
		short num = 0;
		for (int i = 0; i < this.m_TranscendenceFailRewardList.Count; i++)
		{
			if (this.m_TranscendenceFailRewardList[i].m_i32Season == MaterialSeason + 1)
			{
				switch (MaterialGrade)
				{
				case 6:
				case 10:
					num = this.m_TranscendenceFailRewardList[i].m_i16ItemNum[0];
					break;
				case 7:
				case 11:
					num = this.m_TranscendenceFailRewardList[i].m_i16ItemNum[1];
					break;
				case 8:
				case 12:
					num = this.m_TranscendenceFailRewardList[i].m_i16ItemNum[2];
					break;
				case 9:
				case 13:
					num = this.m_TranscendenceFailRewardList[i].m_i16ItemNum[3];
					break;
				}
				break;
			}
		}
		TsLog.LogError(" 실패시 아이템 갯수 [{0}] 등급 , [{1}] 시즌, [{2}] 아이템 갯수", new object[]
		{
			MaterialGrade,
			MaterialSeason,
			num
		});
		return num;
	}
}
