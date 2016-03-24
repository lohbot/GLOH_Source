using System;

namespace GAME
{
	public class CHAR_SHAPE_INFO
	{
		public int nFaceCharKind;

		public byte nFaceGrade;

		public long nFaceCharSolID;

		public int nFaceCostumeUnique;

		public NrCharPartInfo kPartInfo;

		public CHAR_SHAPE_INFO()
		{
			this.nFaceCharKind = 0;
			this.nFaceGrade = 0;
			this.kPartInfo = new NrCharPartInfo();
			this.nFaceCharSolID = 0L;
			this.nFaceCostumeUnique = 0;
		}
	}
}
