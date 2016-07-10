'use strict';

fileSystemApp.controller('FileBrowserController', 
    function FileBrowserController($scope, $resource) {
        var fileSystem;

        $scope.isLoading = true;
        fileSystem = $resource('FileSystem').get(
            function ()
            {
                $scope.isLoading = false;
                $scope.FileSystem = fileSystem;
            } ,
            function () {              
                $scope.isLoading = false;
                //TODO:Improve show error message.
                alert("Somth. went wrong!");
            } );

        $scope.LoadItemInfo = function( item )
        {
            if (!item.IsFile)
            {                
                $scope.isLoading = true;
                fileSystem = $resource('FileSystem/:path', { path: '@path' }).get({ path: item.Path },
                function () {
                    $scope.isLoading = false;
                    $scope.FileSystem = fileSystem;
                },
                function () {
                    $scope.isLoading = false;
                    //TODO:Improve show error message.
                    alert("Somth. went wrong!");
                });
            }
        }
    }
);