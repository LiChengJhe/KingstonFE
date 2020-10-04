<template>
  <div class="container-fluid p-0">
    <div class="row no-gutters">
      <div class="col-9">
        <div class="row">
          <div class="col-12">
            <div class="d-inline" v-for="(item, key) in eqpMap" :key="key">
              <CustomBtn
                v-if="selectedStatus.includes(item.statusInfo.curType)"
                :text="key"
                icon="laptop"
                :backgroundColor="statusColorMap[item.statusInfo.curType]"
                @onClick="eqpOnClick"
              ></CustomBtn>
            </div>
          </div>
        </div>
      </div>
      <div class="col-3">
        <div class="card rounded-0 bg-light h-100" style="min-height:95vh">
          <div class="card-header">設定</div>
          <div class="card-body">
            <form>
              <div class="form-group">
                <label for="factory">廠區</label>
                <select disabled class="form-control" id="factory">
                  <option>竹科</option>
                </select>
              </div>
              <div class="form-group">
                <label for="floor">樓層</label>
                <select disabled class="form-control" id="floor">
                  <option>F1</option>
                </select>
              </div>
              <div class="form-group">
                <label for="area">區域</label>
                <select disabled class="form-control" id="area">
                  <option>A</option>
                </select>
              </div>

              <div class="form-group">
                <label for="status">機台狀態</label>
                <MultiSelect
                  id="status"
                  v-model="selectedStatus"
                  :options="statusNames"
                  :filter="true"
                >
                  <template #value="slotProps">
                    <a
                      v-for="option of slotProps.value"
                      :key="option"
                      href="#"
                      class="badge text-white mx-1"
                      :style="{backgroundColor:statusColorMap[option]}"
                    >{{option}}</a>
                    <template v-if="!slotProps.value || slotProps.value.length === 0">請選擇...</template>
                  </template>
                  <template #option="slotProps">
                    <a
                      href="#"
                      class="badge text-white"
                      :style="{backgroundColor:statusColorMap[slotProps.option]}"
                    >{{slotProps.option}}</a>
                  </template>
                </MultiSelect>
              </div>
            </form>
            <a
              href="#"
              class="btn m-1 text-white"
              :style="{backgroundColor:statusColorMap[status]}"
              :key="status"
              v-for="(status) in selectedStatus"
            >
              {{status}}
              <span class="badge badge-light">{{statusCountMap[status]}}</span>
            </a>
          </div>
        </div>
      </div>
    </div>

    <EqpModal ref="modal" :data="selectedEqp"></EqpModal>
  </div>
</template>

<script>
import DashboardHub from "../hubs/dashboard-hub";
import { Status, StatusColorMap } from "../models/status";
import CustomBtn from "../components/CustomBtn";
import EqpModal from "../components/EqpModal";
import WipService from "../services/wip-service";
import EqpService from "../services/eqp-service";
import _ from "lodash";
export default {
  name: "Dashboard",
  components: {
    CustomBtn,
    EqpModal,
  },
  data() {
    const statusNames = _.keys(Status);
    const statusCountMap = {};
    statusNames.forEach((name) => {
      statusCountMap[name] = 0;
    });
    return {
      hub: new DashboardHub(),
      eqpMap: {},
      selectedEqp: null,
      subscrEvent: null,
      selectedStatus: statusNames,
      statusColorMap: StatusColorMap,
      statusNames: statusNames,
      statusCountMap: statusCountMap,
    };
  },
  methods: {
    eqpOnClick(eqpId) {
      this.updateSelectedEqp(eqpId);
      this.$refs.modal.show();
    },
    updateStatusCountMap() {
      this.statusNames.forEach((name) => {
        this.statusCountMap[name] = 0;
      });
      for (const key in this.eqpMap) {
        const status = this.eqpMap[key].statusInfo.curType;
        this.statusCountMap[status] += 1;
      }
    },
    updateEqps(eqps) {
      let includeSelectedEqp = false;
      eqps.forEach((eqp) => {
        let currentWipCount = 0;
        if (eqp.cassetteInfo) {
          eqp.cassetteInfo.lots.forEach((lot) => {
            lot.wafers.forEach((wafer) => {
              currentWipCount++;
            });
          });
        }
        eqp.wip = {
          currentCount: currentWipCount,
          todayCount: null,
        };
        this.eqpMap[eqp.id] = eqp;
        if (this.selectedEqp && this.selectedEqp.id == eqp.id) {
          includeSelectedEqp = true;
        }
      });
      this.eqpMap = { ...this.eqpMap };
      if (includeSelectedEqp) {
        this.updateSelectedEqp(this.selectedEqp.id);
      }
    },
    updateSelectedEqp(eqpId) {
      this.selectedEqp = this.eqpMap[eqpId];
      WipService.getTodayStatistics$(eqpId).subscribe((res) => {
        this.selectedEqp.wip.todayCount = res.wipCount;
      });
    },
    invokeHub() {
      this.hub.connect(() => {
        this.hub.joinGroup().subscribe();
        this.hub.enableReceiver();
        this.subscrEvent = this.hub.getData$().subscribe((eqps) => {
          this.updateEqps(eqps);
          this.updateStatusCountMap();
        });
      });
    },
  },
  mounted() {
    EqpService.getAll$().subscribe((res) => {
      res.forEach((eqp) => {
       // eqp.statusInfo = { curType: Status.UNKNOWN ,curTime:Date.now()};
        this.eqpMap[eqp.id] = eqp;
      });
      this.eqpMap = { ...this.eqpMap };
      this.updateStatusCountMap();
      this.invokeHub();
    });
  },
  destroyed() {
    this.subscrEvent.unsubscribe();
    this.hub.close();
  },
};
</script>
<style>
.p-multiselect {
  width: 100%;
}
</style>