
(function (app) {
    app.controller("orderListController", orderListController);

    orderListController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function orderListController(apiService, $scope, notificationService, $state) {

        $scope.order = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrder = getOrder;
        $scope.keyword = '';
        $scope.isAll = false;
        $scope.search = search;
        $scope.selectAll = selectAll;
        $scope.search = search;
        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {

        }

        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.order, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.order, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        function search() {
            getOrder();
        }

        function getOrder(page) {
            page = page || 0;
            config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize : 5
                }
            }
            apiService.get("api/order/getAll", config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayInfo("Không tìm thấy bản ghi nào thỏa mãn");
                } else {
                    notificationService.displaySuccess("Tìm thấy " + result.data.TotalCount + " bản ghi");
                }
                $scope.order = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.keyword = '';
            }, function () {
                console.log('Load order failed.');
            });

        }
        $scope.getOrder();
    }

})(angular.module("helloweb.order"));