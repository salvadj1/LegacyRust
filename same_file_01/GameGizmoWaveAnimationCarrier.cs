using System;
using UnityEngine;

public class GameGizmoWaveAnimationCarrier : GameGizmoWaveAnimation
{
	[SerializeField]
	protected Material[] carrierMaterials;

	[SerializeField]
	protected bool hideArrowWhenCarrierExists;

	public GameGizmoWaveAnimationCarrier()
	{
	}

	protected override GameGizmo.Instance ConstructInstance()
	{
		return new GameGizmoWaveAnimationCarrier.Instance(this);
	}

	public class Instance : GameGizmoWaveAnimation.Instance
	{
		protected internal Instance(GameGizmoWaveAnimationCarrier gameGizmo) : base(gameGizmo)
		{
		}

		protected override void Render(bool useCamera, Camera camera)
		{
			if (gizmos.carrier && this.carrierRenderer && this.carrierRenderer.enabled)
			{
				Material[] materialArray = ((GameGizmoWaveAnimationCarrier)this.gameGizmo).carrierMaterials;
				if (materialArray != null && (int)materialArray.Length > 0)
				{
					MeshFilter component = this.carrierRenderer.GetComponent<MeshFilter>();
					if (component)
					{
						Mesh mesh = component.sharedMesh;
						if (mesh)
						{
							try
							{
								this.hideMesh = ((GameGizmoWaveAnimationCarrier)this.gameGizmo).hideArrowWhenCarrierExists;
								base.Render(useCamera, camera);
							}
							finally
							{
								this.hideMesh = false;
							}
							Material[] materialArray1 = materialArray;
							for (int i = 0; i < (int)materialArray1.Length; i++)
							{
								Material material = materialArray1[i];
								if (material)
								{
									int num = mesh.subMeshCount;
									while (true)
									{
										int num1 = num - 1;
										num = num1;
										if (num1 < 0)
										{
											break;
										}
										Graphics.DrawMesh(mesh, this.carrierRenderer.localToWorldMatrix, material, base.layer, camera, num, this.propertyBlock, base.castShadows, base.receiveShadows);
									}
								}
							}
							return;
						}
					}
				}
			}
			base.Render(useCamera, camera);
		}
	}
}