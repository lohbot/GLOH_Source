using System;

namespace GAME
{
	public class CHAR_SHAPE_INFO
	{
		public int nFaceCharKind;

		public byte nFaceGrade;

		public NrCharPartInfo kPartInfo;

		public CHAR_SHAPE_INFO()
		{
			this.nFaceCharKind = 0;
			this.nFaceGrade = 0;
			this.kPartInfo = new NrCharPartInfo();
		}
	}
}
