using System;
using System.Text;
using UnityEngine;

namespace MATSDK
{
	public class MATDelegate : MonoBehaviour
	{
		public void trackerDidSucceed(string data)
		{
			MonoBehaviour.print("MATDelegate trackerDidSucceed: " + data);
		}

		public void trackerDidFail(string error)
		{
			MonoBehaviour.print("MATDelegate trackerDidFail: " + error);
		}

		public void trackerDidEnqueueRequest(string refId)
		{
			MonoBehaviour.print("MATDelegate trackerDidEnqueueRequest: " + refId);
		}

		public void trackerDidReceiveDeepLink(string url)
		{
			MonoBehaviour.print("MATDelegate trackerDidReceiveDeepLink: " + url);
		}

		public static string DecodeFrom64(string encodedString)
		{
			MonoBehaviour.print("MATDelegate.DecodeFrom64(string)");
			return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
		}
	}
}
