using System;
using UnityEngine;

namespace NLibCs
{
	public class ParseFailedException : Exception
	{
		private string strParseFailedException = string.Empty;

		public ParseFailedException(NDataStr dstr, uint value) : base(string.Format("ParseFailedException: NDataStr({0}) to {1}.", dstr.str, value.GetType().ToString()))
		{
			this.strParseFailedException = this.Message;
			Debug.LogError(this.strParseFailedException);
		}
	}
}
