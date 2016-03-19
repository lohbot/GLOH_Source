using System;
using UnityEngine;
using UnityForms;

[AddComponentMenu("Animation/CaptionEventFunction")]
public class NmCaptionEventFunction : MonoBehaviour
{
	public void SetCaption(string index)
	{
		if ("start" == index)
		{
			NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.CAPTION_DLG);
		}
		else if ("end" == index)
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CAPTION_DLG);
		}
		else
		{
			CaptionDlg captionDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CAPTION_DLG) as CaptionDlg;
			if (captionDlg != null)
			{
				captionDlg.SetTalkCation(index);
			}
		}
	}

	public void Caption01()
	{
	}

	public void Caption02()
	{
	}

	public void Caption03()
	{
	}

	public void Caption04()
	{
	}

	public void Caption05()
	{
	}

	public void Caption06()
	{
	}

	public void Caption07()
	{
	}

	public void Caption08()
	{
	}

	public void Caption09()
	{
	}

	public void Caption10()
	{
	}

	public void Caption11()
	{
	}

	public void Caption12()
	{
	}

	public void Caption13()
	{
	}

	public void Caption14()
	{
	}

	public void Caption15()
	{
	}

	public void Caption16()
	{
	}

	public void Caption17()
	{
	}

	public void Caption18()
	{
	}

	public void Caption19()
	{
	}

	public void Caption20()
	{
	}

	public void Caption21()
	{
	}

	public void Caption22()
	{
	}

	public void Caption23()
	{
	}

	public void Caption24()
	{
	}

	public void Caption25()
	{
	}

	public void Caption26()
	{
	}

	public void Caption27()
	{
	}

	public void Caption28()
	{
	}

	public void Caption29()
	{
	}

	public void Caption30()
	{
	}
}
