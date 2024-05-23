# Campus Roaming Experience System based on VR
The project focuses on enhancing virtual campus tours using Unity and panoramic video technology. This system allows users, equipped with VR headsets, to navigate a virtual campus environment without the need for 3D modeling. Users can record and upload panoramic videos of real scenes, which are then integrated into the VR system for immersive exploration.

**System Design and Implementation**:
- The system eschews traditional scene roaming methods, which often trade off between realism and interactivity. It offers a novel approach that combines the photographic realism of videos with the freedom of movement typically associated with modeled environments.
- The project involved complex programming tasks including the calculation of user speed and video playback speeds, as well as the development of a video switching mechanism at intersections within the virtual campus, enhancing the system’s interactivity and user engagement.

**Innovations and Technical Challenges**:
- A significant innovation was the method of intersection turning and video switching, where initial reliance on a VR helmet's hand controller was replaced by head direction tracking. This allowed for more natural interactions, as users simply turn their heads in the direction they wish to explore.
- Challenges included managing the limitations of physical space in VR. The redirection algorithm was introduced to subtly rotate the virtual scene, guiding users to walk in circles in the physical space while perceiving themselves as walking straight in the virtual world. This solution was refined in consultation with a PhD researcher specializing in redirection algorithms.

**Achievements and Impact**:
- The project was recognized for its innovative approach, winning the second prize in the Shandong Province Digital Media Competition and was selected as a provincial innovation and entrepreneurship project.
- It was also awarded the title of College Outstanding Social Practice Team, highlighting its effectiveness and potential for broader applications in fields like education and tourism.

This VR system not only provides a new method for campus tours but also sets a foundation for further research and development in immersive virtual reality experiences.

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









