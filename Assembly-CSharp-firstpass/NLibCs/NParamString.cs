using System;
using System.Text;

namespace NLibCs
{
	public class NParamString
	{
		protected StringBuilder m_sb_ParamString = new StringBuilder(4000);

		private readonly string SECTION_FORMAT = "[FORMAT]";

		private readonly string SECTION_PARAMS = "[PARAMS]";

		private readonly string LINE = "\r\n";

		private readonly string TXTKEY = "$TXTKEY$";

		public void SetFormat(string strFormat)
		{
			this.m_sb_ParamString.Length = 0;
			this.m_sb_ParamString.AppendFormat("{0}{1}{2}{3}{4}{5}", new object[]
			{
				this.SECTION_FORMAT,
				this.LINE,
				strFormat,
				this.LINE,
				this.SECTION_PARAMS,
				this.LINE
			});
		}

		public void SetFormat(string strTextGroupKey, string strTextKey)
		{
			this.m_sb_ParamString.Length = 0;
			this.m_sb_ParamString.AppendFormat("{0}{1}{2}{3}{4}{5}", new object[]
			{
				this.SECTION_FORMAT,
				this.LINE,
				string.Format("{0}\t{1}|{2}", this.TXTKEY, strTextGroupKey, strTextKey),
				this.LINE,
				this.SECTION_PARAMS,
				this.LINE
			});
		}

		public bool AddParam(string strParam, object value)
		{
			this.m_sb_ParamString.AppendFormat("{0}\t{1}{2}", strParam, value, this.LINE);
			return true;
		}

		public bool AddParam(string strParam, string strTextGroupKey, string strTextKey)
		{
			this.m_sb_ParamString.AppendFormat("{0}\t{1}\t{2}|{3}{4}", new object[]
			{
				strParam,
				this.TXTKEY,
				strTextGroupKey,
				strTextKey,
				this.LINE
			});
			return true;
		}

		public void Clear()
		{
			this.m_sb_ParamString.Length = 0;
		}

		public string ParseParamString(string strParamString, NTextManager kTextManager = null)
		{
			NDataReader nDataReader = new NDataReader();
			this.Clear();
			this.m_sb_ParamString.Append("(ParamStringError)");
			if (nDataReader.LoadFrom(strParamString))
			{
				if (nDataReader.BeginSection(this.SECTION_FORMAT))
				{
					this.m_sb_ParamString.Length = 0;
					foreach (NDataReader.Row row in nDataReader.CurrentSection)
					{
						if (this.TXTKEY.Equals(row[0]))
						{
							string text = row[1];
							text = this.__get_text_at_textmanager(kTextManager, text);
							this.m_sb_ParamString.Append(text);
							break;
						}
						this.m_sb_ParamString.Append(row.ToDataString());
						this.m_sb_ParamString.AppendFormat("{0}", this.LINE);
					}
				}
				if (nDataReader.BeginSection(this.SECTION_PARAMS))
				{
					foreach (NDataReader.Row row2 in nDataReader.CurrentSection)
					{
						string oldValue = row2[0];
						string text2 = row2[1];
						if (this.TXTKEY.Equals(text2))
						{
							text2 = row2.GetColumn(2);
							text2 = this.__get_text_at_textmanager(kTextManager, text2);
						}
						this.m_sb_ParamString = this.m_sb_ParamString.Replace(oldValue, text2);
					}
					this.m_sb_ParamString = this.m_sb_ParamString.Replace("{\\r\\n}", "\r\n");
				}
				else
				{
					this.__output_error(string.Format("{0} 섹션이 없습니다.", this.SECTION_PARAMS));
				}
			}
			else
			{
				this.__output_error("Load Failed!");
			}
			return this.m_sb_ParamString.ToString();
		}

		private string __get_text_at_textmanager(NTextManager kTextManager, string strTextGroupKeyPipeTextKey)
		{
			string text = strTextGroupKeyPipeTextKey;
			if (kTextManager != null)
			{
				string[] separator = new string[]
				{
					"|"
				};
				string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length != 2)
				{
					this.__output_error(string.Format("텍스트키 대체 오류! - 텍스트키값이 잘못되었습니다. {0}", text));
				}
				else
				{
					text = kTextManager.GetText(array[0], array[1], string.Empty);
					if (text == null)
					{
						text = strTextGroupKeyPipeTextKey;
					}
				}
			}
			else
			{
				this.__output_error(string.Format("텍스트키 대체 오류! - TextManager 가 설정되지 않았습니다! - {0}", text));
			}
			return text;
		}

		public override string ToString()
		{
			return this.m_sb_ParamString.ToString();
		}

		private void __output_error(string message)
		{
		}
	}
}
