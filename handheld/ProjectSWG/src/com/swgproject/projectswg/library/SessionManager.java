package com.swgproject.projectswg.library;

import java.util.HashMap;

import com.swgproject.projectswg.Login;
import com.swgproject.projectswg.library.DatabaseManager;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.AsyncTask;
import android.util.Log;

public class SessionManager {
	//set variables
	DatabaseManager database;
    SharedPreferences pref;
    Editor editor;
    Context _context;
    int PRIVATE_MODE = 0;
    private static final String PREF_NAME = "Session";
    private static final String IS_LOGIN = "isLoggedIn";
    public static final String KEY_NAME = "username";
    public static final String KEY_ID = "userID";
 
    // Constructor
    public SessionManager(Context context){
        this._context = context;
        pref = _context.getSharedPreferences(PREF_NAME, PRIVATE_MODE);
        editor = pref.edit();
    }
 
    /**
     * Create login session
     * */
    public void createLoginSession(String name, int id){
        // Storing login value as TRUE
        editor.putBoolean(IS_LOGIN, true);
 
        // Storing username in pref
        editor.putString(KEY_NAME, name);
 
        // Storing user id in pref
        editor.putInt(KEY_ID, id);
 
        // commit changes
        editor.commit();
    } 
    
  //Login
  		public class antiHack extends AsyncTask<Void, Void, Boolean> { 

  			//check if username and user id match
  		    protected Boolean doInBackground(Void... params) {
  		        database = new DatabaseManager();
  		    	
  		    	String username = pref.getString(KEY_NAME, null);
  		        int userid = pref.getInt(KEY_ID, -1);
  		        String check = database.antiHackParams(username, userid);
  		        int test = Integer.parseInt(check);
  		        Log.d("login", check);
  		        if (test == 1){
  		        	Log.d("login", "true");
  		        	return true;
  		        }else{
  		        	Log.d("login", "false");
  		        	return false;
  		        }
  		     }
  		}
 
    /**
     * Check login method will check user login status
     * If false it will redirect user to login page
     * Else won't do anything
     * */
    public void checkLogin(){
        // Check if the user changed any session data
    	boolean test = true;
    	try{
    		test = new antiHack().execute().get();
    	}catch(Exception e){
    		test = false;
    	}
    	Log.d("login", Boolean.toString(test));
    	//if the user isnt logged in or the session data does not match log the user out
        if(!this.isLoggedIn() || !test){
            logoutUser();
        }
 
    }
 
    /**
     * Get stored session data
     * */
    public HashMap<String, String> getUserDetails(){
        HashMap<String, String> user = new HashMap<String, String>();
        // user name
        user.put(KEY_NAME, pref.getString(KEY_NAME, null));
 
        // user email id
        user.put(KEY_ID, pref.getString(KEY_ID, null));
 
        // return user
        return user;
    }
 
    /**
     * Clear session details
     * */
    public void logoutUser(){
        // Clearing all data from Shared Preferences
        editor.clear();
        editor.commit();
 
        // After logout redirect user to Loing Activity
        Intent i = new Intent(_context, Login.class);
        // Closing all the Activities
        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
 
        // Add new Flag to start new Activity
        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
 
        // Staring Login Activity
        _context.startActivity(i);
    }
 
    /**
     * Quick check for login
     * **/
    // Get Login State
    public boolean isLoggedIn(){
        return pref.getBoolean(IS_LOGIN, false);
    }
}
