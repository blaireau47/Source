(function ($) {
	var _menuID = 0;
	var _handlers = {};
	$.fn.menu = function (options) {
		if (options == "close") {
			var classes = $(this).attr("class").split(" ");
			var name = null;
			for (var i = 0; i < classes.length; i++) {
				if (classes[i].match("c-")) {
					name = classes[i].replace("-", "");
					break;
				}
			}
			if (!name) {
				$(document).triggerHandler("click." + name);
				$(document).triggerHandler("touch." + name);

			} else {
				$(document).triggerHandler("click");
				$(document).triggerHandler("touch");
			}
			return false;
		}
		_menuID++;
		var myId = _menuID++;
		// L'option target est obligatoire
		// Valeurs par défauts
		var settings = {
			'width': null,
			'height': null,
			'anim': 'fade',
			'target': null,
			'class': null,
			'events': 'click',
			'offset-x': {
				'alignTo': 'left',
				'offset': 0,
				'start': 'parent'
			},
			'offset-y': {
				'alignTo': 'bottom',
				'offset': 0,
				'start': 'parent'
			},
			'beforeOpen': null,
			'afterOpen': null,
			'close': null
		};
		// ----------------------------------------------

		var open = function (name, css) {
			_handlers[name] = function (e) {
				if (!e) var e = window.event;
				var element = (e.target) ? e.target : e.srcElement;
				if (!$(element).is('.t-' + myId) && $(element).parents('.t-' + myId).length == 0) {
					close(name);
				}
			};
			// console.log("open:" + name);
			// TODO : supporter + d'animations
			settings.target.css(css);
			switch (settings.anim) {
				case "slide":
					settings.target.slideDown();
					break;
				default:
					settings.target.fadeIn();
					break;
			}
			$(document).bind("click." + name, _handlers[name]);
			$(document).bind("touch." + name, _handlers[name]);
		};
		var close = function (name) {
			if (typeof (settings.onClose) == "function") { settings.onClose(); }
			// TODO : supporter + d'animations
			// console.log("close:" + name);
			switch (settings.anim) {
				case "slide":
					settings.target.slideUp();
					break;
				default:
					settings.target.fadeOut();
					break;
			}
			$(document).unbind("click." + name, _handlers[name]);
			$(document).unbind("touch." + name, _handlers[name]);
			delete _handlers[name];
		};

		return this.each(function () {
			// If options exist, lets merge them
			// with our default settings
			if (options) {
				$.extend(settings, options);
			}
			settings.target.hide();
			// Click event
			// TODO : supporter d'autre events*
			if (settings.events.indexOf('click') > -1 || settings.events.indexOf('touch') > -1) {
				$(this).dblclick(function (e) { e.preventDefault(); e.stopPropagation(); return false; });

				$(this).bind("click touch", function (e) {
					var element = (e.target) ? e.target : e.srcElement;
					if ($(element).parents(".t-" + myId).length == 0) {
						e.preventDefault();
						e.stopPropagation();
						for (var id in _handlers) {
							//_handlers[id];
							$(document).triggerHandler("click." + id);
							$(document).triggerHandler("touch." + id);

						}
						var $caller = $(this).addClass("c-" + myId);
						settings.target.addClass("t-" + myId);
						var callerPos = $(this).position();
						callerPos.right = callerPos.left + $(this).outerWidth(false);
						callerPos.bottom = callerPos.top + $(this).outerHeight(false);

						// Ajuste la position x
						var css = {
							position: 'absolute',
							'z-index': 10
						};
						if (!settings['position']) {
							// --------------------------- Offset "self" -------------------------- //
							if (settings['offset-x'].offset.toString().match("self")) {
								if (settings['offset-x'].offset.toString().match("-")) {
									settings['offset-x'].offset = -settings.target.outerWidth(false);
								} else {
									settings['offset-x'].offset = settings.target.outerWidth(false);
								}
							}
							if (settings['offset-y'].offset.toString().match("self")) {
								if (settings['offset-y'].offset.toString().match("-")) {
									settings['offset-y'].offset = -settings.target.outerHeight(false);
								} else {
									settings['offset-y'].offset = settings.target.outerHeight(false);
								}
							}
							// -------------------------------------------------------------------- //
							// Alignement
							if (settings['offset-x'].start != "parent") {
								callerPos.left = settings['offset-x'].start;
							}
							if (settings['offset-y'].start != "parent") {
								callerPos.top = settings['offset-y'].start;
							}
							// Pos x
							if (settings['offset-x'].alignTo == 'left') {
								css.left = (callerPos.left + settings['offset-x'].offset) + "px";
							} else {
								css.left = (callerPos.right + settings['offset-x'].offset) + "px";
							}
							// Pos y
							if (settings['offset-y'].alignTo == 'top') {
								css.top = (callerPos.top + settings['offset-y'].offset) + "px";
							} else {
								css.top = (callerPos.bottom + settings['offset-y'].offset) + "px";

							}
						} else {
							if (settings['position'].top) {
								css.top = settings['position'].top;
							}
							if (settings['position'].bottom) {
								css.bottom = settings['position'].bottom;
							}
							if (settings['position'].left) {
								css.left = settings['position'].left;
							}
							if (settings['position'].right) {
								css.right = settings['position'].right;
							}
						}
						// ---------------------------------
						// Hauteur & Largeur
						if (settings.height) {
							if (settings.height == "parent") {
								css.height = $caller.outerHeight(false) + "px";
							} else {
								css.height = settings.height;
							}
						}
						if (settings.width) {
							if (settings.width == "parent") {
								css.width = $caller.outerWidth(false) + "px";
							} else {
								css.width = settings.width;
							}
						}
						if (settings.target.outerWidth(false) < $caller.outerWidth(false)) {
							css.width = $caller.outerWidth(false) + "px";
						}
						// SI la target n'est pas visible
						if (settings.target.is(":hidden")) {
							if (typeof (settings.beforeOpen) == "function") { settings.beforeOpen(); }
							open("caller" + myId, css);
							if (typeof (settings.afterOpen) == "function") { settings.afterOpen(); }
							return false;
						}
					}
				});
			}
		});

	};
})(jQuery);
