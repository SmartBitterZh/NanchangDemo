<?php
$type = $_REQUEST['stype'];
switch($type){
    case 'push' :  
    echo 'history.pushState(data, title [, url])：往历史记录堆栈顶部添加一条记录；data会在onpopstate事件触发时作为参数传递过去；title为页面标题，当前所有浏览器都会 忽略此参数；url为页面地址，可选，缺省为';
    break;
    case 'replace' :  
    echo 'history.replaceState(data, title [, url]) ：更改当前的历史记录，参数同上';
    break;
    case 'onpop' :  
    echo 'window.onpopstate：响应pushState或replaceState的调用；';
    break;
    }
?>