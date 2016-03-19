using System;

namespace Ndoors.Memory
{
	public interface IObjectPoolContainer
	{
		ObjectPoolAttribute objectPoolAttr
		{
			get;
		}

		object Acquire();

		void Release(object obj);

		void Clear();
	}
}
