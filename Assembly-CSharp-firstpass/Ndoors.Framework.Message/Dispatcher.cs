using System;
using System.Reflection;

namespace Ndoors.Framework.Message
{
	public static class Dispatcher
	{
		public static bool DispatchDynamic(object objHandler, string functionName, params object[] Params)
		{
			return Dispatcher.DispatchInternal(objHandler.GetType(), objHandler, functionName, Params);
		}

		public static bool DispatchStatic(string functionName, Type handlerType, object[] Params)
		{
			return Dispatcher.DispatchInternal(handlerType, null, functionName, Params);
		}

		private static MethodInfo _GetMethodInfo(object objHandler, Type typeHandler, string functionName, out Type cType)
		{
			cType = null;
			if (objHandler != null)
			{
				cType = objHandler.GetType();
			}
			else if (typeHandler != null)
			{
				cType = typeHandler;
			}
			if (cType == null)
			{
				TsLog.LogWarning("MSGHANDLE Parameter null null!", new object[0]);
				return null;
			}
			return cType.GetMethod(functionName);
		}

		public static T DispatchRef<T>(object objHandler, Type typeHandler, string functionName, params object[] Params) where T : class
		{
			object obj = null;
			Type type = null;
			MethodInfo methodInfo = Dispatcher._GetMethodInfo(objHandler, typeHandler, functionName, out type);
			if (methodInfo == null)
			{
				TsLog.LogError("MSGHANDLE DispatchRef<T> --- Missing! Message Handler not found! {0}.{1}", new object[]
				{
					(type != null) ? type.Name : "null",
					functionName
				});
			}
			else
			{
				try
				{
					obj = methodInfo.Invoke(objHandler, Params);
				}
				catch (NullReferenceException)
				{
					TsLog.LogWarning("MSGHANDLE DispatchRef<T> --- Invalid return type! {0}.{1}", new object[]
					{
						type.Name,
						functionName
					});
				}
				catch (Exception ex)
				{
					TsLog.LogError("MSGHANDLE DispatchRef<T> --- Invalid invoke! {0}.{1} - {2}", new object[]
					{
						type.Name,
						functionName,
						ex
					});
				}
			}
			return obj as T;
		}

		public static T DispatchReturn<T>(object objHandler, Type typeHandler, string functionName, params object[] Params)
		{
			Type type = null;
			MethodInfo methodInfo = Dispatcher._GetMethodInfo(objHandler, typeHandler, functionName, out type);
			if (methodInfo == null)
			{
				TsLog.LogError("MSGHANDLE DispatchReturn<T> --- Missing! Message Handler not found! {0}.{1}", new object[]
				{
					(type != null) ? type.Name : "null",
					functionName
				});
			}
			else
			{
				try
				{
					return (T)((object)methodInfo.Invoke(objHandler, Params));
				}
				catch (NullReferenceException)
				{
					TsLog.LogWarning(string.Format("MSGHANDLE DispatchReturn<T> --- Invalid return type! {0}.{1}", type.Name, functionName), new object[0]);
				}
				catch (Exception ex)
				{
					TsLog.LogError("MSGHANDLE DispatchReturn<T> --- Invalid invoke! {0}.{1} - {2}", new object[]
					{
						type.Name,
						functionName,
						ex
					});
				}
			}
			return default(T);
		}

		private static bool DispatchInternal(Type typeHandler, object objHandler, string functionName, object[] Params)
		{
			Type type = null;
			MethodInfo methodInfo = Dispatcher._GetMethodInfo(objHandler, typeHandler, functionName, out type);
			bool result = false;
			if (methodInfo == null)
			{
				TsLog.LogWarning("MSGHANDLE --- Missing! Message Handler not found! {0}.{1}", new object[]
				{
					typeHandler.Name,
					functionName
				});
			}
			else
			{
				try
				{
					result = (bool)methodInfo.Invoke(objHandler, Params);
				}
				catch (NullReferenceException)
				{
					TsLog.LogWarning("MSGHANDLE --- Invalid return type! {0}.{1} - function must return bool!", new object[]
					{
						typeHandler.GetType().Name,
						functionName
					});
				}
				catch (Exception ex)
				{
					TsLog.LogError("MSGHANDLE --- Invalid invoke! {0}.{1} - {2}", new object[]
					{
						(objHandler != null) ? objHandler.GetType().Name : typeHandler.GetType().Name,
						functionName,
						ex
					});
				}
			}
			return result;
		}
	}
}
