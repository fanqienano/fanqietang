﻿using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;

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
		this.curJsonFile = op.getSubfield();
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
		if (int.Parse(jdDialog ["type"].ToString()) == 1) {
			di.setType (DialogType.Select);
			JsonData jdo = jdDialog["select"];
			for (int i=0;i<jdo.Count;i++){
				Option op = new Option();
				op.setSubfield(Utils.PathURL + jdo[i]["subfield"].ToString());
				op.setTarId(int.Parse(jdo[i]["tarId"].ToString()));
				op.setOption(jdo[i]["option"].ToString());
				di.addOption(op);
			}
		} else {
			di.setType (DialogType.Dialog);
			di.setContent (jdDialog ["content"].ToString());
			try{
				di.setVoice(jdDialog["voice"].ToString());
			}catch(Exception ex){
				di.setVoice(string.Empty);
			}
		}
		try{
			di.setDelay(long.Parse(jdDialog["delay"].ToString()));
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
	/// <param name="isNeed">If set to <c>true</c> is need.</param>
	public void next(bool isNeed){
		if (isNeed) {
			if (curId < curJson ["data"].Count - 1) {
				this.curId++;
				this.curDialogInfo = getDialogInfo ();
			}
		}
	}

	/// <summary>
	/// is script finish.
	/// </summary>
	/// <returns><c>true</c>, if finish was ised, <c>false</c> otherwise.</returns>
	public Boolean isFinish(){
		if (curId >= curJson ["data"].Count - 1 && curDialogInfo.getType() == DialogType.Dialog) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// save the record
	/// </summary>
	/// <param name="num">Number.</param>
	public void saveRecord(int num){
		if (this.curDialogInfo.getType() == DialogType.Select) {
			Utils.saveRecord (this.curDialogInfo, num, this.curJsonFile, this.curId);
		}
	}

	/// <summary>
	/// save the record
	/// </summary>
	public void saveRecord(){
		if (this.curDialogInfo.getType() == DialogType.Dialog) {
			Utils.saveRecord (this.curDialogInfo, 0, this.curJsonFile, this.curId);
		}
	}

	public List<DialogInfo> loadHistory(){
		List<DialogInfo> dialogInfoList = Utils.readHistory ();
		return dialogInfoList;
	}

}
