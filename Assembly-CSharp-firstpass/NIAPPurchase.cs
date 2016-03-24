using SimpleJSON;
using System;

public class NIAPPurchase
{
	private string paymentSeq;

	private string purchaseToken;

	private string purchaseType;

	private string environment;

	private string packageName;

	private string appName;

	private string productCode;

	private string paymentTime;

	private string developerPayload;

	private string nonce;

	private string signature;

	private string originalPurchaseAsJsonText;

	public NIAPPurchase(string paymentSeq, string purchaseToken, string purchaseType, string environment, string packageName, string appName, string productCode, string paymentTime, string developerPayload, string nonce, string signature, string originalPurchaseAsJsonText)
	{
		this.paymentSeq = paymentSeq;
		this.purchaseToken = purchaseToken;
		this.purchaseType = purchaseType;
		this.environment = environment;
		this.packageName = packageName;
		this.appName = appName;
		this.productCode = productCode;
		this.paymentTime = paymentTime;
		this.developerPayload = developerPayload;
		this.nonce = nonce;
		this.signature = signature;
		this.originalPurchaseAsJsonText = originalPurchaseAsJsonText;
	}

	public static NIAPPurchase Build(string resultString)
	{
		JSONNode jSONNode = JSON.Parse(resultString);
		string text = jSONNode[NIAPConstant.result];
		JSONNode jSONNode2 = JSON.Parse(text);
		string text2 = jSONNode2[NIAPConstant.Param.paymentSeq];
		string text3 = jSONNode2[NIAPConstant.Param.purchaseToken];
		string text4 = jSONNode2[NIAPConstant.Param.purchaseType];
		string text5 = jSONNode2[NIAPConstant.Param.environment];
		string text6 = jSONNode2[NIAPConstant.Param.packageName];
		string text7 = jSONNode2[NIAPConstant.Param.appName];
		string text8 = jSONNode2[NIAPConstant.Param.productCode];
		string text9 = jSONNode2[NIAPConstant.Param.paymentTime];
		string text10 = jSONNode2[NIAPConstant.Param.developerPayload];
		string text11 = jSONNode2[NIAPConstant.Param.nonce];
		string text12 = jSONNode[NIAPConstant.Param.signature];
		string text13 = text;
		return new NIAPPurchase(text2, text3, text4, text5, text6, text7, text8, text9, text10, text11, text12, text13);
	}

	public string getPaymentSeq()
	{
		return this.paymentSeq;
	}

	public string getPurchaseToken()
	{
		return this.purchaseToken;
	}

	public string getPurchaseType()
	{
		return this.purchaseType;
	}

	public string getEnvironment()
	{
		return this.environment;
	}

	public string getPackageName()
	{
		return this.packageName;
	}

	public string getAppName()
	{
		return this.appName;
	}

	public string getProductCode()
	{
		return this.productCode;
	}

	public string getPaymentTime()
	{
		return this.paymentTime;
	}

	public string getDeveloperPayload()
	{
		return this.developerPayload;
	}

	public string getNonce()
	{
		return this.nonce;
	}

	public string getSignature()
	{
		return this.signature;
	}

	public string getOriginalPurchaseAsJsonText()
	{
		return this.originalPurchaseAsJsonText;
	}
}
