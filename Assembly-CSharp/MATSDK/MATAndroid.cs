using System;
using UnityEngine;

namespace MATSDK
{
	public class MATAndroid
	{
		private static MATAndroid instance;

		private AndroidJavaClass ajcMAT;

		public AndroidJavaObject ajcInstance;

		private AndroidJavaClass ajcUnityPlayer;

		private AndroidJavaObject ajcCurrentActivity;

		public static MATAndroid Instance
		{
			get
			{
				if (MATAndroid.instance == null)
				{
					MATAndroid.instance = new MATAndroid();
				}
				return MATAndroid.instance;
			}
		}

		private MATAndroid()
		{
		}

		public void Init(string advertiserId, string conversionKey)
		{
			if (this.ajcMAT == null)
			{
				this.ajcMAT = new AndroidJavaClass("com.mobileapptracker.MobileAppTracker");
				this.ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				this.ajcCurrentActivity = this.ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				this.ajcInstance = this.ajcMAT.CallStatic<AndroidJavaObject>("init", new object[]
				{
					this.ajcCurrentActivity,
					advertiserId,
					conversionKey
				});
				this.ajcInstance.Call("setPluginName", new object[]
				{
					"unity"
				});
			}
		}

		public void MeasureSession()
		{
			this.ajcInstance.Call("setReferralSources", new object[]
			{
				this.ajcCurrentActivity
			});
			this.ajcInstance.Call("measureSession", new object[0]);
		}

		public void MeasureEvent(string eventName)
		{
			this.ajcInstance.Call("measureEvent", new object[]
			{
				eventName
			});
		}

		public void MeasureEvent(MATEvent matEvent)
		{
			AndroidJavaObject mATEventJavaObject = this.GetMATEventJavaObject(matEvent);
			this.ajcInstance.Call("measureEvent", new object[]
			{
				mATEventJavaObject
			});
		}

		public bool GetIsPayingUser()
		{
			return this.ajcInstance.Call<bool>("getIsPayingUser", new object[0]);
		}

		public string GetMatId()
		{
			return this.ajcInstance.Call<string>("getMatId", new object[0]);
		}

		public string GetOpenLogId()
		{
			return this.ajcInstance.Call<string>("getOpenLogId", new object[0]);
		}

		public void SetAge(int age)
		{
			this.ajcInstance.Call("setAge", new object[]
			{
				age
			});
		}

		public void SetAllowDuplicates(bool allow)
		{
			this.ajcInstance.Call("setAllowDuplicates", new object[]
			{
				allow
			});
		}

		public void SetAndroidId(string androidId)
		{
			this.ajcInstance.Call("setAndroidId", new object[]
			{
				androidId
			});
		}

		public void SetAndroidIdMd5(string androidIdMd5)
		{
			this.ajcInstance.Call("setAndroidIdMd5", new object[]
			{
				androidIdMd5
			});
		}

		public void SetAndroidIdSha1(string androidIdSha1)
		{
			this.ajcInstance.Call("setAndroidIdSha1", new object[]
			{
				androidIdSha1
			});
		}

		public void SetAndroidIdSha256(string androidIdSha256)
		{
			this.ajcInstance.Call("setAndroidIdSha256", new object[]
			{
				androidIdSha256
			});
		}

		public void SetAppAdTracking(bool adTrackingEnabled)
		{
			this.ajcInstance.Call("setAppAdTrackingEnabled", new object[]
			{
				adTrackingEnabled
			});
		}

		public void SetCurrencyCode(string currencyCode)
		{
			this.ajcInstance.Call("setCurrencyCode", new object[]
			{
				currencyCode
			});
		}

		public void SetDebugMode(bool debugMode)
		{
			this.ajcInstance.Call("setDebugMode", new object[]
			{
				debugMode
			});
		}

		public void SetDeferredDeeplink(bool enableDeferredDeeplink, int timeoutInMillis)
		{
			this.ajcInstance.Call("setDeferredDeeplink", new object[]
			{
				enableDeferredDeeplink,
				timeoutInMillis
			});
		}

		public void SetDeviceId(string deviceId)
		{
			this.ajcInstance.Call("setDeviceId", new object[]
			{
				deviceId
			});
		}

		public void SetDelegate(bool enable)
		{
			if (enable)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.matunityutils.MATUnityResponse", new object[0]);
				this.ajcInstance.Call("setMATResponse", new object[]
				{
					androidJavaObject
				});
			}
		}

		public void SetEmailCollection(bool collectEmail)
		{
			this.ajcInstance.Call("setEmailCollection", new object[]
			{
				collectEmail
			});
		}

		public void SetExistingUser(bool isExistingUser)
		{
			this.ajcInstance.Call("setExistingUser", new object[]
			{
				isExistingUser
			});
		}

		public void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage)
		{
			this.ajcInstance.Call("setFacebookEventLogging", new object[]
			{
				fbEventLogging,
				this.ajcCurrentActivity,
				limitEventAndDataUsage
			});
		}

		public void SetFacebookUserId(string facebookUserId)
		{
			this.ajcInstance.Call("setFacebookUserId", new object[]
			{
				facebookUserId
			});
		}

		public void SetGender(int gender)
		{
			this.ajcInstance.Call("setGender", new object[]
			{
				gender
			});
		}

		public void SetGoogleAdvertisingId(string googleAid, bool isLATEnabled)
		{
			this.ajcInstance.Call("setGoogleAdvertisingId", new object[]
			{
				googleAid,
				isLATEnabled
			});
		}

		public void SetGoogleUserId(string googleUserId)
		{
			this.ajcInstance.Call("setGoogleUserId", new object[]
			{
				googleUserId
			});
		}

		public void SetLocation(double latitude, double longitude, double altitude)
		{
			this.ajcInstance.Call("setLatitude", new object[]
			{
				latitude
			});
			this.ajcInstance.Call("setLongitude", new object[]
			{
				longitude
			});
			this.ajcInstance.Call("setAltitude", new object[]
			{
				altitude
			});
		}

		public void SetMacAddress(string macAddress)
		{
			this.ajcInstance.Call("setMacAddress", new object[]
			{
				macAddress
			});
		}

		public void SetPackageName(string packageName)
		{
			this.ajcInstance.Call("setPackageName", new object[]
			{
				packageName
			});
		}

		public void SetPhoneNumber(string phoneNumber)
		{
			this.ajcInstance.Call("setPhoneNumber", new object[]
			{
				phoneNumber
			});
		}

		public void SetPayingUser(bool isPayingUser)
		{
			this.ajcInstance.Call("setIsPayingUser", new object[]
			{
				isPayingUser
			});
		}

		public void SetSiteId(string siteId)
		{
			this.ajcInstance.Call("setSiteId", new object[]
			{
				siteId
			});
		}

		public void SetTRUSTeId(string tpid)
		{
			this.ajcInstance.Call("setTRUSTeId", new object[]
			{
				tpid
			});
		}

		public void SetTwitterUserId(string twitterUserId)
		{
			this.ajcInstance.Call("setTwitterUserId", new object[]
			{
				twitterUserId
			});
		}

		public void SetUserEmail(string userEmail)
		{
			this.ajcInstance.Call("setUserEmail", new object[]
			{
				userEmail
			});
		}

		public void SetUserId(string userId)
		{
			this.ajcInstance.Call("setUserId", new object[]
			{
				userId
			});
		}

		public void SetUserName(string userName)
		{
			this.ajcInstance.Call("setUserName", new object[]
			{
				userName
			});
		}

		public void SetPreloadedApp(MATPreloadData preloadData)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.mobileapptracker.MATPreloadData", new object[]
			{
				preloadData.publisherId
			});
			if (preloadData.offerId != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withOfferId", new object[]
				{
					preloadData.offerId
				});
			}
			if (preloadData.agencyId != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAgencyId", new object[]
				{
					preloadData.agencyId
				});
			}
			if (preloadData.publisherReferenceId != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherReferenceId", new object[]
				{
					preloadData.publisherReferenceId
				});
			}
			if (preloadData.publisherSub1 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSub1", new object[]
				{
					preloadData.publisherSub1
				});
			}
			if (preloadData.publisherSub2 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSub2", new object[]
				{
					preloadData.publisherSub2
				});
			}
			if (preloadData.publisherSub3 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSub3", new object[]
				{
					preloadData.publisherSub3
				});
			}
			if (preloadData.publisherSub4 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSub4", new object[]
				{
					preloadData.publisherSub4
				});
			}
			if (preloadData.publisherSub5 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSub5", new object[]
				{
					preloadData.publisherSub5
				});
			}
			if (preloadData.publisherSubAd != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubAd", new object[]
				{
					preloadData.publisherSubAd
				});
			}
			if (preloadData.publisherSubAdgroup != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubAdgroup", new object[]
				{
					preloadData.publisherSubAdgroup
				});
			}
			if (preloadData.publisherSubCampaign != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubCampaign", new object[]
				{
					preloadData.publisherSubCampaign
				});
			}
			if (preloadData.publisherSubKeyword != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubKeyword", new object[]
				{
					preloadData.publisherSubKeyword
				});
			}
			if (preloadData.publisherSubPublisher != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubPublisher", new object[]
				{
					preloadData.publisherSubPublisher
				});
			}
			if (preloadData.publisherSubSite != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withPublisherSubSite", new object[]
				{
					preloadData.publisherSubSite
				});
			}
			if (preloadData.advertiserSubAd != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubAd", new object[]
				{
					preloadData.advertiserSubAd
				});
			}
			if (preloadData.advertiserSubAdgroup != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubAdgroup", new object[]
				{
					preloadData.advertiserSubAdgroup
				});
			}
			if (preloadData.advertiserSubCampaign != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubCampaign", new object[]
				{
					preloadData.advertiserSubCampaign
				});
			}
			if (preloadData.advertiserSubKeyword != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubKeyword", new object[]
				{
					preloadData.advertiserSubKeyword
				});
			}
			if (preloadData.advertiserSubPublisher != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubPublisher", new object[]
				{
					preloadData.advertiserSubPublisher
				});
			}
			if (preloadData.advertiserSubSite != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserSubSite", new object[]
				{
					preloadData.advertiserSubSite
				});
			}
			this.ajcInstance.Call("setPreloadedApp", new object[]
			{
				androidJavaObject
			});
		}

		private AndroidJavaObject GetMATEventJavaObject(MATEvent matEvent)
		{
			AndroidJavaObject androidJavaObject;
			if (matEvent.name == null)
			{
				androidJavaObject = new AndroidJavaObject("com.mobileapptracker.MATEvent", new object[]
				{
					matEvent.id
				});
			}
			else
			{
				androidJavaObject = new AndroidJavaObject("com.mobileapptracker.MATEvent", new object[]
				{
					matEvent.name
				});
			}
			double? revenue = matEvent.revenue;
			if (revenue.HasValue)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withRevenue", new object[]
				{
					matEvent.revenue
				});
			}
			if (matEvent.currencyCode != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withCurrencyCode", new object[]
				{
					matEvent.currencyCode
				});
			}
			if (matEvent.advertiserRefId != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAdvertiserRefId", new object[]
				{
					matEvent.advertiserRefId
				});
			}
			if (matEvent.eventItems != null)
			{
				AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
				MATItem[] eventItems = matEvent.eventItems;
				for (int i = 0; i < eventItems.Length; i++)
				{
					MATItem mATItem = eventItems[i];
					AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("com.mobileapptracker.MATEventItem", new object[]
					{
						mATItem.name
					});
					int? quantity = mATItem.quantity;
					if (quantity.HasValue)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withQuantity", new object[]
						{
							mATItem.quantity
						});
					}
					double? unitPrice = mATItem.unitPrice;
					if (unitPrice.HasValue)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withUnitPrice", new object[]
						{
							mATItem.unitPrice
						});
					}
					double? revenue2 = mATItem.revenue;
					if (revenue2.HasValue)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withRevenue", new object[]
						{
							mATItem.revenue
						});
					}
					if (mATItem.attribute1 != null)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withAttribute1", new object[]
						{
							mATItem.attribute1
						});
					}
					if (mATItem.attribute2 != null)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withAttribute2", new object[]
						{
							mATItem.attribute2
						});
					}
					if (mATItem.attribute3 != null)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withAttribute3", new object[]
						{
							mATItem.attribute3
						});
					}
					if (mATItem.attribute4 != null)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withAttribute4", new object[]
						{
							mATItem.attribute4
						});
					}
					if (mATItem.attribute5 != null)
					{
						androidJavaObject3 = androidJavaObject3.Call<AndroidJavaObject>("withAttribute5", new object[]
						{
							mATItem.attribute5
						});
					}
					androidJavaObject2.Call<bool>("add", new object[]
					{
						androidJavaObject3
					});
				}
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withEventItems", new object[]
				{
					androidJavaObject2
				});
			}
			if (matEvent.receipt != null && matEvent.receiptSignature != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withReceipt", new object[]
				{
					matEvent.receipt,
					matEvent.receiptSignature
				});
			}
			if (matEvent.contentType != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withContentType", new object[]
				{
					matEvent.contentType
				});
			}
			if (matEvent.contentId != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withContentId", new object[]
				{
					matEvent.contentId
				});
			}
			int? level = matEvent.level;
			if (level.HasValue)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withLevel", new object[]
				{
					matEvent.level
				});
			}
			int? quantity2 = matEvent.quantity;
			if (quantity2.HasValue)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withQuantity", new object[]
				{
					matEvent.quantity
				});
			}
			if (matEvent.searchString != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withSearchString", new object[]
				{
					matEvent.searchString
				});
			}
			DateTime? date = matEvent.date1;
			if (date.HasValue)
			{
				DateTime? date2 = matEvent.date1;
				TimeSpan timeSpan = new TimeSpan(date2.Value.Ticks);
				double totalMilliseconds = timeSpan.TotalMilliseconds;
				DateTime dateTime = new DateTime(1970, 1, 1);
				double arg_465_0 = totalMilliseconds;
				TimeSpan timeSpan2 = new TimeSpan(dateTime.Ticks);
				double num = arg_465_0 - timeSpan2.TotalMilliseconds;
				AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.lang.Double", new object[]
				{
					num
				});
				long num2 = androidJavaObject4.Call<long>("longValue", new object[0]);
				AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("java.util.Date", new object[]
				{
					num2
				});
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withDate1", new object[]
				{
					androidJavaObject5
				});
			}
			DateTime? date3 = matEvent.date2;
			if (date3.HasValue)
			{
				DateTime? date4 = matEvent.date2;
				TimeSpan timeSpan3 = new TimeSpan(date4.Value.Ticks);
				double totalMilliseconds2 = timeSpan3.TotalMilliseconds;
				DateTime dateTime2 = new DateTime(1970, 1, 1);
				double arg_52E_0 = totalMilliseconds2;
				TimeSpan timeSpan4 = new TimeSpan(dateTime2.Ticks);
				double num3 = arg_52E_0 - timeSpan4.TotalMilliseconds;
				AndroidJavaObject androidJavaObject6 = new AndroidJavaObject("java.lang.Double", new object[]
				{
					num3
				});
				long num4 = androidJavaObject6.Call<long>("longValue", new object[0]);
				AndroidJavaObject androidJavaObject7 = new AndroidJavaObject("java.util.Date", new object[]
				{
					num4
				});
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withDate2", new object[]
				{
					androidJavaObject7
				});
			}
			if (matEvent.attribute1 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAttribute1", new object[]
				{
					matEvent.attribute1
				});
			}
			if (matEvent.attribute2 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAttribute2", new object[]
				{
					matEvent.attribute2
				});
			}
			if (matEvent.attribute3 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAttribute3", new object[]
				{
					matEvent.attribute3
				});
			}
			if (matEvent.attribute4 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAttribute4", new object[]
				{
					matEvent.attribute4
				});
			}
			if (matEvent.attribute5 != null)
			{
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("withAttribute5", new object[]
				{
					matEvent.attribute5
				});
			}
			return androidJavaObject;
		}
	}
}
