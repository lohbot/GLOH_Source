using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace TsLibs
{
	public class TsDataSection : IEnumerable
	{
		private TsDataReader _owner;

		internal string _sectionName = string.Empty;

		internal StringBuilder _context;

		internal bool _isFinded;

		internal int _dataCount;

		internal int _beginRow = -1;

		internal int _beginRowSub = -1;

		internal int _currentRow = -1;

		internal int _currentRowAtSection = -1;

		internal int _rowsCount = -1;

		public TsDataStr this[string dataKeyName]
		{
			get
			{
				TsDataStr result;
				this._owner.ReadKeyData(dataKeyName, out result.str, string.Empty);
				return result;
			}
		}

		public TsDataReader.Row this[int sectionRowNum]
		{
			get
			{
				if (sectionRowNum == this._currentRowAtSection)
				{
					return this.CurrentRow;
				}
				if (this._currentRowAtSection < sectionRowNum)
				{
					int currentRowAtSection = this._currentRowAtSection;
					while (!this.IsEndOfSection)
					{
						if (sectionRowNum == currentRowAtSection++)
						{
							return this.CurrentRow;
						}
						this.NextLine();
					}
				}
				else
				{
					int num = 0;
					this.FirstSectionLine();
					while (!this.IsEndOfSection)
					{
						TsDataReader.Row currentRow = this.CurrentRow;
						if (sectionRowNum == num++)
						{
							return this.CurrentRow;
						}
						this.NextLine();
					}
				}
				return TsDataReader.Row.EMPTY;
			}
		}

		public string SectionName
		{
			get
			{
				return this._sectionName;
			}
		}

		public int DataCount
		{
			get
			{
				if (this._dataCount == 0)
				{
					while (!this.IsEndOfSection)
					{
						if (this.CurrentRow.LineType == TsDataReader.Row.TYPE.LINE_DATA)
						{
							this._dataCount++;
						}
						this.NextLine();
					}
					this._currentRow = this._beginRow;
				}
				return this._dataCount;
			}
		}

		public string SectionString
		{
			get
			{
				if (this._context == null)
				{
					this._context = new StringBuilder();
				}
				while (!this.IsEndOfSection)
				{
					TsDataReader.Row currentRow = this.CurrentRow;
					if (currentRow.LineType == TsDataReader.Row.TYPE.LINE_DATA)
					{
						this._context.AppendLine(currentRow.ToDataString());
					}
					this.NextDataLine(false);
				}
				return this._context.ToString();
			}
		}

		public TsDataReader.Row CurrentRow
		{
			get
			{
				if (this._rowsCount == -1)
				{
					this._rowsCount = this._owner.m_list_Rows.Count;
				}
				if (0 <= this._currentRow && this._currentRow < this._rowsCount)
				{
					return this._owner.m_list_Rows[this._currentRow];
				}
				return TsDataReader.Row.EMPTY;
			}
		}

		public bool IsEOF
		{
			get
			{
				return this._rowsCount < this._currentRow;
			}
		}

		public bool IsEndOfSection
		{
			get
			{
				return this.IsEOF || (this.CurrentRow.LineType == TsDataReader.Row.TYPE.LINE_SECTION && this._currentRow != this._beginRow && this._beginRow != -1);
			}
		}

		public bool IsEndOfSubSection
		{
			get
			{
				if (this._owner.UseSubSection)
				{
					return this.IsEndOfSection || (this.CurrentRow.LineType == TsDataReader.Row.TYPE.LINE_SUBSECTION && this._currentRow != this._beginRowSub);
				}
				return this.IsEndOfSection;
			}
		}

		internal TsDataSection(TsDataReader owner, string sectionName)
		{
			this._owner = owner;
			this.FindSection(sectionName);
		}

		internal void Clear()
		{
			this._isFinded = false;
			this._sectionName = string.Empty;
			this._dataCount = 0;
			this._currentRow = -1;
			this._currentRowAtSection = -1;
			this._beginRow = -1;
			this._beginRowSub = -1;
			this._rowsCount = -1;
		}

		internal void FirstSectionLine()
		{
			this._currentRow = this._beginRow;
			this._currentRowAtSection = 0;
		}

		internal bool FindSection(string sectionName)
		{
			if (this._isFinded && this._sectionName == sectionName.ToLower())
			{
				this.FirstSectionLine();
				return true;
			}
			this.Clear();
			this._sectionName = sectionName;
			if (sectionName != null && sectionName != string.Empty)
			{
				this._isFinded = this._findSection(this._sectionName);
			}
			else
			{
				this._isFinded = true;
			}
			this._rowsCount = this._owner.m_list_Rows.Count;
			return this._isFinded;
		}

		private bool _findSection(string sectionName)
		{
			int num = 0;
			for (int i = 0; i < this._owner.m_list_Rows.Count; i++)
			{
				TsDataReader.Row row = this._owner.m_list_Rows[i];
				if (row.LineType == TsDataReader.Row.TYPE.LINE_SECTION)
				{
					string text = row.GetColumn(0).ToLower();
					if (text[0] == '[' && sectionName[0] != '[')
					{
						sectionName = string.Format("[{0}]", sectionName);
					}
					if (text.Equals(sectionName.ToLower()))
					{
						this._beginRow = num;
						this._beginRowSub = -1;
						this._currentRow = num + 1;
						this._currentRowAtSection = 0;
						return true;
					}
				}
				num++;
			}
			return false;
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			TsDataSection.<GetEnumerator>c__Iterator26 <GetEnumerator>c__Iterator = new TsDataSection.<GetEnumerator>c__Iterator26();
			<GetEnumerator>c__Iterator.<>f__this = this;
			return <GetEnumerator>c__Iterator;
		}

		public void FirstLine(int offset = 0)
		{
			if (this._rowsCount == -1)
			{
				this._rowsCount = this._owner.m_list_Rows.Count;
			}
			if (this._beginRow == -1)
			{
				this._beginRow = 0;
				this._beginRowSub = 0;
			}
			this._currentRow = this._beginRow + offset;
			this._currentRowAtSection = offset;
		}

		public int NextLine()
		{
			this._currentRowAtSection++;
			this._currentRow++;
			return this._currentRow;
		}

		public int NextDataLine(bool bSubSection = false)
		{
			bool flag = true;
			while (flag)
			{
				if (this.IsEOF)
				{
					break;
				}
				if (bSubSection && this.IsEndOfSubSection)
				{
					break;
				}
				this._currentRow++;
				this._currentRowAtSection++;
				TsDataReader.Row.TYPE lineType = this.CurrentRow.LineType;
				if (lineType == TsDataReader.Row.TYPE.LINE_DATA || lineType == TsDataReader.Row.TYPE.LINE_SECTION || lineType == TsDataReader.Row.TYPE.LINE_SUBSECTION)
				{
					flag = false;
				}
			}
			return this._currentRow;
		}
	}
}
