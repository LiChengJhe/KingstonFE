
<template>
  <div ref="modal" class="modal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
      <div class="modal-content">
        <div class="modal-header text-white bg-dark">
          <h5 class="modal-title">EQP</h5>
          <button type="button" class="close" data-dismiss="modal">
            <span>&times;</span>
          </button>
        </div>
        <div class="modal-body p-0">
          <div v-if="data" class="card rounded-0">
            <div class="card-header bg-white">
              <ul class="nav nav-tabs card-header-tabs">
                <li class="nav-item">
                  <a class="nav-link active" data-toggle="tab" href="#eqp-info" role="tab">EQP Info</a>
                </li>
                <li class="nav-item">
                  <a
                    class="nav-link"
                    data-toggle="tab"
                    href="#cassette-info"
                    role="tab"
                  >Cassette Info</a>
                </li>

                <li class="nav-item">
                  <a class="nav-link" data-toggle="tab" href="#alarm-info" role="tab">Alarm Info</a>
                </li>

                <li class="nav-item">
                  <a class="nav-link" data-toggle="tab" href="#wip-history" role="tab">WIP History</a>
                </li>
              </ul>
            </div>
            <div class="card-body">
              <div class="tab-content">
                <div class="tab-pane fade show active" id="eqp-info">
                  <form>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">EQP Id</span>
                        </h5>
                        <input class="form-control" disabled type="text" :value="data.id" />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">EQP Type</span>
                        </h5>
                        <input class="form-control" disabled type="text" :value="data.type" />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">EQP Sataus</span>
                        </h5>

                        <input
                          class="form-control"
                          disabled
                          type="text"
                          :value="data.statusInfo.curType"
                        />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span
                            class="badge badge-light w-100 text-left"
                          >EQP Sataus Duration (minute)</span>
                        </h5>
                        <div class="input-group">
                        
                          <input
                            class="form-control"
                            disabled
                            type="text"
                            :value="data.statusInfo.prevTime|date-format"
                          />
                          <div class="input-group-append">
                            <span class="input-group-text">~</span>
                          </div>
                            <input
                            type="text"
                            class="form-control"
                            disabled
                            :value="data.statusInfo.curTime|date-format"
                          />
                          <div class="input-group-append">
                            <span class="input-group-text">=</span>
                          </div>
                          <input
                            class="form-control"
                            disabled
                            type="text"
                            :value="getDuration(data.statusInfo.prevTime,data.statusInfo.curTime)"
                          />
                        </div>
                      </div>
                    </div>
                  </form>
                </div>
                <div class="tab-pane fade" id="cassette-info">
                  <DataTable
                    :value="getTableData(data)"
                    :scrollable="true"
                    scrollHeight="300px"
                    :paginator="true"
                    :rows="5"
                    :rowsPerPageOptions="[5,10,15]"
                  >
                    <Column field="lot" header="Lot" sortable></Column>
                    <Column field="waferId" header="Wafer Id" sortable></Column>
                  </DataTable>
                </div>

                <div class="tab-pane fade" id="alarm-info">
                  <form v-if="data.alarmInfo">
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Alarm Id</span>
                        </h5>
                        <input class="form-control" disabled type="text" :value="data.alarmInfo.id" />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Alarm Type</span>
                        </h5>
                        <input
                          class="form-control"
                          disabled
                          type="text"
                          :value="data.alarmInfo.type"
                        />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Alarm Text</span>
                        </h5>
                        <textarea
                          class="form-control"
                          rows="5"
                          disabled
                          :value="data.alarmInfo.text"
                        ></textarea>
                      </div>
                    </div>

                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Alarm Duration (minute)</span>
                        </h5>
                        <div class="input-group">
                          <input
                            type="text"
                            class="form-control"
                            disabled
                            :value="data.alarmInfo.timeFrom|date-format"
                          />
                          <div class="input-group-append">
                            <span class="input-group-text">~</span>
                          </div>
                          <input
                            class="form-control"
                            disabled
                            type="text"
                            :value="data.alarmInfo.timeTo|date-format"
                          />
                          <div class="input-group-append">
                            <span class="input-group-text">=</span>
                          </div>
                          <input
                            class="form-control"
                            disabled
                            type="text"
                            :value="getDuration(data.alarmInfo.timeFrom,data.alarmInfo.timeTo)"
                          />
                        </div>
                      </div>
                    </div>
                  </form>
                </div>

                <div class="tab-pane fade" id="wip-history">
                  <form v-if="data.wip">
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Current WIP Count</span>
                        </h5>
                        <input
                          class="form-control"
                          disabled
                          type="text"
                          :value="data.wip.currentCount"
                        />
                      </div>
                    </div>
                    <div class="form-row">
                      <div class="form-group col">
                        <h5 class="m-0">
                          <span class="badge badge-light w-100 text-left">Today WIP Count</span>
                        </h5>
                        <input
                          class="form-control"
                          disabled
                          type="text"
                          :value="data.wip.todayCount"
                        />
                      </div>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer p-0">
          <button
            type="button"
            class="btn btn-secondary btn-block m-0 rounded-0"
            data-dismiss="modal"
          >Close</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import moment from "moment";

export default {
  name: "EqpModal",
  props: {
    data: null,
  },

  methods: {
    getTableData(eqp) {
      const tableData = [];
      if (eqp.cassetteInfo) {
        eqp.cassetteInfo.lots.forEach((lot) => {
          lot.wafers.forEach((wafer) => {
            tableData.push({
              lot: lot.id,
              waferId: wafer.id,
            });
          });
        });
      }
      return tableData;
    },
    getDuration(from, to) {
      return _.round(new moment(to).diff(new moment(from),'seconds')/60,2);
    },
    show() {
      this.$(this.$refs.modal).modal("show");
    },
  },
};
</script>

