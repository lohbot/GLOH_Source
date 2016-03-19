using System;

public interface ISubStageMobile
{
	void FinishCreateChar();

	void NextSceneProcess();

	NrCharUser GetSelectedChar();
}
