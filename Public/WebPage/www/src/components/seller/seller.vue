<template>
	<div class="seller-wrapper" ref="sellerWrapper">
		<div class="seller">
			<div class="top-wrapper white">
				<div class="base">
					<div class="left">
						<h3 class="title">{{ seller.name}}</h3>
						<div class="counts">
							<div class="star-wrapper">
								<star :size="36" :score="seller.score"></star>
							</div>
							<span class="text">({{ seller.ratingCount}})</span>
							<span>月售{{ seller.sellCount}}单</span>
						</div>
					</div>
					<div class="right">
						<span class="icon icon-favorite" :class="{'active': favoriate}" @click="toggleFavoriate"></span>
						<span class="text" :class="{'active': favoriate}">{{favoriateText}}</span>
					</div>
				</div>
				<div class="delivery-wrapper">
					<div class="minPrice item border-right">
						<h5>起送价</h5>
						<div class="notice">{{seller.minPrice}}<span>元</span></div>
					</div>
					<div class="deliveryPrice item border-right">
						<h5>商家配送</h5>
						<div class="notice">{{seller.deliveryPrice}}<span>元</span></div>
					</div>
					<div class="deliveryTime item">
						<h5>平均配送时间</h5>
						<div class="notice">{{seller.deliveryTime}}<span>分钟</span></div>
					</div>
				</div>
			</div>
			<div class="avtivities-wrapper white">
				<h3 class="title">公告与活动</h3>
				<p class="bulletin">{{seller.bulletin}}</p>
				<ul v-if="seller.supports" class="supports">
					<li class="support-item border-top-1px" v-for="(item,index) in seller.supports">
						<span class="icon" :class="classMap[seller.supports[index].type]"></span>
						<span class="text">{{seller.supports[index].description}}</span>
					</li>
				</ul>
			</div>
			<div class="pics-wrapper white">
				<h3 class="title">商家实景</h3>
				<div class="pics-list" v-if="seller.pics" ref="picListWrapper">
					<ul ref="picList">
						<li v-for="(item,index) in seller.pics" class="item">
							<img :src="item" alt="" width="120" height="90">
						</li>
					</ul>
				</div>
			</div>
			<div class="infos-wrapper white">
				<h3 class="title">商家信息</h3>
				<div class="infos" v-if="seller.infos">
					<ul>
						<li v-for="(item,index) in seller.infos">{{item}}</li>
					</ul>
				</div>
			</div>
		</div>
	</div>
</template>

<script type="test/ecmascript-6">
	import {saveToLocal, loadFormLocal} from './../../common/js/util';
	import BScroll from 'better-scroll'
	import star from './../star/star';
	export default{
		name: 'seller',
		props: {
			seller: {
				type: Object
			}
		},
		data() {
			return {
				favoriate: (()=> {
					return loadFormLocal(this.seller.id, 'favoriate', false);
				})(),
				classMap: ['decrease', 'discount', 'special', 'invoice', 'guarantee']
			};
		},
		computed: {
			favoriateText() {
				return this.favoriate ? '已收藏': '收藏'
			}
		},
		watch: {
			'seller'() {
				this.$nextTick( ()=> {
					this._initScroll();
					this._initPicList();
				});
			}
		},
		methods: {
			toggleFavoriate() {
				this.favoriate = !this.favoriate;
				saveToLocal(this.seller.id, 'favoriate', this.favoriate);
			},
			_initScroll() {
				if(!this.scroll) {
					this.scroll = new BScroll(this.$refs.sellerWrapper,{
						click: true
					});
				}else {
					this.scroll.refresh();
				}
			},
			_initPicList() {
				if( this.seller.pics) {
					let picList = this.$refs.picList;
					let length = this.$refs.picList.getElementsByClassName('item').length
					let width = 120;
					let marRgight = 6;
					picList.style.width = (width + marRgight)*length - 6 + 'px';
					if(!this.pics) {
						this.pics = new BScroll(this.$refs.picListWrapper,{
							scrollX: true,
							eventPassthrough: 'vertical'
						});
					}else {
						this.pics.refresh();
					}
				}
			}
		},
		created() {
			
		},
		mounted() {
			this._initScroll();
			this._initPicList();
		},
		components: {
			star
		}
	};
</script>

<style lang="stylus" rel="stylesheet/stylus">
	@import "../../common/stylus/mixin"
	.seller-wrapper
		position: absolute
		top: 174px
		bottom: 0
		width: 100%
		overflow: hidden
		background: #f3f5f7
		.top-wrapper
			padding: 18px 0
			border-bottom-1px(rgba(7,17,27,0.1))
			.base
				display:flex
				justify-content: space-between
				margin: 0 18px
				padding-bottom: 18px
				border-bottom-1px(rgba(7,17,27,0.1))
				.counts
					vertical-align: top
					margin-top: 8px
					font-size: 0
					.star-wrapper
						display: inline-block
						margin-right: 8px
						vertical-align: top
					span
						display: inline-block
						vertical-align: top
						font-size: 10px
						color: rgb(77,85,93)
						&.text
							margin-right: 12px
				.right
					width: 36px
					span
						display: block
						text-align: center
						color: #d4d6d9
						&:first-child
							font-size: 24px
							&.active
								color: rgb(240,20,20)
						&:last-child
							font-size: 10px
							color: #93999f
							&.active
								color: #4d555d

			.delivery-wrapper
				display: flex
				.item
					flex: 1
					margin-top: 18px
					text-align: center
					&.border-right
						border-right: 1px solid rgba(7,17,27,0.1)
					h5
						font-size: 10px
						color: rgb(147,153,159)
					.notice
						margin-top: 5px
						font-size: 24px
						font-weight: 200
						color: rgb(7,17,27)
						span
							font-size: 10px
		.avtivities-wrapper, .pics-wrapper, .infos-wrapper
			margin-top: 16px
			border-top-1px(rgba(7,17,27,0.1))
			border-bottom-1px(rgba(7,17,27,0.1))
		.avtivities-wrapper
			padding: 18px 18px 0;
			.title
				font-size: 14px
				color: rgb(7,17,27)
			.bulletin
				margin: 8px 0 16px
				padding: 0 12px
				line-height: 24px
				font-size: 12px
				font-weight: 200
				text-align: justify
				color: rgb(240,20,20)
			.supports
				li
					border-top-1px(rgba(7,17,27,0.1))
					padding: 16px 12px
					font-size: 12px
					color: rgb(7,17,27)
					.icon
						vertical-align: top
						display: inline-block
						width: 12px
						height: 12px
						margin-right: 4px
						background-size: 12px 12px
						background-repeat: no-repeat
						&.decrease
							bg-image("decrease_4")
						&.discount
							bg-image("discount_4")
						&.guarantee
							bg-image("guarantee_4")
						&.invoice
							bg-image("invoice_4")
						&.special
							bg-image("special_4") 
		.pics-wrapper
			padding: 18px 0 18px 18px;
			.pics-list
				width: 100%;
				margin-top: 12px
				overflow: hidden
				white-space: nowrap
				ul
					font-size: 0
					li
						display: inline-block
						margin-right: 6px
						&:last-child
							margin-right: 0
		.infos-wrapper
			padding: 18px 18px 0 18px;
			.infos
				margin-top: 12px
				li 
					border-top-1px(rgba(7,17,27,0.1))
					line-height: 16px
					padding: 16px 12px
					font-size: 12px
					color: rgb(7,17,27)


</style>
