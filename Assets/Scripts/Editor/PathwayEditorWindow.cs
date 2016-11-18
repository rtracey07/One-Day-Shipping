using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PathwayEditorWindow : EditorWindow {

	private string k_SaveDirectory = "Assets/Data/Pathways/";
	private string m_PathwayName = "Test";
	private int m_PathwayCount;
	private int m_CurrVertex;
	private bool m_AddVertices = false;
	private bool m_ShowVertex = false;
	private Pathway m_Pathway;

	private GameObject pathHelper;

	private Vector3 m_CurrPosition = Vector3.zero;

	[MenuItem("Custom/Windows/Pathway Creator Tool")]
	public static void InitWindow()
	{
		EditorWindow.GetWindow<PathwayEditorWindow> ();
	}

	void OnGUI()
	{
		NameFields ();
		CreateButton ();
		AssignVertices ();
	}

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

	void CreateButton()
	{
		if (GUILayout.Button ("Create")) {
			m_Pathway = ScriptableObject.CreateInstance<Pathway> ();
			m_Pathway.pathway = new List<Vector3> (m_PathwayCount);
			for (int i = 0; i < m_PathwayCount; i++)
				m_Pathway.pathway.Add (Vector3.zero);

			string pathwayDir = k_SaveDirectory+m_PathwayName+".asset";
			AssetDatabase.CreateAsset (m_Pathway, pathwayDir);
			m_AddVertices = true;
			m_CurrVertex = 0;
		}
	}

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
				if (m_CurrVertex >= m_Pathway.pathway.Count) {
					m_AddVertices = false;
					m_CurrVertex = 0;
				}
			}
			GUILayout.EndHorizontal ();
		}
	}

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

	void OnDestroy()
	{
		DestroyImmediate (pathHelper);
		m_Pathway = null;
	}
}
