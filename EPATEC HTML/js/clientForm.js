angular.module('clients',[])
.controller('clientFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addClient = function () {
        var Client = {
            "C_ID": $scope.clientid,
            "FName": $scope.name,
            "LName": $scope.lastname,
            "CAddress": $scope.address,
            "Phone": $scope.phone,
            "Day":1,
            "Month":2,
            "Year":3,
            "Penalization":7
        }
        console.log(Client); $http.post('http://isaac:7549/api/client/post',Client).
        success(function (data, status, headers, config) {
            alert('Client has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('gracias a la vida que me ha dado tanto')
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
        $http.get('http://isaac:7549/api/client/get/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
            console.log(JSON.stringify(data));
        }).
        error(function (data, status, headers, config) {
            alert('gracias a la vida que me ha dado tanto')
        });      
    }
    $scope.deleteClient = function (){
         $http.post('http://isaac/7549/api/client/delete/C_ID'+$scope.e_ID).success(function(data, status, headers, config){
             alert('The client has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the employee')
         });
    }
    
    
}])