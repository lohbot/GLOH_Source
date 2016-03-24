using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UsequenceLoadableObject : MonoBehaviour
{
	public string path = string.Empty;

	public bool loaded;

	public List<AnimationClip> addAnimations = new List<AnimationClip>();

	public int animsSize;

	public Animation componentAnimation;

	public void SetAnimations()
	{
		this.componentAnimation = base.GetComponent<Animation>();
		if (this.componentAnimation == null)
		{
			return;
		}
		this.animsSize = this.componentAnimation.GetClipCount();
		this.UpdateAnimation(this.componentAnimation);
		this.UpdatePrefab();
	}

	public void ResetAnimations()
	{
		this.componentAnimation = base.GetComponent<Animation>();
		if (this.componentAnimation == null)
		{
			Debug.Log("NO_ANI: " + base.name);
			return;
		}
		if (this.componentAnimation.GetClipCount() < this.GetClipCountExcludeNull())
		{
			this.PopClip(this.componentAnimation, this.componentAnimation.GetClipCount() - this.GetClipCountExcludeNull());
		}
		this.UpdateAnimation(this.componentAnimation);
		this.UpdatePrefab();
	}

	private void UpdateAnimation(Animation componentAnimation)
	{
		for (int i = componentAnimation.GetClipCount() - 1; i >= 0; i--)
		{
			if (this.GetClipNameByIndex(componentAnimation, i) != null)
			{
				componentAnimation.RemoveClip(this.GetClipNameByIndex(componentAnimation, i));
			}
		}
		foreach (AnimationClip current in this.addAnimations)
		{
			if (current != null && componentAnimation.GetClip(current.name) == null)
			{
				componentAnimation.AddClip(current, current.name);
				this.DeleteNullData();
			}
		}
	}

	private void PopClip(Animation animation, int popCount)
	{
		if (animation == null || popCount <= 0)
		{
			return;
		}
		int clipCount = animation.GetClipCount();
		for (int i = clipCount - 1; i >= clipCount - popCount; i--)
		{
			if (i < 0)
			{
				break;
			}
			animation.RemoveClip(this.GetClipNameByIndex(animation, i));
		}
	}

	private string GetClipNameByIndex(Animation animation, int index)
	{
		int num = 0;
		foreach (AnimationState animationState in animation)
		{
			if (num++ == index && animationState != null)
			{
				return animationState.name;
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		this.addAnimations.Clear();
	}

	private void UpdatePrefab()
	{
	}

	public void SyncAnimationToCompAni()
	{
		if (!this.IsSyncAnimation())
		{
			this.addAnimations.Clear();
			foreach (AnimationState animationState in base.animation)
			{
				this.addAnimations.Add(animationState.clip);
			}
		}
	}

	public bool IsSyncAnimation()
	{
		if (this.componentAnimation.GetClipCount() != this.GetClipCountExcludeNull())
		{
			return false;
		}
		for (int i = 0; i < this.componentAnimation.GetClipCount(); i++)
		{
			if (this.addAnimations[i] == null || this.GetClipByIndex(this.componentAnimation, i) != this.addAnimations[i])
			{
				return false;
			}
		}
		return true;
	}

	public int GetClipCountExcludeNull()
	{
		int num = 0;
		for (int i = 0; i < this.addAnimations.Count; i++)
		{
			if (this.addAnimations[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	private AnimationClip GetClipByIndex(Animation animation, int index)
	{
		int num = 0;
		foreach (AnimationState animationState in animation)
		{
			if (num++ == index)
			{
				return animationState.clip;
			}
		}
		return null;
	}

	public void DeleteNullData()
	{
		bool flag = false;
		for (int i = this.addAnimations.Count - 1; i >= 0; i--)
		{
			if (!flag)
			{
				if (this.addAnimations[i] != null)
				{
					flag = true;
				}
			}
			else if (this.addAnimations[i] == null)
			{
				this.addAnimations.RemoveAt(i);
			}
		}
	}

	public void CompareSameData()
	{
		for (int i = this.addAnimations.Count - 1; i > 0; i--)
		{
			if (!(this.addAnimations[i] == null))
			{
				for (int j = i - 1; j >= 0; j--)
				{
					if (this.addAnimations[i] == this.addAnimations[j])
					{
						this.addAnimations[i] = null;
						break;
					}
				}
			}
		}
	}
}
