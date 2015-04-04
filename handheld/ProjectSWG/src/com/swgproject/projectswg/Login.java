package com.swgproject.projectswg;

import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.swgproject.projectswg.ActionBar;
import com.swgproject.projectswg.ActionBar.CheckStatusTask;
import com.swgproject.projectswg.library.SessionManager;

public class Login extends ActionBar{

	public static EditText txtUserName;
	public static EditText txtPassword;
    Button btnLogin;
    Button btnCancel;
    SessionManager session;
     
   		@Override
   		public void onCreate(Bundle savedInstanceState) {
   			super.onCreate(savedInstanceState);
   			setContentView(R.layout.login);
   			session = new SessionManager(getApplicationContext());
   			txtUserName=(EditText)this.findViewById(R.id.loginUsername);
       		txtPassword=(EditText)this.findViewById(R.id.loginPassword);
       		errorMessage = (TextView)this.findViewById(R.id.error);
       		background = findViewById(R.id.status);
            status = findViewById(R.id.image); 
       		loading = findViewById(R.id.loading);
       		new CheckStatusTask().execute();
       		btnLogin=(Button)this.findViewById(R.id.btnLogin);
       		btnLogin.setOnClickListener(new OnClickListener() {
       			public void onClick(View v) {
       				// TODO Auto-generated method stub
       				String username = txtUserName.getText().toString();
       				String password = txtPassword.getText().toString();
       				new loginTask().execute(username,password);
       			}
       		}); 
       		background.setOnClickListener(new OnClickListener() {
       			public void onClick(View v) {
       				// TODO Auto-generated method stub
       				new CheckStatusTask().execute();
       			}
       		});
   		}
   		
   		
}
