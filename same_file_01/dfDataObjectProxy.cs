using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Data Binding/Proxy Data Object")]
[Serializable]
public class dfDataObjectProxy : MonoBehaviour, IDataBindingComponent
{
	[SerializeField]
	protected string typeName;

	private object data;

	private dfDataObjectProxy.DataObjectChangedHandler DataChanged;

	public object Data
	{
		get
		{
			return this.data;
		}
		set
		{
			if (!object.ReferenceEquals(value, this.data))
			{
				this.data = value;
				if (value != null)
				{
					this.typeName = value.GetType().Name;
				}
				if (this.DataChanged != null)
				{
					this.DataChanged(value);
				}
			}
		}
	}

	public Type DataType
	{
		get
		{
			return this.getTypeFromName(this.typeName);
		}
	}

	public string TypeName
	{
		get
		{
			return this.typeName;
		}
		set
		{
			if (this.typeName != value)
			{
				this.typeName = value;
				this.Data = null;
			}
		}
	}

	public dfDataObjectProxy()
	{
	}

	public void Bind()
	{
	}

	public dfObservableProperty GetProperty(string PropertyName)
	{
		if (this.data == null)
		{
			return null;
		}
		return new dfObservableProperty(this.data, PropertyName);
	}

	public Type GetPropertyType(string PropertyName)
	{
		Type dataType = this.DataType;
		if (dataType == null)
		{
			return null;
		}
		MemberInfo memberInfo = dataType.GetMember(PropertyName, BindingFlags.Instance | BindingFlags.Public).FirstOrDefault<MemberInfo>();
		if (memberInfo is FieldInfo)
		{
			return ((FieldInfo)memberInfo).FieldType;
		}
		if (!(memberInfo is PropertyInfo))
		{
			return null;
		}
		return ((PropertyInfo)memberInfo).PropertyType;
	}

	private Type getTypeFromName(string typeName)
	{
		Type[] types = base.GetType().Assembly.GetTypes();
		Type type = (
			from t in types
			where t.Name == typeName
			select t).FirstOrDefault<Type>();
		return type;
	}

	private static Type getTypeFromQualifiedName(string typeName)
	{
		Type type = Type.GetType(typeName);
		if (type != null)
		{
			return type;
		}
		if (typeName.IndexOf('.') == -1)
		{
			return null;
		}
		string str = typeName.Substring(0, typeName.IndexOf('.'));
		Assembly assembly = Assembly.Load(str);
		if (assembly == null)
		{
			return null;
		}
		return assembly.GetType(typeName);
	}

	public void Start()
	{
		if (this.DataType == null)
		{
			Debug.LogError(string.Concat("Unable to retrieve System.Type reference for type: ", this.TypeName));
		}
	}

	public void Unbind()
	{
	}

	public event dfDataObjectProxy.DataObjectChangedHandler DataChanged
	{
		add
		{
			this.DataChanged += value;
		}
		remove
		{
			this.DataChanged -= value;
		}
	}

	[dfEventCategory("Data Changed")]
	public delegate void DataObjectChangedHandler(object data);
}