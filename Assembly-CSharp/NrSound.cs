using System;
using TsBundle;

public class NrSound
{
	public static void ImmedatePlay(string domainKey, string categoryKey, string audioKey)
	{
		NrSound.ImmedatePlay(domainKey, categoryKey, audioKey, false);
	}

	public static void ImmedatePlay(string domainKey, string categoryKey, string audioKey, bool isDontDestroyOnLoad)
	{
		TsAudioManager.Container.RequestAudioClip(domainKey, categoryKey, audioKey, isDontDestroyOnLoad, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public static void ForSystem(string domainKey, string categoryKey, string audioKey)
	{
		TsAudioManager.Container.RequestAudioClip(domainKey, categoryKey, audioKey, new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedForSystem));
	}

	public static bool IsValid(string domainKey, string categoryKey)
	{
		TsAudioContainer.Domain domain = TsAudioManager.Container.FindDomain(domainKey);
		if (domain != null)
		{
			TsAudioContainer.Category category = domain.FindCategory(categoryKey);
			if (category != null)
			{
				return true;
			}
		}
		return false;
	}
}
