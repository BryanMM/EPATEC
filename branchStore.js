angular.module('branchs',[])
.controller('branchFormCtrl', ['$scope', '$http', function ($scope, $http) {
    var branchs;
   
    
    var idBranch;

    $scope.addBranch = function () {

        var Branch = {
            "S_ID": $scope.branchid,
            "SName": $scope.name,
            "SAddress": $scope.address,
            "SPassword":$scope.password
            
        }
        console.log(Branch); $http.post('http://isaac:7549/api/sucursal/post',Client).
        success(function (data, status, headers, config) {
            alert('Branch has been posted');
        }).
        error(function (data, status, headers, config) {
            alert('error posting branch')
        });
    }
    $scope.getBranch = function () {
        var attributes="";
        var values = "";
            
        
            if(Boolean($scope.branchid)){
                attributes+="S_ID";
                console.log(attributes);
                values+=$scope.branchid;
                console.log(attributes,values);
            }
            if(Boolean($scope.name)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="SName";
                values+=$scope.name;
            }
            if(Boolean($scope.address)){
                if(Boolean(attributes)){
                    attributes+=",";
                    values+=",";}
                attributes+="SAddress";
                values+=$scope.address;
            }
           
         
        console.log(attributes, values);
        $http.get('http://isaac:7549/api/sucursal/get/'+attributes+"/"+values)
            .then( function (response) {    
              $scope.branchs = response.data;           
           });      
    }
    $scope.deleteBranch = function (pidBranch){
         idBranch= pidBranch;
         $http.get('http://isaac:7549/api/sucursal/delete/S_ID/'+idBranch).success(function(data, status, headers, config){
             alert('The branch has been deleted!');
         }).error(function(data, status, headers, config){
             alert('Error while deleting the branch')
         });
    }
    $scope.updateBranch = function (pid,pName,pAddress,pPassword){
        var branchAttributes = "SName,SAddress,SPassword";
       
        
        
         
         $http.get('http://isaac:7549/api/sucursal/update/'+branchAttributes+"/"+pName+","+pAddress+","+pPassword+"/"+"S_ID"+"/"+pid).success(function(data, status, headers, config){
             alert('The branch has been update!');
         }).error(function(data, status, headers, config){
             alert('Error while updating the branch')
         });
    }
    
    
}])