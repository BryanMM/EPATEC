var app=angular.module("login", [])
    .controller("loginFormCtrl", ['$scope', '$http', '$location', //function($scope, $http, $location, InforUser, InforUserArtist)  {
    function($scope, $http,$location,$locationProvider)  {                                
        
      
      $scope.checkUser = function (){
  
          
         if($scope.charge=="Dependent"){
                console.log("Login as Dependent");
                $http.get('http://isaac:7549/api/check/get/'+$scope.login+'/'+$scope.pass+'/'+$scope.charge)
                .then( function (response) {
                var data  = response.data;
                console.log(data);
                if(data=="true"){
                    localStorage.setItem("user", $scope.login); 
                    window.location.assign("/pages/dependentView.html")
                }
                else{
                    alert("Invalid Dependent")
                }});
                         
                }
        else if($scope.charge=="Admin"){
                console.log("Login as Admin");
                $http.get('http://isaac:7549/api/check/get/'+$scope.login+'/'+$scope.pass+'/'+$scope.charge)
                .then( function (response) {
                var data  = response.data;
                console.log(data);
              
                if(data=="true"){
                    localStorage.setItem("user", $scope.login); 
                    window.location.assign("/pages/adminView.html")
                }
                else{
                    alert("Invalid Admin")
                }
                         
                });
        }          
        else if($scope.charge=="Manager") {
                console.log("Login as Manager");
                $http.get('http://isaac:7549/api/check/get/'+$scope.login+'/'+$scope.pass+'/'+$scope.charge)
                .then( function (response) {
                var data  = response.data;
                console.log(data);
                
                if(data=="true"){
                    localStorage.setItem("user", $scope.login); 
                    window.location.assign("/pages/managerView.html")
                }
                else{
                    alert("Invalid Manager")
                }
                         
                });
        }  
                          
        else if($scope.charge=="Client") { 
              console.log("Login as Client");
              $http.get('http://isaac:7549/api/check/get/'+$scope.login+'/'+$scope.pass+'/'+$scope.charge)
                .then( function (response) {
                var data  = response.data;
                console.log(data);
                //user.data.login = $scope.login;
                if(data=="true"){
                    localStorage.setItem("user", $scope.login); 
                    window.location= "/pages/clientView.html" 
                  
                   
                
                }
                else{
                    alert("Invalid Manager");
                }
                         
                })
            } 
                
            
           // window.location = '/clientView.html';
        }
    }]);
