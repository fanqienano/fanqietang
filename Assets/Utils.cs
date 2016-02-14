using System.Collections;
using System.Text;
using System.IO;
using UnityEngine;

public class Utils {
	public static readonly string PathURL =
	#if UNITY_ANDROID   //安卓
		"jar:file://" + Application.dataPath + "!/assets/";
	#elif UNITY_IPHONE  //iPhone
		Application.dataPath + "/Raw/";
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
		Application.dataPath + "/StreamingAssets/";
	#else
		string.Empty;
	#endif
	public static readonly string mainJsonPath = PathURL + "main.json";
	public static readonly string recordPath = PathURL + "record.txt";

	public static string readJsonFile(string filePath) {
		string text = File.ReadAllText(@filePath);
		return text;
	}
	
	public static void saveRecord(string curJsonFile, int curId) {
		string[] lines = { curJsonFile, curId.ToString() };
		File.WriteAllLines(@recordPath, lines, Encoding.UTF8);
	}
	
	public static string[] readRecord() {
		if (File.Exists (@recordPath)) {
			string[] lines = File.ReadAllLines (@recordPath);
			return lines;
		}
		return new string[] {mainJsonPath, "0"};
	}

}
