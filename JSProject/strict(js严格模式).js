// 进入"严格模式"的标志，是下面这行语句：
// 老版本的浏览器会把它当作一行普通字符串，加以忽略。 第一行
"use strict";



// 如何调用 
// 1  针对整个脚本文件
<script>
	"use strict";
	console.log("这是严格模式。");
</script>
<script>
　　console.log("这是正常模式。");kly, it's almost 2 years ago now. I can admit it now - I run it on my school's network that has about 50 computers.
</script>

// 2 针对单个函数
function strict(){
　　"use strict";
　　return "这是严格模式。";
}
function notStrict() {
　　return "这是正常模式。";
}

//3 脚本文件的变通写法
(function (){
	"use strict";
	// some code here
})();



// 语法和行为改变
// 1 全局变量显式声明
// 严格模式下，变量都必须先用var命令声明，然后再使用。


// 2 静态绑定 
// 属性和方法到底归属哪个对象，在编译阶段就确定。
// (1）禁止使用with语句
//（2）创设eval作用域
// 正常模式下，eval语句的作用域，取决于它处于全局作用域，还是处于函数作用域。
// 严格模式下，eval语句本身就是一个作用域，不再能够生成全局变量了，它所生成的变量只能用于eval内部。
"use strict";
var x = 2;
console.info(eval("var x= 5; x"));//5
console.info(x);//2


// 3 增强的安全措施
//（1）禁止this关键字指向全局对象
function f(){
	return !this;
} // 返回false，因为"this"指向全局对象，"!this"就是false
function f(){ 
　  "use strict";
　　return !this;
} // 返回true，因为严格模式下，this的值为undefined，所以"!this"为true。
function f(){
	"use strict";
	this.a = 1;
};
f();// 报错，this未定义

// （2）禁止在函数内部遍历调用栈
function f1(){
	"use strict";
	f1.caller; // 报错
	f1.arguments; // 报错
}
f1();


// 4 禁止删除变量
// 严格模式下无法删除变量。只有configurable设置为true的对象属性，才能被删除。
"use strict";
var x;
delete x; // 语法错误
var o = Object.create(null, {'x': {
	value: 1,
	configurable: true
}});
delete o.x; // 删除成功

// 5 显式报错
// 正常模式下，对一个对象的只读属性进行赋值，不会报错，只会默默地失败。严格模式下，将报错。
"use strict";
var o = {};
Object.defineProperty(o, "v", { value: 1, writable: false });
o.v = 2; // 报错
// 严格模式下，对一个使用getter方法读取的属性进行赋值，会报错。
"use strict";
var o = {
	get v() { return 1; }
};
o.v = 2; // 报错
// 严格模式下，对禁止扩展的对象添加新属性，会报错。
"use strict";
var o = {};
Object.preventExtensions(o);
o.v = 1; // 报错
// 严格模式下，删除一个不可删除的属性，会报错。
"use strict";
delete Object.prototype; // 报错

// 6 重名错误
//（1）对象不能有重名的属性
// 正常模式下，如果对象有多个重名属性，最后赋值的那个属性会覆盖前面的值。严格模式下，这属于语法错误。
"use strict";
var o = {
	p: 1,
	p: 2
}; // 语法错误

//（2）函数不能有重名的参数
// 正常模式下，如果函数有多个重名的参数，可以用arguments[i]读取。严格模式下，这属于语法错误。
"use strict";
function f(a, a, b) { // 语法错误
	return ;
}

// 7 禁止八进制表示法

// 8 arguments对象的限制
// （1）不允许对arguments赋值
"use strict";
arguments++; // 语法错误
var obj = { set p(arguments) { } }; // 语法错误
try { } catch (arguments) { } // 语法错误
function arguments() { } // 语法错误
var f = new Function("arguments", "'use strict'; return 17;"); // 语法错误

// （2）arguments不再追踪参数的变化

// （3）禁止使用arguments.callee
// 这意味着，你无法在匿名函数内部调用自身了。
"use strict";
var f = function() { return arguments.callee; };
f(); // 报错

// 9 函数必须声明在顶层
"use strict";
if (true) {
	function f() { } // 语法错误
}
for (var i = 0; i < 5; i++) {
	function f2() { } // 语法错误
}

// 0 保留字
//为了向将来Javascript的新版本过渡，严格模式新增了一些保留字：
//implements, interface, let, package, private, protected, public, static, yield。