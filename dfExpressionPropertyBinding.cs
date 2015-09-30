using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Daikon Forge/Data Binding/Expression Binding")]
[Serializable]
public class dfExpressionPropertyBinding : MonoBehaviour, IDataBindingComponent
{
	public Component DataSource;

	public dfComponentMemberInfo DataTarget;

	[SerializeField]
	protected string expression;

	private Delegate compiledExpression;

	private dfObservableProperty targetProperty;

	private bool isBound;

	public string Expression
	{
		get
		{
			return this.expression;
		}
		set
		{
			if (!string.Equals(value, this.expression))
			{
				this.Unbind();
				this.expression = value;
			}
		}
	}

	public dfExpressionPropertyBinding()
	{
	}

	public void Bind()
	{
		if (this.isBound)
		{
			return;
		}
		if (this.DataSource is dfDataObjectProxy && ((dfDataObjectProxy)this.DataSource).Data == null)
		{
			return;
		}
		dfScriptEngineSettings dfScriptEngineSetting = new dfScriptEngineSettings();
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "Application", typeof(Application) },
			{ "Color", typeof(Color) },
			{ "Color32", typeof(Color32) },
			{ "Random", typeof(UnityEngine.Random) },
			{ "Time", typeof(Time) },
			{ "ScriptableObject", typeof(ScriptableObject) },
			{ "Vector2", typeof(Vector2) },
			{ "Vector3", typeof(Vector3) },
			{ "Vector4", typeof(Vector4) },
			{ "Quaternion", typeof(Quaternion) },
			{ "Matrix", typeof(Matrix4x4) },
			{ "Mathf", typeof(Mathf) }
		};
		dfScriptEngineSetting.Constants = strs;
		dfScriptEngineSettings dfScriptEngineSetting1 = dfScriptEngineSetting;
		if (!(this.DataSource is dfDataObjectProxy))
		{
			dfScriptEngineSetting1.AddVariable(new dfScriptVariable("source", this.DataSource));
		}
		else
		{
			dfDataObjectProxy dataSource = this.DataSource as dfDataObjectProxy;
			dfScriptEngineSetting1.AddVariable(new dfScriptVariable("source", null, dataSource.DataType));
		}
		this.compiledExpression = dfScriptEngine.CompileExpression(this.expression, dfScriptEngineSetting1);
		this.targetProperty = this.DataTarget.GetProperty();
		this.isBound = (this.compiledExpression == null ? false : this.targetProperty != null);
	}

	private void evaluate()
	{
		try
		{
			object dataSource = this.DataSource;
			if (dataSource is dfDataObjectProxy)
			{
				dataSource = ((dfDataObjectProxy)dataSource).Data;
			}
			object obj = this.compiledExpression.DynamicInvoke(new object[] { dataSource });
			this.targetProperty.Value = obj;
		}
		catch (Exception exception)
		{
			Debug.LogError(exception);
		}
	}

	public void OnDisable()
	{
		this.Unbind();
	}

	public override string ToString()
	{
		string str = (this.DataTarget == null || !(this.DataTarget.Component != null) ? "[null]" : this.DataTarget.Component.GetType().Name);
		return string.Format("Bind [expression] -> {0}.{1}", str, (this.DataTarget == null || string.IsNullOrEmpty(this.DataTarget.MemberName) ? "[null]" : this.DataTarget.MemberName));
	}

	public void Unbind()
	{
		if (!this.isBound)
		{
			return;
		}
		this.compiledExpression = null;
		this.targetProperty = null;
		this.isBound = false;
	}

	public void Update()
	{
		if (this.isBound)
		{
			this.evaluate();
		}
		else if ((!(this.DataSource != null) || string.IsNullOrEmpty(this.expression) ? false : this.DataTarget.IsValid))
		{
			this.Bind();
		}
	}
}