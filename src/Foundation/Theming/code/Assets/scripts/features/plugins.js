$.fn.extend(
    {
        onCSSAnimationEnd: function (callback) {
            var $this = $(this).eq(0);
            var prefix = 'webkitAnimationEnd mozAnimationEnd oAnimationEnd oanimationend animationend'
            $this.on(prefix, function () {
                callback.call(this);
                $(this).off(prefix);
            });
            return this;
        },
        onCSSTransitionEnd: function (callback) {
            var $this = $(this).eq(0);
            var prefix = 'webkitTransitionEnd mozTransitionEnd oTransitionEnd otransitionend transitionend'
            $this.on(prefix, function () {
                callback.call(this);
                $(this).off(prefix);
            });
            return this;
        }
    });