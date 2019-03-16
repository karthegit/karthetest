var confirmationCallBack;
function confirmationAlert(message, callback) {
    $("#confirmationModal .message").html(message);
    $("#confirmationModal").modal('show');
    confirmationCallBack = callback;
}
$("#confirmationModal").on("click", "#confirmBtnYes", function () {
    $("#confirmationModal").modal('hide');
    confirmationCallBack(true);

    // return true;
});

$("#confirmationModal").on("click", "#confirmBtnNo", function () {
    confirmationCallBack(false);
    $("#confirmationModal").modal('hide');
});

function successAlert(message) {
    $("#successMsgModal .message").text(message);
    $("#successMsgModal").modal('show');
}
function failureAlert(message) {
    $("#failureMsgModal .message").text(message);
    $("#failureMsgModal").modal('show');
}

function informationAlert(message) {
    $("#informationMsgModal .message").text(message);
    $("#informationMsgModal").modal('show');
}

function hideAlert() {
    $("#successMsgModal , #failureMsgModal, #informationMsgModal").modal('hide');
}

$("#chartPopupCloseBtn").on("click", function () {
    $("#chartPopupModal").modal('hide');
});

var timeoutId;
window.onresize = function () {
    window.clearTimeout(timeoutId);
    //call the function after a 300 second
    timeoutId = window.setTimeout(setPageHeightOnresize, 10);
}

function setPageHeightOnresize() {
    var minHeightValue2 = jQuery(window).height() - jQuery("#header").outerHeight() - jQuery("#footer").outerHeight();
    $("#sideBarMain").css("min-height", minHeightValue2 + "px");
    $("#page-content-wrapper").css("min-height", minHeightValue2 + "px");
    var minWidthValue = $(window).width() - $("#sideBarMain").width() - parseInt($("#main-content-area").css("padding-right"), 10) - 2;
    $("#main-content-area").css("min-width", minWidthValue + "px");
}

$(window).resize(setPageHeightOnresize);

//check is admin and survey data availability
function pageInit(companyId, isAdmin) {
    $("#page-spinner").hide();
    if (isAdmin == 'True') {
        $("#no-survey-data-normal").addClass("hide").removeClass("show");

        if (companyId != null) {
            $("#page-spinner").show();
            $("#data-avail").removeClass("hide").addClass("show");
            $("#admin-user").addClass("hide").removeClass("show");
            $(".printAll").addClass("show").removeClass("hide");
            return true;
        }
        else {
            $("#data-avail").removeClass("show").addClass("hide");
            $(".printAll").removeClass("show").addClass("hide");
            $("#admin-user").addClass("show").removeClass("hide");
            return false;
        }
    } else {
        if (localStorage.getItem("nodata") == 'true') {
            $("#admin-user").addClass("hide");
            $("#no-survey-data-normal").addClass("show").removeClass("hide");
            $("#data-avail").addClass("hide").removeClass("show");
            $(".printAll").addClass("hide").removeClass("show");

            return false;
        }
        else {
            $("#no-survey-data-normal").addClass("hide").removeClass("show");
            $("#data-avail").addClass("show").removeClass("hide");
            $(".printAll").addClass("show").removeClass("hide");
        }
        return true;
    }
    return false;
}
