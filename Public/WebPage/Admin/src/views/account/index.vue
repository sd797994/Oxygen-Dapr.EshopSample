<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        新增用户
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
      <el-table-column label="登录名" prop="id" sortable="custom" align="center">
        <template slot-scope="{row}">
          <span>{{ row.loginName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户昵称" align="center">
        <template slot-scope="{row}">
          <span>{{ row.nickName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="{row}">
          <span>{{ getstate(row.state) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="真实姓名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.userName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="性别" align="center">
        <template slot-scope="{row}">
          <span>{{ getgender(row.gender) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="生日" align="center">
        <template slot-scope="{row}">
          <span>{{ getbirth(row.birthDay) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">
            编辑
          </el-button>
          <el-button size="mini" type="success" @click="handleModifyStatus(row)">
            {{ row.state === 0 ? '锁定' : '解锁' }}
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

    <el-dialog :title="temp.id === null ? '新增用户' : '编辑用户'" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="70px" style="width: 400px; margin-left:50px;">
        <el-form-item label="登录名" prop="name">
          <el-input v-model="temp.loginName" />
        </el-form-item>
        <el-form-item label="密码" prop="name">
          <el-input v-model="temp.password" type="password" />
        </el-form-item>
        <el-form-item label="昵称" prop="name">
          <el-input v-model="temp.nickName" />
        </el-form-item>
        <el-form-item label="姓名" prop="name">
          <el-input v-model="temp.user.userName" />
        </el-form-item>
        <el-form-item label="性别" prop="gender">
          <el-select v-model="temp.user.gender" placeholder="请选择性别">
            <el-option v-for="(item,index) in genders" :key="index" :label="item.name" :value="item.key" />
          </el-select>
        </el-form-item>
        <el-form-item label="生日" prop="birthday">
          <el-date-picker v-model="temp.user.birthDay" />
        </el-form-item>
        <el-form-item label="电话" prop="tel">
          <el-input v-model="temp.user.tel" />
        </el-form-item>
        <el-form-item label="地址" prop="address">
          <el-input v-model="temp.user.address" />
        </el-form-item>
        <el-form-item label="角色" prop="address">
          <el-checkbox-group v-model="temp.roles">
            <el-checkbox v-for="role in allroles" :key="role.roleId" :label="role.roleId">{{ role.roleName }}</el-checkbox>
          </el-checkbox-group>
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
import { fetchList, lockaccount, accountCreate, accountDelete, accountUpdate } from '@/api/account'
import { GetAllRoles } from '@/api/role'
import Pagination from '@/components/Pagination' // secondary package based on el-pagination
const dateFormat = require('dateformat')

export default {
  name: 'ComplexTable',
  components: { Pagination },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 20,
        importance: undefined,
        title: undefined,
        type: undefined,
        sort: '+id'
      },
      importanceOptions: [1, 2, 3],
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        id: null,
        loginName: '',
        password: '',
        nickName: '',
        roles: [],
        user: {
          userName: '',
          gender: 2,
          address: '',
          tel: '',
          birthDay: null
        }
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        type: [{ required: true, message: 'type is required', trigger: 'change' }],
        timestamp: [{ type: 'date', required: true, message: 'timestamp is required', trigger: 'change' }],
        title: [{ required: true, message: 'title is required', trigger: 'blur' }]
      },
      downloadLoading: false,
      genders: [
        { key: 0, name: '男' },
        { key: 1, name: '女' },
        { key: 2, name: '未知' }
      ],
      allroles: []
    }
  },
  created() {
    this.getList()
    this.getallroles()
  },
  methods: {
    getallroles() {
      GetAllRoles().then(response => {
        this.allroles = response.data
      })
    },
    getbirth(date) {
      if (date === null || date === undefined) {
        return ''
      } else {
        return dateFormat(new Date(), 'yyyy年mm月dd日')
      }
    },
    getstate(state) {
      switch (state) {
        case 0:
          return '正常'
        case 1:
          return '锁定'
        default:
          return ''
      }
    },
    getgender(gender) {
      switch (gender) {
        case 0:
          return '男'
        case 1:
          return '女'
        case 2:
          return '未知'
        default:
          return ''
      }
    },
    getList() {
      this.listLoading = true
      fetchList(this.listQuery).then(response => {
        this.list = response.data.pageData
        this.total = response.data.pageTotal
        this.listLoading = false
      }, msg => {})
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleModifyStatus(row) {
      lockaccount(row).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        row.state = row.state === 1 ? 0 : 1
      }, (msg) => { })
    },
    sortChange(data) {
      const { prop, order } = data
      if (prop === 'id') {
        this.sortByID(order)
      }
    },
    sortByID(order) {
      if (order === 'ascending') {
        this.listQuery.sort = '+id'
      } else {
        this.listQuery.sort = '-id'
      }
      this.handleFilter()
    },
    handleCreate() {
      this.cleantmp()
      this.dialogFormVisible = true
    },
    createData() {
      accountCreate(this.temp).then((data) => {
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
      this.dialogFormVisible = true
      this.temp.id = row.id
      this.temp.loginName = row.loginName
      this.temp.nickName = row.nickName
      this.temp.user.userName = row.userName
      this.temp.user.gender = row.gender
      this.temp.user.address = row.address
      this.temp.user.tel = row.tel
      this.temp.user.birthDay = row.birthDay
      this.temp.roles = []
      row.roles.forEach(item => {
        this.temp.roles.push(item.roleId)
      })
    },
    updateData() {
      accountUpdate(this.temp).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.cleantmp()
        this.handleFilter()
        this.dialogFormVisible = false
      }, msg => { })
    },
    handleDelete(row, index) {
      accountDelete({ accountId: row.id }).then((data) => {
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
        loginName: '',
        password: '',
        nickName: '',
        roles: [],
        user: {
          userName: '',
          gender: 2,
          address: '',
          tel: '',
          birthDay: null
        }
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
