/*  */
var exec = require("child_process").exec,
	querystring = require("querystring"),
	fs = require("fs");
    formidable = require("formidable");

// function startGet(response) {
//   console.log("Request handler 'start' was called.");
//   // exec("ls -lah", function(error, stdout, stderr){	
//   //   response.writeHead(200, {"Content-Type": "text/plain"});
//   //   response.write(stdout);
//   //   response.end();
//   // });
	
// 	exec("find /",
//     { timeout: 10000, maxBuffer: 20000*1024 },
//     function (error, stdout, stderr) {
//       response.writeHead(200, {"Content-Type": "text/plain"});
//       response.write(stdout || 'empty');
//       response.end();
//     });
// }

function startPost (response) {
	console.log("Request handler 'start' was called.");

	var body = '<html>'+
    '<head>'+
    '<meta http-equiv="Content-Type" '+
    'content="text/html; charset=UTF-8" />'+
    '</head>'+
    '<body>'+
    '<form action="/upload" enctype="multipart/form-data" '+
    'method="post">'+
    '<input type="file" name="upload">'+
    '<input type="submit" value="Upload file" />'+
    '</form>'+
    '</body>'+
    '</html>';

	response.writeHead(200, {"Content-Type": "text/html"});
	response.write(body);
	response.end();
}

function upload(response, request) {
	console.log("Request handler 'upload' was called.");
	var form = new formidable.IncomingForm();
	form.uploadDir='D:/Program Files/nodejs';

	console.log("about to parse");
	form.parse(request, function (error, fields, files) {
		console.log("parsing done");
	    /*
	     * __dirname
	     * Some systems [Windows] raise an error when you attempt to rename new file into one that already exists.
	     * This call deletes the previous .PNG image prior to renaming the new one in its place.
	     */	    
	    // fs.unlinkSync("D:/Program Files/nodejs/Nodejs/123.png");
		fs.renameSync(files.upload.path, "D:/Program Files/nodejs/Nodejs/123.png");
		response.writeHead(200, {"content-type": "text/html"});
		response.write("received image:<br/>");
		response.write("<img src='/show' />");
		response.end();
	});
}

function show (response) {
	console.log("Request handler 'show' was called.");
	fs.readFile("D:/Program Files/nodejs/Nodejs/123.png", "binary", function (error, file) {
		if (error) {
			response.writeHead(200, {"Content-Type": "text/plain"});
			response.write(error + "\n");
			response.end();
		} else {
			response.writeHead(200, {"Content-Type": "image/png"});
      		response.write(file, "binary");
			response.end();
		}
	});
}

exports.startPost = startPost;
exports.upload = upload;
exports.show  = show;