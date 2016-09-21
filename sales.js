angular.module('sales',[])
.controller('salesFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addClient = function () {
        var Product = {
            "P_ID": $scope.name,
            "Price": $scope.phone,
            "Excent": $scope.priority,
            "PDescription": $scope.status,
            "Quantity": $scope.quantity
        }
        console.log(Product);
        $http.post('http://isaac:7549/api/product/post',Product).
        success(function (data, status, headers, config) {
            alert('Product has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('gracias a la vida que me ha dado tanto')
        });
    }
} ])

