//"污染"了全局变量，无法保证不与其他模块发生变量名冲突，而且模块成员之间看不出直接关系。
//所有的模块成员都放到这个对象里面。
//但是，这样的写法会暴露所有模块成员，内部状态可以被外部改写。
var module1 = new Object({
	_count : 0,
	m1 : function (){
		//...
	},
	m2 : function (){
		//...
	}
});
module1._count = 5;

//立即执行函数写法
var module1 = (function(){
	var _count = 0;
	var m1 = function(){
		//...
	};
	var m2 = function(){
		//...
	};
	return {
		m1 : m1,
		m2 : m2
	};
})();

console.info(module1._count); //undefined

//放大模式
//上面的代码为module1模块添加了一个新方法m3()，然后返回新的module1模块。
var module1 = (function (mod){
	mod.m3 = function (){
		//...
	};
	return mod;
})(module1);

//宽放大模式
//在浏览器环境中，模块的各个部分通常都是从网上获取的，
//有时无法知道哪个部分会先加载。如果采用上一节的写法，
//第一个执行的部分有可能加载一个不存在空对象，这时就要采用"宽放大模式"。
var module1 = ( function (mod){
		//...
	return mod;
})(window.module1 || {});

//输入全局变量
//独立性是模块的重要特点，模块内部最好不与程序的其他部分直接交互。
//为了在模块内部调用全局变量，必须显式地将其他变量输入模块。
var module1 = (function ($, YAHOO) {
	//...
})(jQuery, YAHOO);

//require.js
//async 属性表明这个文件需要异步加载，避免网页失去响应。
//IE 不支持这个属性，只支持 defer，所以把 defer 也写上。
<script src="js/require.js" defer async="true" ></script>
//data-main 属性的作用是，指定网页程序的主模块。
//在上例中，就是 js 目录下面的 main.js，这个文件会第一个被 require.js 加载。
//由于 require.js 默认的文件后缀名是 js，所以可以把 main.js 简写成 main。
<script src="js/require.js" data-main="js/main"></script>
//下面就来看，怎么写 main.js。
// require ()函数接受两个参数。
// 第一个参数是一个数组，表示所依赖的模块，
// 第二个参数是一个回调函数
// main.js
require (['jquery'， 'underscore'， 'backbone'], function ($， _, Backbone){
// some code here
});

//模块的加载
//使用 require.config ()方法，我们可以对模块的加载行为进行自定义。
//require.config ()就写在主模块（main.js）的头部。
require.config ({
	paths: {
		"jquery": "lib/jquery.min.js",
		"underscore": "lib/underscore.min.js",
		"backbone": "lib/backbone.min.js"
	}
});
//另一种则是直接改变基目录（baseUrl）。
require.config ({
	baseUrl: "js/lib",
	paths: {
		"jquery": "jquery.min.js",
		"underscore": "underscore.min.js",
		"backbone": "backbone.min.js"
	}
});

//require.js 要求，每个模块是一个单独的 js 文件。
//这样的话，如果加载多个模块，就会发出多次 HTTP 请求，会影响网页的加载速度。
// http://requirejs.org/docs/optimization.html 多个模块合并在一个文件中

//math.js // 无依赖
define(function (){
	var add = function(x, y){
		return x+y;
	};
	return { add : add};
});
// 有依赖
define (['myLib'], function (myLib){
	function foo (){
		myLib.doSomething ();
	};
	return {
		foo : foo
	};
});

// main.js 
require(['math.js'], function(math){
	alert(math.add(1, 1));
});


// 加载非规范的模块
// 这样的模块在用 require ()加载之前，要先用 require.config ()方法，定义它们的一些特征。
//underscore 和 backbone 这两个库，都没有采用 AMD 规范编写。
//如果要加载它们的话，必须先定义它们的特征。
//shim 属性，专门用来配置不兼容的模块。具体来说，每个模块要定义
//（1）exports 值（输出的变量名），表明这个模块外部调用时的名称；
//（2）deps 数组，表明该模块的依赖性。

require.config ({
	shim: {
		'underscore':{
			exports: '_'
		},
		'backbone': {
			deps: ['underscore'， 'jquery'],
			exports: 'Backbone'
		}
	}
});
//jQuery 的插件
shim: {
	'jquery.scroll': {
		deps: ['jquery'],
		exports: 'jQuery.fn.scroll'
	}
}