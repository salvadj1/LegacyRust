using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[ExecuteInEditMode]
public sealed class LaserGraphics : MonoBehaviour
{
	private const float kNormalPushBack = -0.01f;

	private const float kDotMaxAlpha = 12f;

	private const float kBeamMaxAlpha = 1f;

	private const string singletonName = "__LASER_GRAPHICS__";

	[NonSerialized]
	private List<LaserBeam> beams;

	[NonSerialized]
	private List<LaserBeam> willRender;

	private static int allBeamsMask;

	private static Matrix4x4 world2Cam;

	private static Matrix4x4 cam2World;

	private static Matrix4x4 camProj;

	private readonly static Vector2[] uv;

	[NonSerialized]
	private bool madeLists;

	private static LaserGraphics singleton;

	static LaserGraphics()
	{
		LaserGraphics.uv = new Vector2[] { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 0f), new Vector2(1f, 1f) };
	}

	public LaserGraphics()
	{
	}

	public static void EnsureGraphicsExist()
	{
		if (!LaserGraphics.singleton)
		{
			GameObject gameObject = GameObject.Find("__LASER_GRAPHICS__");
			if (gameObject)
			{
				LaserGraphics.singleton = gameObject.GetComponent<LaserGraphics>();
				if (!LaserGraphics.singleton)
				{
					LaserGraphics.singleton = gameObject.AddComponent<LaserGraphics>();
					LaserGraphics.singleton.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
				}
			}
			else
			{
				GameObject gameObject1 = new GameObject()
				{
					hideFlags = HideFlags.DontSave | HideFlags.NotEditable,
					name = "__LASER_GRAPHICS__"
				};
				gameObject = gameObject1;
				LaserGraphics.singleton = gameObject.AddComponent<LaserGraphics>();
				LaserGraphics.singleton.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
			}
		}
	}

	private static Color RangeBeamColor(Color input)
	{
		float single;
		if (input.r <= input.g)
		{
			single = (input.b <= input.g ? input.g : input.b);
		}
		else
		{
			single = (input.b <= input.r ? input.r : input.b);
		}
		if (single == 0f)
		{
			input.a = 1f;
		}
		else
		{
			input.r = input.r / single;
			input.g = input.g / single;
			input.b = input.b / single;
			input.a = single / 1f;
		}
		return input;
	}

	private static Color RangeDotColor(Color input)
	{
		float single;
		if (input.r <= input.g)
		{
			single = (input.b <= input.g ? input.g : input.b);
		}
		else
		{
			single = (input.b <= input.r ? input.r : input.b);
		}
		if (single == 0f)
		{
			input.a = 0.0833333358f;
		}
		else
		{
			input.r = input.r / single;
			input.g = input.g / single;
			input.b = input.b / single;
			input.a = single / 12f;
		}
		return input;
	}

	private static void RenderBeam(Plane[] frustum, Camera camera, LaserBeam beam, ref LaserBeam.FrameData frame)
	{
		Vector3 vector3 = LaserGraphics.world2Cam.MultiplyPoint(frame.origin);
		Vector3 vector31 = LaserGraphics.world2Cam.MultiplyPoint(frame.point);
		Vector3 vector32 = vector31 - vector3;
		vector32.Normalize();
		float single = 1f - (1f - Mathf.Abs(vector32.z)) * beam.beamOutput;
		Quaternion quaternion = Quaternion.LookRotation(vector32, vector31);
		Quaternion quaternion1 = Quaternion.LookRotation(vector32, vector3);
		Vector3 vector33 = quaternion1 * new Vector3(frame.originWidth, 0f, 0f);
		Vector3 vector34 = quaternion * new Vector3(frame.pointWidth, 0f, 0f);
		frame.beamVertices.m0 = LaserGraphics.cam2World.MultiplyPoint((vector33 * 0.5f) + vector3);
		frame.beamVertices.m2 = LaserGraphics.cam2World.MultiplyPoint((vector34 * 0.5f) + vector31);
		frame.beamVertices.m1 = LaserGraphics.cam2World.MultiplyPoint((vector33 * -0.5f) + vector3);
		frame.beamVertices.m3 = LaserGraphics.cam2World.MultiplyPoint((vector34 * -0.5f) + vector31);
		frame.beamNormals.m0.x = frame.originWidth;
		frame.beamNormals.m2.x = frame.pointWidth;
		frame.beamNormals.m1.x = -frame.originWidth;
		frame.beamNormals.m3.x = -frame.pointWidth;
		frame.beamNormals.m0.y = -frame.distance;
		frame.beamNormals.m1.y = -frame.distance;
		frame.beamNormals.m2.y = -frame.distance;
		frame.beamNormals.m3.y = -frame.distance;
		float single1 = 0f;
		float single2 = single1;
		frame.beamNormals.m1.z = single1;
		frame.beamNormals.m0.z = single2;
		float single3 = frame.distanceFraction;
		single2 = single3;
		frame.beamNormals.m3.z = single3;
		frame.beamNormals.m2.z = single2;
		Color color = LaserGraphics.RangeBeamColor(beam.beamColor * single);
		Color color1 = color;
		frame.beamColor.m3 = (T)color;
		Color color2 = color1;
		color1 = color2;
		frame.beamColor.m2 = (T)color2;
		Color color3 = color1;
		color1 = color3;
		frame.beamColor.m1 = (T)color3;
		frame.beamColor.m0 = color1;
		frame.beamUVs.m0 = LaserGraphics.uv[0];
		frame.beamUVs.m0.x = frame.beamUVs.m0.x * frame.distanceFraction;
		frame.beamUVs.m1 = LaserGraphics.uv[1];
		frame.beamUVs.m1.x = frame.beamUVs.m1.x * frame.distanceFraction;
		frame.beamUVs.m2 = LaserGraphics.uv[2];
		frame.beamUVs.m2.x = frame.beamUVs.m2.x * frame.distanceFraction;
		frame.beamUVs.m3 = LaserGraphics.uv[3];
		frame.beamUVs.m3.x = frame.beamUVs.m3.x * frame.distanceFraction;
		frame.bufBeam = LaserGraphics.MeshBuffer.ForBeamMaterial(beam.beamMaterial);
		if (!LaserGraphics.Computation.beams.Add(frame.bufBeam))
		{
			LaserGraphics.MeshBuffer meshBuffer = frame.bufBeam;
			meshBuffer.measureSize = meshBuffer.measureSize + 1;
		}
		else
		{
			frame.bufBeam.measureSize = 1;
		}
		frame.bufBeam.beams.Add(beam);
		if (!frame.didHit)
		{
			frame.bufDot = null;
			frame.drawDot = false;
		}
		else
		{
			Vector3 vector35 = LaserGraphics.world2Cam.MultiplyVector(-frame.hitNormal);
			if (vector35.z >= 0f)
			{
				frame.bufDot = null;
				frame.drawDot = false;
			}
			else
			{
				Vector3 vector36 = LaserGraphics.cam2World.MultiplyPoint(Vector3.zero);
				if (Physics.Linecast(vector36, Vector3.Lerp(vector36, frame.point, 0.95f), beam.cullLayers))
				{
					frame.bufDot = null;
					frame.drawDot = false;
				}
				else
				{
					Vector3 vector37 = LaserGraphics.world2Cam.MultiplyPoint(frame.point);
					Quaternion quaternion2 = Quaternion.LookRotation(vector37, Vector3.up);
					frame.dotVertices1.m0 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(frame.dotRadius, -frame.dotRadius, 0f)));
					frame.dotVertices1.m1 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(frame.dotRadius, frame.dotRadius, 0f)));
					frame.dotVertices1.m2 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(-frame.dotRadius, -frame.dotRadius, 0f)));
					frame.dotVertices1.m3 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(-frame.dotRadius, frame.dotRadius, 0f)));
					quaternion2 = Quaternion.LookRotation(vector35, Vector3.up);
					frame.dotVertices2.m0 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(frame.dotRadius, -frame.dotRadius, -0.01f)));
					frame.dotVertices2.m1 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(frame.dotRadius, frame.dotRadius, -0.01f)));
					frame.dotVertices2.m2 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(-frame.dotRadius, -frame.dotRadius, -0.01f)));
					frame.dotVertices2.m3 = LaserGraphics.cam2World.MultiplyPoint(vector37 + (quaternion2 * new Vector3(-frame.dotRadius, frame.dotRadius, -0.01f)));
					Color color4 = LaserGraphics.RangeDotColor(beam.dotColor);
					color1 = color4;
					frame.dotColor2.m3 = (T)color4;
					Color color5 = color1;
					color1 = color5;
					frame.dotColor2.m2 = (T)color5;
					Color color6 = color1;
					color1 = color6;
					frame.dotColor2.m1 = (T)color6;
					Color color7 = color1;
					color1 = color7;
					frame.dotColor2.m0 = (T)color7;
					Color color8 = color1;
					color1 = color8;
					frame.dotColor1.m3 = (T)color8;
					Color color9 = color1;
					color1 = color9;
					frame.dotColor1.m2 = (T)color9;
					Color color10 = color1;
					color1 = color10;
					frame.dotColor1.m1 = (T)color10;
					frame.dotColor1.m0 = color1;
					frame.bufDot = LaserGraphics.MeshBuffer.ForDotMaterial(beam.dotMaterial);
					if (!LaserGraphics.Computation.dots.Add(frame.bufDot))
					{
						LaserGraphics.MeshBuffer meshBuffer1 = frame.bufDot;
						meshBuffer1.measureSize = meshBuffer1.measureSize + 2;
					}
					else
					{
						frame.bufDot.measureSize = 2;
					}
					frame.bufDot.beams.Add(beam);
					frame.drawDot = true;
				}
			}
		}
	}

	private void RenderLasers(Camera camera)
	{
		Vector3 vector3 = new Vector3();
		Vector3 vector31 = new Vector3();
		Vector3 vector32 = new Vector3();
		Vector3 vector33 = new Vector3();
		float single;
		if (!this.madeLists)
		{
			this.beams = new List<LaserBeam>();
			this.willRender = new List<LaserBeam>();
			this.madeLists = true;
		}
		int num = camera.cullingMask;
		if (this.beams != null)
		{
			this.beams.Clear();
			this.beams.AddRange(LaserBeam.Collect());
		}
		else
		{
			this.beams = new List<LaserBeam>(LaserBeam.Collect());
		}
		LaserGraphics.allBeamsMask = 0;
		foreach (LaserBeam beam in this.beams)
		{
			LaserGraphics.UpdateBeam(ref beam.frame, beam);
		}
		if ((num & LaserGraphics.allBeamsMask) != 0 && this.beams.Count > 0)
		{
			Plane[] planeArray = GeometryUtility.CalculateFrustumPlanes(camera);
			foreach (LaserBeam laserBeam in this.beams)
			{
				if (!laserBeam.isViewModel && ((num & laserBeam.frame.beamsLayer) != laserBeam.frame.beamsLayer || !GeometryUtility.TestPlanesAABB(planeArray, laserBeam.frame.bounds)))
				{
					continue;
				}
				this.willRender.Add(laserBeam);
			}
			if (this.willRender.Count > 0)
			{
				LaserGraphics.world2Cam = camera.worldToCameraMatrix;
				LaserGraphics.cam2World = camera.cameraToWorldMatrix;
				LaserGraphics.camProj = camera.projectionMatrix;
				try
				{
					foreach (LaserBeam laserBeam1 in this.willRender)
					{
						LaserGraphics.RenderBeam(planeArray, camera, laserBeam1, ref laserBeam1.frame);
					}
					foreach (LaserGraphics.MeshBuffer meshBuffer in LaserGraphics.Computation.beams)
					{
						bool flag = meshBuffer.Resize();
						int num1 = 0;
						LaserGraphics.VertexBuffer vertexBuffer = meshBuffer.buffer;
						float single1 = Single.PositiveInfinity;
						single = single1;
						vector3.z = single1;
						float single2 = single;
						single = single2;
						vector3.y = single2;
						vector3.x = single;
						float single3 = Single.NegativeInfinity;
						single = single3;
						vector31.z = single3;
						float single4 = single;
						single = single4;
						vector31.y = single4;
						vector31.x = single;
						foreach (LaserBeam beam1 in meshBuffer.beams)
						{
							int num2 = num1;
							num1 = num2 + 1;
							int num3 = num2;
							int num4 = num1;
							num1 = num4 + 1;
							int num5 = num4;
							int num6 = num1;
							num1 = num6 + 1;
							int num7 = num6;
							int num8 = num1;
							num1 = num8 + 1;
							int num9 = num8;
							vertexBuffer.v[num3] = beam1.frame.beamVertices.m0;
							vertexBuffer.v[num5] = beam1.frame.beamVertices.m1;
							vertexBuffer.v[num9] = beam1.frame.beamVertices.m2;
							vertexBuffer.v[num7] = beam1.frame.beamVertices.m3;
							vertexBuffer.n[num3] = beam1.frame.beamNormals.m0;
							vertexBuffer.n[num5] = beam1.frame.beamNormals.m1;
							vertexBuffer.n[num9] = beam1.frame.beamNormals.m2;
							vertexBuffer.n[num7] = beam1.frame.beamNormals.m3;
							vertexBuffer.c[num3] = beam1.frame.beamColor.m0;
							vertexBuffer.c[num5] = beam1.frame.beamColor.m1;
							vertexBuffer.c[num9] = beam1.frame.beamColor.m2;
							vertexBuffer.c[num7] = beam1.frame.beamColor.m3;
							vertexBuffer.t[num3] = beam1.frame.beamUVs.m0;
							vertexBuffer.t[num5] = beam1.frame.beamUVs.m1;
							vertexBuffer.t[num9] = beam1.frame.beamUVs.m2;
							vertexBuffer.t[num7] = beam1.frame.beamUVs.m3;
							for (int i = num3; i <= num7; i++)
							{
								if (vertexBuffer.v[i].x < vector3.x)
								{
									vector3.x = vertexBuffer.v[i].x;
								}
								if (vertexBuffer.v[i].x > vector31.x)
								{
									vector31.x = vertexBuffer.v[i].x;
								}
								if (vertexBuffer.v[i].y < vector3.y)
								{
									vector3.y = vertexBuffer.v[i].y;
								}
								if (vertexBuffer.v[i].y > vector31.y)
								{
									vector31.y = vertexBuffer.v[i].y;
								}
								if (vertexBuffer.v[i].z < vector3.z)
								{
									vector3.z = vertexBuffer.v[i].z;
								}
								if (vertexBuffer.v[i].z > vector31.z)
								{
									vector31.z = vertexBuffer.v[i].z;
								}
							}
							beam1.frame.bufBeam = null;
						}
						meshBuffer.beams.Clear();
						meshBuffer.BindMesh(flag, vector3, vector31);
						Graphics.DrawMesh(meshBuffer.mesh, Matrix4x4.identity, meshBuffer.material, 1, camera, 0, null, false, false);
					}
					foreach (LaserGraphics.MeshBuffer dot in LaserGraphics.Computation.dots)
					{
						bool flag1 = dot.Resize();
						int num10 = 0;
						LaserGraphics.VertexBuffer vertexBuffer1 = dot.buffer;
						float single5 = Single.PositiveInfinity;
						single = single5;
						vector32.z = single5;
						float single6 = single;
						single = single6;
						vector32.y = single6;
						vector32.x = single;
						float single7 = Single.NegativeInfinity;
						single = single7;
						vector33.z = single7;
						float single8 = single;
						single = single8;
						vector33.y = single8;
						vector33.x = single;
						foreach (LaserBeam beam2 in dot.beams)
						{
							int num11 = num10;
							num10 = num11 + 1;
							int num12 = num11;
							int num13 = num10;
							num10 = num13 + 1;
							int num14 = num13;
							int num15 = num10;
							num10 = num15 + 1;
							int num16 = num15;
							int num17 = num10;
							num10 = num17 + 1;
							int num18 = num17;
							vertexBuffer1.v[num12] = beam2.frame.dotVertices1.m0;
							vertexBuffer1.v[num14] = beam2.frame.dotVertices1.m1;
							vertexBuffer1.v[num18] = beam2.frame.dotVertices1.m2;
							vertexBuffer1.v[num16] = beam2.frame.dotVertices1.m3;
							vertexBuffer1.n[num12] = beam2.frame.beamNormals.m0;
							vertexBuffer1.n[num14] = beam2.frame.beamNormals.m1;
							vertexBuffer1.n[num18] = beam2.frame.beamNormals.m2;
							vertexBuffer1.n[num16] = beam2.frame.beamNormals.m3;
							vertexBuffer1.c[num12] = beam2.frame.dotColor1.m0;
							vertexBuffer1.c[num14] = beam2.frame.dotColor1.m1;
							vertexBuffer1.c[num18] = beam2.frame.dotColor1.m2;
							vertexBuffer1.c[num16] = beam2.frame.dotColor1.m3;
							vertexBuffer1.t[num12] = LaserGraphics.uv[0];
							vertexBuffer1.t[num14] = LaserGraphics.uv[1];
							vertexBuffer1.t[num18] = LaserGraphics.uv[2];
							vertexBuffer1.t[num16] = LaserGraphics.uv[3];
							for (int j = num12; j <= num16; j++)
							{
								if (vertexBuffer1.v[j].x < vector32.x)
								{
									vector32.x = vertexBuffer1.v[j].x;
								}
								if (vertexBuffer1.v[j].x > vector33.x)
								{
									vector33.x = vertexBuffer1.v[j].x;
								}
								if (vertexBuffer1.v[j].y < vector32.y)
								{
									vector32.y = vertexBuffer1.v[j].y;
								}
								if (vertexBuffer1.v[j].y > vector33.y)
								{
									vector33.y = vertexBuffer1.v[j].y;
								}
								if (vertexBuffer1.v[j].z < vector32.z)
								{
									vector32.z = vertexBuffer1.v[j].z;
								}
								if (vertexBuffer1.v[j].z > vector33.z)
								{
									vector33.z = vertexBuffer1.v[j].z;
								}
							}
							int num19 = num10;
							num10 = num19 + 1;
							num12 = num19;
							int num20 = num10;
							num10 = num20 + 1;
							num14 = num20;
							int num21 = num10;
							num10 = num21 + 1;
							num16 = num21;
							int num22 = num10;
							num10 = num22 + 1;
							num18 = num22;
							vertexBuffer1.v[num12] = beam2.frame.dotVertices2.m0;
							vertexBuffer1.v[num14] = beam2.frame.dotVertices2.m1;
							vertexBuffer1.v[num18] = beam2.frame.dotVertices2.m2;
							vertexBuffer1.v[num16] = beam2.frame.dotVertices2.m3;
							vertexBuffer1.n[num12] = beam2.frame.beamNormals.m0;
							vertexBuffer1.n[num14] = beam2.frame.beamNormals.m1;
							vertexBuffer1.n[num18] = beam2.frame.beamNormals.m2;
							vertexBuffer1.n[num16] = beam2.frame.beamNormals.m3;
							vertexBuffer1.c[num12] = beam2.frame.dotColor2.m0;
							vertexBuffer1.c[num14] = beam2.frame.dotColor2.m1;
							vertexBuffer1.c[num18] = beam2.frame.dotColor2.m2;
							vertexBuffer1.c[num16] = beam2.frame.dotColor2.m3;
							vertexBuffer1.t[num12] = LaserGraphics.uv[0];
							vertexBuffer1.t[num14] = LaserGraphics.uv[1];
							vertexBuffer1.t[num18] = LaserGraphics.uv[2];
							vertexBuffer1.t[num16] = LaserGraphics.uv[3];
							for (int k = num12; k <= num16; k++)
							{
								if (vertexBuffer1.v[k].x < vector32.x)
								{
									vector32.x = vertexBuffer1.v[k].x;
								}
								if (vertexBuffer1.v[k].x > vector33.x)
								{
									vector33.x = vertexBuffer1.v[k].x;
								}
								if (vertexBuffer1.v[k].y < vector32.y)
								{
									vector32.y = vertexBuffer1.v[k].y;
								}
								if (vertexBuffer1.v[k].y > vector33.y)
								{
									vector33.y = vertexBuffer1.v[k].y;
								}
								if (vertexBuffer1.v[k].z < vector32.z)
								{
									vector32.z = vertexBuffer1.v[k].z;
								}
								if (vertexBuffer1.v[k].z > vector33.z)
								{
									vector33.z = vertexBuffer1.v[k].z;
								}
							}
							beam2.frame.bufDot = null;
						}
						dot.beams.Clear();
						if (!flag1)
						{
							dot.mesh.vertices = vertexBuffer1.v;
							dot.mesh.normals = vertexBuffer1.n;
							dot.mesh.colors = vertexBuffer1.c;
							dot.mesh.uv = vertexBuffer1.t;
						}
						else
						{
							dot.mesh.Clear(false);
							dot.mesh.vertices = vertexBuffer1.v;
							dot.mesh.normals = vertexBuffer1.n;
							dot.mesh.colors = vertexBuffer1.c;
							dot.mesh.uv = vertexBuffer1.t;
							dot.mesh.SetIndices(vertexBuffer1.i, MeshTopology.Quads, 0);
						}
						dot.BindMesh(flag1, vector32, vector33);
						Graphics.DrawMesh(dot.mesh, Matrix4x4.identity, dot.material, 1, camera, 0, null, false, false);
					}
				}
				finally
				{
					this.willRender.Clear();
					LaserGraphics.Computation.beams.Clear();
					LaserGraphics.Computation.dots.Clear();
					LaserGraphics.MeshBuffer.Reset();
				}
			}
		}
	}

	internal static void RenderLasersOnCamera(Camera camera)
	{
		if (LaserGraphics.singleton)
		{
			LaserGraphics.singleton.RenderLasers(camera);
		}
	}

	private static void UpdateBeam(ref LaserBeam.FrameData frame, LaserBeam beam)
	{
		RaycastHit2 raycastHit2;
		RaycastHit raycastHit;
		Vector3 vector3 = new Vector3();
		bool flag;
		Transform transforms = beam.transform;
		frame.origin = transforms.position;
		frame.direction = transforms.forward;
		frame.direction.Normalize();
		int num = beam.beamLayers;
		if (num == 0)
		{
			frame.hit = false;
		}
		else if (!beam.isViewModel)
		{
			bool flag1 = Physics.Raycast(frame.origin, frame.direction, out raycastHit, beam.beamMaxDistance, num);
			flag = flag1;
			frame.hit = flag1;
			if (flag)
			{
				frame.hitPoint = raycastHit.point;
				frame.hitNormal = raycastHit.normal;
			}
		}
		else
		{
			bool flag2 = Physics2.Raycast2(frame.origin, frame.direction, out raycastHit2, beam.beamMaxDistance, num);
			flag = flag2;
			frame.hit = flag2;
			if (flag)
			{
				frame.hitPoint = raycastHit2.point;
				frame.hitNormal = raycastHit2.normal;
			}
		}
		if (frame.hit)
		{
			frame.point = frame.hitPoint;
			frame.didHit = true;
			frame.distance = frame.direction.x * frame.point.x + frame.direction.y * frame.point.y + frame.direction.z * frame.point.z - (frame.direction.x * frame.origin.x + frame.direction.y * frame.origin.y + frame.direction.z * frame.origin.z);
			frame.distanceFraction = frame.distance / beam.beamMaxDistance;
			frame.pointWidth = Mathf.Lerp(beam.beamWidthStart, beam.beamWidthEnd, frame.distanceFraction);
			frame.dotRadius = Mathf.Lerp(beam.dotRadiusStart, beam.dotRadiusEnd, frame.distanceFraction);
		}
		else
		{
			frame.didHit = false;
			frame.point.x = frame.origin.x + frame.direction.x * beam.beamMaxDistance;
			frame.point.y = frame.origin.y + frame.direction.y * beam.beamMaxDistance;
			frame.point.z = frame.origin.z + frame.direction.z * beam.beamMaxDistance;
			frame.distance = beam.beamMaxDistance;
			frame.distanceFraction = 1f;
			frame.pointWidth = beam.beamWidthEnd;
		}
		frame.originWidth = beam.beamWidthStart;
		float single = frame.originWidth;
		float single1 = single;
		vector3.z = single;
		float single2 = single1;
		single1 = single2;
		vector3.y = single2;
		vector3.x = single1;
		frame.bounds = new Bounds(frame.origin, vector3);
		float single3 = frame.pointWidth;
		single1 = single3;
		vector3.z = single3;
		float single4 = single1;
		single1 = single4;
		vector3.y = single4;
		vector3.x = single1;
		frame.bounds.Encapsulate(new Bounds(frame.point, vector3));
		frame.beamsLayer = 1 << (beam.gameObject.layer & 31);
		LaserGraphics.allBeamsMask = LaserGraphics.allBeamsMask | frame.beamsLayer;
	}

	private static class Computation
	{
		public readonly static HashSet<LaserGraphics.MeshBuffer> beams;

		public readonly static HashSet<LaserGraphics.MeshBuffer> dots;

		static Computation()
		{
			LaserGraphics.Computation.beams = new HashSet<LaserGraphics.MeshBuffer>();
			LaserGraphics.Computation.dots = new HashSet<LaserGraphics.MeshBuffer>();
		}
	}

	internal sealed class MeshBuffer : IDisposable, IEquatable<LaserGraphics.MeshBuffer>
	{
		public Mesh mesh;

		public readonly Material material;

		private int quadCount;

		internal LaserGraphics.VertexBuffer buffer;

		public int measureSize;

		private readonly int instanceID;

		public readonly List<LaserBeam> beams;

		private MeshBuffer(Material material)
		{
			this.instanceID = material.GetInstanceID();
			this.mesh = new Mesh()
			{
				hideFlags = HideFlags.DontSave
			};
			this.mesh.MarkDynamic();
			this.material = material;
		}

		public void BindMesh(bool rebindVertexLayout, Vector3 min, Vector3 max)
		{
			if (!rebindVertexLayout)
			{
				this.mesh.vertices = this.buffer.v;
				this.mesh.normals = this.buffer.n;
				this.mesh.colors = this.buffer.c;
				this.mesh.uv = this.buffer.t;
			}
			else
			{
				this.mesh.Clear(false);
				this.mesh.vertices = this.buffer.v;
				this.mesh.normals = this.buffer.n;
				this.mesh.colors = this.buffer.c;
				this.mesh.uv = this.buffer.t;
				this.mesh.SetIndices(this.buffer.i, MeshTopology.Quads, 0);
			}
			Bounds bound = new Bounds(Vector3.zero, Vector3.zero);
			bound.SetMinMax(min, max);
			this.mesh.bounds = bound;
		}

		public void Dispose()
		{
			if (this.mesh)
			{
				UnityEngine.Object.DestroyImmediate(this.mesh);
			}
		}

		public override bool Equals(object obj)
		{
			return (!(obj is LaserGraphics.MeshBuffer) ? false : this.instanceID == ((LaserGraphics.MeshBuffer)obj).instanceID);
		}

		public bool Equals(LaserGraphics.MeshBuffer buf)
		{
			return (object.ReferenceEquals(buf, null) ? false : this.instanceID == buf.instanceID);
		}

		public static LaserGraphics.MeshBuffer ForBeamMaterial(Material material)
		{
			if (!LaserGraphics.MeshBuffer.Register.hasBeam || LaserGraphics.MeshBuffer.Register.lastBeam.material != material)
			{
				LaserGraphics.MeshBuffer.Register.lastBeam = LaserGraphics.MeshBuffer.ForMaterial(LaserGraphics.MeshBuffer.Register.beams, material);
				LaserGraphics.MeshBuffer.Register.hasBeam = true;
			}
			return LaserGraphics.MeshBuffer.Register.lastBeam;
		}

		public static LaserGraphics.MeshBuffer ForDotMaterial(Material material)
		{
			if (!LaserGraphics.MeshBuffer.Register.hasDot || LaserGraphics.MeshBuffer.Register.lastDot.material != material)
			{
				LaserGraphics.MeshBuffer.Register.lastDot = LaserGraphics.MeshBuffer.ForMaterial(LaserGraphics.MeshBuffer.Register.dots, material);
				LaserGraphics.MeshBuffer.Register.hasDot = true;
			}
			return LaserGraphics.MeshBuffer.Register.lastDot;
		}

		private static LaserGraphics.MeshBuffer ForMaterial(Dictionary<Material, LaserGraphics.MeshBuffer> all, Material material)
		{
			LaserGraphics.MeshBuffer meshBuffer;
			if (!all.TryGetValue(material, out meshBuffer))
			{
				meshBuffer = new LaserGraphics.MeshBuffer(material);
				all.Add(material, meshBuffer);
			}
			return meshBuffer;
		}

		public override int GetHashCode()
		{
			return this.instanceID;
		}

		public static void Reset()
		{
			object obj = null;
			LaserGraphics.MeshBuffer.Register.lastBeam = (LaserGraphics.MeshBuffer)obj;
			LaserGraphics.MeshBuffer.Register.lastDot = (LaserGraphics.MeshBuffer)obj;
			int num = 0;
			LaserGraphics.MeshBuffer.Register.hasBeam = (bool)num;
			LaserGraphics.MeshBuffer.Register.hasDot = (bool)num;
		}

		public bool Resize()
		{
			return this.SetSize(this.measureSize);
		}

		private bool SetSize(int size)
		{
			if (this.quadCount == size)
			{
				return false;
			}
			if (size != 0)
			{
				this.buffer = LaserGraphics.VertexBuffer.Size(size);
			}
			else
			{
				this.buffer = null;
			}
			this.quadCount = size;
			return true;
		}

		private static class Register
		{
			public readonly static Dictionary<Material, LaserGraphics.MeshBuffer> beams;

			public readonly static Dictionary<Material, LaserGraphics.MeshBuffer> dots;

			public static LaserGraphics.MeshBuffer lastBeam;

			public static LaserGraphics.MeshBuffer lastDot;

			public static bool hasBeam;

			public static bool hasDot;

			static Register()
			{
				LaserGraphics.MeshBuffer.Register.beams = new Dictionary<Material, LaserGraphics.MeshBuffer>();
				LaserGraphics.MeshBuffer.Register.dots = new Dictionary<Material, LaserGraphics.MeshBuffer>();
			}
		}
	}

	internal class VertexBuffer
	{
		public readonly int quadCount;

		public readonly int vertexCount;

		public readonly Vector3[] v;

		public readonly Vector2[] t;

		public readonly Vector3[] n;

		public readonly Color[] c;

		public readonly int[] i;

		private VertexBuffer(int quadCount)
		{
			this.quadCount = quadCount;
			this.vertexCount = quadCount * 4;
			if (this.vertexCount > 0)
			{
				this.v = new Vector3[this.vertexCount];
				this.t = new Vector2[this.vertexCount];
				this.n = new Vector3[this.vertexCount];
				this.c = new Color[this.vertexCount];
				this.i = new int[this.vertexCount];
			}
			for (int i = 0; i < this.vertexCount; i++)
			{
				this.i[i] = i;
			}
		}

		public static LaserGraphics.VertexBuffer Size(int i)
		{
			LaserGraphics.VertexBuffer vertexBuffer;
			if (!LaserGraphics.VertexBuffer.Register.all.TryGetValue(i, out vertexBuffer))
			{
				Dictionary<int, LaserGraphics.VertexBuffer> nums = LaserGraphics.VertexBuffer.Register.all;
				LaserGraphics.VertexBuffer vertexBuffer1 = new LaserGraphics.VertexBuffer(i);
				vertexBuffer = vertexBuffer1;
				nums.Add(i, vertexBuffer1);
			}
			return vertexBuffer;
		}

		private static class Register
		{
			public readonly static Dictionary<int, LaserGraphics.VertexBuffer> all;

			static Register()
			{
				LaserGraphics.VertexBuffer.Register.all = new Dictionary<int, LaserGraphics.VertexBuffer>();
			}
		}
	}
}