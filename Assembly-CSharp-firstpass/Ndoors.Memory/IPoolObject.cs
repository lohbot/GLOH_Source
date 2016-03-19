using System;

namespace Ndoors.Memory
{
	public interface IPoolObject
	{
		void OnCreate(object param);

		void OnDelete();
	}
}
