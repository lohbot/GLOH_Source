using System;
using System.Collections.Generic;
using System.IO;

namespace NPatch
{
	public class Explorer
	{
		private List<float> VersionDirs = new List<float>();

		private string RootDir = string.Empty;

		public List<float> GetVersionDir(string root_dir)
		{
			this.VersionDirs.Clear();
			if (Directory.Exists(root_dir))
			{
				string[] directories = Directory.GetDirectories(root_dir);
				string[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					float item = 0f;
					if (float.TryParse(Path.GetFileName(path), out item))
					{
						this.VersionDirs.Add(item);
					}
				}
			}
			this.VersionDirs.Sort();
			this.RootDir = root_dir;
			return this.VersionDirs;
		}

		public float[] GetVersionDir_Reverse(string root_dir = null)
		{
			if (root_dir != this.RootDir)
			{
				this.GetVersionDir(root_dir);
			}
			float[] array = new float[this.VersionDirs.Count];
			this.VersionDirs.Reverse();
			this.VersionDirs.CopyTo(array);
			this.VersionDirs.Reverse();
			return array;
		}

		public string GetPreVersion(string version)
		{
			float num = 0f;
			float num2 = 0f;
			if (float.TryParse(version, out num))
			{
				foreach (float num3 in this.VersionDirs)
				{
					if (num3 == num)
					{
						break;
					}
					num2 = num3;
				}
			}
			return string.Format("{0:0}", num2);
		}

		public List<float> GetEntryVers(float start_version, float end_version)
		{
			List<float> list = new List<float>();
			foreach (float num in this.VersionDirs)
			{
				if (start_version < num && num <= end_version)
				{
					list.Add(num);
				}
			}
			return list;
		}
	}
}
