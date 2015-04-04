package com.swgproject.projectswg.library;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;

import android.content.Context;
import android.content.SharedPreferences.Editor;
import android.util.Log;

import com.swgproject.projectswg.library.SessionManager;

public class DatabaseManager {
	
	//set variables
	private final String LOGIN_TAG = "Login";
	SessionManager sessionmanager;
	Editor editor;
	Context context;
	StringBuilder response;
	HttpPost post;
	List params;
	DefaultHttpClient httpClient;
	URI absolute = null;
      
	
	public DatabaseManager(){
		//initialize variables
		response = new StringBuilder();
		post = new HttpPost();
		params = new ArrayList();
		httpClient = new DefaultHttpClient();
		try {
	      	absolute = new URI("http://projectswg.com/webservice/");
		} catch (URISyntaxException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public String loginParams(String username, String password){
		params.add(new BasicNameValuePair("tag", "login"));
		params.add(new BasicNameValuePair("username", username));
		params.add(new BasicNameValuePair("password", password));
		return postHttpResponse(params);
	}
	
	public String antiHackParams(String username, int id){
		params.add(new BasicNameValuePair("tag", "antiHack"));
		params.add(new BasicNameValuePair("username", username));
		params.add(new BasicNameValuePair("uid", Integer.toString(id)));
		Log.d(LOGIN_TAG, username);
		Log.d(LOGIN_TAG, Integer.toString(id));
		return postHttpResponse(params);
	}

	public String postHttpResponse(List<? extends NameValuePair> params) {
		//Log.d(LOGIN_TAG, "Going to make a post request");
		try {
			post.setURI(absolute); 
			/*params.add(new BasicNameValuePair("tag", "login"));
			params.add(new BasicNameValuePair("username", username));
			params.add(new BasicNameValuePair("password", password));*/
			post.setEntity(new UrlEncodedFormEntity(params));
			HttpResponse httpResponse = httpClient.execute(post);
			if (httpResponse.getStatusLine().getStatusCode() == 200) {
				//Log.d(LOGIN_TAG, "HTTP POST succeeded");
				HttpEntity messageEntity = httpResponse.getEntity();
				InputStream is = messageEntity.getContent();
				BufferedReader br = new BufferedReader(new InputStreamReader(is));
				String line;
				while ((line = br.readLine()) != null) {
					response.append(line);
				}
			} else {
				//Log.e(LOGIN_TAG, "HTTP POST status code is not 200");
			}
		} catch (Exception e) {
			//Log.e(LOGIN_TAG, e.getMessage());
		}
		//Log.d(LOGIN_TAG, "Done with HTTP posting");
		Log.d(LOGIN_TAG, response.toString());
		return response.toString();
	}

}
