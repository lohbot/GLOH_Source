using System;
using UnityEngine;

public class BATTLE_POS_GRID
{
	public int GRID_ID;

	public int m_nWidthCount;

	public int m_nHeightCount;

	public short nCharUnique;

	public Vector3[] mListPos;

	public short[] m_veBUID;

	public bool[] m_vebActive;

	public Vector3 m_veCenter = Vector3.zero;

	public Vector3 m_veCenterBackPos = Vector3.zero;

	public Vector3 m_veCenterFrontPos = Vector3.zero;

	public void Set(int nGridID, int nWidth, int nHeight)
	{
		if (nWidth == 0 || nHeight == 0)
		{
			return;
		}
		this.GRID_ID = nGridID;
		this.m_nWidthCount = nWidth;
		this.m_nHeightCount = nHeight;
		this.m_veBUID = new short[this.m_nWidthCount * this.m_nHeightCount];
		this.mListPos = new Vector3[this.m_nWidthCount * this.m_nHeightCount];
		this.m_vebActive = new bool[this.m_nWidthCount * this.m_nHeightCount];
		for (int i = 0; i < this.m_veBUID.Length; i++)
		{
			this.m_veBUID[i] = -1;
			this.mListPos[i] = Vector3.zero;
			this.m_vebActive[i] = true;
		}
	}

	public void Set(BATTLE_POS_GRID obj, int nRotate, bool bInfinityBattle = false)
	{
		this.GRID_ID = obj.GRID_ID;
		this.m_nWidthCount = obj.m_nWidthCount;
		this.m_nHeightCount = obj.m_nHeightCount;
		this.m_veBUID = new short[this.m_nWidthCount * this.m_nHeightCount];
		this.mListPos = new Vector3[this.m_nWidthCount * this.m_nHeightCount];
		this.m_vebActive = new bool[this.m_nWidthCount * this.m_nHeightCount];
		for (int i = 0; i < this.m_veBUID.Length; i++)
		{
			this.m_veBUID[i] = obj.m_veBUID[i];
			this.m_vebActive[i] = obj.m_vebActive[i];
		}
		if (nRotate == 0)
		{
			for (int i = 0; i < this.m_veBUID.Length; i++)
			{
				this.mListPos[i] = obj.mListPos[i];
			}
		}
		else if (nRotate == 270)
		{
			int num = 0;
			for (int i = 0; i < this.m_nWidthCount; i++)
			{
				for (int j = this.m_nHeightCount - 1; j >= 0; j--)
				{
					this.mListPos[num] = obj.mListPos[j * this.m_nHeightCount + i];
					num++;
				}
			}
		}
		else if (nRotate == 180)
		{
			if (bInfinityBattle)
			{
				int num2 = 0;
				for (int i = 0; i < this.m_nHeightCount; i++)
				{
					for (int j = 0; j < this.m_nWidthCount; j++)
					{
						this.mListPos[num2] = obj.mListPos[this.mListPos.Length - this.m_nWidthCount * (i + 1) + j];
						num2++;
					}
				}
			}
			else
			{
				for (int i = 0; i < this.m_veBUID.Length; i++)
				{
					this.mListPos[i] = obj.mListPos[this.mListPos.Length - i - 1];
				}
			}
		}
		else if (nRotate == 90)
		{
			int num3 = 0;
			for (int i = this.m_nWidthCount - 1; i >= 0; i--)
			{
				for (int j = 0; j < this.m_nHeightCount; j++)
				{
					this.mListPos[num3] = obj.mListPos[j * this.m_nHeightCount + i];
					num3++;
				}
			}
		}
	}

	public Vector3 GetCenter()
	{
		return this.m_veCenter;
	}

	public void SetCenter(Vector3 nGridCectorPos)
	{
		for (int i = 0; i < this.mListPos.Length; i++)
		{
			this.mListPos[i] = this.mListPos[i] + nGridCectorPos;
			this.m_veCenter += this.mListPos[i];
		}
		this.m_veCenter /= (float)this.mListPos.Length;
		this.SetCenterBackFront();
	}

	public void SetCenterBackFront()
	{
		if ((this.mListPos[0] - this.mListPos[this.m_nWidthCount]).z < 0f)
		{
			this.m_veCenterBackPos.Set(this.m_veCenter.x, this.m_veCenter.y, this.m_veCenter.z + 10f);
			this.m_veCenterFrontPos.Set(this.m_veCenter.x, this.m_veCenter.y, this.mListPos[0].z - 3f);
		}
		else
		{
			this.m_veCenterBackPos.Set(this.m_veCenter.x, this.m_veCenter.y, this.m_veCenter.z - 10f);
			this.m_veCenterFrontPos.Set(this.m_veCenter.x, this.m_veCenter.y, this.mListPos[0].z + 3f);
		}
	}

	public Vector3 GetCenterBack()
	{
		return this.m_veCenterBackPos;
	}

	public Vector3 GetCenterFront()
	{
		return this.m_veCenterFrontPos;
	}

	public void SetBUID(short nBUID, byte nGridPos, byte nSizeX, byte nSizeY)
	{
		int num = (int)nGridPos % this.m_nWidthCount;
		int num2 = (int)nGridPos / this.m_nWidthCount;
		for (int i = 0; i < (int)nSizeY; i++)
		{
			for (int j = 0; j < (int)nSizeX; j++)
			{
				int num3 = num + j + (i + num2) * this.m_nWidthCount;
				if (num3 < 0 || num3 >= this.m_veBUID.Length)
				{
					return;
				}
				this.m_veBUID[num3] = nBUID;
			}
		}
	}

	public void SetActive(bool bActive, byte nGridPos, byte nSizeX, byte nSizeY)
	{
		int num = (int)nGridPos % this.m_nWidthCount;
		int num2 = (int)nGridPos / this.m_nWidthCount;
		for (int i = 0; i < (int)nSizeY; i++)
		{
			for (int j = 0; j < (int)nSizeX; j++)
			{
				int num3 = num + j + (num2 + i) * this.m_nWidthCount;
				if (num3 < 0 || num3 >= this.m_veBUID.Length)
				{
					return;
				}
				this.m_vebActive[num3] = bActive;
			}
		}
	}

	public void RemoveBUID(short nBUID)
	{
		for (int i = 0; i < this.m_veBUID.Length; i++)
		{
			if (this.m_veBUID[i] == nBUID)
			{
				this.m_veBUID[i] = -1;
			}
		}
	}

	public bool GetCenter(short nBUID, ref Vector3 veCenterPos)
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		for (int i = 0; i < this.m_veBUID.Length; i++)
		{
			if (this.m_veBUID[i] == nBUID)
			{
				vector += this.mListPos[i];
				num++;
			}
		}
		if (num <= 0)
		{
			return false;
		}
		vector /= (float)num;
		veCenterPos = vector;
		return true;
	}

	public short GetGridBUID(sbyte nGridPos)
	{
		if ((int)nGridPos < 0)
		{
			return -1;
		}
		if ((int)nGridPos >= this.m_veBUID.Length)
		{
			return -1;
		}
		return this.m_veBUID[(int)nGridPos];
	}

	public int GetGridBuidArrayLength()
	{
		if (this.m_veBUID != null)
		{
			return this.m_veBUID.Length;
		}
		return 0;
	}
}
