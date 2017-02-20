(function() {
    angular.module("bctApp")
        .controller("editCtrl", ["$scope", "$http", "datetime", "viewModel", "urls",
            function($scope, $http, datetime, viewModel, urls) {
                $scope.customers = viewModel.customers;
                $scope.employees = viewModel.employees;
                $scope.shipVia = viewModel.shipVia;
                $scope.urls = urls;
                $scope.order = viewModel.order || {};
                $scope.submit = function() {
                    $http.post(urls.save, $scope.order).then(function(response) {
                        location.href = urls.redirect + response.data.orderId;
                    });
                }
            }
        ]);
})();
