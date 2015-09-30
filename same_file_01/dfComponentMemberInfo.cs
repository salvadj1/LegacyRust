using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class dfComponentMemberInfo
{
	public UnityEngine.Component Component;

	public string MemberName;

	public bool IsValid
	{
		get
		{
			if ((this.Component == null ? true : string.IsNullOrEmpty(this.MemberName)))
			{
				return false;
			}
			if (this.Component.GetType().GetMember(this.MemberName).FirstOrDefault<MemberInfo>() == null)
			{
				return false;
			}
			return true;
		}
	}

	public dfComponentMemberInfo()
	{
	}

	public Type GetMemberType()
	{
		Type type = this.Component.GetType();
		MemberInfo memberInfo = type.GetMember(this.MemberName).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new MissingMemberException(string.Concat("Member not found: ", type.Name, ".", this.MemberName));
		}
		if (memberInfo is FieldInfo)
		{
			return ((FieldInfo)memberInfo).FieldType;
		}
		if (memberInfo is PropertyInfo)
		{
			return ((PropertyInfo)memberInfo).PropertyType;
		}
		if (memberInfo is MethodInfo)
		{
			return ((MethodInfo)memberInfo).ReturnType;
		}
		if (!(memberInfo is EventInfo))
		{
			throw new InvalidCastException(string.Concat("Invalid member type: ", memberInfo.MemberType));
		}
		return ((EventInfo)memberInfo).EventHandlerType;
	}

	public MethodInfo GetMethod()
	{
		MethodInfo methodInfo = this.Component.GetType().GetMember(this.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault<MemberInfo>() as MethodInfo;
		return methodInfo;
	}

	public dfObservableProperty GetProperty()
	{
		Type type = this.Component.GetType();
		MemberInfo memberInfo = this.Component.GetType().GetMember(this.MemberName).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new MissingMemberException(string.Concat("Member not found: ", type.Name, ".", this.MemberName));
		}
		if (!(memberInfo is FieldInfo) && !(memberInfo is PropertyInfo))
		{
			throw new InvalidCastException(string.Concat("Member ", this.MemberName, " is not an observable field or property"));
		}
		return new dfObservableProperty(this.Component, memberInfo);
	}

	public override string ToString()
	{
		string str = (this.Component == null ? "[Missing ComponentType]" : this.Component.GetType().Name);
		return string.Format("{0}.{1}", str, (string.IsNullOrEmpty(this.MemberName) ? "[Missing MemberName]" : this.MemberName));
	}
}