using System;

[Flags]
public enum InterfaceSearchRoute
{
	GameObject = 1,
	Children = 2,
	Parents = 4,
	Root = 8,
	Remote = 16
}