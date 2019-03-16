var config_data = {
    'APP_NAME': 'Merlin Online Reporting Tool',
    'APP_VERSION': '1.0.0',
    'WEBAPI_URL': 'http://localhost/Ceb.MerlinTool.WebAPI/',
    'WEB_URL': 'http://localhost/Ceb.MerlinTool/',
    'OverallTitle': "Overall Supplier Ratings",
    'OverallSubTitle': "Average Supplier Ratings",
    'OverallGridTitle': "Overall and Metric Ratings, by Category",
    'OverallGridSubTitle': "Average rating of all the suppliers in the category",
    'CategoryAnalysisTitle': "Supplier Scores, by Category",
    'SupplierAnalysisTitleByMetric': "Supplier Metric Scores",
    'SupplierAnalysisTitleByKPI': "Supplier KPI Scores",
    'SupplierAnalysis': {
        'metric': {
            'XAxisMetric': 'Metric',
            'YAxisMetric': 'Metric Rating'
        },
        'kpi': {
            'XAxisMetric': 'KPI Rating',
            'YAxisMetric': 'KPI'
        }
    },
    'TrendAnalysis': {
        'overall': {
            'XAxisMetric': 'Time Period',
            'YAxisMetric': 'Supplier Score',
            'Title': 'Overall Supplier Trends'
        },
        'metric': {
            'XAxisMetric': 'Time Period',
            'YAxisMetric': 'Metrics Score',
            'Title': 'Metric Trends',
            'Title1': 'Supplier Metric Scores'
        },
        'kpi': {
            'XAxisMetric': 'Time Period',
            'YAxisMetric': 'KPI Score',
            'Title': 'KPI Trends',
            'Title1': 'Supplier KPI Scores'
        }
    }
};

//var applicationConfig = {};
//applicationConfig.breadcrumb = {};
//applicationConfig.breadcrumb.Overall = {};
//applicationConfig.breadcrumb.categoryanalysis = {};
//applicationConfig.breadcrumb.supplierscorecard = {};
//applicationConfig.breadcrumb.perceptiongap = {};
//applicationConfig.breadcrumb.trend = {};

//applicationConfig.categoryanalysis.categoryId = 0;
//applicationConfig.categoryanalysis.metricId = 0;
//applicationConfig.categoryanalysis.kpiId = 0;

//applicationConfig.supplierscorecard.categoryId = 0;
//applicationConfig.supplierscorecard.supplierId = 0;
//applicationConfig.supplierscorecard.demographic = 0;
//applicationConfig.supplierscorecard.demographicOption = 0;

//applicationConfig.perceptiongap.categoryId = 0;
//applicationConfig.perceptiongap.supplierId = 0;

//applicationConfig.trend.categoryId = 0;
//applicationConfig.trend.supplierId = 0;
//applicationConfig.trend.showTimePeriod = 0;
//applicationConfig.trend.selectedPeriod = '';

function getAvailInstitutionList(callback, page) {
    var param = '';
    if (page !== undefined)
        param = '?page=' + page;
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIAdmin/GetAvailableInstitutions" + param,
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

function getLookup(companyId, filter, callback) {
    var ddElement = '', selElement;
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APILookup/GetFilter?companyId=" + companyId + "&filterType=" + filter + "&categoryId=" + $("#ddl-category").val(),
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                selElement = $("#ddl-category");
                selElement.empty();

                var optionsFromServer = JSON.parse(response.Data);

                if (optionsFromServer == null || optionsFromServer.length == 0) {
                    selElement.attr("disabled", true);
                } else {

                    $.each(optionsFromServer, function () {
                        var $option = $("<option />").val(this.Id).text(this.Text);
                        selElement.append($option);
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

//supplier filter
function getSupplier(companyId, supplierId, categoryId, callback) {
    var ddElement = '', selElement;
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APILookup/GetSupplier?companyId=" + companyId + "&categoryId=" + categoryId + "&supplierId=" + supplierId,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                selElement = $("#ddl-supplier");
                selElement.empty();
                var optionsFromServer = JSON.parse(response.Data);
                if (optionsFromServer == null || optionsFromServer.length == 0) {
                    selElement.attr("disabled", true);
                } else {
                    $.each(optionsFromServer, function () {
                        var $option = $("<option />").val(this.Id).text(this.Text);
                        selElement.append($option);
                    });

                    $("#ddl-demographic").removeAttr("disabled");
                    if (localStorage.getItem("categoryId")) //localStorage.getItem("ca_categoryId")
                        $("#ddl-category").val(localStorage.getItem("categoryId"));//localStorage.getItem("ca_categoryId")
                    else
                        $("#ddl-category").val(optionsFromServer[0].CategoryId);
                    //$("#ddl-category").val(optionsFromServer[0].CategoryId);
                    if (supplierId !== "0")
                        $("#ddl-supplier").val(supplierId);
                    else
                        $("#ddl-supplier").val(optionsFromServer[0].Id);
                    callback();
                }
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

//function exportGrid(element, type, excelData, companyDetails, gridColn, page, downloadName) {
//    var containerId = $(element).closest('div[class^="chartBody"]').attr("id");
//    var title = $("#" + containerId + " .chartTitle span").text();
//    var subtitle = $("#" + containerId + " .chartSubTitle").text();

//    switch (containerId) {
//        case 'supplierOverallContainer':
//        case 'supplierMetricContainer':
//        case 'supplierKPIContainer':
//        case 'supplierKPIContainer':
//            title = title + " : " + $("#selected_filter .supplierName").text();
//            subtitle = subtitle + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleDemographic").text();
//            break;
//        case 'timeSeriesOverallContainer':
//        case 'timeSeriesKPITrendsContainer':
//        case 'timeSeriesMetricTrendsContainer':       
//            subtitle = subtitle + $("#selected_filter .supplierName").text().trim() + $("#selected_filter .subTitleCat").text();
//            break;
//        case 'donutOverallContainer':
//        case 'selectedKPITrendsContainer':
//        case 'selectedMetricTrendsContainer':
//            subtitle = subtitle + ", " + $("#selected_filter .supplierName").text() + $("#selected_filter .subTitleCat").text();
//            break;
//        default:
//            break;
//    }

//    //if (containerId == 'supplierOverallContainer' || containerId == 'supplierMetricContainer' || containerId == 'supplierKPIContainer') {
//    //    title = title + " : " + $("#selected_filter .supplierName").text();
//    //    subtitle = subtitle + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleDemographic").text();
//    //}
//    //if (containerId == 'timeSeriesOverallContainer' || containerId == 'donutOverallContainer')
//    //    subtitle = subtitle + $("#selected_filter .supplierName").text().trim() + $("#selected_filter .subTitleCat").text();

//    $(element).find(".fa").removeClass("hide");
//    var details = {
//        chartJson: excelData, title: title, subTitle: subtitle,
//        companyDetails: companyDetails, columnTitles: gridColn, page: page
//    };
//    $.ajax({
//        url: config_data.WEBAPI_URL + "/api/v1/APIOverAll/ExportTable",
//        type: 'Post',
//        success: function (response) {
//            $(element).find(".fa").addClass("hide");
//            $("#page-spinner").hide();
//            if (response !== undefined && response.Status == 1) {
//                window.location.href = config_data.WEB_URL + "Account/Download?path=" + response.Data + '&type=' + type + "&downloadName=" + downloadName;
//            }
//            else {
//                failureAlert(response.Message);
//            }
//        },
//        error: function (data) {
//            $(element).find(".fa").addClass("hide");
//            $("#page-spinner").hide();
//            failureAlert(data);
//        },
//        data: JSON.stringify(details),
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        processData: false
//    });
//}

function exportGrid(element, excelData, companyDetails, gridColn, chartTitle, page) {
    var containerId = $(element).closest('div[class^="chartBody"]').attr("id");
    var title = $("#" + containerId + " .chartTitle span").text();
    var subtitle = $("#" + containerId + " .chartSubTitle").text();

    if (page !== 'Overall View') {
        title = title + " : " + $("#selected_filter .supplierName").text();
        switch (page) {
            case 'Category Analysis':
                title = $("#" + containerId + " .chartTitle span").text();
                subtitle = subtitle + ", by " + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleMet").text() + $("#selected_filter .subTitleKpi").text();
                break;
            case 'Supplier Scorecard':
                subtitle = subtitle + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleDemographic").text();
                break;
            case 'Trend Analysis':
            case 'Perception Gap':
                var tempSubTitle = subtitle + $("#selected_filter .subTitleCat").text();;
                if (subtitle.length == 0 && $("#selected_filter .subTitleCat").text().startsWith(","))
                    tempSubTitle = subtitle + $("#selected_filter .subTitleCat").text().replace(", ", "").trim();
                subtitle = tempSubTitle;
                break;
            default:
                break;
        }
    }

    $(element).find(".fa").removeClass("hide");
    var details = {
        chartJson: excelData, title: title, subTitle: subtitle, companyDetails: companyDetails, columnTitles: gridColn, page: chartTitle
    };
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIOverAll/ExportTable",
        type: 'Post',
        success: function (response) {
            $(element).find(".fa").addClass("hide");
            $("#page-spinner").hide();
            if (response !== undefined && response.Status == 1) {
                if (chartTitle.indexOf("Trend ") > -1)
                    chartTitle = chartTitle.substring("Trend ".length);
                window.location.href = config_data.WEB_URL + "Account/Download?path=" + response.Data + '&type=excel' + "&downloadName=" + chartTitle;
            }
            else {
                failureAlert(response.Message);
            }
        },
        error: function (data) {
            $(element).find(".fa").addClass("hide");
            $("#page-spinner").hide();
            failureAlert(data);
        },
        data: JSON.stringify(details),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        processData: false
    });
}

