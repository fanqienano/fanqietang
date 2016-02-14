using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;

public class MainListView : MonoBehaviour {

	private GameObject mainListView;
	private GameObject mainListViewGrid;
	private Text textExample;
	private GameObject selectButtonExample;
	private ReadJson readJson;

	private void initExampleGameObject(){
		mainListView = GameObject.Find("MainActivity/MainListView");
		mainListViewGrid = GameObject.Find("MainActivity/MainListView/Grid");
		textExample = GameObject.Find ("MainActivity/Text").GetComponent<Text>();
		selectButtonExample = GameObject.Find ("MainActivity/SelectButton");
		selectButtonExample.SetActive (false);
	}

	private void addItem(DialogInfo di){
		if (di.getType() == DialogType.Dialog) {
			addTextItem (di);
		} else {
			addSelectButton(di);
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
			this.clickButton (newSelectButton, di.getSelect () [0]); 
		});
		Button buttonRight = newSelectButton.transform.GetChild (1).GetComponent<Button>();
		buttonRight.transform.GetChild (0).GetComponent<Text> ().text = di.getSelect () [1].getOption ();
		buttonRight.onClick.AddListener (delegate() {
			this.clickButton (newSelectButton, di.getSelect () [1]); 
		});
		newSelectButton.transform.SetParent (mainListViewGrid.transform);
		newSelectButton.transform.localScale = new Vector3 (1, 1, 1);
		newSelectButton.SetActive (true);
	}

	private void clickButton(GameObject selectButton, Option op){
		selectButton.transform.GetChild (0).GetComponent<Button> ().enabled = false;
		selectButton.transform.GetChild (1).GetComponent<Button> ().enabled = false;
		Debug.Log (op.getOption() + "|" + op.getSubfield());
	}

	// Use this for initialization
	void Start () {
		initExampleGameObject ();
		readJson = new ReadJson ();
		Debug.Log (readJson.getCurDialogInfo ().ToString());
		addItem (readJson.getCurDialogInfo ());
		readJson.next ();
		Debug.Log (readJson.getCurDialogInfo ().ToString());
		addItem (readJson.getCurDialogInfo ());
		readJson.next ();
		Debug.Log (readJson.getCurDialogInfo ().ToString());
		addItem (readJson.getCurDialogInfo ());
		readJson.next ();
		Debug.Log (readJson.getCurDialogInfo ().ToString());
		addItem (readJson.getCurDialogInfo ());
		Debug.Log ("start over");
	}

	// Update is called once per frame
	void Update () {
	}

}
