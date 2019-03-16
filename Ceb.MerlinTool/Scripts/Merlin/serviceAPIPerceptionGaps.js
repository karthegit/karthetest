var excelSupplierOverall = {};
var excelKPI = {};
var excelMetric = {};
var excelComments = {};
var isCollapsed = true;
var url = config_data.WEBAPI_URL + "/api/v1/APIPerceptionGaps";
//var categoryKPI = {};
var tableColorRange;
var record = 0;

var objHighChartScore = {};
var objScoreByMetric = {};
var objScoreByKPI = {};

var supplierCommentsKendoGrid = {
    pdf: {
        fileName: "SupplierComments.pdf",
        allPages: true,
        proxyURL: config_data.WEB_URL + "Home/Save",
        forceProxy: true,
    },
    dataSource: {
        data: {},
        group: [{ field: "Metric" }],
        sort: { field: "KPI", dir: "asc" },
        schema: {
            model: {

                fields: {
                    Metric: { type: "string" },
                    KPI: { type: "string" },
                    Comments: { type: "string" }
                }
            }
        }
        //,
        //pageSize: 10
    },
    groupable: false,
    sortable: {
        mode: "single",
        allowUnsort: false,
        showIndexes: true,
        initialDirection: "asc"
    }, filterable: false,
    scrollable: false,
    //pageable: {
    //    input: true,
    //    numeric: false
    //},
    columns: [
       {
           field: "Metric", title: "Metric", hidden: true, groupHeaderTemplate: "#= value #"
       },
       {
           field: "KPI", title: "KPI", width: "49%"
       },
       { field: "Comments", title: "Comments", width: "49%" }
    ],
    noRecords: true,
    messages: {
        noRecords: "There is no data found"
    },
    dataBinding: function () {
        record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
    }
};

var supplierScoreByKPIKendo = {
    pdf: {
        fileName: "SupplierScoreByKPI.pdf",
        allPages: true,
        proxyURL: config_data.WEB_URL + "Home/Save",
        forceProxy: true,
    },
    dataSource: {
        data: {},
        group: [{ field: "MetricText" }],
        sort: { field: "KPIText", dir: "asc" },
        schema: {
            model: {

                fields: {
                    MetricText: { type: "string" },
                    KPIText: { type: "string" },
                    Score: { type: "number" },
                    KPIAverage: { type: "number" }
                }
            }
        }
        //,
        //pageSize: 10
    },
    groupable: false,
    sortable: {
        mode: "single",
        allowUnsort: false,
        showIndexes: true,
        initialDirection: "asc"
    }, filterable: false,
    scrollable: false,
    //pageable: {
    //    input: true,
    //    numeric: false
    //},
    columns: [
       {
           field: "MetricText", title: "Metric", hidden: true, groupHeaderTemplate: "#= value #"
       },
       {
           field: "KPIText", title: "KPI"
       },
       {
           field: "Score", title: "KPI Score(Internal)", format: "{0:n2}", width: 140, attributes: { style: "text-align: center" }
       },
       {
           field: "KPIAverage", title: "KPI Score(Supplier)", format: "{0:n2}", width: 140, attributes: { style: "text-align: center" }
       }
    ],
    noRecords: true,
    messages: {
        noRecords: "There is no data found"
    },
    dataBinding: function () {
        record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
    },
    dataBound: function (e) {
        var columns = e.sender.columns;
        var flag = false;
        for (var i = 0; i < columns.length; i++) {
            var property = columns[i].field;
            if (property != "MetricText" && property !== "KPIText") {
                //get the index of the column name
                var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + property + "]").index();

                // iterate the table rows and apply custom row and cell styling
                var rows = e.sender.tbody.children();
                for (var j = 0; j < rows.length; j++) {
                    var row = $(rows[j]);
                    var dataItem = e.sender.dataItem(row);

                    var score = dataItem.get(property);//supplier kpi score
                    var cell = row.children().eq(columnIndex);
                    if (score == 0) {
                        cell.css("color", tableColorRange.colorIfzero);
                        cell.css("background", tableColorRange.colorIfzero);
                    } else {
                        var arrRange = applyColorRange(score);
                        //cell.addClass(applyColorRange(score));
                        if (arrRange !== undefined)
                            cell.css(arrRange[0], arrRange[1]);
                    }
                }
            }
        }

        //for (var j = 0; j < rows.length; j++) {
        //    var row = $(rows[j]);
        //    var dataItem = e.sender.dataItem(row);
        //    if (dataItem.get("Score").toFixed(2) < dataItem.get("KPIAverage").toFixed(2)) //average less than score
        //    {
        //        var columnIndex = this.wrapper.find(".k-grid-header [data-field=KPIText]").index();
        //        var cell = row.children().eq(columnIndex);
        //        cell.css("color", tableColorRange.kpiIndicator);
        //    }
        //}
    }
};

function checkifData(companyId, callback) {
    $("#page-spinner").show();

    $.ajax({
        url: url + "/CheckifDataUploaded?companyId=" + companyId,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            callback(response);
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

function getSupplierScore(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();

    $.ajax({
        url: url + "/GetSupplierScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelSupplierOverall = response.excelExport;
                generateChartGauge(response.supplierScore, 'perceptiongapOverallScore', 'Overall Score(Supplier)', 'Overall Score(Internal)');
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

function getSupplierScoreByMetric(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();

    $.ajax({
        url: url + "/GetSupplierMetricScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelMetric = response.excelExport;
                objScoreByMetric = generateHorizontalBar(response, 'metricPerceptionGap', false, config_data.SupplierAnalysis.metric.XAxisMetric, config_data.SupplierAnalysis.metric.YAxisMetric, true, false, 'Supplier Metric Score', 'Metric Score(Internal)', 'Metric Score(Supplier)');
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


function getSupplierScoreByKPI(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();
    var demographic = $("#ddl-demographicOptions").val();
    var demographicOption = $("#ddl-demographic option:selected").text();

    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIPerceptionGaps/GetSupplierKPIScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier + "&demographic=" + demographic + "&demographicOption=" + demographicOption,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelKPI = response.excelExportData;
                tableColorRange = response.tableRangeColor;
                //categoryKPI = response.categories;
                //scoreByKPI = response.supplierScore;


                if (response.supplierScore.length > 0) {
                    if ($("#grid-supplier-scorebyKPI").data("kendoGrid") != undefined) {
                        var grid = $("#grid-supplier-scorebyKPI").data("kendoGrid");
                        var dataSource = new kendo.data.DataSource({
                            data: response.supplierScore,
                            pageSize: grid.dataSource._pageSize,
                            group: grid.dataSource._group,
                            sort: grid.dataSource._sort
                        });
                        grid.setDataSource(dataSource);
                        grid.dataSource.read();
                    }
                    else {
                        $("#grid-supplier-scorebyKPI").empty();
                        supplierScoreByKPIKendo.dataSource.data = response.supplierScore;
                        $("#grid-supplier-scorebyKPI").kendoGrid(supplierScoreByKPIKendo);
                    }
                    //$("#grid-supplier-scorebyKPI .k-i-collapse").trigger("click");
                    //$('#grid-supplier-scorebyKPI').data('kendoGrid').thead.find('th.k-group-cell').html('<a class="k-icon k-i-expand" href="#" tabindex="-1" onClick="expandAllGroup(\'grid-supplier-scorebyKPI\')"></a>');
                } else {
                    $("#grid-supplier-scorebyKPI").replaceWith("<div id='grid-supplier-scorebyKPI'><span  class=nodatadisplay>No data to display</span></div>");
                }

                //var selElement = $("#ddl-metric");
                //selElement.empty();

                //if (scoreByKPI.length == 0) {
                //    selElement.attr("disabled", true);
                //    $("#supplierKPIFilterBtn").attr("disabled", true);
                //} else {
                //    selElement.removeAttr("disabled");
                //    $("#supplierKPIFilterBtn").removeAttr("disabled");
                //    var result = categoryKPI.map(function (a) { return a.name; });

                //    //selElement.append($("<option />").val(0).text('-- Select Metric --'));
                //    $.each(result, function () {
                //        var $option = $("<option />").val(this).text(this);
                //        selElement.append($option);
                //    });
                //}

                //$('#ddl-metric').selectpicker({
                //    dropdownAlignRight: 'auto'
                //});

                //$('#ddl-metric').selectpicker('refresh');

                //generateChart(scoreByKPI, 'supplierKPI', false, config_data.SupplierAnalysis.kpi.XAxisMetric, config_data.SupplierAnalysis.kpi.YAxisMetric, true, false, categoryKPI);
                //objScoreByKPI = generateHorizontalBar(response, 'supplierKPI', false, config_data.SupplierAnalysis.kpi.XAxisMetric, config_data.SupplierAnalysis.kpi.YAxisMetric, true, false, 'Supplier KPI Score');
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

function applyColorRange(score) {
    if (score.toFixed(2) >= parseFloat(tableColorRange.range1From) && score.toFixed(2) <= parseFloat(tableColorRange.range1To)) {
        return ["background", tableColorRange.range1Color];
    } else if (score.toFixed(2) >= parseFloat(tableColorRange.range2From) && score.toFixed(2) <= parseFloat(tableColorRange.range2To)) {
        return ["background", tableColorRange.range2Color];
    } else if (score.toFixed(2) >= parseFloat(tableColorRange.range3From) && score.toFixed(2) <= parseFloat(tableColorRange.range3To)) {
        return ["background", tableColorRange.range3Color];
    }
    else if (score.toFixed(2) >= parseFloat(tableColorRange.range4From) && score.toFixed(2) <= parseFloat(tableColorRange.range4To)) {
        return ["background", tableColorRange.range4Color];
    }
}

function loadComments(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();
    var demographic = $("#ddl-demographicOptions").val();
    var demographicOption = $("#ddl-demographic option:selected").text();
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APISupplierAnalysis/GetSupplierComments?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier + "&demographic=" + demographic + "&demographicOption=" + demographicOption,
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var responseFromServer = JSON.parse(response.Data);
                companyDetails = responseFromServer.companyDetails;
                excelComments = responseFromServer.supplierComment;

                if (responseFromServer.supplierComment.length > 0) {
                    if ($("#grid-supplier-comments").data("kendoGrid") != undefined) {
                        var grid = $("#grid-supplier-comments").data("kendoGrid");
                        var dataSource = new kendo.data.DataSource({
                            data: responseFromServer.supplierComment,
                            pageSize: grid.dataSource._pageSize,
                            group: grid.dataSource._group,
                            sort: grid.dataSource._sort
                        });
                        grid.setDataSource(dataSource);
                        grid.dataSource.read();
                    }
                    else {
                        $("#grid-supplier-comments").empty();
                        supplierCommentsKendoGrid.dataSource.data = responseFromServer.supplierComment;
                        $("#grid-supplier-comments").kendoGrid(supplierCommentsKendoGrid);
                    }

                    $("#grid-supplier-comments .k-i-collapse").trigger("click");
                    ////if ($("#grid-supplier-comments .k-grid-header tr .k-group-cell").find("span").length === 0) {
                    ////    $("#grid-supplier-comments .k-grid-header tr .k-group-cell").append("<span class='k-icon k-i-expand' tabindex=\"-1\" href=\"#\" id=\"expandAllCollapseAll\" onclick=\"expandAllCollapseAllClick()\"  >&nbsp;&nbsp;&nbsp;</span>");
                    ////}
                    //$("#grid-supplier-comments .k-grid-header tr .k-group-cell").html('<a class="k-icon k-i-expand" href="#" tabindex="-1" onClick="expandAllGroup(\'grid-supplier-comments\')"></a>');
                } else {
                    $("#grid-supplier-comments").replaceWith("<div  class='margin-10 no-margin-top' id='grid-supplier-comments'><span  class=nodatadisplay>No data to display</span></div>");
                }
            }
            else {
                failureAlert(response.Message);
                $("#grid-supplier-comments").replaceWith("<div class='margin-10 no-margin-top' id='grid-supplier-comments'><span  class=nodatadisplay>No data to display</span></div>");
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

//function expandAllGroup(id) {
//    $("#" + id + " .k-grouping-row .k-i-expand").trigger("click");
//    $("#" + id).data('kendoGrid').thead.find('th.k-group-cell').html('<a class="k-icon k-i-collapse" href="#" tabindex="-1" onClick="collapseAllGroup(\'' + id + '\')"></a>');
//    $("html, body").animate({ scrollTop: $("#" + id).parents(".chartBody").offset().top }, 1000);
//}

//function collapseAllGroup(id) {
//    $("#" + id + " .k-grouping-row .k-i-collapse").trigger("click");
//    $("#" + id).data('kendoGrid').thead.find('th.k-group-cell').html('<a class="k-icon k-i-expand" href="#" tabindex="-1" onClick="expandAllGroup(\'' + id + '\')"></a>');
//    $("html, body").animate({ scrollTop: $("#" + id).parents(".chartBody").offset().top }, 1000);
//}

function exportHighchart(element, type, container) {
    var chartObject = {};
    var excelData = {};
    var downloadName = {};
    var page = {};
    if (type == 'excel') {
        switch (container) {
            case 'perceptiongapOverallContainer':
                excelData = excelSupplierOverall;
                page = 'Perception Gap'; downloadName = "Supplier Score";
                break;
            case 'supplierMetricContainer':
                excelData = excelMetric;
                paga =downloadName = 'Perception Supplier Metric Scores';
                break;
            //case 'supplierKPIContainer':
            //    excelData = excelKPI;
            //    page = 'Perception Gap';
            //    downloadName = config_data.SupplierAnalysisTitleByKPI;
            //    break;
            default:
                failureAlert('Problem in exporting...')
                break;
        }
        if (excelData != "[]")
            exportGrid(element, excelData, companyDetails, "", downloadName, page);
        else
            failureAlert("No data to export");
    }
    else
        exportChart(element, type, companyDetails, "PerceptionGaps", 'Perception Gap');
}

function exportGridData(element, type, container) {
    if (container == 'supplierKPIContainer') {
        if (excelKPI != "[]")
            exportGrid(element, excelKPI, companyDetails, "", "Perception Supplier KPI Scores", 'Perception Gap');
        else
            failureAlert("No data to export");
    } else {
        if (excelComments != "[]")
            exportGrid(element, excelComments, companyDetails, "", "Supplier Comments", 'Perception Gap');
        else
            failureAlert("No comments to export");
    }
}

function processKendoForExport(callBack) {
    var exportGrid = supplierCommentsKendoGrid;
    if (exportGrid.dataSource.data.length == undefined)
    {
        failureAlert("No comments to export");
        return;
    }
    record = 0;
    //for (var i = 0; i < exportGrid.columns.length; i++) {
    //    exportGrid.columns[i].locked = false;
    //}
    exportGrid.pageable = false;
    exportGrid.dataSource.pageSize = 15;
    callBack();
}

function processKPIKendoForExport(callBack) {
    var exportGrid = supplierScoreByKPIKendo;
    record = 0;
    if (exportGrid.dataSource.data.length == undefined) {
        failureAlert("No data to export");
        return;
    }
    //for (var i = 0; i < exportGrid.columns.length; i++) {
    //    exportGrid.columns[i].locked = false;
    //}
    exportGrid.pageable = false;
    exportGrid.dataSource.pageSize = 15;
    callBack();
}
