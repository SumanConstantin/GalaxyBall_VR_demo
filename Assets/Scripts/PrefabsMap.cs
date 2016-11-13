using UnityEngine;
using System.Collections;

public class PrefabsMap : MonoBehaviour {

	public GameObject playerCharPref;
	public GameObject brickPref;
	public GameObject ballPref;
	
	public static GameObject playerCharPrefab;
	public static GameObject brickPrefab;
	public static GameObject ballPrefab;

	public void Init()
	{
		playerCharPrefab = playerCharPref;
		brickPrefab = brickPref;
		ballPrefab = ballPref;
	}
}
