angular.module('clients',[])
.controller('clientFormCtrl', ['$scope', '$http', function ($scope, $http) {
    var clients;
   
    
    var idClient;
    var date;
    var year ;
    var day;
    var month;
    $scope.addClient = function () {
        date = new Date($scope.birthdate);
        day = date.getDate();
        year = date.getFullYear();
        
        month = date.getMonth() + 1;
        var Client = {
            "C_ID": $scope.clientid,
            "FName": $scope.name,
            "LName": $scope.lastname,
            "CAddress": $scope.address,
            "Phone": $scope.phone,
            "Day":day,
            "Month":month,
            "Year":year,
            "Penalization":$scope.penalization,
            "CPassword":$scope.password
        }
        console.log(Client); $http.post('http://isaac:7549/api/client/post',Client).
        success(function (data, status, headers, config) {
            alert('Client has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('error posting client')
        });
    }
    $scope.getClient = function () {
        var attributes="";
        var values = "";
            
        
            if(Boolean($scope.clientid)){
                attributes+="C_ID";
                console.log(attributes);
                values+=$scope.clientid;
                console.log(attributes,values);
            }
            if(Boolean($scope.name)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="FName";
                values+=$scope.name;
            }
            if(Boolean($scope.lastname)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="LName";
                values+=$scope.lastname;
            }
            if(Boolean($scope.address)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="CAddress";
                values+=$scope.address;
            }
            if(Boolean($scope.phone)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Phone";
                values+=$scope.phone;
            }
            if(Boolean($scope.penalization)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Penalization";
                values+=$scope.penalization;
            }
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/client/get/'+attributes+"/"+values)
            .then( function (response) {    
              $scope.clients = response.data;           
           });      
    }
    $scope.deleteClient = function (pidClient){
         idClient= pidClient;
         $http.get('http://isaac:7549/api/client/delete/C_ID/'+idClient).success(function(data, status, headers, config){
             alert('The client has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the client')
         });
    }
    $scope.updateClient = function (pid,pName,pLName,pAddress,pPhone,pDay,pMonth,pYear,pPenalization,pPassword){
        var clientAttributes = "FName,LName,CAddress,Phone,Day,Month,Year,Penalization,CPassword";
       
        
        
         
         $http.get('http://isaac:7549/api/client/update/'+clientAttributes+"/"+pName+","+pLName+","+pAddress+","+pPhone+","+pDay+","+pMonth+","+pYear+","+pPenalization+","+pPassword+"/"+"C_ID"+"/"+pid).success(function(data, status, headers, config){
             alert('The client has been update!');
         }).error(function(data, status, headers, config){
             alert('Error while updating the client')
         });
    }
    
    
}])