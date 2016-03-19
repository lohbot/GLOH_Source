using GAME;
using System;

public class NEWGUILD_CONSTANT
{
	public eNEWGUILD_CONSTANT eConstant = eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MAX;

	public long i64Value;

	public NEWGUILD_CONSTANT()
	{
		this.Init();
	}

	private void Init()
	{
		this.eConstant = eNEWGUILD_CONSTANT.eNEWGUILD_CONSTANT_MAX;
		this.i64Value = 0L;
	}

	public void SetData(eNEWGUILD_CONSTANT _eConstant, long _Value)
	{
		this.Init();
		this.eConstant = _eConstant;
		this.i64Value = _Value;
	}
}
