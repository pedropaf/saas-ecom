'use strict';
app.controller('loginController', function ($scope, $rootScope, AUTH_EVENTS, AuthService) {

    $scope.credentials = {
        username: '',
        password: ''
    };

    $scope.login = function (credentials) {
        AuthService.login(credentials.username, credentials.password).then(function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginSuccess);
        }, function () {
            $rootScope.$broadcast(AUTH_EVENTS.loginFailed);
        });

        console.log("login token: " + AuthService.loginToken);
    };
});
