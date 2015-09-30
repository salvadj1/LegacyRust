using System;

public interface ICameraFX
{
	void OnViewModelChange(ViewModel viewModel);

	void PostRender();

	void PreCull();
}