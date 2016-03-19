using System;
using UnityEngine;

public static class FontStore
{
	private static SpriteFont[] fonts = new SpriteFont[0];

	public static SpriteFont GetFont(TextAsset fontDef)
	{
		if (fontDef == null)
		{
			return null;
		}
		for (int i = 0; i < FontStore.fonts.Length; i++)
		{
			if (FontStore.fonts[i].fontDef == fontDef)
			{
				if (!Application.isPlaying)
				{
					FontStore.fonts[i] = new SpriteFont(fontDef);
				}
				return FontStore.fonts[i];
			}
		}
		SpriteFont spriteFont = new SpriteFont(fontDef);
		FontStore.AddFont(spriteFont);
		return spriteFont;
	}

	private static void AddFont(SpriteFont f)
	{
		SpriteFont[] array = new SpriteFont[FontStore.fonts.Length + 1];
		FontStore.fonts.CopyTo(array, 0);
		array[FontStore.fonts.Length] = f;
		FontStore.fonts = array;
	}
}
