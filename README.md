# Unity飞碟游戏
游戏内容要求：
游戏有 n 个 round，每个 round 都包括10 次 trial；
每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
每个 trial 的飞碟有随机性，总体难度随 round 上升；
鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定

而我的制作十分简陋，只有两个回合Round1和Round2.

这次要使用到回收工厂类来对飞碟进行回收，因为这种涉及到类似于子弹啊，雨滴等物体的游戏，这种资源最好能回收与再利用，这样就可以节约资源，省的一直创建新对象，因为一个场景往往涉及到成千上万的这种小物体，如果每一个都要创建一遍才能用，会导致卡顿，速度跟不上，所以对资源回收利用很有必要。

那么先制作游戏对象的预制：

①首先需要制作飞碟（Disk），子弹（Bullet）， 地板（Plane），还有粒子系统（Partical System）

②然后设置Main Camera的position（0，0，-10），Rotation（300， 0， 0）；并添加一个Canvas，包含三个UI元素Text，mainText（显示回合Round），scoreText（显示你的得分），timeText（显示时间）

③然后就可以编写代码了，定义六个类：

<1>飞碟回收工厂类（DiskFactory）：主要实现对飞碟资源的回收利用，包括从工厂获取飞碟，新建飞碟对象等等。
需要注意的是，当且仅当请求队列里的所有对象都在被使用（飞碟在场景中活跃）时，才会发生实例化，此时队列会变长。getDisk返回的是可用飞碟在队列里的index，这是为了方便free（也就是外部获取飞碟）。free通过index找到飞碟在队列中的位置，并将飞碟设置为不活跃的。注意，由于飞碟使用了刚体组件，回收时需要把速度重置，并且大小可能会被改变，也应该重置。

<2>用户界面类（UserInterface）:用户界面有两大功能，一是处理用户键入，二是显示得分和倒计时等。用户键入有两种：鼠标左键(发射射线)和空格（发射飞碟）。显示有三种：得分、回合和倒计时。

<3>场景控制器类（SceneController）：场景控制类主要实现接口定义和保存注入对象。另外它有两个私有变量round和point，分别记录游戏正在进行的回合，以及玩家目前的得分。

<4>关卡类（SceneControllerBC）:关卡类保存了各个round飞碟的大小、颜色、发射位置、发射角度、发射数量等信息。它只有一个函数loadRoundData()，用来初始化游戏场景。

<5>游戏规则类（Judge）:游戏规则单独作为一个类，这里需要处理的规则有两个，得分和失分。另外，得分需要判断是否能晋级下一关。能就调用接口函数nextRound()。

<6>游戏场景类（GameModel）: 场景类主要负责飞碟动作的处理。首先需要倒计时功能，可以通过几个整型变量和布尔变量完成。另外需要飞碟发射功能，通过setting函数保存好飞碟的发射信息，每次倒计时完成后，通过emitDisks获取飞碟对象，并通过发射信息初始化飞碟，再给飞碟一个力就可以发射了。而飞碟的回收在Update里完成，一种是飞碟被击中（飞碟不在场景中）了，需要调用Judge获得分数。另一种是飞碟在场景中，但是掉在地上了，需要调用Judge丢失分数。

另外，要注意的是：
  ①子弹射线的发射，以及飞碟被击中的爆炸效果实现：
  ```
  // 发射子弹  
  //鼠标左键按下
            if (queryInt.isShooting() && Input.GetMouseButtonDown(0) && Time.time > nextFireTime ) {  
                nextFireTime = Time.time + fireRate; 
                  
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // 摄像机到鼠标射线  
                bullet.rigidbody.velocity = Vector3.zero;                       // 子弹刚体速度重置  
                bullet.transform.position= transform.position;                  // 子弹从摄像机位置射出  
                bullet.rigidbody.AddForce(ray.direction*speed, ForceMode.Impulse);  
                //鼠标抓取物体
                RaycastHit hit;  
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Disk") {  //抓中，飞碟爆炸
                    // 播放爆炸粒子特效  
                    explosion.transform.position = hit.collider.gameObject.transform.position;  
                    explosion.renderer.material.color = hit.collider.gameObject.renderer.material.color;  
                    explosion.Play();  
                    // 击中飞碟设置为不活跃，自动回收  
                    hit.collider.gameObject.SetActive(false);  
                }  
            }
  ```
  
  
  ②最后记得给Disk添加一个Tag为“Disk”，以便每次鼠标射线射出去，把对应的Disk“炸掉”。
  
  ③飞碟的回收，建立一个list存储飞碟，当飞碟在场景中活跃时，飞碟不空闲：
  

  ```
  diskList[i].activeInHierarchy //判断飞碟是否活跃
  diskList[id].SetActive(false);//飞碟设置为不活跃，也即回收飞碟
  ```
  
  
  
  
