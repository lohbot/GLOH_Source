using MATSDK;
using MATWP8;
using System;
using UnityEngine;

public class SampleWP8MATResponse : MATResponse
{
	private MATDelegate message_receiver = GameObject.Find("MobileAppTracker").GetComponent<MATDelegate>();

	public void DidSucceedWithData(string response)
	{
		if (this.message_receiver != null)
		{
			this.message_receiver.trackerDidSucceed(string.Empty + response);
		}
	}

	public void DidFailWithError(string error)
	{
		if (this.message_receiver != null)
		{
			this.message_receiver.trackerDidFail(string.Empty + error);
		}
	}

	public void EnqueuedActionWithRefId(string refId)
	{
		if (this.message_receiver != null)
		{
			this.message_receiver.trackerDidEnqueueRequest(string.Empty + refId);
		}
	}
}
