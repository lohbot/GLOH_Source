using NPatch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NLibCs
{
	public static class Example
	{
		private class TestBindee : NDataReader.IRowBindee
		{
			public List<NDataReader.Row> m_kRows = new List<NDataReader.Row>();

			public bool ReadFrom(NDataReader.Row tsRow)
			{
				this.m_kRows.Add(tsRow);
				return true;
			}
		}

		public static string NDataReader_MakeTestContext()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append("[KeyTypes]");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key_StringType=StringValue");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key_IntType=100");
			stringBuilder.AppendLine();
			stringBuilder.Append("Key_FloatType=123.456");
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
			return stringBuilder.ToString();
		}

		public static bool NDataReader_LoadFromText()
		{
			string strContext = Example.NDataReader_MakeTestContext();
			NDataReader nDataReader = new NDataReader();
			bool result = nDataReader.LoadFrom(strContext);
			int num = 0;
			foreach (NDataReader.Row row in nDataReader["Table"])
			{
				num++;
				Console.WriteLine(row.ToDataString());
			}
			NDataSection nDataSection = nDataReader["Table"];
			object[] array = new object[]
			{
				0,
				string.Empty,
				0f
			};
			foreach (NDataReader.Row row2 in nDataSection)
			{
				result = row2.GetColumn(ref array);
			}
			return result;
		}

		public static void NDataReader_Performance(string strTestFileName, bool bUseOptimize, bool bLoadFromFile, int nTestMax, bool bDataPrint)
		{
			NDataReader.UseOptimize = bUseOptimize;
			TimeSpan t = TimeSpan.MinValue;
			TimeSpan t2 = TimeSpan.MaxValue;
			TimeSpan t3 = TimeSpan.Zero;
			for (int i = 0; i < nTestMax; i++)
			{
				using (NDataReader nDataReader = new NDataReader())
				{
					Stopwatch stopwatch = new Stopwatch();
					if (bLoadFromFile)
					{
						stopwatch.Reset();
						stopwatch.Start();
						nDataReader.Load(strTestFileName);
						stopwatch.Stop();
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(strTestFileName, Encoding.Default, true))
						{
							string strContext = streamReader.ReadToEnd();
							stopwatch.Reset();
							stopwatch.Start();
							nDataReader.BeginSection("[Table]");
							nDataReader.LoadFrom(strContext);
							stopwatch.Stop();
							streamReader.Close();
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
						foreach (NDataReader.Row value in nDataReader)
						{
							Console.WriteLine(value);
						}
					}
				}
			}
			Console.WriteLine("##### 최적화 여부 : {0} , 로드방식 : {1} #####", NDataReader.UseOptimize, (!bLoadFromFile) ? "컨텍스트로부터" : "파일로부터");
			Console.WriteLine("  * Test Amount : {0}", nTestMax);
			Console.WriteLine("  * Max Time : {0} tick ({1})", t.Ticks, t.ToString());
			Console.WriteLine("  * Min Time : {0} tick ({1})", t2.Ticks, t2.ToString());
			Console.WriteLine("  * Avg Time : {0} tick ({1})", t3.Ticks / (long)nTestMax, TimeSpan.FromTicks(t3.Ticks / (long)nTestMax).ToString());
			Console.WriteLine();
		}

		public static void NDataReader_Load_Instantly()
		{
			string strFileName = "D:\\quest.ndt";
			Example.TestBindee testBindee = new Example.TestBindee();
			using (NDataReader nDataReader = new NDataReader())
			{
				bool flag = nDataReader.Load(strFileName, "[table]", testBindee);
				if (flag)
				{
				}
			}
			foreach (NDataReader.Row current in testBindee.m_kRows)
			{
				Console.WriteLine(current);
			}
		}

		public static void NParamString_Make_and_Parse1()
		{
			NParamString nParamString = new NParamString();
			nParamString.SetFormat("@title@\r\n@castle@ 에서 @itemname@ 이 @itemnum@ 개 지급되었습니다.");
			nParamString.AddParam("@title@", "textgroup", "textkey");
			nParamString.AddParam("@castle@", "강릉성");
			nParamString.AddParam("@itemname@", "쌀");
			nParamString.AddParam("@itemnum@", 10);
			string text = string.Empty;
			text = nParamString.ToString();
			NTextManager nTextManager = new NTextManager();
			nTextManager.RegistTextGroup("textgroup", false);
			nTextManager.SetText("textgroup", "textkey", "아이템지급!");
			NParamString nParamString2 = new NParamString();
		}

		public static void NParamString_Make_and_Parse2()
		{
			NParamString nParamString = new NParamString();
			nParamString.SetFormat("item", "100");
			nParamString.AddParam("@title@", "textgroup", "textkey");
			nParamString.AddParam("@castle@", "강릉성");
			nParamString.AddParam("@itemname@", "쌀");
			nParamString.AddParam("@itemnum@", 10);
			string text = string.Empty;
			text = nParamString.ToString();
			NTextManager nTextManager = new NTextManager();
			nTextManager.RegistTextGroup("textgroup", false);
			nTextManager.RegistTextGroup("item", false);
			nTextManager.SetText("textgroup", "textkey", "아이템지급!");
			nTextManager.SetText("item", "100", "@title@\r\n@castle@ 에서 @itemname@ 이 @itemnum@ 개 지급되었습니다.");
			NParamString nParamString2 = new NParamString();
		}

		public static NTextManager NTextManager_LoadFromGroupList()
		{
			Example.NTextManager_MakeSampleDataFiles();
			NTextManager nTextManager = new NTextManager();
			bool flag = nTextManager.LoadFromGroupList("D:\\TextGroupList.ndt", false, "[TextFiles]");
			if (flag)
			{
				Console.WriteLine(nTextManager.ToString());
				Console.WriteLine("Item1 => " + nTextManager.GetText("item", "item1", "아이템1"));
				Console.WriteLine("Item1 => " + nTextManager.GetText("[[item:item1]]"));
				Console.WriteLine("Quest1 => " + nTextManager["quest"]["quest1"]);
				Console.WriteLine("Quest2 => " + nTextManager["quest", "quest2"]);
			}
			nTextManager.Reload();
			return nTextManager;
		}

		private static void NTextManager_MakeSampleDataFiles()
		{
			using (StreamWriter streamWriter = new StreamWriter("D:\\TextGroupList.ndt", false, Encoding.Unicode))
			{
				streamWriter.WriteLine("[TextFiles]");
				streamWriter.WriteLine(";GroupKey\tTextFileName");
				streamWriter.WriteLine("Item\tD:\\item.ndt");
				streamWriter.WriteLine("Quest\tD:\\quest1.ndt");
				streamWriter.WriteLine("Quest\tD:\\quest2.ndt");
				streamWriter.Close();
			}
			using (StreamWriter streamWriter2 = new StreamWriter("D:\\item.ndt", false, Encoding.Unicode))
			{
				streamWriter2.WriteLine("[Table]");
				streamWriter2.WriteLine(";TextKey\tText");
				streamWriter2.WriteLine("item1\t아이템1");
				streamWriter2.Close();
			}
			using (StreamWriter streamWriter3 = new StreamWriter("D:\\quest1.ndt", false, Encoding.Unicode))
			{
				streamWriter3.WriteLine("[Table]");
				streamWriter3.WriteLine(";TextKey\tText");
				streamWriter3.WriteLine("quest1\t퀘스트1");
				streamWriter3.Close();
			}
			using (StreamWriter streamWriter4 = new StreamWriter("D:\\quest2.ndt", false, Encoding.Unicode))
			{
				streamWriter4.WriteLine("[Table]");
				streamWriter4.WriteLine(";TextKey\tText");
				streamWriter4.WriteLine("quest2\t퀘스트2");
				streamWriter4.Close();
			}
		}

		public static void NPatch_PatchOnce()
		{
			string root_local = "..\\..\\TestData\\Local";
			string root_url = "http://localhost/NPatch";
			bool flag = Launcher.Instance.PatchOnce(root_local, root_url, null);
			if (flag)
			{
			}
		}

		public static void NPatch_Load()
		{
			Example.NPatch_PatchOnce();
			string path = "..\\..\\TestData\\Resource\\100";
			string strListFileName = Path.Combine(path, "Final.patchlist.zip");
			FinalPatchList.Instance.UsePatchLevel = true;
			FinalPatchList.Instance.UseLangCode = false;
			FinalPatchList.Instance.Load(strListFileName, null);
			foreach (KeyValuePair<string, PatchFileInfo> current in FinalPatchList.Instance.FilesList)
			{
				Console.WriteLine("{0} {1}", current.Value, current.Key);
			}
			bool flag = Launcher.Instance.SavePatchLevel();
			if (flag)
			{
			}
		}

		public static void NPatch_Load_inUnity()
		{
		}

		public static void NPatch_Increase_PatchLevel()
		{
		}
	}
}
