<template>
  <div class="app-container">
    <el-form ref="form" :model="mallsetting" label-width="120px">
      <el-form-item label="寄件人">
        <el-input v-model="mallsetting.deliverName" />
      </el-form-item>
      <el-form-item label="寄件地址">
        <el-input v-model="mallsetting.deliverAddress" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">保存</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import { GetMallSetting, CreateOrUpdateMallSetting } from '@/api/mallsetting'
export default {
  data() {
    return {
      mallsetting: {
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
    }
  }
}
</script>

<style scoped>
.line{
  text-align: center;
}
</style>

