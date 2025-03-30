// window.popupUtils = {
//     enableEscapeClose: function (dotnetHelper) {
//         document.addEventListener("keydown", function (event) {
//             if (event.key === "Escape") {
//                 dotnetHelper.invokeMethodAsync("ClosePopup");
//             }
//         });
//     }
// };



window.popupUtils = {
    enableEscapeClose: function (dotNetHelper) {
        document.addEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                dotNetHelper.invokeMethodAsync("ClosePopup");
            }
        });
    },
    disableEscapeClose: function (dotNetHelper) {
        document.removeEventListener("keydown", function (event) {
            if (event.key === "Escape") {
                dotNetHelper.invokeMethodAsync("ClosePopup");
            }
        });
    }
};
