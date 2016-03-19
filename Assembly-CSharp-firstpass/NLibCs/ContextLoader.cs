using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace NLibCs
{
	public static class ContextLoader
	{
		public class AtFile : IContextLoader
		{
			public bool Load(string path, out string context)
			{
				context = string.Empty;
				bool result;
				try
				{
					using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default, true))
						{
							context = streamReader.ReadToEnd();
						}
						result = true;
					}
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			}
		}

		public class AtUnityResource : IContextLoader
		{
			public bool Load(string path, out string context)
			{
				context = string.Empty;
				bool result;
				try
				{
					TextAsset textAsset = Resources.Load(path) as TextAsset;
					context = textAsset.text;
					result = true;
				}
				catch (Exception)
				{
					result = false;
				}
				return result;
			}
		}
	}
}
