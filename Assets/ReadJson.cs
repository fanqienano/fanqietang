using System.Collections;
using LitJson;
using System;

public class ReadJson {
	private JsonData curJson;
	private string curJsonFile = Utils.mainJsonPath;
	private int curId = 0;
	private DialogInfo curDialogInfo = new DialogInfo();

	/// <summary>
	/// Initializes a new instance of the <see cref="ReadJson"/> class;
	/// can get the new DialogInfo
	/// </summary>
	public ReadJson() {
		loadRecord ();
		reloadJsonFile ();
		this.curDialogInfo = getDialogInfo ();
	}

	/// <summary>
	/// load the record;
	/// get the current json file and current id
	/// </summary>
	public void loadRecord(){
		string[] record = Utils.readRecord ();
		this.curJsonFile = record [0];
		this.curId = Convert.ToInt16(record [1]);
	}

	/// <summary>
	/// select to subfield;
	/// get the new DialogInfo
	/// </summary>
	/// <param name="op">Op.</param>
	public void toSubfield(Option op){
		this.curJsonFile = Utils.PathURL + op.getSubfield();
		this.curId = op.getTarId();
		reloadJsonFile ();
		this.curDialogInfo = getDialogInfo ();
	}

	private void reloadJsonFile(){
		this.curJson = JsonMapper.ToObject(Utils.readJsonFile(this.curJsonFile));
	}

	private DialogInfo getDialogInfo(){
		JsonData jdDialog = this.curJson ["data"][this.curId];
		DialogInfo di = new DialogInfo ();
		di.setId (this.curId);
		if ((int)jdDialog ["type"] == 1) {
			di.setType (DialogType.Select);
			JsonData jdo = jdDialog["select"];
			for (int i=0;i<jdo.Count;i++){
				Option op = new Option();
				op.setSubfield(jdo[i]["subfield"].ToString());
				op.setTarId((int) jdo[i]["tarId"]);
				op.setOption(jdo[i]["option"].ToString());
				try{
					op.setDelay((long)jdo[i]["delay"]);
				}catch(Exception ex){
					op.setDelay(0);
				}
				di.addOption(op);
			}
		} else {
			di.setType (DialogType.Dialog);
			di.setContent (jdDialog ["content"].ToString());
		}
		try{
			di.setDelay((long)jdDialog["delay"]);
		}catch(Exception ex){
			di.setDelay(0);
		}
		return di;
	}

	/// <summary>
	/// get current DialogInfo
	/// </summary>
	/// <returns>The current dialog info.</returns>
	public DialogInfo getCurDialogInfo(){
		return this.curDialogInfo;
	}

	/// <summary>
	/// to next dialog;
	/// get new DialogInfo
	/// </summary>
	public Boolean next(){
		if (curId < curJson ["data"].Count - 1) {
			this.curId++;
			this.curDialogInfo = getDialogInfo();
			return true;
		}
		return false;
	}

	/// <summary>
	/// save the record
	/// </summary>
	public void saveRecord(){
		Utils.saveRecord (this.curJsonFile, this.curId);
	}

}
