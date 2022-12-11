/* Copyright (C) Discourse Games - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibted
 * Proprietary and confidential
 * Written by Keelan Moore <km.keelan@gmail.com>,  2016 */

using UnityEngine;
using System.Collections.Generic;




[RequireComponent(typeof(MeshRenderer) , typeof(MeshFilter))]
[ExecuteInEditMode]
public class Light2D : MonoBehaviour {

    // Vertices of the light mesh in local space
	private List<Vector2> lightVertices = new List<Vector2>();
    // List of gameobjects currently casting a shadow 
    private List<GameObject> shadowCasters = new List<GameObject>();
	// Public accessor for objects within lit area.
	public List<GameObject> ShadowCasters{
		get{ return shadowCasters;}
	}
    private List<GameObject> detectableObjects = new List<GameObject>();
	// Public accessor for objects within lit area.
	public List<GameObject> DetectableObjects{
		get{ return detectableObjects;}
	}
	// Layermask - 
	public LayerMask shadowCaster_Mask;
	public LayerMask detectableObject_Mask;

	/* Total angle the light covers:
	 * Defaults to 360.0 for point light
	 * Between 0.0 to 180 for spot light*/
	[Range(0.0f,360.0f),SerializeField]
	protected float totalCoverageAngle = 360.0f;
	public float TotalCoverageAngle {
		get {return totalCoverageAngle;}
		set{
			if(value>360.0f) totalCoverageAngle 		= 360.0f;
			else if(value<00.0f) totalCoverageAngle 	= 0.0f;
			else totalCoverageAngle					= value;
		}
	}
	// List of gameobjects currently being detected by a raycast
	private List<GameObject> rayHit = new List<GameObject>();
    // Range of light
    
    [SerializeField]
    private float range = 10.0f;
    public float Range
    {
        get { return range; }
        set
        {
            if (value < 0)
                range = 0;
            else
                range = value;
        }
    }

    
    // Number of default rays used: Effectively angular resolution
    [SerializeField]
    private int numberOfSegments=15;
    public int NumberOfSegments
    {
        get { return numberOfSegments; }
        set { if (value < 3)
                numberOfSegments = 3;
            else
                numberOfSegments = value;
        }
            
    }
    // Offset between penumbra raycasts
    // decrease if shadow edge is skewed

    [SerializeField]
    private float angleOffset = 0.001f;
    public float AngleOffset
    {
        get { return angleOffset; }
        set
        {
            if (value < 0)
                angleOffset = 0;
            else
                angleOffset = value;
        }
    }
    // Minimum allowable light vertex distances,
    // Optimisation to remove unneccesary vertices.
    [SerializeField]
    private float vertexDistanceTolerance = 0.01f;
    public float VertexDistanceTolerance
    {
        get { return vertexDistanceTolerance; }
        set
        {
            if (value < 0)
                vertexDistanceTolerance = 0;
            else
                vertexDistanceTolerance = value;
        }
    }


    // If static, then don't recalculate
    // light each frame.
    public bool isStatic = false;

	/* Use the event system
	 * If true-light will send messages to objects upon entering 
	 * and exiting the lit area - See documentation*/
	public bool useEvents_SendToLight = true;
	public bool useEvents_SendToCaster = true; 
	// Pre-initialisation of temporary variables.
	private RaycastHit2D hit;
	private RaycastHit2D hit1;
	private Vector2[] BoxColliderPoints= new Vector2[4];
    private Vector2[] EdgeColliderPoints;
    private Vector2[] CircleColliderPoints= new Vector2[2];
	private Vector2 v1;
	private Vector2 v2;
	private float f1;
	private float f2;
	private float f3;
	private int numberOfCasters;

	/* Show debug lines
	 * White: 	Initial raycast fan lines
	 * Cyan: 	Light Bounds
	 * Green: 	ShadowEdge */

	#if UNITY_EDITOR
	public bool showDebugLines=false;
	public bool optimisationFoldout = false;
	#endif

	/* Point outside of any collider: Used in detecting whether 
	 * a caster is within a light source. I.e this must be larger 
	 than any object in the scene*/

	// Mesh Variables
	Mesh m;
	MeshFilter mf;
	private Vector3[] verts;
	private Vector3[] normals;
	private Vector2[] uvs;
	
	private Color[] meshColor;
	private int noTris;
	private int[] tris;

    public Color color = Color.white;

    [SerializeField]
    private float intensity = 1;
    public float Intensity
    {
        get
        {
            return color.a;
        }
        set
        {
            intensity = value;
            color = new Color(color.r, color.g, color.b, value);
        }
    } 

	void Start () {
		mf = this.GetComponent<MeshFilter>();		
		m = new Mesh();
        shadowCasters.Clear();
        detectableObjects.Clear();
        RecalculateLight();
    }

    

	void LateUpdate()
	{

        if (!isStatic)
            RecalculateLight();

	}
    
   

    public void RecalculateLight()
    {
        
        // First Pass
        // Raycast in a fan around orgin
        raycastFan();
        // Raycast at all points of all shadowcasters
        // to generate shadows.
        CalculateShadowCasterAll();
        // Raycast around perimeter to catch objects
        // in between light segments
        raycastPerimeter();
        // Proccess hit objects, remove all objects from
        // shadowCasters and detectableObjects if outside lit region
        ProccessHitObjects();
        // Build the light mesh
        buildMesh();
    }

	protected void raycastFan()
	{
		// Start angle - centered around x-axis
		f1 = totalCoverageAngle/2;
		// Angle Step
		f2 = (totalCoverageAngle / (numberOfSegments));
		// Clearing lists from last frame
		lightVertices.Clear ();
		// add rayHit current to last frame buffer then clear
		rayHit.Clear ();


		/* Casting rays out in a fan, if ray detects object add to current frame list and
		 * place vertex at point of hit if ray detects nothing, place mesh vertex along ray
		 * by a distance equal to its range*/

		for (int i  = 0; i <= numberOfSegments; i++) {

			// Raycast
			hit = Physics2D.Raycast(transform.position , Quaternion.AngleAxis(f1 , Vector3.forward) * transform.right  , Range, shadowCaster_Mask | detectableObject_Mask);

            

            if(hit.collider!=null)
			{
				hitDetected(hit,this.transform.InverseTransformPoint(hit.point));
				#if UNITY_EDITOR
				if(showDebugLines)
					Debug.DrawLine(transform.position , hit.point);
				#endif
			}
			else
			{
				AddLightVertex((Quaternion.AngleAxis(f1 , Vector3.forward) * Vector2.right * range));
				#if UNITY_EDITOR
				if(showDebugLines)
					Debug.DrawLine(transform.position , 
						transform.position + Quaternion.AngleAxis(f1 , Vector3.forward) * transform.right * range);
				#endif
			}
			// Stepping angle
			f1-=f2;
		}

	}
	protected void CalculateShadowCasterAll()
	{
		/* Loop over each found shadowCaster
		 * If one is found along shadow edge, add it to
		 * the list*/
		if(shadowCasters!=null && shadowCasters.Count!=0)
		{
			numberOfCasters = shadowCasters.Count;
			int i = 0;
			while(i < numberOfCasters)
			{
                if((shadowCasters.Count-1)>=i)
					calculateShadowCaster(shadowCasters[i]);
				else
					RemoveObject(shadowCasters[i],0);
				i++;
			}
		}
	}

	void calculateShadowCaster(GameObject caster)
	{
        /* Determine type of collider first, 
		 * then select points for shadowcasting depending 
		 * on collider type*/
        if (caster == null)
            return;

        Collider2D[] colliders = caster.GetComponents<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetType() == typeof(BoxCollider2D))
            {
                // If BoxCollider2D found

                v2 = ((BoxCollider2D)(colliders[i])).size;
                v1 = ((BoxCollider2D)(colliders[i])).offset;

                // Send positions of the 4 corners of the box
                BoxColliderPoints[0] = new Vector2(v1.x + v2.x / 2, v1.y + v2.y / 2);
                BoxColliderPoints[1] = new Vector2(v1.x + v2.x / 2, v1.y - v2.y / 2);
                BoxColliderPoints[2] = new Vector2(v1.x - v2.x / 2, v1.y + v2.y / 2);
                BoxColliderPoints[3] = new Vector2(v1.x - v2.x / 2, v1.y - v2.y / 2);
                // Cast shadow based on this point
                raysOnShadowCasterPoints(BoxColliderPoints, caster.transform);
            }
            else if (colliders[i].GetType() == typeof(PolygonCollider2D))
            {
                //If PolygonCollider2D found
                // Send positions of all the points of the polygon
                raysOnShadowCasterPoints(caster.GetComponent<PolygonCollider2D>().points,
                    caster.transform);
            }
            else if (colliders[i].GetType() == typeof(EdgeCollider2D))
            {
                // If EdgeCollider2D found,
                // Send position of the 2 ends of the edge

                // FIXME - Edge collider with offset not working

                

                EdgeCollider2D edge = caster.GetComponent<EdgeCollider2D>();
                EdgeColliderPoints = edge.points;
                if (edge.offset != Vector2.zero)
                {
                    for (int j = 0; j < EdgeColliderPoints.Length; j++)
                        EdgeColliderPoints[j] += edge.offset;
                }
                
                raysOnShadowCasterPoints(EdgeColliderPoints,caster.transform);
            }

            else if (colliders[i].GetType() == typeof(CircleCollider2D))
            {
                // Get CircleCollider2D
                CircleCollider2D col = caster.GetComponent<CircleCollider2D>();
                // Find the scaled radius - using x scale 
                f1 = col.radius * col.transform.lossyScale.x;
                // Vector from circle center to light
                v1 = (col.transform.position + 
                    Vector3.Scale(col.transform.TransformDirection(col.offset), col.transform.lossyScale)) - this.transform.position;
                // Length of tangents
                f2 = Mathf.Sqrt(v1.sqrMagnitude - f1 * f1);
                // Angle
                f3 = Mathf.Atan2(f1, f2);
                // Vector of correct length. 
                v1 = v1.normalized * f2;



                // Rotate vector of correct length by +- the angle calculated above
                CircleColliderPoints[0] = col.transform.InverseTransformPoint(
                    transform.position + (new Vector3(v1.x * Mathf.Cos(f3) - v1.y * Mathf.Sin(f3),
                        v1.x * Mathf.Sin(f3) + v1.y * Mathf.Cos(f3), 0)));
                CircleColliderPoints[1] = col.transform.InverseTransformPoint(
                    transform.position + (new Vector3(v1.x * Mathf.Cos(-f3) - v1.y * Mathf.Sin(-f3),
                        v1.x * Mathf.Sin(-f3) + v1.y * Mathf.Cos(-f3), 0)));
                raysOnShadowCasterPoints(CircleColliderPoints, col.transform);

            }
        }
	}



	protected void raysOnShadowCasterPoints (Vector2[] points , Transform casterTransform)
	{

		// Cast two rays out towards each point, rotated by a small angle
		// one will hit the object, the other will pass on - casting the 
		// shadow.
		for(int i = 0; i<points.Length ;i++)
		{

			v2 = (Vector2)(casterTransform.TransformPoint(points[i])-this.transform.position);



			if(totalCoverageAngle<360.0f) {
				// If total angle coverage is less than 180
				// then it is a spotlight - Skip raycast if point is
				// outside lit area.
				if(Vector2.Dot(transform.right,v2.normalized) < Mathf.Cos(Mathf.Deg2Rad*totalCoverageAngle*0.5f))
					continue;
			}

			// Cast two rays 
			hit =  Physics2D.Raycast(transform.position,
				Quaternion.AngleAxis(angleOffset,Vector3.forward)*
				v2,
				range,shadowCaster_Mask);
			hit1 =  Physics2D.Raycast(transform.position,
				Quaternion.AngleAxis(-angleOffset,Vector3.forward)*
				v2,
				range,shadowCaster_Mask);

			//Check hit data - if found then return the hit point
			// if not found then return range at ray direction
			// This is done for both raycasts above.
			if(hit.collider!=null) {
                hitDetected(hit, transform.InverseTransformPoint(hit.point));
                if ((hit.collider.transform != casterTransform))
					numberOfCasters = shadowCasters.Count;
//#if UNITY_EDITOR
//                if (showDebugLines)
//                    Debug.DrawLine(transform.position,
//                        transform.position + Quaternion.AngleAxis(angleOffset, Vector3.forward) * v2.normalized * range,
//                        Color.red);
//#endif
            }
            else
            {				
				AddLightVertex( transform.InverseTransformPoint(transform.position+Quaternion.AngleAxis(angleOffset,Vector3.forward)* v2.normalized * range));

				#if UNITY_EDITOR
				if(showDebugLines)
					Debug.DrawLine(transform.position,
						transform.position + Quaternion.AngleAxis(angleOffset,Vector3.forward)* v2.normalized * range ,
						Color.green);
				#endif
			}
			// Second Raycast testing			
			if(hit1.collider!=null){
                hitDetected(hit1, this.transform.InverseTransformPoint(hit1.point));
                if ((hit1.collider.transform != casterTransform))
					numberOfCasters = shadowCasters.Count;
//#if UNITY_EDITOR
//                if (showDebugLines)
//                    Debug.DrawLine(transform.position,
//                        transform.position + Quaternion.AngleAxis(angleOffset, Vector3.forward) * v2.normalized * range,
//                        Color.red);
//#endif
            }
            else
			{
				AddLightVertex( transform.InverseTransformPoint(transform.position+Quaternion.AngleAxis(-angleOffset,Vector3.forward)* v2.normalized * range));
				#if UNITY_EDITOR
				if(showDebugLines)
					Debug.DrawLine(transform.position , transform.position+Quaternion.AngleAxis(-angleOffset,Vector3.forward)* v2.normalized * range,
						Color.blue);
				#endif
			}
		}
	}

	protected void raycastPerimeter()
	{
		sortLightVertices ();
		/* Cast rays between all found light verts i.e casting rays 
		 * along the edge of the light mesh*/
		for(int i = 0 ; i<lightVertices.Count-1 ;i++)
		{

			if(i==(lightVertices.Count-1))				//Casting rays between points
			{
				v1 = this.transform.TransformPoint((lightVertices[lightVertices.Count-1]));
				v2 = this.transform.TransformPoint((lightVertices[0]));
			}else
			{

				v1 = this.transform.TransformPoint((lightVertices[i]));
				v2 = this.transform.TransformPoint((lightVertices[i+1]));
			}

			f1 = Vector2.Distance(v1, v2);
			hit = Physics2D.Raycast(v1,
				v2 -v1,
				f1, shadowCaster_Mask | detectableObject_Mask);
			#if UNITY_EDITOR
			if(showDebugLines)
				Debug.DrawLine(v1,v2,Color.cyan);
			#endif

            
			if(hit) {
				hitDetected(hit);
			}

		}
	}

    protected void ProccessHitObjects()
    {
        /* Find all shadowCasters that were not hit this frame
		check whether they are in the lit area, if not remove the
		shadowCaster*/

		for(int i = shadowCasters.Count-1; i>=0;i--)
		{
			
			if(!rayHit.Contains(shadowCasters[i]))	// if was being hit last frame but not this frame
			{		
				if(!IsPointInPolygon(shadowCasters[i] ) )
					RemoveObject(shadowCasters[i],0);
			}
		}

		for(int i = detectableObjects.Count-1; i>=0;i--)
		{

			if(!rayHit.Contains(detectableObjects[i]))	// if was being hit last frame but not this frame
			{		
				if(!IsPointInPolygon(detectableObjects[i] ) )
					RemoveObject(detectableObjects[i],1);
			}
		}
    }
    
	protected void RemoveObject(GameObject obj, int objectType)
	{
        // If specified - send message to shadow caster telling it it has left
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (useEvents_SendToCaster)
                    obj.SendMessage("OnLight2DExit", this.gameObject, SendMessageOptions.DontRequireReceiver);
                if (useEvents_SendToLight)
                    gameObject.SendMessage("OnLight2DExit", obj, SendMessageOptions.DontRequireReceiver);
            }
        }
		switch (objectType) {
		case 0:
			// Shadow Caster has entered.
			shadowCasters.Remove(obj);
			break;
		case 1:
			// Detectable Object has entered
			detectableObjects.Remove(obj);
			break;
		}
	}
	protected void AddObject(GameObject obj, int objectType)
	{
        if (Application.isPlaying)
        {
            
            if (useEvents_SendToCaster)
                obj.SendMessage("OnLight2DEnter", this, SendMessageOptions.DontRequireReceiver);
            if (useEvents_SendToLight)
                this.gameObject.SendMessage("OnLight2DEnter", obj, SendMessageOptions.DontRequireReceiver);
        }
		
		switch (objectType) {
		case 0:
			// Shadow Caster has entered.
			shadowCasters.Add(obj);
			break;
		case 1:
			// Detectable Object has entered
			detectableObjects.Add(obj);
			break;
		}
	}
		
	protected void AddLightVertex(Vector2 pt)
	{
		if(lightVertices.Count < numberOfSegments)
		{
			lightVertices.Add(pt);
			return;
		}

		if(Vector2.SqrMagnitude(pt - lightVertices[lightVertices.Count-1]) < vertexDistanceTolerance)
			return;

		lightVertices.Add(pt);			
		
	}
	protected void hitDetected(RaycastHit2D hit)
	{
		GameObject go = hit.collider.gameObject;


		//if not on list, add to currentFrameBuffer
		if(!rayHit.Contains(go))
			rayHit.Add(go);

		if (((1 << go.layer) & shadowCaster_Mask) != 0) {
			// Object can cast shadows
			if (!shadowCasters.Contains (go)) {
				AddObject (go,0);
			}
		} else if (((1 << go.layer) & detectableObject_Mask) != 0) {
			// Object is detectable
			if (!detectableObjects.Contains (go))
				AddObject (go,1);
		}

	}
	protected void hitDetected(RaycastHit2D hit, Vector2 v)
	{
		GameObject go = hit.collider.gameObject;


		//if not on list, add to currentFrameBuffer
		if(!rayHit.Contains(go))
			rayHit.Add(go);

		if (((1 << go.layer) & shadowCaster_Mask) != 0) {
			// Object can cast shadows
			if (!shadowCasters.Contains (go)) {
				AddObject (go,0);
			}
            AddLightVertex(v);

        } else if (((1 << go.layer) & detectableObject_Mask) != 0) {
			// Object is detectable
			if (!detectableObjects.Contains (go))
				AddObject (go,1);

            AddLightVertex(v.normalized * range);
		}
	}


	protected bool IsPointInPolygon  (GameObject shadowCaster) {

        if (shadowCaster == null)
            return false;

        v1 = (shadowCaster.transform.position-this.transform.position);
		hit = Physics2D.Raycast(this.transform.position , v1, range);

        if (totalCoverageAngle < 360.0f)
        {
            // If total angle coverage is less than 180
            // then it is a spotlight - Skip raycast if point is
            // outside lit area.
            if (Vector2.Dot(transform.right, v1.normalized) < Mathf.Cos(Mathf.Deg2Rad * totalCoverageAngle * 0.5f))
                return false;
        }

        if (hit.collider!=null)
			if(hit.collider.gameObject == shadowCaster)
			return true;

		return false;
	}

	protected void buildMesh()
	{
		verts = new Vector3[lightVertices.Count+1];
		normals = new Vector3[lightVertices.Count+1];
		uvs = new Vector2[lightVertices.Count+1];
		meshColor = new Color[lightVertices.Count+1];


		// Placing vertex position,normals and uvs
		for(int i=0;i<verts.Length-1;i++)
		{
			verts[i] 	=  lightVertices[i];
			normals[i] = Vector3.forward;
			meshColor[i] = color;

			uvs[i] = new Vector2(0.5f + (verts[i].x)/(2*(range)) , 
				0.5f + (verts[i].y)/(2*(range)));

		}
		// Setting the center vertex position,normal and uv.
		verts[verts.Length-1] = Vector3.zero;
		normals[verts.Length-1] = Vector3.up;
		meshColor[verts.Length-1] = color;
		uvs[verts.Length-1] = new Vector2(0.5f,0.5f);

		noTris = (verts.Length-2)*3;
		tris = new int[noTris];
		for(int x = 0 ; x<noTris ;x+=3)
		{
			tris[x] = verts.Length-1;
			tris[x+1] = (int)(x/3);
			tris[x+2] = (int)((x/3)+1);
		}

		// Appying newly set mesh data
		m.Clear();
		m.vertices = verts;
		m.normals = normals;
		m.colors = meshColor;
		m.triangles = tris;
		m.uv = uvs;
		// Applying mesh to the mesh filter
		mf.mesh = m;
    }

	protected void sortLightVertices()
	{
        // Sort light vertices clockwise sense ensuring the correct
        // start and end points.
        if (lightVertices.Count < numberOfSegments+1)
            return;

		Vector2 a = lightVertices[numberOfSegments];
		lightVertices[numberOfSegments] = lightVertices[lightVertices.Count-1];
		lightVertices[lightVertices.Count-1] = a;

		for(int i = 2; i<lightVertices.Count-1 ; i++)
		{
			int j = i;
			while(j>1)
			{
				if(v2Atan(lightVertices[j-1]) < v2Atan(lightVertices[j]))
				{
					Vector2  temp = lightVertices[j-1];
					lightVertices[j-1] = lightVertices[j];
					lightVertices[j] = temp;
					j--;
				}
				else
					break;
			}
		}
	}

	private float v2Atan(Vector2 v)
	{
		return Mathf.Atan2(v.y,v.x);
	}

    public void setAngle(float angle)
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }

    public void setColorWithoutAlpha(Color newC)
    {
        f1 = intensity;
        color = newC;
        color.a = f1;
    }

   
}

