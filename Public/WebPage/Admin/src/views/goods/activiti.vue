<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        新增活动
      </el-button>
    </div>

    <el-table
      v-loading="loading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column label="活动名" align="center"><template slot-scope="{row}"><span>{{ row.activitieName }}</span></template></el-table-column>
      <el-table-column label="商品名" align="center"><template slot-scope="{row}"><span>{{ row.goodsName }}</span></template></el-table-column>
      <el-table-column label="活动价" align="center"><template slot-scope="{row}"><span v-html="GetPrice(row.price, row.activitiePrice)" /></template></el-table-column>
      <el-table-column label="活动状态" align="center"><template slot-scope="{row}"><span v-html="row.activitieStateInfo" /></template></el-table-column>
      <el-table-column label="活动上/下架" align="center"><template slot-scope="{row}"><span>{{ GetShelf(row.shelfState) }}</span></template></el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">
            编辑
          </el-button>
          <el-button size="mini" type="success" @click="handleModifyStatus(row)">
            {{ row.shelfState === false ? '上架' : '下架' }}
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

    <el-dialog :title="temp.categoryId === null ? '新增活动' : '编辑活动'" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="temp" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="活动名" prop="name">
          <el-input v-model="temp.activitieName" />
        </el-form-item>
        <el-form-item label="活动商品" prop="name">
          <el-autocomplete
            v-model="temp.goodsName"
            :fetch-suggestions="querySearch"
            placeholder="请输入内容"
            :trigger-on-focus="false"
            @select="handleSelect"
          />
        </el-form-item>
        <el-form-item label="活动开始" prop="name">
          <el-date-picker
            v-model="temp.startTime"
            type="datetime"
            placeholder="选择日期时间"
          />
        </el-form-item>
        <el-form-item label="活动结束" prop="name">
          <el-date-picker
            v-model="temp.endTime"
            type="datetime"
            placeholder="选择日期时间"
          />
        </el-form-item>
        <el-form-item label="活动特价" prop="price">
          <el-input-number v-model="temp.activitiePrice" :precision="2" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">
          取消
        </el-button>
        <el-button type="primary" @click="temp.id === null ?createData():updateData()">
          确认
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import { CreateLimitedTimeActivitie, DeleteLimitedTimeActivitie, UpdateLimitedTimeActivitie, GetLimitedTimeActivitieList, UpOrDownShelfActivitie } from '@/api/limitedtimeactivitie'
import { SearchGoods } from '@/api/goods'
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
      countDownTimer: null,
      temp: {
        id: null,
        activitieName: null,
        goodsId: null,
        goodsName: null,
        activitiePrice: null,
        startTime: null,
        endTime: null
      },
      listQuery: {
        page: 1,
        limit: 20
      }
    }
  },
  created() {
    this.getList()
    this.registerCountDownTimer()
  },
  methods: {
    querySearch(queryString, cb) {
      SearchGoods({ goodsName: queryString }).then(response => {
        var result = []
        response.data.forEach(item => {
          result.push({ id: item.id, value: item.goodsName + '(￥' + item.price.toLocaleString() + ')' })
        })
        cb(result)
      }, msg => {})
    },
    GetPrice(price, activitiePrice) {
      return '<s style=\'color:grey\'>￥ ' + price.toLocaleString() + '</s><span style=\'color:red;font:blod\'>￥' + activitiePrice.toLocaleString() + '</span>'
    },
    handleModifyStatus(row) {
      UpOrDownShelfActivitie({ id: row.id, ShelfState: !row.shelfState }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        row.shelfState = !row.shelfState
      }, (msg) => { })
    },
    GetShelf(shelf) {
      if (shelf === true) {
        return '上架'
      }
      return '下架'
    },
    getList() {
      this.loading = true
      GetLimitedTimeActivitieList(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
      }, msg => { this.loading = false })
    },
    handleSelect(item) {
      this.temp.goodsId = item.id
      this.temp.goodsName = item.value
    },
    handleCreate() {
      this.cleantmp()
      this.dialogFormVisible = true
    },
    createData() {
      CreateLimitedTimeActivitie(this.temp).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      }, (msg) => { })
    },
    handleUpdate(row) {
      this.temp = row
      this.dialogFormVisible = true
    },
    updateData() {
      UpdateLimitedTimeActivitie(this.temp).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      }, (msg) => { })
    },
    handleDelete(row, index) {
      DeleteLimitedTimeActivitie({ id: row.id }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.list.splice(index, 1)
      }, (msg) => { })
    },
    cleantmp() {
      this.temp = {
        id: null,
        activitieName: null,
        goodsId: null,
        goodsName: null,
        activitiePrice: null,
        startTime: null,
        endTime: null
      }
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    registerCountDownTimer() {
      setInterval(this.excuteCountDownTimer, 1000)
    },
    excuteCountDownTimer() {
      this.list.forEach(element => {
        if (element.activitieState === 2) {
          element.activitieStateInfo = '<span style=\'color:grey\'>已结束</span>'
        } else if (element.activitieState === 0) {
          var countDownstart = this.getDate(new Date(element.startTime), new Date())
          if (countDownstart.length === 0) {
            element.activitieState = 1
          }
          element.activitieStateInfo = '<span style=\'color:red\'>未开始,' + countDownstart + ' 后开始</span>'
        } else if (element.activitieState === 1) {
          var countDownend = this.getDate(new Date(), new Date(element.endTime))
          if (countDownend.length === 0) {
            element.activitieState = 2
          }
          element.activitieStateInfo = '<span style=\'color:green\'>进行中,' + countDownend + ' 后结束</span>'
        }
      })
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
        result += days + '天'
      }
      if (hours > 0) {
        result += hours + '时'
      }
      if (minutes > 0) {
        result += minutes + '分'
      }
      if (seconds > 0) {
        result += seconds + '秒'
      }
      return result
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
