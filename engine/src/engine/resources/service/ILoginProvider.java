package engine.resources.service;

public interface ILoginProvider {

	/*
	 * Returns the account ID.
	 * -1 is "DB error"
	 * -2 is "invalid user or password"
	 * -3 is "banned"
	 */
	public int getAccountId(String username, String password, String remoteAddress);
	
}
