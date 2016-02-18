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
	public static readonly string wakeTimeStamp = PathURL + "wake.txt";

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

	public static long saveWakeTimeStamp(long delay){
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
		System.DateTime nowTime = System.DateTime.Now;
		long unixTime = (long)System.Math.Round((nowTime - startTime).TotalMilliseconds, System.MidpointRounding.AwayFromZero);
		long wake = unixTime + delay;
		string[] lines = { wake.ToString() };
		File.WriteAllLines(@wakeTimeStamp, lines, Encoding.UTF8);
		return wake;
	}

	public static long getTimeStampNow(){
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
		System.DateTime nowTime = System.DateTime.Now;
		long unixTime = (long)System.Math.Round((nowTime - startTime).TotalMilliseconds, System.MidpointRounding.AwayFromZero);
		return unixTime;
	}

	public long readWakeTimeStamp(){
		if (File.Exists (@wakeTimeStamp)) {
			string[] lines = File.ReadAllLines (@wakeTimeStamp);
			long time = long.Parse(lines[0]);
			return time;
		}
		return 0;
	}

	public static string getVoicePath(string voice){
		return PathURL + voice;
	}

}
