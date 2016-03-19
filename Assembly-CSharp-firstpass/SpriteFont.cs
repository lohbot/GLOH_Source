using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SpriteFont
{
	protected delegate void ParserDel(string line);

	public TextAsset fontDef;

	protected Dictionary<int, int> charMap = new Dictionary<int, int>();

	protected SpriteChar[] chars;

	protected int lineHeight;

	protected int baseHeight;

	protected int texWidth;

	protected int texHeight;

	protected string face;

	protected int pxSize;

	protected float charSpacing = 1f;

	private int kerningsCount;

	public int LineHeight
	{
		get
		{
			return this.lineHeight;
		}
		set
		{
			this.lineHeight = value;
		}
	}

	public int BaseHeight
	{
		get
		{
			return this.baseHeight;
		}
	}

	public int PixelSize
	{
		get
		{
			return this.pxSize;
		}
	}

	public float CharacterSpacing
	{
		get
		{
			return this.charSpacing;
		}
		set
		{
			float num = this.charSpacing;
			this.charSpacing = value;
			if (num != this.charSpacing && this.chars != null)
			{
				for (int i = 0; i < this.chars.Length; i++)
				{
					SpriteChar[] expr_38_cp_0 = this.chars;
					int expr_38_cp_1 = i;
					expr_38_cp_0[expr_38_cp_1].xAdvance = expr_38_cp_0[expr_38_cp_1].xAdvance * this.charSpacing;
					if (this.chars[i].kernings != null)
					{
						foreach (char current in this.chars[i].kernings.Keys)
						{
							this.chars[i].kernings[current] = this.charSpacing * this.chars[i].origKernings[current];
						}
					}
				}
			}
		}
	}

	public SpriteFont(TextAsset def)
	{
		this.Load(def);
	}

	public void Load(TextAsset def)
	{
		if (def == null)
		{
			return;
		}
		Stream stream = new MemoryStream(def.bytes);
		BinaryReader binaryReader = new BinaryReader(stream);
		byte[] bytes = binaryReader.ReadBytes(4);
		if (Encoding.UTF8.GetString(bytes).Equals("BMF" + '\u0003'))
		{
			while (true)
			{
				try
				{
					char c = binaryReader.ReadChar();
					int blockSize = binaryReader.ReadInt32();
					switch (c)
					{
					case '\u0001':
						this.ReadInfoBlock(binaryReader, blockSize);
						break;
					case '\u0002':
						this.ReadCommonBlock(binaryReader, blockSize);
						break;
					case '\u0003':
						this.ReadPagesBlock(binaryReader, blockSize);
						break;
					case '\u0004':
						this.ReadCharsBlock(binaryReader, blockSize);
						break;
					case '\u0005':
						this.ReadKerningPairsBlock(binaryReader, blockSize);
						break;
					default:
						TsLog.Log("Unexpected block type " + (int)c, new object[0]);
						break;
					}
				}
				catch (EndOfStreamException ex)
				{
					TsLog.Log("EndOfStreamException : " + ex.ToString(), new object[0]);
					break;
				}
			}
		}
		else
		{
			int num = 0;
			this.fontDef = def;
			string[] array = this.fontDef.text.Split(new char[]
			{
				'\n'
			});
			int num2 = this.ParseSection("info", array, new SpriteFont.ParserDel(this.HeaderParser), 0);
			num2 = this.ParseSection("common", array, new SpriteFont.ParserDel(this.CommonParser), num2);
			num2 = this.ParseSection("chars count", array, new SpriteFont.ParserDel(this.CharCountParser), num2);
			while (num2 < array.Length && num < this.chars.Length)
			{
				if (this.CharParser(array[num2++], num))
				{
					num++;
				}
			}
			num2 = this.ParseSection("kernings count", array, new SpriteFont.ParserDel(this.KerningCountParser), num2);
			num = 0;
			while (num2 < array.Length && num < this.kerningsCount)
			{
				if (this.KerningParser(array[num2++]))
				{
					num++;
				}
			}
		}
		float characterSpacing = this.charSpacing;
		this.charSpacing = 0f;
		this.CharacterSpacing = characterSpacing;
		binaryReader.Close();
		stream.Close();
		stream.Dispose();
	}

	private void ReadInfoBlock(BinaryReader br, int blockSize)
	{
		this.pxSize = (int)br.ReadUInt16();
		br.ReadBytes(12);
		int count = blockSize - 14;
		this.face = Encoding.Default.GetString(br.ReadBytes(count));
	}

	private void ReadCommonBlock(BinaryReader br, int blockSize)
	{
		this.lineHeight = (int)br.ReadUInt16();
		this.baseHeight = (int)br.ReadUInt16();
		this.texWidth = (int)br.ReadUInt16();
		this.texHeight = (int)br.ReadUInt16();
		int num = (int)br.ReadUInt16();
		if (num > 1)
		{
			TsLog.LogError("Multiple pages/textures detected for font \"" + this.face + "\". only one font atlas is supported.", new object[0]);
		}
		int count = blockSize - 10;
		br.ReadBytes(count);
	}

	private void ReadPagesBlock(BinaryReader br, int blockSize)
	{
		br.ReadBytes(blockSize);
	}

	private void ReadCharsBlock(BinaryReader br, int blockSize)
	{
		int num = blockSize / 20;
		this.chars = new SpriteChar[num + 1];
		for (int i = 0; i < num; i++)
		{
			this.chars[i].id = (int)br.ReadUInt32();
			float num2 = (float)br.ReadUInt16() / (float)this.texWidth;
			float num3 = 1f - (float)br.ReadUInt16() / (float)this.texHeight;
			float num4 = (float)br.ReadUInt16() / (float)this.texWidth;
			float num5 = (float)br.ReadUInt16() / (float)this.texHeight;
			this.chars[i].xOffset = (float)br.ReadInt16();
			this.chars[i].yOffset = (float)(-(float)br.ReadInt16());
			this.chars[i].xAdvance = (float)br.ReadInt16();
			this.chars[i].UVs.x = num2;
			this.chars[i].UVs.y = num3 - num5;
			this.chars[i].UVs.xMax = num2 + num4;
			this.chars[i].UVs.yMax = num3;
			br.ReadBytes(2);
			this.charMap.Add(Convert.ToInt32(this.chars[i].id), i);
		}
	}

	private void ReadKerningPairsBlock(BinaryReader br, int blockSize)
	{
		int num = blockSize / 10;
		this.kerningsCount = num;
		for (int i = 0; i < num; i++)
		{
			int value = (int)br.ReadUInt32();
			int value2 = (int)br.ReadUInt32();
			int num2 = (int)br.ReadInt16();
			SpriteChar spriteChar = this.GetSpriteChar(Convert.ToChar(value2));
			if (spriteChar.kernings == null)
			{
				spriteChar.kernings = new Dictionary<char, float>();
			}
			spriteChar.origKernings = new Dictionary<char, float>();
			spriteChar.kernings.Add(Convert.ToChar(value), (float)num2);
			spriteChar.origKernings.Add(Convert.ToChar(value), (float)num2);
		}
	}

	private int ParseSection(string tag, string[] lines, SpriteFont.ParserDel parser, int pos)
	{
		while (pos < lines.Length)
		{
			string text = lines[pos].Trim();
			if (text.Length >= 1)
			{
				if (text.StartsWith(tag))
				{
					parser(text);
					return ++pos;
				}
			}
			pos++;
		}
		return pos;
	}

	private int FindField(string label, string[] fields, int pos, bool logError)
	{
		while (pos < fields.Length)
		{
			if (label == fields[pos])
			{
				return pos;
			}
			pos++;
		}
		if (logError)
		{
			return -1;
		}
		return -1;
	}

	private int FindField(string label, string[] fields, int pos)
	{
		return this.FindField(label, fields, pos, true);
	}

	private int FindFieldOptional(string label, string[] fields, int pos)
	{
		return this.FindField(label, fields, pos, false);
	}

	private void HeaderParser(string line)
	{
		string[] array = line.Split(new char[]
		{
			' ',
			'='
		});
		int num = this.FindField("face", array, 1);
		this.face = array[num + 1].Trim(new char[]
		{
			'"'
		});
		num = this.FindField("size", array, num);
		this.pxSize = Mathf.Abs(int.Parse(array[num + 1]));
		num = this.FindFieldOptional("charSpacing", array, num);
		if (num != -1)
		{
			this.charSpacing = Mathf.Abs(float.Parse(array[num + 1]));
		}
	}

	private void CommonParser(string line)
	{
		string[] array = line.Split(new char[]
		{
			' ',
			'='
		});
		int num = this.FindField("lineHeight", array, 1);
		this.lineHeight = int.Parse(array[num + 1]);
		num = this.FindField("base", array, num);
		this.baseHeight = (int)float.Parse(array[num + 1]);
		num = this.FindField("scaleW", array, num);
		this.texWidth = int.Parse(array[num + 1]);
		num = this.FindField("scaleH", array, num);
		this.texHeight = int.Parse(array[num + 1]);
		num = this.FindField("pages", array, num);
	}

	private void CharCountParser(string line)
	{
		string[] array = line.Split(new char[]
		{
			'='
		});
		if (array.Length < 2)
		{
			TsLog.LogError("Malformed \"chars count\" line in font definition file \"" + this.fontDef.name + "\". Please check the file or re-create it.", new object[0]);
			return;
		}
		this.chars = new SpriteChar[int.Parse(array[1]) + 1];
	}

	private bool CharParser(string line, int charNum)
	{
		if (!line.StartsWith("char"))
		{
			return false;
		}
		string[] array = line.Split(new char[]
		{
			' ',
			'='
		});
		int num = this.FindField("id", array, 1);
		this.chars[charNum].id = int.Parse(array[num + 1]);
		num = this.FindField("x", array, num);
		float num2 = float.Parse(array[num + 1]) / (float)this.texWidth;
		num = this.FindField("y", array, num);
		float num3 = 1f - float.Parse(array[num + 1]) / (float)this.texHeight;
		num = this.FindField("width", array, num);
		float num4 = float.Parse(array[num + 1]) / (float)this.texWidth;
		num = this.FindField("height", array, num);
		float num5 = float.Parse(array[num + 1]) / (float)this.texHeight;
		num = this.FindField("xoffset", array, num);
		this.chars[charNum].xOffset = float.Parse(array[num + 1]);
		num = this.FindField("yoffset", array, num);
		this.chars[charNum].yOffset = -float.Parse(array[num + 1]);
		num = this.FindField("xadvance", array, num);
		this.chars[charNum].xAdvance = (float)((int)float.Parse(array[num + 1]));
		num = this.FindField("page", array, 0);
		this.chars[charNum].page = int.Parse(array[num + 1]);
		num = this.FindField("chnl", array, 0);
		this.chars[charNum].chnl = int.Parse(array[num + 1]);
		num = this.FindField("layer", array, 0);
		if (-1 < num)
		{
			this.chars[charNum].layer = float.Parse(array[num + 1]);
		}
		else
		{
			this.chars[charNum].layer = 0f;
		}
		this.chars[charNum].UVs.x = num2;
		this.chars[charNum].UVs.y = num3 - num5;
		this.chars[charNum].UVs.xMax = num2 + num4;
		this.chars[charNum].UVs.yMax = num3;
		if (!this.charMap.ContainsKey(Convert.ToInt32(this.chars[charNum].id)))
		{
			this.charMap.Add(Convert.ToInt32(this.chars[charNum].id), charNum);
		}
		return true;
	}

	private void KerningCountParser(string line)
	{
		string[] array = line.Split(new char[]
		{
			'='
		});
		this.kerningsCount = int.Parse(array[1]);
	}

	private bool KerningParser(string line)
	{
		if (!line.StartsWith("kerning"))
		{
			return false;
		}
		string[] array = line.Split(new char[]
		{
			' ',
			'='
		});
		int num = this.FindField("first", array, 1);
		int value = int.Parse(array[num + 1]);
		num = this.FindField("second", array, num);
		int value2 = int.Parse(array[num + 1]);
		num = this.FindField("amount", array, num);
		int num2 = int.Parse(array[num + 1]);
		SpriteChar spriteChar = this.GetSpriteChar(Convert.ToChar(value2));
		if (spriteChar.kernings == null)
		{
			spriteChar.kernings = new Dictionary<char, float>();
		}
		spriteChar.origKernings = new Dictionary<char, float>();
		spriteChar.kernings.Add(Convert.ToChar(value), (float)num2);
		spriteChar.origKernings.Add(Convert.ToChar(value), (float)num2);
		return true;
	}

	public SpriteChar GetSpriteChar(char ch)
	{
		int num;
		if (!this.charMap.TryGetValue((int)ch, out num))
		{
			return default(SpriteChar);
		}
		return this.chars[num];
	}

	public bool ContainsCharacter(char ch)
	{
		return this.charMap.ContainsKey((int)ch);
	}

	public float GetWidth(string str)
	{
		if (str.Length < 1)
		{
			return 0f;
		}
		float num = this.GetSpriteChar(str[0]).xAdvance;
		for (int i = 1; i < str.Length; i++)
		{
			SpriteChar spriteChar = this.GetSpriteChar(str[i]);
			num += spriteChar.xAdvance + spriteChar.GetKerning(str[i - 1]);
		}
		return num;
	}

	public float GetWidth(string str, int start, int end)
	{
		if (start >= str.Length || end < start)
		{
			return 0f;
		}
		end = Mathf.Clamp(end, 0, str.Length - 1);
		float num = this.GetSpriteChar(str[start]).xAdvance;
		for (int i = start + 1; i <= end; i++)
		{
			SpriteChar spriteChar = this.GetSpriteChar(str[i]);
			num += spriteChar.xAdvance + spriteChar.GetKerning(str[i - 1]);
		}
		return num;
	}

	public float GetWidth(StringBuilder sb, int start, int end)
	{
		if (start >= sb.Length || end < start)
		{
			return 0f;
		}
		end = Mathf.Clamp(end, 0, sb.Length - 1);
		float num = this.GetSpriteChar(sb[start]).xAdvance;
		for (int i = start + 1; i <= end; i++)
		{
			SpriteChar spriteChar = this.GetSpriteChar(sb[i]);
			num += spriteChar.xAdvance + spriteChar.GetKerning(sb[i - 1]);
		}
		return num;
	}

	public float GetWidth(char prevChar, char c)
	{
		SpriteChar spriteChar = this.GetSpriteChar(c);
		return spriteChar.xAdvance + spriteChar.GetKerning(prevChar);
	}

	public string RemoveUnsupportedCharacters(string str)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '\0')
			{
				return stringBuilder.ToString();
			}
			if (this.charMap.ContainsKey((int)str[i]) || str[i] == '\n' || str[i] == '\t' || str[i] == '\r')
			{
				stringBuilder.Append(str[i]);
			}
		}
		return stringBuilder.ToString();
	}
}
