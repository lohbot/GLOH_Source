using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TsLibs
{
	public static class TsDataReaderSample
	{
		private class BindingObject : TsDataReader.IBindingRow
		{
			public List<TsDataReader.Row> m_kRows = new List<TsDataReader.Row>();

			public bool ReadFrom(TsDataReader.Row tsRow)
			{
				this.m_kRows.Add(tsRow);
				return true;
			}
		}

		public static bool Sample1_LoadFromText()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append("[Test]");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key1=StringValue");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key2=100");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key3=123.456");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("[Table]");
			stringBuilder.AppendLine();
			stringBuilder.Append("1\tlee\t100.0");
			stringBuilder.AppendLine();
			stringBuilder.Append("2\tkim\t200.0");
			stringBuilder.AppendLine();
			stringBuilder.Append("3\tchoi\t300.0");
			stringBuilder.AppendLine();
			bool result = false;
			using (TsDataReader tsDataReader = new TsDataReader())
			{
				result = tsDataReader.LoadFrom(stringBuilder.ToString());
				string empty = string.Empty;
				int num = 0;
				float num2 = 0f;
				result = tsDataReader.ReadKeyData("Key1", out empty);
				result = tsDataReader.ReadKeyData("Key2", out num);
				result = tsDataReader.ReadKeyData("Key3", out num2);
				if (tsDataReader.BeginSection("[Table]"))
				{
					foreach (TsDataReader.Row row in tsDataReader)
					{
						int num3 = 0;
						string empty2 = string.Empty;
						double num4 = 0.0;
						result = row.GetColumn(0, out num3);
						result = row.GetColumn(1, out empty2);
						result = row.GetColumn(2, out num4);
					}
				}
				if (tsDataReader.BeginSection("[Table]"))
				{
					object[] array = new object[]
					{
						0,
						string.Empty,
						0f
					};
					foreach (TsDataReader.Row row2 in tsDataReader)
					{
						result = row2.GetColumn(ref array);
					}
				}
			}
			return result;
		}

		public static void Sample2_Performance(string strTestFileName, bool bUseOptimize, bool bLoadFromFile, int nTestMax, bool bDataPrint)
		{
			TsDataReader.UseOptimize = bUseOptimize;
			TimeSpan t = TimeSpan.MinValue;
			TimeSpan t2 = TimeSpan.MaxValue;
			TimeSpan t3 = TimeSpan.Zero;
			for (int i = 0; i < nTestMax; i++)
			{
				using (TsDataReader tsDataReader = new TsDataReader())
				{
					Stopwatch stopwatch = new Stopwatch();
					if (bLoadFromFile)
					{
						stopwatch.Reset();
						stopwatch.Start();
						tsDataReader.Load(strTestFileName);
						stopwatch.Stop();
					}
					else
					{
						using (FileStream fileStream = new FileStream(strTestFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
						{
							using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
							{
								string strContext = streamReader.ReadToEnd();
								stopwatch.Reset();
								stopwatch.Start();
								tsDataReader.BeginSection("[Table]");
								tsDataReader.LoadFrom(strContext);
								stopwatch.Stop();
								streamReader.Close();
							}
						}
					}
					if (t < stopwatch.Elapsed)
					{
						t = stopwatch.Elapsed;
					}
					if (stopwatch.Elapsed < t2)
					{
						t2 = stopwatch.Elapsed;
					}
					t3 += stopwatch.Elapsed;
					if (bDataPrint)
					{
						foreach (TsDataReader.Row value in tsDataReader)
						{
							Console.WriteLine(value);
						}
					}
				}
			}
			Console.WriteLine("##### 최적화 여부 : {0} , 로드방식 : {1} #####", TsDataReader.UseOptimize, (!bLoadFromFile) ? "컨텍스트로부터" : "파일로부터");
			Console.WriteLine("  * Test Amount : {0}", nTestMax);
			Console.WriteLine("  * Max Time : {0} tick ({1})", t.Ticks, t.ToString());
			Console.WriteLine("  * Min Time : {0} tick ({1})", t2.Ticks, t2.ToString());
			Console.WriteLine("  * Avg Time : {0} tick ({1})", t3.Ticks / (long)nTestMax, TimeSpan.FromTicks(t3.Ticks / (long)nTestMax).ToString());
			Console.WriteLine();
		}

		public static void Sample3_Load_Instantly()
		{
			string strFileName = "D:\\quest.ndt";
			TsDataReaderSample.BindingObject bindingObject = new TsDataReaderSample.BindingObject();
			using (TsDataReader tsDataReader = new TsDataReader())
			{
				bool flag = tsDataReader.Load(strFileName, "[table]", bindingObject);
				if (flag)
				{
				}
			}
			foreach (TsDataReader.Row current in bindingObject.m_kRows)
			{
				Console.WriteLine(current);
			}
		}
	}
}
