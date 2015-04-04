package engine.resources.common;

public class Profession {
	public static final String DOMESTIC_TXT					= "trader_0a";
	public static final String STRUCTURES_TXT					= "trader_0b";
	public static final String MUNITIONS_TXT					= "trader_0c";
	public static final String ENGINEER_TXT					= "trader_0d";
	public static final String SPY_TXT					= "spy_1a";
	public static final String SMUGGLER_TXT					= "smuggler_1a";
	public static final String OFFICER_TXT					= "officer_1a";
	public static final String JEDI_TXT					= "force_sensitive_1a";
	public static final String COMMANDO_TXT					= "commando_1a";
	public static final String ENTERTAINER_TXT					= "entertainer_1a";
	public static final String MEDIC_TXT					= "medic_1a";
	public static final String BOUNTYHUNTER_TXT					= "bounty_hunter_1a";
	
	public static final Short DOMESTIC_HEX					= 0x00;
	public static final Short STRUCTURES_HEX					= 0x00;
	public static final Short MUNITIONS_HEX					= 0x00;
	public static final Short ENGINEER_HEX					= 0x00;
	public static final Short SPY_HEX					= 0x23;
	public static final Short SMUGGLER_HEX					= 0x19;
	public static final Short OFFICER_HEX					= 0x0F;
	public static final Short JEDI_HEX					= 0x28;
	public static final Short COMMANDO_HEX					= 0x1E;
	public static final Short ENTERTAINER_HEX					= 0x05;
	public static final Short MEDIC_HEX					= 0x0A;
	public static final Short BOUNTYHUNTER_HEX					= 0x14;
	
	private String _label;
	private Short _hex = 0x00;
	
	public Profession(String prof){
		setProfessionByLabel(prof);
	}
	
	public void setProfessionByLabel(String prof){
		this._label = prof;
		if(prof == DOMESTIC_TXT) this._hex = DOMESTIC_HEX;
		if(prof == STRUCTURES_TXT) this._hex = STRUCTURES_HEX;
		if(prof == MUNITIONS_TXT) this._hex = MUNITIONS_HEX;
		if(prof == ENGINEER_TXT) this._hex = ENGINEER_HEX;
		if(prof == SPY_TXT) this._hex = SPY_HEX;
		if(prof == SMUGGLER_TXT) this._hex = SMUGGLER_HEX;
		if(prof == OFFICER_TXT) this._hex = OFFICER_HEX;
		if(prof == JEDI_TXT) this._hex = JEDI_HEX;
		if(prof == COMMANDO_TXT) this._hex = COMMANDO_HEX;
		if(prof == ENTERTAINER_TXT) this._hex = ENTERTAINER_HEX;
		if(prof == MEDIC_TXT) this._hex = MEDIC_HEX;
		if(prof == BOUNTYHUNTER_TXT) this._hex = BOUNTYHUNTER_HEX;
	}

	public String getLabel(){
		return this._label;
	}
	public Short getHex(){
		return this._hex;
	}
}
