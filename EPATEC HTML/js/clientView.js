angular.module("clientView", [])
    .controller("clientViewForm", ['$scope', '$http', '$location',  function($scope, $http,$location){
      var iframe
   /*   $scope.displayOrder = funtion(){
          iframe="/pages/orderForm.html";
          
      }*/
    $("order").click(function(event) {
          console.log("sdmszsds");
          //iframe ="/pages/orderForm.html" ;
          $("#frame").load("/pages/orderForm.html");
          
          
      });
      $scope.displayDelete = function (){
          console.log("sdmszsds");
          iframe="/pages/login.html";
      
                                   
          
      }
    /* $(document).ready(function(){
            $("#order").click(function(event) {
           // iframe="/pages/orderForm.html";
            $("#frame").load("/pages/orderForm.html");
        });*/
    }]);