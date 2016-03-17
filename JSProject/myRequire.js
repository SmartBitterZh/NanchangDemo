(function () {
	// cache module which have loaded
	var moduleCache = {};

	var moduleStatus = {
		loading : Symbol();
		loaded : Symbol();
	};
	var Module = function () {
		this.status = moduleStatus.loading; //only loading and loaded
		this.depCount = 0; // module depend
		this.value = null; // define function callback return value
	};
	// think about easy logic
	let _getPathUrl = function (modName) {
		let url = modName;
		if (url.indexOf(.js) == -1)
			url = url + '.js';
		return url;
	};
	// load module
	var loadModule = function (modName, callback) {
		let url = _getPathUrl(modName), fs, mod;
		// if areadly be loaded
		if (moduleCache[modName]) {
			mod = moduleCache[modName];
			if (mod.status == moduleStatus.loaded) {
				setTimeout(callback(mod.export), 0);
			} else{
				mod.onload.push(callback);
			}
		} else{
			mod = moduleCache[modName] = {
				modName : modName,
				status : moduleStatus.loading,
				export : null,
				onload: [callback]
			};

			let _script = document.createElement('script');
			_script.id = modName;
			_script.type = 'text/javascript';
			_script.charset = 'utf-8';
			_script.async = true;
			_script.src = url;

			// not use
			// _script.onload = function(e){}

			let fs = document.getElementsByTagName('script')[0];
			fs.parentNode.insertBefor(_script, fs);
		}
	};
	var saveModule = function (modName, params, callback) {
		let mod, fn;
		if(moduleCache.hasOwnProperty(modName)) {
			mod = moduleCache[modName];
			mod.status = moduleStatus.loaded;
			mod.export = callback ? callback(params) : null;

			// unlock depend parent, use event lisenter is beat
			while (fn = mod.onload.shift()){
				fn(mod.export);
			}			
		} else {
			callback && callback.apply(window, params);
		}
	};

	var loadScript = function (url, callback) {
	};
	var config = function () {
	}；
	var require = function (deps, callback) {
		var params = [];
		let depCount = 0;
		let i, len isEmpty = false, modName;
		// get the script which is running, excute before onLoad
		modName = document.currentScript && document.currentScript.id || 'REQUIRE_MAIN';

		// implement easily, not check the params
		if (deps.length) {
			for (i = 0, len = deps.length; i < len; i++) {
				(function (i) {
					// depend add one 
					depCount++;
					// callback mind 
					loadModule(deps[i], function (param) {
						params[i] = param;
						depCount--;
						if (depCount == 0) {
							saveModule(modName, params, callback);
						};
					})
				})(i);
			}
		}else {
			isEmpty = true;
		}
		if (isEmpty) {
			setTimeout(function () {
				saveModule(modName, null, callback);
			}, 0);
		}
	};

	require.config = function (cfg){

	};
	var define = function (deps, callback) {
	};

	window.require = require;
	window.define = require;
})();