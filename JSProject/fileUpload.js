/* 传统形式 */

<form id="upload-form" action="upload.php" method="post" enctype="multipart/form-data" >
　　<input type="file" id="upload" name="upload" /> <br />
　　<input type="submit" value="Upload" />
</form>

/* ajax上传 */
// 检查是否支持FormData
　　if(window.FormData) {　
　　　　var formData = new FormData();
　　　　// 建立一个upload表单项，值为上传的文件
　　　　formData.append('upload', document.getElementById('upload').files[0]);
　　　　var xhr = new XMLHttpRequest();
　　　　xhr.open('POST', $(this).attr('action'));
　　　　// 定义上传完成后的回调函数
　　　　xhr.onload = function () {
　　　　　　if (xhr.status === 200) {
　　　　　　　　console.log('上传成功');
　　　　　　} else {
　　　　　　　　console.log('出错了');
　　　　　　}
　　　　};
　　　　xhr.send(formData);
　　}

// 进度条
<progress id="uploadprogress" min="0" max="100" value="0">0</progress>
xhr.upload.onprogress = function (event) {
　　　　if (event.lengthComputable) {
　　　　　　var complete = (event.loaded / event.total * 100 | 0);
　　　　　　var progress = document.getElementById('uploadprogress');
　　　　　　progress.value = progress.innerHTML = complete;
　　　　}
　　};

// 图片预览
// 检查是否支持FileReader对象
　　if (typeof FileReader != 'undefined') {
　　　　var acceptedTypes = {
　　　　　　'image/png': true,
　　　　　　'image/jpeg': true,
　　　　　　'image/gif': true
　　　　};
　　　　if (acceptedTypes[document.getElementById('upload').files[0].type] === true) {
　　　　　　var reader = new FileReader();
　　　　　　reader.onload = function (event) {
　　　　　　　　var image = new Image();
　　　　　　　　image.src = event.target.result;
　　　　　　　　image.width = 100;
　　　　　　　　document.body.appendChild(image);
　　　　　　};
　　　　reader.readAsDataURL(document.getElementById('upload').files[0]);
　　　　}
　　}

// 拖放上传
<div id="holder"></div>
#holder {
　　　　border: 10px dashed #ccc;
　　　　width: 300px;
　　　　min-height: 300px;
　　　　margin: 20px auto;
　　}
　　#holder.hover {
　　　　border: 10px dashed #0c0;
　　}

　// 检查浏览器是否支持拖放上传。
　　if('draggable' in document.createElement('span')){
　　　　var holder = document.getElementById('holder');
　　　　holder.ondragover = function () { this.className = 'hover'; return false; };
　　　　holder.ondragend = function () { this.className = ''; return false; };
　　　　holder.ondrop = function (event) {
　　　　　　event.preventDefault();
　　　　　　this.className = '';
　　　　　　var files = event.dataTransfer.files;
　　　　　　// do something with files
　　　　};
　　}