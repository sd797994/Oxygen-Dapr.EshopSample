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
      <el-table-column label="明细" align="center">
        <template slot-scope="{row}">
          <div
            v-html="GetDetial(row.orderItems)"
          />
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template slot-scope="{row}">
          <span>{{ GetOrderStateName(row.orderState) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单总价" align="center">
        <template slot-scope="{row}">
          <span>{{ GetPrice(row.totalPrice) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="下单人" align="center">
        <template slot-scope="{row}">
          <span>{{ row.userName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="下单时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.createTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="showlog(row.orderId)">
            交易记录
          </el-button>
          <el-button v-show="row.orderState === 0" type="primary" size="mini" @click="mockPay(row)">
            模拟支付
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />
    <el-dialog title="交易记录" :visible.sync="dialogFormVisible">
      <el-timeline>
        <el-timeline-item
          v-for="(tradelog, index) in tradelogs"
          :key="index"
          type="primary"
          color="#0bbd87"
          :timestamp="GetTradeDateStr(tradelog.tradeDate)"
        >
          {{ tradelog.tradeLogValue }}
        </el-timeline-item>
      </el-timeline>
    </el-dialog>
  </div>
</template>
<script>
import { GetOrderList, OrderPay } from '@/api/order'
import { GetTradeLogListByOrderId } from '@/api/tradelog'
import { parseTime } from '@/utils/index.js'
import Pagination from '@/components/Pagination'

export default {
  name: 'ComplexTable',
  components: { Pagination },
  data() {
    return {
      list: null,
      dialogFormVisible: false,
      loading: false,
      total: 0,
      temp: {
        id: null
      },
      tradelogs: [],
      listQuery: {
        page: 1,
        limit: 20
      }
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.loading = true
      GetOrderList(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
      }, msg => { this.loading = false })
    },
    GetPrice(price) {
      return '￥ ' + price.toLocaleString()
    },
    GetOrderStateName(orderState) {
      switch (orderState) {
        case -1:
          return '订单取消'
        case 0:
          return '待支付'
        case 1:
          return '待发货'
        case 2:
          return '已完成'
      }
      return ''
    },
    GetDetial(orderItems) {
      var str = ''
      if (orderItems.length !== 0) {
        orderItems.forEach(item => {
          str += '<span style=\'float:left\'>' + item.goodsName + ' X' + item.count + '</span><span style=\'float:right\'>' + this.GetPrice(item.price) + '</span><div style=\'clear:both\'></div>'
        })
      }
      return str
    },
    showlog(orderId) {
      this.tradelogs = []
      GetTradeLogListByOrderId({ orderId: orderId }).then(response => {
        this.tradelogs = response.data
        this.dialogFormVisible = true
      }, msg => { })
    },
    GetTradeDateStr(datestr) {
      return this.getDate(new Date(datestr), new Date())
    },
    getDate(start, end) {
      var span = end.getTime() - start.getTime()
      var days = Math.floor(span / (24 * 3600 * 1000))
      var leave1 = span % (24 * 3600 * 1000)
      var hours = Math.floor(leave1 / (3600 * 1000))
      var leave2 = leave1 % (3600 * 1000)
      var minutes = Math.floor(leave2 / (60 * 1000))
      var leave3 = leave2 % (60 * 1000)
      var seconds = Math.round(leave3 / 1000)
      var result = ''
      if (days > 0) {
        if (days > 30) {
          result = parseTime(start, '{y}-{m}-{d} {h}:{i}')
        } else {
          result = days + '天前'
        }
      } else if (hours > 0) {
        result = hours + '小时前'
      } else if (minutes > 0) {
        result = minutes + '分种前'
      } else if (seconds > 0) {
        result = '刚刚'
      }
      return result
    },
    mockPay(order) {
      OrderPay({ orderId: order.orderId }).then(response => {
        this.$message({
          type: 'success',
          message: response.message
        })
        order.orderState = 1
      }, msg => { })
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
