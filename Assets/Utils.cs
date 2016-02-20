using System.Collections;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
	public static readonly string historyPath = PathURL + "history.txt";
	public static readonly string splitStr = "|fanqietang|";

	public static string readJsonFile(string filePath) {
		string text = File.ReadAllText(@filePath);
		return text;
	}
	
	public static void saveRecord(DialogInfo dialogInfo, int num, string curJsonFile, int curId) {
		saveHistory (dialogInfo, num);
		string[] lines = { curJsonFile, curId.ToString() };
		File.WriteAllLines(@recordPath, lines, Encoding.UTF8);
	}

	private static void saveHistory(DialogInfo dialogInfo, int num){
		Debug.Log (dialogInfo.getContent());
		StreamWriter sw = new StreamWriter (@historyPath, true);
		if (!File.Exists (@historyPath)) {
			sw.Write("");
		}
		if (dialogInfo.getType () == DialogType.Dialog) {
			sw.WriteLine(dialogInfo.getContent());
		} else {
			sw.WriteLine(num.ToString() + splitStr + dialogInfo.getSelect()[0] + splitStr + dialogInfo.getSelect()[1]);
		}
		sw.Close();
	}

	public static List<DialogInfo> readHistory(){
		List<DialogInfo> historyList = new List<DialogInfo> ();
		if (File.Exists (@historyPath)) {
			string[] lines = File.ReadAllLines (@historyPath);
			foreach (string line in lines){
				DialogInfo di = new DialogInfo();
				if (line.Contains(splitStr)){
					di.setType(DialogType.Select);
					string[] info = Regex.Split(line, splitStr, RegexOptions.IgnoreCase);
					Option op1 = new Option();
					op1.setOption(info[1]);
					Option op2 = new Option();
					op2.setOption(info[2]);
					int clicked = int.Parse(info[0]);
					if (clicked == 0){
						op1.setClicked(true);
					}else{
						op2.setClicked(true);
					}
					di.addOption(op1);
					di.addOption(op2);
				}else{
					di.setType(DialogType.Dialog);
					di.setContent(line);
				}
				historyList.Add(di);
			}
			return historyList;
		}
		return historyList;
	}

	public static void clearHistoryRecord(){
		if (File.Exists (@historyPath)) {
			File.Delete(@historyPath);
		}
		if (File.Exists (@recordPath)) {
			File.Delete(@recordPath);
		}
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
