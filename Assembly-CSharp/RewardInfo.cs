using System;

public class RewardInfo
{
	public int[] bType = new int[3];

	public int[] nItemUnique = new int[3];

	public byte[] bEffect = new byte[3];

	public UIBaseInfoLoader[] imgLoder = new UIBaseInfoLoader[3];

	public object[] kInfo = new object[3];

	public string[] str1 = new string[3];

	public string[] str2 = new string[3];

	public RewardInfo()
	{
		this.bType[0] = 1;
		this.bType[1] = 1;
		this.bType[2] = 1;
	}
}
