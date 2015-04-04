package com.swgproject.projectswg;

import com.swgproject.projectswg.ActionBar;
import com.swgproject.projectswg.ActionBar.CheckStatusTask;
import com.swgproject.projectswg.library.SessionManager;

import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;

public class Chat extends ActionBar {

	SessionManager sessionmanager;
        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.chat);
            background = findViewById(R.id.status);
            status = findViewById(R.id.image);
            loading = findViewById(R.id.loading);
            sessionmanager = new SessionManager(getApplicationContext());
            sessionmanager.checkLogin();
            if(sessionmanager.isLoggedIn()){
            	TextView text = (TextView) findViewById(R.id.text);
            	text.setText("Talk");
            }
            new CheckStatusTask().execute();
            background.setOnClickListener(new OnClickListener() {
       			public void onClick(View v) {
       				// TODO Auto-generated method stub
       				new CheckStatusTask().execute();
       			}
       		});
        }

}
