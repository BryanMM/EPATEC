angular.module('category',[])
.controller('categoryFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addEmployee = function () {
        var Employee = {
            "CA_ID": $scope.ca_id,
            "CDescription": $scope.name,
            
        }
        console.log(Employee);
        $http.post('http://isaac:7549/api/employees/post',Employee).
        success(function (data, status, headers, config) {
            alert('the new category has been posted!');
        }).
        error(function (data, status, headers, config) {
            alert('Error while posting the new category')
        });
      
    }
    $scope.getCategory = function () {
        var attributes="";
        var values = "";
            
            if(Boolean($scope.ca_id)){
                attributes+="CA_ID";
                console.log(attributes);
                values+=$scope.ca_id;
                console.log(attributes,values);
            }
            if(Boolean($scope.cdescription)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="CDDescription";
                values+=$scope.cdescription;
            }
            
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/category/get/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
            console.log(JSON.stringify(data));
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving Category information!')
        });      
    }
    $scope.deleteEmployee = function (){
         $http.post('http://isaac/7549/api/category/delete/CA_ID'+$scope.ca_id).success(function(data, status, headers, config){
             alert('The category has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the category')
         });
    }
    
    
}])