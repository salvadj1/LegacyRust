using System;

public interface IContextRequestableClientIntercept
{
	bool ContextIntercept(Controllable localControllable);
}