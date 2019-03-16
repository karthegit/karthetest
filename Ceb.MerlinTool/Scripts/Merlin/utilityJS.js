var chartSeriesData = {};
var isSortedAsc = {};
var excelData = {};
var highchartObjects = {};

function generateHorizontalBar(chartData, renderTo, isbarClickable, xAxistitle, YAxisTitle, isCombo, isTop10, tooltipText, series1, series2) {


    var scoreList = [];
    var supplierList = [];
    var tempChartData = {};

    chartSeriesData[renderTo] = $.extend(true, [], chartData);
    tempChartData = chartData.supplierScore;

    $.each(tempChartData, function (key, value) {
        scoreList.push(value.score);
        supplierList.push(value.axistext);
    });
    var highchartBarTemp = $.extend(true, {}, highchartBar);
    if (isTop10) {
        highchartBarTemp.series[0].data = scoreList.slice(0, 10);
        highchartBarTemp.xAxis.categories = {};
        highchartBarTemp.xAxis.categories = supplierList.slice(0, 10);

    } else {
        highchartBarTemp.series[0].data = scoreList;//.slice(0, 10);
        highchartBarTemp.xAxis.categories = {};
        highchartBarTemp.xAxis.categories = supplierList;//.slice(0, 10);
    }

    if (isbarClickable) {
        highchartBarTemp.plotOptions = {};
        highchartBarTemp.plotOptions.series = {};
        highchartBarTemp.plotOptions.series.cursor = "pointer";
        highchartBarTemp.plotOptions.series.point = {};
        highchartBarTemp.plotOptions.series.point.events = {};
        highchartBarTemp.plotOptions.series.point.events.click = function () {
            navigateToSupplier(this.id);
        };
    }

    if (series1 == undefined && series2 == undefined) {
        //tooltip
        highchartBarTemp.tooltip = {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: tooltipText + ' (Weighted): <b>{point.y:.2f}</b><br />No of Respondents: {point.n}<br />'
        };
    } else {
        if (renderTo == 'supplierMetric' || renderTo == 'categoryAnanlysis') {
            highchartBarTemp.tooltip = {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: tooltipText + ' (Weighted): <b>{point.y:.2f}</b><br />No of Respondents: {point.n}<br />'
            };
        } else {
            highchartBarTemp.tooltip = {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: series1 + ' (Weighted): <b>{point.y:.2f}</b>'
            };
        }
    }

    if (isCombo !== undefined && isCombo == true) {
        var benchmark = [];
        $.each(tempChartData, function (key, value) {
            benchmark.push(value.score.benchmarkScore);
        });

        highchartBarTemp.legend.enabled = true;
        highchartBarTemp.series[0].name = series1;
        highchartBarTemp.series[1].tooltip = {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: series1 + ' (Weighted): <b>{point.y:.2f}</b>'
        };
        highchartBarTemp.series[1].name = series2;
        highchartBarTemp.series[1].data = benchmark;
        highchartBarTemp.series[1].visible = true;
        highchartBarTemp.series[1].tooltip = {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: series2 + ' (Weighted): <b>{point.y:.2f}</b>'
        };
        highchartBarTemp.scrollbar = {};
        if (chartData.categories !== undefined) {
            highchartBarTemp.xAxis.categories = chartData.categories;
            if (highchartBarTemp.series[0].data.length <= 15) {
                highchartBarTemp.xAxis.max = null;
                highchartBarTemp.xAxis.scrollbar = null;
            } else {
                highchartBarTemp.xAxis.max = {};
                highchartBarTemp.xAxis.max = 10;
                highchartBarTemp.xAxis.scrollbar = {};
                highchartBarTemp.xAxis.scrollbar.enabled = true;
            }
        }
        if (renderTo == "categoryAnanlysis") {
            highchartBarTemp.series[1].type = 'spline';
            highchartBarTemp.series[1].color = '#0a3f6b';
            if (highchartBarTemp.series[0].data.length <= 10) {
                highchartBarTemp.xAxis.max = null;
                highchartBarTemp.xAxis.scrollbar = null;
            } else {
                highchartBarTemp.xAxis.max = {};
                highchartBarTemp.xAxis.max = 10;
                highchartBarTemp.xAxis.scrollbar = {};
                highchartBarTemp.xAxis.scrollbar.enabled = true;
            }
            if (highchartBarTemp.series[0].data.length == 1) {
                highchartBarTemp.series[1].marker.enabled = true;
                highchartBarTemp.series[1].marker.radius = 5;

            } else {
                highchartBarTemp.series[1].marker.enabled = false;
                highchartBarTemp.series[1].marker.radius = null;

            }
        }
    }

    highchartBarTemp.chart.renderTo = renderTo;
    highchartBarTemp.xAxis.title.text = xAxistitle;
    highchartBarTemp.yAxis.title.text = YAxisTitle;


    if (tempChartData.length == 0) {
        highchartBarTemp.yAxis.min = highchartBarTemp.yAxis.max = highchartBarTemp.yAxis.title = highchartBarTemp.xAxis.title = null;
        highchartBarTemp.legend.enabled = false;
        highchartObjects[renderTo] = $.extend(true, {}, highchartBarTemp);
        isSortedAsc[renderTo] = false;

        var objChart = new Highcharts.Chart(highchartBarTemp);
        return $.extend(true, {}, highchartBarTemp);
    }

    if (renderTo == 'metricPerceptionGap') {
        highchartBarTemp.series[0].dataLabels.format = null;
        highchartBarTemp.series[0].dataLabels.formatter = function () {
            if (this.y > 0)
                return Highcharts.numberFormat(this.y, 2);
        };
    }

    var objChart = new Highcharts.Chart(highchartBarTemp);
    highchartObjects[renderTo] = $.extend(true, {}, highchartBarTemp);
    isSortedAsc[renderTo] = false;
    return $.extend(true, {}, highchartBarTemp);
}

function generateChartGauge(chartData, renderTo, seriesName1, seriesName2) {
    var tempData = chartSeriesData[renderTo] = chartData[0];
    var highchartGaugeTemp = $.extend(true, {}, highchartGauge);
    highchartGaugeTemp.pane.background[0].backgroundColor = Highcharts.Color("#c4c4c4").setOpacity(0.2).get();
    highchartGaugeTemp.pane.background[1].backgroundColor = Highcharts.Color("#7fd6f7").setOpacity(0.7).get();
    highchartGaugeTemp.series[0].data[0].y = [tempData.Score1]; //overall
    highchartGaugeTemp.series[1].data[0].y = [tempData.Score2];//internal

    if (seriesName1 !== undefined && seriesName2 !== undefined) {
        highchartGaugeTemp.series[1].name = seriesName1;
        highchartGaugeTemp.series[0].name = seriesName2;
    }

    if (tempData.Score1 == null || tempData.Score2 == null) {
        highchartGaugeTemp.series = null;
        highchartGaugeTemp.pane.background = null;
    }

    Highcharts.chart(renderTo, highchartGaugeTemp);
    highchartObjects[renderTo] = $.extend(true, {}, highchartGaugeTemp);
}

function generateChartHalfDonut(chartData, renderTo) {
    var tempData = chartSeriesData[renderTo] = chartData;
    var tempDonut = $.extend(true, {}, highchartHalfDonut);
    //highchartGauge.pane.background[0].backgroundColor = Highcharts.Color("#7fd6f7").setOpacity(0.3).get();
    //highchartGauge.pane.background[1].backgroundColor = Highcharts.Color("#ffa500").setOpacity(0.3).get();
    tempDonut.pane.background[0].backgroundColor = Highcharts.Color("#c4c4c4").setOpacity(0.2).get();
    //highchartGauge.pane.background[1].backgroundColor = Highcharts.Color("#7fd6f7").setOpacity(0.7).get();

    if (tempData.length > 0)
        tempDonut.series[0].data[0].y = tempData[0].score;//[tempData.score];
    else {
        tempDonut.series = null;
        tempDonut.pane.background = null;
    }
    //highchartGauge.chart.renderTo = renderTo;       
    Highcharts.chart(renderTo, tempDonut);
    highchartObjects[renderTo] = $.extend(true, {}, tempDonut);
}

function generateTimeSeries(chartData, renderTo, xAxistitle, YAxisTitle, tooltipText) {

    chartSeriesData[renderTo] = $.extend(true, [], chartData);
    var highchartTimeSeriesTemp = $.extend(true, {}, timeseriesChart);
    highchartTimeSeriesTemp.chart.renderTo = renderTo;
    highchartTimeSeriesTemp.xAxis.title.text = xAxistitle;
    highchartTimeSeriesTemp.yAxis.title.text = YAxisTitle;

    if (chartData.series.length == 0) {
        highchartTimeSeriesTemp.yAxis.min = highchartTimeSeriesTemp.yAxis.max = highchartTimeSeriesTemp.yAxis.title = highchartTimeSeriesTemp.xAxis.title = null;
        highchartTimeSeriesTemp.legend.enabled = false;
        highchartObjects[renderTo] = $.extend(true, {}, highchartTimeSeriesTemp);
        isSortedAsc[renderTo] = false;
        var objChart = new Highcharts.Chart(highchartTimeSeriesTemp);
        return $.extend(true, {
        }, highchartTimeSeriesTemp);
    }

    highchartTimeSeriesTemp.series = chartData.series;
    highchartTimeSeriesTemp.xAxis.categories = {};
    highchartTimeSeriesTemp.xAxis.categories = chartData.categories;

    //tooltip
    highchartTimeSeriesTemp.tooltip = {
        headerFormat: '<b>{point.x}</b><br/> ',
        pointFormat: '{series.name} (Weighted): <b>{point.y:.2f}</b><br />'
    };

    if (chartData.series.length > 1)
        highchartTimeSeriesTemp.legend.enabled = true;

    if (renderTo == 'timeSeriesOverallSupplier') {
        highchartTimeSeriesTemp.chart.marginBottom = {};
        highchartTimeSeriesTemp.chart.marginBottom = 115;
    }

    if (renderTo == 'timeSeriesMetricTrends') {
        highchartTimeSeriesTemp.legend = {
            "enabled": true,
            itemStyle: {
                "fontSize": "11px",
                "fontFamily": "Arial, Helvetica, sans-serif"
            },
            align: 'left'
        };
        //highchartTimeSeriesTemp.chart.marginBottom = {};
        //highchartTimeSeriesTemp.chart.marginBottom = 120;
    }

    var objChart = new Highcharts.Chart(highchartTimeSeriesTemp);

    if (renderTo == 'timeSeriesOverallSupplier') {
        highchartTimeSeriesTemp.chart.marginBottom = null;
    }

    if (renderTo == 'timeSeriesMetricTrends') {
        highchartTimeSeriesTemp.legend = {};
        highchartTimeSeriesTemp.legend.enabled = true;
    }

    highchartObjects[renderTo] = $.extend(true, {}, highchartTimeSeriesTemp);
    isSortedAsc[renderTo] = false;
    return $.extend(true, {}, highchartTimeSeriesTemp);
}

function sort(container, isTop10, isCombo, isFromUI) {
    var scoreList = [];
    var supplierList = [];
    var chartData = chartSeriesData[container];
    var tempData = chartData.supplierScore;
    var sortOrder = !isSortedAsc[container];

    if ((isSortedAsc[container] && isFromUI) || (!isFromUI && sortOrder)) { // high to low
        tempData.sort(function (a, b) {
            return parseFloat(b.score.y) - parseFloat(a.score.y);
        });
    }
    else {//by default low to heigh
        tempData.sort(function (a, b) {
            return parseFloat(a.score.y) - parseFloat(b.score.y);
        });
    }
    $.each(tempData, function (key, value) {
        scoreList.push(value.score);
        supplierList.push(value.axistext);
    });
    var highchartBarTemp = $.extend(true, {}, highchartObjects[container]);
    if (isTop10) {
        highchartBarTemp.series[0].data = scoreList.slice(0, 10);
        highchartBarTemp.xAxis.categories = {};
        highchartBarTemp.xAxis.categories = supplierList.slice(0, 10);
    }
    else {
        highchartBarTemp.series[0].data = scoreList;
        highchartBarTemp.xAxis.categories = {};
        highchartBarTemp.xAxis.categories = supplierList;
    }

    if (isCombo !== undefined && isCombo == true) {
        var benchmark = [];

        //if (isSortedAsc[container]) {
        //    tempData.sort(function (a, b) {
        //        return parseFloat(b.score.benchmarkScore) - parseFloat(a.score.benchmarkScore);
        //    });
        //}
        //else {
        //    tempData.sort(function (a, b) {
        //        return parseFloat(a.score.benchmarkScore) - parseFloat(b.score.benchmarkScore);
        //    });
        //}

        $.each(scoreList, function (key, value) {
            benchmark.push(value.benchmarkScore);
        });

        highchartBarTemp.legend.enabled = true;
        highchartBarTemp.series[0].name = "Supplier Rating";
        highchartBarTemp.series[1].data = benchmark;
        highchartBarTemp.scrollbar = {};
        if (chartData.categories !== undefined) {
            highchartBarTemp.xAxis.categories = chartData.categories;
            if (highchartBarTemp.series[0].data.length <= 15) {
                highchartBarTemp.xAxis.max = null;
                highchartBarTemp.xAxis.scrollbar = null;
            } else {
                highchartBarTemp.xAxis.max = {};
                highchartBarTemp.xAxis.max = 10;
                highchartBarTemp.xAxis.scrollbar = {};
                highchartBarTemp.xAxis.scrollbar.enabled = true;
            }
        }
    }
    if (isFromUI) {
        //Highcharts.chart('overallScore', highchartBarTemp);
        $("#" + container).highcharts(highchartBarTemp);
        isSortedAsc[container] = !isSortedAsc[container];
        highchartObjects[container] = $.extend(true, {}, highchartBarTemp);

    }
    return highchartBarTemp;
}

function zoom(container, isCombo) {
    var parentDiv = $("#" + container).parent().attr("id");
    var highchartBarTemp;
    if (container == 'supplierMetric' || container == "metricPerceptionGap" || container == 'timeSeriesOverallSupplier' || container == 'timeSeriesMetricTrends'
        || container == 'selectedMetricTrends' || container == 'timeSeriesKPITrends' || container == "categoryAnanlysis")
        highchartBarTemp = highchartObjects[container]
    else
        highchartBarTemp = sort(container, false, isCombo, false);
    highchartBarTemp.xAxis.max = {};
    highchartBarTemp.xAxis.max = 15;
    highchartBarTemp.xAxis.scrollbar = {
    };
    highchartBarTemp.xAxis.scrollbar.enabled = true;
    $("#chartPopupModal .chartTitle .message").text($("#" + parentDiv + " .chartTitle span").text());
    $("#chartPopupModal .chartSubTitle").text($("#" + parentDiv + " #chartSubTitle").text());
    if (highchartBarTemp.series[0].data.length <= 15) {
        highchartBarTemp.xAxis.max = null;
        highchartBarTemp.xAxis.scrollbar = null;
    }
    $("#popupChart").css("width", "99%");
    $("#popupChart").highcharts(highchartBarTemp);
    $("#popupChart").css("width", "100%"); $("#chartPopupModal").modal('show');
}

/* Print */
function dashboardprint(divId, page) {
    var title = $("#" + divId + " .chartTitle span").text();
    var subtitle = $("#" + divId + " .chartSubTitle").text();
    var tempTitle, tempsubTitle;

    if (page !== 'Overall View') {
        tempTitle = title + " : " + $("#selected_filter .supplierName").text();
        switch (page) {
            case 'Category Analysis':
                tempTitle = $("#" + divId + " .chartTitle span").text();
                tempsubTitle = subtitle + ", by " + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleMet").text() + $("#selected_filter .subTitleKpi").text();
                break;
            case 'Supplier Scorecard':
                tempsubTitle = subtitle + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleDemographic").text();
                break;
            case 'Trend Analysis':
            case 'Perception Gap':
                var temp = subtitle + $("#selected_filter .subTitleCat").text();;
                if (subtitle.length == 0 && $("#selected_filter .subTitleCat").text().startsWith(","))
                    temp = subtitle + $("#selected_filter .subTitleCat").text().replace(", ", "").trim();
                tempsubTitle = temp;
                break;
            default:
                break;
        }
    }

    $("#" + divId + " .chartTitle span").text(tempTitle)
    $("#" + divId + " .chartSubTitle").text(tempsubTitle);

    switch (divId) {
        case 'supplierKPIContainer':
            $("#grid-supplier-scorebyKPI .k-i-expand").trigger("click");
            $("#grid-supplier-scorebyKPI").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            break;
        case 'selectedKPITrendsContainer':
            $("#grid-kpi-trends .k-i-expand").trigger("click");
            $("#grid-kpi-trends").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            break;
        case 'timeSeriesKPITrendsContainer':
            $("#grid-kpi-trends-all .k-i-expand").trigger("click");
            $("#grid-kpi-trends-all").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            break;
        case "supplierCommentsContainer":
            $("#grid-supplier-comments .k-i-expand").trigger("click");
            $("#grid-supplier-comments").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            break;
        default:
            break;
    }

    var pathName = window.location.protocol + '//' + window.location.host + '/' + window.location.pathname.split('/')[1];
    var bootstrapPath = pathName + '/Content/bootstrap.min.css';
    var kendoCommon = pathName + '/Content/kendo.common.min.css';
    var kendoSilver = pathName + '/Content/kendo.silver.min.css';
    var stylePath = pathName + '/Content/site.css';
    //if (divId == 'timeSeriesMetricTrendsContainer') {
    //    //var tempJSONData = $.extend(true, {}, highchartObjects[$("#" + divId).find(".hchart").attr("id")]);
    //    var chart = $('#' + $("#" + divId).find(".hchart").attr("id")).highcharts();
    //    chart.update({
    //        legend: {
    //            enabled: true,
    //            maxHeight: null,
    //            y: 25
    //        }
    //    });
    //}
    var contents = document.getElementById(divId).innerHTML;
    var dashboardFrame = document.createElement('iframe');
    dashboardFrame.name = "dashboardFrame";
    dashboardFrame.style.position = "absolute";
    dashboardFrame.style.bottom = "0px";
    dashboardFrame.style.height = "0px";
    dashboardFrame.style.width = "0px";
    dashboardFrame.style.marginLeft = "-20px";
    document.body.appendChild(dashboardFrame);
    var frameDoc = dashboardFrame.contentWindow ? dashboardFrame.contentWindow : dashboardFrame.contentDocument.document ? dashboardFrame.contentDocument.document : dashboardFrame.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><title></title>');
    frameDoc.document.write(' <link rel="stylesheet" href="' + bootstrapPath + '">');
    //  frameDoc.document.write('<link rel="stylesheet"  href="' + kendoCommon + '">');
    //  frameDoc.document.write('<link rel="stylesheet"  href="' + kendoSilver + '">');
    frameDoc.document.write('<link rel="stylesheet"  href="' + stylePath + '">');
    frameDoc.document.write('<style>.chartIcon,.open>.dropdown-menu{display:none;} </style>');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["dashboardFrame"].focus();
        window.frames["dashboardFrame"].print();
        document.body.removeChild(dashboardFrame);
    }, 1000);

    $("#" + divId + " .chartTitle span").text(title);
    $("#" + divId + " .chartSubTitle").text(subtitle);

    switch (divId) {
        case "supplierCommentsContainer":
            $("#grid-supplier-comments .k-i-collapse").trigger("click");
            $("#grid-supplier-comments").parents(".chartBody").find(".expandCollapseText").text("Expand");
            break;
            //case "timeSeriesMetricTrendsContainer":
            //    var chart = $('#' + $("#" + divId).find(".hchart").attr("id")).highcharts();
            //    chart.update({
            //        legend: {
            //            enabled: true,
            //            maxHeight: 39
            //        }
            //    });
            //    break;
        default:
            break;
    }
}

function exportChart(element, type, companyDetails, downloadName, page) {
    var tempJSONData = $.extend(true, {}, highchartObjects[$(element).closest('div[class^="chartBody"]').find(".hchart").attr("id")]);

    if (tempJSONData.series == null)
    {
        $(element).find(".fa").addClass("hide");
        $("#page-spinner").hide();
        failureAlert('No data to export');
        return;
    }

    if (tempJSONData.series != null) {
        if (tempJSONData.series[0].data == null || tempJSONData.series[0].data.length == 0) {
            $(element).find(".fa").addClass("hide");
            $("#page-spinner").hide();
            failureAlert('No data to export');
            return;
        }
    }

    var containerId = $(element).closest('div[class^="chartBody"]').attr("id");
    $(element).find(".fa").removeClass("hide");
    var title = $("#" + containerId + " .chartTitle span").text();
    var subtitle = $("#" + containerId + " .chartSubTitle").text();

    if (tempJSONData.chart.type == "solidgauge") {
        tempJSONData.pane.size = "130%";
    }


    if (page !== 'Overall View') {
        title = title + " : " + $("#selected_filter .supplierName").text();
        switch (page) {
            case 'Category Analysis':
                title = $("#" + containerId + " .chartTitle span").text();
                subtitle = subtitle + ", by " + $("#selected_filter .subTitleCat").text() + $("#selected_filter .subTitleMet").text() + $("#selected_filter .subTitleKpi").text();
                tempJSONData.xAxis.max = null;
                tempJSONData.xAxis.scrollbar = null;
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
                if (tempJSONData.chart.type == 'bar') {
                    tempJSONData.series[0].dataLabels.formatter = "fmtFUNC(){  if (this.y > 0)  return Highcharts.numberFormat(this.y, 2);  }fmtFUNC";                    
                }
                break;
            default:
                break;
        }
    }



    var details = {
        chartJson: tempJSONData, title: title, subTitle: subtitle, exportType: type, companyDetails: companyDetails, page: page
    };
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIOverAll/ExportChart",
        type: 'Post',
        success: function (response) {
            $(element).find(".fa").addClass("hide");
            $("#page-spinner").hide();
            if (response !== undefined && response.Status == 1) {
                window.location.href = config_data.WEB_URL + "Account/Download?path=" + response.Data + '&type=' + type + "&downloadName=" + downloadName;
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

function dashboardprintAll(page) {

    switch (page) {
        case "Supplier Scorecard":
        case "Perception Gap":
            $("#grid-supplier-comments .k-i-expand").trigger("click");
            $("#grid-supplier-scorebyKPI .k-i-expand").trigger("click");
            $("#grid-supplier-comments").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            $("#grid-supplier-scorebyKPI").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            break;
        case 'Trend Analysis':
            if ($("#grid-kpi-trends").length > 0) {
                $("#grid-kpi-trends .k-i-expand").trigger("click");
                $("#grid-kpi-trends").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            }

            if ($("#grid-kpi-trends-all").length > 0) {
                $("#grid-kpi-trends-all .k-i-expand").trigger("click");
                $("#grid-kpi-trends-all").parents(".chartBody").find(".expandCollapseText").text("Collapse");
            }

            //var chart = $('#timeSeriesMetricTrends').highcharts();
            //chart.update({
            //    legend: {
            //        enabled: true,
            //        maxHeight: null
            //    }
            //});

            break;
        default:
            break;
    }

    var divId = page == 'Overall View' ? "supplierRatingContainer" : "data-avail";
    var pathName = window.location.protocol + '//' + window.location.host + '/' + window.location.pathname.split('/')[1];
    var bootstrapPath = pathName + '/Content/bootstrap.min.css';
    var kendoCommon = pathName + '/Content/kendo.common.min.css';
    var kendoSilver = pathName + '/Content/kendo.silver.min.css';
    var stylePath = pathName + '/Content/site.css';
    var contents = "<h1 class='col-md-12'>" + page + "</h1>";
    contents += document.getElementById(divId).innerHTML;
    var dashboardFrame = document.createElement('iframe');
    dashboardFrame.name = "dashboardFrame";
    dashboardFrame.style.position = "absolute";
    dashboardFrame.style.bottom = "0px";
    dashboardFrame.style.height = "0px";
    dashboardFrame.style.width = "0px";
    dashboardFrame.style.marginLeft = "-20px";
    document.body.appendChild(dashboardFrame);
    var frameDoc = dashboardFrame.contentWindow ? dashboardFrame.contentWindow : dashboardFrame.contentDocument.document ? dashboardFrame.contentDocument.document : dashboardFrame.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><title></title>');
    frameDoc.document.write(' <link rel="stylesheet" href="' + bootstrapPath + '">');
    //  frameDoc.document.write('<link rel="stylesheet"  href="' + kendoCommon + '">');
    //  frameDoc.document.write('<link rel="stylesheet"  href="' + kendoSilver + '">');
    frameDoc.document.write('<link rel="stylesheet"  href="' + stylePath + '">');
    frameDoc.document.write('<style>.chartIcon,.open>.dropdown-menu{display:none;} </style>');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["dashboardFrame"].focus();
        window.frames["dashboardFrame"].print();
        document.body.removeChild(dashboardFrame);
    }, 1000);


    switch (page) {
        case "Supplier Scorecard":
        case "Perception Gap":
            $("#grid-supplier-comments .k-i-collapse").trigger("click");
            $("#grid-supplier-comments").parents(".chartBody").find(".expandCollapseText").text("Expand");
            break;
            //case 'Trend Analysis':
            //    var chart = $('#timeSeriesMetricTrends').highcharts();
            //    chart.update({
            //        legend: {
            //            enabled: true,
            //            maxHeight: 39
            //        }
            //    });
            //    break;
        default:
            break;
    }
}
