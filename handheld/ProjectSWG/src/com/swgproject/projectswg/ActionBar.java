package com.swgproject.projectswg;

import org.json.JSONException;
import org.json.JSONObject;

import com.swgproject.projectswg.library.CheckStatus;
import com.swgproject.projectswg.library.SessionManager;
import com.swgproject.projectswg.library.DatabaseManager;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.AsyncTask;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import android.app.Activity;

public class ActionBar extends Activity {
	
	//set variables
	public static final String KEY_NAME = "username";
    public static final String KEY_ID = "userID";
	public View background;
	public View status;
	public View loading;
	public TextView errorMessage;
	SharedPreferences prefs;
	Editor editor;
	Context context;
	SessionManager sessionmanager;
	DatabaseManager database;

	
	//Login
	public class loginTask extends AsyncTask<String, Void, String> { 

		//check if username and password with the database
	    protected String doInBackground(String... params) {
	        String username = params[0];
	        String password = params[1];
	        database = new DatabaseManager();
	        return database.loginParams(username,password);
	     }

	     //send server results to checkResponse
	     protected void onPostExecute(String jsonstring) {
	     	try {
	     		//get values from jsonobject
	     		JSONObject jsonobj=new JSONObject(jsonstring);
	     		checkResponse(jsonobj);
	     	} catch (JSONException e) {
	     		// TODO Auto-generated catch block
	     		e.printStackTrace();
	     	}

	     }
	}
	
	//Check servers response for pass or fail
	public void checkResponse(JSONObject response){
		try {
			//parse JSON and set variables
			int success = response.getInt("success");
			int error = response.getInt("error");
			
	        if(success == 1){
	        	JSONObject user = response.getJSONObject("user");
	        	String name = user.getString("name");
	        	int id = user.getInt("uid");
	        	sessionmanager = new SessionManager(getApplicationContext());
	        	sessionmanager.createLoginSession(name, id);
	        	Intent home = new Intent(getApplicationContext(), Main.class);
	            startActivity(home);
	        }else if(error == 1){
	        	errorMessage.setText(response.getString("error_msg"));
	        }
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	//Status code
	
	class CheckStatusTask extends AsyncTask<Void, Void, Boolean> { 

		//show loading
	    protected void onPreExecute(){
	    	status.setVisibility(View.INVISIBLE);
	    	loading.setVisibility(View.VISIBLE);
	    }
		
    	//check if server is online
        protected Boolean doInBackground(Void... params) {
            return CheckStatus.check();
        }

        //set status bar to offline if flag is false
        protected void onPostExecute(Boolean flag) {
        	status.setVisibility(View.VISIBLE);
	    	loading.setVisibility(View.INVISIBLE);
            if(!flag){
                background.setBackgroundResource(R.drawable.offline);
                status.setBackgroundResource(R.drawable.offline_image);
            }
        }

    }
	
	//Action Bar
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		sessionmanager = new SessionManager(getApplicationContext());
    	if(sessionmanager.isLoggedIn()){
    		getMenuInflater().inflate(R.menu.activity_main, menu);
    	}else{
    		getMenuInflater().inflate(R.menu.activity_offline, menu);
    	}
		return true;
	}
	
	//set navigation actions
	@Override
	  public boolean onOptionsItemSelected(MenuItem item) {
	    switch (item.getItemId()) {
	    //stats calculator
	    case R.id.menu_stats:
	    	Intent stats = new Intent(getApplicationContext(), StatCalculator.class);
            startActivity(stats);
	      break;
	    //chat
	    case R.id.menu_chat:
	    	Intent chat = new Intent(getApplicationContext(), Chat.class);
            startActivity(chat);
		      break;
		//Log in
	    case R.id.menu_login:
	    	Intent login = new Intent(getApplicationContext(), Login.class);
	    	startActivity(login);
		      break;
		//log out
	    case R.id.menu_logout:
	    	sessionmanager = new SessionManager(getApplicationContext());
	    	sessionmanager.logoutUser();
		      break;
		   
		//set default to home page
	    default:
	    	Intent home = new Intent(getApplicationContext(), Main.class);
            startActivity(home);
              break;
	    }

	    return true;
	  } 

}
