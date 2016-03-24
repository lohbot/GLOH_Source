using MATSDK;
using System;
using UnityEngine;

public class MATSample : MonoBehaviour
{
	private string MAT_ADVERTISER_ID;

	private string MAT_CONVERSION_KEY;

	private string MAT_PACKAGE_NAME;

	private void Awake()
	{
		this.MAT_ADVERTISER_ID = "877";
		this.MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
		this.MAT_PACKAGE_NAME = "com.hasoffers.unitytestapp";
		MonoBehaviour.print("Awake called: " + this.MAT_ADVERTISER_ID + ", " + this.MAT_CONVERSION_KEY);
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.fontStyle = FontStyle.Bold;
		gUIStyle.fontSize = 50;
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		gUIStyle.normal.textColor = Color.white;
		GUI.Label(new Rect(10f, 5f, (float)(Screen.width - 20), (float)(Screen.height / 10)), "MAT Unity Test App", gUIStyle);
		GUI.skin.button.fontSize = 40;
		if (GUI.Button(new Rect(10f, (float)(Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Start MAT"))
		{
			MonoBehaviour.print("Start MAT clicked");
			MATBinding.Init(this.MAT_ADVERTISER_ID, this.MAT_CONVERSION_KEY);
			MATBinding.SetPackageName(this.MAT_PACKAGE_NAME);
			MATBinding.SetFacebookEventLogging(true, false);
			MATBinding.CheckForDeferredDeeplinkWithTimeout(750.0);
			MATBinding.AutomateIapEventMeasurement(true);
		}
		else if (GUI.Button(new Rect(10f, (float)(2 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Set Delegate"))
		{
			MonoBehaviour.print("Set Delegate clicked");
			MATBinding.SetDelegate(true);
		}
		else if (GUI.Button(new Rect(10f, (float)(3 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Enable Debug Mode"))
		{
			MonoBehaviour.print("Enable Debug Mode clicked");
			MATBinding.SetDebugMode(true);
		}
		else if (GUI.Button(new Rect(10f, (float)(4 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Allow Duplicates"))
		{
			MonoBehaviour.print("Allow Duplicates clicked");
			MATBinding.SetAllowDuplicates(true);
		}
		else if (GUI.Button(new Rect(10f, (float)(5 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Measure Session"))
		{
			MonoBehaviour.print("Measure Session clicked");
			MATBinding.MeasureSession();
		}
		else if (GUI.Button(new Rect(10f, (float)(6 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Measure Event"))
		{
			MonoBehaviour.print("Measure Event clicked");
			MATBinding.MeasureEvent("evt11");
		}
		else if (GUI.Button(new Rect(10f, (float)(7 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Measure Event With Event Items"))
		{
			MonoBehaviour.print("Measure Event With Event Items clicked");
			MATItem mATItem = default(MATItem);
			mATItem.name = "subitem1";
			mATItem.unitPrice = new double?(5.0);
			mATItem.quantity = new int?(5);
			mATItem.revenue = new double?(3.0);
			mATItem.attribute2 = "attrValue2";
			mATItem.attribute3 = "attrValue3";
			mATItem.attribute4 = "attrValue4";
			mATItem.attribute5 = "attrValue5";
			MATItem mATItem2 = default(MATItem);
			mATItem2.name = "subitem2";
			mATItem2.unitPrice = new double?(1.0);
			mATItem2.quantity = new int?(3);
			mATItem2.revenue = new double?(1.5);
			mATItem2.attribute1 = "attrValue1";
			mATItem2.attribute3 = "attrValue3";
			MATItem[] eventItems = new MATItem[]
			{
				mATItem,
				mATItem2
			};
			MATEvent matEvent = new MATEvent("purchase");
			matEvent.revenue = new double?(10.0);
			matEvent.currencyCode = "AUD";
			matEvent.advertiserRefId = "ref222";
			matEvent.attribute1 = "test_attribute1";
			matEvent.attribute2 = "test_attribute2";
			matEvent.attribute3 = "test_attribute3";
			matEvent.attribute4 = "test_attribute4";
			matEvent.attribute5 = "test_attribute5";
			matEvent.contentType = "test_contentType";
			matEvent.contentId = "test_contentId";
			matEvent.date1 = new DateTime?(DateTime.UtcNow);
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = new DateTime(2, 1, 1);
			matEvent.date2 = new DateTime?(utcNow.Add(new TimeSpan(dateTime.Ticks)));
			matEvent.level = new int?(3);
			matEvent.quantity = new int?(2);
			matEvent.rating = new double?(4.5);
			matEvent.searchString = "test_searchString";
			matEvent.eventItems = eventItems;
			matEvent.transactionState = new int?(1);
			MATBinding.MeasureEvent(matEvent);
		}
		else if (GUI.Button(new Rect(10f, (float)(8 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Test Setter Methods"))
		{
			MonoBehaviour.print("Test Setter Methods clicked");
			MATBinding.SetAge(34);
			MATBinding.SetAllowDuplicates(true);
			MATBinding.SetAppAdTracking(true);
			MATBinding.SetDebugMode(true);
			MATBinding.SetExistingUser(false);
			MATBinding.SetFacebookUserId("temp_facebook_user_id");
			MATBinding.SetGender(0);
			MATBinding.SetGoogleUserId("temp_google_user_id");
			MATBinding.SetLocation(111.0, 222.0, 333.0);
			MATBinding.SetPayingUser(true);
			MATBinding.SetPhoneNumber("111-222-3456");
			MATBinding.SetTwitterUserId("twitter_user_id");
			MATBinding.SetUserId("temp_user_id");
			MATBinding.SetUserName("temp_user_name");
			MATBinding.SetUserEmail("tempuser@tempcompany.com");
			MATBinding.SetAndroidId("111111111111");
			MATBinding.SetDeviceId("123456789123456");
			MATBinding.SetGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
			MATBinding.SetMacAddress("AA:BB:CC:DD:EE:FF");
			MATBinding.SetCurrencyCode("CAD");
			MATBinding.SetTRUSTeId("1234567890");
			MATBinding.SetPreloadedApp(new MATPreloadData("1122334455")
			{
				advertiserSubAd = "some_adv_sub_ad_id",
				publisherSub3 = "some_pub_sub3"
			});
		}
		else if (GUI.Button(new Rect(10f, (float)(9 * Screen.height / 10), (float)(Screen.width - 20), (float)(Screen.height / 10)), "Test Getter Methods"))
		{
			MonoBehaviour.print("Test Getter Methods clicked");
			MonoBehaviour.print("isPayingUser = " + MATBinding.GetIsPayingUser());
			MonoBehaviour.print("matId     = " + MATBinding.GetMATId());
			MonoBehaviour.print("openLogId = " + MATBinding.GetOpenLogId());
		}
	}

	public static string getSampleiTunesIAPReceipt()
	{
		return "dGhpcyBpcyBhIHNhbXBsZSBpb3MgYXBwIHN0b3JlIHJlY2VpcHQ=";
	}
}
