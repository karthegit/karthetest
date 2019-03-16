var highchartBar = {
    chart: {
        type: 'bar',
        spacingTop: 0,
        spacingLeft: 2,
        spacingBottom: 2
    },
    title: {
        text: ''
    },
    xAxis: {
        type: 'category',
        title: {
            text: 'Supplier Name', style: {
                fontSize: '12px',
                fontWeight: 'bold',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        labels: {
            rotation: 0,
            style: {
                fontSize: '12px',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        scrollbar: {
            enabled: false
        },
        min: 0,
        max: null,
        lineWidth: 0,
        minorGridLineWidth: 0,
        lineColor: 'transparent',
        minorTickLength: 0,
        tickLength: 0
    },  
    yAxis: {
        min: 0,
        max: 5,
        title: {
            text: 'Supplier Rating', style: {
                fontSize: '12px',
                fontWeight: 'bold',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        tickInterval: 1
    },
    legend: {
        enabled: false
    },
    tooltip: {
        headerFormat: '<b>{point.x}</b><br/>',
        pointFormat: 'Overall Supplier Score (Weighted): <b>{point.y:.2f}</b><br />No of Respondents: {point.n}<br />'
    },
    credits: false,
    series: [{
        name: 'Population',
        color: '#00Aeef',
        data: [],
        dataLabels: {
            enabled: true,
            rotation: 0,
            color: '#000',
            align: 'right',
            format: '{point.y:.2f}', // one decimal
            y: 0,// 10 pixels down from the top
            x: 40,
            style: {
                fontSize: '12px',
                fontFamily: 'Arial, Helvetica, sans-serif'

            }
        }
    },
    {
        type: 'scatter',
        name: 'Category Average',
        data: {},
        marker: {
            fillColor: '#0a3f6b',
            symbol: 'diamond',
            enabled: true,
            radius: 8
        }
    }]
};

var highchartGauge = {
    "chart": {
        "type": "solidgauge"
    },
    "title": null,
    "pane": {
        "center": ["50%", "95%"],
        "size": "150%",
        "startAngle": -90,
        "endAngle": 90,
        "background": [{
            "innerRadius": "75%",
            "outerRadius": "100%",
            "shape": "arc",
            "borderColor": "transparent",
            //backgroundColor: Highcharts.Color("#7fd6f7").setOpacity(0.3).get(),
            borderWidth: 0,
        },
    {
        "innerRadius": "125%",
        "outerRadius": "100%",
        "shape": "arc",
        "borderColor": "transparent",
        //backgroundColor: Highcharts.Color("#ffa500").setOpacity(0.3).get(),
        borderWidth: 0,
    }]
    },
    "legend": {
        "enabled": true
    },

    "yAxis": {
        "min": 0,
        "max": 5,
        tickColor: '#000',
        tickPosition: 'outside',
        zIndex: 4,
        "lineColor": "transparent",
        "lineWidth": 0,
        "minorTickInterval": "auto",
        "minorTickWidth": 0,
        "minorTickLength": 0,
        "minorTickPosition": "outside",
        "tickPixelInterval": 30,
        "tickWidth": 1,
        "tickPositions": [0, 1, 2, 3, 4, 5],
        "tickLength": 10,
        "labels": {
            "enabled": true,
            "format": "",
            "rotation": "0",
            "crop": false,
            "distance": 15,
            "inside": false,
            "y": 0,
            "allowOverlap": false,
            "step": 1,
            "style": {
                "fontSize": "12px",
                "fontWeight": "bold",
                "textShadow": false,
                color: "black"
            }
        }
    },
    "credits": {
        "enabled": false
    },
    "plotOptions": {
        "solidgauge": {
            "dataLabels": {
                "enabled": true,
                "borderColor": "transparent"
            }
        }
    },
    "tooltip": {
        "pointFormat": "{series.name} : {point.y:.2f}",
        "enabled": true,
        followPointer: true
    },
    "series": [{
        "name": "Supplier Score",
        legendIndex: 0,
        "showInLegend": true,
        "marker": {
            "symbol": "square",
            "fillColor": "#00Aeef"
        },
        "dataLabels": {
            "y": -54,
            "format": "<div><font style=\"font-size: 12px;color:#00Aeef;\">{series.name} : {y:.2f}</font></div>"
            //"format": "<div><font style=\"font-size: 12px;color:#0B5E6F;\">{series.name} : {y:.2f}</font></div>"
        },
        "data": [{
            "color": {
                "linearGradient": [0, 0, 300, 0],
                "stops": [
				    [0.1, "#00Aeef"],
					[0.5, "#00Aeef"],
					[0.9, "#00Aeef"]
                ]
            },
            "y": [3.4595299999999822]
        }],

        "innerRadius": "125%",
        "outerRadius": "100%"
    }, {
        "name": "Category Average",
        "showInLegend": true,
        "marker": {
            "symbol": "square",
            "fillColor": "#0a3f6b"
        },
        "dataLabels": {
            "y": -25,
            "format": "<div><font style=\"font-size: 12px;color:#0a3f6b;\">{series.name} : {y:.2f}</font></div>"

            //"format": "<div><font style=\"font-size: 12px;color:#0B5E6F;\">{series.name} : {y:.2f}</font></div>"
        },
        "data": [{
            "color": {
                "linearGradient": [0, 0, 300, 0],
                "stops": [
					[0.1, "#0a3f6b"],
					[0.5, "#0a3f6b"],
					[0.9, "#0a3f6b"]
                ]
            },
            "y": [3.8346599999999924]
        }],
        "innerRadius": "75%",
        "outerRadius": "100%",
        legendIndex: 1

    }]
};

var timeseriesChart = {
    chart: {
        type: 'line'
    },
    title: {
        text: ''
    },
    xAxis: {
        categories: [],
        title: {
            text: 'Supplier Rating', style: {
                fontSize: '12px',
                fontWeight: 'bold',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        tickInterval: 1
    },
    yAxis: {
        min: 1,
        max: 5,
        tickInterval: 1,
        title: {
            text: 'Supplier Name', style: {
                fontSize: '12px',
                fontWeight: 'bold',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        labels: {
            rotation: 0,
            style: {
                fontSize: '12px',
                fontFamily: 'Arial, Helvetica, sans-serif'
            }
        },
        scrollbar: {
            enabled: false
        },
        lineWidth: 0,
        minorGridLineWidth: 0,
        lineColor: 'transparent',
        minorTickLength: 0,
        tickLength: 0
    },
    plotOptions: {
        line: {
            dataLabels: {
                enabled: true,
                rotation: 0,
                color: '#000',
                align: 'right',
                allowOverlap: true,
                format: '{point.y:.2f}', // one decimal
                y: 0,// 10 pixels down from the top
                x: 40,
                style: {
                    fontSize: '12px',
                    fontFamily: 'Arial, Helvetica, sans-serif'
                }
            }
        }
    },
    credits: false,
    legend: { enabled: false },
    series: [{
        data: []
    }]
};

var highchartHalfDonut = {
    "chart": {
        "type": "solidgauge"
    },
    "title": null,
    "pane": {
        "center": ["50%", "95%"],
        "size": "150%",
        "startAngle": -90,
        "endAngle": 90,
        "background": [
    {
        "innerRadius": "125%",
        "outerRadius": "100%",
        "shape": "arc",
        "borderColor": "transparent",
        //backgroundColor: Highcharts.Color("#ffa500").setOpacity(0.3).get(),
        borderWidth: 0,
    }
        ]
    },
    "legend": {
        "enabled": true
    },

    "yAxis": {
        "min": 0,
        "max": 5,
        tickColor: '#000',
        tickPosition: 'outside',
        zIndex: 4,
        "lineColor": "transparent",
        "lineWidth": 0,
        "minorTickInterval": "auto",
        "minorTickWidth": 0,
        "minorTickLength": 0,
        "minorTickPosition": "outside",
        "tickPixelInterval": 30,
        "tickWidth": 1,
        "tickPositions": [0, 1, 2, 3, 4, 5],
        "tickLength": 10,
        "labels": {
            "enabled": true,
            "format": "",
            "rotation": "0",
            "crop": false,
            "distance": 15,
            "inside": false,
            "y": 0,
            "allowOverlap": false,
            "step": 1,
            "style": {
                "fontSize": "12px",
                "fontWeight": "bold",
                "textShadow": false,
                color: "black"
            }
        }
    },
    "credits": {
        "enabled": false
    },
    "plotOptions": {
        "solidgauge": {
            "dataLabels": {
                "enabled": true,
                "borderColor": "transparent"
            }
        }
    },
    "tooltip": {
        "pointFormat": "{series.name} : {point.y:.2f}",
        "enabled": true,
        followPointer: true
    },
    "series": [{
        "name": "Over all score",
        legendIndex: 0,
        "showInLegend": true,
        "marker": {
            "symbol": "square",
            "fillColor": "#00Aeef"
        },
        "dataLabels": {
            "y": -54,
            "format": "<div><font style=\"font-size: 15px;color:#00Aeef;\">{series.name} : {y:.2f}</font></div>"
            //"format": "<div><font style=\"font-size: 12px;color:#0B5E6F;\">{series.name} : {y:.2f}</font></div>"
        },
        "data": [{
            "color": {
                "linearGradient": [0, 0, 300, 0],
                "stops": [
				    [0.1, "#00Aeef"],
					[0.5, "#00Aeef"],
					[0.9, "#00Aeef"]
                ]
            },
            "y": [3.4595299999999822]
        }],

        "innerRadius": "125%",
        "outerRadius": "100%"
    }]
};