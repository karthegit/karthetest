var excelData = {};
var companyDetails = '';
var objHighchartBar = {};
//Load filters metric and KPI
function getFilterByCategory(companyId, filter, selCategory, selMetric, callback) {
    var ddElement = '', selElement;
    var metric = selMetric.length > 0 ? "&metricId=" + selMetric : '';
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APILookup/GetFilter?companyId=" + companyId + "&filterType=" + filter + "&categoryId=" + selCategory + metric,
        type: 'Get',
        success: function (response) {
            if (response.Status == 1) {
                if (filter == 'metric') {

                    ddElement = "ddl-kpi";
                    selElement = $("#" + ddElement);
                    selElement.empty();
                    selElement.append($("<option />").val(0).text('-- Select KPI --'));
                    selElement.attr("disabled", true);

                    ddElement = "ddl-metric";
                    selElement = $("#" + ddElement);
                    selElement.empty();
                    selElement.append($("<option />").val(0).text('-- Select Metric --'));

                }
                else if (filter == 'kpi') {
                    ddElement = "ddl-kpi";
                    selElement = $("#" + ddElement);
                    selElement.empty();
                    selElement.append($("<option />").val(0).text('-- Select KPI --'));
                    selElement.removeAttr("disabled");
                }
                var optionsFromServer = JSON.parse(response.Data);

                if (optionsFromServer == null || optionsFromServer.length == 0) {
                    selElement.attr("disabled", true);
                } else {
                    $.each(optionsFromServer, function () {
                        selElement.append($("<option />").val(this.Id).text(this.Text));
                    });
                }
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

function getSupplierScoreByCategory(companyId) {
    var category = $("#ddl-category").val();
    var metric = $("#ddl-metric").val() == '-- Select Metric --' ? 0 : $("#ddl-metric").val();
    var kpi = $("#ddl-kpi").val() == '-- Select KPI --' ? 0 : $("#ddl-kpi").val();

    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIMemberCategoryAnalysis/GetSupplierScore?companyId=" + companyId + "&category=" + category + "&metric=" + metric + "&kpi=" + kpi,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelData = response.excelExport;
                //generateChart(response.supplierScore, 'categoryAnanlysis', true, 'Supplier Name', 'Supplier Rating');
                objHighchartBar = generateHorizontalBar(response, 'categoryAnanlysis', true, 'Supplier Name', 'Supplier Rating', true,false, 'Supplier Score, by category', 'Supplier Score, by category', 'Category Average');
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

function exportChartCategoryAnalysis(element, type) {
    if (type == 'excel')
        exportGrid(element, excelData, companyDetails, "", config_data.CategoryAnalysisTitle, 'Category Analysis');
    else
        exportChart(element, type, companyDetails, config_data.CategoryAnalysisTitle, 'Category Analysis');
}
