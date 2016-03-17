/* require module */
var server = require("./server");
var router = require("./router");
var requestHandlers = require("./requestHandlers");

/* init route */
var handle = {}
handle["/"] = requestHandlers.startPost;
handle["/start"] = requestHandlers.startPost;
handle["/upload"] = requestHandlers.upload;
handle["/show"] = requestHandlers.show;

/* server start */
server.start(router.route, handle);