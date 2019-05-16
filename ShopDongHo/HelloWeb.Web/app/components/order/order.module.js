(function () {

    angular.module('helloweb.order', ['helloweb.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state("order", {
            url: "/order",
            parent: 'base',
            templateUrl: "/app/components/order/orderListView.html",
            controller: "orderListController"
        }).state("edit_orderDetail", {
            url: "/edit_orderDetail/:id",
            parent: "base",
            templateUrl: "/app/components/order/orderDetailListView.html",
            controller: "orderDetailListController"
        })
    };
})();