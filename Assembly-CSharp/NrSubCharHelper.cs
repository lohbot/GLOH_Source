using GAME;
using System;
using UnityEngine;

public class NrSubCharHelper
{
	public class NrSubCharEntity
	{
		private static short s_siCurrentSubCharUnique = 31005;

		private int m_siCharKind;

		private NrCharBase m_kChar;

		private string m_strStartChatText = string.Empty;

		public int SubCharKind
		{
			get
			{
				return this.m_siCharKind;
			}
			set
			{
				this.m_siCharKind = value;
			}
		}

		public NrCharBase SubChar
		{
			get
			{
				return this.m_kChar;
			}
			set
			{
				this.m_kChar = value;
			}
		}

		public string StartChatText
		{
			get
			{
				return this.m_strStartChatText;
			}
			set
			{
				this.m_strStartChatText = value;
			}
		}

		public NrSubCharEntity()
		{
			this.InitSubChar();
		}

		public void InitSubChar()
		{
			this.m_siCharKind = 0;
			this.m_kChar = null;
		}

		public void DeleteSubChar(bool bDeleteInfo)
		{
			if (this.m_kChar != null)
			{
				NrTSingleton<NkCharManager>.Instance.DeleteChar(this.m_kChar.GetID());
			}
			this.m_kChar = null;
			if (bDeleteInfo)
			{
				this.m_siCharKind = 0;
			}
		}

		public bool IsEmptySubChar()
		{
			return this.m_siCharKind == 0 && null == this.m_kChar;
		}

		private static short GetEmptySubCharUnique()
		{
			short result = 0;
			if (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique) == null)
			{
				result = NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique;
				NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique += 1;
			}
			else
			{
				short num = 31005;
				short num2 = 31200;
				short num3 = 195;
				for (short num4 = 0; num4 < num3; num4 += 1)
				{
					NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique += 1;
					if (num2 <= NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique)
					{
						NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique = num;
					}
					if (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique) == null)
					{
						result = NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique;
						NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique += 1;
						break;
					}
				}
			}
			if (31200 <= NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique)
			{
				NrSubCharHelper.NrSubCharEntity.s_siCurrentSubCharUnique = 31005;
			}
			return result;
		}

		private short _GetEmptySubCharUnique()
		{
			for (short num = 31005; num <= 31200; num += 1)
			{
				if (NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(num) == null)
				{
					return num;
				}
			}
			return 0;
		}

		public bool MakeSubChar(float fPosX, float fPosZ, float fDirX, float fDirZ)
		{
			NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_siCharKind);
			if (charKindInfo == null)
			{
				return false;
			}
			NEW_MAKECHAR_INFO nEW_MAKECHAR_INFO = new NEW_MAKECHAR_INFO();
			nEW_MAKECHAR_INFO.CharName = TKString.StringChar(charKindInfo.GetName());
			nEW_MAKECHAR_INFO.CharPos.x = fPosX;
			nEW_MAKECHAR_INFO.CharPos.y = 0f;
			nEW_MAKECHAR_INFO.CharPos.z = fPosZ;
			nEW_MAKECHAR_INFO.Direction.x = fDirX;
			nEW_MAKECHAR_INFO.Direction.y = 0f;
			nEW_MAKECHAR_INFO.Direction.z = fDirZ;
			nEW_MAKECHAR_INFO.CharKind = charKindInfo.GetCharKind();
			nEW_MAKECHAR_INFO.CharKindType = 3;
			nEW_MAKECHAR_INFO.CharUnique = NrSubCharHelper.NrSubCharEntity.GetEmptySubCharUnique();
			if (0 >= nEW_MAKECHAR_INFO.CharUnique)
			{
				return false;
			}
			int num = NrTSingleton<NkCharManager>.Instance.SetChar(nEW_MAKECHAR_INFO, false, true);
			if (0 > num)
			{
				return false;
			}
			this.m_kChar = NrTSingleton<NkCharManager>.Instance.GetChar(num);
			if (this.m_kChar == null)
			{
				return false;
			}
			this.m_kChar.m_bSubChar = true;
			return true;
		}

		public void Set(NrSubCharHelper.NrSubCharEntity kSubCharEntity)
		{
			this.m_siCharKind = kSubCharEntity.SubCharKind;
			this.m_kChar = kSubCharEntity.m_kChar;
		}
	}

	public static float s_fSubCharDistance = 9f;

	private NrSubCharHelper.NrSubCharEntity[] m_kSubCharList = new NrSubCharHelper.NrSubCharEntity[10];

	private bool m_bNeedMakeSubChar;

	private NrCharBase m_kParentChar;

	private float m_fFollowParentCheckTime;

	public bool IsMakeSubChar
	{
		get
		{
			return this.m_bNeedMakeSubChar;
		}
		set
		{
			this.m_bNeedMakeSubChar = value;
		}
	}

	public NrCharBase ParentChar
	{
		get
		{
			return this.m_kParentChar;
		}
		set
		{
			this.m_kParentChar = value;
		}
	}

	public NrSubCharHelper()
	{
		for (int i = 0; i < 10; i++)
		{
			this.m_kSubCharList[i] = new NrSubCharHelper.NrSubCharEntity();
		}
	}

	public void Init()
	{
		for (int i = 0; i < 10; i++)
		{
			this.m_kSubCharList[i].InitSubChar();
		}
		this.m_bNeedMakeSubChar = false;
		this.m_kParentChar = null;
		this.m_fFollowParentCheckTime = 0f;
	}

	public void DeleteSubCharAll()
	{
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			nrSubCharEntity.DeleteSubChar(true);
		}
	}

	public void DeleteSubCharAll3DObject()
	{
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			nrSubCharEntity.DeleteSubChar(false);
		}
	}

	public void DeleteSubChar(int siIndex)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return;
		}
		this.m_kSubCharList[siIndex].DeleteSubChar(true);
	}

	public NrCharBase GetSubChar(int siIndex)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return null;
		}
		return this.m_kSubCharList[siIndex].SubChar;
	}

	public NrCharBase GetSubCharByCharKind(int siCharKind)
	{
		if (0 >= siCharKind)
		{
			return null;
		}
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			if (siCharKind == nrSubCharEntity.SubCharKind)
			{
				return nrSubCharEntity.SubChar;
			}
		}
		return null;
	}

	public int GetSubCharKind(int siIndex)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return 0;
		}
		return this.m_kSubCharList[siIndex].SubCharKind;
	}

	public string GetStartChatText(int siCharUnique)
	{
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			if (nrSubCharEntity.SubChar != null && (int)nrSubCharEntity.SubChar.GetCharUnique() == siCharUnique)
			{
				return nrSubCharEntity.StartChatText;
			}
		}
		return string.Empty;
	}

	public NrSubCharHelper.NrSubCharEntity GetSubCharEntity(int siIndex)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return null;
		}
		return this.m_kSubCharList[siIndex];
	}

	public void MakeSubChar()
	{
		if (this.m_kParentChar == null)
		{
			return;
		}
		if (this.m_kParentChar.m_k3DChar == null)
		{
			return;
		}
		if (null == this.m_kParentChar.m_k3DChar.GetRootGameObject())
		{
			return;
		}
		int num = -1;
		Vector3 vector = default(Vector3);
		vector.x = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.x;
		vector.y = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.y;
		vector.z = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.z;
		Vector3 a = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.TransformDirection(Vector3.forward);
		float num2 = NrSubCharHelper.s_fSubCharDistance;
		Vector3 a2 = a * -num2;
		float num3 = Mathf.Atan2(a.x, a.z) * 180f / 3.14159274f;
		num3 = 180f - ((num3 <= 0f) ? (num3 + 360f) : num3);
		float x = vector.x;
		float z = vector.z;
		float f = 0.0174532924f * num3;
		float num4 = Mathf.Cos(f);
		float num5 = Mathf.Sin(f);
		Vector3 a3 = Vector3.zero;
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			if (0 < nrSubCharEntity.SubCharKind)
			{
				num++;
				if (0 < num)
				{
					if (num % 2 != 0)
					{
						vector.x += 2.5f;
					}
					else
					{
						vector.x = x;
						vector.z += 2.5f;
					}
				}
				if (nrSubCharEntity.SubChar == null)
				{
					float num6 = vector.x - x;
					float num7 = vector.z - z;
					a3.x = num4 * num6 + -num5 * num7 + x;
					a3.y = 0f;
					a3.z = num5 * num6 + num4 * num7 + z;
					a3 += a2 / 2f;
					if (!nrSubCharEntity.MakeSubChar(a3.x, a3.z, a.x, a.z))
					{
						return;
					}
				}
			}
		}
		this.m_bNeedMakeSubChar = false;
	}

	public void FollowParent()
	{
		if (Time.time < this.m_fFollowParentCheckTime)
		{
			return;
		}
		if (this.m_kParentChar == null)
		{
			return;
		}
		if (this.m_kParentChar.m_k3DChar == null)
		{
			return;
		}
		if (null == this.m_kParentChar.m_k3DChar.GetRootGameObject())
		{
			return;
		}
		this.m_fFollowParentCheckTime = Time.time + 0.5f;
		int num = -1;
		Vector3 zero = Vector3.zero;
		Vector3 vector = Vector3.zero;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		Vector3 vector2 = Vector3.zero;
		Vector3 a = Vector3.zero;
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			if (0 < nrSubCharEntity.SubCharKind && nrSubCharEntity.SubChar != null)
			{
				if (nrSubCharEntity.SubChar.m_k3DChar != null)
				{
					if (!(null == nrSubCharEntity.SubChar.m_k3DChar.GetRootGameObject()))
					{
						vector2 = nrSubCharEntity.SubChar.m_k3DChar.GetRootGameObject().transform.position;
						num++;
						float num6 = NrSubCharHelper.s_fSubCharDistance;
						if (num == 0)
						{
							zero.x = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.x;
							zero.y = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.y;
							zero.z = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.position.z;
							vector = this.m_kParentChar.m_k3DChar.GetRootGameObject().transform.TransformDirection(Vector3.forward);
							float num7 = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159274f;
							num7 = 180f - ((num7 <= 0f) ? (num7 + 360f) : num7);
							vector *= -num6;
							num2 = zero.x;
							num3 = zero.z;
							float f = 0.0174532924f * num7;
							num4 = Mathf.Cos(f);
							num5 = Mathf.Sin(f);
						}
						else if (num % 2 != 0)
						{
							zero.x += 5f;
						}
						else
						{
							zero.x = num2;
							zero.z += 5f;
						}
						float num8 = zero.x - num2;
						float num9 = zero.z - num3;
						a.x = num4 * num8 + -num5 * num9 + num2;
						a.y = 0f;
						a.z = num5 * num8 + num4 * num9 + num3;
						a += vector;
						if (a.x != vector2.x || a.z != vector2.z)
						{
							nrSubCharEntity.SubChar.SetSpeed(this.m_kParentChar.m_k3DChar.GetSpeed() * 1f);
							nrSubCharEntity.SubChar.MoveTo(a.x, a.y, a.z, false);
						}
					}
				}
			}
		}
	}

	public void SetSubCharKindFromList(int[] siCharKindList)
	{
		for (int i = 0; i < 10; i++)
		{
			this.SetSubCharKind(siCharKindList[i], i);
		}
	}

	public bool SetSubCharKind(int siCharKind)
	{
		return this.SetSubCharKind(siCharKind, 0);
	}

	public bool SetSubCharKind(int siCharKind, int siIndex)
	{
		return this.SetSubCharKind(siCharKind, siIndex, string.Empty);
	}

	public bool SetSubCharKind(int siCharKind, int siIndex, string strStartChatText)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return false;
		}
		if (!this.m_kSubCharList[siIndex].IsEmptySubChar())
		{
			return false;
		}
		this.m_kSubCharList[siIndex].SubCharKind = siCharKind;
		this.m_kSubCharList[siIndex].StartChatText = strStartChatText;
		this.m_bNeedMakeSubChar = true;
		return true;
	}

	public bool SetSubCharKind_EnptyIndex(int siCharKind)
	{
		NrSubCharHelper.NrSubCharEntity[] kSubCharList = this.m_kSubCharList;
		for (int i = 0; i < kSubCharList.Length; i++)
		{
			NrSubCharHelper.NrSubCharEntity nrSubCharEntity = kSubCharList[i];
			if (nrSubCharEntity.SubCharKind == 0)
			{
				nrSubCharEntity.SubCharKind = siCharKind;
				return true;
			}
		}
		return false;
	}

	public void Set(NrSubCharHelper kSubCharHelper)
	{
		this.Init();
		for (int i = 0; i < 10; i++)
		{
			this.m_kSubCharList[i].SubCharKind = kSubCharHelper.GetSubCharKind(i);
			this.m_kSubCharList[i].SubChar = null;
			if (0 < this.m_kSubCharList[i].SubCharKind)
			{
				this.m_bNeedMakeSubChar = true;
			}
		}
	}

	public void SetStartChatText(int siIndex, string strStartChatText)
	{
		if (0 > siIndex || 10 <= siIndex)
		{
			return;
		}
		this.m_kSubCharList[siIndex].StartChatText = strStartChatText;
	}

	public int GetSubCharCount()
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			if (0 < this.m_kSubCharList[i].SubCharKind)
			{
				num++;
			}
		}
		return num;
	}
}
