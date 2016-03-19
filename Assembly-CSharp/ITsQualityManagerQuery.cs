using System;

public interface ITsQualityManagerQuery
{
	bool IsFixedBasemap
	{
		get;
	}

	bool IsFixedTreeBillboard
	{
		get;
	}

	string SettingsXmlPath
	{
		get;
	}
}
