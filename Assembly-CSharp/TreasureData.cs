using System;
using UnityForms;

public class TreasureData
{
	private DrawTexture m_DrawTreasure;

	private DrawTexture m_dtTreasure;

	private bool m_bCheck;

	private int m_i32MapIndex;

	public void Init(int iIndex)
	{
		string name = "BT_WorldMapTreasure" + iIndex.ToString();
		string name2 = "LB_WorldMapTreasure" + iIndex.ToString();
		this.m_DrawTreasure = UICreateControl.DrawTexture(name, "Territory_B_harvest", 80f, 80f);
		this.m_dtTreasure = UICreateControl.DrawTexture(name2, "Win_I_Box", 50f, 50f);
		this.m_bCheck = false;
		this.m_DrawTreasure.SetLocation(0, 0);
		this.m_dtTreasure.SetLocation(0, 0);
	}

	public DrawTexture GetTexture()
	{
		return this.m_DrawTreasure;
	}

	public DrawTexture GetDrawTexture()
	{
		return this.m_dtTreasure;
	}

	public bool GetShow()
	{
		return this.m_bCheck;
	}

	public int GetMapIndex()
	{
		return this.m_i32MapIndex;
	}

	public void SetPostion(float fX, float fY)
	{
		this.m_DrawTreasure.SetLocation(fX, fY, -3f);
		this.m_dtTreasure.SetLocation(fX + 15f, fY + 13f, -4f);
	}

	public void TreasureShow(bool bShow)
	{
		this.m_DrawTreasure.Hide(!bShow);
		this.m_dtTreasure.Hide(!bShow);
	}

	public void SetTreasureData(bool bShow, int i32MapIndex)
	{
		this.m_bCheck = bShow;
		this.m_i32MapIndex = i32MapIndex;
	}

	public bool GetTreasureData(int i32MapIndex)
	{
		return this.m_bCheck && this.m_i32MapIndex == i32MapIndex;
	}
}
