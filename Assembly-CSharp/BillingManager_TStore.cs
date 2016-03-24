using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Xml;
using UnityEngine;
using UnityForms;

public class BillingManager_TStore : BillingManager
{
	private AndroidJavaObject AndroidPlugin;

	private string _appid = string.Empty;

	private string m_ProductID = string.Empty;

	private string m_strTid = string.Empty;

	private bool m_bRecovery;

	private bool m_bRecoverytem;

	private static BillingManager_TStore s_instance;

	public static BillingManager_TStore Instance
	{
		get
		{
			if (BillingManager_TStore.s_instance == null)
			{
				GameObject gameObject = GameObject.Find("BillingManager_TStore");
				if (gameObject == null)
				{
					gameObject = new GameObject("BillingManager_TStore");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				BillingManager_TStore.s_instance = gameObject.GetComponent<BillingManager_TStore>();
				if (BillingManager_TStore.s_instance == null)
				{
					BillingManager_TStore.s_instance = gameObject.AddComponent<BillingManager_TStore>();
				}
			}
			return BillingManager_TStore.s_instance;
		}
	}

	private BillingManager_TStore()
	{
		AndroidJNI.AttachCurrentThread();
		this.Initiate();
	}

	private int Initiate()
	{
		this._appid = "OA00649101";
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		if (androidJavaClass == null)
		{
			return -1;
		}
		this.AndroidPlugin = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		androidJavaClass.Dispose();
		if (this.AndroidPlugin == null)
		{
			return -1;
		}
		return 0;
	}

	public void PurchaseComplete(string strParam)
	{
		TsLog.LogError("PurchaseComplete = {0}", new object[]
		{
			strParam
		});
		string[] array = strParam.Split(new char[]
		{
			'\n'
		});
		string text = string.Empty;
		if (!string.IsNullOrEmpty(this.Receipt.ToString()))
		{
			this.Receipt.Append("&");
		}
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text2 = array2[i];
			if (text2.Contains("product_id"))
			{
				text = text2.Substring("product_id:".Length);
				this.Receipt.Append("product_id=" + text);
				break;
			}
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			GS_BILLINGCHECK_REQ gS_BILLINGCHECK_REQ = new GS_BILLINGCHECK_REQ();
			gS_BILLINGCHECK_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(text);
			TKString.StringChar(this.m_strTid, ref gS_BILLINGCHECK_REQ.UniqueCode);
			TKString.StringChar(this.Receipt.ToString(), ref gS_BILLINGCHECK_REQ.Receipt);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLINGCHECK_REQ, gS_BILLINGCHECK_REQ);
			PlayerPrefs.SetString(NrPrefsKey.SHOP_PRODUCT_ID, gS_BILLINGCHECK_REQ.i64ItemMallIndex.ToString());
			PlayerPrefs.SetString(NrPrefsKey.SHOP_RECEIPT, this.Receipt.ToString());
			TsPlatform.FileLog(string.Format("TStorePurchaseComplete\nReceipt ={0} ProductID = {1} ItemMallIdex = {2}", this.Receipt, text, gS_BILLINGCHECK_REQ.i64ItemMallIndex));
			TsLog.LogWarning("Send BillingCheck ProductID = {0} ItemMallIdex = {1}", new object[]
			{
				text,
				gS_BILLINGCHECK_REQ.i64ItemMallIndex
			});
			this.Receipt.Remove(0, this.Receipt.Length);
		}
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(text);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
	}

	public void PurchasesignData(string strParam)
	{
		TsPlatform.FileLog(string.Format("PurchasesignData signData ={0} ", strParam));
		this.Receipt.Remove(0, this.Receipt.Length);
		TsLog.Log("{0}", new object[]
		{
			strParam
		});
		this.Receipt.Append("MarketType=tstore");
		string[] array = strParam.Split(new char[]
		{
			','
		});
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			string[] array3 = text.Split(new char[]
			{
				':'
			});
			Debug.Log("Param Count =" + array3.Length);
			if (!string.IsNullOrEmpty(this.Receipt.ToString()))
			{
				this.Receipt.Append("&");
			}
			if (array3[0].Contains("signdata"))
			{
				this.Receipt.Append(string.Format("signdata={0}", array3[1]));
			}
			if (array3[0].Contains("txid"))
			{
				this.Receipt.Append(string.Format("txid={0}", array3[1]));
			}
			if (array3[0].Contains("appid"))
			{
				this.Receipt.Append(string.Format("appid={0}", array3[1]));
			}
		}
	}

	public override void PurchaseItem(string strItem, int price)
	{
		NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = true;
		string @string = PlayerPrefs.GetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("664"));
			return;
		}
		this.m_ProductID = strItem;
		string text = string.Format("1{0:yyMMddHHmmss}_{1}", DateTime.Now, NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
		PlayerPrefs.SetString(NrPrefsKey.BUY_PRODUCT_ID, strItem);
		PlayerPrefs.SetString(NrPrefsKey.BUY_UNIQUE_CODE, text);
		PlayerPrefs.SetString(NrPrefsKey.BUY_DATE, string.Format("{0:yyyyMMdd}", DateTime.Now));
		PlayerPrefs.Save();
		int num = this.AndroidPlugin.Call<int>("PaymentRequest", new object[]
		{
			this._appid,
			strItem,
			text
		});
		if (num != 0)
		{
			Main_UI_SystemMessage.ADDMessage(string.Concat(new object[]
			{
				"PurchaseItem =",
				strItem,
				" retCode=",
				num
			}));
			NrTSingleton<NrMainSystem>.Instance.m_bIsBilling = false;
		}
		else
		{
			this.m_strTid = text;
		}
	}

	public void PurchaseFailed(string strMsg)
	{
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 1;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(this.m_ProductID);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, strMsg);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		this.EnableCloseItemMallDlg();
		this.CheckRestoreItem();
	}

	public void ClearData()
	{
		PlayerPrefs.SetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		PlayerPrefs.SetString(NrPrefsKey.BUY_UNIQUE_CODE, string.Empty);
		PlayerPrefs.SetString(NrPrefsKey.BUY_DATE, string.Empty);
		PlayerPrefs.Save();
		TsLog.LogError("ClearData!!!", new object[0]);
		this.m_bRecovery = false;
		this.m_bRecoverytem = false;
		this.m_bRecovery = false;
		if (NrTSingleton<GameGuideManager>.Instance.ExecuteGuide)
		{
			GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
			if (gameGuideDlg != null)
			{
				if (NrTSingleton<GameGuideManager>.Instance.ContinueCheck(GameGuideType.PURCHASE_RESTORE))
				{
					gameGuideDlg.SetTalkText();
				}
				else
				{
					gameGuideDlg.ClickClose(null);
				}
			}
		}
		else
		{
			NrTSingleton<GameGuideManager>.Instance.RemoveGuide(GameGuideType.PURCHASE_RESTORE);
		}
	}

	public void CheckRestoreItem()
	{
		string @string = PlayerPrefs.GetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		string string2 = PlayerPrefs.GetString(NrPrefsKey.BUY_UNIQUE_CODE, string.Empty);
		string string3 = PlayerPrefs.GetString(NrPrefsKey.BUY_DATE, string.Empty);
		if (string.IsNullOrEmpty(@string) || string.IsNullOrEmpty(string2) || string.IsNullOrEmpty(string3))
		{
			return;
		}
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 3;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
		NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, string.Format("Check strItem ={0} _tid={1} _Date={2}", @string, string2, string3));
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
		Debug.Log(string.Format("Check strItem ={0} _tid={1} _Date={2}", @string, string2, string3));
		this.AndroidPlugin.Call<int>("CheckItemTid", new object[]
		{
			this._appid,
			@string,
			string2,
			string3
		});
	}

	public void CheckResultData(string strParam)
	{
		string @string = PlayerPrefs.GetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		if (NrTSingleton<NkCharManager>.Instance.m_kCharAccountInfo.IsMaster())
		{
			Debug.Log(strParam);
		}
		string text = string.Empty;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(strParam);
		XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("result");
		if (elementsByTagName == null || elementsByTagName.Count <= 0)
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ.i8Type = 4;
			gS_BILLING_ITEM_RECODE_REQ.i8Result = 1;
			gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
			NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ, "elemList == null");
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
			this.ClearData();
			return;
		}
		bool flag = false;
		XmlNode xmlNode = elementsByTagName[0].SelectSingleNode("status");
		if (xmlNode == null || xmlNode.InnerText != "0")
		{
			flag = true;
		}
		xmlNode = elementsByTagName[0].SelectSingleNode("detail");
		if (flag)
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ2 = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ2.i8Type = 4;
			gS_BILLING_ITEM_RECODE_REQ2.i8Result = 2;
			gS_BILLING_ITEM_RECODE_REQ2.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
			if (xmlNode == null)
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ2, "Status Error Data== null");
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ2, "Status Error detail  =" + xmlNode.InnerText);
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ2);
			this.ClearData();
			return;
		}
		xmlNode = elementsByTagName[0].SelectSingleNode("appid");
		if (xmlNode == null || !xmlNode.InnerText.Contains(this._appid))
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ3 = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ3.i8Type = 4;
			gS_BILLING_ITEM_RECODE_REQ3.i8Result = 3;
			gS_BILLING_ITEM_RECODE_REQ3.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
			if (xmlNode == null)
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ3, "Data== null");
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ3, "appid Data.InnerText  =" + xmlNode.InnerText);
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ3);
			this.ClearData();
			return;
		}
		xmlNode = elementsByTagName[0].SelectSingleNode("billing_log").SelectSingleNode("item").SelectSingleNode("tid");
		text = PlayerPrefs.GetString(NrPrefsKey.BUY_UNIQUE_CODE, string.Empty);
		if (string.IsNullOrEmpty(text) || xmlNode == null || !xmlNode.InnerText.Contains(text))
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ4 = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ4.i8Type = 4;
			gS_BILLING_ITEM_RECODE_REQ4.i8Result = 4;
			gS_BILLING_ITEM_RECODE_REQ4.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
			if (xmlNode == null)
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ4, "Data== null");
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ4, "tid Data.InnerText  =" + xmlNode.InnerText + "Compare=" + text);
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ4);
			this.ClearData();
			return;
		}
		xmlNode = elementsByTagName[0].SelectSingleNode("billing_log").SelectSingleNode("item").SelectSingleNode("product_id");
		text = PlayerPrefs.GetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		if (string.IsNullOrEmpty(text) || xmlNode == null || !xmlNode.InnerText.Contains(text))
		{
			GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ5 = new GS_BILLING_ITEM_RECODE_REQ();
			gS_BILLING_ITEM_RECODE_REQ5.i8Type = 4;
			gS_BILLING_ITEM_RECODE_REQ5.i8Result = 5;
			gS_BILLING_ITEM_RECODE_REQ5.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
			if (xmlNode == null)
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ5, "Data== null");
			}
			else
			{
				NrTSingleton<ItemMallItemManager>.Instance.RecodeErrorMessage(ref gS_BILLING_ITEM_RECODE_REQ5, "product_id Data.InnerText  =" + xmlNode.InnerText + "Compare=" + text);
			}
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ5);
			this.ClearData();
			return;
		}
		this.m_bRecoverytem = true;
		NrTSingleton<GameGuideManager>.Instance.CheckGameGuide(GameGuideType.PURCHASE_RESTORE);
	}

	public void SendRestoreItem()
	{
		this.m_bRecovery = true;
		string @string = PlayerPrefs.GetString(NrPrefsKey.BUY_PRODUCT_ID, string.Empty);
		string string2 = PlayerPrefs.GetString(NrPrefsKey.BUY_UNIQUE_CODE, string.Empty);
		string string3 = PlayerPrefs.GetString(NrPrefsKey.BUY_DATE, string.Empty);
		if (string.IsNullOrEmpty(@string) || string.IsNullOrEmpty(string2) || string.IsNullOrEmpty(string3))
		{
			Debug.Log("Data NULL");
			this.ClearData();
			return;
		}
		GS_BILLINGITEM_CHECK gS_BILLINGITEM_CHECK = new GS_BILLINGITEM_CHECK();
		TKString.StringChar(string3, ref gS_BILLINGITEM_CHECK.Date);
		string string4 = PlayerPrefs.GetString(NrPrefsKey.BUY_UNIQUE_CODE, string.Empty);
		TKString.StringChar(string4, ref gS_BILLINGITEM_CHECK.UniqueCode);
		TKString.StringChar(@string, ref gS_BILLINGITEM_CHECK.ProductID);
		gS_BILLINGITEM_CHECK.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLINGITEM_CHECK, gS_BILLINGITEM_CHECK);
		GS_BILLING_ITEM_RECODE_REQ gS_BILLING_ITEM_RECODE_REQ = new GS_BILLING_ITEM_RECODE_REQ();
		gS_BILLING_ITEM_RECODE_REQ.i8Type = 2;
		gS_BILLING_ITEM_RECODE_REQ.i8Result = 0;
		gS_BILLING_ITEM_RECODE_REQ.i64ItemMallIndex = NrTSingleton<ItemMallItemManager>.Instance.GetItemIndex(@string);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BILLING_ITEM_RECODE_REQ, gS_BILLING_ITEM_RECODE_REQ);
	}

	public bool IsRecoveryItem()
	{
		return this.m_bRecovery;
	}

	public bool IsCheckRestoreItem()
	{
		return this.m_bRecoverytem;
	}

	public void EnableCloseItemMallDlg()
	{
		ItemMallDlg itemMallDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMMALL_DLG) as ItemMallDlg;
		if (itemMallDlg != null)
		{
			itemMallDlg.CloseEnable = true;
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.MSGBOX_DLG))
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.MSGBOX_DLG);
			}
		}
	}
}
