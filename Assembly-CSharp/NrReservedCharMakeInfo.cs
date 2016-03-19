using GAME;
using System;

public class NrReservedCharMakeInfo
{
	public float Distance;

	public NEW_MAKECHAR_INFO MakeCharInfo;

	public POS3D ReservedMoveTo = new POS3D();

	public CHAR_SHAPE_INFO kShapeInfo = new CHAR_SHAPE_INFO();

	public NrReservedCharMakeInfo(float distance, NEW_MAKECHAR_INFO makecharinfo)
	{
		this.Distance = distance;
		this.MakeCharInfo = makecharinfo;
	}
}
