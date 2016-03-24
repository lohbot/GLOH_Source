using System;

public class GameGuidePurchaseRestore : GameGuideInfo
{
	public override void Init()
	{
		base.Init();
	}

	public override void ExcuteGameGuide()
	{
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
		{
			if (BillingManager_Google.Instance.IsRecoveryItem())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("666"));
				return;
			}
			BillingManager_Google component = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
			if (component != null)
			{
				component.StartRecoveryItem();
				NrTSingleton<GameGuideManager>.Instance.ExecuteGuide = true;
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
		{
			if (BillingManager_TStore.Instance.IsRecoveryItem())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("666"));
				return;
			}
			BillingManager_TStore component2 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
			if (component2 != null)
			{
				component2.SendRestoreItem();
				NrTSingleton<GameGuideManager>.Instance.ExecuteGuide = true;
			}
		}
		else if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
		{
			if (BillingManager_NStore.Instance.IsRecoveryItem())
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("666"));
				return;
			}
			BillingManager_NStore component3 = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
			if (component3 != null)
			{
				component3.StartRecoveryItem();
				NrTSingleton<GameGuideManager>.Instance.ExecuteGuide = true;
			}
		}
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuide()
	{
		return this.m_eCheck == GameGuideCheck.LOGIN && this.CheckGameGuideOnce();
	}

	public override bool CheckGameGuideOnce()
	{
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_Google)
		{
			BillingManager_Google component = BillingManager_Google.Instance.GetComponent<BillingManager_Google>();
			if (component != null && component.m_PurchaseList.Count > 0)
			{
				return true;
			}
		}
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_TStore)
		{
			BillingManager_TStore component2 = BillingManager_TStore.Instance.GetComponent<BillingManager_TStore>();
			if (component2 != null)
			{
				return component2.IsCheckRestoreItem();
			}
		}
		if (BillingManager.eBillingType == BillingManager.eBillingManager_Type.BillingManager_NStore)
		{
			BillingManager_NStore component3 = BillingManager_NStore.Instance.GetComponent<BillingManager_NStore>();
			if (component3 != null && component3.m_PurchaseList.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public override string GetGameGuideText()
	{
		return NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
	}
}
