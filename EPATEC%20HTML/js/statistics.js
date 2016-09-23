angular.module('statistics',[])
.controller('statisticsFormCtrl', ['$scope', '$http', function ($scope, $http) {
   var Sales;
    $scope.case1 = function (){
         $http.get('http://DESKTOP-5CS494L:7549/api/Statistic/GetMostSold/case1/1').
        success(function (data, status, headers, config) {
             $scope.Sales = data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving sales information!')
        });   
         
    }
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
                attributes+="OPlatform";
                values+=$scope.platform;
            }
        if(attributes==""){
        
         $http.get('http://DESKTOP-5CS494L:7549/api/Statistic/GetMostSold/case0/1').
        success(function (data, status, headers, config) {
             $scope.Sales = data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving sales information!')
        });   
         
    }
            
        else{ 
        console.log(attributes, values);
        $http.get('http:///DESKTOP-5CS494L:7549/api/Statistic/GetMostSold/'+attributes+"/"+values).
        success(function (data, status, headers, config) {
             $scope.Sales = data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving sales information!')
        }); }}     
    
    
    $scope.case2 = function (){
         $http.get('http://DESKTOP-5CS494L:7549/api/Statistic/GetMostSold/case0/1').
        success(function (data, status, headers, config) {
             $scope.Sales = data;
        }).
        error(function (data, status, headers, config) {
            alert('Error retrieving sales information!')
        });   
         
    }
    
    
}])
 
