angular.module('employee',[])
.controller('employeeFormCtrl', ['$scope', '$http', function ($scope, $http) {
    var branchStores;
    var employees;
    $http.get('http://isaac:7549/api/sucursal/get/S_ID/undefined')
            .then( function (response) {    
              $scope.branchStores = response.data;           
        });
      
    $scope.addEmployee = function () {
        var Employee = {
            "E_ID": $scope.e_id,
            "CName": $scope.name,
            "LName": $scope.lname,
            "CAddress": $scope.address,
            "Charge": $scope.charge,
            "CPassword": $scope.pass,
            "S_ID": $scope.branchStore
        }
        console.log(Employee);
        $http.post('http://isaac:7549/api/employees/post',Employee).
        success(function (data, status, headers, config) {
            alert('the new employee has been posted!');
        }).
        error(function (data, status, headers, config) {
            alert('Error while posting the new employee')
        });
      
    }
    $scope.getEmployee = function () {
        var attributes="";
        var values = "";
            
            if(Boolean($scope.e_id)){
                attributes+="E_ID";
                console.log(attributes);
                values+=$scope.e_id;
                console.log(attributes,values);
            }
            if(Boolean($scope.name)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="CName";
                values+=$scope.name;
            }
            if(Boolean($scope.lname)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="LName";
                values+=$scope.lname;
            }
            if(Boolean($scope.address)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="CAddress";
                values+=$scope.address;
            }
            if(Boolean($scope.charge)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Charge";
                values+=$scope.charge;
            }
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/employees/get/'+attributes+"/"+values)
        .then( function (response) {    
              $scope.employees = response.data;           
        });    
    }
    $scope.deleteEmployee = function (){
         $http.post('http://isaac/7549/api/employees/delete/E_ID'+$scope.e_ID).success(function(data, status, headers, config){
             alert('The employee has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the employee')
         });
    }
    
    
}])
 