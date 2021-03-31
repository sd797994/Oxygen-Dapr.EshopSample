<template>
  <div class="app">
    <v-header :seller="seller"></v-header>
    <div class="tab border-bottom-1px">
      <div class="tab-item">
        <router-link to="/goods"></router-link>
      </div>
      <!-- <div class="tab-item">
        <router-link to="/ratings">评论</router-link>
      </div>
      <div class="tab-item">
        <router-link to="/seller">商家</router-link>
      </div> -->
    </div>
    <keep-alive>
      <router-view :seller="seller"></router-view>
    </keep-alive>
  </div>
</template>

<script type="text/ecmascript-6">
import {urlParse} from './common/js/util';
import header from './components/header/header';
export default {
  name: 'app',
  data () {
    return {
      seller: {
        id: (() => {
          let queryParam = urlParse();
          return queryParam.id;
        })()
      }
    };
  },
  components: {
    'v-header': header
  },
  mounted () {
    this.$http.post('http://api.dapreshop.com:30882/publicservice/mallsettingquery/GetMallSetting').then((response) => {
       var data = response.body.data;
       this.seller = data;
    });
  }
};
</script>

<style lang="stylus" rel="stylesheet/stylus">
  @import "./common/stylus/mixin.styl"
  .app
    .tab  
      display: flex
      width: 100%
      height: 40px
      line-height: 40px
      border-bottom-1px(rgba(7,17,27,0.1))
      .tab-item
        flex: 1
        text-align: center
        & > a
          display: block
          font-size: 14px
          color: rgb(77,85,93)
          &.active
            color: rgb(240,20,20)
</style>
