// 回调函数 f1();　　f2();
// 回调函数的优点是简单、容易理解和部署，缺点是不利于代码的阅读和维护，
// 各个部分之间高度耦合（Coupling），流程会很混乱，而且每个任务只能指定一个回调函数。
function f1(callback){
	setTimeout(function () {
		// f1的任务代码
		callback();
	}, 1000);
}

f1(f2);



function finder(records, cb) {
    setTimeout(function () {
        records.push(3, 4);
        cb(records);
    }, 500);
}
function processor(records, cb) {
    setTimeout(function () {
        records.push(5, 6);
        cb(records);
    }, 500);
}

// using the callbacks
finder([1, 2], function (records) {
    processor(records, function(records) {
             console.log(records);       
    });
});

// or

function onProcessorDone(records){
    alert(records);   
}

function onFinderDone(records) {
    processor(records, onProcessorDone);
}

finder([1, 2], onFinderDone);



// 事件监听 jquery
// 缺点是整个程序都要变成事件驱动型，运行流程会变得很不清晰。
f1.on('done', f2);
function f1(){
	setTimeout(function () {
		// f1的任务代码
		f1.trigger('done');
　　}, 1000);
}

// using listeners
var eventable = {
    on: function(event, cb) {
        $(this).on(event, cb);
    },
    trigger: function (event, args) {
        $(this).trigger(event, args);
    }
}
    
var finder = {
    run: function (records) {
            var self = this;
        setTimeout(function () {
            records.push(3, 4);
           self.trigger('done', [records]);            
        }, 500);
    }
}
var processor = {
    run: function (records) {
         var self = this;
        setTimeout(function () {
            records.push(5, 6);
            self.trigger('done', [records]);            
        }, 500);
    }
 }
 $.extend(finder, eventable);
 $.extend(processor, eventable);
    
finder.on('done', function (event, records) {
          processor.run(records);  
    });
processor.on('done', function (event, records) {
    alert(records);
});
finder.run([1,2]);


//发布/订阅 Tiny Pub/Sub，这是jQuery的一个插件。
jQuery.subscribe("done", f2);
function f1(){
	setTimeout(function () {
		// f1的任务代码
		jQuery.publish("done");
	}, 1000);
}
jQuery.unsubscribe("done", f2);

// setup
function finder(records, cb) {
    setTimeout(function () {
        records.push(3, 4);
        cb(null, records);
    }, 500);
}
function processor(records, cb) {
    setTimeout(function () {
        records.push(5, 6);
        cb(null, records);
    }, 500);
}
async.waterfall([
    function(cb){
        finder([1, 2], cb);
    },
    processor
    ,
    function(records, cb) {
        alert(records);
    }
]);

// setup

function finder(records, cb) {
    setTimeout(function () {
        records.push(3, 4);
        cb(records);
    }, 500);
}
function processor(records, cb) {
    setTimeout(function () {
        records.push(5, 6);
        cb(records);
    }, 500);
}

async.waterfall([
    function(cb){
        finder([1, 2], function(records) {
            cb(null, records)
        });
    },
    function(records, cb){
        processor(records, function(records) {
            cb(null, records);
        });
    },
    function(records, cb) {
        alert(records);
    }
]);

//Promises对象
//Promises对象是CommonJS工作组提出的一种规范，目的是为异步编程提供统一接口。
//jquery
f1().then(f2);
function f1(){
	var dfd = $.Deferred();
	setTimeout(function () {
		// f1的任务代码
		dfd.resolve();
	}, 500);
	return dfd.promise;
}


// using promises
function finder(records){
    var deferred = when.defer();
    setTimeout(function () {
        records.push(3, 4);
        deferred.resolve(records);
    }, 500);
    return deferred.promise;
}
function processor(records) {
     var deferred = when.defer();
    setTimeout(function () {
        records.push(5, 6);
        deferred.resolve(records);
    }, 500);
    return deferred.promise;
}

finder([1,2])
    .then(processor)
    .then(function(records) {
            alert(records);
    });
  


function log(msg) {
    document.write(msg + '<br />');
}

// using promises
function finder(records){
    var deferred = when.defer();
    setTimeout(function () {
        records.push(3, 4);
        log('records found - resolving promise');
        deferred.resolve(records);
    }, 100);
    return deferred.promise;
}

var promise = finder([1,2]);

// wait 
setTimeout(function () {
    // when this is called the finder promise has already been resolved
    promise.then(function (records) {
        log('records received');        
    });
}, 1500);