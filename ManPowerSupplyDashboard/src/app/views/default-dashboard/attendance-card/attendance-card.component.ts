import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { DashboardService } from '../dashboard.service';
import { AttendanceDashboardModel } from '../models/attendance-dashboard-model';

@Component({
  selector: 'app-attendance-card',
  templateUrl: './attendance-card.component.html',
  styleUrls: ['./attendance-card.component.scss']
})
export class AttendanceCardComponent implements OnInit {

  attendances?: AttendanceDashboardModel[] = null;
  isCurrentMonth : Boolean = true;
  loaded : Boolean = false;

  public barChartOptions: ChartOptions = {};
  public barChartLabels: Label[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartDataSets[] = [
    {
      data: [],
      label: 'Amount',
      stack: 'combined',
      type: 'line',
      fill: false,
      lineTension: 0,
      borderWidth: 1,
      yAxisID: 'left'
    },
    {
      data: [],
      label: 'Attendance',
      stack: 'combined',
      type: 'bar',
      yAxisID: 'right',
      backgroundColor: '#5fb2f0c4',
      hoverBorderColor: '#ef6dfc',
      maxBarThickness:40
    }
  ];

  constructor(private _service: DashboardService) { }

  ngOnInit(): void {
    
    this.getConsolidatedAttendance();    
  }

  getConsolidatedAttendance() {

    this.attendances = null;
    this.loaded = false;

    this._service.getConsolidatedAttendance(this.isCurrentMonth).subscribe((x: AttendanceDashboardModel[]) => {
      this.attendances = x;
      this.setChart();
    })
  }

  setChart(){
    this.barChartLabels = [];
    this.barChartData[0].data = [];
    this.barChartData[1].data = [];
    this.attendances.forEach(attendance => {
      let date = new Date(attendance.date);
      this.barChartLabels.push(`${date.getDate()}/${date.getMonth()}`);
      this.barChartData[0].data.push(attendance.attendanceCount);
      this.barChartData[1].data.push(attendance.amount);
    })

    this.barChartOptions = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top',
        },
        title: {
          display: true,
          text: 'Chart.js Combined Line/Bar Chart'
        }
      },
      scales: {
        yAxes: [{
          id: 'right',
          ticks: {
            beginAtZero: true,
            display: false
          },
          gridLines: {
            display: false,
            drawOnChartArea: true
          },
          position: 'right'
        },
        {
          id: 'left',
          ticks: {
            beginAtZero: true
          },
          gridLines: {
            display: false,
            drawOnChartArea: true
          },
          position: 'left'
        }],
        xAxes: [{
          ticks: {
            beginAtZero: true
          },
          gridLines: {
            display: false
          }
        }]
      },
      tooltips: {
        callbacks: {
          label: tooltipItem => {
            let attendance = this.barChartData[0].data[tooltipItem.index];
            let amount = this.barChartData[1].data[tooltipItem.index];
            return [`Attendance : ${attendance}`, `Amount : ${amount}`];
          }
        }
      }
    };
    this.loaded = true;
  }

  toggle(event: MatSlideToggleChange) {
     this.isCurrentMonth = event.checked;
    this.getConsolidatedAttendance();
  }

}
