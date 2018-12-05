"use strict";
var util = require("gulp-util");
var helix = {};

helix.header = function header(line1, line2) {

	util.log(" __  ______           _   _                 ");
	util.log(" \ \/ / ___|___ _ __ | |_(_)_   _ _ __ ___  ");
	util.log("  \  / |   / _ \ '_ \| __| | | | | '_ ` _ \ ");
	util.log("  /  \ |__|  __/ | | | |_| | |_| | | | | | |");
	util.log(" /_/\_\____\___|_| |_|\__|_|\__,_|_| |_| |_|");
	util.log("                                            ");
	util.log(" 		                                      ");
	util.log(" ------------ www.xcentium.com ------------ ");

};

module.exports = helix;