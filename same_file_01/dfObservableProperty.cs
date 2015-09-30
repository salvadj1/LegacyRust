using System;
using System.Linq;
using System.Reflection;

public class dfObservableProperty : IObservableValue
{
	private object lastValue;

	private bool hasChanged;

	private object target;

	private FieldInfo fieldInfo;

	private PropertyInfo propertyInfo;

	private MethodInfo propertyGetter;

	public bool HasChanged
	{
		get
		{
			if (this.hasChanged)
			{
				return true;
			}
			object obj = this.getter();
			if (object.ReferenceEquals(obj, this.lastValue))
			{
				this.hasChanged = false;
			}
			else if (obj == null || this.lastValue == null)
			{
				this.hasChanged = true;
			}
			else
			{
				this.hasChanged = !obj.Equals(this.lastValue);
			}
			return this.hasChanged;
		}
	}

	public object Value
	{
		get
		{
			return JustDecompileGenerated_get_Value();
		}
		set
		{
			JustDecompileGenerated_set_Value(value);
		}
	}

	public object JustDecompileGenerated_get_Value()
	{
		return this.getter();
	}

	public void JustDecompileGenerated_set_Value(object value)
	{
		this.lastValue = value;
		this.setter(value);
		this.hasChanged = false;
	}

	internal dfObservableProperty(object target, string memberName)
	{
		MemberInfo memberInfo = target.GetType().GetMember(memberName, BindingFlags.Instance | BindingFlags.Public).FirstOrDefault<MemberInfo>();
		if (memberInfo == null)
		{
			throw new ArgumentException(string.Concat("Invalid property or field name: ", memberName), "memberName");
		}
		this.initMember(target, memberInfo);
	}

	internal dfObservableProperty(object target, FieldInfo field)
	{
		this.initField(target, field);
	}

	internal dfObservableProperty(object target, PropertyInfo property)
	{
		this.initProperty(target, property);
	}

	internal dfObservableProperty(object target, MemberInfo member)
	{
		this.initMember(target, member);
	}

	public void ClearChangedFlag()
	{
		this.hasChanged = false;
		this.lastValue = this.getter();
	}

	private object getFieldValue()
	{
		return this.fieldInfo.GetValue(this.target);
	}

	private object getPropertyValue()
	{
		if (this.propertyGetter == null)
		{
			this.propertyGetter = this.propertyInfo.GetGetMethod();
			if (this.propertyGetter == null)
			{
				throw new InvalidOperationException(string.Concat("Cannot read property: ", this.propertyInfo));
			}
		}
		return this.propertyGetter.Invoke(this.target, null);
	}

	private object getter()
	{
		if (this.propertyInfo != null)
		{
			return this.getPropertyValue();
		}
		return this.getFieldValue();
	}

	private void initField(object target, FieldInfo field)
	{
		this.target = target;
		this.fieldInfo = field;
		this.Value = this.getter();
	}

	private void initMember(object target, MemberInfo member)
	{
		if (!(member is FieldInfo))
		{
			this.initProperty(target, (PropertyInfo)member);
		}
		else
		{
			this.initField(target, (FieldInfo)member);
		}
	}

	private void initProperty(object target, PropertyInfo property)
	{
		this.target = target;
		this.propertyInfo = property;
		this.Value = this.getter();
	}

	private void setFieldValue(object value)
	{
		if (this.fieldInfo.IsLiteral)
		{
			return;
		}
		Type fieldType = this.fieldInfo.FieldType;
		if (value == null || fieldType.IsAssignableFrom(value.GetType()))
		{
			this.fieldInfo.SetValue(this.target, value);
		}
		else
		{
			object obj = Convert.ChangeType(value, fieldType);
			this.fieldInfo.SetValue(this.target, obj);
		}
	}

	private void setFieldValueNOP(object value)
	{
	}

	private void setPropertyValue(object value)
	{
		MethodInfo setMethod = this.propertyInfo.GetSetMethod();
		if (!this.propertyInfo.CanWrite || setMethod == null)
		{
			return;
		}
		Type propertyType = this.propertyInfo.PropertyType;
		if (value == null || propertyType.IsAssignableFrom(value.GetType()))
		{
			this.propertyInfo.SetValue(this.target, value, null);
		}
		else
		{
			object obj = Convert.ChangeType(value, propertyType);
			this.propertyInfo.SetValue(this.target, obj, null);
		}
	}

	private void setter(object value)
	{
		if (this.propertyInfo == null)
		{
			this.setFieldValue(value);
		}
		else
		{
			this.setPropertyValue(value);
		}
	}

	private delegate object ValueGetter();

	private delegate void ValueSetter(object value);
}