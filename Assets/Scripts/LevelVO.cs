using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LevelVO
{
	// TODO:
	// These levels should be created using and editor.
	// The editor should save the levels in a json file.
	// On game init the level json files should be loaded and 
	// LevelVO objects should be initialized
	// This class should not "be static".
	// A levelController should manage the levelVO objects.
	public static string jsonLevels =
		"[" +
			"{\"id\":1, waves:[" +
								// Wave 1
								"{\"delayStart\":1, \"bricks\":[" +
									"0,0,0,0,0,0,0,0,0,-1," +
									"0,0,0,0,0,0,0,0,0,-1," +
									"0,0,0,0,0,0,0,0,0,-1," +
									"0,0,0,0,0,0,0,0,0,-1," +
									"0,0,0,0,1,0,0,0,0,-1," +
									"]" +
								"}" +
								// Wave 2
								"," +
								"{\"delayStart\":1, \"bricks\":[" +
									"0,0,0,0,0,0,0,0,0,-1," +
									"0,0,0,1,0,1,0,0,0,-1," +
									"0,0,1,0,0,0,1,0,0,-1," +
									"0,0,0,0,1,0,0,0,0,-1," +
									"0,0,0,0,0,0,0,0,0,-1," +									
									"]" +
								"}" +
								// Wave 3
								"," +
								"{\"delayStart\":1, \"bricks\":[" +
								"0,0,0,0,0,0,0,0,0,-1," +
								"0,1,1,1,1,1,1,1,0,-1," +
								"1,1,0,1,1,1,0,1,1,-1," +
								"0,1,1,1,1,1,1,1,0,-1," +
								"0,0,1,1,0,1,1,0,0,-1," +
								"]" +
								"}" +


							 "]" +
			"}" + 
		"]";


}