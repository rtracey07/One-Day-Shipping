using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/* Editor Window to help with building paths in-game. */
public class PathwayEditorWindow : EditorWindow {

	private string k_SaveDirectory = "Assets/Data/Pathways/";		//Project directory for pathway objects.
	private string m_PathwayName = "Test";							//Name of new pathway.
	private int m_PathwayCount;										//Number of points on path.
	private int m_CurrVertex;										//Current point on path.
	private bool m_AddVertices = false;								//AddVertices.
	private bool m_ShowVertex = false;								//Show Vertices.
	private Pathway m_Pathway;										//Pathway 

	private GameObject pathHelper;
	private Vector3 m_CurrPosition = Vector3.zero;

	[MenuItem("Custom/Windows/Pathway Creator Tool")]
	public static void InitWindow()
	{
		EditorWindow.GetWindow<PathwayEditorWindow> ();
	}

	/* On UI Drawn. */
	void OnGUI()
	{
		NameFields ();
		CreateButton ();
		AssignVertices ();
	}

	/* Name Entering UI Area. */
	void NameFields()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Pathway Name:");
		m_PathwayName = GUILayout.TextField (m_PathwayName);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Pathway Size:");
		m_PathwayCount = EditorGUILayout.IntField (m_PathwayCount);
		GUILayout.EndHorizontal ();
	}

	/* UI Area to house the "Create" Button. */
	void CreateButton()
	{
		if (GUILayout.Button ("Create")) {
			m_Pathway = ScriptableObject.CreateInstance<Pathway> ();
			m_Pathway.pathway = new Vector3[m_PathwayCount];
			for (int i = 0; i < m_PathwayCount; i++)
				m_Pathway.pathway[i] = Vector3.zero;

			string pathwayDir = k_SaveDirectory+m_PathwayName+".asset";
			AssetDatabase.CreateAsset (m_Pathway, pathwayDir);
			m_AddVertices = true;
			m_CurrVertex = 0;
		}
	}

	/* UI Area to add vertices to path. */
	void AssignVertices()
	{
		if (m_AddVertices) {
			GUILayout.BeginHorizontal ();
			m_ShowVertex = GUILayout.Button ("Edit Vertex " + m_CurrVertex);

			if (m_ShowVertex) {
				DrawVertexGizmo ();
			}
			if (GUILayout.Button ("Confirm Vertex Position")) {

				if (pathHelper != null)
					m_CurrPosition = pathHelper.transform.position;
				
				m_Pathway.pathway[m_CurrVertex] = m_CurrPosition;

				m_CurrVertex++;
				if (m_CurrVertex >= m_Pathway.pathway.Length) {
					m_AddVertices = false;
					m_CurrVertex = 0;
				}
			}
			GUILayout.EndHorizontal ();
		}
	}

	/* Draw Gizmo During Edit Mode. */
	void DrawVertexGizmo()
	{
		if (SceneView.sceneViews.Count != 0) {
			SceneView sceneView = SceneView.lastActiveSceneView;
			sceneView.Focus ();

			if(pathHelper == null)
				pathHelper = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Utility/PathHelper.prefab"));

			sceneView.LookAt (pathHelper.transform.position);
		}
	}

	/* Clear instantiated data close. */
	void OnDestroy()
	{
		DestroyImmediate (pathHelper);
		m_Pathway = null;
	}
}
