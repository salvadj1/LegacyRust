using System;

public interface ICarriableTrans
{
	void OnAddedToCarrier(TransCarrier carrier);

	void OnDroppedFromCarrier(TransCarrier carrier);
}