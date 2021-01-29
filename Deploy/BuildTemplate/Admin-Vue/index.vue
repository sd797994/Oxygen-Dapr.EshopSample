<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        新增xxx
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
      <el-table-column label="xxx" align="center">
        <template slot-scope="{row}">
          <span>{{ row.id }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Actions" align="center" width="230" class-name="small-padding fixed-width">
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

    <el-dialog :title="temp.id === null ? '新增xxx' : '编辑xxx'" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="temp" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="xxx" prop="name">
          <el-input v-model="temp.name" />
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
import { Create%placeholder%, Delete%placeholder%, Update%placeholder%, Get%placeholder%List } from '@/api/%placeholder%'
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
      Get%placeholder%List(this.listQuery).then(response => {
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
      Create%placeholder%(this.temp).then(data => {
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
      Update%placeholder%(this.temp).then(data => {
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
      Delete%placeholder%({ id: row.id }).then((data) => {
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
