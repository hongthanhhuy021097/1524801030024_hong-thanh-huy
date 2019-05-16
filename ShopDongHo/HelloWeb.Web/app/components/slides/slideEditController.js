(function (app) {
    app.controller('slideEditController', SlideEditController);

    SlideEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function SlideEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.Slide = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdateSlide = UpdateSlide;
        $scope.moreImages = [];
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.Slide.Alias = commonService.getSeoTitle($scope.Slide.Name);
        }

        function loadSlideDetail() {
            apiService.get('api/slide/getbyid/' + $stateParams.id, null, function (result) {
                $scope.Slide = result.data;
                $scope.moreImages = JSON.parse($scope.Slide.MoreImages);
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateSlide() {
            $scope.Slide.MoreImages = JSON.stringify($scope.moreImages)
            apiService.put('api/slide/update', $scope.Slide,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('slides');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        function loadSlide() {
            apiService.get('api/slide/getallparents', null, function (result) {
                $scope.SlideCategories = result.data;
            }, function () {
                console.log('Cannot get list parent');
            });
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.Slide.Image = fileUrl;
                })
            }
            finder.popup();
        }
        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }
        loadSlideDetail()
    }

})(angular.module('helloweb.slides'));