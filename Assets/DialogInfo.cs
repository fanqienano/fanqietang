using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;

public class DialogInfo
{
	private DialogType type = DialogType.Dialog;
	private int id = 0;
	private string content = string.Empty;
	private List<Option> select = new List<Option> ();
	private long delay = 0;

	public override string ToString(){
		StringBuilder sb = new StringBuilder ();
		JsonWriter writer = new JsonWriter (sb);
		writer.WriteObjectStart ();
		writer.WritePropertyName ("id");
		writer.Write (id);
		if (type == DialogType.Dialog) {
			writer.WritePropertyName ("type");
			writer.Write ("Dialog");
			writer.WritePropertyName ("content");
			writer.Write (content);
			writer.WritePropertyName ("delay");
			writer.Write (delay);
		} else {
			writer.WritePropertyName ("type");
			writer.Write ("Select");
			writer.WritePropertyName ("select1");
			writer.Write (select[0].ToString());
			writer.WritePropertyName ("select2");
			writer.Write (select[1].ToString());
			writer.WritePropertyName ("delay");
			writer.Write (delay);
		}
		writer.WriteObjectEnd ();
		return sb.ToString ();
	}

	public void setType (DialogType type)
	{
		this.type = type;
	}
	
	public DialogType getType ()
	{
		return type;
	}
	
	public void setId (int id)
	{
		this.id = id;
	}
	
	public int getId ()
	{
		return id;
	}
	
	public void setContent (string content)
	{
		this.content = content;
	}
	
	public string getContent ()
	{
		return content;
	}
	
	public void setSelect (List<Option> select)
	{
		this.select = select;
	}
	
	public List<Option> getSelect ()
	{
		return select;
	}

	public void addOption(Option option){
		this.select.Add (option);
	}
	
	public void setDelay (long delay)
	{
		this.delay = delay;
	}
	
	public long getDelay ()
	{
		return delay;
	}

}
