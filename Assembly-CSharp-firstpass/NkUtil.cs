using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class NkUtil
{
	public static Color GetColor(int si32R, int si32G, int si32B)
	{
		return new Color((float)si32R / 255f, (float)si32G / 255f, (float)si32B / 255f);
	}

	public static Color GetColorA(int si32R, int si32G, int si32B, int si32A)
	{
		return new Color((float)si32R / 255f, (float)si32G / 255f, (float)si32B / 255f, (float)si32A / 255f);
	}

	public static Transform GetChild(Transform kTrans, string strName)
	{
		if (kTrans.name.Equals(strName))
		{
			return kTrans;
		}
		for (int i = 0; i < kTrans.childCount; i++)
		{
			Transform child = NkUtil.GetChild(kTrans.GetChild(i), strName);
			if (null != child)
			{
				return child;
			}
		}
		return null;
	}

	public static Transform GetChildContains(Transform kTrans, string strName)
	{
		if (kTrans.name.Contains(strName))
		{
			return kTrans;
		}
		for (int i = 0; i < kTrans.childCount; i++)
		{
			Transform childContains = NkUtil.GetChildContains(kTrans.GetChild(i), strName);
			if (null != childContains)
			{
				return childContains;
			}
		}
		return null;
	}

	public static Transform GetChildNoRecursive(Transform kTrans, string strName)
	{
		if (kTrans.name.Equals(strName))
		{
			return kTrans;
		}
		for (int i = 0; i < kTrans.childCount; i++)
		{
			Transform child = kTrans.GetChild(i);
			if (!(child == null))
			{
				if (child.name.Equals(strName))
				{
					return kTrans;
				}
			}
		}
		return null;
	}

	public static void SetAllChildActive(GameObject goTarget, bool bActive)
	{
		NkUtil.SetAllChildActive(goTarget, bActive, false);
	}

	public static void SetAllChildActive(GameObject goTarget, bool bActive, bool bExceptSelf)
	{
		if (null == goTarget)
		{
			return;
		}
		int childCount = goTarget.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = goTarget.transform.GetChild(i);
			if (!(child == null))
			{
				child.gameObject.SetActive(bActive);
			}
		}
		if (!bExceptSelf)
		{
			goTarget.SetActive(bActive);
		}
	}

	public static void SetAllChildActiveRecursive(GameObject goTarget, bool bActive)
	{
		if (null == goTarget)
		{
			return;
		}
		int childCount = goTarget.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = goTarget.transform.GetChild(i);
			if (!(child == null))
			{
				NkUtil.SetAllChildActive(child.gameObject, bActive);
			}
		}
		goTarget.SetActive(bActive);
	}

	public static void SetChildActiveExceptChar(GameObject goTarget, bool bActive)
	{
		if (null == goTarget)
		{
			return;
		}
		int childCount = goTarget.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = goTarget.transform.GetChild(i);
			if (!(child == null))
			{
				CharacterController component = child.GetComponent<CharacterController>();
				if (!(component != null))
				{
					child.gameObject.SetActive(bActive);
				}
			}
		}
		goTarget.SetActive(bActive);
	}

	public static void SetFindChildActive(GameObject parent, string strFindName, bool _isActive)
	{
		if (null != parent)
		{
			Transform child = NkUtil.GetChild(parent.transform, strFindName);
			if (child != null)
			{
				NkUtil.SetAllChildActive(child.gameObject, _isActive);
			}
		}
		else
		{
			Debug.LogWarning("KKIComment2011/6/2/ : SetFindChildActive:if( null != parent)");
		}
	}

	public static void SetShowHideRenderer(GameObject obj, bool bShow, bool bIncludeParent)
	{
		if (obj != null)
		{
			Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer renderer = array[i];
				if (!(null == renderer))
				{
					if (!renderer.name.Contains("fx_"))
					{
						if (renderer.enabled != bShow)
						{
							renderer.enabled = bShow;
						}
					}
				}
			}
			if (bIncludeParent)
			{
				Renderer[] components = obj.GetComponents<Renderer>();
				Renderer[] array2 = components;
				for (int j = 0; j < array2.Length; j++)
				{
					Renderer renderer2 = array2[j];
					if (!(null == renderer2))
					{
						if (!renderer2.name.Contains("fx_"))
						{
							if (renderer2.enabled != bShow)
							{
								renderer2.enabled = bShow;
							}
						}
					}
				}
			}
		}
	}

	public static void SetShowHideRenderer(GameObject obj, bool bShow, bool bIncludeParent, bool bParticleSystem)
	{
		if (obj != null)
		{
			Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer renderer = array[i];
				if (!(null == renderer))
				{
					if (!renderer.name.Contains("fx_"))
					{
						if (renderer.enabled != bShow)
						{
							renderer.enabled = bShow;
						}
					}
				}
			}
			ParticleSystem[] componentsInChildren2 = obj.GetComponentsInChildren<ParticleSystem>(true);
			ParticleSystem[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				ParticleSystem particleSystem = array2[j];
				if (!(null == particleSystem))
				{
					particleSystem.gameObject.SetActive(bShow);
				}
			}
			Light[] componentsInChildren3 = obj.GetComponentsInChildren<Light>(true);
			Light[] array3 = componentsInChildren3;
			for (int k = 0; k < array3.Length; k++)
			{
				Light light = array3[k];
				if (!(null == light))
				{
					light.gameObject.SetActive(bShow);
				}
			}
			if (bIncludeParent)
			{
				Renderer[] components = obj.GetComponents<Renderer>();
				Renderer[] array4 = components;
				for (int l = 0; l < array4.Length; l++)
				{
					Renderer renderer2 = array4[l];
					if (!(null == renderer2))
					{
						if (!renderer2.name.Contains("fx_"))
						{
							if (renderer2.enabled != bShow)
							{
								renderer2.enabled = bShow;
							}
						}
					}
				}
			}
		}
	}

	public static void RefreshAllChildPosition(Transform parent)
	{
		if (parent == null)
		{
			return;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			NkUtil.RefreshAllChildPosition(parent.GetChild(i));
		}
		parent.localPosition = parent.localPosition;
	}

	public static Vector3 GetHitPointFromVector2(Vector2 vPos)
	{
		Ray ray = new Ray(new Vector3(vPos.x, 200f, vPos.y), Vector3.down);
		int mask = 256;
		if (NkRaycast.Raycast(ray, float.PositiveInfinity, mask))
		{
			return NkRaycast.POINT;
		}
		return Vector3.zero;
	}

	public static float Distance2DFromVec3(Vector3 v1, Vector3 v2)
	{
		Vector3 vector = v1 - v2;
		return (float)Math.Sqrt((double)(vector.x * vector.x + vector.z * vector.z));
	}

	public static T GuarranteeComponent<T>(GameObject kGameObj) where T : Component
	{
		if (null == kGameObj.GetComponent<T>())
		{
			kGameObj.AddComponent<T>();
		}
		Component component = kGameObj.GetComponent<T>();
		return (T)((object)component);
	}

	public static void SetAllChildLayer(GameObject obj, int _layer)
	{
		if (null != obj)
		{
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				NkUtil.SetAllChildLayer(obj.transform.GetChild(i).gameObject, _layer);
			}
			obj.transform.gameObject.layer = _layer;
		}
		else
		{
			Debug.LogWarning("KKIComment2011/5/25/ : SetAllChildActive if (null == obj)");
		}
	}

	public static void SetAnimationCullingMode(GameObject gameObj, AnimationCullingType cullingType)
	{
		if (null != gameObj.animation)
		{
			gameObj.animation.cullingType = cullingType;
		}
		for (int i = 0; i < gameObj.transform.childCount; i++)
		{
			NkUtil.SetAnimationCullingMode(gameObj.transform.GetChild(i).gameObject, cullingType);
		}
	}

	public static void PlayAnimationInChildren(GameObject gameObj, string anikey)
	{
		if (gameObj != null)
		{
			Animation componentInChildren = gameObj.GetComponentInChildren<Animation>();
			if (componentInChildren != null && componentInChildren.GetClip(anikey.ToLower()) != null)
			{
				componentInChildren.Play(anikey.ToLower());
			}
		}
	}

	public static Vector3 stringToVector3(string str)
	{
		string[] array = str.Split(new char[]
		{
			','
		});
		if (array.Length != 3)
		{
			return Vector3.zero;
		}
		return new Vector3
		{
			x = (float)Convert.ToDouble(array[0]),
			y = (float)Convert.ToDouble(array[1]),
			z = (float)Convert.ToDouble(array[2])
		};
	}

	public static void RemoveAudioListener(GameObject go, bool bObject)
	{
		AudioListener[] componentsInChildren = go.GetComponentsInChildren<AudioListener>();
		AudioListener[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			AudioListener audioListener = array[i];
			if (audioListener != null)
			{
				Debug.LogWarning("AudioListener Exist~!! name = " + audioListener.name);
				UnityEngine.Object.DestroyImmediate(audioListener, bObject);
			}
		}
	}

	public static string MakeCode(int itemcode)
	{
		int num = 3;
		string text = "000" + itemcode.ToString();
		return text.Substring(text.Length - num, num);
	}

	public static string MakeCode(string itemcode)
	{
		int num = 3;
		string text = "000" + itemcode;
		return text.Substring(text.Length - num, num);
	}

	public static string GetBundleFilePath(string strAssetPath)
	{
		strAssetPath = strAssetPath.ToLower();
		strAssetPath = strAssetPath.Replace("assets", "BUNDLE");
		strAssetPath = strAssetPath.Replace("prefab/", string.Empty);
		if (strAssetPath.Contains("parts/"))
		{
			string[] array = strAssetPath.Split(new char[]
			{
				'\\',
				'/'
			});
			strAssetPath = strAssetPath.Replace(string.Format("{0}/{1}/", array[4], array[5]), string.Empty);
		}
		else if (strAssetPath.Contains("item/"))
		{
			string[] array2 = strAssetPath.Split(new char[]
			{
				'\\',
				'/'
			});
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].Contains("item"))
				{
					strAssetPath = strAssetPath.Replace(string.Format("{0}/", array2[i + 2]), string.Empty);
					break;
				}
			}
		}
		if (strAssetPath.Contains("hair/"))
		{
			strAssetPath = strAssetPath.Replace("hair/", "head/");
		}
		FileInfo fileInfo = new FileInfo(strAssetPath);
		strAssetPath = strAssetPath.Replace(fileInfo.Extension, ".assetbundle");
		if (!Directory.Exists(fileInfo.DirectoryName.ToLower()))
		{
			Directory.CreateDirectory(fileInfo.DirectoryName.ToLower());
		}
		return strAssetPath.ToLower();
	}

	public static UnityEngine.Object GetPrefab(GameObject kGameObj, string strName)
	{
		return null;
	}

	public static GameObject FindOrCreate(string strGameObjectName)
	{
		GameObject gameObject = GameObject.Find(strGameObjectName);
		if (null == gameObject)
		{
			gameObject = new GameObject(strGameObjectName);
		}
		return gameObject;
	}

	public static bool CheckUpGrounding(CharacterController charCtrl)
	{
		if (!charCtrl.enabled)
		{
			return false;
		}
		float num = charCtrl.height * 0.5f;
		Vector3 vector = new Vector3(0f, num, 0f);
		Vector3 position = charCtrl.transform.position;
		charCtrl.transform.position += vector;
		CollisionFlags collisionFlags = charCtrl.Move(-vector);
		float num2 = num;
		while (collisionFlags == CollisionFlags.None && num2 > 0f)
		{
			Transform transform = charCtrl.transform;
			transform.position += -transform.up * num2;
			num2 -= 0.1f;
			collisionFlags = charCtrl.Move(transform.up * num2);
		}
		if (collisionFlags == CollisionFlags.None)
		{
			Transform transform2 = charCtrl.transform;
			transform2.position = position;
			collisionFlags = charCtrl.Move(transform2.forward * num * 2f);
		}
		return collisionFlags != CollisionFlags.None;
	}

	public static string EncryptPassword(string password)
	{
		SHA1 sHA = SHA1.Create();
		byte[] array = sHA.ComputeHash(Encoding.Default.GetBytes(password));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static bool SetSHA1Hash(string sourceString, ref char[] hashData)
	{
		string text = NkUtil.EncryptPassword(sourceString);
		hashData = text.ToCharArray();
		return true;
	}

	public static int MakeLong(short low, long high)
	{
		return (int)((uint)((ushort)low) | (uint)((uint)high << 16));
	}

	public static short HighWord(int dword)
	{
		return (short)(dword >> 16);
	}

	public static short LowWord(int dword)
	{
		return (short)dword;
	}

	public static string AESEncrypt256(string Input, string key)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		rijndaelManaged.Key = Encoding.UTF8.GetBytes(key);
		rijndaelManaged.IV = new byte[16];
		ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
		byte[] inArray = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(Input);
				cryptoStream.Write(bytes, 0, bytes.Length);
			}
			inArray = memoryStream.ToArray();
		}
		return Convert.ToBase64String(inArray);
	}

	public static string AESDecrypt256(string Input, string key)
	{
		ICryptoTransform transform = new RijndaelManaged
		{
			KeySize = 256,
			BlockSize = 128,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7,
			Key = Encoding.UTF8.GetBytes(key),
			IV = new byte[16]
		}.CreateDecryptor();
		byte[] bytes = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
			{
				byte[] array = Convert.FromBase64String(Input);
				cryptoStream.Write(array, 0, array.Length);
			}
			bytes = memoryStream.ToArray();
		}
		return Encoding.UTF8.GetString(bytes);
	}

	public static string AESEncrypt128(string Input, string key)
	{
		return string.Empty;
	}

	public static string AESDecrypt128(string Input, string key)
	{
		return string.Empty;
	}

	public static string ConvertStrToByteBase64(string Input)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(Input);
		return Convert.ToBase64String(bytes);
	}
}
