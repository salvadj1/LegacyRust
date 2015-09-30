using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class InterfaceDriverComponentAttribute : Attribute
{
	public readonly string SerializedFieldName;

	public readonly string RuntimeFieldName;

	public readonly Type Interface;

	private Type _minimumType = typeof(MonoBehaviour);

	private InterfaceSearchRoute searchRoute = InterfaceSearchRoute.GameObject;

	public string AdditionalProperties
	{
		get;
		set;
	}

	public bool AlwaysSaveDisabled
	{
		get;
		set;
	}

	public InterfaceSearchRoute SearchRoute
	{
		get
		{
			return this.searchRoute;
		}
		set
		{
			if ((int)value == 0)
			{
				value = InterfaceSearchRoute.GameObject;
			}
			this.searchRoute = value;
		}
	}

	public Type UnityType
	{
		get
		{
			return this._minimumType;
		}
		set
		{
			this._minimumType = value ?? typeof(MonoBehaviour);
		}
	}

	public InterfaceDriverComponentAttribute(Type interfaceType, string serializedFieldName, string runtimeFieldName)
	{
		this.Interface = interfaceType;
		this.SerializedFieldName = serializedFieldName;
		this.RuntimeFieldName = runtimeFieldName;
	}
}