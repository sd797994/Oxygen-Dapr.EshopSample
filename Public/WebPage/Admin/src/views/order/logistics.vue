<template>
  <div class="app-container">
    <el-table
      v-loading="loading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column label="订单号" align="center">
        <template slot-scope="{row}">
          <span>{{ row.orderNo }}</span>
        </template>
      </el-table-column>
      <el-table-column label="物流" align="center">
        <template slot-scope="{row}">
          <span>{{ GetlogisticsType(row.logisticsType) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="物流单号" align="center">
        <template slot-scope="{row}">
          <span>{{ row.logisticsNo }}</span>
        </template>
      </el-table-column>
      <el-table-column label="寄件人" align="center">
        <template slot-scope="{row}">
          <span>{{ row.deliverName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="寄件地址" align="center">
        <template slot-scope="{row}">
          <span>{{ row.deliverAddress }}</span>
        </template>
      </el-table-column>
      <el-table-column label="寄件时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.deliveTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="收件人" align="center">
        <template slot-scope="{row}">
          <span>{{ row.receiverName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="收件地址" align="center">
        <template slot-scope="{row}">
          <span>{{ row.receiverAddress }}</span>
        </template>
      </el-table-column>
      <el-table-column label="收件时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.receiveTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="物流状态" align="center">
        <template slot-scope="{row}">
          <span>{{ row.logisticsState===0?'已寄件':row.logisticsState===1?'已收件':'' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row}">
          <el-button v-show="row.id===null" type="primary" size="mini" @click="showdeliverhandle(row)">
            发货
          </el-button>
          <el-button v-show="row.logisticsState===0" type="primary" size="mini" @click="showreceivehandle(row)">
            确认收货
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />
    <el-dialog title="发货" :visible.sync="showdeliver" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="deliver" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="寄件物流" prop="name">
          <el-select v-model="deliver.logisticsType" placeholder="请选择寄件物流">
            <el-option v-for="(item,index) in logisticsType" :key="index" :label="item.name" :value="item.key" />
          </el-select>
        </el-form-item>
        <el-form-item label="回执单号" prop="name">
          <el-input v-model="deliver.logisticsNo" maxlength="20" />
        </el-form-item>
        <el-form-item label="寄件日期" prop="name">
          <el-date-picker v-model="deliver.deliveTime" type="datetime" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="showdeliver = false">
          取消
        </el-button>
        <el-button type="primary" @click="createDeliver()">
          确认
        </el-button>
      </div>
    </el-dialog>
    <el-dialog title="确认收货" :visible.sync="showreceive" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="temp" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="收件日期" prop="name">
          <el-date-picker v-model="deliver.deliveTime" type="datetime" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="showreceive = false">
          取消
        </el-button>
        <el-button type="primary" @click="createReceive()">
          确认
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import { GetLogisticsList, CreateDeliver, CreateReceive } from '@/api/logistics'
import Pagination from '@/components/Pagination'

export default {
  name: 'ComplexTable',
  components: { Pagination },
  data() {
    return {
      list: null,
      showdeliver: false,
      showreceive: false,
      loading: false,
      total: 0,
      deliver: {
        orderId: null,
        logisticsType: null,
        logisticsNo: '',
        deliveTime: null
      },
      receive: {
        logisticsId: null,
        receiveDate: null
      },
      listQuery: {
        page: 1,
        limit: 20
      },
      logisticsType: [
        { key: 0, name: '顺丰' },
        { key: 1, name: '申通' },
        { key: 2, name: '圆通' },
        { key: 3, name: '中通' },
        { key: 4, name: '韵达' },
        { key: 5, name: 'EMS' }
      ]
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.loading = true
      GetLogisticsList(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
      }, msg => { this.loading = false })
    },
    showdeliverhandle(row) {
      this.deliver.orderId = row.orderId
      this.showdeliver = true
    },
    showreceivehandle(row) {
      this.receive.logisticsId = row.id
      this.showreceive = true
    },
    createDeliver() {
      CreateDeliver(this.deliver).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.showdeliver = false
      }, (msg) => { })
    },
    createReceive() {
      CreateReceive(this.receive).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.showreceive = false
      }, (msg) => { })
    },
    cleantmp() {
      this.deliver = {
        orderId: null,
        logisticsType: null,
        logisticsNo: '',
        deliveTime: null
      }
      this.receive = {
        logisticsId: null,
        receiveDate: null
      }
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    GetlogisticsType(type) {
      if (type === null) {
        return ''
      } else {
        return this.logisticsType.filter(function(data) {
          return data.key === type
        })[0].name
      }
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
