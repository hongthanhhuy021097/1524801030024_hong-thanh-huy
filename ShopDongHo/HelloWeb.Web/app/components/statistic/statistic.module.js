﻿
(function () {
    angular.module('helloweb.statistics', ['helloweb.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: "/statistic_revenue",
                parent: 'base',
                templateUrl: "/app/components/statistic/revenueStatisticView.html",
                controller: "revenueStatisticController"
            });
    }
})();