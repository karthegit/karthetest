function uploadFile(isEdit, loggedInCompany, loggedInUser) {
    hideAlert();
    var companyName, companyId, formData,oldFileName;

    if (isEdit) {
        companyName = $("#editCompanyName").val();
        companyId = $("#editRecordId").val();
        formData = new FormData($('form')[1]);
        oldFileName = $('#oldFileName').val();
    } else {
        companyName = $("#companyName").data("kendoDropDownList").text();
        companyId = $("#companyName").data("kendoDropDownList").value();
        formData = new FormData($('form')[0]);
        oldFileName = "";
    }

    if ((document.getElementById("file").files.length == 0 && !isEdit) || ((document.getElementById("editFile").files.length == 0 && isEdit))) {
        failureAlert("Please select survey data file to upload");
    }
    else {
        $("#page-spinner").show();
        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIAdmin/UploadSurveyData?memberName=" + companyName + "&companyId=" + companyId + "&isEdit=" + isEdit + "&updatedBy=" + loggedInUser + "&oldFileName=" + oldFileName,
            type: 'Post',
            success: function (response) {
                $("#page-spinner").hide();
                document.getElementById("file").value = "";
                document.getElementById("editFile").value = "";
                $('#oldFileName').val("");

                if (response.Status == 1) {
                    if (localStorage.getItem("nodata") == 'true' && loggedInCompany == companyId)
                        localStorage.setItem("nodata", false);
                    loadMembers();
                    //$("#companyName").data("kendoDropDownList").dataSource.filter({ field: 'text', operator: 'eq', value: companyName });
                    getAvailInstitutionList(getMembersDropDown);
                    successAlert(response.Message);
                    $("#addNewSurvey").removeClass("show").addClass("hide");
                    $("#editSurvey").removeClass("show").addClass("hide");
                    $("#uploadReport").removeClass("show").addClass("hide");
                }
                else {
                    failureAlert(response.Message);
                }
            },
            error: function (data) {
                failureAlert(data.statusText);
                $("#page-spinner").hide();
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }
}

function uploadSummaryReportForMember(loggedInCompany, loggedInUser) {
    hideAlert();
    if (document.getElementById("reportFile").files.length == 0) {
        failureAlert("Please select report summary to upload");
    }        
    else {
        if (document.getElementById("reportFile").files[0].name.split('.')[1].indexOf('xlsx') == -1)
        {
            failureAlert("Report summary should be a excel sheet format");
            return;
        }
        $("#page-spinner").show();
        var companyId, formData;

        companyId = $("#reportRecordId").val();
        formData = new FormData($('form')[2]);

        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIAdmin/UploadSummaryReport?companyId=" + companyId + "&updatedBy=" + loggedInUser + "&oldSummaryReport=" + $("#oldSummaryReportName").val(),
            type: 'Post',
            success: function (response) {
                $("#page-spinner").hide();
                document.getElementById("reportFile").value = "";
                $("#oldSummaryReportName").val("");
                if (response.Status == 1) {
                    if (localStorage.getItem("nodata") == 'true' && loggedInCompany == companyId)
                        localStorage.setItem("nodata", false);
                    loadMembers();
                    successAlert(response.Message);
                    $("#addNewSurvey").removeClass("show").addClass("hide");
                    $("#editSurvey").removeClass("show").addClass("hide");
                    $("#uploadReport").removeClass("show").addClass("hide");
                }
                else {
                    failureAlert(response.Message);
                }
            },
            error: function (data) {
                failureAlert(data.statusText);
                $("#page-spinner").hide();
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    }
}

function getInstitutionList(callback) {
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIAdmin/GetInstitutions",
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                callback(JSON.parse(response.Data));
            }
            else {
                failureAlert(response.Message);
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

function deleteSurveyData(membersList, callback) {
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIAdmin/DeleteSurveyData",//?members=+membersList,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(membersList),
        type: 'DELETE',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                successAlert(response.Message);
                callback();
                getAvailInstitutionList(getMembersDropDown);
            }
            else {
                //$("#errorMsg").text(response.Message).removeClass("hide").addClass("show");
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

function addNewMember(loggedInUser) {
    var memberName = $("#companyName").data("kendoDropDownList").text();
    if (memberName != "") {
        var memberDetails = {
            MemberName: $("#companyName").data("kendoDropDownList").text(),
            CEBMemberId: $("#companyName").data("kendoDropDownList").value(),
            Industry: $("#industry").val(),
            Revenue: $("#revenue").val(),
            Region: $("#region").val(),
        };
        $("#page-spinner").show();
        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIMember/AddMember?createdBy=" + loggedInUser,
            type: 'POST',
            success: function (response) {
                $("#page-spinner").hide();
                if (response.Status == 1) {
                    $("#page-spinner").show();
                    loadInstitutionList();
                    loadMembers();
                    successAlert(response.Message);
                    $("#addNewRecord").removeClass("show").addClass("hide");
                }
                else {
                    failureAlert(response.Message);
                }
            },
            error: function (data) {
                $("#page-spinner").hide();
                failureAlert(data.statusText);
            },
            data: JSON.stringify(memberDetails),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }
    else {
        $("#page-spinner").hide();
        failureAlert("Please select client to add");
    }

}

function updateMemberDetails(loggedinUser) {
    var memberName = $("#editMemberName").val();
    if (memberName != "") {
        var memberDetails = {
            MemberName: $("#editMemberName").val(),
            Industry: $("#editIndustry").val(),
            Revenue: $("#editRevenue").val(),
            Region: $("#editRegion").val(),
            Id: $("#editRecordId").val(),
            TimePeriod: $("#editTimePeriod").val()
        };

        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIMember/UpdateMember?updatedBy=" + loggedinUser,
            type: 'PUT',
            success: function (response) {
                $("#page-spinner").hide();
                if (response.Status == 1) {
                    loadMembers();
                    successAlert(response.Message);
                    $("#editRecord").removeClass("show").addClass("hide");
                }
                else {
                    failureAlert(response.Message);
                }
            },
            error: function (data) {
                failureAlert(data.statusText);
                $("#page-spinner").hide();
            },
            data: JSON.stringify(memberDetails),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }
    else {
        $("#page-spinner").hide();
        failureAlert("Please select client to update");
    }
}

function deleteMembersList(membersList, memberLoggedIn, callback) {
    var memberArr = membersList.toString().split(',');
    var index = $.inArray(memberLoggedIn, memberArr)
    if (index > -1) {
        $("#page-spinner").hide();
        failureAlert('The logged in user belongs to the selected client. So cannot delete the client');
    }
    else {
        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIMember/DeleteMembers?members=" + membersList,
            type: 'DELETE',
            success: function (response) {
                $("#page-spinner").hide();
                if (response.Status == 1) {
                    successAlert(response.Message);
                    callback();
                }
                else {
                    failureAlert(response.Message);
                }
            },
            error: function (data) {
                $("#page-spinner").hide();
                failureAlert(data.statusText);
            }
        });
    }
}

function getMembersList(page, isImport, callback, url) {
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIMember/GetMembers?isImport=" + isImport,
        type: 'Get',
        success: function (response) {
            if (response.Status == 1) {
                if (page == "importFile") {
                    $("#noMemberMessage").removeClass("show").addClass("hide");
                    $("#uploadNewSurvey").removeClass("hide").addClass("show");
                }
                callback(JSON.parse(response.Data));
            }
            else {
                if (page == "importFile") {
                    $("#addNewSurveyBtn").trigger("click");
                    $("#noMemberMessage").removeClass("hide").addClass("show");
                    $("#uploadNewSurvey").removeClass("show").addClass("hide");
                    $(".importServiceItem").removeClass("show").addClass("hide");
                    $("#h2Title").removeClass("show").addClass("hide");
                }
                else {
                    failureAlert(response.Message);
                    $("#addNewRecordBtn").trigger("click");
                    $(".importServiceItem").removeClass("show").addClass("hide");
                }
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

var selectedItems = [];

var importKendoGrid = {
    dataSource: {
        data: {},
        schema: {
            model: {
                fields: {
                    MemberName: { type: "string" },
                    FileName: { type: "string" },
                    SummaryReportPath: { type: "string" },
                    TimePeriod: { type: "number" }
                }
            }
        },
        pageSize: 20
    },
    scrollable: {
        virtual: true
    },

    sortable: true,
    filterable: true,
    pageable: {
        input: true,
        numeric: false
    },
    columns: [
       { template: "<input type='checkbox' class='checkbox' style='cursor:pointer' />", width: "30px" },
       { field: "Id", hidden: true },
       {
           field: "MemberName", title: "Client Name", filterable: {
               cell: {
                   operator: "contains",
                   suggestionOperator: "contains"
               }
           }
       },
       { field: "FileName", title: "File Name", template: '<a href="Download?folderPath=#=FilePath#&fileName=#=FileName#" class="link">#= FileName #</a>', filterable: false, },
       { field: "SummaryReportName", title: "Report Summary", template: '<a href="Download?folderPath=#=SummaryFilePath#&fileName=#=SummaryReportName#" class="link">#= SummaryReportName #</a>', filterable: false },
       { field: "TimePeriod", title: "Recent Time Period", width: "230px", filterable: false },
       { field: "NoOfAvailPeriods", title: "No.Of Time Period", width: "150px", filterable: false, template: "<span class='linkText text-center block'>#=NoOfAvailPeriods#</span>" },
       { title: "Update/Delete", width: "130px", template: '<div><div class="k-button" onClick="uploadReport(this)"  style="min-width:30px;" title="Upload Summary Report"><span class="kendoUploadIcon fa fa-upload pointer"></span></div>&nbsp;<div class="k-button" onClick="surveyUpdate(this)"  style="min-width:30px" title="Edit"><span class="k-icon k-edit pointer"></span></div>&nbsp;<div class="k-button" onClick="surveyDelete(this)" style="min-width:30px"  title="Delete"><span class="k-icon k-delete pointer"></span></div></div>' }
    ],
    noRecords: true,
    messages: {
        noRecords: "There is no data found"
    },
    dataBinding: function () {
        record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
    }
};

var kendoGridObj = {
    dataSource: {
        data: {},
        schema: {
            model: {
                fields: {
                    Id: { type: "number" },
                    MemberName: { type: "string" },
                    Industry: { type: "string" },
                    Revenue: { type: "string" },
                    Region: { type: "string" },
                    TimePeriod: { type: "string" },
                    NoOfAvailPeriods: { type: "number" }
                }
            }
        },
        pageSize: 20
    },
    scrollable: true,
    sortable: true,
    filterable: true,
    pageable: {
        input: true,
        numeric: false
    },
    columns: [
{ field: "Id", hidden: true },
{ field: "NoOfAvailPeriods", hidden: true },
{ template: "<input type='checkbox' class='checkbox' style='cursor:pointer' />", width: "30px" },
{
    field: "MemberName", title: "Client Name", template: "<span class='linkText pointer'  onclick=loadMemberReport(#=Id#,#=NoOfAvailPeriods#)>#=MemberName#</span>", filterable: {
        cell: {
            operator: "contains",
            suggestionOperator: "contains"
        }
    }
},
{ field: "Industry", title: "Industry" },
{ field: "Revenue", title: "Revenue", filterable: false, },
{ field: "Region", title: "Region" },
{ field: "TimePeriod", title: "Time Period", filterable: false, },
{ title: "Edit/Delete", width: "105px", template: '<div><div class="k-button" onClick="memberProfileEdit(this)"  style="min-width:30px"><span class="k-icon k-edit pointer" title="Update survey data"></span></div>&nbsp;&nbsp;&nbsp;&nbsp;<div class="k-button" onClick="memberProfileDelete(this)" style="min-width:30px"><span class="k-icon k-delete pointer" title="Delete survey data"></span></div></div>' }
    ],
    noRecords: true,
    messages: {
        noRecords: "There is no data found"
    },
    dataBinding: function () {
        record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
    }
};

function memberProfileEdit(e) {
    var tr = $(e).closest("tr"); // get the current table row (tr)
    // get the data bound to the current table row
    var item = $("#memberProfileDetails").data("kendoGrid").dataItem(tr);
    if (item.Id != "") {
        // Show hide div
        $("html, body").animate({ scrollTop: 0 }, 1000);
        $("#addNewRecord").fadeOut("slow");
        $("#addNewRecord").removeClass("show").addClass("hide");
        $("#editRecord").fadeIn("slow");
        $("#editRecord").removeClass("hide").addClass("show");
        // populate values in text boxes
        refreshField("editRecord");
        $("#editRecordId").val(item.Id);
        $("#editMemberName").val(item.MemberName);
        $("#editIndustry").val(item.Industry);
        $("#editRegion").val(item.Region);
        $("#editRevenue").val(item.Revenue);
        $("#editFileName").val(item.FileName);
        $("#editTimePeriod").val(item.TimePeriod);

    }
}

function memberProfileDelete(e) {
    var tr = $(e).closest("tr");
    // get the data bound to the current table row
    var item = $("#memberProfileDetails").data("kendoGrid").dataItem(tr);
    deleteMembers(item.Id);
}

function surveyUpdate(e) {
    var tr = $(e).closest("tr"); // get the current table row (tr)
    // get the data bound to the current table row
    var item = $("#memberProfileDetails").data("kendoGrid").dataItem(tr);
    if (item.Id != "") {
        // Show hide div
        $("#addNewSurvey").removeClass("show").addClass("hide");
        $("#uploadReport").removeClass("show").addClass("hide");
        $("html, body").animate({ scrollTop: 0 }, 1000);
        $("#editSurvey input[type='text']").val('');
        $("#editSurvey").fadeIn("slow");
        $("#editSurvey").removeClass("hide").addClass("show");
        // populate values in text boxes
        refreshField("editSurvey");
        $("#editRecordId").val(item.Id);
        $("#editCompanyName").val(item.MemberName);
        //$("#editFile").val(item.FileName);
        $('#oldFileName').val(item.FileName);
    }
}


function surveyDelete(e) {
    var tr = $(e).closest("tr"); // get the current table row (tr)   
    var item = $("#memberProfileDetails").data("kendoGrid").dataItem(tr);
    var data = []
    data.push(
        {
            Id: item.Id,
            FileName: item.FileName,
            MemberName: item.MemberName,
            SummaryReportName: item.SummaryReportName,
            TimePeriod: item.TimePeriod
        }        
    );
    deleteData(data);
    
    //deleteData(item.Id);
}

function uploadReport(e) {
    var tr = $(e).closest("tr"); // get the current table row (tr)
    // get the data bound to the current table row
    var item = $("#memberProfileDetails").data("kendoGrid").dataItem(tr);
    if (item.Id != "") {
        // Show hide div
        $("#addNewSurvey").removeClass("show").addClass("hide");
        $("#editSurvey").removeClass("show").addClass("hide");
        $("html, body").animate({ scrollTop: 0 }, 1000);
        //$("#uploadReport input[type='text']").val('');
        $("#uploadReport").fadeIn("slow");
        $("#uploadReport").removeClass("hide").addClass("show");
        // populate values in text boxes
        refreshField("uploadReport");
        $("#reportRecordId").val(item.Id);
        $("#reportCompanyName").val(item.MemberName);
        $("#oldSummaryReportName").val(item.SummaryReportName)
      //  $("#reportFile").val(item.FileName);
    }
}