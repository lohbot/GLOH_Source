using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class TsMeshCharPortraitSetter : MonoBehaviour
{
	private NkUtil.REQUEST_CHAR_TEXTURE_INFO _requestCharPortraitInfo;

	public void SetTexture(string charCode)
	{
		int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(charCode);
		string costumePortraitInBattleSoldier = this.GetCostumePortraitInBattleSoldier(charKindByCode);
		this._requestCharPortraitInfo = NkUtil.RequestCharTexture(eCharImageType.LARGE, charKindByCode, -1, new NkUtil.RequestCharPortraitCallback(this.CharPortraitBundleCallback), costumePortraitInBattleSoldier);
		this.SetCharPortrait(this._requestCharPortraitInfo._tex2D);
	}

	public void CharPortraitBundleCallback(Texture2D charPortrait)
	{
		if (this == null)
		{
			return;
		}
		if (charPortrait == null)
		{
			return;
		}
		this.SetCharPortrait(charPortrait);
	}

	private void SetCharPortrait(Texture2D charPortrait)
	{
		if (charPortrait == null)
		{
			return;
		}
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		Material material = component.material;
		if (material == null)
		{
			return;
		}
		material.mainTexture = charPortrait;
	}

	private string GetCostumePortraitInBattleSoldier(int charkind)
	{
		if (Scene.CurScene != Scene.Type.BATTLE)
		{
			return string.Empty;
		}
		List<NkBattleChar> charListByKind = NrTSingleton<NkBattleCharManager>.Instance.GetCharListByKind(charkind);
		if (charListByKind == null)
		{
			return string.Empty;
		}
		foreach (NkBattleChar current in charListByKind)
		{
			if (current != null)
			{
				if (current.m_k3DChar != null)
				{
					string costumePortraitPath = current.m_k3DChar.GetCostumePortraitPath();
					if (!string.IsNullOrEmpty(costumePortraitPath))
					{
						return costumePortraitPath;
					}
				}
			}
		}
		return string.Empty;
	}
}
