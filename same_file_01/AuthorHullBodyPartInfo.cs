using System;
using UnityEngine;

[Serializable]
public class AuthorHullBodyPartInfo
{
	public Transform transform;

	public string rootPath;

	public AuthorChHit hit;

	public AuthorChJoint joint;

	public AuthorHullBodyPartInfo()
	{
	}
}