using System;
using UnityEngine;

namespace WellFired
{
	public static class USRuntimeUtility
	{
		public static bool CanPlaySequence(USSequencer sequence)
		{
			return !sequence.IsPlaying && sequence.RunningTime < sequence.Duration;
		}

		public static bool CanPauseSequence(USSequencer sequence)
		{
			return sequence.IsPlaying && sequence.RunningTime > 0f && sequence.RunningTime < sequence.Duration;
		}

		public static bool CanStopSequence(USSequencer sequence)
		{
			return sequence.RunningTime > 0f;
		}

		public static bool CanSkipSequence(USSequencer sequence)
		{
			return sequence.RunningTime < sequence.Duration;
		}

		public static bool IsObserverTimeline(Transform transform)
		{
			return transform.GetComponent<USTimelineObserver>() != null;
		}

		public static bool IsObserverContainer(Transform transform)
		{
			USTimelineContainer component = transform.GetComponent<USTimelineContainer>();
			if (component == null)
			{
				return false;
			}
			USTimelineBase[] timelines = component.Timelines;
			for (int i = 0; i < timelines.Length; i++)
			{
				USTimelineBase uSTimelineBase = timelines[i];
				if (USRuntimeUtility.IsObserverTimeline(uSTimelineBase.transform))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsTimelineContainer(Transform transform)
		{
			return transform.GetComponent<USTimelineContainer>() != null;
		}

		public static bool IsTimeline(Transform transform)
		{
			return transform.GetComponent<USTimelineBase>() != null;
		}

		public static bool IsEventTimeline(Transform transform)
		{
			return transform.GetComponent<USTimelineEvent>() != null;
		}

		public static bool IsPropertyTimeline(Transform transform)
		{
			return transform.GetComponent<USTimelineProperty>() != null;
		}

		public static bool IsEvent(Transform transform)
		{
			return transform.GetComponent<USEventBase>() != null;
		}

		public static int GetNumberOfTimelinesOfType(Type type, USTimelineContainer timelineContainer)
		{
			return timelineContainer.transform.GetComponentsInChildren(type).Length;
		}

		public static bool HasPropertyTimeline(Transform transform)
		{
			if (!USRuntimeUtility.IsTimelineContainer(transform))
			{
				return false;
			}
			Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Transform transform2 = componentsInChildren[i];
				if (USRuntimeUtility.IsPropertyTimeline(transform2))
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasObserverTimeline(Transform transform)
		{
			if (!USRuntimeUtility.IsTimelineContainer(transform))
			{
				return USRuntimeUtility.IsObserverTimeline(transform);
			}
			Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Transform transform2 = componentsInChildren[i];
				if (USRuntimeUtility.IsObserverTimeline(transform2))
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasTimelineContainerWithAffectedObject(USSequencer sequence, Transform affectedObject)
		{
			USTimelineContainer[] timelineContainers = sequence.TimelineContainers;
			for (int i = 0; i < timelineContainers.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = timelineContainers[i];
				if (uSTimelineContainer.AffectedObject == affectedObject)
				{
					return true;
				}
			}
			return false;
		}

		public static void CreateAndAttachObserver(USSequencer sequence)
		{
			USTimelineContainer[] timelineContainers = sequence.TimelineContainers;
			for (int i = 0; i < timelineContainers.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = timelineContainers[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					if (uSTimelineBase is USTimelineObserver)
					{
						return;
					}
				}
			}
			Camera x = null;
			Camera[] array = UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			Camera[] array2 = array;
			for (int k = 0; k < array2.Length; k++)
			{
				Camera camera = array2[k];
				if (camera.tag == "MainCamera")
				{
					x = camera;
				}
			}
			if (x == null)
			{
				Debug.LogWarning("There is no main camera in the scene, we need one for our observer track.");
			}
			GameObject gameObject = new GameObject("_Timelines for Observer");
			gameObject.transform.parent = sequence.transform;
			USTimelineContainer uSTimelineContainer2 = gameObject.AddComponent(typeof(USTimelineContainer)) as USTimelineContainer;
			uSTimelineContainer2.Index = -1;
			new GameObject("Observer")
			{
				transform = 
				{
					parent = gameObject.transform
				}
			}.AddComponent(typeof(USTimelineObserver));
		}

		public static void StartRecordingSequence(USSequencer sequence, string capturePath, int captureFramerate, int upScaleAmount)
		{
			USRecordSequence orSpawnRecorder = USRuntimeUtility.GetOrSpawnRecorder();
			orSpawnRecorder.StartRecording();
			orSpawnRecorder.RecordOnStart = true;
			orSpawnRecorder.CapturePath = capturePath;
			orSpawnRecorder.CaptureFrameRate = captureFramerate;
			orSpawnRecorder.UpscaleAmount = upScaleAmount;
		}

		public static void StopRecordingSequence()
		{
			USRecordSequence orSpawnRecorder = USRuntimeUtility.GetOrSpawnRecorder();
			if (orSpawnRecorder)
			{
				UnityEngine.Object.DestroyImmediate(orSpawnRecorder.gameObject);
			}
		}

		private static USRecordSequence GetOrSpawnRecorder()
		{
			USRecordSequence uSRecordSequence = UnityEngine.Object.FindObjectOfType(typeof(USRecordSequence)) as USRecordSequence;
			if (uSRecordSequence)
			{
				return uSRecordSequence;
			}
			GameObject gameObject = new GameObject("Recording Object");
			return gameObject.AddComponent<USRecordSequence>();
		}

		public static float FindNextKeyframeTime(USSequencer sequence)
		{
			float num = sequence.Duration;
			USTimelineContainer[] timelineContainers = sequence.TimelineContainers;
			for (int i = 0; i < timelineContainers.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = timelineContainers[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					USTimelineProperty uSTimelineProperty = uSTimelineBase as USTimelineProperty;
					if (uSTimelineProperty)
					{
						foreach (USPropertyInfo current in uSTimelineProperty.Properties)
						{
							foreach (USInternalCurve current2 in current.curves)
							{
								float num2 = current2.FindNextKeyframeTime(sequence.RunningTime);
								if (num2 < num)
								{
									num = num2;
								}
							}
						}
					}
				}
			}
			return num;
		}

		public static string ConvertToSerializableName(string value)
		{
			string result = string.Empty;
			if (value.IndexOf('.') != -1)
			{
				result = value.Remove(0, value.LastIndexOf('.') + 1);
			}
			else
			{
				result = value;
			}
			return result;
		}

		public static float FindPrevKeyframeTime(USSequencer sequence)
		{
			float num = 0f;
			USTimelineContainer[] timelineContainers = sequence.TimelineContainers;
			for (int i = 0; i < timelineContainers.Length; i++)
			{
				USTimelineContainer uSTimelineContainer = timelineContainers[i];
				USTimelineBase[] timelines = uSTimelineContainer.Timelines;
				for (int j = 0; j < timelines.Length; j++)
				{
					USTimelineBase uSTimelineBase = timelines[j];
					USTimelineProperty uSTimelineProperty = uSTimelineBase as USTimelineProperty;
					if (uSTimelineProperty)
					{
						foreach (USPropertyInfo current in uSTimelineProperty.Properties)
						{
							foreach (USInternalCurve current2 in current.curves)
							{
								float num2 = current2.FindPrevKeyframeTime(sequence.RunningTime);
								if (num2 > num)
								{
									num = num2;
								}
							}
						}
					}
				}
			}
			return num;
		}
	}
}
