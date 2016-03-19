using System;
using System.Collections;

namespace NXBand
{
	public class NXBandResultListBands : NXBandResult
	{
		public NXBandTypePageInfo pageInfo;

		public NXBandTypeCache cache;

		public ArrayList bands;

		public NXBandResultListBands()
		{
			this._resultType = BandResultType.ListBands;
			this.bands = new ArrayList();
		}

		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				base.ToString(),
				" ,pageInfo : <",
				this.pageInfo,
				">, cache : <",
				this.cache,
				">, members : <"
			});
			foreach (NXBandTypeBand nXBandTypeBand in this.bands)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"<",
					nXBandTypeBand,
					">, "
				});
			}
			text += ">";
			return text;
		}
	}
}
