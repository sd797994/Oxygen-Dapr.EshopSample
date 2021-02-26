<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">
        新增商品
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
      <el-table-column label="商品名" align="center">
        <template slot-scope="{row}">
          <span>{{ row.goodsName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="图片" prop="name">
        <template slot-scope="{row}">
          <img style="width:120px" :src="row.goodsImage">
        </template>
      </el-table-column>
      <el-table-column label="商品分类" align="center">
        <template slot-scope="{row}">
          <span>{{ row.categoryName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="上架状态" align="center">
        <template slot-scope="{row}">
          <span>{{ GetShelf(row.shelfState) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="库存" align="center">
        <template slot-scope="{row}">
          <span>{{ row.stock }}</span>
        </template>
      </el-table-column>
      <el-table-column label="价格" align="center">
        <template slot-scope="{row}">
          <span>{{ GetPrice(row.price) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="330" class-name="small-padding fixed-width">
        <template slot-scope="{row,$index}">
          <el-button type="primary" size="mini" @click="handleUpdate(row)">
            编辑
          </el-button>
          <el-button type="primary" size="mini" @click="OpenChangeStock(row)">
            编辑库存
          </el-button>
          <el-button size="mini" type="success" @click="handleModifyStatus(row)">
            {{ row.shelfState === false ? '上架' : '下架' }}
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

    <el-dialog :title="temp.id === null ? '新增商品' : '编辑商品'" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="temp" label-position="left" label-width="90px" style="width: 400px; margin-left:50px;">
        <el-form-item label="商品名" prop="goodsName">
          <el-input v-model="temp.goodsName" />
        </el-form-item>
        <el-form-item label="图片" prop="goodsImage">
          <img style="width:120px" :src="temp.goodsImage">
          <el-upload
            action="#"
            :http-request="onChange"
            accept="image/jpeg"
          >
            <el-button size="small" type="primary">点击上传</el-button>
          </el-upload>
        </el-form-item>
        <el-form-item label="商品类型">
          <el-select v-model="temp.categoryId" placeholder="请选择">
            <el-option
              v-for="item in categorys"
              :key="item.id"
              :label="item.categoryName"
              :value="item.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item v-show="temp.id === null" label="商品库存" prop="stock">
          <el-input-number v-model="temp.stock" />
        </el-form-item>
        <el-form-item label="商品价格" prop="price">
          <el-input-number v-model="temp.price" :precision="2" />
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
import { CreateGoods, DeleteGoods, UpdateGoods, GetGoodsList, GetAllCategoryList, UpOrDownShelfGoods, UpdateGoodsStock } from '@/api/goods'
import { UploadImage } from '@/api/imageutil'
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
      categorys: [],
      temp: {
        id: null,
        categoryId: null,
        categoryName: null,
        goodsImage: null,
        goodsName: null,
        shelfState: null,
        stock: null,
        price: null
      },
      listQuery: {
        page: 1,
        limit: 20
      }
    }
  },
  created() {
    this.getList()
    this.GetAllCategoryList()
  },
  methods: {
    OpenChangeStock(row) {
      this.$prompt('请输入库存', '提示', {
        closeOnClickModal: false,
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        inputValue: row.stock
      }).then(({ value }) => {
        UpdateGoodsStock({ goodsId: row.id, deductionStock: value }).then(response => {
          this.$message({
            type: 'success',
            message: response.message
          })
          row.stock = value
        }, msg => { })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '取消输入'
        })
      })
    },
    GetAllCategoryList() {
      GetAllCategoryList().then(response => {
        this.categorys = response.data
      }, msg => {})
    },
    GetShelf(shelf) {
      if (shelf === true) {
        return '上架'
      }
      return '下架'
    },
    GetPrice(price) {
      return '￥ ' + price.toLocaleString()
    },
    getList() {
      this.loading = true
      GetGoodsList(this.listQuery).then(response => {
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
      CreateGoods(this.temp).then(data => {
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
      UpdateGoods(this.temp).then(data => {
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
      DeleteGoods({ id: row.id }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        this.list.splice(index, 1)
      }, (msg) => { })
    },
    handleModifyStatus(row) {
      UpOrDownShelfGoods({ id: row.id, ShelfState: !row.shelfState }).then((data) => {
        this.$message({
          message: data.message,
          type: 'success'
        })
        row.shelfState = !row.shelfState
      }, (msg) => { })
    },
    cleantmp() {
      this.temp = {
        id: null,
        categoryId: null,
        categoryName: null,
        goodsImage: null,
        goodsName: null,
        shelfState: null,
        stock: null,
        price: null
      }
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    onChange(data) {
      var _this = this
      var reader = new FileReader()
      reader.onload = function(e) {
        var result = e.target.result
        UploadImage({ base64Body: result }).then(response => {
          _this.temp.goodsImage = 'http://image.dapreshop.com:30882/' + response.data
        }, msg => {
        })
      }
      reader.readAsDataURL(data.file)
    }
  }
}
</script>
<style lang="scss" scoped>
.filter-container {
   margin-bottom: 20px
}
</style>
