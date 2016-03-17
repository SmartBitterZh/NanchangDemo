/** same property use prototype, it will point to same memory address **/
/** constructor use this define the property **/

function Animal(){
	this.species = "Animal";
}

function Cat(name, color){
	this.name = name;
	this.color = color;
}
Cat.prototype.type = "Cat";
Cat.prototype.eat = function(){ /* eat mouse */  };

/* 

继承 

1, 使用call或apply方法，将父对象的构造函数绑定在子对象上，即在子对象构造函数中加一行
function Cat(name, color){
	Animal.apply(this, arguments);
	this.name = name;
	this.color = color;
}

2,  prototype模式

代码的第一行，我们将Cat的prototype对象指向一个Animal的实例。
它相当于完全删除了prototype 对象原先的值，然后赋予一个新值。
Cat.prototype = new Animal();

任何一个prototype对象都有一个constructor属性，指向它的构造函数。
如果没有"Cat.prototype = new Animal();"这一行，Cat.prototype.constructor是指向Cat的；
Cat.prototype.constructor = Cat;

3, 直接继承prototype
由于Animal对象中，不变的属性都可以直接写入Animal.prototype。
所以，我们也可以让Cat()跳过 Animal()，直接继承Animal.prototype。
这样做的优点是效率比较高（不用执行和建立Animal的实例了），比较省内存。
缺点是 Cat.prototype和Animal.prototype现在指向了同一个对象，那么任何对Cat.prototype的修改，都会反映到Animal.prototype。

function Animal(){ }
Animal.prototype.species = "动物";

Cat.prototype = Animal.prototype;
Cat.prototype.constructor = Cat;

4, 利用空对象作为中介
F是空对象，所以几乎不占内存。这时，修改Cat的prototype对象，就不会影响到Animal的prototype对象

var F = function(){};
F.prototype = Animal.prototype;
Cat.prototype = new F();
Cat.prototype.constructor = Cat;
Cat.uber = Animal.prototype;

意思是为子对象设一个uber属性，这个属性直接指向父对象的prototype属性。
（uber是一个德语词，意思是"向上"、"上一层"。）这等于在子对象上打开一条通道，
可以直接调用父对象的方法。这一行放在这里，只是为了实现继承的完备性，纯属备用性质。

extend(Cat,Animal);
var cat1 = new Cat("大毛","黄色");
alert(cat1.species); // 动物

5, 拷贝继承
如果把父对象的所有属性和方法，拷贝进子对象

extendDeepCopy(Cat, Animal);
var cat1 = new Cat("大毛","黄色");
alert(cat1.species);


非构造函数的继承
把父对象的属性，全部拷贝给子对象，也能实现继承。
如果父对象的属性等于数组或另一个对象，那么实际上，
子对象获得的只是一个内存地址，而不是真正拷贝，因此存在父对象被篡改的可能。


var Chinese = {
　　　　nation:'中国'
　　};
Chinese.birthPlaces = ['北京','上海','香港'];

var Doctor ={
　　　　career:'医生'
　　}


var Doctor = object(Chinese);
var Doctor = extendEasyCopy(Chinese);

*/


function extend(Child, Parent){
	var FNull = function(){};
	FNull.prototype = Parent.prototype;
	Child.prototype = new FNull();
	Child.prototype.constructor = Child;
	Child.uber = Parent.prototype;
}

function extendCopy(Child, Parent){
	var p = Parent.prototype;
	var c = Child.prototype;
	for (var i in p) {
		c[i]  = p[i];
	}
	c.uber = p;
}

function object(Parent){
	function FNull(){}
	FNull.prototype = Parent;
	return new FNull();
}

function extendEasyCopy(Parent){
	var c = {};
	for (var i in Parent) {
		c[i] = Parent[i];
	};
	c.uber = Parent;
	return c;
}

function extendDeepCopy(Child, Parent){
	var c = Child || {};
	for (var i in Parent) {
			if(typeof Parent[i] === 'object'){
				c[i] = (Parent[i].constructor === Array)? [] : {} ;
				extendDeepCopy(c[i], Parent[i]);
			}else{
				c[i] = Parent[i];
			}
		};	
}
