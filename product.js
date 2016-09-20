angular.module('products',[])
.controller('productFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addProduct = function () {
        var Product = {
            "PR_ID": $scope.id,
            "PName": $scope.name,
            "Price": $scope.price,
            "Quantity": $scope.quantity,
            "Extent": $scope.extent,
            "PDescription": $scope.description,
           
            "S_ID": $scope.sucursalid,
            "P_ID": $scope.proveedorid,
      
        }
        console.log(Product); $http.post('http://isaac:7549/api/product/post',Product).
        success(function (data, status, headers, config) {
            alert('Product has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('error adding product')
        });
    }
    $scope.getProduct = function () {
        var attributes="";
        var values = "";
            
        
            if(Boolean($scope.id)){
                attributes+="PR_ID";
                console.log(attributes);
                values+=$scope.id;
                console.log(attributes,values);
            }
            if(Boolean($scope.name)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="PName";
                values+=$scope.name;
            }
            if(Boolean($scope.price)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Price";
                values+=$scope.price;
            }
            if(Boolean($scope.quantity)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Quantity";
                values+=$scope.quantity;
            }
            if(Boolean($scope.extent)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Extent";
                values+=$scope.extent;
            }
            if(Boolean($scope.description)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="PDescription";
                values+=$scope.description;
            }
            if(Boolean($scope.sucursalid)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="S_ID";
                values+=$scope.sucursalid;
            }
            if(Boolean($scope.proveedorid)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="P_ID";
                values+=$scope.proveedorid;
            }
            if(Boolean($scope.category)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="Category";
                values+=$scope.category;
            }
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/product/get/'+attributes+"/"+values)
            .then( function (response) {
            $scope.productList = response.data;
         });
       
        success(function (data, status, headers, config) {
            
            alert(JSON.stringify(data));
            console.log(JSON.stringify(data));
        }).
        error(function (data, status, headers, config) {
            alert('error searching product')
        });      
    }
    $scope.deleteProduct = function (product){
         $http.post('http://isaac/7549/api/product/delete/PR_ID'+'/'+product).success(function(data, status, headers, config){
             alert('The product has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the product')
         });
    }
    $scope.showSucursal = function () {

        $http.get('http://isaac:7549/api/product/get/S_ID').
        success(function (data, status, headers, config) {
            alert(JSON.stringify(data));
            console.log(JSON.stringify(data));
        }).
        error(function (data, status, headers, config) {
            alert('error showing sucursal')
        });      
    }
    $scope.showCategory = function () {

        $http.get('http://isaac:7549/api/product/get/Category').
        success(function (data, status, headers, config) {
            alert(JSON.stringify(data));
            console.log(JSON.stringify(data));
        }).
        error(function (data, status, headers, config) {
            alert('error showing categorys')
        });      
    }
    
    
}])