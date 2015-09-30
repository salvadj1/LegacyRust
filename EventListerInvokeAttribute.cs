using System;

[AttributeUsage(AttributeTargets.Delegate)]
public class EventListerInvokeAttribute : Attribute
{
	internal readonly Type InvokeClass;

	internal readonly string InvokeMember;

	internal readonly Type InvokeCall;

	public EventListerInvokeAttribute(Type invokeClass, string invokeMember, Type invokeCall)
	{
		this.InvokeClass = invokeClass;
		this.InvokeMember = invokeMember;
		this.InvokeCall = invokeCall;
	}
}