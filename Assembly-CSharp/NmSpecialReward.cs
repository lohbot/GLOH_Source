using PROTOCOL;
using System;
using UnityEngine;

public class NmSpecialReward : MonoBehaviour
{
	private TsWeakReference<Battle_ResultDlg_Content> m_pkDlg;

	private int m_nIndex = -1;

	private bool m_bSelectCard;

	private float m_fBlockTime = 1f;

	private float m_fStartBlockTime;

	private void Start()
	{
		this.m_fStartBlockTime = Time.time;
	}

	public void SetData(Battle_ResultDlg_Content pkDlg, int nIndex)
	{
		if (pkDlg != null)
		{
			this.m_pkDlg = pkDlg;
			this.m_nIndex = nIndex;
			Animation component = base.gameObject.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				string animation = string.Format("card{0}_off", (this.m_nIndex + 1).ToString());
				component.Play(animation);
			}
		}
	}

	public bool CheckSelectCard()
	{
		return this.m_bSelectCard;
	}

	private void Update()
	{
	}

	private void OnMouseDown()
	{
	}

	private void OnMouseExit()
	{
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			if (component.isPlaying)
			{
				component.Stop();
			}
			string animation = string.Format("card{0}_off", (this.m_nIndex + 1).ToString());
			component.Play(animation);
		}
	}

	private void OnMouseEnter()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Began)
		{
			return;
		}
		if (this.m_fStartBlockTime + this.m_fBlockTime > Time.time)
		{
			return;
		}
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			if (component.isPlaying)
			{
				component.Stop();
			}
			NrSound.ImmedatePlay("UI_SFX", "BATTLE", "WIN-CARD-SELECT", true);
			string animation = string.Format("card{0}_on", (this.m_nIndex + 1).ToString());
			component.Play(animation);
		}
	}

	private void OnMouseUpAsButton()
	{
		if (NkInputManager.GetMouseButtonUp(0) && !this.m_bSelectCard)
		{
			if (SendPacket.GetInstance().IsBlockSendPacket())
			{
				return;
			}
			NrSound.ImmedatePlay("UI_SFX", "BATTLE", "WIN-CARD-SUCCESS", true);
			this.m_bSelectCard = true;
			int iSelectIndex = 0;
			if ("card1" == base.gameObject.name)
			{
				iSelectIndex = 0;
			}
			if ("card2" == base.gameObject.name)
			{
				iSelectIndex = 1;
			}
			if ("card3" == base.gameObject.name)
			{
				iSelectIndex = 2;
			}
			if ("card4" == base.gameObject.name)
			{
				iSelectIndex = 3;
			}
			if (this.m_pkDlg != null)
			{
				this.m_pkDlg.ClickRewardCardButton(iSelectIndex);
			}
		}
	}

	private void OnMouseOver()
	{
	}
}
