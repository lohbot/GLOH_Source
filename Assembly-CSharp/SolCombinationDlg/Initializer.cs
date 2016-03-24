using System;
using System.Collections.Generic;

namespace SolCombinationDlg
{
	public class Initializer
	{
		public Dictionary<int, string> InitGradeTextKeyDic(int ENTIRE_VIEW)
		{
			string s = "2914";
			string value = "2921";
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(ENTIRE_VIEW, value);
			int num = 6;
			for (int i = 0; i <= num; i++)
			{
				int num2 = int.Parse(s) + i;
				dictionary.Add(num - i, num2.ToString());
			}
			return dictionary;
		}

		public Dictionary<int, string> InitGradeTextureKeyDic()
		{
			return new Dictionary<int, string>
			{
				{
					6,
					"Win_I_WorrGradeEX"
				},
				{
					5,
					"Win_I_WorrGradeSS"
				},
				{
					4,
					"Win_I_WorrGradeS"
				},
				{
					3,
					"Win_I_WorrGradeA"
				},
				{
					2,
					"Win_I_WorrGradeB"
				},
				{
					1,
					"Win_I_WorrGradeC"
				},
				{
					0,
					"Win_I_WorrGradeD"
				}
			};
		}
	}
}
