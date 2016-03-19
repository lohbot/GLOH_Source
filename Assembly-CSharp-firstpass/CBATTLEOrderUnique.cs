using System;

public class CBATTLEOrderUnique
{
	public int iOrderUnique;

	public CBATTLEOrderUnique()
	{
	}

	public CBATTLEOrderUnique(int _iOrderUnique)
	{
	}

	public void Init(int _iOrderUnique)
	{
		this.iOrderUnique = BATTLE_DEFINE.MakeBFOrderUnique(_iOrderUnique);
	}
}
