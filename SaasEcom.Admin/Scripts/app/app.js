
/*######################*********************#############################
  
 Created by: Pedro Alonso
  http://twitter.com/pedropaf
  http://pedroalonso.net

 ######################*********************##############################*/

var app = angular.module('admin', ['ngRoute', 'ngResource', 'ui.bootstrap', 'toaster', 'chieffancypants.loadingBar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/Scripts/app/views/login.html"
    });

    $routeProvider.when("/about", {
        controller: "aboutController",
        templateUrl: "/Scripts/app/views/about.html"
    });

    $routeProvider.otherwise({ redirectTo: "/login" });
});

app.constant('AUTH_EVENTS', {
    loginSuccess: 'auth-login-success',
    loginFailed: 'auth-login-failed',
    logoutSuccess: 'auth-logout-success',
    sessionTimeout: 'auth-session-timeout',
    notAuthenticated: 'auth-not-authenticated',
    notAuthorized: 'auth-not-authorized'
});

app.constant('USER_ROLES', {
    all: '*',
    admin: 'admin',
    editor: 'editor',
    guest: 'guest'
});

