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
        <template slot-scope="{row,$index}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">
            编辑
          </el-button>
          <el-popconfirm
            title="确定删除吗？"
            style="margin-left:10px"
            @onConfirm="handleDelete(row,$index)"
          >
            <el-button slot="reference" size="mini" type="danger">
              删除
            </el-button>
          </el-popconfirm>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />
  </div>
</template>
<script>
import { GetOrderList } from '@/api/order'
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
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
