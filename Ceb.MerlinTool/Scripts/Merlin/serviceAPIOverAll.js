var record = 0;
var grid = {};
var gridColn = {};
var companyDetails = '';
var gridExcelData = '';
var tableColorRange = '';
var objHighchartBar = {};

function getOverAllScore(companyId) {
    $("#page-spinner").show();
    $.ajax({
        url: config_data.WEBAPI_URL + "/api/v1/APIOverAll/GetOverAllScore",
        type: 'Get',
        success: function (response) {
            $("#page-spinner").hide();
            if (response.Status == 1) {
                var response = JSON.parse(response.Data);
                companyDetails = response.companyDetails;
                excelData = response.excelExport;
                objHighchartBar = generateHorizontalBar(response, 'overallScore', false, 'Supplier Name', 'Supplier Rating', false, true,'Overall Supplier Score');

                gridColn = response.tableColumnTitles;
                gridExcelData = response.catMetAvgScore;
                tableColorRange = response.tableRangeColor;
                generateGrid(JSON.parse(response.catMetAvgScore), response.tableColumnTitles);
            }
            else {
                failureAlert(response.Message);
            }
        },
        error: function (data) {
            $("#page-spinner").hide();
            failureAlert(data);
        },
        data: { 'companyId': companyId },
        contentType: 'json'
    });
}

function generateGrid(response, columnTitles) {
    var model = generateModel(response);
    var columns = [];

    var tableIndex = [{
        field: "SNo",
        title: "S.No",
        width: 40,
        locked: true
    }, {
        field: "Category",
        title: "Category<sup class='noteIconSup'><i class='fa fa-asterisk'></i></sup>",
        sortable: {
            initialDirection: "asc"
        },
        width: 155,
        locked: true,
        template: "<span class='linkText pointer' onclick='categorytemplate(#=fk_CategoryId#)'>#= Category#</span>",
        attributes: {
            "class": "strong text-left tableChartCategory"
        },

    }];

    var dynamicGridArray = generateColumns(Object.keys(response[0]), columnTitles);
    columns = tableIndex.concat(dynamicGridArray);

    grid = {       
        pdf: {
            fileName: "Overall and metric ratings by Category.pdf",
            allPages: true,
            proxyURL: config_data.WEB_URL + "Home/Save",
            forceProxy: true,
        },
        dataSource: {
            data: response,
            schema: {
                model: {
                    Id: "S.No",
                    fields: model
                }
            },
            pageSize: 10
        },
        pageable: true,
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

    $("#grid-ratings").kendoGrid($.extend({
        dataBound: function (e) {

            for (property in columnTitles) {
                if (property != "Category" && property != "SNo" && property !== "fk_CategoryId") {
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
                        if (score == 0 || score==null) {
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

function processKendoForExport(callBack) {
    var exportGrid = grid;
    record = 0;
    for (var i = 0; i < exportGrid.columns.length; i++) {
        exportGrid.columns[i].locked = false;
    }
    exportGrid.pageable = false;
    exportGrid.dataSource.pageSize = 15;
    $("#grid-ratings-export").kendoGrid($.extend({
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
                        if (score == 0 || score==null) {
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

function generateColumns(columnNames, columnTitles) {

    var index = columnNames.indexOf("Category");
    if (index > -1) {
        columnNames.splice(index, 1);
    }

    index = columnNames.indexOf("SNo");
    if (index > -1) {
        columnNames.splice(index, 1);
    }

    return columnNames.map(function (name) {
        if (name == "fk_CategoryId") {
            return {
                field: name, hidden: true
            };
        }
        else {
            var attributesVal = "";
            if (name == "OverAllScore")
                attributesVal = "tableChartOverallScore";
            else
                attributesVal = "tableChartColumn";
            return {
                field: name, format: "{0:n2}", title: columnTitles[name], width: 140, attributes: {
                    "class": attributesVal, locked: true
                }
            };
        }
    });
}

function generateModel(response) {

    var sampleDataItem = response[0];
    var model = {};
    for (var property in sampleDataItem) {
        {
            if (property == 'Category')
                model[property] = { type: "string" };
            else
                model[property] = { type: "number" };
        }
    }
    return model;
}

function exportChartOverall(element, type) {
    if (type == 'excel')
        exportGrid(element, excelData, companyDetails, "", config_data.OverallTitle, 'Overall View');
    else
        exportChart(element, type, companyDetails, config_data.OverallTitle, 'Overall View');
}

function exportGridData(element, type) {
    exportGrid(element, gridExcelData, companyDetails, gridColn, config_data.OverallGridTitle, 'Overall View');
}
