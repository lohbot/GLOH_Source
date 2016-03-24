using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	public static class USRecordRuntimePreferences
	{
		private static string resolutionPref = "uSequencer-RecordPrefs-CaptureResolution";

		private static string frameRatePref = "uSequencer-RecordPrefs-CaptureFrameRate";

		private static string upscaleAmountPref = "uSequencer-RecordPrefs-UpscaleAmount";

		private static string pathPref = "uSequencer-RecordPrefs-CapturePath";

		private static string presetNamePref = "uSequencer-RecordPrefs-PresetName";

		private static string presetPref = "uSequencer-RecordPrefs-PresetPrefData";

		private static string selectedPresetPref = "uSequencer-RecordPrefs-SelectedPreset";

		private static USRecord.PlayerResolution defaultResolution = USRecord.PlayerResolution._960x540;

		private static USRecord.FrameRate defaultFrameRate = USRecord.FrameRate._60;

		private static USRecord.Upscaling defaultUpscaling = USRecord.Upscaling._2;

		private static string defaultPreset = "(Default;c:/Blah/Save_Loc;_2;_60)";

		private static int defaultSelectedPreset;

		public static int SelectedPreset
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.selectedPresetPref))
				{
					PlayerPrefs.SetInt(USRecordRuntimePreferences.selectedPresetPref, USRecordRuntimePreferences.defaultSelectedPreset);
				}
				return PlayerPrefs.GetInt(USRecordRuntimePreferences.selectedPresetPref);
			}
			set
			{
				PlayerPrefs.SetInt(USRecordRuntimePreferences.selectedPresetPref, value);
			}
		}

		public static string PresetName
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.presetNamePref))
				{
					PlayerPrefs.SetString(USRecordRuntimePreferences.presetNamePref, "default");
				}
				return PlayerPrefs.GetString(USRecordRuntimePreferences.presetNamePref);
			}
			set
			{
				PlayerPrefs.SetString(USRecordRuntimePreferences.presetNamePref, value);
			}
		}

		public static USRecord.PlayerResolution Resolution
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.resolutionPref))
				{
					PlayerPrefs.SetInt(USRecordRuntimePreferences.resolutionPref, (int)USRecordRuntimePreferences.defaultResolution);
				}
				return (USRecord.PlayerResolution)PlayerPrefs.GetInt(USRecordRuntimePreferences.resolutionPref);
			}
			set
			{
				PlayerPrefs.SetInt(USRecordRuntimePreferences.resolutionPref, (int)value);
			}
		}

		public static USRecord.FrameRate FrameRate
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.frameRatePref))
				{
					PlayerPrefs.SetInt(USRecordRuntimePreferences.frameRatePref, (int)USRecordRuntimePreferences.defaultFrameRate);
				}
				return (USRecord.FrameRate)PlayerPrefs.GetInt(USRecordRuntimePreferences.frameRatePref);
			}
			set
			{
				PlayerPrefs.SetInt(USRecordRuntimePreferences.frameRatePref, (int)value);
			}
		}

		public static USRecord.Upscaling UpscaleAmount
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.upscaleAmountPref))
				{
					PlayerPrefs.SetInt(USRecordRuntimePreferences.upscaleAmountPref, (int)USRecordRuntimePreferences.defaultUpscaling);
				}
				return (USRecord.Upscaling)PlayerPrefs.GetInt(USRecordRuntimePreferences.upscaleAmountPref);
			}
			set
			{
				PlayerPrefs.SetInt(USRecordRuntimePreferences.upscaleAmountPref, (int)value);
			}
		}

		public static string CapturePath
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.pathPref))
				{
					PlayerPrefs.SetString(USRecordRuntimePreferences.pathPref, USRecordRuntimePreferences.GetDefaultCapturePath());
				}
				return PlayerPrefs.GetString(USRecordRuntimePreferences.pathPref);
			}
			set
			{
				PlayerPrefs.SetString(USRecordRuntimePreferences.pathPref, value);
			}
		}

		private static string Presets
		{
			get
			{
				if (!PlayerPrefs.HasKey(USRecordRuntimePreferences.presetPref))
				{
					PlayerPrefs.SetString(USRecordRuntimePreferences.presetPref, USRecordRuntimePreferences.defaultPreset);
				}
				return PlayerPrefs.GetString(USRecordRuntimePreferences.presetPref);
			}
			set
			{
				PlayerPrefs.SetString(USRecordRuntimePreferences.presetPref, value);
			}
		}

		public static string GetDefaultCapturePath()
		{
			int startIndex = Application.dataPath.LastIndexOf("/");
			string str = Application.dataPath.Remove(startIndex);
			return str + "/Output";
		}

		public static void SetNewPreset(int newSelectedPreset)
		{
			List<PresetInfo> presetInfo = USRecordRuntimePreferences.GetPresetInfo();
			USRecordRuntimePreferences.SelectedPreset = newSelectedPreset;
			USRecordRuntimePreferences.PresetName = presetInfo[USRecordRuntimePreferences.SelectedPreset].Name;
			USRecordRuntimePreferences.CapturePath = presetInfo[USRecordRuntimePreferences.SelectedPreset].CapturePath;
			USRecordRuntimePreferences.UpscaleAmount = presetInfo[USRecordRuntimePreferences.SelectedPreset].UpscaleAmount;
			USRecordRuntimePreferences.FrameRate = presetInfo[USRecordRuntimePreferences.SelectedPreset].FrameRate;
		}

		public static void Reset()
		{
			List<PresetInfo> presetInfo = USRecordRuntimePreferences.GetPresetInfo();
			USRecordRuntimePreferences.PresetName = presetInfo[USRecordRuntimePreferences.SelectedPreset].Name;
			USRecordRuntimePreferences.CapturePath = presetInfo[USRecordRuntimePreferences.SelectedPreset].CapturePath;
			USRecordRuntimePreferences.UpscaleAmount = presetInfo[USRecordRuntimePreferences.SelectedPreset].UpscaleAmount;
			USRecordRuntimePreferences.FrameRate = presetInfo[USRecordRuntimePreferences.SelectedPreset].FrameRate;
		}

		public static void Destroy()
		{
			if (USRecordRuntimePreferences.CapturePath.Length == 0)
			{
				Debug.LogError(string.Format("Directory Path, specified in the uSequencer Preference Window is invalid, resetting to the default : {0}", USRecordRuntimePreferences.GetDefaultCapturePath()));
				USRecordRuntimePreferences.CapturePath = USRecordRuntimePreferences.GetDefaultCapturePath();
			}
		}

		public static List<PresetInfo> GetPresetInfo()
		{
			List<PresetInfo> list = new List<PresetInfo>();
			string presets = USRecordRuntimePreferences.Presets;
			if (presets.Length > 0)
			{
				string[] array = presets.Split(new char[]
				{
					')'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (text.Length > 0)
					{
						string text2 = text.Replace("(", string.Empty).Replace(")", string.Empty);
						string[] array3 = text2.Split(new char[]
						{
							';'
						});
						string name = array3[0];
						string capturePath = array3[1];
						USRecord.Upscaling upscaleAmount = (USRecord.Upscaling)((int)Enum.Parse(typeof(USRecord.Upscaling), array3[2]));
						USRecord.FrameRate framerate = (USRecord.FrameRate)((int)Enum.Parse(typeof(USRecord.FrameRate), array3[3]));
						list.Add(new PresetInfo(name, capturePath, upscaleAmount, framerate));
					}
				}
			}
			return list;
		}

		public static void SaveAsNewPreset(string name, string capturePath, USRecord.Upscaling upscaleAmount, USRecord.FrameRate frameRate)
		{
			name = USRecordRuntimePreferences.UniqifyName(name);
			string str = string.Format("({0};{1};{2};{3})", new object[]
			{
				name,
				capturePath,
				upscaleAmount,
				frameRate
			});
			USRecordRuntimePreferences.Presets += str;
		}

		public static void DeletePreset()
		{
			List<PresetInfo> presetInfo = USRecordRuntimePreferences.GetPresetInfo();
			if (presetInfo.Count<PresetInfo>() <= 1)
			{
				return;
			}
			presetInfo.Remove(presetInfo[USRecordRuntimePreferences.SelectedPreset]);
			USRecordRuntimePreferences.Presets = string.Empty;
			foreach (PresetInfo current in presetInfo)
			{
				USRecordRuntimePreferences.SaveAsNewPreset(current.Name, current.CapturePath, current.UpscaleAmount, current.FrameRate);
			}
			if (USRecordRuntimePreferences.SelectedPreset >= presetInfo.Count<PresetInfo>())
			{
				USRecordRuntimePreferences.SelectedPreset--;
			}
			USRecordRuntimePreferences.PresetName = presetInfo[USRecordRuntimePreferences.SelectedPreset].Name;
			USRecordRuntimePreferences.CapturePath = presetInfo[USRecordRuntimePreferences.SelectedPreset].CapturePath;
			USRecordRuntimePreferences.UpscaleAmount = presetInfo[USRecordRuntimePreferences.SelectedPreset].UpscaleAmount;
			USRecordRuntimePreferences.FrameRate = presetInfo[USRecordRuntimePreferences.SelectedPreset].FrameRate;
		}

		public static void SavePresets(List<PresetInfo> presets)
		{
			USRecordRuntimePreferences.Presets = string.Empty;
			foreach (PresetInfo current in presets)
			{
				USRecordRuntimePreferences.SaveAsNewPreset(current.Name, current.CapturePath, current.UpscaleAmount, current.FrameRate);
			}
		}

		private static string UniqifyName(string name)
		{
			bool flag = true;
			List<PresetInfo> presetInfo = USRecordRuntimePreferences.GetPresetInfo();
			foreach (PresetInfo current in presetInfo)
			{
				if (current.Name == name)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				name += "1";
				return USRecordRuntimePreferences.UniqifyName(name);
			}
			return name;
		}
	}
}
