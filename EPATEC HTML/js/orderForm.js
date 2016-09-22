var app2=angular.module('orders',[])
.controller('orderFormCtrl', ['$scope', '$http','$location', function ($scope, $http, $location, user, charge) {
      var branchStores;
      var clientID = localStorage.user;
      var productList;
      var Order;
      var  listProduct=[]; 
      var products=""; 
      var amounts=""; 
      var date=new Date();
      var factura;

     console.log(clientID); $http.get('http://isaac:7549/api/sucursal/get/S_ID/undefined')
            .then( function (response) {    
              $scope.branchStores = response.data;           
        });
      
      $scope.addToCart = function (value1,value2){
          
          if(products=="" && amounts=="" ){
              products=value1;
              amounts=value2;
              
          }
          else{
              products = products+ "," + value1 ;
              amounts = amounts+","  + value2  ;
          }
          console.log('http://isaac:7549/api/preorder/'+products+'/'+amounts);
           
      } 
      $scope.addProduct = function () {
          
         $http.get('http://isaac:7549/api/product/get/PName/'+$scope.productName)
            .then( function (response) {
            $scope.productList = response.data;
          
        });
       
        console.log(Order);

        $scope.sendOrder = function () {
          order={
              "S_ID": $scope.branchStore,
              "C_ID": $scope.clientID,
              "Phone":$scope.number,
              "Status":$scope.status
              
              
          }}

    
    
    
    }
      
 
      
}]);

