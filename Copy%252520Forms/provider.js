angular.module('provider',[])
.controller('providerFormCtrl', ['$scope', '$http', function ($scope, $http) {
    var branchStores;
      console.log(clientID); $http.get('http://isaac:7549/api/sucursal/get/S_ID/undefined')
            .then( function (response) {    
              $scope.branchStores = response.data;           
        });
    $scope.addProvider = function () {
        var Provider = {
            "P_ID": $scope.p_ID,
            "PName": $scope.pName,
            "LName": $scope.lName,
            "PAddress": $scope.pAddress,
            "Phone": $scope.phone,
            "Day": $scope.day,
            "Month": $scope.month,
            "Year": $scope.year
            
        }
        console.log(Provider);
        $http.post('http://DESKTOP-E6QPTVT:7549/api/provider/post',Provider).
        success(function (data, status, headers, config) {
            alert('Provider has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('Error posting the new provider!')
        });
    }
    
    $scope.deleteProvider = function (p_id){
         idClient= pidClient;
         $http.get('http://DESKTOP-E6QPTVT:7549/api/provider/delete/P_ID/'+p_id).success(function(data, status, headers, config){
             alert('The client has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the client')
         });
    }
    $scope.getProvider = function () {
        var attributes="";
        var values = "";
            
            if(Boolean($scope.p_ID)){
                attributes+="P_ID";
                console.log(attributes);
                values+=$scope.p_ID;
                console.log(attributes,values);
            }
            if(Boolean($scope.pName)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="PName";
                values+=$scope.pName;
            }
          if(Boolean($scope.lName)){
            if(Boolean(attributes)){
                attributes+=",";
                values+=",";}
            attributes+="LName";
            values+=$scope.lName;
            }
          if(Boolean($scope.pAddress)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="PAddress";
                values+=$scope.pAddress;
            }
          if(Boolean($scope.phone)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Phone";
                values+=$scope.phone;
            }
          if(Boolean($scope.day)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Day";
                values+=$scope.day;
            }
          if(Boolean($scope.month)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Month";
                values+=$scope.month;
            }
          if(Boolean($scope.year)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Year";
                values+=$scope.year;
            }
                
        console.log(attributes, values);
        $http.get('http:///DESKTOP-E6QPTVT:7549/api/provider/get/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
             $scope.Provider = data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving providers information!')
        }); }    
} ])