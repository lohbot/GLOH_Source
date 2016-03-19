using System;

public class ACTSource
{
	public void SetActive(bool active)
	{
		if (this.IsValid())
		{
			if (active)
			{
				this.Active();
			}
			else
			{
				this.Disable();
			}
		}
	}

	protected virtual bool IsValid()
	{
		return true;
	}

	protected virtual void Disable()
	{
	}

	protected virtual void Active()
	{
	}
}
