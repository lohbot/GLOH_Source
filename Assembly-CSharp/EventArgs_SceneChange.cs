using Ndoors.Framework.Stage;
using System;

public class EventArgs_SceneChange : EventArgs
{
	public Scene.Type PreScene;

	public Scene.Type CurScene;

	public void Set(Scene.Type prescene, Scene.Type curscene)
	{
		this.PreScene = prescene;
		this.CurScene = curscene;
	}
}
