// 到指定的分页页面
function toPage(page) {
	var form = document.forms[0];
	form.page.value = page;
	form.submit();
}

// 控制输入的页码为整数
function inputIntNumberCheck() {
	var theEvent = window.event || arguments.callee.caller.arguments[0];
	var elm;
	var ver = navigator.appVersion;
	if (ver.indexOf("MSIE") != -1) { // IE
		if (!((theEvent.keyCode >= 48) && (theEvent.keyCode <= 57)
				|| (theEvent.keyCode == 8) || (theEvent.keyCode == 46))) {
			theEvent.keyCode = 0;
		}
	} else { // Netscape
		if (!((theEvent.which >= 48) && (theEvent.which <= 57)
				|| (theEvent.which == 8) || (theEvent.which == 0))) {
			theEvent.stopPropagation();
			theEvent.preventDefault();
		}
	}
}

// 验证是否已选择操作记录
function validateSelect(items) {
	if (items.length) {
		for (var i = 0; i < items.length; i++) {
			if (items[i].checked)
				return true;
		}
	} else {
		if (items.checked)
			return true;
	}
	return false;
}

// 全选或全部取消选择
function selectAll(all, items) {
	var status = all.checked;
	if (items.length) {
		for (var i = 0; i < items.length; i++) {
			if (!items[i].disabled)
				items[i].checked = status;
		}
	} else {
		if (!items.disabled)
			items.checked = status;
	}
}

// 触发批量删除操作
function deleteSelected(url) {
	var items = document.forms[0].ids;
	if (validateSelect(items)) {
		var form = document.forms[0];
		form.action = url;
		form.submit();
	} else {
		alert("请选择要删除的记录!");
	}
}
		
// 提交分页请求之前判断跳转的页码是否已超出最大页码数
function confirm(totalPage){
	var form = document.forms[0];
		if(form.page.value > totalPage){
			form.page.value = totalPage;
		}
	return true;
}