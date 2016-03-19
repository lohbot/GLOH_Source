using System;

namespace NLibCs
{
	public static class NDataGenericLoader
	{
		public delegate ISetRow DataCtor();

		public static bool Load(string ndtfile, ITable table, NDataGenericLoader.DataCtor ctor, bool useFileNameEncryption = false)
		{
			NDataReader nDataReader = new NDataReader();
			nDataReader.UseFileNameEncryption = useFileNameEncryption;
			if (nDataReader.Load(ndtfile))
			{
				NDataGenericLoader.CommonParse(nDataReader, table, ctor);
				return true;
			}
			return false;
		}

		private static void CommonParse(NDataReader dr, ITable table, NDataGenericLoader.DataCtor ctor)
		{
			if (!dr.BeginSection("[Table]"))
			{
				return;
			}
			foreach (NDataReader.Row data in dr)
			{
				ISetRow setRow = ctor();
				setRow.SetData(data);
				table.Add(setRow);
			}
		}
	}
}
