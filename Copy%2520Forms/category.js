angular.module('category',[])
.controller('categoryFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addEmployee = function () {
        var Category = {
            "CA_ID": $scope.ca_id,
            "CDescription": $scope.name,
            
        }
        console.log(Employee);
        $http.post('http://DESKTOP-E6QPTVT:7549/api/category/post',Category).
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
            
         
        if(attributes==""){
            console.log(attributes, values);
        $http.get('http://DESKTOP-E6QPTVT:7549/api/category/get/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
            $scope.Category = data;
            
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving Category information!')
        });  
            
        }
            
        else{
            console.log(attributes, values);
            $http.get('http://DESKTOP-E6QPTVT:7549/api/category/get/asd/undefined').
            success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
            }).
            error(function (data, status, headers, config) {
                alert('Error retrieving Category information!')
            });      
    }}
    $scope.deleteEmployee = function (){
         $http.post('http://DESKTOP-E6QPTVT/7549/api/category/delete/CA_ID'+$scope.ca_id).success(function(data, status, headers, config){
             alert('The category has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the category')
         });
    }
    
    
}])