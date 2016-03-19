using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class FiveRocksEventData : NrTSingleton<FiveRocksEventData>
{
	public delegate void FunDelegate();

	private Dictionary<string, FiveRocksEventData.FunDelegate> mapFun = new Dictionary<string, FiveRocksEventData.FunDelegate>();

	private FiveRocksEventData()
	{
		this.mapFun.Add("shop_main", new FiveRocksEventData.FunDelegate(this.shop_main));
		this.mapFun.Add("shop_gold", new FiveRocksEventData.FunDelegate(this.shop_gold));
		this.mapFun.Add("shop_hearts", new FiveRocksEventData.FunDelegate(this.shop_hearts));
		this.mapFun.Add("shop_card", new FiveRocksEventData.FunDelegate(this.shop_card));
		this.mapFun.Add("shop_item", new FiveRocksEventData.FunDelegate(this.shop_item));
		this.mapFun.Add("shop_ori", new FiveRocksEventData.FunDelegate(this.shop_ori));
		this.mapFun.Add("user_card", new FiveRocksEventData.FunDelegate(this.user_card));
		this.mapFun.Add("post_open", new FiveRocksEventData.FunDelegate(this.post_open));
		this.mapFun.Add("colosseum", new FiveRocksEventData.FunDelegate(this.colosseum));
		this.mapFun.Add("coupon", new FiveRocksEventData.FunDelegate(this.coupon));
	}

	public void CheckEventData(string Key)
	{
		if (this.mapFun.ContainsKey(Key))
		{
			this.mapFun[Key]();
		}
		else if (Key.Contains("notice"))
		{
			string[] array = Key.Split(new char[]
			{
				'_'
			});
			if (array.Length > 0 && !string.IsNullOrEmpty(array[1]))
			{
				NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
				nrMobileNoticeWeb.OnNoticeEvent(array[1]);
			}
		}
	}

	private void shop_main()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_RECOMMEND, true);
	}

	private void shop_gold()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_GOLD, true);
	}

	private void shop_hearts()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HEARTS, true);
	}

	private void shop_card()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_HERO, true);
	}

	private void shop_item()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_ITEMBOX, true);
	}

	private void shop_ori()
	{
		NrTSingleton<ItemMallItemManager>.Instance.Send_GS_ITEMMALL_INFO_REQ(eITEMMALL_TYPE.BUY_ORI, true);
	}

	private void user_card()
	{
		GS_TICKET_SELL_INFO_REQ obj = new GS_TICKET_SELL_INFO_REQ();
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_TICKET_SELL_INFO_REQ, obj);
	}

	private void post_open()
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG);
	}

	private void colosseum()
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUMMAIN_DLG);
	}

	private void coupon()
	{
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COUPON_DLG);
	}
}
