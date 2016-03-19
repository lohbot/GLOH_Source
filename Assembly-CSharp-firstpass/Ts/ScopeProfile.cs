using System;

namespace Ts
{
	public struct ScopeProfile : IDisposable
	{
		public ScopeProfile(string profileName)
		{
			if (string.IsNullOrEmpty(profileName))
			{
			}
		}

		public void Dispose()
		{
		}
	}
}
