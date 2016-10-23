using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/CutScene")]
public class CutScene : ScriptableObject {

	public List<CutSceneEvent> events;
}
