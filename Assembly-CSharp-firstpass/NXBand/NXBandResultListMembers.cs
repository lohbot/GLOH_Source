using System;
using System.Collections;

namespace NXBand
{
	public class NXBandResultListMembers : NXBandResult
	{
		public NXBandTypePageInfo pageInfo;

		public NXBandTypeCache cache;

		public ArrayList members;

		public NXBandResultListMembers()
		{
			this._resultType = BandResultType.ListMembers;
			this.members = new ArrayList();
		}

		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				base.ToString(),
				", pageInfo : <",
				this.pageInfo,
				">, cache : <",
				this.cache,
				">, members : <"
			});
			foreach (NXBandTypeMember nXBandTypeMember in this.members)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"<",
					nXBandTypeMember,
					">, "
				});
			}
			text += ">";
			return text;
		}
	}
}
