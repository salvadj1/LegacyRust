using System;

[Obsolete("Use HealthDimmer as a private field, and call UpdateHeathAmount on that instead of using this component")]
public class HealthTextureDimmer : IDLocal
{
	[NonSerialized]
	private HealthDimmer healthDimmer;

	public HealthTextureDimmer()
	{
	}

	protected void OnPoolRetire()
	{
		this.healthDimmer.Reset();
	}

	public void UpdateHealthAmount(float newHealth)
	{
		this.healthDimmer.UpdateHealthAmount(this, newHealth, false);
	}
}