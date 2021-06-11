<template>
  <div class="app-container">
    <el-form ref="form" :model="sentinelConfig">      
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存并重启网关</el-button>
      </el-form-item>
      <el-form-item>
        <el-tabs type="border-card">
        <el-tab-pane label="限流规则">
            <div class="grid">
                <el-table
                    :data="sentinelConfig.flowRules"
                    border
                    fit
                    highlight-current-row
                    style="width: 100%;"
                    >
                    <el-table-column label="Dapr服务名" prop="id" width="200" align="center">
                        <template slot-scope="{row}">
                        <el-select v-model="row.serviceName" @change="row.pathName = ''" placeholder="请选择Dapr服务名">
                            <el-option v-for="item2 in services" :key="item2.serviceName" :label="item2.serviceName" :value="item2.serviceName" />
                        </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="子路径" prop="id" width="400" align="center">
                        <template slot-scope="{row}">
                        <el-select style="width:100%" v-model="row.pathName" placeholder="请选择子路径">
                            <el-option v-for="item2 in services.filter(x => x.serviceName === row.serviceName)[0].pathName" :key="item2" :label="item2" :value="item2" />
                        </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="限流类型" prop="id" width="200" align="center">
                        <template slot-scope="{row}">
                          <el-select style="width:100%" v-model.number="row.controlBehavior" placeholder="请选择限流类型">
                              <el-option v-for="item2 in controlBehavior" :key="item2.key" :label="getcontrolBehaviorlabel(item2.value)" :value="item2.value" />
                          </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="队列等待时长(ms)" prop="id" width="150" align="center">
                        <template slot-scope="{row}">
                        <el-input v-show="row.controlBehavior === 1" v-model.number="row.maxQueueingTimeMs" />
                        </template>
                    </el-table-column>
                    <el-table-column label="阈值(qps)" prop="id" width="100" align="center">
                        <template slot-scope="{row}">
                        <el-input v-model.number="row.threshold" />
                        </template>
                    </el-table-column>
                    <el-table-column label="操作" align="center" width="200" class-name="small-padding fixed-width">
                        <template slot-scope="{row,$index}">
                        <el-button size="mini" type="danger" @click="handleDeleteflowRules(row,$index)">
                            删除
                        </el-button>
                        </template>
                    </el-table-column>
                </el-table>
                <el-form-item>
                    <el-button type="primary" @click="handleAddflowRules()">
                        新增限流规则
                    </el-button>
                </el-form-item>
            </div>
        </el-tab-pane>
        <el-tab-pane label="熔断规则">
            <div class="grid">
                <el-table
                    :data="sentinelConfig.breakingRules"
                    border
                    fit
                    highlight-current-row
                    style="width: 100%;"
                    >
                    <el-table-column label="Dapr服务名" prop="id" width="200" align="center">
                        <template slot-scope="{row}">
                        <el-select v-model="row.serviceName" @change="row.pathName = ''" placeholder="请选择Dapr服务名">
                            <el-option v-for="item2 in services" :key="item2.serviceName" :label="item2.serviceName" :value="item2.serviceName" />
                        </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="子路径" prop="id" width="400" align="center">
                        <template slot-scope="{row}">
                        <el-select style="width:100%" v-model="row.pathName" placeholder="请选择子路径">
                            <el-option v-for="item2 in services.filter(x => x.serviceName === row.serviceName)[0].pathName" :key="item2" :label="item2" :value="item2" />
                        </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="熔断类型" prop="id" width="200" align="center">
                        <template slot-scope="{row}">
                          <el-select style="width:100%" v-model.number="row.strategy" placeholder="请选择熔断类型">
                              <el-option v-for="item2 in strategy" :key="item2.key" :label="getstrategylabel(item2.value)" :value="item2.value" />
                          </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="重试时间(ms)" prop="id" width="150" align="center">
                        <template slot-scope="{row}">
                          <el-input v-model.number="row.retryTimeoutMs" />
                        </template>
                    </el-table-column>
                    <el-table-column label="慢调用临界值(ms)" prop="id" width="150" align="center">
                        <template slot-scope="{row}">
                          <el-input v-show="row.strategy === 0" v-model.number="row.maxAllowedRtMs" />
                        </template>
                    </el-table-column>
                    <el-table-column label="阈值" prop="id" width="150" align="center">
                        <template slot-scope="{row}">
                        <el-input v-model.number="row.threshold">
                            <template v-if="row.strategy === 0 || row.strategy === 1" slot="append">%</template>
                        </el-input>
                        </template>
                    </el-table-column>
                    <el-table-column label="操作" align="center" width="200" class-name="small-padding fixed-width">
                        <template slot-scope="{row,$index}">
                        <el-button size="mini" type="danger" @click="handleDeletebreakingRules(row,$index)">
                            删除
                        </el-button>
                        </template>
                    </el-table-column>
                </el-table>
                <el-form-item>
                    <el-button type="primary" @click="handleAddbreakingRules()">
                        新增限流规则
                    </el-button>
                </el-form-item>
            </div>
        </el-tab-pane>
        </el-tabs>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
import { GetSentinelConfig, GetCommonData, SaveSentinelConfig } from '@/api/mallsetting'
export default {
  data() {
    return {
      sentinelConfig: {
        flowRules: [],
        breakingRules: []
      },
      services: [],
      controlBehavior: [],
      strategy: []
    }
  },
  created() {
    GetSentinelConfig().then(response => {
        this.sentinelConfig.flowRules = response.data.flowRules
        this.sentinelConfig.breakingRules = response.data.breakingRules
      }, msg => { })
    GetCommonData().then(response => {
       this.services = response.data.services
       this.controlBehavior = response.data.controlBehavior
       this.strategy = response.data.strategy
      }, msg => { })
  },
  methods: {
    getservices(services, key){
        debugger
        var s = services.filter(x => x.serviceName===key)
        return s
    },
    onSubmit() {
      SaveSentinelConfig(this.sentinelConfig).then(response => {
        this.$message({
          type: 'info',
          message: '保存成功'
        })
      }, msg => { })
    },
    handleDeleteflowRules(row,index) {
      this.sentinelConfig.flowRules.splice(index, 1)
    },
    handleAddflowRules() {
       this.sentinelConfig.flowRules.push({
            serviceName: this.services[0].serviceName,
            pathName: this.services[0].pathName[0],
            threshold: 0,
            controlBehavior: 0,
            maxQueueingTimeMs:0,
       })
    },
    handleDeletebreakingRules(row,index) {
      this.sentinelConfig.breakingRules.splice(index, 1)
    },
    handleAddbreakingRules() {
       this.sentinelConfig.breakingRules.push({
            serviceName: this.services[0].serviceName,
            pathName: this.services[0].pathName[0],
            threshold: 0,
            strategy: 0,
            retryTimeoutMs: 0,
            maxAllowedRtMs: 0,
       })
    },
    getcontrolBehaviorlabel(key) {
      const controlBehaviorLibel = [ '拒绝服务', '队列等待' ]
      return controlBehaviorLibel[key]
    },
    getstrategylabel(key) {
      const strategylabel = ['慢调用比例', '错误比例', '错误数']
      return strategylabel[key]
    }
  }
}
</script>


  /// <summary>
        /// 熔断策略
        /// </summary>
        public Strategy Strategy { get; set; }
        /// <summary>
        /// 重试时间
        /// </summary>
        public int RetryTimeoutMs { get; set; }
        /// <summary>
        /// 慢调用临界值单位为 ms
        /// </summary>
        public int? MaxAllowedRtMs { get; set; }
        /// <summary>
        /// 比例阈值
        /// </summary>
        public decimal Threshold { get; set; }