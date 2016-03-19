using System;
using System.Text;

public interface ITsDebugQualityManager
{
	bool EnableCollectShader
	{
		get;
		set;
	}

	void GetShaderList(out StringBuilder report);
}
