// alert(1);

// require.config({
// 	paths : {
// 		jquery : 'lib/jquery-2.2.1.min',
// 		math : 'math'
// 	},
//	baseUrl: 'lib'
// });

// require(['jquery', 'math'], function ($, math) {
//   	alert(math.add(1, 1));
// });

require(['math'], function (math) {
  	alert(math.add(1, 1));
});