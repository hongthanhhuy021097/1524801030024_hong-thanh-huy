(function (app) {
    app.controller('feedBacksListController', feedBacksListController);

    feedBacksListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function feedBacksListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.feedBacks = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.search = search;

        function search() {
            getAllFeedBacks();
        }

        function getAllFeedBacks(page) {
            page = page || 0;
            config = {
                params: {
                    page : page,
                    pageSize : 5
                }
            }
            apiService.get("/api/feedBacks/getAll", config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.');
                } else {
                    notificationService.displaySuccess('Có ' + result.data.TotalCount + ' bản ghi được tìm thấy.');
                }
                $scope.feedBacks = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function(result){
                    notificationService.displayWarning(result.data.Message);
                });
        }
        getAllFeedBacks();
    }

})(angular.module("helloweb.feedBacks"))