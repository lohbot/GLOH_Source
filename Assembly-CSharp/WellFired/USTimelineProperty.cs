using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class USTimelineProperty : USTimelineBase
	{
		[SerializeField]
		private List<USPropertyInfo> propertyList = new List<USPropertyInfo>();

		public List<USPropertyInfo> Properties
		{
			get
			{
				return this.propertyList;
			}
		}

		public override void StartTimeline()
		{
			this.TryToFixComponentReferences();
		}

		public bool HasPropertiesForComponent(Component component)
		{
			foreach (USPropertyInfo current in this.propertyList)
			{
				if (component == current.Component)
				{
					return true;
				}
			}
			return false;
		}

		public USPropertyInfo GetProperty(string propertyName, Component component)
		{
			foreach (USPropertyInfo current in this.propertyList)
			{
				if (!(current == null))
				{
					string componentType = current.ComponentType;
					string b = USRuntimeUtility.ConvertToSerializableName(component.GetType().ToString());
					if (componentType == b && (current.propertyName == propertyName || current.fieldName == propertyName))
					{
						return current;
					}
				}
			}
			return null;
		}

		public bool ContainsProperty(string propertyName, Component component)
		{
			foreach (USPropertyInfo current in this.propertyList)
			{
				if (!(current == null))
				{
					if (current.Component == component && (current.propertyName == propertyName || current.fieldName == propertyName))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddProperty(USPropertyInfo propertyInfo)
		{
			if (propertyInfo.propertyName != null && this.ContainsProperty(propertyInfo.propertyName, propertyInfo.Component))
			{
				throw new Exception("Cannot Add a property that we already have");
			}
			if (propertyInfo.fieldName != null && this.ContainsProperty(propertyInfo.fieldName, propertyInfo.Component))
			{
				throw new Exception("Cannot Add a field that we already have");
			}
			this.propertyList.Add(propertyInfo);
		}

		public void RemoveProperty(USPropertyInfo propertyInfo)
		{
			this.propertyList.Remove(propertyInfo);
		}

		public void ClearProperties()
		{
			this.propertyList.Clear();
		}

		public override void SkipTimelineTo(float time)
		{
			this.Process(time, 1f);
		}

		public override void Process(float sequencerTime, float playbackRate)
		{
			if (!base.AffectedObject)
			{
				return;
			}
			for (int i = 0; i < this.propertyList.Count; i++)
			{
				USPropertyInfo uSPropertyInfo = this.propertyList[i];
				if (uSPropertyInfo != null)
				{
					if (!uSPropertyInfo.Component)
					{
						uSPropertyInfo.Component = base.AffectedObject.GetComponent(uSPropertyInfo.ComponentType);
					}
					if (uSPropertyInfo.Component && uSPropertyInfo.Component.transform != base.AffectedObject)
					{
						uSPropertyInfo.Component = base.AffectedObject.GetComponent(uSPropertyInfo.ComponentType);
					}
					uSPropertyInfo.SetValue(sequencerTime);
				}
			}
		}

		public void TryToFixComponentReferences()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			foreach (USPropertyInfo current in this.propertyList)
			{
				if (current != null)
				{
					if (!current.Component)
					{
						current.Component = base.AffectedObject.GetComponent(current.ComponentType);
					}
					if (current.Component && current.Component.transform != base.AffectedObject)
					{
						current.Component = base.AffectedObject.GetComponent(current.ComponentType);
					}
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (!base.ShouldRenderGizmos)
			{
				return;
			}
			foreach (USPropertyInfo current in this.Properties)
			{
				if (!(current == null) && !(current.Component == null) && current.Component.GetType() == typeof(Transform))
				{
					if (!(current.PropertyName != "localPosition") || !(current.PropertyName != "position"))
					{
						float num = float.PositiveInfinity;
						float num2 = 0f;
						foreach (USInternalCurve current2 in current.curves)
						{
							num = Math.Min(num, current2.FirstKeyframeTime);
							num2 = Math.Max(num2, current2.LastKeyframeTime);
						}
						float num3 = (num2 - num) / 40f;
						float num4 = num;
						while (num4 < num2)
						{
							Vector3 vector = (Vector3)current.GetValueForTime(num4);
							Vector3 vector2 = (Vector3)current.GetValueForTime(num4 + num3);
							num4 += num3;
							Transform parent = base.AffectedObject.transform.parent;
							if (current.PropertyName == "localPosition" && parent != null)
							{
								vector = parent.TransformPoint(vector);
								vector2 = parent.TransformPoint(vector2);
							}
							Gizmos.DrawLine(vector, vector2);
						}
					}
				}
			}
		}

		public override void LateBindAffectedObjectInScene(Transform newAffectedObject)
		{
			foreach (USPropertyInfo current in this.propertyList)
			{
				if (current != null && current.Component == null)
				{
					current.Component = base.AffectedObject.GetComponent(current.ComponentType);
				}
			}
			base.LateBindAffectedObjectInScene(newAffectedObject);
		}

		public override string GetJson()
		{
			string text = string.Empty;
			foreach (USPropertyInfo current in this.Properties)
			{
				text = string.Format("{0},{1}", text, current.GetJSON());
			}
			if (text.IndexOf(',') == 0)
			{
				text = text.Substring(1, text.Length - 1);
			}
			text = string.Format("[{0}]", text);
			return text;
		}
	}
}
