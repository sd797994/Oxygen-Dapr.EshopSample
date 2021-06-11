<template>
  <div style="height:auto;">
      <iframe src="http://swagger.dapreshop.com:30882/swagger/index.html" style="width:100%;height:100%;" id="bdIframe"></iframe>
  </div>
</template>

<script>
export default {
  mounted() {
    const oIframe = document.getElementById('bdIframe')
    const deviceHeight = document.documentElement.clientHeight
    oIframe.style.height = (Number(deviceHeight)-55) + 'px'
    },
}
</script>
