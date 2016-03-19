using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

[Serializable]
public abstract class EventTriggerItem : MonoBehaviour
{
	public List<AssetData> m_AssetDataList;

	public abstract string GetComment();

	public virtual bool IsVaildValue()
	{
		return true;
	}

	public virtual void GetAssetList(ref List<UnityEngine.Object> ObjectList)
	{
		if (this.m_AssetDataList == null)
		{
			this.m_AssetDataList = new List<AssetData>();
		}
		this.m_AssetDataList.Clear();
		FieldInfo[] fields = base.GetType().GetFields();
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			if (fieldInfo.FieldType.IsArray)
			{
				UnityEngine.Object[] array2 = fieldInfo.GetValue(this) as UnityEngine.Object[];
				if (array2 != null && array2.Length > 0)
				{
					GameObject[] array3 = array2 as GameObject[];
					Component[] array4 = array2 as Component[];
					if (array3 == null && array4 == null)
					{
						UnityEngine.Object[] array5 = array2;
						for (int j = 0; j < array5.Length; j++)
						{
							UnityEngine.Object @object = array5[j];
							if (!ObjectList.Contains(@object))
							{
								ObjectList.Add(@object);
							}
							AssetData assetData = new AssetData();
							assetData.m_ArraySize = array2.Length;
							assetData.m_ValueName = fieldInfo.Name;
							assetData.m_AssetPath = EventTriggerEditorUtil._OnGetAssetPath(@object);
							assetData.m_AssetPath = assetData.m_AssetPath.Substring("Assets/".Length, assetData.m_AssetPath.Length - "Assets/".Length);
							assetData.m_TypeName = @object.GetType().AssemblyQualifiedName;
							this.m_AssetDataList.Add(assetData);
						}
						fieldInfo.SetValue(this, null);
					}
				}
			}
			else
			{
				UnityEngine.Object object2 = fieldInfo.GetValue(this) as UnityEngine.Object;
				if (!(object2 == null))
				{
					GameObject x = object2 as GameObject;
					Component x2 = object2 as Component;
					if (!(x != null) && !(x2 != null))
					{
						if (!ObjectList.Contains(object2))
						{
							ObjectList.Add(object2);
						}
						AssetData assetData2 = new AssetData();
						assetData2.m_ArraySize = 0;
						assetData2.m_ValueName = fieldInfo.Name;
						assetData2.m_AssetPath = EventTriggerEditorUtil._OnGetAssetPath(object2);
						assetData2.m_AssetPath = assetData2.m_AssetPath.Substring("Assets/".Length, assetData2.m_AssetPath.Length - "Assets/".Length);
						assetData2.m_TypeName = object2.GetType().AssemblyQualifiedName;
						this.m_AssetDataList.Add(assetData2);
						fieldInfo.SetValue(this, null);
					}
				}
			}
		}
	}

	public virtual void SetAssetList(EventTriggerBundle BundleList)
	{
		if (this.m_AssetDataList == null)
		{
			return;
		}
		FieldInfo[] fields = base.GetType().GetFields();
		foreach (AssetData current in this.m_AssetDataList)
		{
			UnityEngine.Object @object = BundleList.GetObject(current.m_AssetPath);
			if (@object != null)
			{
				FieldInfo[] array = fields;
				for (int i = 0; i < array.Length; i++)
				{
					FieldInfo fieldInfo = array[i];
					if (fieldInfo.Name == current.m_ValueName)
					{
						if (fieldInfo.FieldType.IsArray)
						{
							Array array2 = fieldInfo.GetValue(this) as Array;
							if (array2 == null || array2.Length <= 0)
							{
								array2 = Array.CreateInstance(Type.GetType(current.m_TypeName), current.m_ArraySize);
								fieldInfo.SetValue(this, array2);
							}
							for (int j = 0; j < current.m_ArraySize; j++)
							{
								UnityEngine.Object x = (UnityEngine.Object)array2.GetValue(j);
								if (x == null)
								{
									array2.SetValue(@object, j);
									break;
								}
							}
						}
						else
						{
							fieldInfo.SetValue(this, @object);
						}
						break;
					}
				}
			}
		}
		this.m_AssetDataList.Clear();
	}

	public virtual bool FindObject(string objectname)
	{
		if (objectname == null)
		{
			return false;
		}
		FieldInfo[] fields = base.GetType().GetFields();
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			UnityEngine.Object @object = fieldInfo.GetValue(this) as UnityEngine.Object;
			if (!(@object == null))
			{
				if (@object.name.IndexOf(objectname) >= 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ChageValueByType(Type type, UnityEngine.Object value)
	{
		if (type == null)
		{
			return;
		}
		FieldInfo[] fields = base.GetType().GetFields();
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			if (fieldInfo.FieldType.AssemblyQualifiedName == type.AssemblyQualifiedName)
			{
				fieldInfo.SetValue(this, value);
			}
		}
	}

	public virtual void ChangeValue(UnityEngine.Object Org, UnityEngine.Object New)
	{
		if (Org == null || New == null)
		{
			return;
		}
		FieldInfo[] fields = base.GetType().GetFields();
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			UnityEngine.Object @object = fieldInfo.GetValue(this) as UnityEngine.Object;
			if (!(@object == null))
			{
				Component component = @object as Component;
				if (component != null)
				{
					GameObject gameObject = Org as GameObject;
					if (!(gameObject == null))
					{
						Component component2 = gameObject.GetComponent(component.GetType().Name);
						if (!(component2 == null))
						{
							if (@object.Equals(component2))
							{
								GameObject gameObject2 = New as GameObject;
								if (!(gameObject2 == null))
								{
									Component component3 = gameObject2.GetComponent(component.GetType().Name);
									if (!(component3 == null))
									{
										fieldInfo.SetValue(this, component3);
									}
								}
							}
						}
					}
				}
				else if (@object.Equals(Org))
				{
					fieldInfo.SetValue(this, New);
				}
			}
		}
	}

	public virtual void ReadXML(XmlReader Reader)
	{
		try
		{
			string attribute = Reader.GetAttribute("Count");
			for (int i = 0; i < int.Parse(attribute); i++)
			{
				while (Reader.Read())
				{
					if (Reader.NodeType == XmlNodeType.Element)
					{
						break;
					}
				}
				string name = Reader.Name;
				if (name.ToLower() == "Array".ToLower())
				{
					string attribute2 = Reader.GetAttribute("Name");
					string attribute3 = Reader.GetAttribute("Type");
					int num = int.Parse(Reader.GetAttribute("Size"));
					Type type = Type.GetType(attribute3);
					Array array = null;
					FieldInfo[] fields = base.GetType().GetFields();
					FieldInfo[] array2 = fields;
					for (int j = 0; j < array2.Length; j++)
					{
						FieldInfo fieldInfo = array2[j];
						if (fieldInfo.FieldType.IsArray)
						{
							if (!(fieldInfo.Name != attribute2))
							{
								array = (fieldInfo.GetValue(this) as Array);
								if (array == null || array.Length <= 0)
								{
									array = Array.CreateInstance(type, num);
									fieldInfo.SetValue(this, array);
								}
							}
						}
					}
					for (int k = 0; k < num; k++)
					{
						while (Reader.Read())
						{
							if (Reader.NodeType == XmlNodeType.Element)
							{
								break;
							}
						}
						string attribute4 = Reader.GetAttribute(0);
						array.SetValue(this.ParserString(type, attribute4), k);
					}
				}
				else
				{
					FieldInfo[] fields2 = base.GetType().GetFields();
					FieldInfo[] array3 = fields2;
					for (int l = 0; l < array3.Length; l++)
					{
						FieldInfo fieldInfo2 = array3[l];
						if (!(fieldInfo2.Name != name))
						{
							string attribute5 = Reader.GetAttribute(0);
							fieldInfo2.SetValue(this, this.ParserString(fieldInfo2.FieldType, attribute5));
							break;
						}
					}
				}
			}
			Reader.Read();
		}
		catch (Exception ex)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
			{
				TsLog.Log(ex.Message, new object[0]);
			}
		}
	}

	public virtual void WriteXML(XmlWriter Writer)
	{
		Writer.WriteStartElement("Item");
		Writer.WriteAttributeString("Name", base.GetType().Name);
		FieldInfo[] fields = base.GetType().GetFields();
		Writer.WriteStartElement("Value");
		Writer.WriteAttributeString("Count", (fields.Length - 1).ToString());
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
			if (fieldInfo != null)
			{
				if (fieldInfo.FieldType.IsArray)
				{
					if (fieldInfo.GetValue(this) is Array)
					{
						Writer.WriteStartElement("Array");
						Array array2 = fieldInfo.GetValue(this) as Array;
						string value = array2.GetType().FullName.Substring(0, array2.GetType().FullName.Length - "[]".Length);
						Writer.WriteAttributeString("Name", fieldInfo.Name);
						Writer.WriteAttributeString("Size", array2.Length.ToString());
						Writer.WriteAttributeString("Type", value);
						for (int j = 0; j < array2.Length; j++)
						{
							object value2 = array2.GetValue(j);
							string value3 = value2.ToString();
							Writer.WriteStartElement(fieldInfo.Name);
							Writer.WriteAttributeString("Value", value3);
							Writer.WriteEndElement();
						}
						Writer.WriteEndElement();
					}
				}
				else if (fieldInfo != null)
				{
					if (fieldInfo.GetValue(this) != null)
					{
						if (!(fieldInfo.GetValue(this) is ICollection))
						{
							string value4 = fieldInfo.GetValue(this).ToString();
							Writer.WriteStartElement(fieldInfo.Name);
							Writer.WriteAttributeString("Value", value4);
							Writer.WriteEndElement();
						}
					}
				}
			}
		}
		Writer.WriteEndElement();
		Writer.WriteEndElement();
	}

	public void ErrorLogPrint(EventTriggerItem Item, string Msg)
	{
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			TsLog.LogError(string.Concat(new string[]
			{
				"EventTrigger Error:",
				Item.gameObject.name,
				"(",
				Msg,
				")"
			}), new object[0]);
		}
	}

	private object ParserString(Type ValueType, string Value)
	{
		object result;
		if (ValueType == typeof(int))
		{
			result = int.Parse(Value);
		}
		else if (ValueType == typeof(bool))
		{
			result = bool.Parse(Value);
		}
		else if (ValueType == typeof(double))
		{
			result = double.Parse(Value);
		}
		else if (ValueType == typeof(float))
		{
			result = float.Parse(Value);
		}
		else if (ValueType == typeof(long))
		{
			result = long.Parse(Value);
		}
		else if (ValueType == typeof(uint))
		{
			result = uint.Parse(Value);
		}
		else if (ValueType == typeof(ulong))
		{
			result = ulong.Parse(Value);
		}
		else if (ValueType == typeof(ushort))
		{
			result = ushort.Parse(Value);
		}
		else
		{
			result = Value;
		}
		return result;
	}
}
