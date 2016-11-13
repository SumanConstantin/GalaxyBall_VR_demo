using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LevelModel : MonoBehaviour {
private static JSONNode jsonNode;

public static int levelID = 0;
public static int waveIDMax = 0;

private static int _waveID = 0;
public static int WaveID
{
	get{
		return _waveID;
	}
	set{
		_waveID = value; 
		GameEvent.WaveIdUpdated(value);
	}
}

private static object levelData;
//private static int[] waves;
//private static var jsonNode;

public static int[] bricks;

public static void InitLevelData(int _levelID)
{
	levelID = _levelID;

	if(jsonNode == null)
	{
		jsonNode = JSONNode.Parse(LevelVO.jsonLevels);
	}

	waveIDMax = jsonNode[levelID-1]["waves"].Count;
}

public static bool InitWave(int waveID)
{
	WaveID = waveID;

	bricks = new int[100];

	if(jsonNode[levelID-1]["waves"][waveID-1] == null)
	{
		return false;
	}

	JSONArray arr = jsonNode[levelID-1]["waves"][waveID-1]["bricks"].AsArray;
	int count = arr.Count;
	for( int i = 0; i < count; i++)
	{
		//print("i:"+i+"; "+arr[i].AsInt);
		bricks[i] = arr[i].AsInt;
	}

	return true;
}
}