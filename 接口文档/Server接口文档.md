[TOC]

# Server接口文档

| 修订版本号 | 修订日期 | 修订人 | 修改内容                          |
| ---------- | -------- | ------ | --------------------------------- |
| 1.1        | 2021.8.5 | 李卓栋 |                                   |
| 1.2        | 2021.8.6 | 李卓栋 | 上传视频接口；<br />厘清实体/关系 |

## 一、基本配置

- 测试使用的`ServerURL`:`http:// 1.116.155.101/`（暂时未确定）

- 前端向服务器发送的数据的`Content-Type`为`application/x-www-form-urlencoded`;如包含图片，发送的数据的`Content-Type`为`multipart/form-data`

- 所有服务器返回数据均为`json`（即服务器返回数据的Content-Type为`application/json`），统一格式如下：

  ```
  {
      "code": 0,
      "message": "success!!",
      "data": ...
  }
  ```

- 若本次请求正常，则返回`code`为0，`msg`为“success”；小于0代表错误, 大于0代表非错误性提示。**任何请求都会返回code和message**，下方接口数据中的**传出参数为data中的讯息**，某些接口没有data数据

- 登录时后端会返回token，之后的操作需要使用token，前端将token放于请求头"**Token**"进行请求,下一次无需重复登录

  示例：

  ```
  {
      "Token": "bdsnf37r43592hb"
  }
  ```

- **统一的错误码**

| 错误码 | 信息                   |
| ------ | ---------------------- |
| 0      | success！！            |
| -1     | 服务器错误             |
| 601    | 登录已过期，请重新登录 |
| 602    | 请求字段不能为空       |

## 二、实体与关系

### 实体

*节点*

### Node

| 字段名            | 类型    | 备注                           |
| ----------------- | ------- | ------------------------------ |
| id                | int     | 节点序号                       |
| name              | varchar | 节点名称（例如饭堂门口、操场） |
| （待定）longitude | decimal | 经度                           |
| （待定）latidude  | decimal | 纬度                           |

#### PathData

| 字段名 | 类型  | 备注                                                         |
| ------ | ----- | ------------------------------------------------------------ |
| length | float | 路径长度[^1](https://gitee.com/crazyMessi/vrcampus18/blob/master/接口文档/路径的长度即其对应的视频的时长) |
| start  | int   | 起始节点                                                     |
| end    | int   | 终点                                                         |
| vUrl   | text  | 路径对应的视频的路径                                         |

#### Graph

| 字段名    | 类型   | 备注                 |
| --------- | ------ | -------------------- |
| graphId   | long   | 图id（系统自动生成） |
| graphName | String | 用户为图设置的名称   |

### 关系

#### user_graph

用户id与图id

#### graph_node

图id与节点id



## 接口

### 1、账号管理(/user)

#### 1.1注册

| URL       | method |
| --------- | ------ |
| /register | POST   |

| 传入参数 | 类型   | 是否可为空 | 说明 |
| -------- | ------ | ---------- | ---- |
| userName | String | 否         |      |
| password | String | 否         |      |

| 错误码 | 信息         |
| ------ | ------------ |
| 1      | 用户名已注册 |

| 传出参数 | 类型 | 说明                 |
| -------- | ---- | -------------------- |
| userId   | long | 用户id，需要用户牢记 |



```json
{
    "code": 0,
    "message": null,
    "data": {
        "userId": 3,""
        "userName": "test",
        "password": null
    }
}
```





#### 1.2登录

| URL    | method |
| ------ | ------ |
| /login | POST   |

| 传入参数 | 类型   | 是否可为空 | 说明     |
| -------- | ------ | ---------- | -------- |
| userId   | long   | 否         | 用户账号 |
| password | String | 否         | 密码     |

| 错误码 | 信息           |
| ------ | -------------- |
| 100    | 账号或密码错误 |

| 传出参数 | 类型   | 说明  |
| -------- | ------ | ----- |
| token    | String | token |

```json
{
    "code": 0,
    "message": null,
    "data": {
        "token": "VXauOkMT5g+8LMWcs08oPaCTn2RAk0McHtgtMQ3lCWNIFheIiKMLMUwaZHy0siaV"
    }
}
```





### 2、图管理(/GraphManager)

**2.1创建图**

| **URL**      | method |
| ------------ | ------ |
| /createGraph | POST   |



| 传入参数 | 类型   | 是否可为空 | 说明 |
| -------- | ------ | ---------- | ---- |
| name     | string | 否         | 图名 |

| 错误码 | 信息 |
| ------ | ---- |
| 1      |      |

```
{
    "code": 0,
    "message": "success!!"
}
```







**2.2 查看图列表**

| **URL**       | method |
| ------------- | ------ |
| /viewMyGraphs | /Get   |



| 错误码 | 信息 |
| ------ | ---- |
| 1      |      |

| 传出参数 | 类型    | 说明   |
| -------- | ------- | ------ |
| myGraphs | Graph[] | 图数组 |

实例：

```
{
    "code": 0,
    "message": null,
    "data": [
        {
            "graphId": 1,
            "graphName": "test_graph"
        },
        {
            "graphId": 2,
            "graphName": "test_graph"
        },
        {
            "graphId": 3,
            "graphName": "test_graph3"
        }
    ]
}
```





**2.3 删除图**

//TODO

**2.4 查看(选中)图**

| **URL**     | method |
| ----------- | ------ |
| /choseGraph | /Get   |

| 传入参数 | 类型 | 是否可为空 | 说明 |
| -------- | ---- | ---------- | ---- |
| id       |      |            |      |

| 错误码 | 信息 |
| ------ | ---- |
| 1      |      |

| 传出参数 | 类型       | 说明             |
| -------- | ---------- | ---------------- |
| nodes    | Node[]     | 本图中的所有节点 |
| pathData | PathData[] | 本图中所有路径   |

*示例*

```json
{
    "code": 0,
    "message": null,
    "data": {
        "nodes": [
            {
                "id": 0,
                "name": null,
                "longitude": 0.0,
                "latitude": 0.0
            },
            {
                "id": 0,
                "name": null,
                "longitude": 0.0,
                "latitude": 0.0
            },
            {
                "id": 0,
                "name": null,
                "longitude": 0.0,
                "latitude": 0.0
            },
            {
                "id": 0,
                "name": null,
                "longitude": 0.0,
                "latitude": 0.0
            },
            {
                "id": 0,
                "name": null,
                "longitude": 0.0,
                "latitude": 0.0
            }
        ],
        "pathData": [
            {
                "start": 0,
                "end": 0,
                "length": 0.0,
                "vurl": null
            },
            {
                "start": 0,
                "end": 0,
                "length": 0.0,
                "vurl": null
            },
            {
                "start": 0,
                "end": 0,
                "length": 0.0,
                "vurl": null
            },
            {
                "start": 0,
                "end": 0,
                "length": 0.0,
                "vurl": null
            },
            {
                "start": 0,
                "end": 0,
                "length": 0.0,
                "vurl": null
            }
        ]
    }
}
```





在调用（方法ChoseGraph之后，服务器会记住/更新当前正在编辑的图，此后的操作将在该图上进行）

### 3、图编辑(/GraphEdit)

**3.1 增加/编辑节点**

| **URL**  | method |
| -------- | ------ |
| /addNode | POST   |

| 传入参数  | 类型   | 是否可为空 | 说明                                   |
| --------- | ------ | ---------- | -------------------------------------- |
| Name      | String | 是         | 节点名称                               |
| longitude | double | 是         | 经度                                   |
| latitude  | double | 是         | 纬度                                   |
| nodeId    | int    | 是         | **节点id，若为空则创建；若非空则编辑** |

| 错误码 | 信息 |
| ------ | ---- |
|        |      |

| 传出参数 | 类型 | 说明                         |
| -------- | ---- | ---------------------------- |
| node     | Node | 节点完整信息（插入或编辑后） |

```
{
    "code": 0,
    "message": null,
    "data": {
        "id": 1,
        "name": "科研楼背面",
        "longitude": 20.0,
        "latitude": 100.0
    }
}
```

**3.2添加路径** 

| **URL**   | method |
| --------- | ------ |
| /addVideo | POST   |

| 传入参数   | 类型 | 是否可为空 | 说明                             |
| ---------- | ---- | ---------- | -------------------------------- |
| video_file | file | 是         | 视频文件。视频长度将由该文件定义 |
| start      | int  | 否         | 视频起点序号                     |
| end        | int  | 否         | 视频中终点序号                   |
| length     | int  | 是         | 视频文件长度                     |







