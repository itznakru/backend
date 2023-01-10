"use strict";

/**
 * Represents the relevant data of a listener
 * 
 * @param {integer} id
 * @param {Array.<integer>} 
 * @param {Function} fn 
 */
function Listener(id, dependencies, fn) {
	this.id = id;
	this.dependencies = dependencies;
	this.fn = fn;
}

export default Listener;
//module.exports = Listener;