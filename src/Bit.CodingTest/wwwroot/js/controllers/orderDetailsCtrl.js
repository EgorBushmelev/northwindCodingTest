(function() {
    angular.module("bctApp")
        .controller("orderDetailsCtrl", ["$scope", "$uibModal", "$http", "sweetAlert", "viewModel", "urls", function($scope, $uibModal, $http, sweetAlert, viewModel, urls) {
            $scope.products = viewModel.products;
            $scope.orderDetails = viewModel.order.orderDetails;
            $scope.order = viewModel.order;

            $scope.deleteModal = function(detail) {
                sweetAlert.swal({
                    title: "Are you sure you want to delete the order detail?",
                    text: "You won't be able to revert this!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Delete"
                }).then(function() {
                    $http.post(urls.deleteOrderDetail, detail).then(function() {
                        location.reload();
                    });
                });
            };

            $scope.showForm = function(detail) {
                var modalInstance = $uibModal.open({
                    templateUrl: "orderDetailModal.html",
                    controller: OrderDetailsModalInstanceCtrl,
                    scope: $scope,
                    resolve: {
                        orderId: function() {
                            return viewModel.order.orderId;
                        },
                        orderDetails: function() {
                            return $scope.orderDetails;
                        },
                        products: function() {
                            return $scope.products;
                        },
                        detail: detail,
                        urls: function() {
                            return urls;
                        }
                    }
                });

                modalInstance.result.then(function() {
                    location.reload();
                });
            };
        }]);

    var OrderDetailsModalInstanceCtrl = function($scope, $http, $uibModalInstance, orderId, orderDetails, products, detail, urls) {
        if (detail === undefined) {
            $scope.isNewDetail = true;
            $scope.detail = {
                orderId: orderId
            };
        } else {
            $scope.detail = _.clone(detail);
        }

        $scope.availableProducts = _.filter(products, function(product) {
            return detail && detail.product.id === product.id ||!_.find(orderDetails, function(orderDetail) {
                return product.id === orderDetail.product.id;
            });
        });

        $scope.submitForm = function() {
            if ($scope.form.orderDetailsForm.$valid) {
                $http.post(urls.editOrderDetail, $scope.detail).then(function() {
                    $uibModalInstance.close("closed");
                });
            }
        };

        $scope.cancel = function() {
            $uibModalInstance.dismiss("cancel");
        };
    };
})();
