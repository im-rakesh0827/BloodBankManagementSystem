function initializeToasts() {
    if (typeof toastr !== 'undefined') {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    } else {
        console.error("Toastr is not loaded.");
    }
}

function showErrorToast(message) {
    if (typeof toastr !== 'undefined') {
        toastr.error(message);
    } else {
        console.error("Toastr is not loaded.");
    }
}

function showSuccessToast(message) {
    if (typeof toastr !== 'undefined') {
        toastr.success(message);
    } else {
        console.error("Toastr is not loaded.");
    }
}

function initializeJQueryToasts() {
    //No need to do anything here for this plugin.
}

function ShowToastAlert(alertMessage, alertHeader, alertIcon) {
    $.toast({
        heading: alertHeader,
        text: alertMessage,
        icon: alertIcon,
        loader: true,
        showHideTransition: 'fade', // 'slide', 'plain'
        loaderBg: '#9EC600',
        // position: 'top-right' 
        position: 'bottom-right'
    });
}

function ShowDeleteConfirmation(objRef, message) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success mx-4",
            cancelButton: "btn btn-danger mx-4"
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            objRef.invokeMethodAsync('PerformDelete')
                .then(() => {
                    swalWithBootstrapButtons.fire({
                        title: "Deleted!",
                        text: message+" deleted successfully",
                        icon: "success"
                    }, 800);
                })
                .catch((error) => {
                    swalWithBootstrapButtons.fire({
                        title: "Error!",
                        text: "Error occured while deleting",
                        icon: "error"
                    });
                    console.error("Error deleting:", error);
                });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire({
                title: "Cancelled",
                text: message+" is safe.",
                icon: "error"
            });
        }
    });
}