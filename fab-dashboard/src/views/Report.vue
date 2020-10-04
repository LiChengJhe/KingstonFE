<template>
  <div class="container-fluid p-0">
    <div class="row no-gutters">
      <div class="col-2">
        <Listbox
          :options="eqpIds"
          :filter="true"
          @change="listBoxOnChange"
          listStyle="height:85vh;"
        />
      </div>
      <div class="col-10">
        <highcharts :options="chartOptions"></highcharts>
      </div>
    </div>
  </div>
</template>

<script>
import EqpService from "../services/eqp-service";
import WipService from "../services/wip-service";
import _ from "lodash";
import Highcharts from "highcharts";
export default {
  name: "Report",
  components: {},
  data() {
    return {
      eqpIds: null,
      chartOptions: {
        chart: {
          type: "column",
          height: "60%",
        },
        exporting: {
          buttons: {
            contextButton: {
              menuItems: [
                "printChart",
                "separator",
                "downloadPNG",
                "downloadJPEG",
                "downloadPDF",
                "downloadSVG",
                "separator",
                "downloadCSV",
                "downloadXLS",
                "openInCloud",
              ],
            },
          },
        },
        title: {
          text: "每日WIP",
        },
        tooltip: {
          xDateFormat: "%Y-%m-%d",
        },
        xAxis: {
          type: "datetime",
          labels: {
            rotation: -45,
            formatter: function () {
              return Highcharts.dateFormat("%m-%d", this.value);
            },
          },
        },
        yAxis: {
          min: 0,
          title: {
            text: "WIP",
          },
        },
        legend: {
          enabled: false,
        },
        series: [
          {
            name: "WIP",
            data: [],
          },
        ],
      },
    };
  },
  created() {
    EqpService.getAll$().subscribe((res) => {
      this.eqpIds = res.map((o) => o.id);
      this.listBoxOnChange({value:_.first(this.eqpIds ) });
    });
  },
  methods: {
    listBoxOnChange(e) {
      WipService.getDailyStatistics$(e.value).subscribe((res) => {
        this.chartOptions.series[0].data = _.map(res, (o) => [
          o.date,
          o.wipCount,
        ]);
          this.chartOptions.title.text=`${e.value}-每日WIP`;
      });
    },

  },
  mounted() {},
};
</script>
