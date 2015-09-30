using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Tweens/Tween Event Binding")]
[Serializable]
public class dfTweenEventBinding : MonoBehaviour
{
	public Component Tween;

	public Component EventSource;

	public string StartEvent;

	public string StopEvent;

	public string ResetEvent;

	private bool isBound;

	private FieldInfo startEventField;

	private FieldInfo stopEventField;

	private FieldInfo resetEventField;

	private Delegate startEventHandler;

	private Delegate stopEventHandler;

	private Delegate resetEventHandler;

	public dfTweenEventBinding()
	{
	}

	public void Bind()
	{
		if (this.isBound && !this.isValid())
		{
			return;
		}
		this.isBound = true;
		if (!string.IsNullOrEmpty(this.StartEvent))
		{
			this.bindEvent(this.StartEvent, "Play", out this.startEventField, out this.startEventHandler);
		}
		if (!string.IsNullOrEmpty(this.StopEvent))
		{
			this.bindEvent(this.StopEvent, "Stop", out this.stopEventField, out this.stopEventHandler);
		}
		if (!string.IsNullOrEmpty(this.ResetEvent))
		{
			this.bindEvent(this.ResetEvent, "Reset", out this.resetEventField, out this.resetEventHandler);
		}
	}

	private void bindEvent(string eventName, string handlerName, out FieldInfo eventField, out Delegate eventHandler)
	{
		eventField = null;
		eventHandler = null;
		MethodInfo method = this.Tween.GetType().GetMethod(handlerName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMemberException(string.Concat("Method not found: ", handlerName));
		}
		eventField = this.getField(this.EventSource.GetType(), eventName);
		if (eventField == null)
		{
			throw new MissingMemberException(string.Concat("Event not found: ", eventName));
		}
		try
		{
			MethodInfo methodInfo = eventField.FieldType.GetMethod("Invoke");
			ParameterInfo[] parameters = methodInfo.GetParameters();
			ParameterInfo[] parameterInfoArray = method.GetParameters();
			if ((int)parameters.Length != (int)parameterInfoArray.Length)
			{
				if ((int)parameters.Length <= 0 || (int)parameterInfoArray.Length != 0)
				{
					throw new InvalidCastException(string.Concat("Event signature mismatch: ", eventHandler));
				}
				eventHandler = this.createDynamicWrapper(this.Tween, eventField.FieldType, parameters, method);
			}
			else
			{
				eventHandler = Delegate.CreateDelegate(eventField.FieldType, this.Tween, method, true);
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Event binding failed - Failed to create event handler: ", exception.ToString()));
			return;
		}
		Delegate @delegate = Delegate.Combine(eventHandler, (Delegate)eventField.GetValue(this.EventSource));
		eventField.SetValue(this.EventSource, @delegate);
	}

	private Delegate createDynamicWrapper(object target, Type delegateType, ParameterInfo[] eventParams, MethodInfo eventHandler)
	{
		Type[] array = ((IEnumerable<Type>)(new Type[] { target.GetType() })).Concat<Type>(
			from p in (IEnumerable<ParameterInfo>)eventParams
			select p.ParameterType).ToArray<Type>();
		DynamicMethod dynamicMethod = new DynamicMethod(string.Concat("DynamicEventWrapper_", eventHandler.Name), typeof(void), array);
		ILGenerator lGenerator = dynamicMethod.GetILGenerator();
		lGenerator.Emit(OpCodes.Ldarg_0);
		lGenerator.EmitCall(OpCodes.Callvirt, eventHandler, Type.EmptyTypes);
		lGenerator.Emit(OpCodes.Ret);
		return dynamicMethod.CreateDelegate(delegateType, target);
	}

	private FieldInfo getField(Type type, string fieldName)
	{
		return (
			from f in type.GetAllFields()
			where f.Name == fieldName
			select f).FirstOrDefault<FieldInfo>();
	}

	private bool isValid()
	{
		if (this.Tween == null || !(this.Tween is dfTweenComponentBase))
		{
			return false;
		}
		if (this.EventSource == null)
		{
			return false;
		}
		if ((!string.IsNullOrEmpty(this.StartEvent) || !string.IsNullOrEmpty(this.StopEvent) ? false : string.IsNullOrEmpty(this.ResetEvent)))
		{
			return false;
		}
		Type type = this.EventSource.GetType();
		if (!string.IsNullOrEmpty(this.StartEvent) && this.getField(type, this.StartEvent) == null)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(this.StopEvent) && this.getField(type, this.StopEvent) == null)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(this.ResetEvent) && this.getField(type, this.ResetEvent) == null)
		{
			return false;
		}
		return true;
	}

	private void OnDisable()
	{
		this.Unbind();
	}

	private void OnEnable()
	{
		if (this.isValid())
		{
			this.Bind();
		}
	}

	private void Start()
	{
		if (this.isValid())
		{
			this.Bind();
		}
	}

	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.isBound = false;
		if (this.startEventField != null)
		{
			this.unbindEvent(this.startEventField, this.startEventHandler);
			this.startEventField = null;
			this.startEventHandler = null;
		}
		if (this.stopEventField != null)
		{
			this.unbindEvent(this.stopEventField, this.stopEventHandler);
			this.stopEventField = null;
			this.stopEventHandler = null;
		}
		if (this.resetEventField != null)
		{
			this.unbindEvent(this.resetEventField, this.resetEventHandler);
			this.resetEventField = null;
			this.resetEventHandler = null;
		}
	}

	private void unbindEvent(FieldInfo eventField, Delegate eventDelegate)
	{
		Delegate value = (Delegate)eventField.GetValue(this.EventSource);
		Delegate @delegate = Delegate.Remove(value, eventDelegate);
		eventField.SetValue(this.EventSource, @delegate);
	}
}