using System;

namespace WellFired
{
	public class USRecord
	{
		public enum PlayerResolution
		{
			_1920x1080,
			_1280x720,
			_960x540,
			_854x480,
			_720x576,
			_640x480,
			_Custom
		}

		public enum FrameRate
		{
			_24,
			_25,
			_30,
			_50,
			_60
		}

		public enum Upscaling
		{
			_1,
			_2,
			_4,
			_8
		}

		public static int GetResolutionX()
		{
			switch (USRecordRuntimePreferences.Resolution)
			{
			case USRecord.PlayerResolution._1920x1080:
				return 1920;
			case USRecord.PlayerResolution._1280x720:
				return 1280;
			case USRecord.PlayerResolution._960x540:
				return 960;
			case USRecord.PlayerResolution._854x480:
				return 854;
			case USRecord.PlayerResolution._720x576:
				return 720;
			case USRecord.PlayerResolution._640x480:
				return 640;
			case USRecord.PlayerResolution._Custom:
				return 60;
			default:
				return 960;
			}
		}

		public static int GetResolutionY()
		{
			switch (USRecordRuntimePreferences.Resolution)
			{
			case USRecord.PlayerResolution._1920x1080:
				return 1080;
			case USRecord.PlayerResolution._1280x720:
				return 720;
			case USRecord.PlayerResolution._960x540:
				return 540;
			case USRecord.PlayerResolution._854x480:
				return 480;
			case USRecord.PlayerResolution._720x576:
				return 576;
			case USRecord.PlayerResolution._640x480:
				return 480;
			case USRecord.PlayerResolution._Custom:
				return 60;
			default:
				return 420;
			}
		}

		public static int GetFramerate()
		{
			switch (USRecordRuntimePreferences.FrameRate)
			{
			case USRecord.FrameRate._24:
				return 24;
			case USRecord.FrameRate._25:
				return 25;
			case USRecord.FrameRate._30:
				return 30;
			case USRecord.FrameRate._50:
				return 50;
			case USRecord.FrameRate._60:
				return 60;
			default:
				return 60;
			}
		}

		public static int GetUpscaleAmount()
		{
			switch (USRecordRuntimePreferences.UpscaleAmount)
			{
			case USRecord.Upscaling._1:
				return 1;
			case USRecord.Upscaling._2:
				return 2;
			case USRecord.Upscaling._4:
				return 4;
			case USRecord.Upscaling._8:
				return 8;
			default:
				return 2;
			}
		}
	}
}
