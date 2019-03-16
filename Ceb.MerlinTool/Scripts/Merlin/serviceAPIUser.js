function getRequestToServer(page, isImport, callback) {
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIUser/GetAllUsers",
        type: 'Get',
        success: function (response) {
            //$("#page-spinner").hide();
            if (response.Status == 1) {
                $("#noMemberMessage").removeClass("show").addClass("hide");
                $("#uploadNewAccessRight").removeClass("hide").addClass("show");
                callback(JSON.parse(response.Data));
            }
            else {
                $("#addNewAccessBtn").trigger("click");
                $("#noMemberMessage").removeClass("hide").addClass("show");
                //$("#uploadNewAccessRight").removeClass("show").addClass("hide");
                $(".accessRightServiceItem").removeClass("show").addClass("hide");
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function addNewUser(createdBy) {
    var emailId = $("#email").val();
    if (emailId.length == 0) {
        failureAlert("Email cannot be empty.");
    } else {
        if (validateEmail(emailId)) {
            $("#page-spinner").show();
            var user = {
                companyId: $("#companyName").data("kendoDropDownList").value(),
                CEBId: $("#companyName").data("kendoDropDownList").options.dataSource.filter(function (el) { return el.id == $("#companyName").data("kendoDropDownList").value() })[0].displayId,
                email: emailId,
                userCreatedBy: createdBy,
                isAdmin: ($("#isAdmin").prop('checked') == true)
            }
            $.ajax({
                url: config_data.WEBAPI_URL + "/api/v1/APIUser/AddUser",
                type: 'POST',
                success: function (response) {
                    $("#page-spinner").hide();
                    if (response.Status == 1) {
                        $("#page-spinner").show();
                        loadUsers();
                        successAlert(response.Message);
                        $("#addNewAccesRights").removeClass("show").addClass("hide");
                        $(".importServiceItem").removeClass("hide").addClass("show");
                    }
                    else {
                        failureAlert(response.Message);
                    }
                },
                error: function (data) {
                    $("#page-spinner").hide();
                    failureAlert(data.statusText);
                },
                data: JSON.stringify(user),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            });
        } else {
            failureAlert("Not a valid email");
        }
    }
}

function deleteUser(usersList, userLoggedIn, callback) {
    var userArr = usersList.toString().split(',');
    var index = $.inArray(userLoggedIn, userArr)
    if (index > -1) {
        $("#page-spinner").hide();
        failureAlert('Cannot delete the logged in User');
    }
    else {
        $.ajax({
            url: config_data.WEBAPI_URL + "/api/v1/APIUser/Delete?users=" + usersList,
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

var accessDetailsKendoGrid = {
    dataSource: {
        data: {},
        schema: {
            model: {

                fields: {
                    MemberName: { type: "string" },
                    Name: { type: "string" },
                    Email: { type: "string" },
                    IsAdmin: { type: "boolean" }
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
       {
           field: "MemberName", title: "Client Name", filterable: {
               cell: {
                   operator: "contains",
                   suggestionOperator: "contains"
               }
           }
       },
       {
           field: "Name", title: "Name", filterable: {
               cell: {
                   operator: "contains",
                   suggestionOperator: "contains"
               }
           }
       },
       { field: "Email", title: "Email" },
       { field: "IsAdmin", title: "IsAdmin", template: '<input type="checkbox" #= IsAdmin ? \'checked="checked"\' : "" # class="chkbx" />', width: 68, filterable: false, attributes: {style: "text-align: center"} },
       { title: "Delete", width: 60, template: '<div class="k-button" onClick="accessRightsDelete(this)" style="min-width:30px"><span class="k-icon k-delete pointer" title="Delete"></span></div>', attributes: { style: "text-align: center" }
       }      
    ],
    noRecords: true,
    messages: {
        noRecords: "There is no data found"
    },
    dataBinding: function () {
        record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
    }
};


var checkedIds = {};

//on click of the checkbox:
function selectRow() {
    var checkboxElement = this;
    var checked = this.checked,
    row = $(this).closest("tr"),
    grid = $("#accessRightsDetails").data("kendoGrid"),
    dataItem = grid.dataItem(row),
    text = checked ? 'grant' : 'remove';

    confirmationAlert("Are you sure you want to " + text + " admin rights to  " + dataItem.Name + "?", function (flag) {
        if (flag) {
            $("#page-spinner").show();

            checkedIds[dataItem.id] = checked;
            var user = {
                email: dataItem.Email,
                isAdmin: checked
                //,        updatedBy:loggedInUser
            }
            //update user
            $.ajax({
                url: config_data.WEBAPI_URL + "/api/v1/APIUser/Update",
                type: 'POST',
                success: function (response) {
                    $("#page-spinner").hide();
                    if (response.Status == 1) {
                        $("#page-spinner").show();
                        loadUsers();
                        successAlert(response.Message);
                        $("#addNewAccesRights").removeClass("show").addClass("hide");
                        $(".importServiceItem").removeClass("hide").addClass("show");
                    }
                    else {
                        failureAlert(response.Message);
                    }
                },
                error: function (data) {
                    $("#page-spinner").hide();
                    failureAlert(data.statusText);
                },
                data: JSON.stringify(user),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            });
        }
        else {
            checkboxElement.checked = true;
        }
    });
}

function accessRightsDelete(e) {
    var tr = $(e).closest("tr");   
    var item = $("#accessRightsDetails").data("kendoGrid").dataItem(tr);
    deleteUsers(item.Id);
}