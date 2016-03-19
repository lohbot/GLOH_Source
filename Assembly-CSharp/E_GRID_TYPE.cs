using System;

public enum E_GRID_TYPE : byte
{
	NORMAL,
	MOVE = 4,
	ATTACK = 8,
	FOCUS = 16,
	OVER = 32,
	MAGIC = 64,
	MAGICSELECT = 128
}
