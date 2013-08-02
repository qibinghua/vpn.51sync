<?php
class MobileMenu extends Base {
	public $menuImageCount = 2;
	public function index() {
	}
	public function postMenu() {
		$data ['uid'] = $this->user ['uid'];
		$data ['name'] = $this->requestPost ( 'menuName' );
		$data ['intro'] = $this->requestPost ( 'menuDesc' );
		//处理菜谱图片
		for ($i=1;$i<=$this->menuImageCount;$i++){
			$image = $this->requestFile($_FILES['menuImg'.$i],false,'menu');
			if($image){
				$menuImage[] = $image;
			}
		}
		$data ['image'] = json_encode($menuImage);
		// 步骤
		
		$data ['steps'] = json_decode($this->requestPost ( 'steps' )); 
		$len = count($data);
		//处理步骤图片
		for ($i=1;$i<=$len;$i++){
			$stepImage[] = $this->requestFile($_FILES['stepImg'.$i],false,'step');
		}
		foreach ($data ['steps'] as $k=>$step){
			$step->stepImg = (string) $stepImage[$k];
			$data ['steps'][$k] = $step;
		}
		$data['steps'] = json_encode($data['steps']);
		
		
		
		$data ['classid'] = $this->requestPost ( 'type' ); // 菜谱类别
		$data ['tasteid'] = $this->requestPost ( 'taste' ); // 口味id
		$data ['foodstuff_main'] = $this->requestPost ( 'mains' ); // 主料
		$data ['foodstuff_sub'] = $this->requestPost ( 'subs' ); // 辅料
		
		$data ['state'] = 1; // 菜谱状态
		//$data ['mkdate'] = strtotime ( $this->requestPost ( 'date' ) );
		$data['mkdate'] = time();
		if ($data ['mkdate'] > time ()) {
			echo $this->responseStateMsg ( STATE_ERROR, '制作日期不合法' );
		} elseif (empty ( $data ['steps'] )) {
			echo $this->responseStateMsg ( STATE_ERROR, '制作步骤不能为空' );
		} elseif (empty ( $data ['foodstuff_main'] )) {
			echo $this->responseStateMsg ( STATE_ERROR, '制作主料不能为空' );
		} elseif (empty ( $data ['classid'] )) {
			echo $this->responseStateMsg ( STATE_ERROR, '请选择菜谱类别' );
		} elseif (empty ( $data ['image'] )) {
			echo $this->responseStateMsg ( STATE_ERROR, '请上传一张菜谱图片' );
		} elseif (empty ( $data ['name'] )) {
			echo $this->responseStateMsg ( STATE_ERROR, '请填写菜谱名称' );
		} else {
			
			$id = $this->db->insert ( 'cookbook', $data );
			
			if ($id) {
				echo $this->responseStateMsg ( STATE_SUCCESS, '提交菜谱成功', array (
						'menuId' => $id 
				) );
			} else {
				echo $this->responseStateMsg ( STATE_SUCCESS, '提交菜谱失败' );
			}
		}
	}
	/**
	 * 上传食谱repo
	 */
	public function postRepo() {
		$data ['uid'] = $this->user ['uid'];
		$data ['fsid'] = (int)$this->requestPost ( 'menuId' ); // 菜单id
		$sql = "select uid from cookbook where id='".$data ['fsid']."' limit 1";
		$rs = $this->db->fetchOne($sql);
		$data['fsuid'] = $rs['uid'];
		if($data['fsuid']){
			$data ['intro'] = $this->requestPost ( 'repoDesc' ); // 心得
			$data ['image'] = $this->requestFile ( $_FILES['repoImg'] ); // repo自己做的图片
			$data ['state'] = 3; // repo状态
			$data ['mkdate'] = time ();
			if (empty ( $data ['fsid'] )) {
				echo $this->responseStateMsg ( STATE_ERROR, '请选择一个菜谱' );
			} elseif (empty ( $data ['intro'] )) {
				echo $this->responseStateMsg ( STATE_ERROR, '请填写您的心得' );
			} elseif (empty ( $data ['image'] )) {
				echo $this->responseStateMsg ( STATE_ERROR, '请上传一张REPO图片' );
			} else {
				$id = $this->db->insert ( 'repo', $data );
				if ($id) {
					echo $this->responseStateMsg ( STATE_SUCCESS, '提交Repo成功', array (
							'repoId' => $id 
					) );
					//给食谱访问量+1
			$sql = "update cookbook set count_repo=count_repo+1 where id='".$data ['fsid']."'";
			$this->db->execute($sql);
				} else {
					echo $this->responseStateMsg ( STATE_SUCCESS, '提交Repo失败' );
				}
			}
		}else{
			echo $this->responseStateMsg ( STATE_SUCCESS, '提交Repo失败,菜谱不存在' );
		}
	}

    /**
     * 食谱列表
     */
    public function menuLists(){
        $page = (int)$this->requestPost('pageIndex');
        $limit = (int)$this->requestPost('pageSize');
        $page || $page=1;
        $limit || $limit =10;
        $offset = ($page-1)*$limit;
        $cond = '1';
        if(!$this->emptyUid()){
        	$uid = $this->user['uid'];
        	$cond = "uid='".$uid."'";
        }
        $sql = "select id as menuId,classid,tagid,tasteid,name as menuName,image as menuImg,count_repo from cookbook where ".$cond." limit ".$offset.",".$limit;
echo $sql;
exit;
        $rs = $this->db->fetch($sql);
        foreach ($rs as $k=>$menu){
        	$menu['menuImg'] = json_decode($menu['menuImg']);
        	$rs[$k]=$menu;
        }
        //echo $this->responseStateMsg ( STATE_SUCCESS, '',array('menus'=>$rs) );
    }

    /**
     * 删除食谱,将会删除所有的评论和repo
     */
    public function deleteMenu(){
        //$uid = $this->requestPost('uid');
        $uid = $this->user['uid'];
        $menuId  = (int)$this->requestPost('menuId');
        //删除菜谱
        $sql = "delete from cookbook where id='".$menuId."' and uid='".$uid."' limit 1";
        $this->db->execute($sql);
        //删除repo
        $sql = "delete from repo where fsid='".$menuId."'";
        $this->db->execute($sql);
        //删除评论
        $sql = "delete from comment where fsid='".$menuId."'";
        $this->db->execute($sql);
        echo $this->responseStateMsg ( STATE_SUCCESS, '删除菜谱成功' );
    }

    /**
     * 食谱详情
     */
    public function menuInfo(){
        $menuId  = (int)$this->requestPost('menuId');
        //获取食谱详细信息
        $sql ="select user.uid as userId,user.face as makerHead, user.nick as makerName,mkdate as makeDate, name as menuName,id as menuId,image as menuImg,intro as menuDesc,
        count_repo as repoNum,count_fav as favedNum,foodstuff_main as mains,foodstuff_sub as subs,steps from cookbook left join
        user on cookbook.uid = user.uid where cookbook.id='".$menuId."'";
        $menu = $this->db->fetchOne($sql);
        $menu['menuImg'] = json_decode($menu['menuImg']);
        $menu['mains'] = json_decode($menu['mains']);
        $menu['subs'] = json_decode($menu['subs']);
        $menu['steps'] = json_decode($menu['steps']);
        $sql = "select cmt.id,cmt.content,cmt.mkdate as `date`,u.uid as userId,u.nick as userName,u.face as userHead from comment cmt left join user u 
        		on cmt.uid=u.uid where cmt.fsid='".$menuId."' order by cmt.id desc limit 2";
        $menu['comments']=array();
        $menu['comments'] = $this->db->fetch($sql);
        $sql = "select repo.rid as repoId,repo.mkdate as repoDate,repo.image as repoImg,
        		repo.intro as repoDesc,user.face as repoHead,user.nick as repoMaker from repo left join user 
        		on repo.uid=user.uid where repo.state=1 and repo.fsid='".$menuId."' limit 10";
        $menu['repos']=array();
        $menu['repos'] = $this->db->fetch($sql);
        if(!$this->emptyUid()){
        	$uid = $this->user['uid'];
	        //是否收藏
	        $sql = "select id from favorite where uid='".$uid."' and item='".$menuId."' and item_type='m' limit 1";
	        $frs = $this->db->fetchOne($sql);
	        if($frs['id']>0){
	        	$menu['isCollected']=2;
	        }else{
	        	$menu['isCollected']=1;
	        }
	        //是否加入购物车
	        $sql = "select id from cart where uid='".$uid."' and fsid='".$menuId."' limit 1";
	        $frs = $this->db->fetchOne($sql);
	        if($frs['id']>0){
	        	$menu['isAddList']=2;
	        }else{
	        	$menu['isAddList']=1;
	        }
        }
        echo $this->responseStateMsg ( STATE_SUCCESS, '食谱详情获取成功',$menu );
		//给食谱访问量+1
		$sql = "update cookbook set count_view=count_view+1 where id='".$menuId."'";
		$this->db->execute($sql);
		$log = Base::getModel('log');
		$uid = (int)$menu['userId'];
		$log->logDay($uid,'view',1);

    }

    /**
     * 食谱repo列表
     */
    public function menuRepos(){
        $menuId  = (int)$this->requestPost('menuId');
        $page = (int)$this->requestPost('pageIndex');
        $limit = (int)$this->requestPost('pageSize');
        $page || $page=1;
        $limit || $limit =10;
        $offset = ($page-1)*$limit;
        $sql = "select r.rid as repoId,r.image as repoImg,r.intro as repoDesc,r.mkdate as `date`,u.face as headImg,u.nick as userName 
        		from repo r left join user u on r.uid=u.uid where r.state=1 and r.fsid='".$menuId."' limit ".$offset.",".$limit;
        
        $rs['repos'] = $this->db->fetch($sql);
        echo $this->responseStateMsg ( STATE_SUCCESS, '',$rs );
    }

    /**
     * 食谱评论列表
     */
    public function menuComments(){
        $menuId  = (int)$this->requestPost('menuId');
        $page = (int)$this->requestPost('pageIndex');
        $limit = (int)$this->requestPost('pageSize');
        $page|| $page=1;
        $limit || $limit =10;
        $offset = ($page-1)*$limit;
        $sql = "select cmt.id,cmt.content,cmt.mkdate as `date`,u.uid as userId,u.nick as userName,u.face as userImg from comment as cmt left join user as u
         on cmt.uid=u.uid where cmt.fsid='".$menuId."' limit ".$offset.",".$limit;

        $rs['comments'] = $this->db->fetch($sql);
        echo $this->responseStateMsg ( STATE_SUCCESS, '',$rs );
    }

    /**
     * 发表评论
      */
    public function postComment(){
        $menuId  = (int)$this->requestPost('menuId');
        $uid = $this->user['uid'];
        $content = $this->requestPost('content');
        if(!empty($content)){
            $data['content'] = $content;
            $data['uid'] = $uid;
            $data['nick'] = $this->user['nick'];
            $data['mkdate'] = time();
            $data['fsid'] = $menuId;
            $id = $this->db->insert('comment',$data);
            if($id){
                echo $this->responseStateMsg ( STATE_SUCCESS, '发表评论成功',array('cmtid'=>$id) );
                //给食谱评论+1
                $sql = "update cookbook set count_comment=count_comment+1 where id='".$menuId."'";
                $this->db->execute($sql);
            }else{
                echo $this->responseStateMsg ( STATE_ERROR, '发表评论失败' );
            }
        }else{
        	echo $this->responseStateMsg ( STATE_ERROR, '评论内容不能为空' );
        }

    }
}