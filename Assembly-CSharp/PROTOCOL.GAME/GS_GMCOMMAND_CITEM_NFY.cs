using GAME;
using System;

namespace PROTOCOL.GAME
{
	public class GS_GMCOMMAND_CITEM_NFY
	{
		public char[] strName_GM = new char[21];

		public ITEM item;

		public GS_GMCOMMAND_CITEM_NFY()
		{
			this.item = new ITEM();
		}
	}
}
