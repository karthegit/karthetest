var excelSupplierOverall = {};
var excelMetric = {};
var record = 0;
var excelKPI = {};
var gridColn = {};
var gridExcelData = '';
var tableColorRange = '';
var allResponseReceived = 0;

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
                    Score: { type: "number" }
                }
            }
        }
    },
    groupable: false,
    filterable: false,
    scrollable: false,
    sortable: {
        mode: "single",
        allowUnsort: false,
        showIndexes: true,
        initialDirection: "asc"
    },
    columns: [
       {
           field: "MetricText", title: "Metric", hidden: true, groupHeaderTemplate: "#= value #"
       },
       {
           field: "KPIText", title: "KPI"
       },
       {
           field: "Score", title: "Supplier Score", format: "{0:n2}", width: 140, attributes: { style: "text-align: center" }
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
                    if (score == 0 || score == null) {
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
    }
};


function getAvailableTimePeriods(companyId, callback) {
    $("#page-spinner").show();
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APITrendAnalysis/GetAvailableTimePeriod?companyId=" + companyId,
        type: 'Get',
        success: function (response) {
            //$("#page-spinner").hide();
            if (response.Status == 1) {
                selElement = $("#ddl-timeperiod");
                selElement.empty();
                var optionsFromServer = JSON.parse(response.Data);
                if (optionsFromServer == null || optionsFromServer.length == 0) {
                    selElement.attr("disabled", true);
                } else {
                    $.each(optionsFromServer, function () {
                        var $option = $("<option />").val(this.Id).text(this.Text);
                        selElement.append($option);
                    });
                    //if ($("#chk_timeperiod").is(":checked")) //for all time periods
                    //{
                    selElement.append($("<option />").val("All").text('-- All Time periods --'));
                    selElement.val("All");
                    $("#ddl-timeperiod").addClass("grayoutSelectedTimeoreiod").attr("disabled", "disabled");;
                    //} else {
                    //    $("#ddl-timeperiod").removeClass("grayoutSelectedTimeoreiod").removeAttr("disabled");                        
                    //}
                }
            }
            else {
                failureAlert(response.Message);
            }

            callback();
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data.statusText);
        }
    });
}

function getOverallScore(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();
    var timePeriod = $("#ddl-timeperiod").val().toLowerCase();
    var showAll = $('#chk_timeperiod').is(":checked");

    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APITrendAnalysis/GetScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier + "&timePeriod=" + (showAll ? "All" : timePeriod),
        type: 'Get',
        success: function (response) {
            allResponseReceived++;
            if (allResponseReceived == 3) {
                $("#page-spinner").hide();
                allResponseReceived = 0;
            }
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelSupplierOverall = response.excelExport;
                if (showAll && timePeriod !== "All") {
                    $("#donutOverallContainer").parent().css("display", "none");
                    $("#timeSeriesOverallContainer").parent().css("display", "");
                    objScore = generateTimeSeries(response, 'timeSeriesOverallSupplier', config_data.TrendAnalysis.overall.XAxisMetric, config_data.TrendAnalysis.overall.YAxisMetric, config_data.TrendAnalysis.overall.Title);

                } else {
                    $("#donutOverallContainer").parent().css("display", "");
                    $("#timeSeriesOverallContainer").parent().css("display", "none");
                    generateChartHalfDonut(response.supplierScore, 'donutOverallSupplier');
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

function getMetricsScore(companyId) {
    //$("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();
    var timePeriod = $("#ddl-timeperiod").val().toLowerCase();;
    var showAll = $('#chk_timeperiod').is(":checked");

    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APITrendAnalysis/GetMetricsScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier + "&timePeriod=" + (showAll ? "All" : timePeriod),
        type: 'Get',
        success: function (response) {
            allResponseReceived++;
            if (allResponseReceived == 3) {
                $("#page-spinner").hide();
                allResponseReceived = 0;
            }
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelMetric = response.excelExport;
                if (showAll && timePeriod !== "All") {
                    $("#selectedMetricTrendsContainer").parent().css("display", "none");
                    $("#timeSeriesMetricTrendsContainer").parent().css("display", "");
                    generateTimeSeries(response, 'timeSeriesMetricTrends', config_data.TrendAnalysis.metric.XAxisMetric, config_data.TrendAnalysis.metric.YAxisMetric, config_data.TrendAnalysis.metric.Title);

                } else {
                    $("#selectedMetricTrendsContainer").parent().css("display", "");
                    $("#timeSeriesMetricTrendsContainer").parent().css("display", "none");
                    generateHorizontalBar(response, 'selectedMetricTrends', false, "Metric", "Metric  Rating", false, false, config_data.TrendAnalysis.metric.Title, 'Metric Rating', '');
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

function getKPIScore(companyId) {
    $("#page-spinner").show();
    var category = $("#ddl-category").val();
    var supplier = $("#ddl-supplier").val();
    var timePeriod = $("#ddl-timeperiod").val().toLowerCase();;
    var showAll = $('#chk_timeperiod').is(":checked");

    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APITrendAnalysis/GetKPIScore?companyId=" + companyId + "&category=" + category + "&supplier=" + supplier + "&timePeriod=" + (showAll ? "All" : timePeriod),
        type: 'Get',
        success: function (response) {
            allResponseReceived++;
            if (allResponseReceived == 3) {
                $("#page-spinner").hide();
                allResponseReceived = 0;
            }
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                //excelKPI = response.excelExport;
                gridExcelData = response.supplierScore;
                tableColorRange = response.tableRangeColor;

                var griddata = JSON.parse(response.supplierScore);
                $(".expandCollapseText").text("Collapse");


                if (showAll && timePeriod !== "All") {
                    $("#selectedKPITrendsContainer").parent().css("display", "none");
                    $("#timeSeriesKPITrendsContainer").parent().css("display", "");
                    if (griddata.length > 0) {
                        gridColn = (response.tableColumnTitles);
                        generateGrid(griddata, gridColn);
                    }
                    else {
                        $("#grid-kpi-trends-all").replaceWith("<div id='grid-kpi-trends-all'><span  class=nodatadisplay>No data to display</span></div>");
                    }

                } else {
                    $("#selectedKPITrendsContainer").parent().css("display", "");
                    $("#timeSeriesKPITrendsContainer").parent().css("display", "none");
                    gridColn = '';
                    var griddata = JSON.parse(response.supplierScore);
                    if (griddata.length > 0) {
                        if ($("#grid-kpi-trends").data("kendoGrid") != undefined) {
                            var grid = $("#grid-kpi-trends").data("kendoGrid");
                            var dataSource = new kendo.data.DataSource({
                                data: griddata,
                                pageSize: grid.dataSource._pageSize,
                                group: grid.dataSource._group,
                                sort: grid.dataSource._sort
                            });
                            grid.setDataSource(dataSource);
                            grid.dataSource.read();
                        }
                        else {
                            $("#grid-kpi-trends").empty();
                            supplierScoreByKPIKendo.dataSource.data = griddata;
                            $("#grid-kpi-trends").kendoGrid(supplierScoreByKPIKendo);
                        }
                    } else {
                        $("#grid-kpi-trends").replaceWith("<div id='grid-kpi-trends'><span  class=nodatadisplay>No data to display</span></div>");
                    }
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

function generateGrid(response, columnTitles) {
    var model = generateModel(response);
    var columns = [];

    var tableIndex = [{
        field: "MetricText", title: "Metric", hidden: true, groupHeaderTemplate: "#= value #"
    },
       {
           field: "KPIText", title: "KPI"
       }];

    var dynamicGridArray = generateColumns(Object.keys(response[0]), columnTitles);
    columns = tableIndex.concat(dynamicGridArray);

    grid = {
        pdf: {
            fileName: "KPIscore.pdf",
            allPages: true,
            proxyURL: config_data.WEB_URL + "Home/Save",
            forceProxy: true,
        },
        dataSource: {
            data: response,
            group: [{ field: "MetricText" }],
            sort: { field: "KPIText", dir: "asc" },
            schema: {
                model: {
                    fields: model
                }
            }
        },
        groupable: false,
        filterable: false,
        sortable : {
            mode: "single",
            allowUnsort: false,
            showIndexes: true,
            initialDirection: "asc"
        },
        scrollable: false,
        columns: columns,
        noRecords: true,
        messages: {
            noRecords: "There is no data found"
        },
        excelExport: function (e) {
            record = 0;
            var sheet = e.workbook.sheets[0];
            var template = kendo.template(this.columns[0].template);

            for (var i = 1; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];

                var dataItem = {
                    Id: row.cells[0].value
                };

                row.cells[0].value = template(dataItem);
            }
        },
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    }

    // $("#grid-ratings").kendoGrid(grid);  

    $("#grid-kpi-trends-all").kendoGrid($.extend({
        dataBound: function (e) {

            for (property in columnTitles) {
                if (property != "MetricText" && property !== "KPIText") {
                    // get the index of the column name
                    var columns = e.sender.columns;
                    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + property + "]").index();

                    // iterate the table rows and apply custom row and cell styling
                    var rows = e.sender.tbody.children();
                    for (var j = 0; j < rows.length; j++) {
                        var row = $(rows[j]);
                        var dataItem = e.sender.dataItem(row);

                        var score = dataItem.get(property);

                        var cell = row.children().eq(columnIndex);
                        //cell.addClass(applyColorRange(score));
                        //cell.css(applyColorRange(score)[0], applyColorRange(score)[1])
                        if (score == 0 || score == null) {
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
        }
    }, grid));
}

function generateColumns(columnNames, columnTitles) {

    var index = columnNames.indexOf("MetricText");
    if (index > -1) {
        columnNames.splice(index, 1);
    }

    index = columnNames.indexOf("KPIText");
    if (index > -1) {
        columnNames.splice(index, 1);
    }

    return columnNames.map(function (name) {
        return {
            field: name, format: "{0:n2}", width: 140, attributes: { style: "text-align: center" }, title: columnTitles[name]
        };

    });
}

function generateModel(response) {

    var sampleDataItem = response[0];
    var model = {};
    for (var property in sampleDataItem) {
        {
            if (property == 'MetricText' || property == 'KPIText')
                model[property] = { type: "string" };
            else
                model[property] = { type: "number" };
        }
    }
    return model;
}

function processKendoForExport(callBack) {
    var exportGrid = grid;
    record = 0;
    for (var i = 0; i < exportGrid.columns.length; i++) {
        exportGrid.columns[i].locked = false;
    }
    exportGrid.pageable = false;
    exportGrid.dataSource.pageSize = 15;
    $("#grid-kpi-trends-all-export").kendoGrid($.extend({
        dataBound: function (e) {

            for (property in gridColn) {
                if (property != "Category" && property !== "SNo" && property !== "fk_CategoryId") {
                    // get the index of the column name
                    var columns = e.sender.columns;
                    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + property + "]").index();

                    // iterate the table rows and apply custom row and cell styling
                    var rows = e.sender.tbody.children();
                    for (var j = 0; j < rows.length; j++) {
                        var row = $(rows[j]);
                        var dataItem = e.sender.dataItem(row);

                        var score = dataItem.get(property);

                        var cell = row.children().eq(columnIndex);
                        if (score == 0 || score == null) {
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
        }
    }, exportGrid));
    callBack();
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

function exportHighchart(element, type, container) {
    var chartObject = {};
    var excelData = {};
    var downloadName = {};
    var page = '';
    switch (container) {
        case 'timeSeriesOverallContainer':
        case 'donutOverallContainer':
            excelData = excelSupplierOverall;
            downloadName = page = config_data.TrendAnalysis.overall.Title;
            break;
        case 'timeSeriesMetricTrendsContainer':
            excelData = excelMetric;
            downloadName = page = config_data.TrendAnalysis.metric.Title;
            break;
        case 'selectedMetricTrendsContainer':
            excelData = excelMetric;
            downloadName = config_data.TrendAnalysis.metric.Title1;
            page = "Trend " + config_data.TrendAnalysis.metric.Title1;
            break;
        case 'timeSeriesKPITrendsContainer':
            excelData = excelKPI;
            downloadName = page = config_data.TrendAnalysis.kpi.Title;
            break;
        default:
            failureAlert('Problem in exporting...')
            break;
    }
    if (excelData == "[]")
        failureAlert("No data to export");
    else {
        if (type == 'excel')
            exportGrid(element, excelData, companyDetails, "", page, 'Trend Analysis');
        else
            exportChart(element, type, companyDetails, downloadName, 'Trend Analysis');
    }
}

function exportGridData(element, type) {
    var title;
    if (gridExcelData != "[]") {
        var containerId = $(element).closest('div[class^="chartBody"]').attr("id");

        if (containerId == 'timeSeriesKPITrendsContainer')
            title = config_data.TrendAnalysis.kpi.Title;
        else
            title = "Trend " + config_data.TrendAnalysis.kpi.Title1;

        exportGrid(element, gridExcelData, companyDetails, gridColn, title, 'Trend Analysis');

        //else
        //    exportGrid(element, type, gridExcelData, companyDetails, "", "Trend " + config_data.TrendAnalysis.kpi.Title1, config_data.TrendAnalysis.kpi.Title1);

    }
    else
        failureAlert("No data to export");
}

function processKPIKendoForExport(callBack) {
    var exportGrid = supplierScoreByKPIKendo;
    record = 0;
    exportGrid.pageable = false;
    exportGrid.dataSource.pageSize = 15;
    callBack();
}