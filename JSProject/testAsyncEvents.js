
	var events = (function () {
		var topics = {};
		return {
			subscribe : function (topic, listener) {
				// if not exist, create a topic
				if(!topics[topic]) 
					topics[topic] = { queue : [] };
				// push the listener into topic
				var index = topics[topic].queue.push(listener);
				// provide remove topic handle
				return (function (topic, index) {
					return {
						remove : function () {
							delete topics[topic].queue[index];
						}
					}
				})(topic, index);
			},
			publish : function (topic, info) {
				if(!topics[topic] || !topics[topic].queue.length) return; 
				var items = topics[topic].queue;
				items.forEach(function (item) {
					item(info || {});
				});
			}
		};
	})();