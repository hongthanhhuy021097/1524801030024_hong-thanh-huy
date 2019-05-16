(function () {
    angular.module('helloweb.feedBacks', ['helloweb.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('feedBacks', {
            url: "/feedBacks",
            templateUrl: "/app/components/feedBacks/feedBacksListView.html",
            parent: 'base',
            controller: "feedBacksListController"
        })
    }
})();