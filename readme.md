# Campus Roaming Experience System based on VR
Used Unity and panorama video to build a virtual campus roaming system without 3D modeling, so that the user wearing a VR headset can walk freely in the scene.

Users only need to use the panoramic camera to record the video of the scene, and upload it to the system, then they can roam around the place.

A whole new interaction method for the current applications and systems on the market.

Compared with the traditional campus promotional video, this system is more active, immersive and realistic, and also greatly increases the ease of use, which can be put into other fields such as children's education and tourism in the future.

Details can be viewed here： https://1141283192.wixsite.com/zqgong/campusvr

### 管理端（web）

**功能**

* 平台拥有**账户**管理系统（登陆注册）
* 每个账户名下可设多张**图**
* 用户选中图后可对图的节点、路路径信息进行编辑，并上传对应的视频到服务器

目前后端接口已实现



### 用户端（本地unity项目）

**场景**

* login

  该场景中

  * 用户可以查看平台内所有公司（每家公司对应对应管理端中的一个**账户**）
  * 选择一家公司后，用户将看到该公司名下所有的**图**
  * 选择其中一张图，将加载场景**VRCampus**，进入该图内漫游。

* VRCampus



### 









