using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITsLightCollector
{
	void ChnageCollector(Func<IEnumerable<Light>> enumerableFunc);

	void Clear();
}
