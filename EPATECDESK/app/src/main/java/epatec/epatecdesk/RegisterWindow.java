package epatec.epatecdesk;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.support.annotation.NonNull;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.app.LoaderManager.LoaderCallbacks;
import android.content.CursorLoader;
import android.content.Loader;
import android.database.Cursor;
import android.net.Uri;
import android.os.AsyncTask;

import android.os.Build;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.inputmethod.EditorInfo;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.subhrajyoti.passwordview.PasswordView;

import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.sql.Date;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;

import java.util.List;

import static android.Manifest.permission.READ_CONTACTS;

/**
 * Created by Isaac on 9/3/2016.
 */


public class RegisterWindow extends AppCompatActivity{
    final static String Format = "dd/MM/YYYY";
    private TextView Log_in;
    private PasswordView Password;
    private PasswordView Conf_password;
    private TextView Name;
    private EditText BDate;
    private TextView Email;
    private TextView ID;
    private TextView Address;
    private TextView Tel;
    private Button Register;
    private boolean status = false;
    View focus;
    @Override
    protected void onCreate(Bundle savedInstanceState){
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);
        Intent registered = getIntent();
        //Catches the ID of the Button in the buy activity
        Register = (Button) findViewById(R.id.Register);
        Email = (TextView) findViewById(R.id.Email);
        Password = (PasswordView) findViewById(R.id.Pass);
        Password.useStrikeThrough(true);
        Name = (TextView) findViewById(R.id.Name);
        Conf_password = (PasswordView) findViewById(R.id.Conf_pass);
        Conf_password.useStrikeThrough(true);
        BDate = (EditText) findViewById(R.id.Bdate);
        TextWatcher frmt = new TextWatcher() {
            private String current = "";
            private String format = "DDMMYYYY";
            private Calendar cal = Calendar.getInstance();
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                if (!s.toString().equals(current)) {
                    String clean = s.toString().replaceAll("[^\\d.]", "");
                    String cleanC = current.replaceAll("[^\\d.]", "");

                    int cl = clean.length();
                    int sel = cl;
                    for (int i = 2; i <= cl && i < 6; i += 2) {
                        sel++;
                    }
                    //Fix for pressing delete next to a forward slash
                    if (clean.equals(cleanC)) sel--;

                    if (clean.length() < 8){
                        clean = clean + format.substring(clean.length());
                    }else{
                        //This part makes sure that when we finish entering numbers
                        //the date is correct, fixing it otherwise
                        int day  = Integer.parseInt(clean.substring(0,2));
                        int mon  = Integer.parseInt(clean.substring(2,4));
                        int year = Integer.parseInt(clean.substring(4,8));

                        if(mon > 12) mon = 12;
                        cal.set(Calendar.MONTH, mon-1);
                        year = (year<1900)?1900:(year>2100)?2100:year;
                        cal.set(Calendar.YEAR, year);
                        // ^ first set year for the line below to work correctly
                        //with leap years - otherwise, date e.g. 29/02/2012
                        //would be automatically corrected to 28/02/2012

                        day = (day > cal.getActualMaximum(Calendar.DATE))? cal.getActualMaximum(Calendar.DATE):day;
                        clean = String.format("%02d%02d%02d",day, mon, year);
                    }

                    clean = String.format("%s/%s/%s", clean.substring(0, 2),
                            clean.substring(2, 4),
                            clean.substring(4, 8));

                    sel = sel < 0 ? 0 : sel;
                    current = clean;
                    BDate.setText(current);
                    BDate.setSelection(sel < current.length() ? sel : current.length());
                }
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        };
        BDate.addTextChangedListener(frmt);
        ID = (TextView) findViewById(R.id.Ced);
        Address = (TextView) findViewById(R.id.Addr);
        Tel = (TextView) findViewById(R.id.Phone);
        Log_in = (TextView) findViewById(R.id.Log_in);
        //Catches the action performed in the word log in to work as an hyperlink
        Tel.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                String date = BDate.getText().toString();
                String[] date2 = date.split("/");
                if(date2.length <2){
                    BDate.setError("The date is incorrect");
                    focus = BDate;
                    focus.requestFocus();
                    status=false;
                }else if(!isDateValid(date)){
                    BDate.setError("The date specified doesn't match");
                    focus = BDate;
                    focus.requestFocus();
                    status = false;
                }
                status = true;
                return false;
            }
        });
        Address.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                String id = ID.getText().toString();
                if(id.length()!= 9){
                    ID.setError("The ID is not correct");
                    focus= ID;
                    status=false;
                    focus.requestFocus();
                }
                status = true;
                return false;
            }
        });
        ID.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                ID.setError("The Format is 999999999");
                String pass = Password.getText().toString();
                String confp = Conf_password.getText().toString();
                if (!pass.matches(confp)){
                    Conf_password.setError("The password does not match");
                    focus = Conf_password;
                    focus.requestFocus();
                    status = false;
                    return true;
                }
                status = true;
                return false;
            }
        });
        Log_in.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent log = new Intent(RegisterWindow.this, LOGIN.class);
                startActivity(log);
            }
        });

        //Checks the data is well written
        Email.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                String name = Name.getText().toString();
                if(name.contains("@")||name.contains("/")||name.contains("\"")||name.contains("#")||name.contains("$")){
                    Name.setError("Your name has some incorrect charactes");
                    focus = Name;
                    focus.requestFocus();
                    status = false;
                    return true;
                }else if (name.isEmpty()){
                    Name.setError("The name box is empty");
                    focus = Name;
                    focus.requestFocus();
                    status = false;
                    return true;
                }
                status=true;
                return false;
            }
        });
        Password.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                String name = Email.getText().toString();
                if(name.contains("/")||name.contains("\"")||name.contains("#")||name.contains("$")){
                    Email.setError("Your name has some incorrect charactes");
                    focus = Email;
                    focus.requestFocus();
                    status = false;
                    return true;
                }else if(name.isEmpty()){
                    Email.setError(" You must provide a last name");
                    focus = Email;
                    focus.requestFocus();
                    status = false;
                    return true;
                }
                status = true;
                return false;
            }
        });

        //Redirects the user to the Buy Window
        Register.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                String result="";
                try {
                    result = checkUserExists(ID.getText().toString(),Password.getText().toString());
                    System.out.println(result);
                } catch (IOException e) {
                    e.printStackTrace();
                }
                if(status) {
                    String server = "http://isaac:7549/api/client/post";
                    if(result.length()==5 ){
                        HttpClient newpost = new DefaultHttpClient();
                        HttpPost sendinfo = new HttpPost(server);
                        String json="";
                        JSONObject post = new JSONObject();
                        try {
                            post.accumulate("C_ID",ID.getText().toString());
                            post.accumulate("FName",Name.getText().toString());
                            post.accumulate("LName",Email.getText().toString());
                            post.accumulate("CAddress",Address.getText().toString());
                            post.accumulate("Phone",Tel.getText().toString());
                            String[] bday = BDate.getText().toString().split("/");
                            post.accumulate("Day",bday[0]);
                            post.accumulate("Month",bday[1]);
                            post.accumulate("Year",bday[2]);
                            post.accumulate("Penalization",0);
                            post.accumulate("CPassword",Password.getText().toString());

                            json = post.toString();

                            StringEntity ent = new StringEntity(json);
                            sendinfo.setEntity(ent);
                            sendinfo.setHeader("Accept", "application/json");
                            sendinfo.setHeader("Content-type", "application/json");
                            newpost.execute(sendinfo);
                        } catch (UnsupportedEncodingException e) {
                            e.printStackTrace();
                        } catch (ClientProtocolException e) {
                            e.printStackTrace();
                        } catch (IOException e) {
                            e.printStackTrace();
                        }catch (JSONException e) {
                            e.printStackTrace();
                        }


                        Intent buy = new Intent(RegisterWindow.this, ClientProfile.class);
                        startActivity(buy);
                    }else{
                        Context context = getApplicationContext();
                        CharSequence text = "An user with the same ID already exists, please try again";
                        int duration = Toast.LENGTH_SHORT;

                        Toast toast = Toast.makeText(context, text, duration);
                        toast.show();
                    }

                }else{
                    Context context = getApplicationContext();
                    CharSequence text = "You must fill all the fields";
                    int duration = Toast.LENGTH_SHORT;

                    Toast toast = Toast.makeText(context, text, duration);
                    toast.show();
                }
            }
        });

    }
    private static boolean isDateValid(String date){
        try{
            DateFormat df = new SimpleDateFormat(Format);
            df.setLenient(false);
            df.parse(date);
            return true;
        } catch (ParseException e) {
            return false;
        }
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
