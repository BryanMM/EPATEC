angular.module('client',[])
.controller('clientFormCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.addClient = function () {
        var Client = {
            "C_ID": $scope.clientid,
            "FName": $scope.name,
            "LName": $scope.lastname,
            "CAddress": $scope.address,
            "Phone": $scope.phone,
            "Day":1,
            "Month":2,
            "Year":3,
            "Penalization":7
        }
        console.log(Client); $http.post('http://isaac:7549/api/client/post',Client).
        success(function (data, status, headers, config) {
            alert('Client has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('gracias a la vida que me ha dado tanto')
        });
    }
} ])

