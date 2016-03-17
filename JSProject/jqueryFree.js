

var common = (function (){
	var toArray = function (nodeList){
		return Array.prototype.slice.call(myNodeList);
	};
	var append = function (parent, child){
		parent.appendChild(child);
	};
	var prepend = function (parent, child){
		parent.insertBefore(child, parent.childNodes[0]);
	};
	var remove = function (child){
		child.parentNode.removeChild(child);
	};

	return { 
		$ : document.querySelectorAll.bind(document),
		toArray : toArray,
		append : append,
		prepend : prepend,
		remove : remove,
		addClass : addClass
	};
})();

Element.prototype.on = Element.prototype.addEventListener;
NodeList.prototype.on = function (event, fn) {
	[]['forEach'].call(this, function (el) {
		el.on(event, fn);
	});
	return this;
};

Element.prototype.trigger = function (type, data) {
	var event = document.createEvent('HIMLEvents');
	event.initEvent(type, true, true);
	event.data = data || {};
	event.eventName = type;
	event.target = this;
	this.dispatchEvent(event);
	return this;
};
NodeList.prototype.trigger = function (event) {
	[]['forEach'].call(this, function (el) {
		el['trigger'](event);
	});
	return this;
};