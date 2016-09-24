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
            if(Boolean($scope.CPassword)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="EPassword";
                values+=$scope.pass;
            }
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/employees/get/'+attributes+"/"+values)
        .then( function (response) {    
              $scope.employees = response.data;           
        });    
    }
    $scope.editEmployee = function(){
        var pID= document.getElementById('idTable').textContent;
        var pName= document.getElementById('nameTable').textContent;
        var pLName= document.getElementById('lnameTable').textContent;
        var pAddress= document.getElementById('addressTable').textContent;
        var pCharge= document.getElementById('chargeTable').textContent;
        var pPass= document.getElementById('passTable').textContent;
       
          console.log(pID+","+ pName+","+pLName+","+pAddress+","+pCharge+","+pPass);
          $http.get('http://isaac:7549/api/employees/update/CName,LName,CAddress,Charge,EPassword,S_ID/'+ pName+","+pLName+","+pAddress+","+pCharge+","+pPass+","+$scope.branchStore
                  +"/"+"E_ID"+"/"+pID )
        .then( function (response) {
                
                         
        });}
        
    
    $scope.deleteEmployee = function (pID){
         $http.get('http://isaac:7549/api/employees/delete/E_ID/'+pID).then( function(response){
             alert('The employee has been deleted!');
         });
    }
    
    
}])
 