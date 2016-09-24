angular.module('client',[])
.controller('clientFormCtrl', ['$scope', '$http', function ($scope, $http) {
    var date;
    var year ;
    var day;
    var month;
    $scope.addClient = function () {
        date = new Date($scope.birthdate);
        day = date.getDate();
        year = date.getFullYear();
        
        month = date.getMonth() + 1;
        var Client = {
            "C_ID": $scope.clientid,
            "FName": $scope.name,
            "LName": $scope.lastname,
            "CAddress": $scope.address,
            "Phone": $scope.phone,
            "Day":day,
            "Month":month,
            "Year":year,
            "Penalization":$scope.penalization,
            "CPassword":$scope.password
        }
        console.log(Client); $http.post('http://isaac:7549/api/client/post',Client).
        success(function (data, status, headers, config) {
            alert('Client has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('error posting client')
        });
    }
} ])

