using System;
using System.Text;

namespace NPatch
{
	public class ListCompressor
	{
		public class TextDef
		{
			public string word;

			public string code;

			public TextDef(string w, string c)
			{
				this.word = w;
				this.code = c;
			}
		}

		public ListCompressor.TextDef[] TextDefs = ListCompressor.REPLACE_ITEMS;

		public static ListCompressor.TextDef[] REPLACE_ITEMS = new ListCompressor.TextDef[]
		{
			new ListCompressor.TextDef("mobile_and", "\"A\""),
			new ListCompressor.TextDef("mobile_ios", "\"I\""),
			new ListCompressor.TextDef("map_tileinfo", "MTI"),
			new ListCompressor.TextDef("battlemap_cellatbinfo", "BMCI"),
			new ListCompressor.TextDef("assetbundle", "AB"),
			new ListCompressor.TextDef("customizing", "CU"),
			new ListCompressor.TextDef("footstep", "FS"),
			new ListCompressor.TextDef("scimitar", "SC"),
			new ListCompressor.TextDef("greeting", "GR"),
			new ListCompressor.TextDef("monster", "MO"),
			new ListCompressor.TextDef("monarch", "MN"),
			new ListCompressor.TextDef("master", "MA"),
			new ListCompressor.TextDef("sword", "SW"),
			new ListCompressor.TextDef("spear", "SP"),
			new ListCompressor.TextDef("general", "G"),
			new ListCompressor.TextDef("greed", "R"),
			new ListCompressor.TextDef("vehicle", "V"),
			new ListCompressor.TextDef("dungeon", "D"),
			new ListCompressor.TextDef("attack", "A"),
			new ListCompressor.TextDef("event", "E"),
			new ListCompressor.TextDef("quest", "Q"),
			new ListCompressor.TextDef("sound", "S"),
			new ListCompressor.TextDef("magic", "M"),
			new ListCompressor.TextDef("normal", "O"),
			new ListCompressor.TextDef("weapon", "W"),
			new ListCompressor.TextDef("troop", "T"),
			new ListCompressor.TextDef("none", "N"),
			new ListCompressor.TextDef("hero", "H"),
			new ListCompressor.TextDef("item", "I"),
			new ListCompressor.TextDef("battle", "B"),
			new ListCompressor.TextDef("skill", "K"),
			new ListCompressor.TextDef("female", "<"),
			new ListCompressor.TextDef("male", ">"),
			new ListCompressor.TextDef("hit", "*")
		};

		public static ListCompressor.TextDef[] REPLACE_ITEMS_FOR_LOH = new ListCompressor.TextDef[]
		{
			new ListCompressor.TextDef("_mobile", "\"A\""),
			new ListCompressor.TextDef("_iosmobile", "\"I\""),
			new ListCompressor.TextDef("map_tileinfo", "MTI"),
			new ListCompressor.TextDef("battlemap_cellatbinfo", "BMCI"),
			new ListCompressor.TextDef("assetbundle", "AB"),
			new ListCompressor.TextDef("customizing", "CU"),
			new ListCompressor.TextDef("footstep", "FS"),
			new ListCompressor.TextDef("scimitar", "SC"),
			new ListCompressor.TextDef("greeting", "GR"),
			new ListCompressor.TextDef("monster", "MO"),
			new ListCompressor.TextDef("monarch", "MN"),
			new ListCompressor.TextDef("master", "MA"),
			new ListCompressor.TextDef("sword", "SW"),
			new ListCompressor.TextDef("spear", "SP"),
			new ListCompressor.TextDef("general", "G"),
			new ListCompressor.TextDef("greed", "R"),
			new ListCompressor.TextDef("vehicle", "V"),
			new ListCompressor.TextDef("dungeon", "D"),
			new ListCompressor.TextDef("attack", "A"),
			new ListCompressor.TextDef("event", "E"),
			new ListCompressor.TextDef("quest", "Q"),
			new ListCompressor.TextDef("sound", "S"),
			new ListCompressor.TextDef("magic", "M"),
			new ListCompressor.TextDef("normal", "O"),
			new ListCompressor.TextDef("weapon", "W"),
			new ListCompressor.TextDef("troop", "T"),
			new ListCompressor.TextDef("none", "N"),
			new ListCompressor.TextDef("hero", "H"),
			new ListCompressor.TextDef("item", "I"),
			new ListCompressor.TextDef("battle", "B"),
			new ListCompressor.TextDef("skill", "K"),
			new ListCompressor.TextDef("female", "<"),
			new ListCompressor.TextDef("male", ">"),
			new ListCompressor.TextDef("hit", "*")
		};

		public bool UseReplaceWord
		{
			get;
			set;
		}

		public ListCompressor()
		{
			this.UseReplaceWord = false;
		}

		public ListCompressor(bool bUserReplaceWord)
		{
			this.UseReplaceWord = bUserReplaceWord;
		}

		public string ReplaceWord(string str, bool bWordToCode)
		{
			if (!this.UseReplaceWord)
			{
				return str;
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			if (bWordToCode)
			{
				stringBuilder.Append(str.ToLower());
				ListCompressor.TextDef[] textDefs = this.TextDefs;
				for (int i = 0; i < textDefs.Length; i++)
				{
					ListCompressor.TextDef textDef = textDefs[i];
					stringBuilder = stringBuilder.Replace(textDef.word, textDef.code);
				}
			}
			else
			{
				ListCompressor.TextDef[] textDefs2 = this.TextDefs;
				for (int j = 0; j < textDefs2.Length; j++)
				{
					ListCompressor.TextDef textDef2 = textDefs2[j];
					stringBuilder = stringBuilder.Replace(textDef2.code, textDef2.word);
				}
			}
			return stringBuilder.ToString();
		}

		public StringBuilder ReplaceWord(StringBuilder sb, bool bWordToCode)
		{
			if (!this.UseReplaceWord)
			{
				return sb;
			}
			if (bWordToCode)
			{
				ListCompressor.TextDef[] textDefs = this.TextDefs;
				for (int i = 0; i < textDefs.Length; i++)
				{
					ListCompressor.TextDef textDef = textDefs[i];
					sb = sb.Replace(textDef.word, textDef.code);
				}
			}
			else
			{
				ListCompressor.TextDef[] textDefs2 = this.TextDefs;
				for (int j = 0; j < textDefs2.Length; j++)
				{
					ListCompressor.TextDef textDef2 = textDefs2[j];
					sb = sb.Replace(textDef2.code, textDef2.word);
				}
			}
			return sb;
		}
	}
}
