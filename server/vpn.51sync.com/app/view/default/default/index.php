<?php $this->_extends('_layouts/default_layout'); ?>

<?php $this->_block('contents'); ?>
    <div class="global_msg">建议大家使用openvpn来或手动使用l2tp连接，这样加密程度会比较高，为什么这么做，你懂得。<br>注册后可以获取测试帐号测试速度。</div>
    <p>本站提供的虚拟专用网络服务使用L2TP、PPTP、OPENVPN方式，无须安装任何软件就可以方便的在Windows、Linux、Mac、Android、iPhone、iPad等平台上使用。</p>
	<p>OPENVPN使用的证书下载后,解压缩后放到openvpn客户端的安装目录下的config目录中,然后打开openvpn客户端输入用户名密码连接。<br>
		<a target="_blank" href="http://openvpn.se/files/install_packages/openvpn-2.0.9-gui-1.0.3-install.exe">[<u>openvpn客户端下载</u>]</a><br>
		<a target="_blank" href="client.zip">[<u>openvpn证书下载</u>]</a><br>
		<a target="_blank" href="http://vpn.fjut.edu.cn/howto/win7.html">[<u>windows vpn 设置教程</u>]</a><br>
        <a target="_blank" href="vpnclient.zip"><span class="approve">[<u>本站的vpn连接客户端</u>]</span></a> <b>下载后解压点击setup.exe安装</b><br>
        </p>
	<p>如果异常断开，请过一分钟再连接就可以连上，iphone锁屏自动关vpn是苹果的设置问题，属于正常现象，详情百度。
	</p>
    <p>本站所使用服务器线路均为电信和网通直连线路，稳定，速度快。并且，为保证服务质量，严格限制用户数目。</p>
    <p><b>本站仅支持支付宝，可以选择担保交易或即时交易，如需实时开通账号，请选择即时交易。</b></p>
    <p>禁止做违反香港地区法律的任何事情</p>
    <?php if(!$uid):?>
    <p><input onclick="location.href='<?=url('default::/signup');?>';" class="meta-button" value="申请帐户" type="button" /></p>
    <?php endif;?>
<h2>计划列表</h2>
<table class="datalist" border="0" cellspacing="0" cellpadding="0" width="100%">
    <tr>
        
        <th width="200">产品名称</th>
        <th width="150">月付(RMB)</th>
        <th width="150">季付(RMB)</th>
        <th width="150">半年付(RMB)</th>
        <th width="150">年付(RMB)</th>
        <th width="*">计划内容</th>
    </tr>
    <?php if($rs->count()):foreach($rs as $row): ?>
    <?php $row['sku']=json_decode($row['sku']);$an= "semi-annually";?>
    <tr>
        <td><?=$row['name']?></td>
        <td><?=$row['sku']->monthly->price?></td>
        <td><?=$row['sku']->quarterly->price?></td>
        <td><?=$row['sku']->$an->price?></td>
        <td><?=$row['sku']->annually->price?></td>
        <td><?=$row->desc()?></td>
    </tr>
    <?php endforeach;?>
    <?php else:?>
    <tr>
        <td colspan="4" >无内容</td>
    </tr>
    <?php endif;?>
</table>
<?php $this->_endblock('contents'); ?>
