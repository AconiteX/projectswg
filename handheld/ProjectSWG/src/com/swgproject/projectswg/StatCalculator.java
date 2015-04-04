package com.swgproject.projectswg;

import com.swgproject.projectswg.ActionBar;
import com.swgproject.projectswg.ActionBar.CheckStatusTask;

import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;

public class StatCalculator extends ActionBar {

        @Override
        protected void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            setContentView(R.layout.stats);
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
