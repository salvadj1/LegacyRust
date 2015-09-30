using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Data Binding/Event Binding")]
[Serializable]
public class dfEventBinding : MonoBehaviour, IDataBindingComponent
{
	public dfComponentMemberInfo DataSource;

	public dfComponentMemberInfo DataTarget;

	private bool isBound;

	private Component sourceComponent;

	private Component targetComponent;

	private FieldInfo eventField;

	private Delegate eventDelegate;

	private MethodInfo handlerProxy;

	public dfEventBinding()
	{
	}

	private bool areTypesCompatible(ParameterInfo lhs, ParameterInfo rhs)
	{
		if (lhs.ParameterType.Equals(rhs.ParameterType))
		{
			return true;
		}
		if (lhs.ParameterType.IsAssignableFrom(rhs.ParameterType))
		{
			return true;
		}
		return false;
	}

	public void Bind()
	{
		if (this.isBound || this.DataSource == null)
		{
			return;
		}
		if (!this.DataSource.IsValid || !this.DataTarget.IsValid)
		{
			Debug.LogError(string.Format("Invalid event binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		this.sourceComponent = this.DataSource.Component;
		this.targetComponent = this.DataTarget.Component;
		MethodInfo method = this.DataTarget.GetMethod();
		if (method == null)
		{
			Debug.LogError(string.Concat("Event handler not found: ", this.targetComponent.GetType().Name, ".", this.DataTarget.MemberName));
			return;
		}
		this.eventField = this.getField(this.sourceComponent, this.DataSource.MemberName);
		if (this.eventField == null)
		{
			Debug.LogError(string.Concat("Event definition not found: ", this.sourceComponent.GetType().Name, ".", this.DataSource.MemberName));
			return;
		}
		try
		{
			MethodInfo methodInfo = this.eventField.FieldType.GetMethod("Invoke");
			ParameterInfo[] parameters = methodInfo.GetParameters();
			ParameterInfo[] parameterInfoArray = method.GetParameters();
			if ((int)parameters.Length != (int)parameterInfoArray.Length)
			{
				if ((int)parameters.Length <= 0 || (int)parameterInfoArray.Length != 0)
				{
					base.enabled = false;
					throw new InvalidCastException(string.Concat("Event signature mismatch: ", method));
				}
				this.eventDelegate = this.createEventProxyDelegate(this.targetComponent, this.eventField.FieldType, parameters, method);
			}
			else
			{
				this.eventDelegate = Delegate.CreateDelegate(this.eventField.FieldType, this.targetComponent, method, true);
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			base.enabled = false;
			Debug.LogError(string.Concat("Event binding failed - Failed to create event handler: ", exception.ToString()));
			return;
		}
		Delegate @delegate = Delegate.Combine(this.eventDelegate, (Delegate)this.eventField.GetValue(this.sourceComponent));
		this.eventField.SetValue(this.sourceComponent, @delegate);
		this.isBound = true;
	}

	private void callProxyEventHandler()
	{
		if (this.handlerProxy != null)
		{
			this.handlerProxy.Invoke(this.targetComponent, null);
		}
	}

	[dfEventProxy]
	private void ChildControlEventProxy(dfControl container, dfControl child)
	{
		this.callProxyEventHandler();
	}

	private Delegate createEventProxyDelegate(object target, Type delegateType, ParameterInfo[] eventParams, MethodInfo eventHandler)
	{
		MethodInfo methodInfo = (
			from m in typeof(dfEventBinding).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
			where (!m.IsDefined(typeof(dfEventProxyAttribute), true) ? false : this.signatureIsCompatible(eventParams, m.GetParameters()))
			select m).FirstOrDefault<MethodInfo>();
		if (methodInfo == null)
		{
			return null;
		}
		this.handlerProxy = eventHandler;
		return Delegate.CreateDelegate(delegateType, this, methodInfo, true);
	}

	[dfEventProxy]
	private void DragEventProxy(dfControl control, dfDragEventArgs dragEvent)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void FocusEventProxy(dfControl control, dfFocusEventArgs args)
	{
		this.callProxyEventHandler();
	}

	private FieldInfo getField(Component sourceComponent, string fieldName)
	{
		return (
			from f in sourceComponent.GetType().GetAllFields()
			where f.Name == fieldName
			select f).FirstOrDefault<FieldInfo>();
	}

	[dfEventProxy]
	private void KeyEventProxy(dfControl control, dfKeyEventArgs keyEvent)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void MouseEventProxy(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.callProxyEventHandler();
	}

	public void OnDisable()
	{
		this.Unbind();
	}

	public void OnEnable()
	{
		if (this.DataSource != null && !this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, int value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, float value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, bool value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, string value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Vector2 value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Vector3 value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Vector4 value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Quaternion value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, dfButton.ButtonState value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, dfPivotPoint value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Texture2D value)
	{
		this.callProxyEventHandler();
	}

	[dfEventProxy]
	private void PropertyChangedProxy(dfControl control, Material value)
	{
		this.callProxyEventHandler();
	}

	private bool signatureIsCompatible(ParameterInfo[] lhs, ParameterInfo[] rhs)
	{
		if (lhs == null || rhs == null)
		{
			return false;
		}
		if ((int)lhs.Length != (int)rhs.Length)
		{
			return false;
		}
		for (int i = 0; i < (int)lhs.Length; i++)
		{
			if (!this.areTypesCompatible(lhs[i], rhs[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void Start()
	{
		if (this.DataSource != null && !this.isBound && this.DataSource.IsValid && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	public override string ToString()
	{
		string str = (this.DataSource == null || !(this.DataSource.Component != null) ? "[null]" : this.DataSource.Component.GetType().Name);
		string str1 = (this.DataSource == null || string.IsNullOrEmpty(this.DataSource.MemberName) ? "[null]" : this.DataSource.MemberName);
		string str2 = (this.DataTarget == null || !(this.DataTarget.Component != null) ? "[null]" : this.DataTarget.Component.GetType().Name);
		string str3 = (this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName) ? "[null]" : this.DataTarget.MemberName);
		return string.Format("Bind {0}.{1} -> {2}.{3}", new object[] { str, str1, str2, str3 });
	}

	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.isBound = false;
		Delegate value = (Delegate)this.eventField.GetValue(this.sourceComponent);
		Delegate @delegate = Delegate.Remove(value, this.eventDelegate);
		this.eventField.SetValue(this.sourceComponent, @delegate);
		this.eventField = null;
		this.eventDelegate = null;
		this.handlerProxy = null;
		this.sourceComponent = null;
		this.targetComponent = null;
	}
}