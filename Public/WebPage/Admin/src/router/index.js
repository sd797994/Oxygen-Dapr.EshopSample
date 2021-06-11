import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      name: 'Dashboard',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '首页', icon: 'dashboard' }
    }]
  },
  {
    path: '/rbac',
    component: Layout,
    name: 'RBAC',
    meta: { title: '用户角色权限', icon: 'el-icon-s-help' },
    hidden: false,
    children: [
      {
        path: 'permission',
        name: 'Permission',
        component: () => import('@/views/permission/index'),
        meta: { title: '权限管理', icon: 'table' },
        hidden: false
      },
      {
        path: 'role',
        name: 'Role',
        component: () => import('@/views/role/index'),
        meta: { title: '角色管理', icon: 'tree' },
        hidden: false
      },
      {
        path: 'account',
        name: 'Account',
        component: () => import('@/views/account/index'),
        meta: { title: '用户管理', icon: 'user' },
        hidden: false
      }
    ]
  },
  {
    path: '/goodsmanager',
    component: Layout,
    name: 'Goods',
    meta: { title: '商品管理', icon: 'el-icon-present' }, //
    hidden: false,
    children: [
      {
        path: 'goodscategory',
        name: 'Goodscategory',
        component: () => import('@/views/goods/category'),
        meta: { title: '商品类别管理', icon: 'el-icon-files' },
        hidden: false
      },
      {
        path: 'goods',
        name: 'Goods',
        component: () => import('@/views/goods/index'),
        meta: { title: '商品管理', icon: 'el-icon-present' },
        hidden: false
      },
      {
        path: 'activiti',
        name: 'Activiti',
        component: () => import('@/views/goods/activiti'),
        meta: { title: '限时活动', icon: 'el-icon-alarm-clock' },
        hidden: false
      }
    ]
  },
  {
    path: '/trade',
    component: Layout,
    name: 'Trade',
    meta: { title: '交易管理', icon: 'el-icon-box' },
    hidden: false,
    children: [
      {
        path: 'order',
        name: 'Order',
        component: () => import('@/views/order/index'),
        meta: { title: '订单管理', icon: 'el-icon-s-order' },
        hidden: false
      },
      {
        path: 'logistics',
        name: 'Logistics',
        component: () => import('@/views/order/logistics'),
        meta: { title: '物流配送', icon: 'el-icon-truck' },
        hidden: false
      }
    ]
  },
  {
    path: '/public',
    component: Layout,
    name: 'Public',
    meta: { title: '基础设置', icon: 'el-icon-setting' },
    hidden: false,
    children: [
      {
        path: 'mallsetting',
        name: 'Mallsetting',
        component: () => import('@/views/public/mallsetting'),
        meta: { title: '商城设置', icon: 'el-icon-s-tools' },
        hidden: false
      }, {
        path: 'swaggerDocument',
        name: 'swaggerDocument',
        component: () => import('@/views/public/swaggerdocument'),
        meta: { title: 'swagger文档', icon: 'el-icon-document' },
        hidden: false
      }, {
        path: 'sentinelSetting',
        name: 'sentinelSetting',
        component: () => import('@/views/public/sentinelsetting'),
        meta: { title: '服务保护配置', icon: 'el-icon-edit-outline' },
        hidden: false
      }, {
        path: 'eventhandleerrorinfo',
        name: 'Eventhandleerrorinfo',
        component: () => import('@/views/public/eventhandleerrorinfo'),
        meta: { title: '事件异常统计', icon: 'el-icon-s-opportunity' },
        hidden: false
      }
    ]
  },
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: getrouter()
})

const router = createRouter()
export function getrouter() {
  return constantRoutes
}
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}
export function setpermissionbyuser(data, userpermission) {
  if (data.length > 1) {
    data.forEach(v => {
      userpermission.forEach(permission => {
        if (permission.path === v.path) {
          v.hidden = permission.hidden
        }
      })
      if (Object.prototype.hasOwnProperty.call(v, 'children') && v.children.length) {
        setpermissionbyuser(v.children, userpermission)
      }
    })
  } else {
    userpermission.forEach(permission => {
      if (permission.path === data.path) {
        data.hidden = permission.hidden
      }
      if (Object.prototype.hasOwnProperty.call(data, 'children') && data.children.length) {
        setpermissionbyuser(data.children, userpermission)
      }
    })
  }
}
export default router
