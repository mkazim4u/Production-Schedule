<%@ Control Language="VB" AutoEventWireup="true" CodeFile="PS_Chart.ascx.vb"
    Inherits="PS_Chart" %>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        
        var completedJobs = <%= GetCompletedJobs() %>    
        var unCompletedJobs = <%= GetUnCompletedJobs() %>    

        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);

            
        
        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Task');
            data.addColumn('number', 'Hours per Day');
            data.addColumn('string', 'Jobs');
            data.addColumn('number', 'Jobs per Month');
            data.addRows(3);
            data.setValue(0, 0, 'Completed');
            data.setValue(0, 1, completedJobs, '%');
            data.setValue(1, 0, 'UnCompleted');
            data.setValue(1, 1, unCompletedJobs, '%');
            data.setValue(2, 0, 'Cancelled');
            data.setValue(2, 1, 10, '%');
            var options = { colors: ['green', 'red', 'yellow'], is3D: true, title: ['Production Schedule : March, 2011'], width: 450, height: 450 };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    
​
</script>
    <div id="chart_div" align="center" >
    </div>
