package epatec.epatecdesk;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import com.loopj.android.http.AsyncHttpClient;
import com.loopj.android.http.JsonHttpResponseHandler;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Random;
import android.view.View.OnClickListener;
import cz.msebera.android.httpclient.Header;
import cz.msebera.android.httpclient.entity.StringEntity;
import cz.msebera.android.httpclient.message.BasicHeader;
import cz.msebera.android.httpclient.protocol.HTTP;

/**
 * Created by Isaac on 9/4/2016.
 */
public class ClientProfile extends Activity implements AdapterView.OnItemSelectedListener {
    List<Integer> Sucursals = new ArrayList<>();
    List<String> ListProducts = new ArrayList<>();
    Spinner SpSucursals;
    String clientID = "";
    ListView Products;
    EditText product;
    EditText quantity;
    Button addproduct;
    Integer Subsidiary = 0;
    List<String> products = new ArrayList<>();
    List<String> quantities = new ArrayList<>();
    Button SendOrder;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_buy);
        Intent create = getIntent();
        clientID = create.getStringExtra("C_ID");
        System.out.println(clientID);
        Products = (ListView)findViewById(R.id.listView);
        SpSucursals = (Spinner) findViewById(R.id.spinner);
        SpSucursals.setPrompt("Subsidiary");
        AsyncHttpClient httpClient = new AsyncHttpClient();
        product = (EditText) findViewById(R.id.product);
        quantity = (EditText) findViewById(R.id.quantity);
        addproduct = (Button) findViewById(R.id.addproduct);
        SendOrder = (Button) findViewById(R.id.SubmitOrder);
        final String spproduct = "You need to specify a product";
        final String spquantity = "You need to specify the quantity";
        final String success = "The product has been added successfuly";
        httpClient.get("http://isaac:7549/api/sucursal/get/S_ID/undefined", null, new JsonHttpResponseHandler() {
            @Override
            public void onSuccess(int statusCode, Header[] headers, JSONArray timeline) {
                // Pull out the first event on the public timeline
                JSONObject firstEvent = null;
                try {
                    for (int i = 0; i < timeline.length(); i++) {
                        firstEvent = (JSONObject) timeline.get(i);
                        Sucursals.add(firstEvent.getInt("S_ID"));
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }


                // Do something with the response

            }
        });

        httpClient.get("http://isaac:7549/api/product/get/PR_ID/undefined", null, new JsonHttpResponseHandler() {
            @Override
            public void onSuccess(int statusCode, Header[] headers, JSONArray timeline) {
                // Pull out the first event on the public timeline
                JSONObject firstEvent = null;
                try {
                    for (int i = 0; i < timeline.length(); i++) {
                        firstEvent = (JSONObject) timeline.get(i);
                        ListProducts.add(firstEvent.getString("PName"));
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }


                // Do something with the response

            }
        });
        httpClient.cancelAllRequests(true);
        final ArrayAdapter<String> productarray = new ArrayAdapter<String>(this,R.layout.support_simple_spinner_dropdown_item,ListProducts);
        productarray.setDropDownViewResource(R.layout.support_simple_spinner_dropdown_item);
        Products.setAdapter(productarray);


        ArrayAdapter<Integer> list = new ArrayAdapter<>(this, R.layout.support_simple_spinner_dropdown_item, Sucursals);

        list.setDropDownViewResource(R.layout.support_simple_spinner_dropdown_item);
        SpSucursals.setAdapter(list);



        addproduct.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                if (product.getText().toString() == "") {
                    Toast.makeText(ClientProfile.this, spproduct, Toast.LENGTH_LONG);
                } else if (quantity.getText().toString() == "") {
                    Toast.makeText(ClientProfile.this, spquantity, Toast.LENGTH_LONG);
                } else if (searchproduct(product.getText().toString())) {
                    products.add(product.getText().toString());
                    quantities.add(quantity.getText().toString());
                    product.setText("");
                    quantity.setText("");
                    Toast.makeText(ClientProfile.this, success, Toast.LENGTH_LONG);
                } else {
                    Toast.makeText(ClientProfile.this, "There has been an error", Toast.LENGTH_LONG);
                }
            }
        });





        SendOrder.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                String sproducts="";
                String svalues = "";
                if((product.getText().toString()!= "") &&(quantity.getText().toString()!="")){
                    sproducts = product.getText().toString()+",";
                    svalues = quantity.getText().toString()+",";
                }else{
                    Toast.makeText(ClientProfile.this, "You must provide a product or a quantity",Toast.LENGTH_LONG);
                }
                for(int i = 0;i< products.size();i++){
                    if (i ==  products.size()-1){
                        sproducts += products.get(i);
                        svalues += quantities.get(i);
                    }else{
                        sproducts += products.get(i)+",";
                        svalues += quantities.get(i)+",";
                    }

                }if(!products.isEmpty()){
                    Random r = new Random();
                    Calendar c = Calendar.getInstance();
                    int hour = c.get(Calendar.HOUR);
                    int seg = c.get(Calendar.SECOND);
                    int min = c.get(Calendar.MINUTE);
                    int random = ((r.nextInt(1000-1)+1)*hour)/(seg+min);
                    AsyncHttpClient newclient = new AsyncHttpClient();
                    try {
                        JSONObject order = new JSONObject();
                        order.put("Products",sproducts);
                        order.put("Amount",svalues);
                        order.put("O_ID",random);
                        order.put("OPriority",0);
                        order.put("OStatus","Empacando");
                        order.put("OrderDate",c.get(Calendar.DATE));
                        order.put("OPlatform","Mobile");
                        order.put("S_ID",Subsidiary);
                        order.put("C_ID", clientID);
                        StringEntity se = new StringEntity(order.toString());
                        newclient.post(getApplicationContext(),"http://isaac:7549/api/order/post",se,"application/json",new JsonHttpResponseHandler());
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }catch (UnsupportedEncodingException e){
                        e.printStackTrace();
                    }
                }


            }
        });

        }


    @Override
    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
        Subsidiary = Integer.parseInt(parent.getItemAtPosition(position).toString());
        Toast.makeText(this,"Estoy aqui",Toast.LENGTH_LONG);
    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {

    }
    private boolean searchproduct(String search){
        for(int i = 0;i < ListProducts.size();i++){
            if(ListProducts.get(i)==search)
                return true;
            else if ((i==ListProducts.size()-1) && (ListProducts.get(i)!=search))
                return false;
        }
        return true;
    }
}
