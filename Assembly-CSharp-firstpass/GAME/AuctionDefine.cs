using System;

namespace GAME
{
	public static class AuctionDefine
	{
		public enum eAUCTIONREGISTERTYPE
		{
			eAUCTIONREGISTERTYPE_ITEM,
			eAUCTIONREGISTERTYPE_SOL,
			eAUCTIONREGISTERTYPE_ALL,
			eAUCTIONREGISTERTYPE_MAX
		}

		public enum eAUCTIONNOTIFY
		{
			eAUCTIONNOTIFY_NOUSE
		}

		public enum eAUCTIONCOMMODITYTYPE
		{
			eAUCTIONCOMMODITYTYPE_SUCCESSFULBID,
			eAUCTIONCOMMODITYTYPE_NOTSUCCESSFULBID,
			eAUCTIONCOMMODITYTYPE_CANCEL
		}

		public enum ePAYTYPE
		{
			ePAYTYPE_HEARTS,
			ePAYTYPE_GOLD,
			ePAYTYPE_ALL,
			ePAYTYPE_MAX
		}

		public enum eSORT_TYPE
		{
			eSORT_TYPE_NONE,
			eSORT_TYPE_OPTION_MIN,
			eSORT_TYPE_OPTION_MAX,
			eSORT_TYPE_COST_MIN,
			eSORT_TYPE_COST_MAX,
			eSORT_TYPE_DIRECTCOST_MIN,
			eSORT_TYPE_DIRECTCOST_MAX,
			eSORT_TYPE_REMAINTIME_MIN,
			eSORT_TYPE_REMAINTIME_MAX,
			eAUCTION_SORT_MAX
		}

		public const int AUCTION_PAGE_LIST_MAX_NUM = 4;

		public const int AUCTION_CONTROL_REFRESH_TIME = 600;

		public const int AUCTION_LISTITEM_MAX = 999;
	}
}