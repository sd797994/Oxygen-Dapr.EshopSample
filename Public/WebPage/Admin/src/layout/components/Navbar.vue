<template>
  <div class="navbar">
    <hamburger :is-active="sidebar.opened" class="hamburger-container" @toggleClick="toggleSideBar" />

    <breadcrumb class="breadcrumb-container" />

    <div class="right-menu">
      <el-dropdown class="avatar-container" trigger="click">
        <div class="avatar-wrapper">
          <img :src="user.userImage" class="user-avatar">
          <i class="el-icon-caret-bottom" />
        </div>
        <el-dropdown-menu slot="dropdown" class="user-dropdown">
          <router-link to="/">
            <el-dropdown-item>
              首页
            </el-dropdown-item>
          </router-link>
          <el-dropdown-item divided @click.native="showuserdialog">
            <span style="display:block;">编辑个人信息</span>
          </el-dropdown-item>
          <el-dropdown-item divided @click.native="logout">
            <span style="display:block;">登出</span>
          </el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <el-dialog title="编辑个人信息" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="user" label-position="left" label-width="70px" style="width: 400px; margin-left:50px;">
        <el-form-item label="姓名" prop="name">
          <el-input v-model="user.userName" />
        </el-form-item>
        <el-form-item label="头像" prop="name">
          <img :src="user.userImage" style="width:120px">
          <el-upload
            action="#"
            :http-request="onChange"
            accept="image/jpeg"
          >
            <el-button size="small" type="primary">点击上传</el-button>
          </el-upload>
        </el-form-item>
        <el-form-item label="性别" prop="gender">
          <el-select v-model="user.gender" placeholder="请选择性别">
            <el-option v-for="(item,index) in genders" :key="index" :label="item.name" :value="item.key" />
          </el-select>
        </el-form-item>
        <el-form-item label="生日" prop="birthday">
          <el-date-picker v-model="user.birthDay" />
        </el-form-item>
        <el-form-item label="电话" prop="tel">
          <el-input v-model="user.tel" />
        </el-form-item>
        <el-form-item label="地址" prop="address">
          <el-input v-model="user.address" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">
          取消
        </el-button>
        <el-button type="primary" @click="updateuser()">
          确认
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import Breadcrumb from '@/components/Breadcrumb'
import Hamburger from '@/components/Hamburger'
import { UploadImage } from '@/api/imageutil'
export default {
  components: {
    Breadcrumb,
    Hamburger
  },
  data() {
    return {
      dialogFormVisible: false,
      genders: [
        { key: 0, name: '男' },
        { key: 1, name: '女' },
        { key: 2, name: '未知' }
      ]
    }
  },
  computed: {
    ...mapGetters([
      'sidebar',
      'user'
    ])
  },
  methods: {
    toggleSideBar() {
      this.$store.dispatch('app/toggleSideBar')
    },
    async logout() {
      await this.$store.dispatch('user/logout')
      this.$router.push(`/login?redirect=${this.$route.fullPath}`)
    },
    showuserdialog() {
      this.dialogFormVisible = true
    },
    async updateuser() {
      await this.$store.dispatch('user/updateuser')
      this.dialogFormVisible = false
    },
    onChange(data) {
      var _this = this
      var reader = new FileReader()
      reader.onload = function(e) {
        var result = e.target.result
        UploadImage({ base64Body: result }).then(response => {
          _this.user.userImage = 'http://image.dapreshop.com:30882/' + response.data
        }, msg => {
        })
      }
      reader.readAsDataURL(data.file)
    }
  }
}
</script>

<style lang="scss" scoped>
.navbar {
  height: 50px;
  overflow: hidden;
  position: relative;
  background: #fff;
  box-shadow: 0 1px 4px rgba(0,21,41,.08);

  .hamburger-container {
    line-height: 46px;
    height: 100%;
    float: left;
    cursor: pointer;
    transition: background .3s;
    -webkit-tap-highlight-color:transparent;

    &:hover {
      background: rgba(0, 0, 0, .025)
    }
  }

  .breadcrumb-container {
    float: left;
  }

  .right-menu {
    float: right;
    height: 100%;
    line-height: 50px;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-block;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;
      vertical-align: text-bottom;

      &.hover-effect {
        cursor: pointer;
        transition: background .3s;

        &:hover {
          background: rgba(0, 0, 0, .025)
        }
      }
    }

    .avatar-container {
      margin-right: 30px;

      .avatar-wrapper {
        margin-top: 5px;
        position: relative;

        .user-avatar {
          cursor: pointer;
          width: 40px;
          height: 40px;
          border-radius: 10px;
        }

        .el-icon-caret-bottom {
          cursor: pointer;
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }
}
</style>
