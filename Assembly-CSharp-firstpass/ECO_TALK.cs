using System;
using TsLibs;
using UnityEngine;

public class ECO_TALK : NrTableData
{
	public string strCharCode = string.Empty;

	public string strHit = string.Empty;

	public string strAttack = string.Empty;

	public string strWalk = string.Empty;

	public string strRun = string.Empty;

	public string[] strIdle = new string[5];

	public string[] strInteraction = new string[5];

	public ECO_TALK()
	{
		base.SetTypeIndex(NrTableData.eResourceType.eRT_ECO_TALK);
		this.Init();
	}

	public void Init()
	{
		this.strCharCode = string.Empty;
		this.strHit = string.Empty;
		this.strAttack = string.Empty;
		this.strWalk = string.Empty;
		this.strRun = string.Empty;
		this.strIdle[0] = string.Empty;
		this.strIdle[1] = string.Empty;
		this.strIdle[2] = string.Empty;
		this.strIdle[3] = string.Empty;
		this.strIdle[4] = string.Empty;
		this.strInteraction[0] = string.Empty;
		this.strInteraction[1] = string.Empty;
		this.strInteraction[2] = string.Empty;
		this.strInteraction[3] = string.Empty;
		this.strInteraction[4] = string.Empty;
	}

	public override void SetData(TsDataReader.Row row)
	{
		this.Init();
		int num = 0;
		int num2 = 0;
		row.GetColumn(num2++, out this.strCharCode);
		row.GetColumn(num2++, out num);
		this.strHit = num.ToString();
		row.GetColumn(num2++, out num);
		this.strAttack = num.ToString();
		row.GetColumn(num2++, out num);
		this.strWalk = num.ToString();
		row.GetColumn(num2++, out num);
		this.strRun = num.ToString();
		row.GetColumn(num2++, out num);
		this.strIdle[0] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strIdle[1] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strIdle[2] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strIdle[3] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strIdle[4] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strInteraction[0] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strInteraction[1] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strInteraction[2] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strInteraction[3] = num.ToString();
		row.GetColumn(num2++, out num);
		this.strInteraction[4] = num.ToString();
	}

	public string GetRandTalk()
	{
		int num = UnityEngine.Random.Range(0, 5);
		return this.strInteraction[num];
	}
}
