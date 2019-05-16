(function (app) {
    app.controller('orderDetailListController', orderDetailListController);
    orderDetailListController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams'];

    function orderDetailListController(apiService, $scope, notificationService, $state, $stateParams) {
        $scope.orderDetail = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.search = search;
        $scope.loadOrderDetail = loadOrderDetail;

        function search() {
            loadOrderDetail();
        }

        function loadOrderById() {
            apiService.get('api/order/getOrderById/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        function loadProducts(page) {
            page = page || 0
            var config = {
                params: {
                    keyword: '',
                    page: page,
                    pageSize: 1000
                }
            }
            apiService.get('api/product/getall', config, function (result) {
                $scope.products = result.data.Items;
            }, function () {
                console.log('Cannot get list parent');
            });
        }

        function loadOrderDetail(page) {
            page = page || 0;
            config = {
                params: {
                    id : $stateParams.id,
                    page: page,
                    pageCount: 5
                }
            }

            apiService.get('api/order/getOrderDetailbyid', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayInfo("Không tìm thấy bản ghi nào thỏa mãn");
                } else {
                    notificationService.displaySuccess("Tìm thấy " + result.data.TotalCount + " bản ghi");
                }
                $scope.orderDetail = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.keyword = '';
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.loadOrderDetail();
        loadOrderById();
        loadProducts();
    }

})(angular.module("helloweb.order"));