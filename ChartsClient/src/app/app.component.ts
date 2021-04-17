import { Component } from '@angular/core';
import * as Highcharts  from 'highcharts';
import * as signalR  from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  connection:signalR.HubConnection;
  constructor() {
    this.connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5000/salehub").build();
    this.connection.start();

    this.connection.on("receiveMessage",data=>{
      this.chart.showLoading();
      console.log(data);
      this.chartoption.series = data;
      //this.chart.series[0].setData(data);
      let nu = this.chart.series.length;
      for (let i = 0;i<nu;i++){
        debugger;
        this.chart.series[0].remove();
        debugger;
      }
      for (let i = 0;i<data.length;i++){
        debugger;
        this.chart.addSeries(data[i]);
      }
      this.updateFromInput = true;
      this.chart.hideLoading();

    });

    const self = this;
    this.chartCallback = chart =>{
      self.chart = chart;
    }
  }

  chart;
  updateFromInput = false;
  chartCallback;

  Highcharts : typeof Highcharts = Highcharts;
  chartoption : Highcharts.Options = {
    //Grafik title
    chart:{
      type:"bar"
    },
    title:{
      text : "Başlık"
    },
    //Alt title
    subtitle:{
      text : "Alt Başlık"
    },
    //Y Ekseni
    yAxis:{
      title : {
        text : "Harcamalar"
      },
      range : 5000
    },
    //X Ekseni
    xAxis:{
      categories : ["Ocak","Şubat","Mart","Nisan","Mayıs","Haziran","Temmuz","Ağustos","Eylül","Ekim","Kasım","Aralık"],
      title : {
        text : "Aylar"
      },
      accessibility:{
        rangeDescription:"2019 - 2020"
      }
    },
    legend:{
      layout : "vertical",
      align:"right",
      verticalAlign : "middle"
    },
    plotOptions:{
      series:{
        label:{
          connectorAllowed : true
        },
        pointStart:0
      }
    }
  }

}
