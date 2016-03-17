/* 
constructor 比较复杂，用到了this和prototype，编写和阅读都很费力 
*/

function Cat(){
	this.name = "大毛";
}

var cat1 = new Cat();

/* 
Object.create() 
这种方法比"构造函数法"简单，
但是不能实现私有属性和私有方法，
实例对象之间也不能共享数据，对"类"的模拟不够全面。
*/
if (!Object.create) {
	Object.create = function (o) {
		function F() {}
		F.prototype = o;
		return new F();
	};
}

var Cat = {
	name: "大毛",
	makeSound: function(){ alert("喵喵喵"); }
};

var cat2 = Object.create(Cat);

/* 极简主义法 
这种方法的好处是，容易理解，结构清晰优雅，符合传统的"面向对象编程"的构造
*/
// 封装
var Cat = {
	createNew = function(){
		var cat = {};
		cat.name = "大毛";
		cat.makeSound = function(){ alert("喵喵喵"); };
		return cat;
	}
};
var cat1 = Cat.createNew();

// 继承
var Animal = {
	createNew: function(){
		var animal = {};
		animal.sleep = function(){ alert("睡懒觉"); };
		return animal;
	}
};

var Cat = {
	createNew = function(){
		var cat = Animal.createNew();
		cat.name = "大毛";
		cat.makeSound = function(){ alert("喵喵喵"); };
		return cat;
	}
};
// 私有属性和私有方法, 只要不是定义在cat对象上的方法和属性，都是私有的。
// 数据共享
var Cat = {
　　　　sound : "喵喵喵",
　　　　createNew: function(){
　　　　　　var cat = {};
　　　　　　cat.makeSound = function(){ alert(Cat.sound); };
　　　　　　cat.changeSound = function(x){ Cat.sound = x; };
　　　　　　return cat;
　　　　}
　　};