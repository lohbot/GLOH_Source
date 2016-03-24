using System;

public class NIAPConstant
{
	public class InvokeMethod
	{
		public static readonly string init = "initIAP";

		public static readonly string getProductDetails = "getProductDetails";

		public static readonly string requestPayment = "requestPayment";

		public static readonly string requestConsume = "requestConsume";

		public static readonly string getPurchases = "getPurchases";

		public static readonly string getSinglePurchase = "getSinglePurchase";
	}

	public class Param
	{
		public static readonly string productCodes = "productCodes";

		public static readonly string productCode = "productCode";

		public static readonly string niapRequestCode = "niapRequestCode";

		public static readonly string payLoad = "payLoad";

		public static readonly string purchaseAsJsonText = "purchaseAsJsonText";

		public static readonly string signature = "signature";

		public static readonly string extraValue = "extraValue";

		public static readonly string publicKey = "publicKey";

		public static readonly string message = "message";

		public static readonly string code = "code";

		public static readonly string paymentSeq = "paymentSeq";

		public static readonly string purchaseToken = "purchaseToken";

		public static readonly string purchaseType = "purchaseType";

		public static readonly string environment = "environment";

		public static readonly string packageName = "packageName";

		public static readonly string appName = "appName";

		public static readonly string paymentTime = "paymentTime";

		public static readonly string developerPayload = "developerPayload";

		public static readonly string nonce = "nonce";

		public static readonly string originalPurchaseAsJsonText = "originalPurchaseAsJsonText";
	}

	public static readonly string invokeMethod = "invokeMethod";

	public static readonly string result = "result";

	public static readonly string error = "error";
}
