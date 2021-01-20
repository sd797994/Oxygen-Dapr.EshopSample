<template>
  <div class="dashboard-container">
    <div class="dashboard-text">hi : {{ name }}~</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { resetRouterByUser } from '@/router'
export default {
  name: 'Dashboard',
  computed: {
    ...mapGetters([
      'name',
      'gender'
    ])
  },
  created() {
    resetRouterByUser()
  }
}
</script>

<style lang="scss" scoped>
.dashboard {
  &-container {
    margin: 30px;
  }
  &-text {
    font-size: 30px;
    line-height: 46px;
  }
}
</style>
