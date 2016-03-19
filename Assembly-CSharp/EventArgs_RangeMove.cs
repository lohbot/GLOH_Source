using System;

public class EventArgs_RangeMove : EventArgs
{
	public float x;

	public float y;

	public void Set(float x, float y)
	{
		this.x = x;
		this.y = y;
	}
}
