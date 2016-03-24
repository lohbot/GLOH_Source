using System;

namespace MATSDK
{
	public struct MATPreloadData
	{
		public string advertiserSubAd;

		public string advertiserSubAdgroup;

		public string advertiserSubCampaign;

		public string advertiserSubKeyword;

		public string advertiserSubPublisher;

		public string advertiserSubSite;

		public string agencyId;

		public string offerId;

		public string publisherId;

		public string publisherReferenceId;

		public string publisherSub1;

		public string publisherSub2;

		public string publisherSub3;

		public string publisherSub4;

		public string publisherSub5;

		public string publisherSubAd;

		public string publisherSubAdgroup;

		public string publisherSubCampaign;

		public string publisherSubKeyword;

		public string publisherSubPublisher;

		public string publisherSubSite;

		public MATPreloadData(string publisherId)
		{
			this.advertiserSubAd = null;
			this.advertiserSubAdgroup = null;
			this.advertiserSubCampaign = null;
			this.advertiserSubKeyword = null;
			this.advertiserSubPublisher = null;
			this.advertiserSubSite = null;
			this.agencyId = null;
			this.offerId = null;
			this.publisherId = publisherId;
			this.publisherReferenceId = null;
			this.publisherSub1 = null;
			this.publisherSub2 = null;
			this.publisherSub3 = null;
			this.publisherSub4 = null;
			this.publisherSub5 = null;
			this.publisherSubAd = null;
			this.publisherSubAdgroup = null;
			this.publisherSubCampaign = null;
			this.publisherSubKeyword = null;
			this.publisherSubPublisher = null;
			this.publisherSubSite = null;
		}
	}
}
