using System;

[Serializable]
public class EventTriggerFlag
{
	private const short MAX_FLAG = 63;

	private static readonly long[] _BitFlag = new long[]
	{
		1L,
		2L,
		4L,
		8L,
		16L,
		32L,
		64L,
		128L,
		256L,
		512L,
		1024L,
		2048L,
		4096L,
		8192L,
		16384L,
		32768L,
		65536L,
		131072L,
		262144L,
		524288L,
		1048576L,
		2097152L,
		4194304L,
		8388608L,
		16777216L,
		33554432L,
		67108864L,
		134217728L,
		268435456L,
		536870912L,
		1073741824L,
		2147483648L,
		4294967296L,
		8589934592L,
		17179869184L,
		34359738368L,
		68719476736L,
		137438953472L,
		274877906944L,
		549755813888L,
		1099511627776L,
		2199023255552L,
		4398046511104L,
		8796093022208L,
		17592186044416L,
		35184372088832L,
		70368744177664L,
		140737488355328L,
		281474976710656L,
		562949953421312L,
		1125899906842624L,
		2251799813685248L,
		4503599627370496L,
		9007199254740992L,
		18014398509481984L,
		36028797018963968L,
		72057594037927936L,
		144115188075855872L,
		288230376151711744L,
		576460752303423488L,
		1152921504606846976L,
		2305843009213693952L,
		4611686018427387904L
	};

	private int _FlagIndex;

	private int _FlagArrayIndex;

	public EventTriggerFlag(long index)
	{
		this.Set(index);
	}

	public void Set(long index)
	{
		this._FlagArrayIndex = (int)(index / 63L);
		this._FlagIndex = (int)(index - (long)(this._FlagArrayIndex * 63));
		if (this._FlagIndex >= 63)
		{
			TsLog.LogError("EventTriggerFlag - FlagIndex Error", new object[0]);
			this._FlagIndex = 0;
		}
	}

	public long Get()
	{
		return (long)(this._FlagArrayIndex * 63 + this._FlagIndex);
	}

	public bool IsFlag(long[] FlagArray)
	{
		return FlagArray.Length > this._FlagArrayIndex && (FlagArray[this._FlagArrayIndex] & EventTriggerFlag._BitFlag[this._FlagIndex]) > 0L;
	}

	public short AddFlag(ref long[] FlagArray)
	{
		if (FlagArray.Length <= this._FlagArrayIndex)
		{
			return -1;
		}
		FlagArray[this._FlagArrayIndex] |= EventTriggerFlag._BitFlag[this._FlagIndex];
		return (short)this._FlagArrayIndex;
	}

	public short DelFlag(ref long[] FlagArray)
	{
		if (FlagArray.Length <= this._FlagArrayIndex)
		{
			return -1;
		}
		FlagArray[this._FlagArrayIndex] = (FlagArray[this._FlagArrayIndex] & ~EventTriggerFlag._BitFlag[this._FlagIndex]);
		return (short)this._FlagArrayIndex;
	}

	public override string ToString()
	{
		return string.Format("{0}({1}:{2})", this.Get(), this._FlagArrayIndex, this._FlagIndex);
	}
}
