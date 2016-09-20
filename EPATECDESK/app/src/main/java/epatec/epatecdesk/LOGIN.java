package epatec.epatecdesk;


import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.subhrajyoti.passwordview.PasswordView;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;


/**
 * A login screen that offers login via email/password.
 */
public class LOGIN extends AppCompatActivity {
    private PasswordView UserPassword;
    private EditText UserID;
    private TextView Register;
    private Button signin;

    @Override
    protected void onCreate(final Bundle savedInstanceState) {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        Intent log_in = getIntent();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        UserPassword = (PasswordView) findViewById(R.id.password);
        UserID = (EditText) findViewById(R.id.email);
        // Set up the login form.
        Register = (TextView) findViewById(R.id.textView2);
        signin = (Button) findViewById(R.id.email_sign_in_button);
        final String result ="true";

        signin.setOnClickListener(new View.OnClickListener(){
            public void onClick(View v) {
                String action="";


                try {
                    action = checkUserExists(UserID.getText().toString(),UserPassword.getText().toString());

                } catch (IOException e) {
                    e.printStackTrace();
                }
                System.out.println(action+" Coso");
                if(action.length() == 4){
                    Intent ClientProfile = new Intent(LOGIN.this, ClientProfile.class);
                    LOGIN.this.startActivity(ClientProfile);
                }

            }
        });
        Register.setOnClickListener(new View.OnClickListener(){

            @Override
            public void onClick(View v) {
                Intent register = new Intent(LOGIN.this,RegisterWindow.class);
                LOGIN.this.startActivity(register);
            }
        });

    }

    private String checkUserExists(String uID, String uPassword) throws IOException {
        //System.out.println(uID+" "+uPassword);
        String server = "http://isaac:7549/api/check/get/"+uID+"/"+uPassword+"/Client";
        System.out.println(server);
        HttpResponse stringresponse;
        HttpClient Client = new DefaultHttpClient();
        HttpGet response = new HttpGet(server);
        stringresponse = Client.execute(response);

        BufferedReader rd = new BufferedReader(new InputStreamReader(stringresponse.getEntity().getContent()));
        StringBuilder builder = new StringBuilder();
        String str = "";

        while ((str = rd.readLine()) != null) {
            builder.append(str);
        }

        String text = builder.toString();
        String returnvalue = text.substring(1,text.length()-1);
        return returnvalue;


    }
}

