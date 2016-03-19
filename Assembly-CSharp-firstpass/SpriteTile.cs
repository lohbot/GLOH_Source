using System;
using UnityEngine;

[Serializable]
public class SpriteTile
{
	public enum SPRITE_TILE_MODE
	{
		STM_MIN,
		STM_1x1 = 0,
		STM_1x3,
		STM_3x1,
		STM_3x3,
		STM_MAX
	}

	public SpriteTile.SPRITE_TILE_MODE m_eTileMode;

	public float m_fTileWidth;

	public float m_fTileHeight;

	public Vector2[] m_v2UVs = new Vector2[1];

	public Vector3[] m_v3Vertices = new Vector3[1];

	public int[] m_siFaces = new int[1];

	public Color[] m_crColors = new Color[1];

	public float m_fWorldScale = 1f;

	private static int[] m_siTileCount = new int[]
	{
		1,
		3,
		3,
		9
	};

	private static float m_fFixedMiddleWidth = 2f;

	private static float m_fFixedMiddleHeight = 2f;

	public SpriteTile()
	{
		this.m_eTileMode = SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		this.m_fTileWidth = 0f;
		this.m_fTileHeight = 0f;
	}

	public void SetSpriteTile(SpriteTile.SPRITE_TILE_MODE eTileMode, float fTileWidth, float fTileHeight)
	{
		this.m_eTileMode = eTileMode;
		this.m_fTileWidth = fTileWidth;
		this.m_fTileHeight = fTileHeight;
	}

	public void DecideUV(Rect rc)
	{
		int num = SpriteTile.m_siTileCount[(int)this.m_eTileMode];
		this.MakeVariables();
		switch (this.m_eTileMode)
		{
		case SpriteTile.SPRITE_TILE_MODE.STM_MIN:
			this.m_v2UVs[0].x = rc.x;
			this.m_v2UVs[0].y = rc.yMax;
			this.m_v2UVs[1].x = rc.x;
			this.m_v2UVs[1].y = rc.y;
			this.m_v2UVs[2].x = rc.xMax;
			this.m_v2UVs[2].y = rc.y;
			this.m_v2UVs[3].x = rc.xMax;
			this.m_v2UVs[3].y = rc.yMax;
			break;
		case SpriteTile.SPRITE_TILE_MODE.STM_1x3:
		{
			float num2 = rc.height / this.m_fTileHeight * SpriteTile.m_fFixedMiddleHeight;
			float num3 = (rc.height - num2) / 2f;
			float[] array = new float[]
			{
				0f,
				num3,
				num3 + num2,
				rc.height
			};
			for (int i = 0; i < num; i++)
			{
				int num4 = 4 * i;
				int num5 = num - i;
				this.m_v2UVs[num4].x = rc.x;
				this.m_v2UVs[num4].y = rc.y + array[num5];
				this.m_v2UVs[num4 + 1].x = rc.x;
				this.m_v2UVs[num4 + 1].y = rc.y + array[num5 - 1];
				this.m_v2UVs[num4 + 2].x = rc.xMax;
				this.m_v2UVs[num4 + 2].y = rc.y + array[num5 - 1];
				this.m_v2UVs[num4 + 3].x = rc.xMax;
				this.m_v2UVs[num4 + 3].y = rc.y + array[num5];
			}
			break;
		}
		case SpriteTile.SPRITE_TILE_MODE.STM_3x1:
		{
			float num6 = rc.width / this.m_fTileWidth * SpriteTile.m_fFixedMiddleWidth;
			float num7 = (rc.width - num6) / 2f;
			float[] array2 = new float[]
			{
				0f,
				num7,
				num7 + num6,
				rc.width
			};
			for (int j = 0; j < num; j++)
			{
				int num4 = 4 * j;
				int num8 = j + 1;
				this.m_v2UVs[num4].x = rc.x + array2[j];
				this.m_v2UVs[num4].y = rc.yMax;
				this.m_v2UVs[num4 + 1].x = rc.x + array2[j];
				this.m_v2UVs[num4 + 1].y = rc.y;
				this.m_v2UVs[num4 + 2].x = rc.x + array2[num8];
				this.m_v2UVs[num4 + 2].y = rc.y;
				this.m_v2UVs[num4 + 3].x = rc.x + array2[num8];
				this.m_v2UVs[num4 + 3].y = rc.yMax;
			}
			break;
		}
		case SpriteTile.SPRITE_TILE_MODE.STM_3x3:
		{
			num = 3;
			float num2 = rc.height / this.m_fTileHeight * SpriteTile.m_fFixedMiddleHeight;
			float num3 = (rc.height - num2) / 2f;
			float[] array3 = new float[]
			{
				0f,
				num3,
				num3 + num2,
				rc.height
			};
			float num6 = rc.width / this.m_fTileWidth * SpriteTile.m_fFixedMiddleWidth;
			float num7 = (rc.width - num6) / 2f;
			float[] array4 = new float[]
			{
				0f,
				num7,
				num7 + num6,
				rc.width
			};
			for (int k = 0; k < num; k++)
			{
				int num5 = num - k;
				for (int l = 0; l < num; l++)
				{
					int num8 = l + 1;
					int num4 = 4 * k * num + 4 * l;
					this.m_v2UVs[num4].x = rc.x + array4[l];
					this.m_v2UVs[num4].y = rc.y + array3[num5];
					this.m_v2UVs[num4 + 1].x = rc.x + array4[l];
					this.m_v2UVs[num4 + 1].y = rc.y + array3[num5 - 1];
					this.m_v2UVs[num4 + 2].x = rc.x + array4[num8];
					this.m_v2UVs[num4 + 2].y = rc.y + array3[num5 - 1];
					this.m_v2UVs[num4 + 3].x = rc.x + array4[num8];
					this.m_v2UVs[num4 + 3].y = rc.y + array3[num5];
				}
			}
			break;
		}
		default:
			TsLog.LogError("--Calc UV Error--", new object[0]);
			return;
		}
	}

	public void DecideVertices(Vector3 offset, Vector3 topLeft, Vector3 bottomRight)
	{
		int num = SpriteTile.m_siTileCount[(int)this.m_eTileMode];
		this.MakeVariables();
		switch (this.m_eTileMode)
		{
		case SpriteTile.SPRITE_TILE_MODE.STM_MIN:
			this.m_v3Vertices[0].x = offset.x + topLeft.x;
			this.m_v3Vertices[0].y = offset.y + topLeft.y;
			this.m_v3Vertices[0].z = offset.z;
			this.m_v3Vertices[1].x = offset.x + topLeft.x;
			this.m_v3Vertices[1].y = offset.y + bottomRight.y;
			this.m_v3Vertices[1].z = offset.z;
			this.m_v3Vertices[2].x = offset.x + bottomRight.x;
			this.m_v3Vertices[2].y = offset.y + bottomRight.y;
			this.m_v3Vertices[2].z = offset.z;
			this.m_v3Vertices[3].x = offset.x + bottomRight.x;
			this.m_v3Vertices[3].y = offset.y + topLeft.y;
			this.m_v3Vertices[3].z = offset.z;
			break;
		case SpriteTile.SPRITE_TILE_MODE.STM_1x3:
		{
			float num2 = (bottomRight.y - topLeft.y) * this.m_fWorldScale;
			float num3 = (this.m_fTileHeight - SpriteTile.m_fFixedMiddleHeight) / 2f * this.m_fWorldScale * (float)((num2 <= 0f) ? -1 : 1);
			float[] array = new float[]
			{
				0f,
				num3,
				num2 - num3,
				num2
			};
			for (int i = 0; i < num; i++)
			{
				int num4 = 4 * i;
				this.m_v3Vertices[num4].x = offset.x + topLeft.x;
				this.m_v3Vertices[num4].y = offset.y + topLeft.y + array[i];
				this.m_v3Vertices[num4].z = offset.z;
				this.m_v3Vertices[num4 + 1].x = offset.x + topLeft.x;
				this.m_v3Vertices[num4 + 1].y = offset.y + topLeft.y + array[i + 1];
				this.m_v3Vertices[num4 + 1].z = offset.z;
				this.m_v3Vertices[num4 + 2].x = offset.x + bottomRight.x;
				this.m_v3Vertices[num4 + 2].y = offset.y + topLeft.y + array[i + 1];
				this.m_v3Vertices[num4 + 2].z = offset.z;
				this.m_v3Vertices[num4 + 3].x = offset.x + bottomRight.x;
				this.m_v3Vertices[num4 + 3].y = offset.y + topLeft.y + array[i];
				this.m_v3Vertices[num4 + 3].z = offset.z;
			}
			break;
		}
		case SpriteTile.SPRITE_TILE_MODE.STM_3x1:
		{
			float num5 = (bottomRight.x - topLeft.x) * this.m_fWorldScale;
			float num6 = (this.m_fTileWidth - SpriteTile.m_fFixedMiddleWidth) / 2f * this.m_fWorldScale * (float)((num5 <= 0f) ? -1 : 1);
			float[] array2 = new float[]
			{
				0f + topLeft.x,
				num6 + topLeft.x,
				num5 - num6 + topLeft.x,
				num5 + topLeft.x
			};
			for (int j = 0; j < num; j++)
			{
				int num4 = 4 * j;
				this.m_v3Vertices[num4].x = offset.x + array2[j];
				this.m_v3Vertices[num4].y = offset.y + topLeft.y;
				this.m_v3Vertices[num4].z = offset.z;
				this.m_v3Vertices[num4 + 1].x = offset.x + array2[j];
				this.m_v3Vertices[num4 + 1].y = offset.y + bottomRight.y;
				this.m_v3Vertices[num4 + 1].z = offset.z;
				this.m_v3Vertices[num4 + 2].x = offset.x + array2[j + 1];
				this.m_v3Vertices[num4 + 2].y = offset.y + bottomRight.y;
				this.m_v3Vertices[num4 + 2].z = offset.z;
				this.m_v3Vertices[num4 + 3].x = offset.x + array2[j + 1];
				this.m_v3Vertices[num4 + 3].y = offset.y + topLeft.y;
				this.m_v3Vertices[num4 + 3].z = offset.z;
			}
			break;
		}
		case SpriteTile.SPRITE_TILE_MODE.STM_3x3:
		{
			num = 3;
			float num7 = (bottomRight.y - topLeft.y) * this.m_fWorldScale;
			float num8 = (this.m_fTileHeight - SpriteTile.m_fFixedMiddleHeight) / 2f * this.m_fWorldScale * (float)((num7 <= 0f) ? -1 : 1);
			float[] array3 = new float[]
			{
				0f,
				num8,
				num7 - num8,
				num7
			};
			float num9 = (bottomRight.x - topLeft.x) * this.m_fWorldScale;
			float num10 = (this.m_fTileWidth - SpriteTile.m_fFixedMiddleWidth) / 2f * this.m_fWorldScale * (float)((num9 <= 0f) ? -1 : 1);
			float[] array4 = new float[]
			{
				0f + topLeft.x,
				num10 + topLeft.x,
				num9 - num10 + topLeft.x,
				num9 + topLeft.x
			};
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num; l++)
				{
					int num4 = 4 * k * num + 4 * l;
					this.m_v3Vertices[num4].x = offset.x + array4[l];
					this.m_v3Vertices[num4].y = offset.y + topLeft.y + array3[k];
					this.m_v3Vertices[num4].z = offset.z;
					this.m_v3Vertices[num4 + 1].x = offset.x + array4[l];
					this.m_v3Vertices[num4 + 1].y = offset.y + topLeft.y + array3[k + 1];
					this.m_v3Vertices[num4 + 1].z = offset.z;
					this.m_v3Vertices[num4 + 2].x = offset.x + array4[l + 1];
					this.m_v3Vertices[num4 + 2].y = offset.y + topLeft.y + array3[k + 1];
					this.m_v3Vertices[num4 + 2].z = offset.z;
					this.m_v3Vertices[num4 + 3].x = offset.x + array4[l + 1];
					this.m_v3Vertices[num4 + 3].y = offset.y + topLeft.y + array3[k];
					this.m_v3Vertices[num4 + 3].z = offset.z;
				}
			}
			break;
		}
		default:
			TsLog.LogError("--Calc Vertices Error--", new object[0]);
			return;
		}
	}

	public void DecideColors(Color crMeshColor)
	{
		this.MakeVariables();
		for (int i = 0; i < this.m_crColors.Length; i++)
		{
			this.m_crColors[i] = crMeshColor;
		}
	}

	public void MakeVariables()
	{
		int num = SpriteTile.m_siTileCount[(int)this.m_eTileMode];
		int num2 = 4 * num;
		int num3 = 6 * num;
		if (this.m_v3Vertices.Length != num2)
		{
			this.m_v3Vertices = new Vector3[num2];
		}
		if (this.m_v2UVs.Length != num2)
		{
			this.m_v2UVs = new Vector2[num2];
		}
		if (this.m_crColors.Length != num2)
		{
			this.m_crColors = new Color[num2];
		}
		if (this.m_siFaces.Length != num3)
		{
			this.m_siFaces = new int[num3];
			for (int i = 0; i < num; i++)
			{
				int num4 = i * 6;
				int num5 = i * 4;
				this.m_siFaces[num4] = num5;
				this.m_siFaces[num4 + 1] = num5 + 3;
				this.m_siFaces[num4 + 2] = num5 + 1;
				this.m_siFaces[num4 + 3] = num5 + 3;
				this.m_siFaces[num4 + 4] = num5 + 2;
				this.m_siFaces[num4 + 5] = num5 + 1;
			}
		}
	}

	public static SpriteTile.SPRITE_TILE_MODE ConvertMode(string szSpriteTileMode)
	{
		switch (int.Parse(szSpriteTileMode))
		{
		case 0:
			return SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		case 1:
			return SpriteTile.SPRITE_TILE_MODE.STM_1x3;
		case 2:
			return SpriteTile.SPRITE_TILE_MODE.STM_3x1;
		case 3:
			return SpriteTile.SPRITE_TILE_MODE.STM_3x3;
		default:
			return SpriteTile.SPRITE_TILE_MODE.STM_MIN;
		}
	}
}
