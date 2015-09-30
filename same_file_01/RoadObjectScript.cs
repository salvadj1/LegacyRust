using EasyRoads3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObjectScript : MonoBehaviour
{
	public static string version;

	public int objectType;

	public bool displayRoad = true;

	public float roadWidth = 5f;

	public float indent = 3f;

	public float surrounding = 5f;

	public float raise = 1f;

	public float raiseMarkers = 0.5f;

	public bool OOQDOOQQ;

	public bool renderRoad = true;

	public bool beveledRoad;

	public bool applySplatmap;

	public int splatmapLayer = 4;

	public bool autoUpdate = true;

	public float geoResolution = 5f;

	public int roadResolution = 1;

	public float tuw = 15f;

	public int splatmapSmoothLevel;

	public float opacity = 1f;

	public int expand;

	public int offsetX;

	public int offsetY;

	private Material surfaceMaterial;

	public float surfaceOpacity = 1f;

	public float smoothDistance = 1f;

	public float smoothSurDistance = 3f;

	private bool handleInsertFlag;

	public bool handleVegetation = true;

	public float OOCQDOOCQD = 2f;

	public float ODDQCCDCDC = 1f;

	public int materialType;

	private string[] materialStrings;

	private MarkerScript[] mSc;

	private bool ODQDOQOCCD;

	private bool[] OQCCDQCDDD;

	private bool[] ODODODCODD;

	public string[] OODQCQODQQ;

	public string[] ODODQOQO;

	public int[] ODODQOQOInt;

	public int OQDCQDCDDD = -1;

	public int ODOCDOOOQQ = -1;

	public static GUISkin OODCDOQDCC;

	public static GUISkin OODQQDDCDD;

	public bool OQOCDODDQC;

	private Vector3 cPos;

	private Vector3 ePos;

	public bool OOCCDCOQCQ;

	public static Texture2D OQOOODODQD;

	public int markers = 1;

	public OQCDQQDQCC OOQQCODOCD;

	private GameObject ODOQDQOO;

	public bool ODODCOCCDQ;

	public bool doTerrain;

	private Transform OCQOCOCQQO;

	public GameObject[] OCQOCOCQQOs;

	private static string OCQCDDDOCC;

	public Transform obj;

	private string OOQCQCDDOQ;

	public static string erInit;

	public static Transform OCQQQOQOQC;

	private RoadObjectScript OODCCOODCC;

	public bool flyby;

	private Vector3 pos;

	private float fl;

	private float oldfl;

	private bool ODDCQQQQOO;

	private bool ODOQCDDCOO;

	private bool ODQQOQCQCO;

	public Transform OODDDCQCOC;

	public int OdQODQOD = 1;

	public float OOQQQDOD;

	public float OOQQQDODOffset;

	public float OOQQQDODLength;

	public bool ODODDDOO;

	public static string[] ODOQDOQO;

	public static string[] ODODOQQO;

	public static string[] ODODQOOQ;

	public int ODQDOOQO;

	public string[] ODQQQQQO;

	public string[] ODODDQOO;

	public bool[] ODODQQOD;

	public int[] OOQQQOQO;

	public int ODOQOOQO;

	public bool forceY;

	public float yChange;

	public float floorDepth = 2f;

	public float waterLevel = 1.5f;

	public bool lockWaterLevel = true;

	public float lastY;

	public string distance = "0";

	public string markerDisplayStr = "Hide Markers";

	public static string[] objectStrings;

	public string objectText = "Road";

	public bool applyAnimation;

	public float waveSize = 1.5f;

	public float waveHeight = 0.15f;

	public bool snapY = true;

	private TextAnchor origAnchor;

	public bool autoODODDQQO;

	public Texture2D roadTexture;

	public Texture2D roadMaterial;

	public string[] ODQOOCCQQO;

	public string[] OOOOCOCCDC;

	public int selectedWaterMaterial;

	public int selectedWaterScript;

	private bool doRestore;

	public bool doFlyOver;

	public static GameObject tracer;

	public Camera goCam;

	public float speed = 1f;

	public float offset;

	public bool camInit;

	public GameObject customMesh;

	public static bool disableFreeAlerts;

	public bool multipleTerrains;

	public bool editRestore = true;

	public Material roadMaterialEdit;

	public static int backupLocation;

	public string[] backupStrings = new string[] { "Outside Assets folder path", "Inside Assets folder path" };

	public Vector3[] leftVecs = new Vector3[0];

	public Vector3[] rightVecs = new Vector3[0];

	static RoadObjectScript()
	{
		RoadObjectScript.version = string.Empty;
		RoadObjectScript.erInit = string.Empty;
		RoadObjectScript.disableFreeAlerts = true;
	}

	public RoadObjectScript()
	{
	}

	public bool CheckWaterHeights()
	{
		bool flag = true;
		float single = Terrain.activeTerrain.transform.position.y;
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				IEnumerator enumerator1 = current.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						if (((Transform)enumerator1.Current).position.y - single > 0.1f)
						{
							continue;
						}
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable = enumerator1 as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		return flag;
	}

	public void OCCOCQQQDO(MarkerScript markerScript)
	{
		this.OCQOCOCQQO = markerScript.transform;
		List<GameObject> gameObjects = new List<GameObject>();
		for (int i = 0; i < (int)this.OCQOCOCQQOs.Length; i++)
		{
			if (this.OCQOCOCQQOs[i] != markerScript.gameObject)
			{
				gameObjects.Add(this.OCQOCOCQQOs[i]);
			}
		}
		gameObjects.Add(markerScript.gameObject);
		this.OCQOCOCQQOs = gameObjects.ToArray();
		this.OCQOCOCQQO = markerScript.transform;
		this.OOQQCODOCD.OODCDQDOCO(this.OCQOCOCQQO, this.OCQOCOCQQOs, markerScript.OCCCCODCOD, markerScript.OQCQOQQDCQ, this.OODDDCQCOC, out markerScript.OCQOCOCQQOs, out markerScript.trperc, this.OCQOCOCQQOs);
		this.ODOCDOOOQQ = -1;
	}

	public void OCCOOQDCQO()
	{
		if ((!this.ODODDDOO || this.objectType == 2) && this.OQCCDQCDDD != null)
		{
			for (int i = 0; i < (int)this.OQCCDQCDDD.Length; i++)
			{
				this.OQCCDQCDDD[i] = false;
				this.ODODODCODD[i] = false;
			}
		}
	}

	public void OCOOCODDOC(float geo, bool renderMode, bool camMode)
	{
		this.OOQQCODOCD.OOODOQDODQ.Clear();
		int num = 0;
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				IEnumerator enumerator1 = current.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						Transform transforms = (Transform)enumerator1.Current;
						MarkerScript component = transforms.GetComponent<MarkerScript>();
						component.objectScript = this.obj.GetComponent<RoadObjectScript>();
						if (!component.OOCCDCOQCQ)
						{
							component.OOCCDCOQCQ = this.OOQQCODOCD.OOOCQDOCDC(transforms);
						}
						OQDQOQDOQO oQDQOQDOQO = new OQDQOQDOQO()
						{
							position = transforms.position,
							num = this.OOQQCODOCD.OOODOQDODQ.Count,
							object1 = transforms,
							object2 = component.surface,
							tension = component.tension,
							ri = component.ri
						};
						if (oQDQOQDOQO.ri < 1f)
						{
							oQDQOQDOQO.ri = 1f;
						}
						oQDQOQDOQO.li = component.li;
						if (oQDQOQDOQO.li < 1f)
						{
							oQDQOQDOQO.li = 1f;
						}
						oQDQOQDOQO.rt = component.rt;
						oQDQOQDOQO.lt = component.lt;
						oQDQOQDOQO.rs = component.rs;
						if (oQDQOQDOQO.rs < 1f)
						{
							oQDQOQDOQO.rs = 1f;
						}
						oQDQOQDOQO.OQDOOODDQD = component.rs;
						oQDQOQDOQO.ls = component.ls;
						if (oQDQOQDOQO.ls < 1f)
						{
							oQDQOQDOQO.ls = 1f;
						}
						oQDQOQDOQO.OOOCDQODDO = component.ls;
						oQDQOQDOQO.renderFlag = component.bridgeObject;
						oQDQOQDOQO.OCCOQCQDOD = component.distHeights;
						oQDQOQDOQO.newSegment = component.newSegment;
						oQDQOQDOQO.floorDepth = component.floorDepth;
						oQDQOQDOQO.waterLevel = this.waterLevel;
						oQDQOQDOQO.lockWaterLevel = component.lockWaterLevel;
						oQDQOQDOQO.sharpCorner = component.sharpCorner;
						oQDQOQDOQO.OQCDCODODQ = this.OOQQCODOCD;
						component.markerNum = num;
						component.distance = "-1";
						component.OODDQCQQDD = "-1";
						this.OOQQCODOCD.OOODOQDODQ.Add(oQDQOQDOQO);
						num++;
					}
				}
				finally
				{
					IDisposable disposable = enumerator1 as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		this.distance = "-1";
		this.OOQQCODOCD.ODQQQCQCOO = this.OODCCOODCC.roadWidth;
		this.OOQQCODOCD.ODOCODQOOC(geo, this.obj, this.OODCCOODCC.OOQDOOQQ, renderMode, camMode, this.objectType);
		if (this.OOQQCODOCD.leftVecs.Count > 0)
		{
			this.leftVecs = this.OOQQCODOCD.leftVecs.ToArray();
			this.rightVecs = this.OOQQCODOCD.rightVecs.ToArray();
		}
	}

	public void OCOQDCQOCD(MarkerScript markerScript)
	{
		if (markerScript.OQCQOQQDCQ != markerScript.ODOOQQOO || markerScript.OQCQOQQDCQ != markerScript.ODOOQQOO)
		{
			this.OOQQCODOCD.OODCDQDOCO(this.OCQOCOCQQO, this.OCQOCOCQQOs, markerScript.OCCCCODCOD, markerScript.OQCQOQQDCQ, this.OODDDCQCOC, out markerScript.OCQOCOCQQOs, out markerScript.trperc, this.OCQOCOCQQOs);
			markerScript.ODQDOQOO = markerScript.OCCCCODCOD;
			markerScript.ODOOQQOO = markerScript.OQCQOQQDCQ;
		}
		if (this.OODCCOODCC.autoUpdate)
		{
			this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
		}
	}

	public void OCOQDDODDQ(ArrayList arr, string[] DOODQOQO, string[] OODDQOQO)
	{
		this.ODOCOQCCOC(base.transform, arr, DOODQOQO, OODDQOQO);
	}

	public void OCQDCQDDCO()
	{
		this.OOQQCODOCD.OCQDCQDDCO(this.OODCCOODCC.applySplatmap, this.OODCCOODCC.splatmapSmoothLevel, this.OODCCOODCC.renderRoad, this.OODCCOODCC.tuw, this.OODCCOODCC.roadResolution, this.OODCCOODCC.raise, this.OODCCOODCC.opacity, this.OODCCOODCC.expand, this.OODCCOODCC.offsetX, this.OODCCOODCC.offsetY, this.OODCCOODCC.beveledRoad, this.OODCCOODCC.splatmapLayer, this.OODCCOODCC.OdQODQOD, this.OOQQQDOD, this.OOQQQDODOffset, this.OOQQQDODLength);
	}

	public ArrayList ODCOQCODCC()
	{
		ArrayList arrayLists = new ArrayList();
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				IEnumerator enumerator1 = current.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						Transform transforms = (Transform)enumerator1.Current;
						MarkerScript component = transforms.GetComponent<MarkerScript>();
						arrayLists.Add(component.ODDGDOOO);
						arrayLists.Add(component.ODDQOODO);
						!(transforms.name == "Marker0003");
						arrayLists.Add(component.ODDQOOO);
					}
				}
				finally
				{
					IDisposable disposable = enumerator1 as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		return arrayLists;
	}

	public void ODCQOCDQOC()
	{
		UnityEngine.Object.DestroyImmediate(this.OODCCOODCC.OCQOCOCQQO.gameObject);
		this.OCQOCOCQQO = null;
		this.OOQODQOCOC();
	}

	public void ODDOOODDCQ()
	{
		RoadObjectScript[] roadObjectScriptArray = (RoadObjectScript[])UnityEngine.Object.FindObjectsOfType(typeof(RoadObjectScript));
		ArrayList arrayLists = new ArrayList();
		RoadObjectScript[] roadObjectScriptArray1 = roadObjectScriptArray;
		for (int i = 0; i < (int)roadObjectScriptArray1.Length; i++)
		{
			RoadObjectScript roadObjectScript = roadObjectScriptArray1[i];
			if (roadObjectScript.transform != base.transform)
			{
				arrayLists.Add(roadObjectScript.transform);
			}
		}
		if (this.ODODQOQO == null)
		{
			this.ODODQOQO = this.OOQQCODOCD.OCDODCOCOC();
			this.ODODQOQOInt = this.OOQQCODOCD.OCCQOQCQDO();
		}
		this.OCOOCODDOC(0.5f, true, false);
		this.OOQQCODOCD.OCOOOOCOQO(Vector3.zero, this.OODCCOODCC.raise, this.obj, this.OODCCOODCC.OOQDOOQQ, arrayLists, this.handleVegetation);
		this.OCQDCQDDCO();
	}

	public void ODOCOQCCOC(Transform tr, ArrayList arr, string[] DOODQOQO, string[] OODDQOQO)
	{
		RoadObjectScript.version = "2.4.6";
		RoadObjectScript.OODCDOQDCC = (GUISkin)UnityEngine.Resources.Load("ER3DSkin", typeof(GUISkin));
		RoadObjectScript.OQOOODODQD = (Texture2D)UnityEngine.Resources.Load("ER3DLogo", typeof(Texture2D));
		if (RoadObjectScript.objectStrings == null)
		{
			RoadObjectScript.objectStrings = new string[] { "Road Object", "River Object", "Procedural Mesh Object" };
		}
		this.obj = tr;
		this.OOQQCODOCD = new OQCDQQDQCC();
		this.OODCCOODCC = this.obj.GetComponent<RoadObjectScript>();
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				this.OODDDCQCOC = current;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		OQCDQQDQCC.terrainList.Clear();
		Terrain[] terrainArray = (Terrain[])UnityEngine.Object.FindObjectsOfType(typeof(Terrain));
		for (int i = 0; i < (int)terrainArray.Length; i++)
		{
			Terrain terrain = terrainArray[i];
			Terrains component = new Terrains()
			{
				terrain = terrain
			};
			if (terrain.gameObject.GetComponent<EasyRoads3DTerrainID>())
			{
				component.id = terrain.gameObject.GetComponent<EasyRoads3DTerrainID>().terrainid;
			}
			else
			{
				EasyRoads3DTerrainID easyRoads3DTerrainID = (EasyRoads3DTerrainID)terrain.gameObject.AddComponent("EasyRoads3DTerrainID");
				string str = UnityEngine.Random.Range(100000000, 999999999).ToString();
				easyRoads3DTerrainID.terrainid = str;
				component.id = str;
			}
			this.OOQQCODOCD.OCDQQCDOQO(component);
		}
		ODCDDDDQQD.OCDQQCDOQO();
		if (this.roadMaterialEdit == null)
		{
			this.roadMaterialEdit = (Material)UnityEngine.Resources.Load("materials/roadMaterialEdit", typeof(Material));
		}
		if (this.objectType == 0 && GameObject.Find(string.Concat(base.gameObject.name, "/road")) == null)
		{
			(new GameObject("road")).transform.parent = base.transform;
		}
		this.OOQQCODOCD.OODQOQCDCQ(this.obj, RoadObjectScript.OCQCDDDOCC, this.OODCCOODCC.roadWidth, this.surfaceOpacity, out this.OOCCDCOQCQ, out this.indent, this.applyAnimation, this.waveSize, this.waveHeight);
		this.OOQQCODOCD.ODDQCCDCDC = this.ODDQCCDCDC;
		this.OOQQCODOCD.OOCQDOOCQD = this.OOCQDOOCQD;
		this.OOQQCODOCD.OdQODQOD = this.OdQODQOD + 1;
		this.OOQQCODOCD.OOQQQDOD = this.OOQQQDOD;
		this.OOQQCODOCD.OOQQQDODOffset = this.OOQQQDODOffset;
		this.OOQQCODOCD.OOQQQDODLength = this.OOQQQDODLength;
		this.OOQQCODOCD.objectType = this.objectType;
		this.OOQQCODOCD.snapY = this.snapY;
		this.OOQQCODOCD.terrainRendered = this.ODODCOCCDQ;
		this.OOQQCODOCD.handleVegetation = this.handleVegetation;
		this.OOQQCODOCD.raise = this.raise;
		this.OOQQCODOCD.roadResolution = this.roadResolution;
		this.OOQQCODOCD.multipleTerrains = this.multipleTerrains;
		this.OOQQCODOCD.editRestore = this.editRestore;
		this.OOQQCODOCD.roadMaterialEdit = this.roadMaterialEdit;
		if (RoadObjectScript.backupLocation != 0)
		{
			OOCDQCOODC.backupFolder = "/Assets/EasyRoads3D/backups";
		}
		else
		{
			OOCDQCOODC.backupFolder = "/EasyRoads3D";
		}
		this.ODODQOQO = this.OOQQCODOCD.OCDODCOCOC();
		this.ODODQOQOInt = this.OOQQCODOCD.OCCQOQCQDO();
		if (this.ODODCOCCDQ)
		{
			this.doRestore = true;
		}
		this.OOQODQOCOC();
		if (arr != null || RoadObjectScript.ODODQOOQ == null)
		{
			this.OOOOOOODCD(arr, DOODQOQO, OODDQOQO);
		}
		if (this.doRestore)
		{
			return;
		}
	}

	public void ODQDCQQDDO(Vector3 pos, bool doInsert)
	{
		string str;
		if (!this.displayRoad)
		{
			this.displayRoad = true;
			this.OOQQCODOCD.OODDDCQCCQ(this.displayRoad, this.OODDDCQCOC);
		}
		int num = -1;
		int num1 = -1;
		float single = 10000f;
		float single1 = 10000f;
		Vector3 vector3 = pos;
		OQDQOQDOQO item = (OQDQOQDOQO)this.OOQQCODOCD.OOODOQDODQ[0];
		OQDQOQDOQO oQDQOQDOQO = (OQDQOQDOQO)this.OOQQCODOCD.OOODOQDODQ[1];
		this.OOQQCODOCD.ODDDDCCDCO(pos, out num, out num1, out single, out single1, out item, out oQDQOQDOQO, out vector3);
		pos = vector3;
		if (doInsert && num >= 0 && num1 >= 0)
		{
			if (!this.OODCCOODCC.OOQDOOQQ || num1 != this.OOQQCODOCD.OOODOQDODQ.Count - 1)
			{
				OQDQOQDOQO item1 = (OQDQOQDOQO)this.OOQQCODOCD.OOODOQDODQ[num1];
				string str1 = item1.object1.name;
				int num2 = num1 + 2;
				for (int i = num1; i < this.OOQQCODOCD.OOODOQDODQ.Count - 1; i++)
				{
					item1 = (OQDQOQDOQO)this.OOQQCODOCD.OOODOQDODQ[i];
					if (num2 >= 10)
					{
						str = (num2 >= 100 ? string.Concat("Marker0", num2.ToString()) : string.Concat("Marker00", num2.ToString()));
					}
					else
					{
						str = string.Concat("Marker000", num2.ToString());
					}
					item1.object1.name = str;
					num2++;
				}
				item1 = (OQDQOQDOQO)this.OOQQCODOCD.OOODOQDODQ[num];
				Transform oODDDCQCOC = (Transform)UnityEngine.Object.Instantiate(item1.object1.transform, pos, item1.object1.rotation);
				oODDDCQCOC.gameObject.name = str1;
				oODDDCQCOC.parent = this.OODDDCQCOC;
				MarkerScript component = oODDDCQCOC.GetComponent<MarkerScript>();
				component.OOCCDCOQCQ = false;
				float single2 = single / (single + single1);
				float single3 = item.ri - oQDQOQDOQO.ri;
				component.ri = item.ri - single3 * single2;
				single3 = item.li - oQDQOQDOQO.li;
				component.li = item.li - single3 * single2;
				single3 = item.rt - oQDQOQDOQO.rt;
				component.rt = item.rt - single3 * single2;
				single3 = item.lt - oQDQOQDOQO.lt;
				component.lt = item.lt - single3 * single2;
				single3 = item.rs - oQDQOQDOQO.rs;
				component.rs = item.rs - single3 * single2;
				single3 = item.ls - oQDQOQDOQO.ls;
				component.ls = item.ls - single3 * single2;
				this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
				if (this.materialType == 0)
				{
					this.OOQQCODOCD.OOQOOCDQOD(this.materialType);
				}
				if (this.objectType == 2)
				{
					component.surface.gameObject.SetActive(false);
				}
			}
			else
			{
				this.OODDQODDCC(pos);
			}
		}
		this.OOQODQOCOC();
	}

	public void ODQDOOOCOC()
	{
		this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
		if (this.OOQQCODOCD != null)
		{
			this.OOQQCODOCD.ODQDOOOCOC();
		}
		this.ODODDDOO = false;
	}

	public void OODDQODDCC(Vector3 pos)
	{
		string str;
		if (!this.displayRoad)
		{
			this.displayRoad = true;
			this.OOQQCODOCD.OODDDCQCCQ(this.displayRoad, this.OODDDCQCOC);
		}
		pos.y = pos.y + this.OODCCOODCC.raiseMarkers;
		if (this.forceY && this.ODOQDQOO != null)
		{
			float single = Vector3.Distance(pos, this.ODOQDQOO.transform.position);
			Vector3 oDOQDQOO = this.ODOQDQOO.transform.position;
			pos.y = oDOQDQOO.y + this.yChange * (single / 100f);
		}
		else if (this.forceY && this.markers == 0)
		{
			this.lastY = pos.y;
		}
		GameObject gameObject = null;
		gameObject = (this.ODOQDQOO == null ? (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("marker", typeof(GameObject))) : (GameObject)UnityEngine.Object.Instantiate(this.ODOQDQOO));
		Transform oODDDCQCOC = gameObject.transform;
		oODDDCQCOC.position = pos;
		oODDDCQCOC.parent = this.OODDDCQCOC;
		RoadObjectScript roadObjectScript = this;
		roadObjectScript.markers = roadObjectScript.markers + 1;
		if (this.markers >= 10)
		{
			str = (this.markers >= 100 ? string.Concat("Marker0", this.markers.ToString()) : string.Concat("Marker00", this.markers.ToString()));
		}
		else
		{
			str = string.Concat("Marker000", this.markers.ToString());
		}
		oODDDCQCOC.gameObject.name = str;
		MarkerScript component = oODDDCQCOC.GetComponent<MarkerScript>();
		component.OOCCDCOQCQ = false;
		component.objectScript = this.obj.GetComponent<RoadObjectScript>();
		if (this.ODOQDQOO == null)
		{
			component.waterLevel = this.OODCCOODCC.waterLevel;
			component.floorDepth = this.OODCCOODCC.floorDepth;
			component.ri = this.OODCCOODCC.indent;
			component.li = this.OODCCOODCC.indent;
			component.rs = this.OODCCOODCC.surrounding;
			component.ls = this.OODCCOODCC.surrounding;
			component.tension = 0.5f;
			if (this.objectType == 1)
			{
				pos.y = pos.y - this.waterLevel;
				oODDDCQCOC.position = pos;
			}
		}
		if (this.objectType == 2 && component.surface != null)
		{
			component.surface.gameObject.SetActive(false);
		}
		this.ODOQDQOO = oODDDCQCOC.gameObject;
		if (this.markers > 1)
		{
			this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
			if (this.materialType == 0)
			{
				this.OOQQCODOCD.OOQOOCDQOD(this.materialType);
			}
		}
	}

	public void OOOOOOODCD(ArrayList arr, string[] DOODQOQO, string[] OODDQOQO)
	{
		bool flag = false;
		RoadObjectScript.ODODOQQO = DOODQOQO;
		RoadObjectScript.ODODQOOQ = OODDQOQO;
		ArrayList arrayLists = new ArrayList();
		if (this.obj == null)
		{
			this.ODOCOQCCOC(base.transform, null, null, null);
		}
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				IEnumerator enumerator1 = current.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						MarkerScript component = ((Transform)enumerator1.Current).GetComponent<MarkerScript>();
						component.OQODQQDO.Clear();
						component.ODOQQQDO.Clear();
						component.OQQODQQOO.Clear();
						component.ODDOQQOO.Clear();
						arrayLists.Add(component);
					}
				}
				finally
				{
					IDisposable disposable = enumerator1 as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		this.mSc = (MarkerScript[])arrayLists.ToArray(typeof(MarkerScript));
		ArrayList arrayLists1 = new ArrayList();
		int num = 0;
		int num1 = 0;
		if (this.ODQQQQQO != null)
		{
			if (arr.Count == 0)
			{
				return;
			}
			for (int i = 0; i < (int)RoadObjectScript.ODODOQQO.Length; i++)
			{
				ODODDQQO item = (ODODDQQO)arr[i];
				for (int j = 0; j < (int)this.ODQQQQQO.Length; j++)
				{
					if (RoadObjectScript.ODODOQQO[i] == this.ODQQQQQO[j])
					{
						num++;
						if ((int)this.ODODQQOD.Length <= j)
						{
							arrayLists1.Add(false);
						}
						else
						{
							arrayLists1.Add(this.ODODQQOD[j]);
						}
						MarkerScript[] markerScriptArray = this.mSc;
						for (int k = 0; k < (int)markerScriptArray.Length; k++)
						{
							MarkerScript markerScript = markerScriptArray[k];
							int num2 = -1;
							int num3 = 0;
							while (num3 < (int)markerScript.ODDOOQDO.Length)
							{
								if (item.id != markerScript.ODDOOQDO[num3])
								{
									num3++;
								}
								else
								{
									num2 = num3;
									break;
								}
							}
							if (num2 < 0)
							{
								markerScript.OQODQQDO.Add(item.id);
								markerScript.ODOQQQDO.Add(item.markerActive);
								markerScript.OQQODQQOO.Add(true);
								markerScript.ODDOQQOO.Add(item.splinePosition);
							}
							else
							{
								markerScript.OQODQQDO.Add(markerScript.ODDOOQDO[num2]);
								markerScript.ODOQQQDO.Add(markerScript.ODDGDOOO[num2]);
								markerScript.OQQODQQOO.Add(markerScript.ODDQOOO[num2]);
								if (item.sidewaysDistanceUpdate == 0 || item.sidewaysDistanceUpdate == 2 && (float)markerScript.ODDQOODO[num2] != item.oldSidwaysDistance)
								{
									markerScript.ODDOQQOO.Add(markerScript.ODDQOODO[num2]);
								}
								else
								{
									markerScript.ODDOQQOO.Add(item.splinePosition);
								}
							}
						}
					}
				}
				item.sidewaysDistanceUpdate == 0;
				flag = false;
			}
		}
		for (int l = 0; l < (int)RoadObjectScript.ODODOQQO.Length; l++)
		{
			ODODDQQO oDODDQQO = (ODODDQQO)arr[l];
			bool flag1 = false;
			for (int m = 0; m < (int)this.ODQQQQQO.Length; m++)
			{
				if (RoadObjectScript.ODODOQQO[l] == this.ODQQQQQO[m])
				{
					flag1 = true;
				}
			}
			if (!flag1)
			{
				num1++;
				arrayLists1.Add(false);
				MarkerScript[] markerScriptArray1 = this.mSc;
				for (int n = 0; n < (int)markerScriptArray1.Length; n++)
				{
					MarkerScript markerScript1 = markerScriptArray1[n];
					markerScript1.OQODQQDO.Add(oDODDQQO.id);
					markerScript1.ODOQQQDO.Add(oDODDQQO.markerActive);
					markerScript1.OQQODQQOO.Add(true);
					markerScript1.ODDOQQOO.Add(oDODDQQO.splinePosition);
				}
			}
		}
		this.ODODQQOD = (bool[])arrayLists1.ToArray(typeof(bool));
		this.ODQQQQQO = new string[(int)RoadObjectScript.ODODOQQO.Length];
		RoadObjectScript.ODODOQQO.CopyTo(this.ODQQQQQO, 0);
		ArrayList arrayLists2 = new ArrayList();
		for (int o = 0; o < (int)this.ODODQQOD.Length; o++)
		{
			if (this.ODODQQOD[o])
			{
				arrayLists2.Add(o);
			}
		}
		this.OOQQQOQO = (int[])arrayLists2.ToArray(typeof(int));
		MarkerScript[] markerScriptArray2 = this.mSc;
		for (int p = 0; p < (int)markerScriptArray2.Length; p++)
		{
			MarkerScript array = markerScriptArray2[p];
			array.ODDOOQDO = (string[])array.OQODQQDO.ToArray(typeof(string));
			array.ODDGDOOO = (bool[])array.ODOQQQDO.ToArray(typeof(bool));
			array.ODDQOOO = (bool[])array.OQQODQQOO.ToArray(typeof(bool));
			array.ODDQOODO = (float[])array.ODDOQQOO.ToArray(typeof(float));
		}
		!flag;
	}

	public void OOOOQCDODD(MarkerScript markerScript)
	{
		if (markerScript.OQCQOQQDCQ != markerScript.ODOOQQOO)
		{
			this.OOQQCODOCD.OODCDQDOCO(this.OCQOCOCQQO, this.OCQOCOCQQOs, markerScript.OCCCCODCOD, markerScript.OQCQOQQDCQ, this.OODDDCQCOC, out markerScript.OCQOCOCQQOs, out markerScript.trperc, this.OCQOCOCQQOs);
			markerScript.ODOOQQOO = markerScript.OQCQOQQDCQ;
		}
		this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
	}

	public void OOQODQOCOC()
	{
		string str;
		int num = 0;
		IEnumerator enumerator = this.obj.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name != "Markers")
				{
					continue;
				}
				num = 1;
				IEnumerator enumerator1 = current.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						Transform transforms = (Transform)enumerator1.Current;
						if (num >= 10)
						{
							str = (num >= 100 ? string.Concat("Marker0", num.ToString()) : string.Concat("Marker00", num.ToString()));
						}
						else
						{
							str = string.Concat("Marker000", num.ToString());
						}
						transforms.name = str;
						this.ODOQDQOO = transforms.gameObject;
						num++;
					}
				}
				finally
				{
					IDisposable disposable = enumerator1 as IDisposable;
					if (disposable == null)
					{
					}
					disposable.Dispose();
				}
			}
		}
		finally
		{
			IDisposable disposable1 = enumerator as IDisposable;
			if (disposable1 == null)
			{
			}
			disposable1.Dispose();
		}
		this.markers = num - 1;
		this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
	}

	public void OQCOCQDQDD()
	{
		ArrayList arrayLists = new ArrayList();
		ArrayList arrayLists1 = new ArrayList();
		ArrayList arrayLists2 = new ArrayList();
		for (int i = 0; i < (int)RoadObjectScript.ODODOQQO.Length; i++)
		{
			if (this.ODODQQOD[i])
			{
				arrayLists.Add(RoadObjectScript.ODODQOOQ[i]);
				arrayLists2.Add(RoadObjectScript.ODODOQQO[i]);
				arrayLists1.Add(i);
			}
		}
		this.ODODDQOO = (string[])arrayLists.ToArray(typeof(string));
		this.OOQQQOQO = (int[])arrayLists1.ToArray(typeof(int));
	}

	public void OQCQQDODDC()
	{
		if (this.OOQQCODOCD == null)
		{
			this.ODOCOQCCOC(base.transform, null, null, null);
		}
		OQCDQQDQCC.ODOQCCODQC = true;
		if (!this.ODODCOCCDQ)
		{
			this.geoResolution = 0.5f;
			this.ODODCOCCDQ = true;
			this.doTerrain = false;
			this.OOQODQOCOC();
			if (this.objectType < 2)
			{
				this.ODDOOODDCQ();
			}
			this.OOQQCODOCD.terrainRendered = true;
			this.OCQDCQDDCO();
		}
		if (this.displayRoad && this.objectType < 2)
		{
			Material material = (Material)UnityEngine.Resources.Load("roadMaterial", typeof(Material));
			if (this.OOQQCODOCD.road.renderer != null)
			{
				this.OOQQCODOCD.road.renderer.material = material;
			}
			IEnumerator enumerator = this.OOQQCODOCD.road.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform current = (Transform)enumerator.Current;
					if (current.gameObject.renderer == null)
					{
						continue;
					}
					current.gameObject.renderer.material = material;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.OOQQCODOCD.road.transform.parent = null;
			this.OOQQCODOCD.road.layer = 0;
			this.OOQQCODOCD.road.name = base.gameObject.name;
		}
		else if (this.OOQQCODOCD.road != null)
		{
			UnityEngine.Object.DestroyImmediate(this.OOQQCODOCD.road);
		}
	}

	private void OQDODCODOQ(string ctrl, MarkerScript markerScript)
	{
		int num = 0;
		Transform[] oCQOCOCQQOs = markerScript.OCQOCOCQQOs;
		for (int i = 0; i < (int)oCQOCOCQQOs.Length; i++)
		{
			MarkerScript component = oCQOCOCQQOs[i].GetComponent<MarkerScript>();
			if (ctrl == "rs")
			{
				component.LeftSurrounding(markerScript.rs - markerScript.ODOQQOOO, markerScript.trperc[num]);
			}
			else if (ctrl == "ls")
			{
				component.RightSurrounding(markerScript.ls - markerScript.DODOQQOO, markerScript.trperc[num]);
			}
			else if (ctrl == "ri")
			{
				component.LeftIndent(markerScript.ri - markerScript.OOQOQQOO, markerScript.trperc[num]);
			}
			else if (ctrl == "li")
			{
				component.RightIndent(markerScript.li - markerScript.ODODQQOO, markerScript.trperc[num]);
			}
			else if (ctrl == "rt")
			{
				component.LeftTilting(markerScript.rt - markerScript.ODDQODOO, markerScript.trperc[num]);
			}
			else if (ctrl == "lt")
			{
				component.RightTilting(markerScript.lt - markerScript.ODDOQOQQ, markerScript.trperc[num]);
			}
			else if (ctrl == "floorDepth")
			{
				component.FloorDepth(markerScript.floorDepth - markerScript.oldFloorDepth, markerScript.trperc[num]);
			}
			num++;
		}
	}

	public void OQOCODCDOO()
	{
		if (this.markers > 1)
		{
			this.OCOOCODDOC(this.OODCCOODCC.geoResolution, false, false);
		}
	}

	public void OQQDQCQQOC()
	{
		this.OOQQCODOCD.OQQDQCQQOC(this.OODCCOODCC.renderRoad, this.OODCCOODCC.tuw, this.OODCCOODCC.roadResolution, this.OODCCOODCC.raise, this.OODCCOODCC.beveledRoad, this.OODCCOODCC.OdQODQOD, this.OOQQQDOD, this.OOQQQDODOffset, this.OOQQQDODLength);
	}

	public void OQQOOCCQCO()
	{
		this.OOQQCODOCD.OOQDODCQOQ(12);
	}

	public ArrayList RebuildObjs()
	{
		RoadObjectScript[] roadObjectScriptArray = (RoadObjectScript[])UnityEngine.Object.FindObjectsOfType(typeof(RoadObjectScript));
		ArrayList arrayLists = new ArrayList();
		RoadObjectScript[] roadObjectScriptArray1 = roadObjectScriptArray;
		for (int i = 0; i < (int)roadObjectScriptArray1.Length; i++)
		{
			RoadObjectScript roadObjectScript = roadObjectScriptArray1[i];
			if (roadObjectScript.transform != base.transform)
			{
				arrayLists.Add(roadObjectScript.transform);
			}
		}
		return arrayLists;
	}

	public void ResetMaterials(MarkerScript markerScript)
	{
		if (this.OOQQCODOCD != null)
		{
			this.OOQQCODOCD.OODCDQDOCO(this.OCQOCOCQQO, this.OCQOCOCQQOs, markerScript.OCCCCODCOD, markerScript.OQCQOQQDCQ, this.OODDDCQCOC, out markerScript.OCQOCOCQQOs, out markerScript.trperc, this.OCQOCOCQQOs);
		}
	}

	public void StartCam()
	{
		this.OCOOCODDOC(0.5f, false, true);
	}

	public void UpdateBackupFolder()
	{
	}
}