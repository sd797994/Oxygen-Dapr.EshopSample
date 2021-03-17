<template>
  <div class="app-container">
    <el-form ref="form" :model="mallsetting" label-width="120px">
      <el-form-item label="商铺名">
        <el-input v-model="mallsetting.shopName" style="width:10%" />
      </el-form-item>
      <el-form-item label="一句话描述">
        <el-input v-model="mallsetting.shopDescription" style="width:40%" />
      </el-form-item>
      <el-form-item label="图片" prop="shopIconUrl">
        <img style="width:120px" :src="mallsetting.shopIconUrl">
        <el-upload
          action="#"
          :http-request="onChange"
          accept="image/jpeg"
        >
          <el-button size="small" type="primary">点击上传</el-button>
        </el-upload>
      </el-form-item>
      <el-form-item label="公告">
        <el-input v-model="mallsetting.notice" style="width:100%" />
      </el-form-item>
      <el-form-item label="寄件人">
        <el-input v-model="mallsetting.deliverName" style="width:10%" />
      </el-form-item>
      <el-form-item label="寄件地址">
        <el-input v-model="mallsetting.deliverAddress" style="width:30%" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { GetMallSetting, CreateOrUpdateMallSetting } from '@/api/mallsetting'
import { UploadImage } from '@/api/imageutil'
export default {
  data() {
    return {
      mallsetting: {
        shopName:'',
        shopDescription:'',
        shopIconUrl:'',
        notice:'',
        deliverName: '',
        deliverAddress: ''
      }
    }
  },
  created() {
    GetMallSetting().then(response => {
        this.mallsetting = response.data
      }, msg => { })
  },
  methods: {
    onSubmit() {
      CreateOrUpdateMallSetting(this.mallsetting).then(response => {
        this.$message({
          type: 'info',
          message: '保存成功'
        })
      }, msg => { })
    },
    onChange(data) {
      var _this = this
      var reader = new FileReader()
      reader.onload = function(e) {
        var result = e.target.result
        UploadImage({ base64Body: result }).then(response => {
          debugger
          _this.mallsetting.shopIconUrl = 'http://image.dapreshop.com:30882/' + response.data
        }, msg => {
        })
      }
      reader.readAsDataURL(data.file)
    }
  }
}
</script>

<style scoped>
.line{
  text-align: center;
}
</style>

