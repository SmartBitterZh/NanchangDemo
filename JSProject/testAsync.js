(function () {
	function showResult(info) {
		document.getElementById('show').innerHTML = info;
	};
	// 1 call back
	// var add = function (x, y, callback) {		
	// 	setTimeout(function () {
	// 		var result = x + y;
	// 		callback(result);
	// 	}, 5000);
	// };
	// add(1,1, showResult);


	// 2 no ok for event
	// var add = function (x, y) {
	// 	setTimeout(function () {
	// 		var result = x + y;
	// 		add.trigger('done', [result]);
	// 	});
	// };
	// add.addEventListener('done', showResult);
	// add(1,1);


	// 3 Observer  jquery plugin Tiny Pub/Sub
	// jQuery.subscribe("done", showResult);
	// var add = function (x, y) {
	// 	setTimeout(function () {
	// 		var result = x + y;
	// 		jQuery.publish("done");
	// 	}, 5000);
	// };
	// jQuery.unsubscribe("done", showResult);
	// add(1,1);

	// var subscription = events.subscribe('show', showResult);
	// var add = function (x, y) {
	// 	setTimeout(function () {
	// 		var result = x + y;

	// 		events.publish('show', result);

	// 		subscription.remove();
	// 	}, 5000);
	// };
	// add(1,1);


	// 4 Promise
	// var timeout = function (ms) {
	// 	return new Promise((resolve, reject) => {
	// 		setTimeout(resolve, ms, 'done')
	// 	});
	// }
	// // five second show done
	// timeout(5000).then((value) => {
	//   showResult(value);
	// });

	// 5 Generator function add * and use next to call next yield value(stop and run)
	// yield will not run until call it.
	
	// function* helloWorldGenerator () {
	// 	yield 'hello';
	// 	yield 'world';
	// 	return 'ending';
	// }

	// var hw = helloWorldGenerator();
	// var result = hw.next();
	// alert(result.value+result.done);

	function* gen () {
		yield 123 + 456;
	}
})();
