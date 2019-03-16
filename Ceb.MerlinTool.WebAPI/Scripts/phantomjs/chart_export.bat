@echo off
%1\phantomjs %1\highcharts-convert.js -infile %2 -outfile %3 -width %4 -height %5 -constr Chart