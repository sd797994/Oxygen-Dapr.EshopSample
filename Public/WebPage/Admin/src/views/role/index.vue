<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        新增角色
      </el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column label="角色名" prop="id" sortable="custom" align="center">
        <template slot-scope="{row}">
          <span>{{ row.roleName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="超级管理员" align="center">
        <template slot-scope="{row}">
          <span>{{ getissuper(row.superAdmin) }}</span>
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

    <el-dialog :title="temp.id === null ? '新增角色' : '编辑角色'" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="temp" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="角色名" prop="name">
          <el-input v-model="temp.roleName" />
        </el-form-item>
        <el-form-item label="超级管理员" prop="name">
          <el-checkbox v-model="temp.superAdmin" />
        </el-form-item>
        <el-form-item label="权限">
          <el-tree
            ref="tree"
            :data="permission"
            :props="defaultProps"
            :default-expand-all="true"
            show-checkbox
            node-key="id"
            class="permission-tree"
          />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">
          取消
        </el-button>
        <el-button type="primary" @click="temp.roleId === null ?createData():updateData()">
          确认
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import { Getrolelist, RoleCreate, RoleUpdate, RoleDelete } from '@/api/role'
import { GetAllPermissions } from '@/api/permission'
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
      temp: {
        roleId: null,
        roleName: null,
        superAdmin: null,
        permissions: []
      },
      defaultProps: {
        children: 'child',
        label: 'permissionName'
      },
      permission: null,
      listQuery: {
        page: 1,
        limit: 20
      }
    }
  },
  created() {
    this.getList()
    this.initPermission()
  },
  methods: {
    initPermission() {
      GetAllPermissions().then(response => {
        this.permission = response.data
      })
    },
    getissuper(issuper) {
      if (issuper === true) {
        return '是'
      }
      return '否'
    },
    getList() {
      this.loading = true
      Getrolelist(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.loading = false
        if (this.list != null && this.list.length > 0) {
          this.showadd = false
        }
      }, msg => { this.loading = false })
    },
    handleCreate() {
      this.cleantmp()
      this.$nextTick(() => {
        this.$refs.tree.setCheckedKeys([])
      })
      this.dialogFormVisible = true
    },
    createData() {
      this.temp.permissions = this.$refs.tree.getCheckedKeys()
      RoleCreate(this.temp).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      })
    },
    handleUpdate(row) {
      this.temp.roleId = row.roleId
      this.temp.roleName = row.roleName
      this.temp.superAdmin = row.superAdmin
      this.temp.permissions = row.permissions
      this.$nextTick(() => {
        this.$refs.tree.setCheckedKeys(this.temp.permissions)
      })
      this.dialogFormVisible = true
    },
    updateData() {
      this.temp.permissions = this.$refs.tree.getCheckedKeys()
      RoleUpdate(this.temp).then(data => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      })
    },
    handleDelete(row, index) {
      RoleDelete({ roleId: row.roleId }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.list.splice(index, 1)
      }, (msg) => { })
    },
    cleantmp() {
      this.temp = {
        roleId: null,
        roleName: null,
        superAdmin: null,
        permissions: []
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
