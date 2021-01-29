<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="dialogFormVisible = true">
        {{ showadd ? '初始化权限' : '重新初始化权限' }}
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
      <el-table-column label="服务名" prop="id" sortable="custom" align="center">
        <template slot-scope="{row}">
          <span>{{ row.serverName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="权限名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.permissionName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="接口地址" align="center">
        <template slot-scope="{row}">
          <span>{{ row.path }}</span>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="getList" />

    <el-dialog title="新增权限" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" label-position="left" label-width="70px" style="margin-left:50px;">
        <el-table
          v-loading="listLoading"
          :data="initlist"
          border
          fit
          highlight-current-row
          style="width: 100%;"
        >
          <el-table-column label="服务名" prop="id" width="180" align="center">
            <template slot-scope="{row}">
              <el-input v-model="row.srvName" />
            </template>
          </el-table-column>
          <el-table-column label="接口名" prop="id" width="180" align="center">
            <template slot-scope="{row}">
              <el-input v-model="row.funcName" />
            </template>
          </el-table-column>
          <el-table-column label="接口地址" prop="id" width="400" align="center">
            <template slot-scope="{row}">
              <el-input v-model="row.path" />
            </template>
          </el-table-column>
          <el-table-column label="操作" align="center" width="100" class-name="small-padding fixed-width">
            <template slot-scope="{row,$index}">
              <el-button size="mini" type="danger" @click="handleDelete(row,$index)">
                删除
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="getinitList()">
          获取
        </el-button>
        <el-button @click="dialogFormVisible = false">
          取消
        </el-button>
        <el-button type="primary" @click="createData()">
          确认
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import { Initpermission, Savepermission, Getpermission } from '@/api/permission'
import Pagination from '@/components/Pagination'

export default {
  name: 'ComplexTable',
  components: { Pagination },
  data() {
    return {
      list: null,
      dialogFormVisible: false,
      initlist: null,
      listLoading: false,
      loading: false,
      total: 0,
      showadd: true,
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
      Getpermission(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
        if (this.list != null && this.list.length > 0) {
          this.showadd = false
        }
      }, msg => { this.loading = false })
    },
    getinitList() {
      this.listLoading = true
      Initpermission().then(response => {
        this.initlist = response.data
        this.listLoading = false
      }, msg => { this.listLoading = false })
    },
    createData() {
      if (this.initlist !== null) {
        var input = []
        this.initlist.forEach(item => {
          input.push({ serverName: item.srvName, permissionName: item.funcName, path: item.path })
        })
        Savepermission(input).then(data => {
          this.$message({
            message: data.message,
            type: 'success'
          })
          this.dialogFormVisible = false
          this.getList()
        }, msg => { })
      } else {
        this.$message({
          message: '请先初始化权限',
          type: 'error'
        })
      }
    },
    handleDelete(row, index) {
      this.initlist.splice(index, 1)
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
