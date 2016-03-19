using System;
using System.Collections;

namespace TsBundle
{
	public delegate IEnumerator LoadAsyncCallback(IDownloadedItem wItem, object targetObj, string name, Type type);
}
