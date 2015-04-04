package com.swgproject.projectswg;

import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;

import com.swgproject.projectswg.ActionBar;

public class Main extends ActionBar {

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.main);
            background = findViewById(R.id.status);
            status = findViewById(R.id.image); 
            loading = findViewById(R.id.loading);
            new CheckStatusTask().execute();
            background.setOnClickListener(new OnClickListener() {
       			public void onClick(View v) {
       				// TODO Auto-generated method stub
       				new CheckStatusTask().execute();
       			}
       		});
        }


}
