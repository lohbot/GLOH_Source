using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FX_TEX_TiledAnimation : MonoBehaviour
{
	[Serializable]
	private struct _SpriteAniData
	{
		public float _accumTime;

		public float _lifeTime;

		public int _index;

		public bool _loop;

		public Vector2 _textureoffset;

		public void Init()
		{
			this._accumTime = Time.realtimeSinceStartup;
			this._lifeTime = Time.time;
			this._index = 0;
			this._loop = true;
			this._textureoffset = Vector2.zero;
		}
	}

	public int colCount;

	public int rowCount;

	public Vector2 offset = Vector2.zero;

	public bool loop = true;

	public bool apply;

	public float lifetime;

	public float initsec;

	public string options = "_MainTex";

	[SerializeField]
	private float _startTime;

	[SerializeField]
	private int _loopCount = 1;

	[SerializeField]
	private int _playedCount = 1;

	[SerializeField]
	private bool _disableClear;

	public List<_CellData> totalCells = new List<_CellData>();

	[SerializeField]
	private FX_TEX_TiledAnimation._SpriteAniData _spriteData;

	public float StartTime
	{
		get
		{
			return this._startTime;
		}
		set
		{
			this._startTime = value;
		}
	}

	public int LoopCount
	{
		get
		{
			return this._loopCount;
		}
		set
		{
			this._loopCount = value;
		}
	}

	public bool DisableClear
	{
		get
		{
			return this._disableClear;
		}
		set
		{
			this._disableClear = value;
		}
	}

	public int cellDataSize
	{
		get
		{
			return this.totalCells.Count;
		}
		set
		{
			if (value != this.totalCells.Count)
			{
				List<_CellData> list = new List<_CellData>(this.totalCells);
				if (value < this.totalCells.Count)
				{
					this.totalCells.CopyTo(list.ToArray(), 0);
					list.RemoveRange(value, this.totalCells.Count - value);
				}
				else if (value > this.totalCells.Count)
				{
					for (int i = this.totalCells.Count; i < value; i++)
					{
						list.Add(new _CellData());
					}
				}
				this.totalCells = list;
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		FX_TEX_TiledAnimation.<Start>c__Iterator6C <Start>c__Iterator6C = new FX_TEX_TiledAnimation.<Start>c__Iterator6C();
		<Start>c__Iterator6C.<>f__this = this;
		return <Start>c__Iterator6C;
	}

	private void OnDisable()
	{
		if (this.DisableClear)
		{
			this._spriteData.Init();
		}
	}

	private void Update()
	{
		this.SetSpriteAnimation();
	}

	public void SetSpriteAnimation()
	{
		if (!this._spriteData._loop)
		{
			return;
		}
		Vector2 scale = new Vector2(1f / (float)this.colCount, 1f / (float)this.rowCount);
		int num = this.totalCells[this._spriteData._index].textureNum % this.colCount;
		int num2 = this.totalCells[this._spriteData._index].textureNum / this.colCount;
		this._spriteData._textureoffset = new Vector2((float)num * scale.x, 1f - scale.y - (float)num2 * scale.y);
		base.renderer.material.SetTextureOffset(this.options.ToString(), this._spriteData._textureoffset);
		base.renderer.material.SetTextureScale(this.options.ToString(), scale);
		if (Time.realtimeSinceStartup - this._spriteData._accumTime < this.totalCells[this._spriteData._index].sec)
		{
			return;
		}
		this._spriteData._accumTime = Time.realtimeSinceStartup;
		this._spriteData._index = this._spriteData._index + 1;
		if (this._spriteData._index >= this.totalCells.Count)
		{
			if (this.LoopCount != -1)
			{
				if (this._playedCount < this.LoopCount)
				{
					this._spriteData._index = 0;
					this._playedCount++;
				}
				else
				{
					this._spriteData._loop = false;
				}
			}
			else
			{
				this._spriteData._index = 0;
			}
		}
	}

	public void SetCellDataInit(float sec)
	{
		if (this.totalCells.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (_CellData current in this.totalCells)
		{
			current.sec = sec;
			current.textureNum = num;
			num++;
		}
	}
}
