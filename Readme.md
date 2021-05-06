# Dapr-Eshop

Dapr-Eshop是一款由C#编写、运行在k8s上以dapr作为服务网格组件支撑的分布式电商系统Demo，旨在让.net开发者通过该demo快速学习并上手dapr,相关系列博文：
https://www.cnblogs.com/gmmy/p/14606109.html

## 环境依赖

windows平台：

​	docker for windows on kubernetes 1.19+

​	helm3

​	dapr  --version 1.0

linux平台：

​	docker for linux

​	kubernetes 1.19+

​	helm3

​	dapr --version1.0

## 安装手册

首先确保本机已经安装好相关云原生依赖组件,kubernetes除基础套件外需要单独安装ingress-controller,默认推荐ingress-nginx：https://kubernetes.github.io/ingress-nginx/  请注意默认ingress-controller并未暴露NodePort，我由于demo中使用了30882这个端口，所以需要修改service将nodeport暴露：

```
kubectl edit svc ingress-nginx-controller
```

修改type: NodePort 以及nodePort: 30882

下面开始正式安装运行服务：

```shell
git clone https://github.com/sd797994/Oxygen-Dapr.EshopSample.git
cd Deploy
./RunBasic.bat #下载基础设施,含项目使用的数据库、dapr用于状态和事件管理的中间件及网关和链路追踪组件
cd Release
./BuildImages.bat #构建服务镜像,由于是第一次运行，需要拉取基础镜像以及npm install包，可能会较慢
./Deploy.bat #运行
```

在本地打开Host文件将以下内容拷贝并保存

```
127.0.0.1 admin.dapreshop.com #后端管理页面
127.0.0.1 m.dapreshop.com #M站页面
127.0.0.1 api.dapreshop.com #网关
127.0.0.1 image.dapreshop.com #图片服务器
127.0.0.1 zipkin.dapreshop.com #链路追踪
```

查询kubectl get po -n dapreshop 观察所有po都running后即可通过浏览器访问 admin.dapreshop.com:30882、m.dapreshop.com:30882

Enjoy

## 项目结构说明

简要拓扑图（仅含服务间简易依赖关系)

![image-20210331143423979](https://user-images.githubusercontent.com/26075482/113103072-688c2580-9231-11eb-82ae-285fa3153638.png)

### 服务节点及分层设计简述

------

1、网关：一个简单的nginx镜像，主要目的是将外网请求通过重写路由匹配成dapr需要的内网调用路由规则并转发给自己的daprd边车，再由边车发送给对应的服务节点

2、图片及作业服务：简单的单层web服务器、其中作业服务主要使用了hangfire

3、其他业务服务

业务服务采用领域驱动设计清洁模型分层、每个小项目会分为应用层、领域层、基础设施层。关于领域驱动设计和清洁模型可参考文章：
https://www.jdon.com/ddd.html  
https://www.jdon.com/artichect/the-clean-architecture.html

应用层包含查询服务、用例服务、事件订阅器。领域层包含聚合(根、实体、值对象)、事件、规约、仓储抽象。基础设施层包含仓储实现、orm及其他基础支撑。其中领域层和基础设施层会依赖于Public/Base 下的领域服务base和基础设施base，主要是公共部分的抽象和封装

### 功能说明

------

整个项目主要的逻辑是通过管理端创建权限、角色、用户来登录和管理后台系统，拥有相关权限的操作人员可以访问商品分类、商品、活动、订单、物流以及商城基本设置等页面进行商品or特价活动创建、订单管理、物流收发货以及商城基本设置管理。前端M站可以浏览商品、加入购物车并下单

如果想体验之前博客园系列文章提到的限流、oauth等等中间件功能，可以查看Oxygen-Dapr.EshopSample\Deploy\middleware 文件夹并根据之前文章内容进行使用

### 其他依赖

------

管理端页面fork：https://github.com/PanJiaChen/vue-element-admin/tree/i18n

M站页面fork: https://github.com/JerryYgh/m-eleme

## License

MIT
