angular.module('provider',[])
.controller('providerFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addProvider = function () {
        var Provider = {
            "P_ID": $scope.p_ID,
            "PName": $scope.pName,
            "LName": $scope.lName,
            "PAddress": $scope.pAddress,
            "Phone": $scope.phone,
            "Day": $scope.day,
            "Month": $scope.month,
            "Year": $scope.year
        }
        console.log(Provider);
        $http.post('http://isaac:7549/api/provider/post',Provider).
        success(function (data, status, headers, config) {
            alert('Provider has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('gracias a la vida que me ha dado tanto')
        });
    }
} ])