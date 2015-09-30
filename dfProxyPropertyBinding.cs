using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Data Binding/Proxy Property Binding")]
[Serializable]
public class dfProxyPropertyBinding : MonoBehaviour, IDataBindingComponent
{
	public dfComponentMemberInfo DataSource;

	public dfComponentMemberInfo DataTarget;

	public bool TwoWay;

	private dfObservableProperty sourceProperty;

	private dfObservableProperty targetProperty;

	private bool isBound;

	private bool eventsAttached;

	public dfProxyPropertyBinding()
	{
	}

	private void attachEvent()
	{
		if (this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = true;
		dfDataObjectProxy component = this.DataSource.Component as dfDataObjectProxy;
		if (component != null)
		{
			component.DataChanged += new dfDataObjectProxy.DataObjectChangedHandler(this.handle_DataChanged);
		}
	}

	public void Awake()
	{
	}

	public void Bind()
	{
		if (this.isBound)
		{
			return;
		}
		if (!this.IsDataSourceValid())
		{
			Debug.LogError(string.Format("Invalid data binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		if (!this.DataTarget.IsValid)
		{
			Debug.LogError(string.Format("Invalid data binding configuration - Source:{0}, Target:{1}", this.DataSource, this.DataTarget));
			return;
		}
		dfDataObjectProxy component = this.DataSource.Component as dfDataObjectProxy;
		this.sourceProperty = component.GetProperty(this.DataSource.MemberName);
		this.targetProperty = this.DataTarget.GetProperty();
		this.isBound = (this.sourceProperty == null ? false : this.targetProperty != null);
		if (this.isBound)
		{
			this.targetProperty.Value = this.sourceProperty.Value;
		}
		this.attachEvent();
	}

	private void detachEvent()
	{
		if (!this.eventsAttached)
		{
			return;
		}
		this.eventsAttached = false;
		dfDataObjectProxy component = this.DataSource.Component as dfDataObjectProxy;
		if (component != null)
		{
			component.DataChanged -= new dfDataObjectProxy.DataObjectChangedHandler(this.handle_DataChanged);
		}
	}

	private void handle_DataChanged(object data)
	{
		this.Unbind();
		if (this.IsDataSourceValid())
		{
			this.Bind();
		}
	}

	private bool IsDataSourceValid()
	{
		return (this.DataSource != null || this.DataSource.Component != null || !string.IsNullOrEmpty(this.DataSource.MemberName) ? true : (this.DataSource.Component as dfDataObjectProxy).Data != null);
	}

	public void OnDisable()
	{
		this.Unbind();
	}

	public void OnEnable()
	{
		if (!this.isBound && this.IsDataSourceValid() && this.DataTarget.IsValid)
		{
			this.Bind();
		}
	}

	public void Start()
	{
		if (!this.isBound && this.IsDataSourceValid() && this.DataTarget.IsValid)
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
		this.detachEvent();
		this.sourceProperty = null;
		this.targetProperty = null;
		this.isBound = false;
	}

	public void Update()
	{
		if (this.sourceProperty == null || this.targetProperty == null)
		{
			return;
		}
		if (this.sourceProperty.HasChanged)
		{
			this.targetProperty.Value = this.sourceProperty.Value;
			this.sourceProperty.ClearChangedFlag();
		}
		else if (this.TwoWay && this.targetProperty.HasChanged)
		{
			this.sourceProperty.Value = this.targetProperty.Value;
			this.targetProperty.ClearChangedFlag();
		}
	}
}