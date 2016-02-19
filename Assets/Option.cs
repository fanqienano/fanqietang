using UnityEngine;
using System.Text;
using LitJson;

public class Option
{
	private string option = string.Empty;
	private string subfield = string.Empty;
	private int tarId = 0;
	
	public override string ToString(){
		StringBuilder sb = new StringBuilder ();
		JsonWriter writer = new JsonWriter (sb);
		writer.WriteObjectStart ();
		writer.WritePropertyName ("option");
		writer.Write (option);
		writer.WritePropertyName ("subfield");
		writer.Write (subfield);
		writer.WritePropertyName ("tarId");
		writer.Write (tarId);
		writer.WriteObjectEnd ();
		return sb.ToString();
	}

	public void setOption (string option)
	{
		this.option = option;
	}
	
	public string getOption ()
	{
		return option;
	}
	
	public void setSubfield (string subfield)
	{
		this.subfield = subfield;
	}
	
	public string getSubfield ()
	{
		return subfield;
	}
	
	public void setTarId (int tarId)
	{
		this.tarId = tarId;
	}
	
	public int getTarId ()
	{
		return tarId;
	}

}
