'use strict';

var fileSystemApp = angular.module('fileSystemApp', ['ngResource']);

fileSystemApp.filter('escape', function () {
    return window.encodeURIComponent;
});