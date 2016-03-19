using System;
using UnityEngine;

namespace omniata
{
	public class DefaultReachability : Reachability
	{
		public bool Reachable()
		{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}
}
