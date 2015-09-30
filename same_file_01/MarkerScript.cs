using EasyRoads3D;
using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class MarkerScript : MonoBehaviour
{
	public float tension = 0.5f;

	public float ri;

	public float OOQOQQOO;

	public float li;

	public float ODODQQOO;

	public float rs;

	public float ODOQQOOO;

	public float ls;

	public float DODOQQOO;

	public float rt;

	public float ODDQODOO;

	public float lt;

	public float ODDOQOQQ;

	public bool OCCCCODCOD;

	public bool ODQDOQOO;

	public float OQCQOQQDCQ;

	public float ODOOQQOO;

	public Transform[] OCQOCOCQQOs;

	public float[] trperc;

	public Vector3 oldPos = Vector3.zero;

	public bool autoUpdate;

	public bool changed;

	public Transform surface;

	public bool OOCCDCOQCQ;

	private Vector3 position;

	private bool updated;

	private int frameCount;

	private float currentstamp;

	private float newstamp;

	private bool mousedown;

	private Vector3 lookAtPoint;

	public bool bridgeObject;

	public bool distHeights;

	public RoadObjectScript objectScript;

	public ArrayList OQODQQDO = new ArrayList();

	public ArrayList ODOQQQDO = new ArrayList();

	public ArrayList OQQODQQOO = new ArrayList();

	public ArrayList ODDOQQOO = new ArrayList();

	public ArrayList ODDDDQOO = new ArrayList();

	public ArrayList DQQOQQOO = new ArrayList();

	public string[] ODDOOQDO;

	public bool[] ODDGDOOO;

	public bool[] ODDQOOO;

	public float[] ODDQOODO;

	public float[] ODOQODOO;

	public float[] ODDOQDO;

	public int markerNum;

	public string distance = "0";

	public string OQOQODQCQC = "0";

	public string OODDQCQQDD = "0";

	public bool newSegment;

	public float floorDepth = 2f;

	public float oldFloorDepth = 2f;

	public float waterLevel = 0.5f;

	public bool lockWaterLevel = true;

	public bool sharpCorner;

	public MarkerScript()
	{
	}

	public void FloorDepth(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.floorDepth = markerScript.floorDepth + change * perc;
		if (this.floorDepth > 0f)
		{
			this.floorDepth = 0f;
		}
		this.oldFloorDepth = this.floorDepth;
	}

	public bool InSelected()
	{
		for (int i = 0; i < (int)this.objectScript.OCQOCOCQQOs.Length; i++)
		{
			if (this.objectScript.OCQOCOCQQOs[i] == base.gameObject)
			{
				return true;
			}
		}
		return false;
	}

	public void LeftIndent(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.ri = markerScript.ri + change * perc;
		if (this.ri < this.objectScript.indent)
		{
			this.ri = this.objectScript.indent;
		}
		this.OOQOQQOO = this.ri;
	}

	public void LeftSurrounding(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.rs = markerScript.rs + change * perc;
		if (this.rs < this.objectScript.indent)
		{
			this.rs = this.objectScript.indent;
		}
		this.ODOQQOOO = this.rs;
	}

	public void LeftTilting(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.rt = markerScript.rt + change * perc;
		if (this.rt < 0f)
		{
			this.rt = 0f;
		}
		this.ODDQODOO = this.rt;
	}

	private void OnDrawGizmos()
	{
		if (this.objectScript != null)
		{
			if (!this.objectScript.ODODCOCCDQ)
			{
				Vector3 vector3 = base.transform.position - this.oldPos;
				if (this.OCCCCODCOD && this.oldPos != Vector3.zero && vector3 != Vector3.zero)
				{
					int num = 0;
					Transform[] oCQOCOCQQOs = this.OCQOCOCQQOs;
					for (int i = 0; i < (int)oCQOCOCQQOs.Length; i++)
					{
						Transform transforms = oCQOCOCQQOs[i];
						transforms.position = transforms.position + (vector3 * this.trperc[num]);
						num++;
					}
				}
				if (this.oldPos != Vector3.zero && vector3 != Vector3.zero)
				{
					this.changed = true;
					if (this.objectScript.ODODCOCCDQ)
					{
						this.objectScript.OOQQCODOCD.specialRoadMaterial = true;
					}
				}
				this.oldPos = base.transform.position;
			}
			else if (this.objectScript.ODODDDOO)
			{
				base.transform.position = this.oldPos;
			}
		}
	}

	public void RightIndent(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.li = markerScript.li + change * perc;
		if (this.li < this.objectScript.indent)
		{
			this.li = this.objectScript.indent;
		}
		this.ODODQQOO = this.li;
	}

	public void RightSurrounding(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.ls = markerScript.ls + change * perc;
		if (this.ls < this.objectScript.indent)
		{
			this.ls = this.objectScript.indent;
		}
		this.DODOQQOO = this.ls;
	}

	public void RightTilting(float change, float perc)
	{
		MarkerScript markerScript = this;
		markerScript.lt = markerScript.lt + change * perc;
		if (this.lt < 0f)
		{
			this.lt = 0f;
		}
		this.ODDOQOQQ = this.lt;
	}

	private void SetObjectScript()
	{
		this.objectScript = base.transform.parent.parent.GetComponent<RoadObjectScript>();
		if (this.objectScript.OOQQCODOCD == null)
		{
			ArrayList arrayLists = ODODDCCOQO.OCDCQOOODO(false);
			this.objectScript.OCOQDDODDQ(arrayLists, ODODDCCOQO.OOQOOQODQQ(arrayLists), ODODDCCOQO.OQQDOODOOQ(arrayLists));
		}
	}

	private void Start()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				this.surface = (Transform)enumerator.Current;
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
	}
}