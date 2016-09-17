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
                if(id.length()!= 10){
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
                }else if(!name.contains("@")){
                    Email.setError("The email format does not match.");
                    focus = Email;
                    focus.requestFocus();
                    status = false;
                    return true;
                }else if (name.isEmpty()){
                    Email.setError("The name box is empty");
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
                if(status) {
                    Intent buy = new Intent(RegisterWindow.this, ClientProfile.class);
                    startActivity(buy);
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
}
