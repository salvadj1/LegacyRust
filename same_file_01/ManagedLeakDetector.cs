using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class ManagedLeakDetector : MonoBehaviour
{
	private Vector2 scroll;

	public ManagedLeakDetector()
	{
	}

	private static bool CheckRelation(Type a, Type b)
	{
		return (a.IsAssignableFrom(b) ? true : b.IsAssignableFrom(a));
	}

	private void OnGUI()
	{
		if (Event.current.type == EventType.Repaint)
		{
			if (!Camera.main)
			{
				GUI.Box(new Rect(-5f, -5f, (float)(Screen.width + 10), (float)(Screen.height + 10)), GUIContent.none);
			}
			ManagedLeakDetector.ReadResult readResult = new ManagedLeakDetector.ReadResult();
			readResult.Read();
			ManagedLeakDetector.Counter[] counterArray = readResult.counters;
			float single = (float)(Screen.width - 10);
			this.scroll = GUI.BeginScrollView(new Rect(5f, 5f, single, (float)(Screen.height - 10)), this.scroll, new Rect(0f, 0f, single, (float)((int)counterArray.Length * 20)));
			int num = 0;
			ManagedLeakDetector.Counter[] counterArray1 = counterArray;
			for (int i = 0; i < (int)counterArray1.Length; i++)
			{
				ManagedLeakDetector.Counter counter = counterArray1[i];
				GUI.Label(new Rect(0f, (float)num, single, 20f), string.Format("{0:000} [{1:0000}] {2}", counter.actualInstanceCount, counter.derivedInstanceCount, counter.type));
				num = num + 20;
			}
		}
	}

	public static string Poll()
	{
		return ManagedLeakDetector.Poll(typeof(UnityEngine.Object));
	}

	public static string Poll(Type searchType)
	{
		return ManagedLeakDetector.Poll(searchType, typeof(UnityEngine.Object));
	}

	public static string Poll(Type searchType, Type minType)
	{
		return (new ManagedLeakDetector.ReadResult(searchType, minType)).ToString();
	}

	private class Counter
	{
		public int actualInstanceCount;

		public int derivedInstanceCount;

		public int enabledCount;

		public Type type;

		public Counter()
		{
		}
	}

	private class ReadResult
	{
		public ManagedLeakDetector.SumEnable sumComponent;

		public ManagedLeakDetector.SumEnable sumBehaviour;

		public ManagedLeakDetector.SumEnable sumRenderer;

		public ManagedLeakDetector.SumEnable sumCollider;

		public ManagedLeakDetector.SumEnable sumCloth;

		public ManagedLeakDetector.SumEnable sumGameObject;

		public ManagedLeakDetector.SumEnable sumScriptableObject;

		public ManagedLeakDetector.SumEnable sumMaterial;

		public ManagedLeakDetector.SumEnable sumTexture;

		public ManagedLeakDetector.SumEnable sumAnimation;

		public ManagedLeakDetector.SumEnable sumMesh;

		public ManagedLeakDetector.SumEnable sumAudioClip;

		public ManagedLeakDetector.SumEnable sumAnimationClip;

		public ManagedLeakDetector.SumEnable sumParticleEmitter;

		public ManagedLeakDetector.SumEnable sumParticleSystem;

		public bool complete;

		public ManagedLeakDetector.Counter[] counters;

		public readonly Type searchType;

		public readonly Type minType;

		public ReadResult(Type searchType, Type minType)
		{
			bool flag;
			bool flag1;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			bool flag6;
			this.minType = minType ?? typeof(UnityEngine.Object);
			this.searchType = searchType ?? typeof(UnityEngine.Object);
			this.sumComponent.name = "Components";
			this.sumBehaviour.name = "Behaviours";
			this.sumRenderer.name = "Renderers";
			this.sumCollider.name = "Colliders";
			this.sumCloth.name = "Cloths";
			this.sumGameObject.name = "Game Objects";
			this.sumScriptableObject.name = "Scriptable Objects";
			this.sumMaterial.name = "Materials";
			this.sumTexture.name = "Textures";
			this.sumAnimation.name = "Animations";
			this.sumMesh.name = "Meshes";
			this.sumAudioClip.name = "Audio Clips";
			this.sumAnimationClip.name = "Animation Clips";
			this.sumParticleEmitter.name = "Particle Emitters (Legacy)";
			this.sumParticleSystem.name = "Particle Systems";
			this.sumComponent.check = ManagedLeakDetector.CheckRelation(searchType, typeof(Component));
			flag = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Behaviour), searchType));
			this.sumBehaviour.check = flag;
			flag1 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Renderer), searchType));
			this.sumRenderer.check = flag1;
			flag2 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Collider), searchType));
			this.sumCollider.check = flag2;
			flag3 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Cloth), searchType));
			this.sumCloth.check = flag3;
			flag4 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(ParticleSystem), searchType));
			this.sumParticleSystem.check = flag4;
			flag5 = (!this.sumBehaviour.check ? false : ManagedLeakDetector.CheckRelation(typeof(Animation), searchType));
			this.sumAnimation.check = flag5;
			flag6 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(ParticleEmitter), searchType));
			this.sumParticleEmitter.check = flag6;
			this.sumGameObject.check = ManagedLeakDetector.CheckRelation(typeof(GameObject), searchType);
			this.sumScriptableObject.check = ManagedLeakDetector.CheckRelation(typeof(ScriptableObject), searchType);
			this.sumMaterial.check = ManagedLeakDetector.CheckRelation(typeof(Material), searchType);
			this.sumTexture.check = ManagedLeakDetector.CheckRelation(typeof(Texture), searchType);
			this.sumMesh.check = ManagedLeakDetector.CheckRelation(typeof(Mesh), searchType);
			this.sumAudioClip.check = ManagedLeakDetector.CheckRelation(typeof(AudioClip), searchType);
			this.sumAnimationClip.check = ManagedLeakDetector.CheckRelation(typeof(AnimationClip), searchType);
		}

		public ReadResult(Type searchType) : this(searchType, typeof(UnityEngine.Object))
		{
		}

		public ReadResult() : this(typeof(UnityEngine.Object))
		{
		}

		private static void Print(StringBuilder sb, ref ManagedLeakDetector.SumEnable en)
		{
			if (en.check)
			{
				if (en.enabled != 0)
				{
					sb.AppendFormat("{0} {1} ({2})\r\n", en.name, en.total, en.enabled);
				}
				else if (en.total != 0)
				{
					sb.AppendFormat("{0} {1}\r\n", en.name, en.total);
				}
			}
		}

		public void Read()
		{
			this.Read(false);
		}

		public void Read(bool forceUpdate)
		{
			ManagedLeakDetector.Counter counter;
			ManagedLeakDetector.Counter counter1;
			bool flag;
			bool flag1;
			bool flag2;
			bool flag3;
			bool flag4;
			bool flag5;
			bool flag6;
			if (this.complete && !forceUpdate)
			{
				return;
			}
			Dictionary<Type, ManagedLeakDetector.Counter> types = new Dictionary<Type, ManagedLeakDetector.Counter>();
			ManagedLeakDetector.Counter counter2 = new ManagedLeakDetector.Counter()
			{
				type = this.minType
			};
			types.Add(this.minType, counter2);
			this.sumComponent.Reset();
			this.sumBehaviour.Reset();
			this.sumRenderer.Reset();
			this.sumCollider.Reset();
			this.sumCloth.Reset();
			this.sumGameObject.Reset();
			this.sumScriptableObject.Reset();
			this.sumMaterial.Reset();
			this.sumTexture.Reset();
			this.sumAnimation.Reset();
			this.sumMesh.Reset();
			this.sumAudioClip.Reset();
			this.sumAnimationClip.Reset();
			this.sumParticleSystem.Reset();
			this.sumParticleEmitter.Reset();
			this.sumComponent.check = ManagedLeakDetector.CheckRelation(this.searchType, typeof(Component));
			flag = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Behaviour), this.searchType));
			this.sumBehaviour.check = flag;
			flag1 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Renderer), this.searchType));
			this.sumRenderer.check = flag1;
			flag2 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Collider), this.searchType));
			this.sumCollider.check = flag2;
			flag3 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(Cloth), this.searchType));
			this.sumCloth.check = flag3;
			flag4 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(ParticleSystem), this.searchType));
			this.sumParticleSystem.check = flag4;
			flag5 = (!this.sumBehaviour.check ? false : ManagedLeakDetector.CheckRelation(typeof(Animation), this.searchType));
			this.sumAnimation.check = flag5;
			flag6 = (!this.sumComponent.check ? false : ManagedLeakDetector.CheckRelation(typeof(ParticleEmitter), this.searchType));
			this.sumParticleEmitter.check = flag6;
			this.sumGameObject.check = ManagedLeakDetector.CheckRelation(typeof(GameObject), this.searchType);
			this.sumScriptableObject.check = ManagedLeakDetector.CheckRelation(typeof(ScriptableObject), this.searchType);
			this.sumMaterial.check = ManagedLeakDetector.CheckRelation(typeof(Material), this.searchType);
			this.sumTexture.check = ManagedLeakDetector.CheckRelation(typeof(Texture), this.searchType);
			this.sumMesh.check = ManagedLeakDetector.CheckRelation(typeof(Mesh), this.searchType);
			this.sumAudioClip.check = ManagedLeakDetector.CheckRelation(typeof(AudioClip), this.searchType);
			this.sumAnimationClip.check = ManagedLeakDetector.CheckRelation(typeof(AnimationClip), this.searchType);
			UnityEngine.Object[] objArray = UnityEngine.Object.FindObjectsOfType(this.searchType);
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				UnityEngine.Object obj = objArray[i];
				Type type = obj.GetType();
				if (!types.TryGetValue(type, out counter))
				{
					counter1 = new ManagedLeakDetector.Counter()
					{
						type = type,
						actualInstanceCount = 1
					};
					ManagedLeakDetector.Counter counter3 = counter1;
					counter = counter3;
					types.Add(type, counter3);
				}
				else
				{
					ManagedLeakDetector.Counter counter4 = counter;
					counter4.actualInstanceCount = counter4.actualInstanceCount + 1;
				}
				if (this.sumComponent.check && typeof(Component).IsAssignableFrom(type))
				{
					this.sumComponent.total = this.sumComponent.total + 1;
					if (this.sumBehaviour.check && typeof(Behaviour).IsAssignableFrom(type))
					{
						if (((Behaviour)obj).enabled)
						{
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
							ManagedLeakDetector.Counter counter5 = counter;
							counter5.enabledCount = counter5.enabledCount + 1;
							this.sumBehaviour.enabled = this.sumBehaviour.enabled + 1;
							this.sumBehaviour.total = this.sumBehaviour.total + 1;
							if (this.sumAnimation.check && typeof(Animation).IsAssignableFrom(type))
							{
								this.sumAnimation.enabled = this.sumAnimation.enabled + 1;
								this.sumAnimation.total = this.sumAnimation.total + 1;
							}
						}
						else if (this.sumAnimation.check && typeof(Animation).IsAssignableFrom(type))
						{
							this.sumAnimation.total = this.sumAnimation.total + 1;
						}
					}
					else if (this.sumRenderer.check && typeof(Renderer).IsAssignableFrom(type))
					{
						this.sumRenderer.total = this.sumRenderer.total + 1;
						if (((Renderer)obj).enabled)
						{
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
							this.sumRenderer.enabled = this.sumRenderer.enabled + 1;
							ManagedLeakDetector.Counter counter6 = counter;
							counter6.enabledCount = counter6.enabledCount + 1;
						}
					}
					else if (this.sumCollider.check && typeof(Collider).IsAssignableFrom(type))
					{
						this.sumCollider.total = this.sumCollider.total + 1;
						if (((Collider)obj).enabled)
						{
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
							this.sumCollider.enabled = this.sumCollider.enabled + 1;
							ManagedLeakDetector.Counter counter7 = counter;
							counter7.enabledCount = counter7.enabledCount + 1;
						}
					}
					else if (this.sumParticleSystem.check && typeof(ParticleSystem).IsAssignableFrom(type))
					{
						this.sumParticleSystem.total = this.sumParticleSystem.total + 1;
						if (((ParticleSystem)obj).IsAlive())
						{
							ManagedLeakDetector.Counter counter8 = counter;
							counter8.enabledCount = counter8.enabledCount + 1;
							this.sumParticleSystem.enabled = this.sumParticleSystem.enabled + 1;
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
						}
					}
					else if (this.sumCloth.check && typeof(Cloth).IsAssignableFrom(type))
					{
						this.sumCloth.total = this.sumCloth.total + 1;
						if (((Cloth)obj).enabled)
						{
							ManagedLeakDetector.Counter counter9 = counter;
							counter9.enabledCount = counter9.enabledCount + 1;
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
							this.sumCloth.enabled = this.sumCloth.enabled + 1;
						}
					}
					else if (this.sumParticleEmitter.check && typeof(ParticleEmitter).IsAssignableFrom(type))
					{
						this.sumParticleEmitter.total = this.sumParticleEmitter.total + 1;
						if (((ParticleEmitter)obj).enabled)
						{
							ManagedLeakDetector.Counter counter10 = counter;
							counter10.enabledCount = counter10.enabledCount + 1;
							this.sumParticleEmitter.enabled = this.sumParticleEmitter.enabled + 1;
							this.sumComponent.enabled = this.sumComponent.enabled + 1;
						}
					}
				}
				else if (this.sumGameObject.check && typeof(GameObject).IsAssignableFrom(type))
				{
					this.sumGameObject.total = this.sumGameObject.total + 1;
					if (((GameObject)obj).activeInHierarchy)
					{
						this.sumGameObject.enabled = this.sumGameObject.enabled + 1;
						ManagedLeakDetector.Counter counter11 = counter;
						counter11.enabledCount = counter11.enabledCount + 1;
					}
				}
				else if (this.sumMaterial.check && typeof(Material).IsAssignableFrom(type))
				{
					this.sumMaterial.total = this.sumMaterial.total + 1;
				}
				else if (this.sumTexture.check && typeof(Texture).IsAssignableFrom(type))
				{
					this.sumTexture.total = this.sumTexture.total + 1;
				}
				else if (this.sumAudioClip.check && typeof(AudioClip).IsAssignableFrom(type))
				{
					this.sumAudioClip.total = this.sumAudioClip.total + 1;
				}
				else if (this.sumAnimationClip.check && typeof(AnimationClip).IsAssignableFrom(type))
				{
					this.sumAnimationClip.total = this.sumAnimationClip.total + 1;
				}
				else if (this.sumMesh.check && typeof(Mesh).IsAssignableFrom(type))
				{
					this.sumMesh.total = this.sumMesh.total + 1;
				}
				else if (this.sumScriptableObject.check && typeof(ScriptableObject).IsAssignableFrom(type))
				{
					this.sumScriptableObject.total = this.sumScriptableObject.total + 1;
				}
				if (type != this.minType)
				{
					for (type = type.BaseType; type != typeof(UnityEngine.Object); type = type.BaseType)
					{
						if (!types.TryGetValue(type, out counter))
						{
							counter1 = new ManagedLeakDetector.Counter()
							{
								type = type,
								derivedInstanceCount = 1
							};
							types.Add(type, counter1);
						}
						else
						{
							ManagedLeakDetector.Counter counter12 = counter;
							counter12.derivedInstanceCount = counter12.derivedInstanceCount + 1;
						}
					}
					ManagedLeakDetector.Counter counter13 = counter2;
					counter13.derivedInstanceCount = counter13.derivedInstanceCount + 1;
				}
			}
			List<ManagedLeakDetector.Counter> counters = new List<ManagedLeakDetector.Counter>(types.Values);
			counters.Sort((ManagedLeakDetector.Counter firstPair, ManagedLeakDetector.Counter nextPair) => {
				int num = nextPair.actualInstanceCount.CompareTo(firstPair.actualInstanceCount);
				if (num != 0)
				{
					return num;
				}
				return nextPair.derivedInstanceCount.CompareTo(firstPair.derivedInstanceCount);
			});
			this.counters = counters.ToArray();
			this.complete = true;
		}

		public override string ToString()
		{
			this.Read();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Instances, Deriving Instances, Type, (# Enabled [if not shown 0] )");
			ManagedLeakDetector.Counter[] counterArray = this.counters;
			for (int i = 0; i < (int)counterArray.Length; i++)
			{
				ManagedLeakDetector.Counter counter = counterArray[i];
				if (counter.enabledCount == 0)
				{
					stringBuilder.AppendFormat("{0,8} [{1,8}] {2}\r\n", counter.actualInstanceCount, counter.derivedInstanceCount, counter.type);
				}
				else
				{
					stringBuilder.AppendFormat("{0,8} [{1,8}] {2} ({3} enabled)\r\n", new object[] { counter.actualInstanceCount, counter.derivedInstanceCount, counter.type, counter.enabledCount });
				}
			}
			stringBuilder.AppendLine("basic counters: if not there, there is none.");
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumComponent);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumBehaviour);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumRenderer);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumCollider);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumCloth);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumGameObject);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumScriptableObject);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumMaterial);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumTexture);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumAnimation);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumMesh);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumAudioClip);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumAnimationClip);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumParticleSystem);
			ManagedLeakDetector.ReadResult.Print(stringBuilder, ref this.sumParticleEmitter);
			stringBuilder.AppendFormat("Count done for search {0} (min:{1})", this.searchType, this.minType);
			return stringBuilder.ToString();
		}
	}

	private struct SumEnable
	{
		public bool check;

		public int total;

		public int enabled;

		public string name;

		public Type type;

		public int disabled
		{
			get
			{
				return this.total - this.enabled;
			}
		}

		public void Reset()
		{
			this.total = 0;
			this.enabled = 0;
		}
	}
}