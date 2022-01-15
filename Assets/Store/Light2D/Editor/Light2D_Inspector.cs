using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Light2D)) , CanEditMultipleObjects]
public class PointLight2D_Inspector : Editor {

	private SerializedObject obj;

	SerializedProperty shadowCaster_Mask;
	GUIContent shadowCaster_Mask_label;

    SerializedProperty detectableObject_Mask;
    GUIContent detectableObject_Mask_label;

    SerializedProperty angle;
	GUIContent angle_label;

	SerializedProperty Range;
	GUIContent range_label;

	SerializedProperty numberOfSegments;
	GUIContent numberOfSegments_label;

	SerializedProperty angleOffset;
	GUIContent angleOffset_label;

	SerializedProperty useEvents_SendToCaster;
	GUIContent useEvents_SendToCaster_label;

	SerializedProperty useEvents_SendToLight;
	GUIContent useEvents_SendToLight_label;

	SerializedProperty vertexDistanceTolerance;
	GUIContent vertexDistanceTolerance_label;

	SerializedProperty color;

	SerializedProperty optimisationFoldout;

	SerializedProperty showDebugLines;
	GUIContent showDebugLines_label;

    SerializedProperty isStatic;
    GUIContent isStatic_label;

    void OnEnable()
	{
		if(obj==null)
			obj = new SerializedObject(targets);

		if(vertexDistanceTolerance==null)
			vertexDistanceTolerance = obj.FindProperty("vertexDistanceTolerance");
		vertexDistanceTolerance_label = new GUIContent("Vertex Distance", "Minimum vertex distance for light mesh\nRecommended range 0-0.1");

		if(shadowCaster_Mask==null)
			shadowCaster_Mask = obj.FindProperty("shadowCaster_Mask");

		shadowCaster_Mask_label = new GUIContent("Caster", "The mask used for objects that cast shadows");

        if (detectableObject_Mask == null)
            detectableObject_Mask = obj.FindProperty("detectableObject_Mask");

        detectableObject_Mask_label = new GUIContent("Detectable", "The mask used for objects detected by light\nbut without casting shadow");

        if (Range==null)
			Range = obj.FindProperty("range");
		range_label = new GUIContent("Range","Light Range (Radius)");

		if(numberOfSegments==null)
			numberOfSegments = obj.FindProperty("numberOfSegments");

		numberOfSegments_label = new GUIContent("Segments","Number of default rays used\n Effectively angular resolution\nRecommended: 10-20");

		if(angleOffset==null)
			angleOffset = obj.FindProperty("angleOffset");
		angleOffset_label = new GUIContent("Angle Offset","Offset between penumbra raycasts\n decrease if light edge overlaps objects");

		if(useEvents_SendToCaster==null)
			useEvents_SendToCaster = obj.FindProperty("useEvents_SendToCaster");
		
		useEvents_SendToCaster_label = new GUIContent("Send To Caster","Use trigger events for objects entering and exiting light\nOnShadowCasterEnter/Exit called on shadowcasters\nDisable if not used to increase performance");


		if(useEvents_SendToLight==null)
			useEvents_SendToLight = obj.FindProperty("useEvents_SendToLight");

		useEvents_SendToLight_label = new GUIContent("Send To Light","Use trigger events for objects entering and exiting light\nOnShadowCasterEnter/Exit called on light\nDisable if not used to increase performance");


		if(showDebugLines==null)
			showDebugLines = obj.FindProperty("showDebugLines");
		showDebugLines_label = new GUIContent("Debug Lines","White: Initial raycast fan lines\n Cyan: Light Bounds\n Green: ShadowEdge");


		if(optimisationFoldout==null)
			optimisationFoldout = obj.FindProperty("optimisationFoldout");

		if(angle==null)
			angle = obj.FindProperty("totalCoverageAngle");
		angle_label = new GUIContent("Light Angle");

		if(color==null)
			color = obj.FindProperty("color");

        if (isStatic == null)
            isStatic = obj.FindProperty("isStatic");
        isStatic_label = new GUIContent("Static", "Is light static.\nTrue: Light is not recalulated\nFalse: Light recalculated each frame");


    }

    public override void OnInspectorGUI()
	{
		obj.Update();

       

		EditorGUILayout.Space();
		EditorGUI.indentLevel = 1;

		EditorGUILayout.PropertyField(Range,range_label);
		if(Range.floatValue<0)
			Range.floatValue=0;

		EditorGUILayout.PropertyField(angle,angle_label);

		EditorGUILayout.PropertyField(color);
		float intensity = color.colorValue.a;

		intensity = EditorGUILayout.Slider("intensity",intensity,0.0f,1.0f);
		if(color.colorValue.a != intensity)
			color.colorValue = new Color(color.colorValue.r,color.colorValue.g,color.colorValue.b,intensity);

		EditorGUILayout.Space();
        GUILayout.Label("Culling Masks");
        EditorGUILayout.PropertyField(shadowCaster_Mask,shadowCaster_Mask_label);
        EditorGUILayout.PropertyField(detectableObject_Mask, detectableObject_Mask_label);

        GUILayout.Label("Use Events");
		EditorGUILayout.PropertyField(useEvents_SendToCaster,useEvents_SendToCaster_label);
		EditorGUILayout.PropertyField(useEvents_SendToLight,useEvents_SendToLight_label);

		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(showDebugLines,showDebugLines_label);

		EditorGUI.indentLevel = 0;
		bool optimise = optimisationFoldout.boolValue;
		optimise = EditorGUILayout.Foldout(optimise,"Optimisations");

		if(optimise)
		{
			EditorGUI.indentLevel = 1;
			EditorGUILayout.PropertyField(angleOffset,angleOffset_label);
			if(angleOffset.floatValue<0)
				angleOffset.floatValue=0.0f;
			EditorGUILayout.PropertyField(numberOfSegments,numberOfSegments_label);
			if(numberOfSegments.intValue < 3)
				numberOfSegments.intValue=3;
			EditorGUILayout.PropertyField(vertexDistanceTolerance,vertexDistanceTolerance_label);
			if(vertexDistanceTolerance.floatValue<0)
				vertexDistanceTolerance.floatValue=0.0f;

            EditorGUILayout.PropertyField(isStatic, isStatic_label);
            if (GUILayout.Button("Recalculate"))
                recalculateLight();
            EditorGUI.indentLevel = 0;
		}
		if(optimise!=optimisationFoldout.boolValue)
			optimisationFoldout.boolValue = optimise;

        


		obj.ApplyModifiedProperties();
	}

    private void recalculateLight()
    {
        Light2D t = (Light2D)target;
        t.RecalculateLight();
    }
     
	[MenuItem("GameObject/Light2D/Light2D")]
	static void CreateLightStandard()
	{
		
		GameObject go =(GameObject) AssetDatabase.LoadAssetAtPath("Assets/Light2D/prefabs/Light2D.prefab", typeof(GameObject));

		if(Camera.current!=null)
			go.transform.position = new Vector3(Camera.current.transform.position.x,
				Camera.current.transform.position.y,0);
		

		PrefabUtility.InstantiatePrefab(go);
	}
	[MenuItem("GameObject/Light2D/Occlusion Light")]
	static void CreateLightOcclusion()
	{
		GameObject go =(GameObject) AssetDatabase.LoadAssetAtPath("Assets/Light2D/prefabs/OcclusionLight2D.prefab",typeof(GameObject));
		if(Camera.current!=null)
			go.transform.position = new Vector3(Camera.current.transform.position.x,
				Camera.current.transform.position.y,0);
		
		PrefabUtility.InstantiatePrefab(go);
	}
}
	