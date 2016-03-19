using System;

namespace NLibCs
{
	public abstract class NDataTable
	{
		public abstract bool ParseRowData(NDataReader.Row row);

		public bool Load(string filename)
		{
			using (NDataReader nDataReader = new NDataReader())
			{
				nDataReader.Load(filename);
				NDataSection nDataSection = nDataReader["Table"];
				foreach (NDataReader.Row row in nDataSection)
				{
					if (!this.ParseRowData(row))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
