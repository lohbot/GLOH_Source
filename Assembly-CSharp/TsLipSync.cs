using System;
using System.Collections.Generic;
using Ts;
using UnityEngine;

public class TsLipSync : MonoBehaviour
{
	private float elpTime;

	private static Dictionary<string, string> phonemeToViseme = new Dictionary<string, string>
	{
		{
			"_",
			"Base"
		},
		{
			"m",
			"Base"
		},
		{
			"b",
			"Base"
		},
		{
			"p",
			"Base"
		},
		{
			"x",
			"Base"
		},
		{
			"f",
			"Base"
		},
		{
			"v",
			"Base"
		},
		{
			"a",
			"A"
		},
		{
			"aa",
			"A"
		},
		{
			"ax",
			"A"
		},
		{
			"ih",
			"A"
		},
		{
			"ae",
			"A"
		},
		{
			"ah",
			"A"
		},
		{
			"ey",
			"A"
		},
		{
			"ay",
			"A"
		},
		{
			"h",
			"A"
		},
		{
			"n",
			"I"
		},
		{
			"ng",
			"I"
		},
		{
			"ch",
			"I"
		},
		{
			"j",
			"I"
		},
		{
			"dh",
			"I"
		},
		{
			"d",
			"I"
		},
		{
			"g",
			"I"
		},
		{
			"t",
			"I"
		},
		{
			"k",
			"I"
		},
		{
			"z",
			"I"
		},
		{
			"zh",
			"I"
		},
		{
			"th",
			"I"
		},
		{
			"s",
			"I"
		},
		{
			"sh",
			"I"
		},
		{
			"w",
			"U"
		},
		{
			"uw",
			"U"
		},
		{
			"e",
			"E"
		},
		{
			"iy",
			"E"
		},
		{
			"eh",
			"E"
		},
		{
			"y",
			"E"
		},
		{
			"l",
			"E"
		},
		{
			"el",
			"E"
		},
		{
			"aw",
			"O"
		},
		{
			"ow",
			"O"
		},
		{
			"ao",
			"O"
		},
		{
			"oy",
			"O"
		}
	};

	private List<KeyValuePair<string, float>> playVisemes = new List<KeyValuePair<string, float>>();

	private int visemePos;

	private AnimationState curVisAnim;

	private AnimationState preVisAnim;

	public void Speak(string syncFile)
	{
		this.elpTime = 0f;
		this.visemePos = 0;
		string[] array = syncFile.Split(new char[]
		{
			'\n'
		});
		for (int i = 1; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				'\t'
			});
			if (array2.Length == 2)
			{
				float num = float.Parse(array2[1]) / 1000f;
				this.playVisemes.Add(new KeyValuePair<string, float>(array2[0], num - 0.01f));
			}
		}
		if (base.animation["AutoBase"] != null)
		{
			Debug.LogWarning("already has LipAnim :" + base.name);
		}
		else
		{
			Debug.LogWarning("TsLipSync.Parse for :" + base.name);
		}
		this.AddViseme("A");
		this.AddViseme("I");
		this.AddViseme("U");
		this.AddViseme("E");
		this.AddViseme("O");
		if (base.animation["AutoBase"] == null)
		{
			using (new ScopeProfile("TsLipSync.Speak"))
			{
				AnimationClip animationClip = new AnimationClip();
				this.MakeClip("bone01 pelvis/bone01 spine/bone01 spine1/bone01 neck/bone01 head/bone01 facial lip", animationClip);
				this.MakeClip("bone01 pelvis/bone01 spine/bone01 spine1/bone01 neck/bone01 head/bone01 facial jaw", animationClip);
				this.MakeClip("bone01 pelvis/bone01 spine/bone01 spine1/bone01 neck/bone01 head/bone01 facial mouth l", animationClip);
				this.MakeClip("bone01 pelvis/bone01 spine/bone01 spine1/bone01 neck/bone01 head/bone01 facial mouth r", animationClip);
				base.animation.AddClip(animationClip, "AutoBase");
			}
		}
		AnimationState animationState = base.animation["AutoBase"];
		animationState.layer = 19;
		animationState.weight = 1f;
		animationState.wrapMode = WrapMode.Loop;
		animationState.enabled = true;
	}

	public void Update()
	{
		this.elpTime += Time.deltaTime;
		if (this.visemePos == -1)
		{
			return;
		}
		if (this.elpTime >= this.playVisemes[this.visemePos].Value)
		{
			string text;
			if (TsLipSync.phonemeToViseme.TryGetValue(this.playVisemes[this.visemePos].Key, out text))
			{
				this.SetFrame(text);
				Debug.Log(string.Concat(new object[]
				{
					"Play anim T:",
					this.playVisemes[this.visemePos].Value,
					" Vis:",
					this.playVisemes[this.visemePos].Key,
					"-",
					text
				}));
			}
			else
			{
				this.SetFrame("Base");
			}
			this.visemePos++;
			if (this.visemePos == this.playVisemes.Count)
			{
				this.visemePos = -1;
				this.Done();
				return;
			}
		}
		Debug.Log(string.Concat(new object[]
		{
			"AutoBase : ",
			base.animation["AutoBase"].weight,
			"\nA : ",
			base.animation["A"].weight,
			"\nI : ",
			base.animation["I"].weight,
			"\nU : ",
			base.animation["U"].weight,
			"\nE : ",
			base.animation["E"].weight,
			"\nO : ",
			base.animation["O"].weight,
			"\n"
		}));
		if (base.animation["AutoBase"].weight < 1f)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"AutoBase Anim Weight Error : ",
				base.animation["AutoBase"].weight,
				" at ",
				base.name
			}));
		}
		if (this.curVisAnim != null && this.curVisAnim.weight != 1f)
		{
			this.curVisAnim.weight = Mathf.Min(1f, this.curVisAnim.weight + 0.2f);
		}
		if (this.preVisAnim != null && this.preVisAnim.weight != 0f)
		{
			this.preVisAnim.weight = Mathf.Max(0f, this.preVisAnim.weight - 0.2f);
		}
	}

	public void Done()
	{
		Debug.LogWarning("TsLipSync.Done " + base.name);
		this.DisableAnim("A");
		this.DisableAnim("I");
		this.DisableAnim("U");
		this.DisableAnim("E");
		this.DisableAnim("O");
		base.animation.Sample();
		base.animation["AutoBase"].enabled = false;
		UnityEngine.Object.Destroy(this);
	}

	private void DisableAnim(string animName)
	{
		AnimationState animationState = base.animation[animName];
		if (animationState != null)
		{
			animationState.weight = 0f;
			animationState.enabled = false;
		}
	}

	private AnimationClip MakeClip(string path, AnimationClip sourceClip)
	{
		Transform transform = base.transform.Find(path);
		if (transform == null)
		{
			Debug.LogError("target not found:" + path + " on GO:" + base.name);
		}
		sourceClip.SetCurve(path, typeof(Transform), "localPosition.x", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localPosition.x, 0f, 0f),
			new Keyframe(0.033f, transform.localPosition.x, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localPosition.y", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localPosition.y, 0f, 0f),
			new Keyframe(0.033f, transform.localPosition.y, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localPosition.z", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localPosition.z, 0f, 0f),
			new Keyframe(0.033f, transform.localPosition.z, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localRotation.x", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localRotation.x, 0f, 0f),
			new Keyframe(0.033f, transform.localRotation.x, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localRotation.y", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localRotation.y, 0f, 0f),
			new Keyframe(0.033f, transform.localRotation.y, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localRotation.z", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localRotation.z, 0f, 0f),
			new Keyframe(0.033f, transform.localRotation.z, 0f, 0f)
		}));
		sourceClip.SetCurve(path, typeof(Transform), "localRotation.w", new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, transform.localRotation.w, 0f, 0f),
			new Keyframe(0.033f, transform.localRotation.w, 0f, 0f)
		}));
		return sourceClip;
	}

	private void AddViseme(string animName)
	{
		if (base.animation[animName] == null)
		{
			AnimationClip animationClip = (AnimationClip)Resources.Load("viseme/" + animName, typeof(AnimationClip));
			if (animationClip == null)
			{
				Debug.LogError("Anim not found :" + animName);
			}
			EventTriggerHelper.AddAniClip(base.gameObject, animationClip);
		}
		AnimationState animationState = base.animation[animName];
		animationState.layer = 20;
		animationState.blendMode = AnimationBlendMode.Additive;
		animationState.normalizedSpeed = 0f;
		animationState.weight = 0f;
		animationState.normalizedTime = 1f;
		animationState.wrapMode = WrapMode.ClampForever;
		animationState.enabled = true;
		Transform transform = base.transform.Find("bone01 pelvis/bone01 spine/bone01 spine1/bone01 neck/bone01 head");
		if (transform)
		{
			animationState.AddMixingTransform(transform.Find("bone01 facial jaw"));
			animationState.AddMixingTransform(transform.Find("bone01 facial lip"));
			animationState.AddMixingTransform(transform.Find("bone01 facial mouth r"));
			animationState.AddMixingTransform(transform.Find("bone01 facial mouth l"));
		}
	}

	private void SetFrame(string Viseme)
	{
		AnimationState x = base.animation[Viseme];
		if (x == this.curVisAnim)
		{
			return;
		}
		if (this.preVisAnim != null)
		{
			this.preVisAnim.weight = 0f;
		}
		this.preVisAnim = this.curVisAnim;
		this.curVisAnim = base.animation[Viseme];
	}
}
