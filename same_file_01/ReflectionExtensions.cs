using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ReflectionExtensions
{
	public static FieldInfo[] GetAllFields(this Type type)
	{
		if (type == null)
		{
			return new FieldInfo[0];
		}
		BindingFlags bindingFlag = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		return (
			from f in type.GetFields(bindingFlag).Concat<FieldInfo>(type.BaseType.GetAllFields())
			where !f.IsDefined(typeof(HideInInspector), true)
			select f).ToArray<FieldInfo>();
	}

	public static object GetProperty(this object target, string property)
	{
		if (target == null)
		{
			throw new NullReferenceException("Target is null");
		}
		MemberInfo[] member = target.GetType().GetMember(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (member == null || (int)member.Length == 0)
		{
			throw new IndexOutOfRangeException(string.Concat("Property not found: ", property));
		}
		MemberInfo memberInfo = member[0];
		if (memberInfo is FieldInfo)
		{
			return ((FieldInfo)memberInfo).GetValue(target);
		}
		if (!(memberInfo is PropertyInfo))
		{
			throw new InvalidOperationException(string.Concat("Member type not supported: ", memberInfo.MemberType));
		}
		return ((PropertyInfo)memberInfo).GetValue(target, null);
	}

	public static void SetProperty(this object target, string property, object value)
	{
		if (target == null)
		{
			throw new NullReferenceException("Target is null");
		}
		MemberInfo[] member = target.GetType().GetMember(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (member == null || (int)member.Length == 0)
		{
			throw new IndexOutOfRangeException(string.Concat("Property not found: ", property));
		}
		MemberInfo memberInfo = member[0];
		if (memberInfo is FieldInfo)
		{
			((FieldInfo)memberInfo).SetValue(target, value);
			return;
		}
		if (!(memberInfo is PropertyInfo))
		{
			throw new InvalidOperationException(string.Concat("Member type not supported: ", memberInfo.MemberType));
		}
		((PropertyInfo)memberInfo).SetValue(target, value, null);
	}
}