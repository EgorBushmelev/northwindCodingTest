(function() {
    angular.module("bctApp")
        .controller("ordersListCtrl", ["$scope", "$http", "sweetAlert", "viewModel", "urls", function($scope, $http, sweetAlert, viewModel, urls) {
            $scope.orders = viewModel;
            $scope.urls = urls;

            $scope.filteredOrders = [];
            $scope.currentPage = 1;
            $scope.numPerPage = 10;
            $scope.maxSize = 5;

            $scope.$watch("currentPage + numPerPage", function() {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage);
                var end = begin + $scope.numPerPage;

                $scope.filteredOrders = $scope.orders.slice(begin, end);
            });


            $scope.deleteModal = function(order) {
                sweetAlert.swal({
                    title: "Are you sure you want to delete the order?",
                    text: "You won't be able to revert this!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Delete"
                }).then(function() {
                    $http.post(urls.deleteOrder, order.orderId).then(function() {
                        location.reload();
                    });
                });
            };
        }]);
})();
