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
      <el-table-column label="订阅器名称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.handlerName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="异常详情" align="center">
        <template slot-scope="{row}">
          <span>{{ row.errMessage }}</span>
        </template>
      </el-table-column>
      <el-table-column label="堆栈信息" align="center">
        <template slot-scope="{row}">
          <a style="color:rgb(64, 158, 255)" @click="openstarckContent(row.errStackTrace)">查看堆栈信息</a>
        </template>
      </el-table-column>
      <el-table-column label="是否系统异常" align="center">
        <template slot-scope="{row}">
          <span>{{ row.isSystemErr === true ?'是':'否' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="原始事件" align="center">
        <template slot-scope="{row}">
          <a style="color:rgb(64, 158, 255)" @click="openContent(row.eventData)">查看原始事件</a>
        </template>
      </el-table-column>
      <el-table-column label="记录时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.logDate }}</span>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />
    <el-dialog title="查看堆栈信息" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <p style="white-space: pre-line;">{{stackContent}}</p>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">
          确定
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import { GetEventHandleErrorInfoList } from '@/api/eventhandleerrorinfo'
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
      },
      stackContent: ''
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.loading = true
      GetEventHandleErrorInfoList(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
      }, msg => { this.loading = false })
    },
    handleCreate() {
      this.cleantmp()
      this.dialogFormVisible = true
    },
    createData() {
      CreateEventHandleErrorInfo(this.temp).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      }, (msg) => { })
    },
    openstarckContent(contnet){
      this.stackContent = contnet
      this.dialogFormVisible = true
    },
    openContent (content) {
      this.$alert(content, '', {
          confirmButtonText: '确定'
      })
    },
    handleUpdate(row) {
      this.temp = row
      this.dialogFormVisible = true
    },
    updateData() {
      UpdateEventHandleErrorInfo(this.temp).then(data => {
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
      DeleteEventHandleErrorInfo({ id: row.id }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.list.splice(index, 1)
      }, (msg) => { })
    },
    cleantmp() {
      this.temp = {
        categoryId: null,
        categoryName: null,
        sort: null
      }
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
