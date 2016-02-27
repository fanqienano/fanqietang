using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainListView : MonoBehaviour {

	private GameObject mainListView;
	private GameObject mainListViewGrid;
	private Text textExample;
	private GameObject selectButtonExample;
	private ReadJson readJson;
	private long wakeTimeStamp = 0;
	private bool isWaiting = false;
	private bool isDelay = false;
	private bool isFinish = false;
	private long delayTime = 0;
	private long wakeTime = 0;
	private bool needNext = true;
	private AudioSource audioSource;
	
	private void initExampleGameObject(){
		mainListView = GameObject.Find("MainActivity/MainListView");
		mainListViewGrid = GameObject.Find("MainActivity/MainListView/Grid");
		textExample = GameObject.Find ("MainActivity/Text").GetComponent<Text>();
		selectButtonExample = GameObject.Find ("MainActivity/SelectButton");
		selectButtonExample.SetActive (false);
	}

	public void playVideo(string str){
		if (audioSource.isPlaying){
			audioSource.Stop();
		}
		audioSource.clip = (AudioClip)Resources.Load(str, typeof(AudioClip));//调用Resources方法加载AudioClip资源
		audioSource.Play();
	}

	private void addItem(DialogInfo di){
		if (!di.getVoice ().Equals (string.Empty)) {
			playVideo(di.getVoice ());
		}
		if (di.getType() == DialogType.Dialog) {
			addTextItem (di);
		} else {
			addSelectButton(di);
		}
		if (di.getDelay () > 0) {
			Utils.saveWakeTimeStamp(di.getDelay ());
		}
	}

	private void addTextItem(DialogInfo di){
		Text newText = Instantiate(textExample);
		newText.text = di.getContent ();
		newText.transform.SetParent (mainListViewGrid.transform);
		newText.transform.localScale = new Vector3 (1, 1, 1);
	}

	private void addSelectButton(DialogInfo di){
		GameObject newSelectButton = Instantiate(selectButtonExample);
		Button buttonLeft = newSelectButton.transform.GetChild (0).GetComponent<Button>();
		buttonLeft.transform.GetChild (0).GetComponent<Text> ().text = di.getSelect () [0].getOption ();
		buttonLeft.onClick.AddListener (delegate() {
			this.clickButton (newSelectButton, di.getSelect () [0], 0); 
		});
		Button buttonRight = newSelectButton.transform.GetChild (1).GetComponent<Button>();
		buttonRight.transform.GetChild (0).GetComponent<Text> ().text = di.getSelect () [1].getOption ();
		buttonRight.onClick.AddListener (delegate() {
			this.clickButton (newSelectButton, di.getSelect () [1], 1); 
		});
		newSelectButton.transform.SetParent (mainListViewGrid.transform);
		newSelectButton.transform.localScale = new Vector3 (1, 1, 1);
		newSelectButton.SetActive (true);
	}

	private void clickButton(GameObject selectButton, Option op, int num){
		if (audioSource.isPlaying){
			audioSource.Stop();
		}
		selectButton.transform.GetChild (0).GetComponent<Button> ().enabled = false;
		selectButton.transform.GetChild (1).GetComponent<Button> ().enabled = false;
		readJson.saveRecord(num);
		isWaiting = false;
		Debug.Log (op.getOption() + "|" + op.getSubfield());
		readJson.toSubfield (op);
		needNext = false;
	}

	// Use this for initialization
	void Start () {
		initExampleGameObject ();
		InvokeRepeating("loadDialogInvoke", 1, 1f); //time, 1s later, 1s repeat
		audioSource = gameObject.AddComponent<AudioSource>();
		readJson = new ReadJson ();
		loadHistory ();
		Debug.Log ("start over");
	}

	private void loadHistory(){
		List<DialogInfo> historyList = readJson.loadHistory ();
		if (historyList.Count > 0) {
			Debug.Log("has history");
			foreach (DialogInfo di in historyList){
				if (di.getType() == DialogType.Dialog){
					addTextItem (di);
				}else{
					addHistoryButton(di);
				}
			}
		} else {
			Debug.Log("no history");
			addItem (readJson.getCurDialogInfo ());
			readJson.saveRecord();
		}
	}

	private void addHistoryButton(DialogInfo di){
		GameObject newSelectButton = Instantiate(selectButtonExample);
		Button buttonLeft = newSelectButton.transform.GetChild (0).GetComponent<Button>();
		buttonLeft.transform.GetChild (0).GetComponent<Text> ().text = di.getSelect () [0].getOption ();
		buttonLeft.enabled = false;
		Button buttonRight = newSelectButton.transform.GetChild (1).GetComponent<Button>();
		buttonRight.transform.GetChild (0).GetComponent<Text> ().text = di.getSelect () [1].getOption ();
		buttonRight.enabled = false;
		newSelectButton.transform.SetParent (mainListViewGrid.transform);
		newSelectButton.transform.localScale = new Vector3 (1, 1, 1);
		newSelectButton.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
	}

	void loadDialogInvoke() {
		delayTime = readJson.getCurDialogInfo ().getDelay ();
		if (delayTime > 0 && !isDelay) {
			wakeTime = Utils.saveWakeTimeStamp(delayTime);
		}
		if (Utils.getTimeStampNow () >= wakeTime) {
			isDelay = false;
		} else {
			isDelay = true;
		}
		isFinish = readJson.isFinish ();
		if (isFinish) {
			CancelInvoke ("loadDialogInvoke");
			Debug.Log ("CancelInvoke");
		} else {
			if (!isWaiting && !isDelay) {
				readJson.next (needNext);
				addItem (readJson.getCurDialogInfo ());
				readJson.saveRecord();
				needNext = true;
				if (readJson.getCurDialogInfo ().getType () == DialogType.Select) {
					isWaiting = true;
				}
			}
		}
//		Rigidbody instance = Instantiate(projectile);
//		instance.velocity = Random.insideUnitSphere * 5;
	}

}
