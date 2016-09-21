angular.module('statistics',[])
.controller('statisticsFormCtrl', ['$scope', '$http', function ($scope, $http) {
   
    $scope.getSales = function () {
        var attributes="";
        var values = "";
            
            if(Boolean($scope.S_ID)){
                attributes+="S_ID";
                console.log(attributes);
                values+=$scope.S_ID;
                console.log(attributes,values);
            }
            if(Boolean($scope.platform)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Platform";
                values+=$scope.platform;
            }
            
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/order/get/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
             $scope.salesList = response.data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving sales information!')
        });      
    }
    $scope.salesAmount = function (){
         $http.post('http://isaac/7549/api/orders/get/E_ID'+$scope.e_ID).success(function(data, status, headers, config){
             alert('The employee has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the employee')
         });
    }
    
    
}])
 
